SET QUOTED_IDENTIFIER ON;
GO
SET ANSI_NULLS ON;
GO

IF EXISTS (SELECT
                *
        FROM dbo.sysobjects
        WHERE id = OBJECT_ID(N'[dbo].[isp_sqlsecure_getpolicyassessment]'))
        DROP PROCEDURE [dbo].[isp_sqlsecure_getpolicyassessment];
GO

CREATE PROCEDURE [dbo].[isp_sqlsecure_getpolicyassessment] (@policyid int,
@assessmentid int = NULL,    	-- default to policy settings for backward compatibility
@registeredserverid int = 0,
@alertsonly bit = 0,
@usebaseline bit = 0,
@rundate datetime = NULL,
@fullRefresh int = 1
) WITH ENCRYPTION
AS -- <Idera SQLsecure version and copyright>
        --
        -- Description :
        --             Perform a security assessment for the policy or assessment using the configured metrics
        --				for the servers in the policy and return a table of results flagging all metrics that exceed the configured thresholds
        --
        --				If @registeredserverid is 0, then all servers in the policy will be evaluated
        --					otherwise, only the selected server will have the policy applied to it
        --				If @alertsonly is 1, only return the risks
        --				If the assessmentid is for a saved assessment, then usebaseline and rundate parameters are ignored
        --					and the values are pulled from the assessment table

        DECLARE @outtbl TABLE (
                snapshotid int,
                registeredserverid int,
                connectionname nvarchar(400),
                collectiontime datetime,
                metricid int,
                metricname nvarchar(256),
                metrictype nvarchar(32),
                metricseveritycode int,
                metricseverity nvarchar(16),
                metricseverityvalues nvarchar(4000),
                metricdescription nvarchar(4000),
                metricreportkey nvarchar(32),
                metricreporttext nvarchar(4000),
                severitycode int,
                severity nvarchar(16),
                currentvalue nvarchar(1500),
                thresholdvalue nvarchar(1500)
        );
        CREATE TABLE #tempdetails (
                policyid int,
                assessmentid int,
                metricid int,
                snapshotid int,
                detailfinding varchar(2048),
                databaseid int NULL,
                objecttype varchar(5),
                objectid int NULL,
                objectname varchar(400)
        );
        DECLARE @err int,
                @sevcodeok int,
                @valid bit,
                @isadmin bit,
                @debug bit,
                @runtime datetime,
                @serverruntime datetime;
        SELECT
                @sevcodeok = 0,
                @valid = 0,
                @isadmin = 0,
                @debug = 0;

        IF (@debug = 1)
        BEGIN
                SET NOCOUNT OFF;
                PRINT '@policyid=' + CONVERT(nvarchar, @policyid)
                + ', @assessmentid=' + CONVERT(nvarchar, ISNULL(@assessmentid,
                -1))
                + ', @registeredserverid='
                + CONVERT(nvarchar, ISNULL(@registeredserverid, -1));
                PRINT '@alertsonly=' + CONVERT(nvarchar, ISNULL(@alertsonly, -1))
                + ', @usebaseline=' + CONVERT(nvarchar, ISNULL(@usebaseline,
                -1))
                + ', @rundate=' + CONVERT(nvarchar, ISNULL(@rundate, -1));
        END;
        ELSE
        BEGIN
                SET NOCOUNT ON;
        END;

        EXEC @isadmin = [isp_sqlsecure_isadmin];

        -- replace the settings assessmentid with the current assessmentid
        IF (@assessmentid IS NULL
                OR EXISTS (SELECT
                        *
                FROM assessment
                WHERE assessmentid = @assessmentid
                AND assessmentstate = N'S')
                )
        BEGIN
                SELECT
                        @assessmentid = NULL;		-- make sure it is null so the current assessment will be created if it doesn't exist
                SELECT
                        @assessmentid = assessmentid
                FROM assessment
                WHERE policyid = @policyid
                AND assessmentstate = N'C';
                IF (@debug = 1)
                BEGIN
                        SET NOCOUNT OFF;
                        PRINT 'Policy passed, @assessmentid='
                        + CONVERT(nvarchar, ISNULL(@assessmentid, -1));
                END;
        END;

        -- if it still wasn't found, then create the current assessment
        IF (@assessmentid IS NULL
                AND @isadmin = 1
                )
        BEGIN
                IF (@debug = 1)
                BEGIN
                        SET NOCOUNT OFF;
                        PRINT 'creating new current assessment';
                END;
                EXEC [dbo].[isp_sqlsecure_createassessmentfrompolicy] @policyid = @policyid,
                                                                      @assessmentid = NULL,
                                                                      @type = N'C',
                                                                      @copy = 2,
                                                                      @newassessmentid = @assessmentid OUTPUT;
                IF (@debug = 1)
                BEGIN
                        SET NOCOUNT OFF;
                        PRINT 'New @assessmentid='
                        + CONVERT(nvarchar, @assessmentid);
                END;
        END;

        --get the id of the settings to update with selection criteria
        DECLARE @state nchar(1),
                @settingsid int;
        SELECT
                @state = assessmentstate
        FROM assessment
        WHERE policyid = @policyid
        AND assessmentid = @assessmentid;

        IF (@state IS NULL)
        BEGIN
                SELECT
                        @state = N'S',
                        @assessmentid = assessmentid
                FROM assessment
                WHERE policyid = @policyid
                AND assessmentstate = N'S';
        END;
        IF (@state = N'S')
                SELECT
                        @settingsid = @assessmentid;
        ELSE
        IF (@state = N'C')
                SELECT
                        @settingsid = assessmentid
                FROM assessment
                WHERE policyid = @policyid
                AND assessmentstate = N'S';

        IF (@debug = 1)
        BEGIN
                PRINT '@state=' + @state;
                PRINT '@assessmentid=' + CONVERT(nvarchar, ISNULL(@assessmentid,
                -1));
                PRINT '@settingsid=' + CONVERT(nvarchar, ISNULL(@settingsid, -1));
        END;
        IF (@isadmin = 1)
        BEGIN
                IF (@settingsid IS NOT NULL)
                        UPDATE [assessment]
                        SET assessmentdate = @rundate,
                            usebaseline = @usebaseline
                        WHERE policyid = @policyid
                        AND assessmentid = @settingsid;

                -- check to make sure the assessment data is current.
                EXEC [dbo].[isp_sqlsecure_isassessmentdatacurrent] @policyid = @policyid,
                                                                   @assessmentid = @assessmentid,
                                                                   @valid = @valid OUTPUT;
        END;

        IF (@debug = 1)
        BEGIN
                SET NOCOUNT OFF;
                PRINT '@valid=' + CONVERT(nvarchar, @valid);
        END;

        -- get the list of servers for the selected policy
        CREATE TABLE #servertbl (
                registeredserverid int
        );
        INSERT #servertbl
        EXEC @err = [dbo].[isp_sqlsecure_getpolicymemberlist] @policyid = @policyid,
                                                              @assessmentid = @assessmentid;

        DECLARE @returnservertbl TABLE (
                registeredserverid int
        );
        INSERT INTO @returnservertbl
                SELECT
                        registeredserverid
                FROM #servertbl;
        IF (@registeredserverid > 0)
        BEGIN
                DELETE FROM @returnservertbl
                WHERE registeredserverid != @registeredserverid;
        END;

		
    -- accept the current assessment unless fullRefresh is true
	if (@fullRefresh != 1)
	begin
		SET @valid = 1
	end

        IF (@valid = 1)
        BEGIN
                INSERT INTO @outtbl (snapshotid,
                registeredserverid,
                connectionname,
                collectiontime,
                metricid,
                metricname,
                metrictype,
                metricseveritycode,
                metricseverity,
                metricseverityvalues,
                metricdescription,
                metricreportkey,
                metricreporttext,
                severitycode,
                severity,
                currentvalue,
                thresholdvalue)
                        (SELECT
                                snapshotid,
                                registeredserverid,
                                connectionname,
                                collectiontime,
                                metricid,
                                metricname,
                                metrictype,
                                metricseveritycode,
                                metricseverity,
                                metricseverityvalues,
                                metricdescription,
                                metricreportkey,
                                metricreporttext,
                                severitycode,
                                severity,
                                currentvalue,
                                thresholdvalue
                        FROM policyassessment
                        WHERE policyid = @policyid
                        AND assessmentid = @assessmentid
                        AND registeredserverid IN (SELECT
                                registeredserverid
                        FROM @returnservertbl)
                        );
        END;
        ELSE
        BEGIN
                BEGIN TRANSACTION;

                        IF (@isadmin = 1)
                        BEGIN
                                DELETE policyassessmentdetail
                                WHERE policyid = @policyid
                                        AND assessmentid = @assessmentid;
                                DELETE policyassessment
                                WHERE policyid = @policyid
                                        AND assessmentid = @assessmentid;
                        END;

                        -- if it is a current assessment, then refresh the metric settings
                        IF (@state = N'C')
                        BEGIN
                                IF (@isadmin = 1)
                                BEGIN
                                        IF (@debug = 1)
                                        BEGIN
                                                PRINT 'Refreshing assessment from policy settings @assessmentid='
                                                + CONVERT(nvarchar, @assessmentid);
                                        END;
                                        EXEC [dbo].[isp_sqlsecure_createassessmentfrompolicy] @policyid = @policyid,
                                                                                              @assessmentid = @assessmentid,
                                                                                              @type = N'C',
                                                                                              @copy = 3,
                                                                                              @newassessmentid = @assessmentid OUTPUT;
                                END;
                        END;
                        ELSE
                        IF (@state IN (N'D', N'P'))
                        BEGIN
                                SELECT
                                        @rundate = assessmentdate,
                                        @usebaseline = usebaseline
                                FROM assessment
                                WHERE policyid = @policyid
                                AND assessmentid = @assessmentid;
                        END;

                        IF EXISTS (SELECT
                                        *
                                FROM #servertbl)
                        BEGIN
                                -- create constants for use on metrics
                                DECLARE @everyonesid varbinary(85),
                                        @sysadminsid varbinary(85),
                                        @builtinadminsid varbinary(85);
                                SELECT
                                        @everyonesid = 0x01010000000000010000000000000000000000000000000000000000000000000000000000000000,
                                        @sysadminsid = 0x03,
                                        @builtinadminsid = 0x01020000000000052000000020020000;


                                -- get the list of metrics for the policy
                                DECLARE @metricid int,
                                        @metricname nvarchar(256),
                                        @metrictype nvarchar(32),
                                        @metricdescription nvarchar(1024),
                                        @metricreportkey nvarchar(32),
                                        @metricreporttext nvarchar(4000),
                                        @severity int,
                                        @severityvalues nvarchar(4000),
                                        @configuredvalues nvarchar(4000);
								--START (Barkha Khatri) Declaring constants for comparisons
								--moving metric cursor inside snapcursor 
								--as now we are having different types of server in one policy
								DECLARE @onpremiseservertype nvarchar(3),
										@azuresqldatabaseservertype nvarchar(3),
										@sqlserveronazurevmservertype nvarchar(3);

								SELECT @onpremiseservertype = 'OP',
									   @azuresqldatabaseservertype = 'ADB',
									   @sqlserveronazurevmservertype = 'AVM';
								--START (Barkha Khatri) Declaring constants for comparisons
                                -- process the snapshots for each metric
                                DECLARE @snapshotid int,
                                        @connection nvarchar(400),
                                        @snapshottime datetime,
                                        @status nchar(1),
                                        @baseline nchar(1),
                                        @collectorversion nvarchar(32),
                                        @version nvarchar(256),
                                        @os nvarchar(512),
                                        @authentication nchar(1),
                                        @loginauditmode nvarchar(20),
                                        @c2audittrace nchar(1),
                                        @crossdb nchar(1),
                                        @proxy nchar(1),
                                        @remotedac nchar(1),
                                        @remoteaccess nchar(1),
                                        @startupprocs nchar(1),
                                        @sqlmail nchar(1),
                                        @databasemail nchar(1),
                                        @ole nchar(1),
                                        @webassistant nchar(1),
                                        @xp_cmdshell nchar(1),
                                        @agentmailprofile nvarchar(128),
                                        @hide nchar(1),
                                        @agentsysadmin nchar(1),
                                        @dc nchar(1),
                                        @replication nchar(1),
                                        @sapassword nchar(1),
                                        @systemtables nchar(1),
                                        @systemdrive nchar(2),
                                        @adhocqueries nchar(1),
                                        @weakpasswordenabled nchar(1);

                                DECLARE snapcursor CURSOR STATIC FOR
                                SELECT
                                        a.snapshotid,
                                        a.registeredserverid,
                                        a.connectionname,
                                        a.endtime,
                                        a.status,
                                        a.baseline,
                                        a.collectorversion,
                                        a.version,
                                        a.os,
                                        a.authenticationmode,
                                        a.loginauditmode,
                                        a.enablec2audittrace,
                                        a.crossdbownershipchaining,
                                        a.enableproxyaccount,
                                        a.remoteadminconnectionsenabled,
                                        a.remoteaccessenabled,
                                        a.scanforstartupprocsenabled,
                                        a.sqlmailxpsenabled,
                                        a.databasemailxpsenabled,
                                        a.oleautomationproceduresenabled,
                                        a.webassistantproceduresenabled,
                                        a.xp_cmdshellenabled,
                                        a.agentmailprofile,
                                        a.hideinstance,
                                        a.agentsysadminonly,
                                        a.serverisdomaincontroller,
                                        a.replicationenabled,
                                        a.sapasswordempty,
                                        a.allowsystemtableupdates,
                                        a.systemdrive,
                                        a.adhocdistributedqueriesenabled,
                                        a.isweakpassworddetectionenabled
                                FROM serversnapshot a,
                                     dbo.getsnapshotlist(@rundate, @usebaseline) b
                                WHERE a.registeredserverid IN (SELECT
                                        registeredserverid
                                FROM #servertbl)
                                AND a.snapshotid = b.snapshotid;
                                OPEN snapcursor;

                                DECLARE @sevcode int,
                                        @sev nvarchar(16),
                                        @metricval nvarchar(1500),
                                        @metricthreshold nvarchar(1500);
                                DECLARE @loginname nvarchar(200),
                                        @intval int,
                                        @intval2 int;
                                DECLARE @strval nvarchar(1024),
                                        @strval2 nvarchar(1024),
                                        @strval3 nvarchar(1024),
                                        @sql nvarchar(4000);
                                DECLARE @tblval TABLE (
                                        val nvarchar(1024) COLLATE DATABASE_DEFAULT
                                );
                                -- store sysadmin users in table that can be used with dynamic sql for multiple checks
                                CREATE TABLE #sysadminstbl (
                                        id int,
                                        name nvarchar(256) COLLATE DATABASE_DEFAULT
                                );
								DECLARE @serverType nvarchar(5)
                                FETCH NEXT FROM snapcursor INTO @snapshotid,
                                @registeredserverid, @connection, @snapshottime,
                                @status, @baseline, @collectorversion, @version, @os,
                                @authentication, @loginauditmode, @c2audittrace,
                                @crossdb, @proxy, @remotedac, @remoteaccess,
                                @startupprocs, @sqlmail, @databasemail, @ole,
                                @webassistant, @xp_cmdshell, @agentmailprofile, @hide,
                                @agentsysadmin, @dc, @replication, @sapassword,
                                @systemtables, @systemdrive, @adhocqueries,
                                @weakpasswordenabled;

                                WHILE @@fetch_status = 0
                                BEGIN
										
										--START(Barkha Khatri) Changing metric cursor to get the metrics applicable on a particular server type
										select @serverType=servertype from dbo.registeredserver where registeredserverid=@registeredserverid
										DECLARE metriccursor CURSOR STATIC FOR
										SELECT
												metricid,
												metricname,
												metrictype,
												metricdescription,
												reportkey,
												reporttext,
												severity,
												severityvalues
										FROM vwpolicymetric
										WHERE policyid = @policyid
										AND assessmentid = @assessmentid
										AND isenabled = 1
										AND ((@serverType=@onpremiseservertype and applicableonpremise=1)OR (@serverType=@azuresqldatabaseservertype and applicableonazuredb=1)OR(@serverType=@sqlserveronazurevmservertype and applicableonazurevm=1)) ;
										--END(Barkha Khatri) Changing metric cursor to get the metrics applicable on a particular server type
										OPEN metriccursor;
                                        IF (@debug = 1)
                                        BEGIN
                                                SELECT
                                                        @serverruntime = GETDATE();
                                                PRINT CONVERT(nvarchar, @serverruntime, 8)
                                                + ': @connection=' + @connection;
                                                PRINT '@snapshotid='
                                                + CONVERT(nvarchar, @snapshotid);
												
                                        END;

                                        -- save a list of sysadmin members in this snapshot for use by multiple metrics
                                        DELETE FROM #sysadminstbl;
										IF(@serverType=@onpremiseservertype or @serverType=@sqlserveronazurevmservertype)
										BEGIN
                                        INSERT INTO #sysadminstbl
                                                SELECT DISTINCT
                                                        a.memberprincipalid,
                                                        c.name
                                                FROM serverrolemember a,
                                                     serverprincipal b,
                                                     serverprincipal c
                                                WHERE a.snapshotid = @snapshotid
                                                AND a.snapshotid = b.snapshotid
                                                AND a.principalid = b.principalid
                                                AND b.sid = @sysadminsid
                                                AND a.snapshotid = c.snapshotid
                                                AND a.memberprincipalid = c.principalid;
										END
										--START(Barkha Khatri) For azure SQL DB -considering users having loginmanager and dbmanager role as admins
										ELSE IF(@serverType=@azuresqldatabaseservertype)
										BEGIN
										INSERT INTO #sysadminstbl
											SELECT DISTINCT principalid,name
											FROM serverprincipal
											WHERE snapshotid=@snapshotid AND 
											principalid IN 
													(SELECT memberprincipalid
													 FROM serverrolemember	
													 WHERE snapshotid=@snapshotid
													 AND principalid IN 
																	(SELECT principalid
																	FROM serverprincipal WHERE snapshotid=@snapshotid
																	AND( name='loginmanager' or name='dbmanager'))
													GROUP BY memberprincipalid
													HAVING COUNT(memberprincipalid)>=2)	
										END
										--END(Barkha Khatri) For azure SQL DB -considering users having loginmanager and dbmanager role as admins
                                        FETCH FIRST FROM metriccursor INTO @metricid,
                                        @metricname, @metrictype, @metricdescription,
                                        @metricreportkey, @metricreporttext, @severity,
                                        @severityvalues;

                                        WHILE @@fetch_status = 0
                                        BEGIN
                                        BEGIN TRY
                                                IF (@debug = 1)
                                                BEGIN
                                                        SELECT
                                                                @runtime = GETDATE();
                                                        PRINT CONVERT(nvarchar, @runtime, 8)
                                                        + ': @metricid='
                                                        + CONVERT(nvarchar, @metricid);
                                                END;
                                                -- This sets the metric so it will not be displayed if no value is found
                                                --     each metric should handle this situation appropriately
                                                SELECT
                                                        @err = 0,
                                                        @sevcode = -1,
                                                        @metricval = N'',
                                                        @metricthreshold = N'',
                                                        @configuredvalues = @severityvalues;
                                                -- clean up old values
                                                SELECT
                                                        @intval = 0,
                                                        @intval2 = 0,
                                                        @strval = N'',
                                                        @strval2 = N'',
                                                        @strval3 = N'',
                                                        @sql = N'';
                                                DELETE FROM @tblval;

                                                -- Collection Time
                                                IF (@metricid = 1)
                                                BEGIN
                                                        -- remove the quotes surrounding the value and test for numeric
                                                        SET @severityvalues = REPLACE(@severityvalues,
                                                        '''', '');
                                                        IF (ISNUMERIC(@severityvalues) = 1)
                                                                SELECT
                                                                        @intval = CAST(@severityvalues AS int);
                                                        SET @err = @@ERROR;
                                                        IF (@err = 0)
                                                        BEGIN
                                                                SELECT
                                                                        @intval2 = DATEDIFF(D,
                                                                        @snapshottime,
                                                                        ISNULL(@rundate,
                                                                        GETDATE()));
                                                                IF (@intval2 <= @intval)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = N'Audit data is within the selected date range.';
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @severity,
                                                                                @metricval = CONVERT(nvarchar, @snapshottime)
                                                                                + N': Audit data is '
                                                                                + CONVERT(nvarchar, @intval2)
                                                                                + N' days from the selected date.';
                                                        END;

                                                        SELECT
                                                                @metricthreshold = N'Audit data is acceptable if within '
                                                                + @severityvalues
                                                                + N' days of the selected date.';
                                                END;
                                                -- SQL Server version
                                                ELSE
                                                IF (@metricid = 2)
                                                BEGIN
                                                        --make sure the version doesn't start with a 0 before comparing
                                                        SELECT
                                                                @strval =
                                                                                 CASE
                                                                                         WHEN LEFT(@version,
                                                                                                 1) = '0' THEN SUBSTRING(@version,
                                                                                                 2,
                                                                                                 LEN(@version)
                                                                                                 - 1)
                                                                                         ELSE @version
                                                                                 END;

                                                        --find the matching entry based on major and minor version
                                                        SELECT
                                                                @intval = CHARINDEX(''''
                                                                + LEFT(@strval,
                                                                CHARINDEX('.',
                                                                @strval,
                                                                CHARINDEX('.',
                                                                @strval) + 1)),
                                                                @severityvalues);

                                                        SELECT
                                                                @intval2 = 1;	-- force a finding if it can't be parsed

                                                        IF (@intval > 0)
                                                        BEGIN
                                                                -- set the threshold to the matching version
                                                                SELECT
                                                                        @metricthreshold = SUBSTRING(@severityvalues,
                                                                        @intval + 1,
                                                                        CHARINDEX('''',
                                                                        @severityvalues,
                                                                        @intval + 1)
                                                                        - @intval - 1);
                                                                DECLARE @v1 int,
                                                                        @v2 int,
                                                                        @v3 int,
                                                                        @v4 int,
                                                                        @t1 int,
                                                                        @t2 int,
                                                                        @t3 int,
                                                                        @t4 int,
                                                                        @ver nvarchar(20);
                                                                SELECT
                                                                        @v1 = 0,
                                                                        @v2 = 0,
                                                                        @v3 = 0,
                                                                        @v4 = 0,
                                                                        @t1 = 0,
                                                                        @t2 = 0,
                                                                        @t3 = 0,
                                                                        @t4 = 0;

                                                                --parse the server version into component numbers to compare string values of different lengths
                                                                SELECT
                                                                        @intval = CHARINDEX('.',
                                                                        @strval);
                                                                IF (@intval > 1)
                                                                BEGIN
                                                                        SELECT
                                                                                @ver = LEFT(@strval,
                                                                                @intval - 1);
                                                                        SELECT
                                                                                @v1 =
                                                                                             CASE
                                                                                                     WHEN ISNUMERIC(@ver) = 1 THEN CONVERT(int, @ver)
                                                                                                     ELSE 0
                                                                                             END,
                                                                                @strval = RIGHT(@strval,
                                                                                LEN(@strval)
                                                                                - @intval);

                                                                        SELECT
                                                                                @intval = CHARINDEX('.',
                                                                                @strval);
                                                                        IF (@intval > 1)
                                                                        BEGIN
                                                                                SELECT
                                                                                        @v2 = CONVERT(int, LEFT(@strval,
                                                                                        @intval - 1)),
                                                                                        @strval = RIGHT(@strval,
                                                                                        LEN(@strval)
                                                                                        - @intval);

                                                                                SELECT
                                                                                        @intval = CHARINDEX('.',
                                                                                        @strval);
                                                                                IF (@intval > 1)
                                                                                BEGIN
                                                                                        SELECT
                                                                                                @v3 = CONVERT(int, LEFT(@strval,
                                                                                                @intval - 1)),
                                                                                                @strval = RIGHT(@strval,
                                                                                                LEN(@strval)
                                                                                                - @intval);

                                                                                        IF (LEN(@strval) > 0
                                                                                                AND CHARINDEX('.',
                                                                                                @strval) = 0
                                                                                                AND ISNUMERIC(@strval) = 1
                                                                                                )
                                                                                                SELECT
                                                                                                        @v4 = CONVERT(int, @strval);
                                                                                END;
                                                                                ELSE
                                                                                        SELECT
                                                                                                @v3 =
                                                                                                             CASE
                                                                                                                     WHEN ISNUMERIC(@strval) = 1 AND
                                                                                                                             CHARINDEX('.',
                                                                                                                             @strval) = 0 THEN CONVERT(int, @strval)
                                                                                                                     ELSE 0
                                                                                                             END;
                                                                        END;
                                                                        ELSE
                                                                                SELECT
                                                                                        @v2 =
                                                                                                     CASE
                                                                                                             WHEN ISNUMERIC(@strval) = 1 AND
                                                                                                                     CHARINDEX('.',
                                                                                                                     @strval) = 0 THEN CONVERT(int, @strval)
                                                                                                             ELSE 0
                                                                                                     END;
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @v1 =
                                                                                             CASE
                                                                                                     WHEN ISNUMERIC(@strval) = 1 AND
                                                                                                             CHARINDEX('.',
                                                                                                             @strval) = 0 THEN CONVERT(int, @strval)
                                                                                                     ELSE 0
                                                                                             END;

                                                                --parse the threshold version into component numbers to compare string values of different lengths
                                                                SELECT
                                                                        @strval = @metricthreshold;
                                                                SELECT
                                                                        @intval = CHARINDEX('.',
                                                                        @strval);
                                                                IF (@intval > 1)
                                                                BEGIN
                                                                        SELECT
                                                                                @ver = LEFT(@strval,
                                                                                @intval - 1);
                                                                        SELECT
                                                                                @t1 =
                                                                                             CASE
                                                                                                     WHEN ISNUMERIC(@ver) = 1 AND
                                                                                                             CHARINDEX('.',
                                                                                                             @ver) = 0 THEN CONVERT(int, @ver)
                                                                                                     ELSE 0
                                                                                             END,
                                                                                @strval = RIGHT(@strval,
                                                                                LEN(@strval)
                                                                                - @intval);

                                                                        SELECT
                                                                                @intval = CHARINDEX('.',
                                                                                @strval);
                                                                        IF (@intval > 1)
                                                                        BEGIN
                                                                                SELECT
                                                                                        @t2 = CONVERT(int, LEFT(@strval,
                                                                                        @intval - 1)),
                                                                                        @strval = RIGHT(@strval,
                                                                                        LEN(@strval)
                                                                                        - @intval);

                                                                                SELECT
                                                                                        @intval = CHARINDEX('.',
                                                                                        @strval);
                                                                                IF (@intval > 1)
                                                                                BEGIN
                                                                                        SELECT
                                                                                                @t3 = CONVERT(int, LEFT(@strval,
                                                                                                @intval - 1)),
                                                                                                @strval = RIGHT(@strval,
                                                                                                LEN(@strval)
                                                                                                - @intval);

                                                                                        IF (LEN(@strval) > 0
                                                                                                AND CHARINDEX('.',
                                                                                                @strval) = 0
                                                                                                AND ISNUMERIC(@strval) = 1
                                                                                                )
                                                                                                SELECT
                                                                                                        @t4 = CONVERT(int, @strval);
                                                                                END;
                                                                                ELSE
                                                                                        SELECT
                                                                                                @t3 =
                                                                                                             CASE
                                                                                                                     WHEN ISNUMERIC(@strval) = 1 AND
                                                                                                                             CHARINDEX('.',
                                                                                                                             @strval) = 0 THEN CONVERT(int, @strval)
                                                                                                                     ELSE 0
                                                                                                             END;
                                                                        END;
                                                                        ELSE
                                                                                SELECT
                                                                                        @t2 =
                                                                                                     CASE
                                                                                                             WHEN ISNUMERIC(@strval) = 1 AND
                                                                                                                     CHARINDEX('.',
                                                                                                                     @strval) = 0 THEN CONVERT(int, @strval)
                                                                                                             ELSE 0
                                                                                                     END;
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @t1 =
                                                                                             CASE
                                                                                                     WHEN ISNUMERIC(@strval) = 1 AND
                                                                                                             CHARINDEX('.',
                                                                                                             @strval) = 0 THEN CONVERT(int, @strval)
                                                                                                     ELSE 0
                                                                                             END;

                                                                --compare level by level to see if there is a mismatch
                                                                IF ((@t1 > @v1)
                                                                        OR (@t1 = @v1
                                                                        AND @t2 > @v2
                                                                        )
                                                                        OR (@t1 = @v1
                                                                        AND @t2 = @v2
                                                                        AND @t3 > @v3
                                                                        )
                                                                        OR (@t1 = @v1
                                                                        AND @t2 = @v2
                                                                        AND @t3 = @v3
                                                                        AND @t4 > @v4
                                                                        )
                                                                        )
                                                                        SET @intval2 = 1;
                                                                ELSE
                                                                        SET @intval2 = 0;
                                                        END;

                                                        IF (@intval2 = 0)
                                                                SELECT
                                                                        @sevcode = @sevcodeok;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity;

                                                        SELECT
                                                                @metricval = N'Current version is '
                                                                + @version;
                                                        SELECT
                                                                @metricthreshold = N'Acceptable levels for each SQL Server version are '
                                                                + @severityvalues
                                                                + N' and above.';
                                                END;
                                                -- SQL Authentication Enabled
                                                ELSE
                                                IF (@metricid = 3)
                                                BEGIN
                                                        SELECT
                                                                @severityvalues = N'M';
                                                        IF (@authentication <> @severityvalues)
                                                                SELECT
                                                                        @sevcode = @sevcodeok;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity;

                                                        SELECT
                                                                @metricval = dbo.getauthenticationmodename(@authentication);
                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if '''
                                                                + dbo.getauthenticationmodename(@severityvalues)
                                                                + N''' is enabled.';
                                                END;
                                                -- Login Audit Level
                                                ELSE
                                                IF (@metricid = 4)
                                                BEGIN
                                                        IF (CHARINDEX(''''
                                                                + @loginauditmode
                                                                + '''',
                                                                @severityvalues) > 0)
                                                                SELECT
                                                                        @sevcode = @sevcodeok;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity;
                                                        -- convert values to display values
                                                        SELECT
                                                                @strval = @severityvalues,
                                                                @metricthreshold = '',
                                                                @intval = CHARINDEX(''',''',
                                                                @strval);
                                                        WHILE (@intval > 0)
                                                        BEGIN
                                                                IF LEN(@metricthreshold) > 0
                                                                        SELECT
                                                                                @metricthreshold = @metricthreshold
                                                                                + ',';
                                                                SELECT
                                                                        @metricthreshold = @metricthreshold
                                                                        + ''''
                                                                        + dbo.getloginauditmodename(SUBSTRING(@strval,
                                                                        2, @intval - 2))
                                                                        + '''';
                                                                SELECT
                                                                        @strval = SUBSTRING(@strval,
                                                                        @intval + 2,
                                                                        LEN(@strval)
                                                                        - (@intval + 1)),
                                                                        @intval = CHARINDEX(''',''',
                                                                        @strval);
                                                        END;
                                                        IF LEN(@strval) > 0
                                                        BEGIN
                                                                IF LEN(@metricthreshold) > 0
                                                                        SELECT
                                                                                @metricthreshold = @metricthreshold
                                                                                + ', ';
                                                                SELECT
                                                                        @metricthreshold = @metricthreshold
                                                                        + ''''
                                                                        + dbo.getloginauditmodename(SUBSTRING(@strval,
                                                                        2,
                                                                        LEN(@strval) - 2))
                                                                        + '''';
                                                        END;

                                                        SELECT
                                                                @metricval = dbo.getloginauditmodename(@loginauditmode);
                                                        SELECT
                                                                @metricthreshold = N'Login auditing is acceptable if set to '
                                                                + @metricthreshold
                                                                + '.';
                                                END;
                                                -- Cross Database Ownership Chaining Enabled
                                                ELSE
                                                IF (@metricid = 5)
                                                BEGIN
                                                        SELECT
                                                                @severityvalues = N'Y';
                                                        IF (@crossdb <> @severityvalues)
                                                                SELECT
                                                                        @sevcode = @sevcodeok;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity;

                                                        SELECT
                                                                @metricval = dbo.getyesnotext(@crossdb);
                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if Cross Database Ownership Chaining is enabled.';
                                                END;
                                                -- Guest User Enabled
                                                ELSE
                                                IF (@metricid = 6)
                                                BEGIN
                                                        SELECT
                                                                @sql = N'declare dbcursor cursor static for
												select databasename
													from sqldatabase
													where snapshotid = '
                                                                + CONVERT(nvarchar, @snapshotid)
                                                                + N'
														and databasename not in ('
                                                                + @severityvalues
                                                                + N')
														and guestenabled = ''Y''
													order by databasename';
                                                        EXEC (@sql);
                                                        OPEN dbcursor;
                                                        SELECT
                                                                @intval2 = 0;
                                                        FETCH NEXT FROM
                                                        dbcursor INTO @strval;
                                                        WHILE @@fetch_status = 0
                                                        BEGIN
                                                                IF (@intval2 = 1
                                                                        OR LEN(@metricval)
                                                                        + LEN(@strval) > 1010
                                                                        )
                                                                BEGIN
                                                                        IF @intval2 = 0
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + N', more...',
                                                                                        @intval2 = 1;
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @metricval = @metricval
                                                                                + CASE
                                                                                        WHEN LEN(@metricval) > 0 THEN N', '
                                                                                        ELSE N''
                                                                                END + N''''
                                                                                + @strval
                                                                                + N'''';

                                                                IF (@isadmin = 1)
                                                                        INSERT INTO policyassessmentdetail (policyid,
                                                                        assessmentid,
                                                                        metricid,
                                                                        snapshotid,
                                                                        detailfinding,
                                                                        databaseid,
                                                                        objecttype,
                                                                        objectid,
                                                                        objectname)
                                                                                VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Database with Guest user access: ''' + @strval + N'''', NULL, -- database ID,
                                                                                N'DB', -- object type
                                                                                NULL, -- object id
                                                                                @strval);

                                                                FETCH NEXT FROM
                                                                dbcursor INTO @strval;
                                                        END;

                                                        IF (LEN(@metricval) = 0)
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'None found.';
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity,
                                                                        @metricval = N'Databases with Guest user access: '
                                                                        + @metricval;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if Guest user access is available on databases other than: '
                                                                + @severityvalues;

                                                        CLOSE dbcursor;
                                                        DEALLOCATE dbcursor;
                                                END;
                                                -- Suspect Logins
                                                ELSE
                                                IF (@metricid = 7)
                                                BEGIN
                                                        -- This should return the same results as the report SP
                                                        SELECT
                                                                @severityvalues = N'Y';
                                                        SELECT
                                                                @intval = COUNT(*)
                                                        FROM serverprincipal a
                                                        INNER JOIN windowsaccount b
                                                                ON a.snapshotid = b.snapshotid
                                                                AND a.sid = b.sid
                                                                LEFT JOIN ancillarywindowsgroup c
                                                                        ON a.snapshotid = c.snapshotid
                                                                        AND a.name = c.windowsgroupname
                                                        WHERE a.snapshotid = @snapshotid
                                                        AND a.type IN (
                                                        'G', 'U')	-- Principal type is Windows Group or User
                                                        AND b.state = 'S'			-- State is suspect
                                                        AND c.windowsgroupname IS NULL;	-- Account is not OS controlled well-known

                                                        IF (@intval = 0)
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @strval = N'N';
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity,
                                                                        @strval = N'Y';

                                                        SELECT
                                                                @metricval = dbo.getyesnotext(@strval);
                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if any Windows Accounts have permissions on the server, but could not be verified with Active Directory.';
                                                END;
                                                -- C2 Audit Trace Enabled
                                                ELSE
                                                IF (@metricid = 8)
                                                BEGIN
                                                        SELECT
                                                                @strval = @c2audittrace,
                                                                @severityvalues = N'Y';
                                                        IF (@strval = @severityvalues)
                                                                SELECT
                                                                        @sevcode = @sevcodeok;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity;

                                                        SELECT
                                                                @metricval = dbo.getyesnotext(@strval);
                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if the C2 audit trace is not enabled.';
                                                END;
                                                -- Proxy Account Enabled
                                                ELSE
                                                IF (@metricid = 9)
                                                BEGIN
                                                        SELECT
                                                                @strval = @proxy,
                                                                @severityvalues = N'N';
                                                        IF (@strval = @severityvalues)
                                                                SELECT
                                                                        @sevcode = @sevcodeok;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity;

                                                        SELECT
                                                                @metricval = dbo.getyesnotext(@strval);
                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if the proxy account is enabled.';
                                                END;
                                                -- DAC Remote Access Enabled
                                                ELSE
                                                IF (@metricid = 10)
                                                BEGIN
                                                        SELECT
                                                                @strval = @remotedac,
                                                                @severityvalues = N'N';
                                                        -- this is a 2005 and up only value, so just mark it ok in 2000
                                                        IF (@version > N'9.'
                                                                OR @version < N'6.'
                                                                )
                                                                IF (@strval = @severityvalues)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok;
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @severity;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @strval = N'N/A';

                                                        SELECT
                                                                @metricval = dbo.getyesnotext(@strval);
                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if remote access through a Dedicated Administrator Connection is enabled.';
                                                END;
                                                -- Integration Services
                                                ELSE
                                                IF (@metricid = 11)
                                                BEGIN
                                                        IF (LEN(@severityvalues) > 0)
                                                        BEGIN
                                                                CREATE TABLE #roletbl (
                                                                        rolename
                                                                        nvarchar(128)
                                                                        COLLATE
                                                                        DATABASE_DEFAULT
                                                                );

                                                                INSERT INTO #roletbl
                                                                EXEC @err = [dbo].[isp_sqlsecure_getmsdbvalidroleslist];

                                                                SELECT
                                                                        @sql = N'declare proccursur cursor static for
													select a.name as objectname, c.name as granteename, b.permission
														from databaseobject a,
															databaseobjectpermission b,
															databaseprincipal c
														where a.snapshotid = '
                                                                        + CONVERT(nvarchar, @snapshotid)
                                                                        + N' 
															and a.dbid = 4 and a.type = N''P''
															and a.name in ('
                                                                        + @severityvalues
                                                                        + N')
															and a.snapshotid = b.snapshotid
															and a.dbid = b.dbid
															and a.objectid = b.objectid
															and b.snapshotid = c.snapshotid
															and b.dbid = c.dbid
															and (b.isgrant = N''Y'' or b.isgrantwith = N''Y'')
															and b.grantee = c.uid
															and c.name not in (select rolename from #roletbl)
														order by objectname, granteename';

                                                                EXEC (@sql);
                                                                OPEN proccursur;

                                                                SELECT
                                                                        @intval2 = 0;
                                                                FETCH NEXT FROM
                                                                proccursur INTO @strval,
                                                                @strval2,
                                                                @strval3;
                                                                WHILE @@fetch_status = 0
                                                                BEGIN
                                                                        IF (@intval2 = 1
                                                                                OR LEN(@metricval)
                                                                                + LEN(@strval) > 1010
                                                                                )
                                                                        BEGIN
                                                                                IF @intval2 = 0
                                                                                        SELECT
                                                                                                @metricval = @metricval
                                                                                                + N', more...',
                                                                                                @intval2 = 1;
                                                                        END;
                                                                        ELSE
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + CASE
                                                                                                WHEN LEN(@metricval) > 0 THEN N', '
                                                                                                ELSE N''
                                                                                        END + @strval3
                                                                                        + N' granted to '''
                                                                                        + @strval2
                                                                                        + N''' on '''
                                                                                        + @strval
                                                                                        + N'''';

                                                                        IF (@isadmin = 1)
                                                                                INSERT INTO policyassessmentdetail (policyid,
                                                                                assessmentid,
                                                                                metricid,
                                                                                snapshotid,
                                                                                detailfinding,
                                                                                databaseid,
                                                                                objecttype,
                                                                                objectid,
                                                                                objectname)
                                                                                        VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Permissions on stored procedure found: ' + @strval3 + N' granted to ''' + @strval2 + N''' on ''' + @strval + N'''', 4, -- database ID,
                                                                                        N'P', -- object type
                                                                                        NULL, @strval);

                                                                        FETCH NEXT FROM
                                                                        proccursur INTO @strval,
                                                                        @strval2,
                                                                        @strval3;
                                                                END;
                                                                CLOSE proccursur;
                                                                DEALLOCATE proccursur;

                                                                IF (LEN(@metricval) = 0)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = N'No permissions found.';
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @severity,
                                                                                @metricval = N'Permissions on stored procedures found: '
                                                                                + @metricval;

                                                                DROP TABLE #roletbl;
                                                        END;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'No list of Integration Services stored procedures was provided.';

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if permissions have been granted on any of these stored procedures: '
                                                                + @severityvalues;
                                                END;
                                                -- 'OLAP SQL Authentication Enabled
                                                ELSE
                                                IF (@metricid = 12)
                                                BEGIN
                                                        -- this metric has been removed, but a placeholder is left here so it won't be reused
                                                        SELECT
                                                                @err = 0,
                                                                @sevcode = -1,
                                                                @metricval = N'',
                                                                @metricthreshold = @severityvalues;
                                                END;
                                                -- SQL Mail Enabled
                                                ELSE
                                                IF (@metricid = 13)
                                                BEGIN
                                                        -- test for 2005 or later
                                                        IF ((@version > N'9.'
                                                                OR @version < N'6.'
                                                                )
                                                                AND @sqlmail = N'N'
                                                                AND @databasemail = N'N'
                                                                )
                                                        BEGIN
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'SQL Mail is not enabled.';
                                                        END;
                                                        ELSE
                                                        BEGIN
                                                                -- This should return the same results as the report SP
                                                                SELECT
                                                                        @severityvalues = N'Y';
                                                                SELECT
                                                                        @intval = COUNT(*)
                                                                FROM databaseobject
                                                                WHERE snapshotid = @snapshotid
                                                                AND [type] = 'X'
                                                                AND [name] LIKE 'xp_%mail';

                                                                IF (@intval = 0)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = N'No SQL Mail stored procedures were found.';
                                                                ELSE
                                                                BEGIN
                                                                        SELECT
                                                                                @sevcode = @severity,
                                                                                @metricval = N'SQL Mail stored procedures found';
                                                                        IF (@sqlmail = @severityvalues)
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + N' and SQL Mail is enabled';
                                                                        IF (@databasemail = @severityvalues)
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + N' and Database Mail is enabled';
                                                                END;
                                                                SELECT
                                                                        @metricval = @metricval
                                                                        + '.';
                                                        END;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if Sql Mail stored procedures like ''xp_%mail'' are found and SQL Mail or Database Mail are enabled (Sql 2005 only).';
                                                END;
                                                -- SQL Agent Mail Enabled
                                                ELSE
                                                IF (@metricid = 14)
                                                BEGIN
                                                        IF (LEN(RTRIM(@agentmailprofile)) = 0)
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'SQL Agent Mail profile not found.';
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity,
                                                                        @metricval = N'SQL Agent Mail profile found: '
                                                                        + @agentmailprofile;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if a Sql Agent Mail profile exists.';
                                                END;
                                                -- Sample Databases Exist
                                                ELSE
                                                IF (@metricid = 15)
                                                BEGIN
                                                        IF LEN(@severityvalues) > 0
                                                        BEGIN
                                                                SELECT
                                                                        @sql = N'declare dbcursor cursor static for
													select [databasename]
														from vwdatabases 
														where snapshotid = '
                                                                        + CONVERT(nvarchar, @snapshotid)
                                                                        + N'
															and [databasename] in ('
                                                                        + @severityvalues
                                                                        + N')
														order by [databasename]';
                                                                EXEC (@sql);
                                                                OPEN dbcursor;

                                                                SELECT
                                                                        @strval = '',
                                                                        @intval2 = 0;
                                                                FETCH NEXT FROM
                                                                dbcursor INTO @strval;
                                                                WHILE @@fetch_status = 0
                                                                BEGIN
                                                                        IF (@intval2 = 1
                                                                                OR LEN(@metricval)
                                                                                + LEN(@strval) > 1010
                                                                                )
                                                                        BEGIN
                                                                                IF @intval2 = 0
                                                                                        SELECT
                                                                                                @metricval = @metricval
                                                                                                + N', more...',
                                                                                                @intval2 = 1;
                                                                        END;
                                                                        ELSE
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + CASE
                                                                                                WHEN LEN(@metricval) > 0 THEN N', '
                                                                                                ELSE N''
                                                                                        END + N''''
                                                                                        + @strval
                                                                                        + N'''';

                                                                        IF (@isadmin = 1)
                                                                                INSERT INTO policyassessmentdetail (policyid,
                                                                                assessmentid,
                                                                                metricid,
                                                                                snapshotid,
                                                                                detailfinding,
                                                                                databaseid,
                                                                                objecttype,
                                                                                objectid,
                                                                                objectname)
                                                                                        VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Sample database found: ''' + @strval + N'''', NULL, -- database ID,
                                                                                        N'DB', -- object type
                                                                                        NULL, @strval);

                                                                        FETCH NEXT FROM
                                                                        dbcursor INTO @strval;
                                                                END;

                                                                CLOSE dbcursor;
                                                                DEALLOCATE dbcursor;

                                                                IF (LEN(@metricval) = 0)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = N'None found.';
                                                                ELSE
                                                                BEGIN
                                                                        SELECT
                                                                                @sevcode = @severity,
                                                                                @metricval = N'Sample databases found: '
                                                                                + @metricval;
                                                                END;
                                                        END;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'No sample databases were provided.';

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if these sample databases exist: '
                                                                + @severityvalues;
                                                END;
                                                -- sa Account Exists
                                                ELSE
                                                IF (@metricid = 16)
                                                BEGIN
                                                        -- only apply this check if the version is 2005 or greater
                                                        IF (@version > N'9.'
                                                                OR @version < N'6.'
                                                                )
                                                        BEGIN
                                                                -- check to make sure the sa account is either disabled or renamed
                                                                SELECT
                                                                        @severityvalues = N'N';
                                                                SELECT
                                                                        @metricval = [name],
                                                                        @strval = [disabled]
                                                                FROM serverprincipal
                                                                WHERE snapshotid = @snapshotid
                                                                AND sid = 0x01;
                                                                IF (LOWER(@metricval) = N'sa'
                                                                        AND @strval = @severityvalues
                                                                        )
                                                                        SELECT
                                                                                @sevcode = @severity;
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @sevcodeok;

                                                                SELECT
                                                                        @metricval = N'The sa account is named '''
                                                                        + @metricval
                                                                        + N''''
                                                                        + CASE
                                                                                WHEN @strval = @severityvalues THEN N'.'
                                                                                ELSE N' and is enabled.'
                                                                        END;
                                                        END;
                                                        ELSE
                                                        BEGIN
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'The sa account is always enabled on SQL Server 2000.';
                                                        END;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if the sa account has not been renamed and is enabled.';
                                                END;
                                                -- sa Account Blank Password
                                                ELSE
                                                IF (@metricid = 17)
                                                BEGIN
                                                        SELECT
                                                                @strval = @sapassword,
                                                                @severityvalues = N'N';
                                                        IF (@strval = @severityvalues)
                                                                SELECT
                                                                        @sevcode = @sevcodeok;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity;

                                                        SELECT
                                                                @metricval = dbo.getyesnotext(@strval);
                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if the sa account has a blank password.';
                                                END;
                                                -- System Table Updates
                                                ELSE
                                                IF (@metricid = 18)
                                                BEGIN
                                                        SELECT
                                                                @strval = @systemtables,
                                                                @severityvalues = N'N';
                                                        IF (@strval = @severityvalues)
                                                                SELECT
                                                                        @sevcode = @sevcodeok;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity;

                                                        SELECT
                                                                @metricval = dbo.getyesnotext(@strval);
                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if System Table Updates are enabled.';
                                                END;
                                                -- Everyone System Table Access
                                                ELSE
                                                IF (@metricid = 19)
                                                BEGIN
                                                        -- This uses the same logic as the report SP
                                                        IF NOT EXISTS (SELECT
                                                                        *
                                                                FROM windowsaccount
                                                                WHERE snapshotid = @snapshotid
                                                                AND sid = @everyonesid)
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'Everyone account not found on server.';
                                                        ELSE
                                                        BEGIN
                                                                -- this code is based on getuserpermissions to setup the list of db users
                                                                CREATE TABLE #logintbl (
                                                                        sid
                                                                        varbinary(85)
                                                                );

                                                                -- insert everyone user in the table, just in case even though it shouldn't
                                                                INSERT INTO #logintbl (sid)
                                                                        VALUES (@everyonesid);

                                                                INSERT INTO #logintbl
                                                                EXEC @err = [dbo].[isp_sqlsecure_getwindowsgroupparents] @snapshotid = @snapshotid,
                                                                                                                         @inputsid = @everyonesid;

                                                                CREATE TABLE #usertbl (
                                                                        dbid
                                                                        int,
                                                                        uid int
                                                                );

                                                                DECLARE @dbid int,
                                                                        @uid int;

                                                                DECLARE logincursor CURSOR FOR
                                                                SELECT DISTINCT
                                                                        a.dbid,
                                                                        a.uid
                                                                FROM databaseprincipal a,
                                                                     #logintbl b
                                                                WHERE a.snapshotid = @snapshotid
                                                                AND a.usersid = b.sid
                                                                -- select any object from master db to get system tables for now
                                                                AND a.dbid = 1;

                                                                OPEN logincursor;
                                                                FETCH NEXT FROM
                                                                logincursor INTO @dbid,
                                                                @uid;

                                                                WHILE @@fetch_status = 0
                                                                BEGIN
                                                                        INSERT INTO #usertbl
                                                                        EXEC isp_sqlsecure_getdatabaseuserparents @snapshotid,
                                                                                                                  @dbid,
                                                                                                                  @uid;

                                                                        FETCH NEXT FROM
                                                                        logincursor INTO @dbid,
                                                                        @uid;

                                                                END;

                                                                CLOSE logincursor;
                                                                DEALLOCATE logincursor;

                                                                -- check if user 'guest' is valid. If so, then current login will have public database role even if there is
                                                                --	no database user mapped to it.
                                                                IF EXISTS (SELECT
                                                                                *
                                                                        FROM databaseprincipal a
                                                                        WHERE UPPER(a.name) = 'GUEST'
                                                                        AND UPPER(a.hasaccess) = 'Y'
                                                                        AND a.snapshotid = @snapshotid)
                                                                BEGIN
                                                                        -- public uid is always 0
                                                                        INSERT INTO #usertbl (dbid,
                                                                        uid)
                                                                                SELECT
                                                                                        dbid,
                                                                                        0
                                                                                FROM databaseprincipal a
                                                                                WHERE UPPER(a.name) = 'GUEST'
                                                                                AND UPPER(a.hasaccess) = 'Y'
                                                                                AND a.snapshotid = @snapshotid;

                                                                        -- insert guest user as well
                                                                        INSERT INTO #usertbl (dbid,
                                                                        uid)
                                                                                SELECT DISTINCT
                                                                                        dbid,
                                                                                        uid
                                                                                FROM databaseprincipal a
                                                                                WHERE UPPER(a.name) = 'GUEST'
                                                                                AND UPPER(a.hasaccess) = 'Y'
                                                                                AND snapshotid = @snapshotid;
                                                                END;

                                                                -- insert alias users
                                                                INSERT INTO #usertbl (dbid,
                                                                uid)
                                                                        SELECT DISTINCT
                                                                                dbid,
                                                                                altuid
                                                                        FROM databaseprincipal
                                                                        WHERE snapshotid = @snapshotid
                                                                        AND isalias = 'Y'
                                                                        AND altuid IS NOT NULL
                                                                        AND usersid IN (SELECT DISTINCT
                                                                                sid
                                                                        FROM #logintbl);

                                                                SELECT
                                                                        @intval = COUNT(*)
                                                                FROM vwdatabaseobjectpermission a,
                                                                     #usertbl b,
                                                                     databaseprincipal c
                                                                WHERE a.snapshotid = @snapshotid
                                                                AND a.dbid = b.dbid
                                                                AND a.grantee = b.uid
                                                                AND a.snapshotid = c.snapshotid
                                                                AND a.dbid = c.dbid
                                                                AND (b.uid = c.uid
                                                                OR (UPPER(c.isalias) = 'Y'
                                                                AND c.altuid = b.uid
                                                                )
                                                                );
                                                                IF (@intval = 0)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = N'Everyone does not have access to any system tables.';
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @severity,
                                                                                @metricval = N'Everyone has access to '
                                                                                + CONVERT(nvarchar, @intval)
                                                                                + N' system tables.';

                                                                DROP TABLE #usertbl;
                                                                DROP TABLE #logintbl;
                                                        END;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if the Everyone Windows group has access to any system tables.';
                                                END;
                                                -- Startup Stored Procedures Enabled
                                                ELSE
                                                IF (@metricid = 20)
                                                BEGIN
                                                        SELECT
                                                                @strval = @startupprocs,
                                                                @severityvalues = N'N';
                                                        IF (@strval = @severityvalues)
                                                                SELECT
                                                                        @sevcode = @sevcodeok;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity;

                                                        SELECT
                                                                @metricval = dbo.getyesnotext(@strval);
                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if scan for stored procedures at startup is enabled.';
                                                END;
                                                -- Startup Stored Procedures
                                                ELSE
                                                IF (@metricid = 21)
                                                BEGIN
                                                        SELECT
                                                                @sql = N'declare proccursur cursor static for
													select [name]
														from vwdatabaseobject
														where snapshotid = '
                                                                + CONVERT(nvarchar, @snapshotid)
                                                                + N'
															and [type] in (N''P'', N''X'')
															 and runatstartup = N''Y''';
                                                        IF (LEN(@severityvalues) > 0)
                                                                SELECT
                                                                        @sql = @sql
                                                                        + ' and [name] not in ('
                                                                        + @severityvalues
                                                                        + N')';
                                                        SELECT
                                                                @sql = @sql
                                                                + ' order by [name]';

                                                        EXEC (@sql);
                                                        OPEN proccursur;

                                                        SELECT
                                                                @intval2 = 0;
                                                        FETCH NEXT FROM
                                                        proccursur INTO @strval;
                                                        WHILE @@fetch_status = 0
                                                        BEGIN
                                                                IF (@intval2 = 1
                                                                        OR LEN(@metricval)
                                                                        + LEN(@strval) > 1010
                                                                        )
                                                                BEGIN
                                                                        IF @intval2 = 0
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + N', more...',
                                                                                        @intval2 = 1;
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @metricval = @metricval
                                                                                + CASE
                                                                                        WHEN LEN(@metricval) > 0 THEN N', '
                                                                                        ELSE N''
                                                                                END + N''''
                                                                                + @strval
                                                                                + N'''';

                                                                IF (@isadmin = 1)
                                                                        INSERT INTO policyassessmentdetail (policyid,
                                                                        assessmentid,
                                                                        metricid,
                                                                        snapshotid,
                                                                        detailfinding,
                                                                        databaseid,
                                                                        objecttype,
                                                                        objectid,
                                                                        objectname)
                                                                                VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Unapproved stored procedures found: ''' + @strval + N'''', NULL, -- database ID,
                                                                                N'P', -- object type
                                                                                NULL, @strval);

                                                                FETCH NEXT FROM
                                                                proccursur INTO @strval;
                                                        END;

                                                        IF (LEN(@metricval) = 0)
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'None found';
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity,
                                                                        @metricval = N'Unapproved stored procedures found: '
                                                                        + @metricval;

                                                        CLOSE proccursur;
                                                        DEALLOCATE proccursur;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if startup stored procedures exist'
                                                                + CASE
                                                                        WHEN LEN(@severityvalues) > 0 THEN N' and are not in '
                                                                                + @severityvalues
                                                                                + N'.'
                                                                        ELSE N'.'
                                                                END;
                                                END;
                                                -- Stored Procedures Encrypted
                                                ELSE
                                                IF (@metricid = 22)
                                                BEGIN
                                                        DECLARE proccursur CURSOR STATIC FOR
                                                        SELECT
                                                                b.databasename,
                                                                COUNT(a.objectid)
                                                        FROM vwdatabaseobject a,
                                                             vwdatabases b
                                                        WHERE a.snapshotid = @snapshotid
                                                        AND a.snapshotid = b.snapshotid
                                                        AND a.dbid = b.dbid
                                                        AND a.[type] = N'P'
                                                        AND a.isencrypted = N'N'
                                                        GROUP BY b.databasename;

                                                        OPEN proccursur;

                                                        SELECT
                                                                @intval2 = 0;
                                                        FETCH NEXT FROM
                                                        proccursur INTO @strval,
                                                        @intval;
                                                        WHILE @@fetch_status = 0
                                                        BEGIN
                                                                SELECT
                                                                        @strval = @strval
                                                                        + N' ('
                                                                        + CONVERT(nvarchar, @intval)
                                                                        + N')',
                                                                        @strval2 = 'Unencrypted stored procedures found in these databases: ';
                                                                IF (@intval2 = 1
                                                                        OR LEN(@metricval)
                                                                        + LEN(@strval) > (1010
                                                                        - LEN(@strval2))
                                                                        )
                                                                BEGIN
                                                                        IF @intval2 = 0
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + N', more...',
                                                                                        @intval2 = 1;
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @metricval = @metricval
                                                                                + CASE
                                                                                        WHEN LEN(@metricval) > 0 THEN N', '
                                                                                        ELSE N''
                                                                                END + N''''
                                                                                + @strval
                                                                                + N'''';

                                                                IF (@isadmin = 1)
                                                                        INSERT INTO policyassessmentdetail (policyid,
                                                                        assessmentid,
                                                                        metricid,
                                                                        snapshotid,
                                                                        detailfinding,
                                                                        databaseid,
                                                                        objecttype,
                                                                        objectid,
                                                                        objectname)
                                                                                VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Unencrypted stored procedures found in the database: ''' + @strval + N'''', NULL, -- database ID,
                                                                                N'DB', -- object type
                                                                                NULL, @strval);

                                                                FETCH NEXT FROM
                                                                proccursur INTO @strval,
                                                                @intval;
                                                        END;

                                                        IF (LEN(@metricval) = 0)
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'No unencrypted stored procedures were found.';
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity,
                                                                        @metricval = @strval2
                                                                        + @metricval;

                                                        CLOSE proccursur;
                                                        DEALLOCATE proccursur;

                                                        SELECT
                                                                @metricthreshold = 'Server is vulnerable if unencrypted stored procedures exist.';
                                                END;
                                                -- User Defined Extended Stored Procedures (XPs)
                                                ELSE
                                                IF (@metricid = 23)
                                                BEGIN
                                                        SELECT
                                                                @sql = N'declare proccursur cursor static for
													select [name]
														from vwdatabaseobject
														where snapshotid = '
                                                                + CONVERT(nvarchar, @snapshotid)
                                                                + N'
															and [type] = N''X''
															and userdefined = N''Y''';
                                                        IF (LEN(@severityvalues) > 0)
                                                                SELECT
                                                                        @sql = @sql
                                                                        + ' and [name] not in ('
                                                                        + @severityvalues
                                                                        + N')';
                                                        SELECT
                                                                @sql = @sql
                                                                + ' order by [name]';

                                                        EXEC (@sql);
                                                        OPEN proccursur;

                                                        SELECT
                                                                @intval2 = 0;
                                                        FETCH NEXT FROM
                                                        proccursur INTO @strval;
                                                        WHILE @@fetch_status = 0
                                                        BEGIN
                                                                IF (@intval2 = 1
                                                                        OR LEN(@metricval)
                                                                        + LEN(@strval) > 1010
                                                                        )
                                                                BEGIN
                                                                        IF @intval2 = 0
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + N', more...',
                                                                                        @intval2 = 1;
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @metricval = @metricval
                                                                                + CASE
                                                                                        WHEN LEN(@metricval) > 0 THEN N', '
                                                                                        ELSE N''
                                                                                END + N''''
                                                                                + @strval
                                                                                + N'''';

                                                                IF (@isadmin = 1)
                                                                        INSERT INTO policyassessmentdetail (policyid,
                                                                        assessmentid,
                                                                        metricid,
                                                                        snapshotid,
                                                                        detailfinding,
                                                                        databaseid,
                                                                        objecttype,
                                                                        objectid,
                                                                        objectname)
                                                                                VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Unapproved extended stored procedure found: ''' + @strval + N'''', NULL, -- database ID,
                                                                                N'X', -- object type
                                                                                NULL, @strval);

                                                                FETCH NEXT FROM
                                                                proccursur INTO @strval;
                                                        END;

                                                        IF (LEN(@metricval) = 0)
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'None found';
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity,
                                                                        @metricval = N'Unapproved extended stored procedures found: '
                                                                        + @metricval;

                                                        CLOSE proccursur;
                                                        DEALLOCATE proccursur;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if user defined extended stored procedures exist'
                                                                + CASE
                                                                        WHEN LEN(@severityvalues) > 0 THEN N' not in '
                                                                                + @severityvalues
                                                                        ELSE N''
                                                                END + N'.';
                                                END;
                                                -- Dangerous Extended Stored Procedures (XPs)
                                                ELSE
                                                IF (@metricid = 24)
                                                BEGIN
                                                        IF (LEN(@severityvalues) > 0)
                                                        BEGIN
                                                                SELECT
                                                                        @sql = N'declare proccursur cursor static for
													select a.name as objectname, c.name as granteename, b.permission
														from databaseobject a,
															databaseobjectpermission b,
															databaseprincipal c
														where a.snapshotid = '
                                                                        + CONVERT(nvarchar, @snapshotid)
                                                                        + N' 
															and a.type = N''X''
															and a.name in ('
                                                                        + @severityvalues
                                                                        + N')
															and a.snapshotid = b.snapshotid
															and a.dbid = b.dbid
															and a.objectid = b.objectid
															and b.snapshotid = c.snapshotid
															and b.dbid = c.dbid
															and (b.isgrant = N''Y'' or b.isgrantwith = N''Y'')
															and b.grantee = c.uid
														order by objectname, granteename';
                                                                EXEC (@sql);
                                                                OPEN proccursur;

                                                                SELECT
                                                                        @intval2 = 0;
                                                                FETCH NEXT FROM
                                                                proccursur INTO @strval,
                                                                @strval2,
                                                                @strval3;
                                                                WHILE @@fetch_status = 0
                                                                BEGIN
                                                                        IF (@intval2 = 1
                                                                                OR LEN(@metricval)
                                                                                + LEN(@strval) > 1010
                                                                                )
                                                                        BEGIN
                                                                                IF @intval2 = 0
                                                                                        SELECT
                                                                                                @metricval = @metricval
                                                                                                + N', more...',
                                                                                                @intval2 = 1;
                                                                        END;
                                                                        ELSE
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + CASE
                                                                                                WHEN LEN(@metricval) > 0 THEN N', '
                                                                                                ELSE N''
                                                                                        END + @strval3
                                                                                        + N' granted to '''
                                                                                        + @strval2
                                                                                        + N''' on '''
                                                                                        + @strval
                                                                                        + N'''';

                                                                        IF (@isadmin = 1)
                                                                                INSERT INTO policyassessmentdetail (policyid,
                                                                                assessmentid,
                                                                                metricid,
                                                                                snapshotid,
                                                                                detailfinding,
                                                                                databaseid,
                                                                                objecttype,
                                                                                objectid,
                                                                                objectname)
                                                                                        VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Permission on extended stored procedures found: ''' + @strval3 + N' granted to ''' + @strval2 + N''' on ''' + @strval + N'''', NULL, -- database ID,
                                                                                        N'X', -- object type
                                                                                        NULL, @strval);

                                                                        FETCH NEXT FROM
                                                                        proccursur INTO @strval,
                                                                        @strval2,
                                                                        @strval3;
                                                                END;
                                                                CLOSE proccursur;
                                                                DEALLOCATE proccursur;

                                                                IF (LEN(@metricval) = 0)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = N'No permissions found.';
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @severity,
                                                                                @metricval = N'Permissions on extended stored procedures found: '
                                                                                + @metricval;
                                                        END;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'No list of dangerous extended stored procedures was provided.';

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if permissions have been granted on any of these extended stored procedures: '
                                                                + @severityvalues;
                                                END;
                                                -- Remote Access
                                                ELSE
                                                IF (@metricid = 25)
                                                BEGIN
                                                        SELECT
                                                                @strval = @remoteaccess,
                                                                @severityvalues = N'N';
                                                        IF (@strval = @severityvalues)
                                                                SELECT
                                                                        @sevcode = @sevcodeok;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity;

                                                        SELECT
                                                                @metricval = dbo.getyesnotext(@strval);
                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if remote access is enabled.';
                                                END;
                                                -- Protocols
                                                ELSE
                                                IF (@metricid = 26)
                                                BEGIN
                                                        SELECT
                                                                @sql = N'declare protocolcursur cursor static for
												select protocolname
													from serverprotocol
													where snapshotid = '
                                                                + CONVERT(nvarchar, @snapshotid)
                                                                + N'
														and protocolname not in ('
                                                                + @severityvalues
                                                                + N')
													order by protocolname';
                                                        EXEC (@sql);
                                                        OPEN protocolcursur;

                                                        SELECT
                                                                @intval2 = 0;
                                                        FETCH NEXT FROM
                                                        protocolcursur INTO @strval;
                                                        WHILE @@fetch_status = 0
                                                        BEGIN
                                                                IF (@intval2 = 1
                                                                        OR LEN(@metricval)
                                                                        + LEN(@strval) > 1010
                                                                        )
                                                                BEGIN
                                                                        IF @intval2 = 0
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + N', more...',
                                                                                        @intval2 = 1;
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @metricval = @metricval
                                                                                + CASE
                                                                                        WHEN LEN(@metricval) > 0 THEN N', '
                                                                                        ELSE N''
                                                                                END + N''''
                                                                                + @strval
                                                                                + N'''';

                                                                IF (@isadmin = 1)
                                                                        INSERT INTO policyassessmentdetail (policyid,
                                                                        assessmentid,
                                                                        metricid,
                                                                        snapshotid,
                                                                        detailfinding,
                                                                        databaseid,
                                                                        objecttype,
                                                                        objectid,
                                                                        objectname)
                                                                                VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Unapproved protocol found: ''' + @strval + N'''', NULL, -- database ID,
                                                                                N'PR', -- there is no type for protocols
                                                                                NULL, @strval);

                                                                FETCH NEXT FROM
                                                                protocolcursur INTO @strval;
                                                        END;

                                                        IF (LEN(@metricval) = 0)
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'None found.';
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity,
                                                                        @metricval = N'Unapproved protocols found: '
                                                                        + @metricval;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if using protocols other than: '
                                                                + @severityvalues;

                                                        CLOSE protocolcursur;
                                                        DEALLOCATE protocolcursur;
                                                END;
                                                -- Common TCP Port Used
                                                ELSE
                                                IF (@metricid = 27)
                                                BEGIN
                                                        SELECT
                                                                @sql = N'declare portcursur cursor static for
												select case when dynamicport = N''Y'' then N''dynamic'' else isnull([port],'''') end as port
													from serverprotocol
													where snapshotid = '
                                                                + CONVERT(nvarchar, @snapshotid)
                                                                + N'
														and protocolname = ''TCP/IP''
														and case when dynamicport = N''Y'' then N''dynamic'' else isnull([port],'''') end in ('''''
                                                                + CASE
                                                                        WHEN LEN(@severityvalues) > 0 THEN N','
                                                                                + @severityvalues
                                                                        ELSE N''
                                                                END + N')
													order by port';
                                                        --	Check for dynamic removed from where clause per PR 802421 02/05/2010
                                                        --	dynamic must now be entered in the list to be a finding
                                                        --	or dynamicport = N''Y'')
                                                        EXEC (@sql);
                                                        OPEN portcursur;

                                                        SELECT
                                                                @intval2 = 0;
                                                        FETCH NEXT FROM
                                                        portcursur INTO @strval;
                                                        WHILE @@fetch_status = 0
                                                        BEGIN
                                                                IF (@intval2 = 1
                                                                        OR LEN(@metricval)
                                                                        + LEN(@strval) > 1010
                                                                        )
                                                                BEGIN
                                                                        IF @intval2 = 0
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + N', more...',
                                                                                        @intval2 = 1;
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @metricval = @metricval
                                                                                + CASE
                                                                                        WHEN LEN(@metricval) > 0 THEN N', '
                                                                                        ELSE N''
                                                                                END + N''''
                                                                                + @strval
                                                                                + N'''';

                                                                IF (@isadmin = 1)
                                                                        INSERT INTO policyassessmentdetail (policyid,
                                                                        assessmentid,
                                                                        metricid,
                                                                        snapshotid,
                                                                        detailfinding,
                                                                        databaseid,
                                                                        objecttype,
                                                                        objectid,
                                                                        objectname)
                                                                                VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Unapproved TCP/IP port found: ''' + @strval + N'''', NULL, -- database ID,
                                                                                N'PR', -- there is no type for ports
                                                                                NULL, @strval);

                                                                FETCH NEXT FROM
                                                                portcursur INTO @strval;
                                                        END;

                                                        IF (LEN(@metricval) = 0)
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'None found.';
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity,
                                                                        @metricval = N'Unapproved TCP/IP ports found: '
                                                                        + @metricval;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if TCP/IP uses any of these ports: '
                                                                + @severityvalues;

                                                        CLOSE portcursur;
                                                        DEALLOCATE portcursur;
                                                END;
                                                -- Hidden From Browsing
                                                ELSE
                                                IF (@metricid = 28)
                                                BEGIN
                                                        SELECT
                                                                @strval = @hide,
                                                                @severityvalues = N'Y';
                                                        IF (@strval = @severityvalues)
                                                                SELECT
                                                                        @sevcode = @sevcodeok;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity;

                                                        SELECT
                                                                @metricval = dbo.getyesnotext(@strval);
                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if Hide from Browsing is not enabled.';
                                                END;
                                                -- Agent Job Execution
                                                ELSE
                                                IF (@metricid = 29)
                                                BEGIN
                                                        SELECT
                                                                @strval = @agentsysadmin,
                                                                @severityvalues = N'Y';
                                                        IF (@strval = @severityvalues)
                                                                SELECT
                                                                        @sevcode = @sevcodeok;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity;

                                                        SELECT
                                                                @metricval = dbo.getyesnotext(@strval);
                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if SQL Agent CmdExec job execution is not restricted to system administrators.';
                                                END;
                                                -- Replication Enabled
                                                ELSE
                                                IF (@metricid = 30)
                                                BEGIN
                                                        SELECT
                                                                @strval = @replication,
                                                                @severityvalues = N'N';
                                                        IF (@strval = @severityvalues)
                                                                SELECT
                                                                        @sevcode = @sevcodeok;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity;

                                                        SELECT
                                                                @metricval = dbo.getyesnotext(@strval);
                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if replication is enabled.';
                                                END;
                                                -- Unexpected Registry Key Owners
                                                ELSE
                                                IF (@metricid = 31)
                                                BEGIN
                                                        IF (LEN(@severityvalues) > 0)
                                                        BEGIN
                                                                SELECT
                                                                        @sql = N'declare keycursor cursor static for
													select objectname, ownername
														from vwregistrykey
														where snapshotid = '
                                                                        + CONVERT(nvarchar, @snapshotid)
                                                                        + N' and lower(ownername) not in ('
                                                                        + LOWER(@severityvalues)
                                                                        + N')';
                                                                IF (CHARINDEX('%',
                                                                        @severityvalues) > 0)
                                                                BEGIN
                                                                        SELECT
                                                                                @strval = LOWER(@severityvalues),
                                                                                @intval = CHARINDEX(''',''',
                                                                                @strval);
                                                                        WHILE (@intval > 0)
                                                                        BEGIN
                                                                                SELECT
                                                                                        @intval2 = CHARINDEX('%',
                                                                                        @strval);
                                                                                IF (@intval2 < @intval)		-- this item contains a wildcard
                                                                                        SELECT
                                                                                                @sql = @sql
                                                                                                + ' and lower(ownername) not like '
                                                                                                + SUBSTRING(@strval,
                                                                                                1, @intval);
                                                                                SELECT
                                                                                        @strval = SUBSTRING(@strval,
                                                                                        @intval + 2,
                                                                                        LEN(@strval)
                                                                                        - (@intval + 1));
                                                                                SELECT
                                                                                        @intval = CHARINDEX(''',''',
                                                                                        @strval);
                                                                        END;
                                                                        IF (LEN(@strval) > 0)
                                                                        BEGIN
                                                                                SELECT
                                                                                        @intval2 = CHARINDEX('%',
                                                                                        @strval);
                                                                                IF (@intval2 > 0)		-- this item contains a wildcard
                                                                                        SELECT
                                                                                                @sql = @sql
                                                                                                + ' and lower(ownername) not like '
                                                                                                + @strval;
                                                                        END;
                                                                END;
                                                                SELECT
                                                                        @sql = @sql
                                                                        + N' order by objectname, ownername';

                                                                EXEC (@sql);
                                                                OPEN keycursor;

                                                                SELECT
                                                                        @intval = 0,
                                                                        @intval2 = 0;
                                                                FETCH NEXT FROM
                                                                keycursor INTO @strval,
                                                                @strval2;
                                                                WHILE @@fetch_status = 0
                                                                BEGIN
                                                                        SELECT
                                                                                @intval = @intval
                                                                                + 1;
                                                                        IF (@intval2 = 1
                                                                                OR LEN(@metricval)
                                                                                + LEN(@strval)
                                                                                + LEN(@strval2) > 1400
                                                                                )
                                                                        BEGIN
                                                                                IF @intval2 = 0
                                                                                        SELECT
                                                                                                @metricval = @metricval
                                                                                                + N', more...',
                                                                                                @intval2 = 1;
                                                                        END;
                                                                        ELSE
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + CASE
                                                                                                WHEN LEN(@metricval) > 0 THEN N', '
                                                                                                ELSE N''
                                                                                        END + N''''
                                                                                        + @strval
                                                                                        + N''' has owner '''
                                                                                        + @strval2
                                                                                        + '''';

                                                                        IF (@isadmin = 1)
                                                                                INSERT INTO policyassessmentdetail (policyid,
                                                                                assessmentid,
                                                                                metricid,
                                                                                snapshotid,
                                                                                detailfinding,
                                                                                databaseid,
                                                                                objecttype,
                                                                                objectid,
                                                                                objectname)
                                                                                        VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Registry key with unapproved owners found: ''' + @strval + N'''', NULL, -- database ID,
                                                                                        N'RK', -- no registry key type
                                                                                        NULL, @strval);

                                                                        FETCH NEXT FROM
                                                                        keycursor INTO @strval,
                                                                        @strval2;
                                                                END;

                                                                CLOSE keycursor;
                                                                DEALLOCATE keycursor;

                                                                IF (LEN(@metricval) = 0)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = N'No keys found.';
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @severity,
                                                                                @metricval = CONVERT(nvarchar, @intval)
                                                                                + N' keys with unapproved owners found: '
                                                                                + @metricval;
                                                        END;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'No list of approved owners was provided.';

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if registry key owners are not these users: '
                                                                + @severityvalues;
                                                END;
                                                -- Unexpected Registry Key Permissions
                                                ELSE
                                                IF (@metricid = 32)
                                                BEGIN
                                                        IF (LEN(@severityvalues) > 0)
                                                        BEGIN
                                                                SELECT
                                                                        @sql = N'declare filecursor cursor static for
													select objectname, grantee
														from vwregistrykeypermission
														where snapshotid = '
                                                                        + CONVERT(nvarchar, @snapshotid)
                                                                        + N' and accesstype is not null and lower(grantee) not in ('
                                                                        + LOWER(@severityvalues)
                                                                        + N')';
                                                                IF (CHARINDEX('%',
                                                                        @severityvalues) > 0)
                                                                BEGIN
                                                                        SELECT
                                                                                @strval = LOWER(@severityvalues),
                                                                                @intval = CHARINDEX(''',''',
                                                                                @strval);
                                                                        WHILE (@intval > 0)
                                                                        BEGIN
                                                                                SELECT
                                                                                        @intval2 = CHARINDEX('%',
                                                                                        @strval);
                                                                                IF (@intval2 < @intval)		-- this item contains a wildcard
                                                                                        SELECT
                                                                                                @sql = @sql
                                                                                                + ' and lower(grantee) not like '
                                                                                                + SUBSTRING(@strval,
                                                                                                1, @intval);
                                                                                SELECT
                                                                                        @strval = SUBSTRING(@strval,
                                                                                        @intval + 2,
                                                                                        LEN(@strval)
                                                                                        - (@intval + 1));
                                                                                SELECT
                                                                                        @intval = CHARINDEX(''',''',
                                                                                        @strval);
                                                                        END;
                                                                        IF (LEN(@strval) > 0)
                                                                        BEGIN
                                                                                SELECT
                                                                                        @intval2 = CHARINDEX('%',
                                                                                        @strval);
                                                                                IF (@intval2 > 0)		-- this item contains a wildcard
                                                                                        SELECT
                                                                                                @sql = @sql
                                                                                                + ' and lower(grantee) not like '
                                                                                                + @strval;
                                                                        END;
                                                                END;
                                                                SELECT
                                                                        @sql = @sql
                                                                        + N' order by objectname, grantee';

                                                                EXEC (@sql);
                                                                OPEN filecursor;

                                                                SELECT
                                                                        @intval = 0,
                                                                        @intval2 = 0;
                                                                FETCH NEXT FROM
                                                                filecursor INTO @strval,
                                                                @strval2;
                                                                WHILE @@fetch_status = 0
                                                                BEGIN
                                                                        IF (@strval <> @strval3)
                                                                                SELECT
                                                                                        @intval = @intval
                                                                                        + 1;
                                                                        IF (LEN(@metricval)
                                                                                +
                                                                                         CASE
                                                                                                 WHEN (@strval3 = @strval) THEN 13
                                                                                                 ELSE LEN(@strval)
                                                                                         END
                                                                                + LEN(@strval2) > 1400
                                                                                OR @intval2 = 1
                                                                                )
                                                                        BEGIN
                                                                                IF @intval2 = 0
                                                                                        SELECT
                                                                                                @metricval = @metricval
                                                                                                + N', more...',
                                                                                                @intval2 = 1;
                                                                        END;
                                                                        ELSE
                                                                        BEGIN
                                                                                IF (@strval = @strval3)
                                                                                        SELECT
                                                                                                @metricval = @metricval
                                                                                                + N' and grantee '''
                                                                                                + @strval2
                                                                                                + '''';
                                                                                ELSE
                                                                                        SELECT
                                                                                                @metricval = @metricval
                                                                                                + CASE
                                                                                                        WHEN LEN(@metricval) > 0 THEN N', '
                                                                                                        ELSE N''
                                                                                                END + N''''
                                                                                                + @strval
                                                                                                + N''' has grantee '''
                                                                                                + @strval2
                                                                                                + '''';
                                                                        END;

                                                                        IF (@isadmin = 1)
                                                                                INSERT INTO policyassessmentdetail (policyid,
                                                                                assessmentid,
                                                                                metricid,
                                                                                snapshotid,
                                                                                detailfinding,
                                                                                databaseid,
                                                                                objecttype,
                                                                                objectid,
                                                                                objectname)
                                                                                        VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Registry key with unapproved permissions found: ''' + @strval + N''' has grantee ''' + @strval2 + N'''', NULL, -- database ID,
                                                                                        N'RK', -- no registry key object type
                                                                                        NULL, @strval);

                                                                        SELECT
                                                                                @strval3 = @strval;

                                                                        FETCH NEXT FROM
                                                                        filecursor INTO @strval,
                                                                        @strval2;
                                                                END;

                                                                CLOSE filecursor;
                                                                DEALLOCATE filecursor;

                                                                IF (LEN(@metricval) = 0)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = N'No keys found.';
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @severity,
                                                                                @metricval = CONVERT(nvarchar, @intval)
                                                                                + N' keys with unapproved permissions found: '
                                                                                + @metricval;
                                                        END;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'No list of approved users was provided.';

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if registry key permissions are granted to users other than: '
                                                                + @severityvalues;
                                                END;
                                                -- Files on NTFS Drives
                                                ELSE
                                                IF (@metricid = 33)
                                                BEGIN
                                                        SELECT
                                                                @severityvalues = N'NTFS,REFS';
                                                        DECLARE drivecursor CURSOR FOR
                                                        SELECT DISTINCT
                                                                LOWER(LEFT(objectname,
                                                                2)) AS drive,
                                                                disktype
                                                        FROM vwfilesystemobject
                                                        WHERE snapshotid = @snapshotid
                                                        AND UPPER(disktype) not in (select Value from splitbydelimiter(@severityvalues,','));

                                                        DECLARE @disktype nvarchar(16);

                                                        OPEN drivecursor;
                                                        FETCH NEXT FROM
                                                        drivecursor INTO @strval,
                                                        @disktype;
                                                        SELECT
                                                                @intval2 = 0;
                                                        WHILE @@fetch_status = 0
                                                        BEGIN
                                                                IF (@intval2 = 1
                                                                        OR LEN(@metricval)
                                                                        + LEN(@strval) > 1010
                                                                        )
                                                                BEGIN
                                                                        IF @intval2 = 0
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + N', more...',
                                                                                        @intval2 = 1;
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @metricval = @metricval
                                                                                + CASE
                                                                                        WHEN LEN(@metricval) > 0 THEN N', '
                                                                                        ELSE N''
                                                                                END + @strval
                                                                                + N' is '
                                                                                + @disktype;

                                                                IF (@isadmin = 1)
                                                                        INSERT INTO policyassessmentdetail (policyid,
                                                                        assessmentid,
                                                                        metricid,
                                                                        snapshotid,
                                                                        detailfinding,
                                                                        databaseid,
                                                                        objecttype,
                                                                        objectid,
                                                                        objectname)
                                                                                VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Non-NTFS drive found: ' + @strval + N' is ' + @disktype, NULL, -- database ID,
                                                                                N'DR', -- no disk drive object type
                                                                                NULL, @strval);

                                                                FETCH NEXT FROM
                                                                drivecursor INTO @strval,
                                                                @disktype;
                                                        END;

                                                        CLOSE drivecursor;
                                                        DEALLOCATE drivecursor;

                                                        IF (LEN(@metricval) = 0)
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = 'All files are on NTFS drives.';
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if files are found on drives that are not formatted as NTFS.';
                                                END;
                                                -- Unexpected Executable File Owners
                                                ELSE
                                                IF (@metricid = 34)
                                                BEGIN
                                                        IF (LEN(@severityvalues) > 0)
                                                        BEGIN
                                                                SELECT
                                                                        @sql = N'declare filecursor cursor static for
													select distinct lower(objectname) as objectname, ownername
														from vwfilesystemobject
														where snapshotid = '
                                                                        + CONVERT(nvarchar, @snapshotid)
                                                                        + N' and lower(right(objectname,4)) = N''.exe'' and lower(ownername) not in ('
                                                                        + LOWER(@severityvalues)
                                                                        + N')';
                                                                IF (CHARINDEX('%',
                                                                        @severityvalues) > 0)
                                                                BEGIN
                                                                        SELECT
                                                                                @strval = LOWER(@severityvalues),
                                                                                @intval = CHARINDEX(''',''',
                                                                                @strval);
                                                                        WHILE (@intval > 0)
                                                                        BEGIN
                                                                                SELECT
                                                                                        @intval2 = CHARINDEX('%',
                                                                                        @strval);
                                                                                IF (@intval2 < @intval)		-- this item contains a wildcard
                                                                                        SELECT
                                                                                                @sql = @sql
                                                                                                + ' and lower(ownername) not like '
                                                                                                + SUBSTRING(@strval,
                                                                                                1, @intval);
                                                                                SELECT
                                                                                        @strval = SUBSTRING(@strval,
                                                                                        @intval + 2,
                                                                                        LEN(@strval)
                                                                                        - (@intval + 1));
                                                                                SELECT
                                                                                        @intval = CHARINDEX(''',''',
                                                                                        @strval);
                                                                        END;
                                                                        IF (LEN(@strval) > 0)
                                                                        BEGIN
                                                                                SELECT
                                                                                        @intval2 = CHARINDEX('%',
                                                                                        @strval);
                                                                                IF (@intval2 > 0)		-- this item contains a wildcard
                                                                                        SELECT
                                                                                                @sql = @sql
                                                                                                + ' and lower(ownername) not like '
                                                                                                + @strval;
                                                                        END;
                                                                END;
                                                                SELECT
                                                                        @sql = @sql
                                                                        + N' order by objectname, ownername';

                                                                EXEC (@sql);
                                                                OPEN filecursor;

                                                                SELECT
                                                                        @intval = 0,
                                                                        @intval2 = 0;
                                                                FETCH NEXT FROM
                                                                filecursor INTO @strval,
                                                                @strval2;
                                                                WHILE @@fetch_status = 0
                                                                BEGIN
                                                                        SELECT
                                                                                @intval = @intval
                                                                                + 1;
                                                                        IF (LEN(@metricval)
                                                                                + LEN(@strval)
                                                                                + LEN(@strval2) > 1400)
                                                                        BEGIN
                                                                                IF @intval2 = 0
                                                                                        SELECT
                                                                                                @metricval = @metricval
                                                                                                + N', more...',
                                                                                                @intval2 = 1;
                                                                        END;
                                                                        ELSE
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + CASE
                                                                                                WHEN LEN(@metricval) > 0 THEN N', '
                                                                                                ELSE N''
                                                                                        END + N''''
                                                                                        + @strval
                                                                                        + N''' has owner '''
                                                                                        + @strval2
                                                                                        + N'''';

                                                                        IF (@isadmin = 1)
                                                                                INSERT INTO policyassessmentdetail (policyid,
                                                                                assessmentid,
                                                                                metricid,
                                                                                snapshotid,
                                                                                detailfinding,
                                                                                databaseid,
                                                                                objecttype,
                                                                                objectid,
                                                                                objectname)
                                                                                        VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'File with unapproved owner found: ''' + @strval + N''' has owner ''' + @strval2 + N'''', NULL, -- database ID,
                                                                                        N'FI', -- no file object type
                                                                                        NULL, @strval);

                                                                        FETCH NEXT FROM
                                                                        filecursor INTO @strval,
                                                                        @strval2;
                                                                END;

                                                                CLOSE filecursor;
                                                                DEALLOCATE filecursor;

                                                                IF (LEN(@metricval) = 0)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = N'No files found.';
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @severity,
                                                                                @metricval = CONVERT(nvarchar, @intval)
                                                                                + N' files with unapproved owners found: '
                                                                                + @metricval;
                                                        END;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'No list of approved owners was provided.';

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if executable file owners are not these users: '
                                                                + @severityvalues;
                                                END;
                                                -- Unexpected Executable File Permissions
                                                ELSE
                                                IF (@metricid = 35)
                                                BEGIN
                                                        IF (LEN(@severityvalues) > 0)
                                                        BEGIN
                                                                SELECT
                                                                        @sql = N'declare filecursor cursor static for
													select distinct lower(objectname) as objectname, grantee
														from vwfilesystemobjectpermission
														where snapshotid = '
                                                                        + CONVERT(nvarchar, @snapshotid)
                                                                        + N' and lower(right(objectname,4)) = N''.exe'' and accesstype is not null and lower(grantee) not in ('
                                                                        + LOWER(@severityvalues)
                                                                        + N')';
                                                                IF (CHARINDEX('%',
                                                                        @severityvalues) > 0)
                                                                BEGIN
                                                                        SELECT
                                                                                @strval = LOWER(@severityvalues),
                                                                                @intval = CHARINDEX(''',''',
                                                                                @strval);
                                                                        WHILE (@intval > 0)
                                                                        BEGIN
                                                                                SELECT
                                                                                        @intval2 = CHARINDEX('%',
                                                                                        @strval);
                                                                                IF (@intval2 < @intval)		-- this item contains a wildcard
                                                                                        SELECT
                                                                                                @sql = @sql
                                                                                                + ' and lower(grantee) not like '
                                                                                                + SUBSTRING(@strval,
                                                                                                1, @intval);
                                                                                SELECT
                                                                                        @strval = SUBSTRING(@strval,
                                                                                        @intval + 2,
                                                                                        LEN(@strval)
                                                                                        - (@intval + 1));
                                                                                SELECT
                                                                                        @intval = CHARINDEX(''',''',
                                                                                        @strval);
                                                                        END;
                                                                        IF (LEN(@strval) > 0)
                                                                        BEGIN
                                                                                SELECT
                                                                                        @intval2 = CHARINDEX('%',
                                                                                        @strval);
                                                                                IF (@intval2 > 0)		-- this item contains a wildcard
                                                                                        SELECT
                                                                                                @sql = @sql
                                                                                                + ' and lower(grantee) not like '
                                                                                                + @strval;
                                                                        END;
                                                                END;
                                                                SELECT
                                                                        @sql = @sql
                                                                        + N' order by objectname, grantee';

                                                                EXEC (@sql);
                                                                OPEN filecursor;

                                                                SELECT
                                                                        @intval = 0,
                                                                        @intval2 = 0;
                                                                FETCH NEXT FROM
                                                                filecursor INTO @strval,
                                                                @strval2;
                                                                WHILE @@fetch_status = 0
                                                                BEGIN
                                                                        IF (@strval <> @strval3)
                                                                                SELECT
                                                                                        @intval = @intval
                                                                                        + 1;
                                                                        IF (LEN(@metricval)
                                                                                +
                                                                                         CASE
                                                                                                 WHEN (@strval3 = @strval) THEN 13
                                                                                                 ELSE LEN(@strval)
                                                                                         END
                                                                                + LEN(@strval2) > 1400
                                                                                OR @intval2 = 1
                                                                                )
                                                                        BEGIN
                                                                                IF @intval2 = 0
                                                                                        SELECT
                                                                                                @metricval = @metricval
                                                                                                + N', more...',
                                                                                                @intval2 = 1;
                                                                        END;
                                                                        ELSE
                                                                        BEGIN
                                                                                IF (@strval = @strval3)
                                                                                        SELECT
                                                                                                @metricval = @metricval
                                                                                                + N' and grantee '''
                                                                                                + @strval2
                                                                                                + '''';
                                                                                ELSE
                                                                                        SELECT
                                                                                                @metricval = @metricval
                                                                                                + CASE
                                                                                                        WHEN LEN(@metricval) > 0 THEN N', '
                                                                                                        ELSE N''
                                                                                                END + N''''
                                                                                                + @strval
                                                                                                + N''' has grantee '''
                                                                                                + @strval2
                                                                                                + '''';
                                                                        END;

                                                                        IF (@isadmin = 1)
                                                                                INSERT INTO policyassessmentdetail (policyid,
                                                                                assessmentid,
                                                                                metricid,
                                                                                snapshotid,
                                                                                detailfinding,
                                                                                databaseid,
                                                                                objecttype,
                                                                                objectid,
                                                                                objectname)
                                                                                        VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'File with unapproved permission found: ''' + @strval + N''' has grantee ''' + @strval2 + N'''', NULL, -- database ID,
                                                                                        N'FI', -- no file object type
                                                                                        NULL, @strval);

                                                                        SELECT
                                                                                @strval3 = @strval;

                                                                        FETCH NEXT FROM
                                                                        filecursor INTO @strval,
                                                                        @strval2;
                                                                END;

                                                                CLOSE filecursor;
                                                                DEALLOCATE filecursor;

                                                                IF (LEN(@metricval) = 0)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = N'No files found.';
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @severity,
                                                                                @metricval = CONVERT(nvarchar, @intval)
                                                                                + N' files with unapproved permissions found: '
                                                                                + @metricval;
                                                        END;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'No list of approved users was provided.';

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if executable file permissions are granted to users other than: '
                                                                + @severityvalues;
                                                END;
                                                -- Unexpected Database File Owners
                                                ELSE
                                                IF (@metricid = 36)
                                                BEGIN
                                                        IF (LEN(@severityvalues) > 0)
                                                        BEGIN
                                                                SELECT
                                                                        @sql = N'declare filecursor cursor static for
													select distinct lower(objectname) as objectname, ownername
														from vwfilesystemobject
														where snapshotid = '
                                                                        + CONVERT(nvarchar, @snapshotid)
                                                                        + N' and objecttype = N''DB'' and lower(ownername) not in ('
                                                                        + LOWER(@severityvalues)
                                                                        + N')';
                                                                IF (CHARINDEX('%',
                                                                        @severityvalues) > 0)
                                                                BEGIN
                                                                        SELECT
                                                                                @strval = LOWER(@severityvalues),
                                                                                @intval = CHARINDEX(''',''',
                                                                                @strval);
                                                                        WHILE (@intval > 0)
                                                                        BEGIN
                                                                                SELECT
                                                                                        @intval2 = CHARINDEX('%',
                                                                                        @strval);
                                                                                IF (@intval2 < @intval)		-- this item contains a wildcard
                                                                                        SELECT
                                                                                                @sql = @sql
                                                                                                + ' and lower(ownername) not like '
                                                                                                + SUBSTRING(@strval,
                                                                                                1, @intval);
                                                                                SELECT
                                                                                        @strval = SUBSTRING(@strval,
                                                                                        @intval + 2,
                                                                                        LEN(@strval)
                                                                                        - (@intval + 1));
                                                                                SELECT
                                                                                        @intval = CHARINDEX(''',''',
                                                                                        @strval);
                                                                        END;
                                                                        IF (LEN(@strval) > 0)
                                                                        BEGIN
                                                                                SELECT
                                                                                        @intval2 = CHARINDEX('%',
                                                                                        @strval);
                                                                                IF (@intval2 > 0)		-- this item contains a wildcard
                                                                                        SELECT
                                                                                                @sql = @sql
                                                                                                + ' and lower(ownername) not like '
                                                                                                + @strval;
                                                                        END;
                                                                END;
                                                                SELECT
                                                                        @sql = @sql
                                                                        + N' order by objectname, ownername';

                                                                EXEC (@sql);
                                                                OPEN filecursor;

                                                                SELECT
                                                                        @intval = 0,
                                                                        @intval2 = 0;
                                                                FETCH NEXT FROM
                                                                filecursor INTO @strval,
                                                                @strval2;
                                                                WHILE @@fetch_status = 0
                                                                BEGIN
                                                                        SELECT
                                                                                @intval = @intval
                                                                                + 1;
                                                                        IF (LEN(@metricval)
                                                                                + LEN(@strval)
                                                                                + LEN(@strval2) > 1400)
                                                                        BEGIN
                                                                                IF @intval2 = 0
                                                                                        SELECT
                                                                                                @metricval = @metricval
                                                                                                + N', more...',
                                                                                                @intval2 = 1;
                                                                        END;
                                                                        ELSE
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + CASE
                                                                                                WHEN LEN(@metricval) > 0 THEN N', '
                                                                                                ELSE N''
                                                                                        END + N''''
                                                                                        + @strval
                                                                                        + N''' has owner '''
                                                                                        + @strval2
                                                                                        + '''';

                                                                        IF (@isadmin = 1)
                                                                                INSERT INTO policyassessmentdetail (policyid,
                                                                                assessmentid,
                                                                                metricid,
                                                                                snapshotid,
                                                                                detailfinding,
                                                                                databaseid,
                                                                                objecttype,
                                                                                objectid,
                                                                                objectname)
                                                                                        VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'File with unapproved owner found: ''' + @strval + N''' has owner ''' + @strval2 + N'''', NULL, -- database ID,
                                                                                        N'FI', -- no file object type
                                                                                        NULL, @strval);

                                                                        FETCH NEXT FROM
                                                                        filecursor INTO @strval,
                                                                        @strval2;
                                                                END;

                                                                CLOSE filecursor;
                                                                DEALLOCATE filecursor;

                                                                IF (LEN(@metricval) = 0)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = N'No files found.';
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @severity,
                                                                                @metricval = CONVERT(nvarchar, @intval)
                                                                                + N' files with unapproved owners found: '
                                                                                + @metricval;
                                                        END;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'No list of approved owners was provided.';

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if database file owners are not these users: '
                                                                + @severityvalues;
                                                END;
                                                -- Everyone Database File Access
                                                ELSE
                                                IF (@metricid = 37)
                                                BEGIN
                                                        IF NOT EXISTS (SELECT
                                                                        *
                                                                FROM serveroswindowsaccount
                                                                WHERE snapshotid = @snapshotid
                                                                AND sid = @everyonesid)
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'Everyone account not found in file permissions on server.';
                                                        ELSE
                                                        BEGIN
                                                                CREATE TABLE #oslogintbl (
                                                                        sid
                                                                        varbinary(85)
                                                                );

                                                                -- insert everyone user in the table
                                                                INSERT INTO #oslogintbl (sid)
                                                                        VALUES (@everyonesid);

                                                                INSERT INTO #oslogintbl
                                                                EXEC @err = [dbo].[isp_sqlsecure_getwindowsgroupparentsos] @snapshotid = @snapshotid,
                                                                                                                           @inputsid = @everyonesid;

                                                                --check for permissions
                                                                DECLARE filecursor CURSOR STATIC FOR
                                                                SELECT
                                                                        a.objectname,
                                                                        a.grantee AS perm,
                                                                        N'P' AS [type]
                                                                FROM vwfilesystemobjectpermission a
                                                                WHERE a.snapshotid = @snapshotid
                                                                AND a.objecttype = N'DB'
                                                                AND a.accesstype IS NOT NULL
                                                                AND a.sid IN (SELECT
                                                                        [sid]
                                                                FROM #oslogintbl)
                                                                UNION
                                                                --check for file owners of no permissions found
                                                                SELECT
                                                                        a.objectname,
                                                                        a.ownername AS perm,
                                                                        N'O' AS [type]
                                                                FROM vwfilesystemobject a
                                                                WHERE a.snapshotid = @snapshotid
                                                                AND a.objecttype = N'DB'
                                                                AND a.ownersid IN (SELECT
                                                                        [sid]
                                                                FROM #oslogintbl)
                                                                ORDER BY objectname,
                                                                [type],
                                                                perm;

                                                                OPEN filecursor;

                                                                SELECT
                                                                        @intval = 0,
                                                                        @intval2 = 0;
                                                                DECLARE @type char;
                                                                FETCH NEXT FROM
                                                                filecursor INTO @strval,
                                                                @strval2, @type;
                                                                WHILE @@fetch_status = 0
                                                                BEGIN
                                                                        IF (@strval <> @strval3)
                                                                                SELECT
                                                                                        @intval = @intval
                                                                                        + 1;
                                                                        IF (LEN(@metricval)
                                                                                + LEN(@strval)
                                                                                + LEN(@strval2) > 1400
                                                                                OR @intval2 = 1
                                                                                )
                                                                        BEGIN
                                                                                IF @intval2 = 0
                                                                                        SELECT
                                                                                                @metricval = @metricval
                                                                                                + N', more...',
                                                                                                @intval2 = 1;
                                                                        END;
                                                                        ELSE
                                                                        IF (@strval = @strval3)
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + N' and '
                                                                                        + CASE
                                                                                                WHEN @type = 'O' THEN 'owner'
                                                                                                ELSE 'grantee'
                                                                                        END + N' '''
                                                                                        + @strval2
                                                                                        + '''';
                                                                        ELSE
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + CASE
                                                                                                WHEN LEN(@metricval) > 0 THEN N', '
                                                                                                ELSE N''
                                                                                        END + N''''
                                                                                        + @strval
                                                                                        + N''' has '
                                                                                        + CASE
                                                                                                WHEN @type = 'O' THEN 'owner'
                                                                                                ELSE 'grantee'
                                                                                        END + N' '''
                                                                                        + @strval2
                                                                                        + '''';

                                                                        SELECT
                                                                                @strval3 = @strval;

                                                                        IF (@isadmin = 1)
                                                                                INSERT INTO policyassessmentdetail (policyid,
                                                                                assessmentid,
                                                                                metricid,
                                                                                snapshotid,
                                                                                detailfinding,
                                                                                databaseid,
                                                                                objecttype,
                                                                                objectid,
                                                                                objectname)
                                                                                        VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'File with Everyone access found: ''' + @strval + N''' has ' + CASE WHEN @type = 'O' THEN 'owner' ELSE 'grantee' END + N' ''' + @strval2 + N'''', NULL, -- database ID,
                                                                                        N'FI', -- object type
                                                                                        NULL, @strval);

                                                                        FETCH NEXT FROM
                                                                        filecursor INTO @strval,
                                                                        @strval2, @type;
                                                                END;

                                                                CLOSE filecursor;
                                                                DEALLOCATE filecursor;

                                                                IF (LEN(@metricval) = 0)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = N'Everyone does not have access to any database files.';
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @severity,
                                                                                @metricval = CONVERT(nvarchar, @intval)
                                                                                + N' files with Everyone access found: '
                                                                                + @metricval;

                                                                DROP TABLE #oslogintbl;
                                                        END;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if the Everyone Windows group has access to any database files.';
                                                END;
                                                -- Unexpected Database File Permissions
                                                ELSE
                                                IF (@metricid = 38)
                                                BEGIN
                                                        IF (LEN(@severityvalues) > 0)
                                                        BEGIN
                                                                SELECT
                                                                        @sql = N'declare filecursor cursor static for
													select distinct lower(objectname) as objectname, grantee
														from vwfilesystemobjectpermission
														where snapshotid = '
                                                                        + CONVERT(nvarchar, @snapshotid)
                                                                        + N' and objecttype = N''DB'' and accesstype is not null and lower(grantee) not in ('
                                                                        + LOWER(@severityvalues)
                                                                        + N')';
                                                                IF (CHARINDEX('%',
                                                                        @severityvalues) > 0)
                                                                BEGIN
                                                                        SELECT
                                                                                @strval = LOWER(@severityvalues),
                                                                                @intval = CHARINDEX(''',''',
                                                                                @strval);
                                                                        WHILE (@intval > 0)
                                                                        BEGIN
                                                                                SELECT
                                                                                        @intval2 = CHARINDEX('%',
                                                                                        @strval);
                                                                                IF (@intval2 < @intval)		-- this item contains a wildcard
                                                                                        SELECT
                                                                                                @sql = @sql
                                                                                                + ' and lower(grantee) not like '
                                                                                                + SUBSTRING(@strval,
                                                                                                1, @intval);
                                                                                SELECT
                                                                                        @strval = SUBSTRING(@strval,
                                                                                        @intval + 2,
                                                                                        LEN(@strval)
                                                                                        - (@intval + 1));
                                                                                SELECT
                                                                                        @intval = CHARINDEX(''',''',
                                                                                        @strval);
                                                                        END;
                                                                        IF (LEN(@strval) > 0)
                                                                        BEGIN
                                                                                SELECT
                                                                                        @intval2 = CHARINDEX('%',
                                                                                        @strval);
                                                                                IF (@intval2 > 0)		-- this item contains a wildcard
                                                                                        SELECT
                                                                                                @sql = @sql
                                                                                                + ' and lower(grantee) not like '
                                                                                                + @strval;
                                                                        END;
                                                                END;
                                                                SELECT
                                                                        @sql = @sql
                                                                        + N' order by objectname, grantee';

                                                                EXEC (@sql);
                                                                OPEN filecursor;

                                                                SELECT
                                                                        @intval = 0,
                                                                        @intval2 = 0;
                                                                FETCH NEXT FROM
                                                                filecursor INTO @strval,
                                                                @strval2;
                                                                WHILE @@fetch_status = 0
                                                                BEGIN
                                                                        IF (@strval <> @strval3)
                                                                                SELECT
                                                                                        @intval = @intval
                                                                                        + 1;
                                                                        IF (LEN(@metricval)
                                                                                +
                                                                                         CASE
                                                                                                 WHEN (@strval3 = @strval) THEN 13
                                                                                                 ELSE LEN(@strval)
                                                                                         END
                                                                                + LEN(@strval2) > 1400
                                                                                OR @intval2 = 1
                                                                                )
                                                                        BEGIN
                                                                                IF @intval2 = 0
                                                                                        SELECT
                                                                                                @metricval = @metricval
                                                                                                + N', more...',
                                                                                                @intval2 = 1;
                                                                        END;
                                                                        ELSE
                                                                        BEGIN
                                                                                IF (@strval = @strval3)
                                                                                        SELECT
                                                                                                @metricval = @metricval
                                                                                                + N' and grantee '''
                                                                                                + @strval2
                                                                                                + '''';
                                                                                ELSE
                                                                                        SELECT
                                                                                                @metricval = @metricval
                                                                                                + CASE
                                                                                                        WHEN LEN(@metricval) > 0 THEN N', '
                                                                                                        ELSE N''
                                                                                                END + N''''
                                                                                                + @strval
                                                                                                + N''' has grantee '''
                                                                                                + @strval2
                                                                                                + '''';
                                                                        END;

                                                                        IF (@isadmin = 1)
                                                                                INSERT INTO policyassessmentdetail (policyid,
                                                                                assessmentid,
                                                                                metricid,
                                                                                snapshotid,
                                                                                detailfinding,
                                                                                databaseid,
                                                                                objecttype,
                                                                                objectid,
                                                                                objectname)
                                                                                        VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'File with unapproved permission found: ''' + @strval + N''' has grantee ''' + @strval2 + N'''', NULL, -- database ID,
                                                                                        N'FI', -- object type
                                                                                        NULL, @strval);

                                                                        SELECT
                                                                                @strval3 = @strval;

                                                                        FETCH NEXT FROM
                                                                        filecursor INTO @strval,
                                                                        @strval2;
                                                                END;

                                                                CLOSE filecursor;
                                                                DEALLOCATE filecursor;

                                                                IF (LEN(@metricval) = 0)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = N'No files found.';
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @severity,
                                                                                @metricval = CONVERT(nvarchar, @intval)
                                                                                + N' files with unapproved permissions found: '
                                                                                + @metricval;
                                                        END;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'No list of approved users was provided.';

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if database file permissions are granted to users other than: '
                                                                + @severityvalues;
                                                END;
                                                -- Operating System Version
                                                ELSE
                                                IF (@metricid = 39)
                                                BEGIN
                                                        -- changing this to ignore beginning and ending spaces which are strangely included in some of the version strings
                                                        -- they will be trimmed from both the os version and the match list for consistency
                                                        IF (LEN(@severityvalues) > 0)
                                                        BEGIN
                                                                SELECT
                                                                        @intval = 1;
                                                                SELECT
                                                                        @intval2 = CHARINDEX(''',''',
                                                                        @severityvalues);
                                                                WHILE @intval2 > 0
                                                                BEGIN
                                                                        INSERT INTO @tblval
                                                                                VALUES (LTRIM(RTRIM(SUBSTRING(@severityvalues, @intval + 1, @intval2 - @intval - 1))));
                                                                        SELECT
                                                                                @intval = @intval2
                                                                                + 2,
                                                                                @intval2 = CHARINDEX(''',''',
                                                                                @severityvalues,
                                                                                @intval + 1);
                                                                END;
                                                                INSERT INTO @tblval
                                                                        VALUES (LTRIM(RTRIM(SUBSTRING(@severityvalues, @intval + 1, LEN(@severityvalues) - @intval - 1))));
                                                        END;
                                                        SELECT
                                                                @strval = LTRIM(RTRIM(@os));
                                                        IF EXISTS (SELECT
                                                                        1
                                                                FROM @tblval
                                                                WHERE val = @strval)
                                                                SELECT
                                                                        @sevcode = @sevcodeok;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity;
                                                        SELECT
                                                                @metricval = N'Current version is '
                                                                + CASE
                                                                        WHEN @strval IS NULL THEN N'unknown. Check the snapshot status and the activity log for possible causes.'
                                                                        ELSE @os + '.'
                                                                END;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if OS version is not in '
                                                                + @severityvalues
                                                                + '.';
                                                END;
                                                -- SQL Server Service Login Account Acceptable
                                                ELSE
                                                IF (@metricid = 40)
                                                BEGIN
                                                        IF (LEN(@severityvalues) > 0)
                                                        BEGIN
                                                                SELECT
                                                                        @strval = loginname
                                                                FROM vwservice
                                                                WHERE snapshotid = @snapshotid
                                                                AND servicetype = 0;
                                                                IF (LEN(@strval) > 0)
                                                                BEGIN
                                                                        IF (CHARINDEX(''''
                                                                                + @strval + '''',
                                                                                @severityvalues) > 0)
                                                                                SELECT
                                                                                        @sevcode = @sevcodeok;
                                                                        ELSE
                                                                                SELECT
                                                                                        @sevcode = @severity;
                                                                        SELECT
                                                                                @metricval = N'Current login account is '
                                                                                + @strval + '.';
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = N'No login account or service not found.';
                                                        END;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'No approved login account was provided.';

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if SQL Server Service Login Account is a user other than: '
                                                                + @severityvalues;
                                                END;
                                                -- Reporting Services Enabled
                                                ELSE
                                                IF (@metricid = 41)
                                                BEGIN
                                                        SELECT
                                                                @severityvalues = N'Running';
                                                        SELECT
                                                                @strval = [state]
                                                        FROM vwservice
                                                        WHERE snapshotid = @snapshotid
                                                        AND servicetype = 6;
                                                        IF (LEN(@strval) > 0)
                                                        BEGIN
                                                                IF (@strval <> @severityvalues)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok;
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @severity;
                                                                SELECT
                                                                        @metricval = N'Current state is '
                                                                        + @strval + '.';
                                                        END;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'Service not found or state undetermined.';

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if Reporting Services are '
                                                                + @severityvalues
                                                                + '.';
                                                END;
                                                -- Analysis Services Enabled
                                                ELSE
                                                IF (@metricid = 42)
                                                BEGIN
                                                        SELECT
                                                                @severityvalues = N'Running';
                                                        SELECT
                                                                @strval = [state]
                                                        FROM vwservice
                                                        WHERE snapshotid = @snapshotid
                                                        AND servicetype = 3;
                                                        IF (LEN(@strval) > 0)
                                                        BEGIN
                                                                IF (@strval <> @severityvalues)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok;
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @severity;
                                                                SELECT
                                                                        @metricval = N'Current state is '
                                                                        + @strval + '.';
                                                        END;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'Service not found or state undetermined.';

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if Analysis Services are '
                                                                + @severityvalues
                                                                + '.';
                                                END;
                                                -- Analysis Services Login Account Acceptable
                                                ELSE
                                                IF (@metricid = 43)
                                                BEGIN
                                                        IF (LEN(@severityvalues) > 0)
                                                        BEGIN
                                                                SELECT
                                                                        @strval = loginname
                                                                FROM vwservice
                                                                WHERE snapshotid = @snapshotid
                                                                AND servicetype = 3;
                                                                IF (LEN(@strval) > 0)
                                                                BEGIN
                                                                        IF (CHARINDEX(''''
                                                                                + @strval + '''',
                                                                                @severityvalues) > 0)
                                                                                SELECT
                                                                                        @sevcode = @sevcodeok;
                                                                        ELSE
                                                                                SELECT
                                                                                        @sevcode = @severity;
                                                                        SELECT
                                                                                @metricval = N'Current login account is '
                                                                                + @strval + '.';
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = N'No login account or service not found.';
                                                        END;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'No approved login account was provided.';

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if Analysis Services Service Login Account is a user other than: '
                                                                + @severityvalues;
                                                END;
                                                -- Notification Services Enabled
                                                ELSE
                                                IF (@metricid = 44)
                                                BEGIN
                                                        SELECT
                                                                @severityvalues = N'Running';
                                                        SELECT
                                                                @strval = [state]
                                                        FROM vwservice
                                                        WHERE snapshotid = @snapshotid
                                                        AND servicetype IN (
                                                        9, 11);
                                                        IF (LEN(@strval) > 0)
                                                        BEGIN
                                                                IF (@strval <> @severityvalues)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok;
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @severity;
                                                                SELECT
                                                                        @metricval = N'Current state is '
                                                                        + @strval + '.';
                                                        END;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'Service not found or state undetermined.';

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if Notification Services are '
                                                                + @severityvalues
                                                                + '.';
                                                END;
                                                -- Notification Services Login Account Acceptable
                                                ELSE
                                                IF (@metricid = 45)
                                                BEGIN
                                                        IF (LEN(@severityvalues) > 0)
                                                        BEGIN
                                                                SELECT
                                                                        @strval = loginname
                                                                FROM vwservice
                                                                WHERE snapshotid = @snapshotid
                                                                AND servicetype IN (
                                                                9, 11);
                                                                IF (LEN(@strval) > 0)
                                                                BEGIN
                                                                        IF (CHARINDEX(''''
                                                                                + @strval + '''',
                                                                                @severityvalues) > 0)
                                                                                SELECT
                                                                                        @sevcode = @sevcodeok;
                                                                        ELSE
                                                                                SELECT
                                                                                        @sevcode = @severity;
                                                                        SELECT
                                                                                @metricval = N'Current login account is '
                                                                                + @strval + '.';
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = N'No login account or service not found.';
                                                        END;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'No approved login account was provided.';

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if Notification Services Service Login Account is a user other than: '
                                                                + @severityvalues;
                                                END;
                                                -- Integration Services Enabled
                                                ELSE
                                                IF (@metricid = 46)
                                                BEGIN
                                                        SELECT
                                                                @severityvalues = N'Running';
                                                        SELECT
                                                                @strval = [state]
                                                        FROM vwservice
                                                        WHERE snapshotid = @snapshotid
                                                        AND servicetype IN (
                                                        5, 12, 15);
                                                        IF (LEN(@strval) > 0)
                                                        BEGIN
                                                                IF (@strval <> @severityvalues)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok;
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @severity;
                                                                SELECT
                                                                        @metricval = N'Current state is '
                                                                        + @strval + '.';
                                                        END;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'Service not found or state undetermined.';

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if Integration Services are '
                                                                + @severityvalues
                                                                + '.';
                                                END;
                                                -- Integration Services Login Account Acceptable
                                                ELSE
                                                IF (@metricid = 47)
                                                BEGIN
                                                        IF (LEN(@severityvalues) > 0)
                                                        BEGIN
                                                                SELECT
                                                                        @strval = loginname
                                                                FROM vwservice
                                                                WHERE snapshotid = @snapshotid
                                                                AND servicetype IN (
                                                                5, 12, 15);
                                                                IF (LEN(@strval) > 0)
                                                                BEGIN
                                                                        IF (CHARINDEX(''''
                                                                                + @strval + '''',
                                                                                @severityvalues) > 0)
                                                                                SELECT
                                                                                        @sevcode = @sevcodeok;
                                                                        ELSE
                                                                                SELECT
                                                                                        @sevcode = @severity;
                                                                        SELECT
                                                                                @metricval = N'Current login account is '
                                                                                + @strval + '.';
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = N'No login account or service not found.';
                                                        END;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'No approved login account was provided.';

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if Integration Services Service Login Account is a user other than: '
                                                                + @severityvalues;
                                                END;
                                                -- SQL Server Agent Enabled
                                                ELSE
                                                IF (@metricid = 48)
                                                BEGIN
                                                        SELECT
                                                                @severityvalues = N'Running';
                                                        SELECT
                                                                @strval = [state]
                                                        FROM vwservice
                                                        WHERE snapshotid = @snapshotid
                                                        AND servicetype = 1;
                                                        IF (LEN(@strval) > 0)
                                                        BEGIN
                                                                IF (@strval <> @severityvalues)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok;
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @severity;
                                                                SELECT
                                                                        @metricval = N'Current state is '
                                                                        + @strval + '.';
                                                        END;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'Service not found or state undetermined.';

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if SQL Server Agent is '
                                                                + @severityvalues
                                                                + '.';
                                                END;
                                                -- SQL Server Agent Login Account Acceptable
                                                ELSE
                                                IF (@metricid = 49)
                                                BEGIN
                                                        IF (LEN(@severityvalues) > 0)
                                                        BEGIN
                                                                SELECT
                                                                        @strval = loginname
                                                                FROM vwservice
                                                                WHERE snapshotid = @snapshotid
                                                                AND servicetype = 1;
                                                                IF (LEN(@strval) > 0)
                                                                BEGIN
                                                                        IF (CHARINDEX(''''
                                                                                + @strval + '''',
                                                                                @severityvalues) > 0)
                                                                                SELECT
                                                                                        @sevcode = @sevcodeok;
                                                                        ELSE
                                                                                SELECT
                                                                                        @sevcode = @severity;
                                                                        SELECT
                                                                                @metricval = N'Current login account is '
                                                                                + @strval + '.';
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = N'No login account or service not found.';
                                                        END;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'No approved login account was provided.';

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if SQL Server Agent Service Login Account is a user other than: '
                                                                + @severityvalues;
                                                END;
                                                -- Full-Text Search Enabled
                                                ELSE
                                                IF (@metricid = 50)
                                                BEGIN
                                                        SELECT
                                                                @severityvalues = N'Running';
                                                        SELECT
                                                                @strval = [state]
                                                        FROM vwservice
                                                        WHERE snapshotid = @snapshotid
                                                        AND servicetype IN (
                                                        2, 10, 14);
                                                        IF (LEN(@strval) > 0)
                                                        BEGIN
                                                                IF (@strval <> @severityvalues)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok;
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @severity;
                                                                SELECT
                                                                        @metricval = N'Current state is '
                                                                        + @strval + '.';
                                                        END;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'Service not found or state undetermined.';

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if Full-Text Search is '
                                                                + @severityvalues
                                                                + '.';
                                                END;
                                                -- Full-Text Search Login Account Acceptable
                                                ELSE
                                                IF (@metricid = 51)
                                                BEGIN
                                                        IF (LEN(@severityvalues) > 0)
                                                        BEGIN
                                                                -- process all FT services because collector can collect multiples in a snapshot if 2000 and 2005 or later are installed together
                                                                SELECT
                                                                        @sql = N'declare svclogincursor cursor for
													select displayname, loginname 
														from vwservice 
														where snapshotid = '
                                                                        + CONVERT(nvarchar, @snapshotid)
                                                                        + N'
															and servicetype in (2,10,14)
															and lower(loginname) not in ('
                                                                        + LOWER(@severityvalues)
                                                                        + N')';

                                                                EXEC (@sql);

                                                                OPEN svclogincursor;
                                                                FETCH NEXT FROM
                                                                svclogincursor INTO @strval,
                                                                @strval2;

                                                                SELECT
                                                                        @intval2 = 0;
                                                                WHILE @@fetch_status = 0
                                                                BEGIN
                                                                        IF (@intval2 = 1
                                                                                OR LEN(@metricval)
                                                                                + LEN(@strval) > 1010
                                                                                )
                                                                        BEGIN
                                                                                IF @intval2 = 0
                                                                                        SELECT
                                                                                                @metricval = @metricval
                                                                                                + N', more...',
                                                                                                @intval2 = 1;
                                                                        END;
                                                                        ELSE
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + CASE
                                                                                                WHEN LEN(@metricval) > 0 THEN N', '
                                                                                                ELSE N''
                                                                                        END + N''''
                                                                                        + @strval2
                                                                                        + N''' for '''
                                                                                        + @strval
                                                                                        + N'''';

                                                                        IF (@isadmin = 1)
                                                                                INSERT INTO policyassessmentdetail (policyid,
                                                                                assessmentid,
                                                                                metricid,
                                                                                snapshotid,
                                                                                detailfinding,
                                                                                databaseid,
                                                                                objecttype,
                                                                                objectid,
                                                                                objectname)
                                                                                        VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Login Account is ''' + @strval2 + N''' for ''' + @strval + N'''', NULL, -- database ID,
                                                                                        N'iLOGN', -- object type
                                                                                        NULL, @strval);

                                                                        FETCH NEXT FROM
                                                                        svclogincursor INTO @strval,
                                                                        @strval2;
                                                                END;

                                                                CLOSE svclogincursor;
                                                                DEALLOCATE svclogincursor;

                                                                IF (LEN(@metricval) = 0)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = 'The service login account is acceptable or the service is not installed.';
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @severity,
                                                                                @metricval = N'Current login account is '
                                                                                + @metricval
                                                                                + '.';
                                                        END;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'No approved login account was provided.';

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if Full-Text Search Service Login Account is a user other than: '
                                                                + @severityvalues;
                                                END;
                                                -- SQL Server Browser Enabled
                                                ELSE
                                                IF (@metricid = 52)
                                                BEGIN
                                                        SELECT
                                                                @severityvalues = N'Running';
                                                        SELECT
                                                                @strval = [state]
                                                        FROM vwservice
                                                        WHERE snapshotid = @snapshotid
                                                        AND servicetype = 4;
                                                        IF (LEN(@strval) > 0)
                                                        BEGIN
                                                                IF (@strval <> @severityvalues)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok;
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @severity;
                                                                SELECT
                                                                        @metricval = N'Current state is '
                                                                        + @strval + '.';
                                                        END;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'Service not found or state undetermined.';

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if SQL Server Browser is '
                                                                + @severityvalues
                                                                + '.';
                                                END;
                                                -- SQL Server Browser Login Account Acceptable
                                                ELSE
                                                IF (@metricid = 53)
                                                BEGIN
                                                        IF (LEN(@severityvalues) > 0)
                                                        BEGIN
                                                                SELECT
                                                                        @strval = loginname
                                                                FROM vwservice
                                                                WHERE snapshotid = @snapshotid
                                                                AND servicetype = 4;
                                                                IF (LEN(@strval) > 0)
                                                                BEGIN
                                                                        IF (CHARINDEX(''''
                                                                                + @strval + '''',
                                                                                @severityvalues) > 0)
                                                                                SELECT
                                                                                        @sevcode = @sevcodeok;
                                                                        ELSE
                                                                                SELECT
                                                                                        @sevcode = @severity;
                                                                        SELECT
                                                                                @metricval = N'Current login account is '
                                                                                + @strval + '.';
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = N'No login account or service not found.';
                                                        END;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'No approved login account was provided.';

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if SQL Server Browser Service Login Account is a user other than: '
                                                                + @severityvalues;
                                                END;
                                                -- Audited Servers
                                                ELSE
                                                IF (@metricid = 54)
                                                BEGIN
                                                        -- any server found is valid. See post server loop processing for findings.
                                                        SELECT
                                                                @sevcode = @sevcodeok;
                                                        SELECT
                                                                @metricval = N'Server has audit data.';

                                                        SELECT
                                                                @metricthreshold = N'Assessment may not be valid if all servers do not have audit data.';
                                                END;
                                                -- Complete Audits
                                                ELSE
                                                IF (@metricid = 55)
                                                BEGIN
                                                        SELECT
                                                                @strval = @status,
                                                                @severityvalues = N'S';
                                                        IF (@strval = @severityvalues)
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'Snapshot was successful';
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity,
                                                                        @metricval = N'Snapshot has warnings';

                                                        -- Make sure snapshot is a 2.0 snapshot or issue finding
                                                        IF (@collectorversion IS NOT NULL
                                                                AND @collectorversion >= '2.'
                                                                )
                                                                -- don't set sevcode to ok because it may already be a finding for warnings
                                                                SELECT
                                                                        @metricval = @metricval
                                                                        + N'.';
                                                        ELSE
                                                        BEGIN
                                                                SELECT
                                                                        @sevcode = @severity,
                                                                        @metricval = @metricval
                                                                        + CASE
                                                                                WHEN RIGHT(@metricval,
                                                                                        10) = 'successful' THEN N', but'
                                                                                ELSE N' and'
                                                                        END
                                                                        + N' was created using SQLsecure version 1.2 or earlier.';
                                                        END;

                                                        --check to see if weak password detection was enabled during this snapshot
                                                        IF (@weakpasswordenabled != 'Y')
                                                        BEGIN
                                                                SELECT
                                                                        @sevcode = @severity,
                                                                        @metricval = @metricval
                                                                        + N'   Password Health may have been omitted because Weak password detection was disabled during data collection.';
                                                        END;


                                                        -- Check that filters do not exclude anything
                                                        DECLARE @vermatch nvarchar(256),
                                                                @currule nvarchar(256),
                                                                @rule nvarchar(256),
                                                                @classname nvarchar(128),
                                                                @scope nvarchar(64);

                                                        DECLARE filtercursor CURSOR FOR
                                                        SELECT
                                                                rulename,
                                                                class,
                                                                classname,
                                                                scope,
                                                                matchstring
                                                        FROM SQLsecure.dbo.vwsnapshotfilterrules
                                                        WHERE snapshotid = @snapshotid
                                                        ORDER BY rulename,
                                                        classname;

                                                        OPEN filtercursor;
                                                        FETCH NEXT FROM
                                                        filtercursor INTO @rule,
                                                        @intval,
                                                        @classname,
                                                        @scope, @strval;

                                                        SELECT
                                                                @currule = N'';
                                                        WHILE @@fetch_status = 0
                                                        BEGIN
                                                                IF (@intval <> 43)	-- skip checking extended stored procedures because they are system only and have no match strings
                                                                        IF (@scope <> N'A'
                                                                                OR LEN(@strval) > 0
                                                                                )	-- if not processing all or there is a match string then it is a finding
                                                                                SELECT
                                                                                        @sevcode = @severity;

                                                                -- add all filter rules to the output regardless of finding
                                                                SELECT
                                                                        @scope =
                                                                                        CASE @scope
                                                                                                WHEN N'A' THEN N'All'
                                                                                                WHEN N'S' THEN N'System'
                                                                                                ELSE N'User'
                                                                                        END;
                                                                SELECT
                                                                        @strval =
                                                                                         CASE
                                                                                                 WHEN @rule = @currule THEN N', '
                                                                                                 ELSE N'  Filter:'
                                                                                                         + @rule + N': '
                                                                                         END + @scope
                                                                        + N' '
                                                                        + @classname
                                                                        + CASE
                                                                                WHEN LEN(@strval) = 0 THEN N''
                                                                                ELSE N' matching '''
                                                                                        + @strval + ''''
                                                                        END;
                                                                IF (@intval2 = 1
                                                                        OR LEN(@metricval)
                                                                        + LEN(@strval) > 1010
                                                                        )
                                                                BEGIN
                                                                        IF @intval2 = 0
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + N', more...',
                                                                                        @intval2 = 1;
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @metricval = @metricval
                                                                                + @strval;

                                                                IF (@isadmin = 1)
                                                                        INSERT INTO policyassessmentdetail (policyid,
                                                                        assessmentid,
                                                                        metricid,
                                                                        snapshotid,
                                                                        detailfinding,
                                                                        databaseid,
                                                                        objecttype,
                                                                        objectid,
                                                                        objectname)
                                                                                VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Filter found: ' + @rule + N': ' + @scope + N' ' + @classname + CASE WHEN LEN(@strval) = 0 THEN N'' ELSE N' matching ''' + @strval END + N'''', NULL, -- database ID,
                                                                                N'RL', -- no filter rule object type
                                                                                NULL, @rule);

                                                                SELECT
                                                                        @currule = @rule;

                                                                FETCH NEXT FROM
                                                                filtercursor INTO @rule,
                                                                @intval,
                                                                @classname,
                                                                @scope, @strval;
                                                        END;

                                                        CLOSE filtercursor;
                                                        DEALLOCATE filtercursor;

                                                        -- if everything else is ok, then check and make sure all objects were included
                                                        IF (@sevcode < @severity)
                                                        BEGIN
                                                                SELECT
                                                                        @intval = COUNT(*)
                                                                FROM filterruleclass a
                                                                LEFT JOIN vwsnapshotfilterrules b
                                                                        ON (a.objectclass = b.class
                                                                        AND b.snapshotid = @snapshotid
                                                                        )
                                                                WHERE b.snapshotid IS NULL;
                                                                IF (@version > N'9.'
                                                                        OR @version < N'6.'
                                                                        )
                                                                BEGIN
                                                                        IF (@intval <> 6)
                                                                                SELECT
                                                                                        @sevcode = @severity,
                                                                                        @metricval = @metricval
                                                                                        + N'  Some objects may have been omitted by filtering during data collection.';
                                                                END;
                                                                ELSE
                                                                BEGIN
                                                                        IF (@intval <> 13)
                                                                                SELECT
                                                                                        @sevcode = @severity,
                                                                                        @metricval = @metricval
                                                                                        + N'  Some objects may have been omitted by filtering during data collection.';
                                                                END;
                                                        END;

                                                        SELECT
                                                                @metricthreshold = N'Server may be vulnerable if Snapshot status is not '
                                                                + dbo.getsnapshotstatus(@severityvalues)
                                                                + ' or data collection filters are excluding data.';
                                                END;
                                                -- Baseline Data
                                                ELSE
                                                IF (@metricid = 56)
                                                BEGIN
                                                        SELECT
                                                                @strval = ISNULL(@baseline,
                                                                N''),
                                                                @severityvalues = N'Y';
                                                        IF (@strval = @severityvalues)
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'Snapshot is marked as baseline.';
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity,
                                                                        @metricval = N'Snapshot is not marked as baseline.'
                                                                        + @strval;

                                                        SELECT
                                                                @metricthreshold = N'Audit data may not be valid if snapshot is not marked as baseline.';
                                                END;
                                                -- Password Policy Enabled
                                                ELSE
                                                IF (@metricid = 57)
                                                BEGIN
                                                        SELECT
                                                                @severityvalues = N'N';
                                                        DECLARE logincursor CURSOR FOR
                                                        SELECT
                                                                name
                                                        FROM vwserverprincipal
                                                        WHERE snapshotid = @snapshotid
                                                        AND type = N'S'
                                                        AND ispolicychecked = @severityvalues;

                                                        OPEN logincursor;
                                                        FETCH NEXT FROM
                                                        logincursor INTO @strval;

                                                        SELECT
                                                                @intval2 = 0;
                                                        WHILE @@fetch_status = 0
                                                        BEGIN
                                                                IF (@intval2 = 1
                                                                        OR LEN(@metricval)
                                                                        + LEN(@strval) > 1010
                                                                        )
                                                                BEGIN
                                                                        IF @intval2 = 0
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + N', more...',
                                                                                        @intval2 = 1;
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @metricval = @metricval
                                                                                + CASE
                                                                                        WHEN LEN(@metricval) > 0 THEN N', '
                                                                                        ELSE N''
                                                                                END + N''''
                                                                                + @strval
                                                                                + N'''';

                                                                IF (@isadmin = 1)
                                                                        INSERT INTO policyassessmentdetail (policyid,
                                                                        assessmentid,
                                                                        metricid,
                                                                        snapshotid,
                                                                        detailfinding,
                                                                        databaseid,
                                                                        objecttype,
                                                                        objectid,
                                                                        objectname)
                                                                                VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Password policy not enforced on ''' + @strval + N'''', NULL, -- database ID,
                                                                                N'iLOGN', -- object type
                                                                                NULL, @strval);

                                                                FETCH NEXT FROM
                                                                logincursor INTO @strval;
                                                        END;

                                                        CLOSE logincursor;
                                                        DEALLOCATE logincursor;

                                                        IF (LEN(@metricval) = 0)
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = 'All SQL Logins have password policy enforced.';
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity,
                                                                        @metricval = N'Password policy not enforced on '
                                                                        + @metricval;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if password policy is not enforced on all SQL Logins.';
                                                END;
                                                -- 'Public database role permissions
                                                ELSE
                                                IF (@metricid = 58)
                                                BEGIN
                                                        SELECT
                                                                @severityvalues = N'Y';
                                                        DECLARE databasecursor CURSOR FOR
                                                        SELECT DISTINCT
                                                                b.databasename
                                                        FROM databaseobjectpermission a,
                                                             sqldatabase b
                                                        WHERE a.snapshotid = @snapshotid
                                                        AND a.grantee = 0			--public uid is 0
                                                        AND a.snapshotid = b.snapshotid
                                                        AND a.dbid = b.dbid
                                                        UNION
                                                        SELECT DISTINCT
                                                                b.databasename
                                                        FROM databaserolemember a,
                                                             sqldatabase b
                                                        WHERE a.snapshotid = @snapshotid
                                                        AND a.rolememberuid = 0		--public uid is 0
                                                        AND a.snapshotid = b.snapshotid
                                                        AND a.dbid = b.dbid;
                                                        OPEN databasecursor;
                                                        FETCH NEXT FROM
                                                        databasecursor INTO @strval;

                                                        SELECT
                                                                @intval2 = 0;
                                                        WHILE @@fetch_status = 0
                                                        BEGIN
                                                                IF (@intval2 = 1
                                                                        OR LEN(@metricval)
                                                                        + LEN(@strval) > 1010
                                                                        )
                                                                BEGIN
                                                                        IF @intval2 = 0
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + N', more...',
                                                                                        @intval2 = 1;
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @metricval = @metricval
                                                                                + CASE
                                                                                        WHEN LEN(@metricval) > 0 THEN N', '
                                                                                        ELSE N''
                                                                                END + N''''
                                                                                + @strval
                                                                                + N'''';

                                                                IF (@isadmin = 1)
                                                                        INSERT INTO policyassessmentdetail (policyid,
                                                                        assessmentid,
                                                                        metricid,
                                                                        snapshotid,
                                                                        detailfinding,
                                                                        databaseid,
                                                                        objecttype,
                                                                        objectid,
                                                                        objectname)
                                                                                VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Public has permissions on ''' + @strval + N'''', NULL, -- database ID,
                                                                                N'DB', -- object type
                                                                                NULL, @strval);

                                                                FETCH NEXT FROM
                                                                databasecursor INTO @strval;
                                                        END;

                                                        CLOSE databasecursor;
                                                        DEALLOCATE databasecursor;

                                                        IF (LEN(@metricval) = 0)
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = 'No databases found with public permissions.';
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity,
                                                                        @metricval = N'Public has permissions on '
                                                                        + @metricval;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if the public role has been granted any permissions or is a role member.';
                                                END;
                                                -- 'Blank Passwords
                                                ELSE
                                                IF (@metricid = 59)
                                                BEGIN
                                                        SELECT
                                                                @severityvalues = N'Y';
                                                        DECLARE logincursor CURSOR FOR
                                                        SELECT
                                                                name
                                                        FROM vwserverprincipal
                                                        WHERE snapshotid = @snapshotid
                                                        AND type = N'S'
                                                        AND ispasswordnull = @severityvalues;

                                                        OPEN logincursor;
                                                        FETCH NEXT FROM
                                                        logincursor INTO @strval;

                                                        SELECT
                                                                @intval2 = 0;
                                                        WHILE @@fetch_status = 0
                                                        BEGIN
                                                                IF (@intval2 = 1
                                                                        OR LEN(@metricval)
                                                                        + LEN(@strval) > 1010
                                                                        )
                                                                BEGIN
                                                                        IF @intval2 = 0
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + N', more...',
                                                                                        @intval2 = 1;
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @metricval = @metricval
                                                                                + CASE
                                                                                        WHEN LEN(@metricval) > 0 THEN N', '
                                                                                        ELSE N''
                                                                                END + N''''
                                                                                + @strval
                                                                                + N'''';

                                                                IF (@isadmin = 1)
                                                                        INSERT INTO policyassessmentdetail (policyid,
                                                                        assessmentid,
                                                                        metricid,
                                                                        snapshotid,
                                                                        detailfinding,
                                                                        databaseid,
                                                                        objecttype,
                                                                        objectid,
                                                                        objectname)
                                                                                VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Login with blank password found: ''' + @strval + N'''', NULL, -- database ID,
                                                                                N'iLOGN', -- object type
                                                                                NULL, @strval);

                                                                FETCH NEXT FROM
                                                                logincursor INTO @strval;
                                                        END;

                                                        CLOSE logincursor;
                                                        DEALLOCATE logincursor;

                                                        IF (LEN(@metricval) = 0)
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'No SQL Logins have blank passwords.';
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity,
                                                                        @metricval = N'These SQL Logins have blank passwords: '
                                                                        + @metricval;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if SQL Logins are found that have a blank password.';
                                                END;
                                                -- 'Fixed roles assigned to public or guest
                                                ELSE
                                                IF (@metricid = 60)
                                                BEGIN
                                                        SELECT
                                                                @severityvalues = N'Y';
                                                        DECLARE databasecursor CURSOR FOR
                                                        SELECT DISTINCT
                                                                b.databasename
                                                        FROM databaserolemember a,
                                                             sqldatabase b
                                                        WHERE a.snapshotid = @snapshotid
                                                        AND a.rolememberuid IN (
                                                        0, 2)		--public uid is 0 and guest uid is 2
                                                        AND a.groupuid > 16383		--database fixed roles are 16384-16393
                                                        AND a.groupuid < 16394
                                                        AND a.snapshotid = b.snapshotid
                                                        AND a.dbid = b.dbid;
                                                        OPEN databasecursor;
                                                        FETCH NEXT FROM
                                                        databasecursor INTO @strval;
                                                        SELECT
                                                                @intval2 = 0;
                                                        WHILE @@fetch_status = 0
                                                        BEGIN
                                                                IF (@intval2 = 1
                                                                        OR LEN(@metricval)
                                                                        + LEN(@strval) > 1010
                                                                        )
                                                                BEGIN
                                                                        IF @intval2 = 0
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + N', more...',
                                                                                        @intval2 = 1;
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @metricval = @metricval
                                                                                + CASE
                                                                                        WHEN LEN(@metricval) > 0 THEN N', '
                                                                                        ELSE N''
                                                                                END + N''''
                                                                                + @strval
                                                                                + N'''';

                                                                IF (@isadmin = 1)
                                                                        INSERT INTO policyassessmentdetail (policyid,
                                                                        assessmentid,
                                                                        metricid,
                                                                        snapshotid,
                                                                        detailfinding,
                                                                        databaseid,
                                                                        objecttype,
                                                                        objectid,
                                                                        objectname)
                                                                                VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Public or guest is a member of a fixed role on ''' + @strval + N'''', NULL, -- database ID,
                                                                                N'DB', -- object type
                                                                                NULL, @strval);

                                                                FETCH NEXT FROM
                                                                databasecursor INTO @strval;
                                                        END;

                                                        CLOSE databasecursor;
                                                        DEALLOCATE databasecursor;

                                                        IF (LEN(@metricval) = 0)
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = 'No databases found with public or guest assigned to fixed database roles.';
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity,
                                                                        @metricval = N'Public or guest is a member of a fixed role on '
                                                                        + @metricval;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if the public role or the guest user is a member of any fixed database role.';
                                                END;
                                                -- Builtin/administrators
                                                ELSE
                                                IF (@metricid = 61)
                                                BEGIN
                                                        SELECT
                                                                @severityvalues = N'Y';
                                                        SELECT
                                                                @intval = 1
                                                        FROM serverrolemember a,
                                                             serverprincipal b,		-- Builtin\admin principal
                                                             serverprincipal c		-- sysadmin principal
                                                        WHERE a.snapshotid = @snapshotid
                                                        AND a.snapshotid = b.snapshotid
                                                        AND a.memberprincipalid = b.principalid
                                                        AND b.sid = @builtinadminsid
                                                        AND a.snapshotid = c.snapshotid
                                                        AND a.principalid = c.principalid
                                                        AND c.sid = @sysadminsid;

                                                        IF (@intval = 0)
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @strval = N'N';
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity,
                                                                        @strval = N'Y';

                                                        SELECT
                                                                @metricval = dbo.getyesnotext(@strval);
                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if BUILTIN\Administrators is a member of the sysadmin server role.';
                                                END;
                                                --************************************************* version 2.5 security checks
                                                -- Password Expiration Enabled
                                                ELSE
                                                IF (@metricid = 62)
                                                BEGIN
                                                        SELECT
                                                                @severityvalues = N'N';
                                                        DECLARE logincursor CURSOR FOR
                                                        SELECT
                                                                name
                                                        FROM vwserverprincipal
                                                        WHERE snapshotid = @snapshotid
                                                        AND type = N'S'
                                                        AND isexpirationchecked = @severityvalues;

                                                        OPEN logincursor;
                                                        FETCH NEXT FROM
                                                        logincursor INTO @strval;

                                                        SELECT
                                                                @intval2 = 0;
                                                        WHILE @@fetch_status = 0
                                                        BEGIN
                                                                IF (@intval2 = 1
                                                                        OR LEN(@metricval)
                                                                        + LEN(@strval) > 1010
                                                                        )
                                                                BEGIN
                                                                        IF @intval2 = 0
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + N', more...',
                                                                                        @intval2 = 1;
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @metricval = @metricval
                                                                                + CASE
                                                                                        WHEN LEN(@metricval) > 0 THEN N', '
                                                                                        ELSE N''
                                                                                END + N''''
                                                                                + @strval
                                                                                + N'''';

                                                                IF (@isadmin = 1)
                                                                        INSERT INTO policyassessmentdetail (policyid,
                                                                        assessmentid,
                                                                        metricid,
                                                                        snapshotid,
                                                                        detailfinding,
                                                                        databaseid,
                                                                        objecttype,
                                                                        objectid,
                                                                        objectname)
                                                                                VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Password expiration not enabled for ''' + @strval + N'''', NULL, -- database ID,
                                                                                N'iLOGN', -- object type
                                                                                NULL, @strval);

                                                                FETCH NEXT FROM
                                                                logincursor INTO @strval;
                                                        END;

                                                        CLOSE logincursor;
                                                        DEALLOCATE logincursor;

                                                        IF (LEN(@metricval) = 0)
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = 'All SQL Logins have password expiration enabled.';
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity,
                                                                        @metricval = N'Password expiration not enabled for '
                                                                        + @metricval;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if SQL Logins are found that do not implement password expiration.';
                                                END;

                                                -- Server is Domain Controller
                                                ELSE
                                                IF (@metricid = 63)
                                                BEGIN
                                                        SELECT
                                                                @severityvalues = N'Y';
                                                        IF (@dc <> @severityvalues)
                                                                SELECT
                                                                        @sevcode = @sevcodeok;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity;

                                                        SELECT
                                                                @metricval = dbo.getyesnotext(@dc);
                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if it is a primary or backup domain controller.';
                                                END;

                                                --************************************************* version 2.6 security checks
                                                -- Active Directory Helper Service Login Account Acceptable
                                                ELSE
                                                IF (@metricid = 67)
                                                BEGIN
                                                        IF (LEN(@severityvalues) > 0)
                                                        BEGIN
                                                                SELECT
                                                                        @strval = loginname
                                                                FROM vwservice
                                                                WHERE snapshotid = @snapshotid
                                                                AND servicetype IN (
                                                                8, 13);
                                                                IF (LEN(@strval) > 0)
                                                                BEGIN
                                                                        IF (CHARINDEX(''''
                                                                                + @strval + '''',
                                                                                @severityvalues) > 0)
                                                                                SELECT
                                                                                        @sevcode = @sevcodeok;
                                                                        ELSE
                                                                                SELECT
                                                                                        @sevcode = @severity;
                                                                        SELECT
                                                                                @metricval = N'Current login account is '
                                                                                + @strval + '.';
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = N'No login account or service not found.';
                                                        END;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'No approved login account was provided.';

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if Active Directory Helper Service Login Account is a user other than: '
                                                                + @severityvalues;
                                                END;
                                                -- Reporting Services Service Login Account Acceptable
                                                ELSE
                                                IF (@metricid = 68)
                                                BEGIN
                                                        IF (LEN(@severityvalues) > 0)
                                                        BEGIN
                                                                SELECT
                                                                        @strval = loginname
                                                                FROM vwservice
                                                                WHERE snapshotid = @snapshotid
                                                                AND servicetype = 6;
                                                                IF (LEN(@strval) > 0)
                                                                BEGIN
                                                                        IF (CHARINDEX(''''
                                                                                + @strval + '''',
                                                                                @severityvalues) > 0)
                                                                                SELECT
                                                                                        @sevcode = @sevcodeok;
                                                                        ELSE
                                                                                SELECT
                                                                                        @sevcode = @severity;
                                                                        SELECT
                                                                                @metricval = N'Current login account is '
                                                                                + @strval + '.';
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = N'No login account or service not found.';
                                                        END;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'No approved login account was provided.';

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if Reporting Services Service Login Account is a user other than: '
                                                                + @severityvalues;
                                                END;
                                                -- Volume Shadow Copy Service (VSS) Writer Login Account Acceptable
                                                ELSE
                                                IF (@metricid = 69)
                                                BEGIN
                                                        IF (LEN(@severityvalues) > 0)
                                                        BEGIN
                                                                SELECT
                                                                        @strval = loginname
                                                                FROM vwservice
                                                                WHERE snapshotid = @snapshotid
                                                                AND servicetype = 7;
                                                                IF (LEN(@strval) > 0)
                                                                BEGIN
                                                                        IF (CHARINDEX(''''
                                                                                + @strval + '''',
                                                                                @severityvalues) > 0)
                                                                                SELECT
                                                                                        @sevcode = @sevcodeok;
                                                                        ELSE
                                                                                SELECT
                                                                                        @sevcode = @severity;
                                                                        SELECT
                                                                                @metricval = N'Current login account is '
                                                                                + @strval + '.';
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = N'No login account or service not found.';
                                                        END;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'No approved login account was provided.';

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if VSS Writer Service Login Account is a user other than: '
                                                                + @severityvalues;
                                                END;
                                                -- VSS Writer Enabled
                                                ELSE
                                                IF (@metricid = 70)
                                                BEGIN
                                                        SELECT
                                                                @severityvalues = N'Running';
                                                        SELECT
                                                                @strval = [state]
                                                        FROM vwservice
                                                        WHERE snapshotid = @snapshotid
                                                        AND servicetype = 7;
                                                        IF (LEN(@strval) > 0)
                                                        BEGIN
                                                                IF (@strval <> @severityvalues)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok;
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @severity;
                                                                SELECT
                                                                        @metricval = N'Current state is '
                                                                        + @strval + '.';
                                                        END;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'Service not found or state undetermined.';

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if VSS Writer is '
                                                                + @severityvalues
                                                                + '.';
                                                END;
					-- Unauthorized Accounts Check
											ELSE IF (@metricid = 71)
											BEGIN
												IF(LEN(@severityvalues) > 0)
													BEGIN
														IF OBJECT_ID('tempdb..#usersWithExtendedPermissions') IS NOT NULL
															DROP TABLE #usersWithExtendedPermissions;
														CREATE TABLE #usersWithExtendedPermissions
														(name           NVARCHAR(128),
														 permissionName NVARCHAR(128),
														 isSysadmin     BIT
														);
														SELECT @sql = N'INSERT INTO #usersWithExtendedPermissions
																											SELECT name, permissionName, isSysadmin FROM
																													(
																														select distinct name, ''sysadmin'' as permissionName, 1 as isSysadmin 
																														from #sysadminstbl 
																														UNION
																														select distinct sprinc.name, permission as permissionName, 0 as isSysadmin from serverprincipal sprinc
																														left outer join [dbo].[serverpermission] as spermis on spermis.grantee = sprinc.principalid
																														where permission in (''IMPERSONATE ANY LOGIN'', ''SELECT ALL USER SECURABLES'', ''CONNECT ANY DATABASE'')
																														and sprinc.snapshotid = '+CONVERT( NVARCHAR, @snapshotid)+'
																														UNION
																														select distinct dprinc.name, permission as permissionName, 0 as isSysadmin from databaseprincipal dprinc
																														left outer join [dbo].[databaseobjectpermission] as dop on dop.grantee = dprinc.uid and dop.dbid = dprinc.dbid
																														where permission in (''ALTER ANY COLUMN MASTER KEY'', ''ALTER ANY COLUMN ENCRYPTION KEY'', ''VIEW ANY COLUMN MASTER KEY DEFINITION'', ''VIEW ANY COLUMN ENCRYPTION KEY DEFINITION'', ''ALTER ANY SECURITY POLICY'', ''ALTER ANY MASK'', ''UNMASK'')
																														and dprinc.snapshotid = '+CONVERT(NVARCHAR, @snapshotid)+'
																													) result
																											where lower(name) not in ('+LOWER(@severityvalues)+N')';
														IF(CHARINDEX('%', @severityvalues) > 0)
															BEGIN
																SELECT @strval = LOWER(@severityvalues),
																	   @intval = CHARINDEX(''',''', @strval);
																WHILE(@intval > 0)
																	BEGIN
																		SELECT @intval2 = CHARINDEX('%', @strval);
																		IF(@intval2 < @intval)		-- this item contains a wildcard
																			SELECT @sql = @sql+' and lower(name) not like '+SUBSTRING(@strval, 1, @intval);
																		SELECT @strval = SUBSTRING(@strval, @intval+2, LEN(@strval)-(@intval+1));
																		SELECT @intval = CHARINDEX(''',''', @strval);
																	END;
																IF(LEN(@strval) > 0)
																	BEGIN
																		SELECT @intval2 = CHARINDEX('%', @strval);
																		IF(@intval2 > 0)		-- this item contains a wildcard
																			SELECT @sql = @sql+' and lower(name) not like '+@strval;
																	END;
															END;
														SELECT @sql = @sql+N' order by name';
														SELECT @sql = @sql+'
																							declare sysadmincursor cursor for
																							SELECT name from #usersWithExtendedPermissions 
																							GROUP BY name 
																							ORDER BY name';
														EXEC (@sql);
														OPEN sysadmincursor;
														SELECT @intval = 0,
															   @intval2 = 0;
														DECLARE @unauthorizedUserName AS NVARCHAR(128);
														FETCH NEXT FROM sysadmincursor INTO @unauthorizedUserName;
														WHILE @@fetch_status = 0
															BEGIN
																DECLARE @currentRoleText AS NVARCHAR(MAX);
																SET @currentRoleText = '';
																IF(EXISTS
																  (
																	  SELECT 1
																	  FROM #usersWithExtendedPermissions
																	  WHERE name = @unauthorizedUserName
																  ))
																	BEGIN
																		DECLARE @unautorizedPermissionsList VARCHAR(MAX);
																		SET @unautorizedPermissionsList = '';
																		SELECT @unautorizedPermissionsList = @unautorizedPermissionsList+permissionName+', '
																		FROM #usersWithExtendedPermissions
																		WHERE name = @unauthorizedUserName;
																		SET @unautorizedPermissionsList = SUBSTRING(@unautorizedPermissionsList, 1, LEN(@unautorizedPermissionsList)-1);
																		SET @currentRoleText = ''''+@unauthorizedUserName+''' ('+@unautorizedPermissionsList+') ';
																	END;
																SELECT @intval = @intval + 1;
																IF(LEN(@metricval) + LEN(@currentRoleText) + LEN(@strval2) > 1400)
																	BEGIN
																		IF @intval2 = 0
																			SELECT @metricval = @metricval+N', more...',
																				   @intval2 = 1;
																	END;
																ELSE
																SELECT @metricval = @metricval+CASE
																								   WHEN LEN(@metricval) > 0
																								   THEN N', '
																								   ELSE N''
																							   END+N''+@currentRoleText+N'';
																IF(@isadmin = 1)
																	INSERT INTO policyassessmentdetail
																	(policyid,
																	 assessmentid,
																	 metricid,
																	 snapshotid,
																	 detailfinding,
																	 databaseid,
																	 objecttype,
																	 objectid,
																	 objectname
																	)
																	VALUES
																	(@policyid,
																	 @assessmentid,
																	 @metricid,
																	 @snapshotid,
																	 N'Unauthorized member found: '+@currentRoleText+N'',
																	 NULL, -- database ID,
																	 N'iLOGN', -- object type
																	 NULL,
																	 @strval
																	);
																FETCH NEXT FROM sysadmincursor INTO @unauthorizedUserName;
															END;
														CLOSE sysadmincursor;
														DEALLOCATE sysadmincursor;
														IF(LEN(@metricval) = 0)
															SELECT @sevcode = @sevcodeok,
																   @metricval = N'No logins found.';
														ELSE
														SELECT @sevcode = @severity,
															   @metricval = N'Unauthorized member found: '+@metricval;
													END;
												ELSE
												SELECT @sevcode = @sevcodeok,
													   @metricval = N'No list of unapproved logins was provided.';
												SELECT @metricthreshold = N'Server is vulnerable if not authorized logins are sysadmins or they have extended permissions. Authorized logins are: '+@severityvalues;
											END;
                                                -- sa Account disabled  (this is a subset of metric 16)
                                                ELSE
                                                IF (@metricid = 72)
                                                BEGIN
                                                        -- only apply this check if the version is 2005 or greater
                                                        IF (@version > N'9.'
                                                                OR @version < N'6.'
                                                                )
                                                        BEGIN
                                                                -- check to make sure the sa account is either disabled or renamed
                                                                SELECT
                                                                        @severityvalues = N'N';
                                                                SELECT
                                                                        @metricval = [name],
                                                                        @strval = [disabled]
                                                                FROM serverprincipal
                                                                WHERE snapshotid = @snapshotid
                                                                AND sid = 0x01;

                                                                IF (LOWER(@metricval) = N'sa'
                                                                        AND @strval = @severityvalues
                                                                        )
                                                                        SELECT
                                                                                @sevcode = @severity;
                                                                ELSE
                                                                IF (LOWER(@metricval) <> N'sa'
                                                                        AND @strval = @severityvalues
                                                                        )
                                                                        SELECT
                                                                                @sevcode = 2;
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @sevcodeok;

                                                                SELECT
                                                                        @metricval = N'The sa account is enabled.';
                                                        END;
                                                        ELSE
                                                        BEGIN
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'The sa account is always enabled on SQL Server 2000.';
                                                        END;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if the sa account is enabled.';
                                                END;
                                                -- 'ALTER TRACE permissions
                                                ELSE
                                                IF (@metricid = 73)
                                                BEGIN
                                                        -- only apply this check if the version is 2005 or greater
                                                        IF (@version > N'9.'
                                                                OR @version < N'6.'
                                                                )
                                                        BEGIN
                                                                IF (LEN(@severityvalues) > 0)
                                                                BEGIN
                                                                        SELECT
                                                                                @sql = N'declare altertracecursor cursor for
														select b.name 
															from serverpermission a, serverprincipal b
															where a.snapshotid='
                                                                                + CONVERT(nvarchar, @snapshotid)
                                                                                + N' 
																and a.classid=100 
																and a.permission = ''ALTER TRACE''
																and a.snapshotid = b.snapshotid
																and a.grantee = b.principalid
																and a.grantee not in (select id from #sysadminstbl)
																and lower(b.name) not in ('
                                                                                + LOWER(@severityvalues)
                                                                                + N')';
                                                                        IF (CHARINDEX('%',
                                                                                @severityvalues) > 0)
                                                                        BEGIN
                                                                                SELECT
                                                                                        @strval = LOWER(@severityvalues),
                                                                                        @intval = CHARINDEX(''',''',
                                                                                        @strval);
                                                                                WHILE (@intval > 0)
                                                                                BEGIN
                                                                                        SELECT
                                                                                                @intval2 = CHARINDEX('%',
                                                                                                @strval);
                                                                                        IF (@intval2 < @intval)		-- this item contains a wildcard
                                                                                                SELECT
                                                                                                        @sql = @sql
                                                                                                        + ' and lower(b.name) not like '
                                                                                                        + SUBSTRING(@strval,
                                                                                                        1, @intval);
                                                                                        SELECT
                                                                                                @strval = SUBSTRING(@strval,
                                                                                                @intval + 2,
                                                                                                LEN(@strval)
                                                                                                - (@intval + 1));
                                                                                        SELECT
                                                                                                @intval = CHARINDEX(''',''',
                                                                                                @strval);
                                                                                END;
                                                                                IF (LEN(@strval) > 0)
                                                                                BEGIN
                                                                                        SELECT
                                                                                                @intval2 = CHARINDEX('%',
                                                                                                @strval);
                                                                                        IF (@intval2 > 0)		-- this item contains a wildcard
                                                                                                SELECT
                                                                                                        @sql = @sql
                                                                                                        + ' and lower(b.name) not like '
                                                                                                        + @strval;
                                                                                END;
                                                                        END;
                                                                        SELECT
                                                                                @sql = @sql
                                                                                + N' order by a.permission';

                                                                        EXEC (@sql);
                                                                        OPEN altertracecursor;
                                                                        FETCH NEXT FROM
                                                                        altertracecursor INTO @strval;

                                                                        SELECT
                                                                                @intval2 = 0;
                                                                        WHILE @@fetch_status = 0
                                                                        BEGIN
                                                                                IF (@intval2 = 1
                                                                                        OR LEN(@metricval)
                                                                                        + LEN(@strval) > 1400
                                                                                        )
                                                                                BEGIN
                                                                                        IF @intval2 = 0
                                                                                                SELECT
                                                                                                        @metricval = @metricval
                                                                                                        + N', more...',
                                                                                                        @intval2 = 1;
                                                                                END;
                                                                                ELSE
                                                                                        SELECT
                                                                                                @metricval = @metricval
                                                                                                + CASE
                                                                                                        WHEN LEN(@metricval) > 0 THEN N', '
                                                                                                        ELSE N''
                                                                                                END + N''''
                                                                                                + @strval
                                                                                                + N'''';

                                                                                IF (@isadmin = 1)
                                                                                        INSERT INTO policyassessmentdetail (policyid,
                                                                                        assessmentid,
                                                                                        metricid,
                                                                                        snapshotid,
                                                                                        detailfinding,
                                                                                        databaseid,
                                                                                        objecttype,
                                                                                        objectid,
                                                                                        objectname)
                                                                                                VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'ALTER TRACE permission granted to ''' + @strval + N'''', NULL, -- database ID,
                                                                                                N'iSRV', -- object type
                                                                                                NULL, @strval);

                                                                                FETCH NEXT FROM
                                                                                altertracecursor INTO @strval;
                                                                        END;

                                                                        CLOSE altertracecursor;
                                                                        DEALLOCATE altertracecursor;

                                                                        IF (LEN(@metricval) = 0)
                                                                                SELECT
                                                                                        @sevcode = @sevcodeok,
                                                                                        @metricval = 'No ALTER TRACE permissions found.';
                                                                        ELSE
                                                                                SELECT
                                                                                        @sevcode = @severity,
                                                                                        @metricval = N'Unauthorized logins found: '
                                                                                        + @metricval;
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = N'No list of approved logins was provided.';
                                                        END;
                                                        ELSE
                                                        BEGIN
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'The ALTER TRACE permission does not exist on SQL Server 2000.';
                                                        END;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if the ALTER TRACE permission has been granted to a login that is not a sysadmin and is not: '
                                                                + @severityvalues;
                                                END;
                                                -- CONTROL SERVER permissions
                                                ELSE
                                                IF (@metricid = 74)
                                                BEGIN
                                                        -- only apply this check if the version is 2005 or greater
                                                        IF (@version > N'9.'
                                                                OR @version < N'6.'
                                                                )
                                                        BEGIN
                                                                IF (LEN(@severityvalues) > 0)
                                                                BEGIN
                                                                        SELECT
                                                                                @sql = N'declare controlcursor cursor for
														select b.name 
															from serverpermission a, serverprincipal b
															where a.snapshotid='
                                                                                + CONVERT(nvarchar, @snapshotid)
                                                                                + N' 
																and a.classid=100 
																and a.permission = ''CONTROL SERVER''
																and a.snapshotid = b.snapshotid
																and a.grantee = b.principalid
																and a.grantee not in (select id from #sysadminstbl)
																and lower(b.name) not in ('
                                                                                + LOWER(@severityvalues)
                                                                                + N')';
                                                                        IF (CHARINDEX('%',
                                                                                @severityvalues) > 0)
                                                                        BEGIN
                                                                                SELECT
                                                                                        @strval = LOWER(@severityvalues),
                                                                                        @intval = CHARINDEX(''',''',
                                                                                        @strval);
                                                                                WHILE (@intval > 0)
                                                                                BEGIN
                                                                                        SELECT
                                                                                                @intval2 = CHARINDEX('%',
                                                                                                @strval);
                                                                                        IF (@intval2 < @intval)		-- this item contains a wildcard
                                                                                                SELECT
                                                                                                        @sql = @sql
                                                                                                        + ' and lower(b.name) not like '
                                                                                                        + SUBSTRING(@strval,
                                                                                                        1, @intval);
                                                                                        SELECT
                                                                                                @strval = SUBSTRING(@strval,
                                                                                                @intval + 2,
                                                                                                LEN(@strval)
                                                                                                - (@intval + 1));
                                                                                        SELECT
                                                                                                @intval = CHARINDEX(''',''',
                                                                                                @strval);
                                                                                END;
                                                                                IF (LEN(@strval) > 0)
                                                                                BEGIN
                                                                                        SELECT
                                                                                                @intval2 = CHARINDEX('%',
                                                                                                @strval);
                                                                                        IF (@intval2 > 0)		-- this item contains a wildcard
                                                                                                SELECT
                                                                                                        @sql = @sql
                                                                                                        + ' and lower(b.name) not like '
                                                                                                        + @strval;
                                                                                END;
                                                                        END;
                                                                        SELECT
                                                                                @sql = @sql
                                                                                + N' order by a.permission';

                                                                        EXEC (@sql);
                                                                        OPEN controlcursor;
                                                                        FETCH NEXT FROM
                                                                        controlcursor INTO @strval;

                                                                        SELECT
                                                                                @intval2 = 0;
                                                                        WHILE @@fetch_status = 0
                                                                        BEGIN
                                                                                IF (@intval2 = 1
                                                                                        OR LEN(@metricval)
                                                                                        + LEN(@strval) > 1400
                                                                                        )
                                                                                BEGIN
                                                                                        IF @intval2 = 0
                                                                                                SELECT
                                                                                                        @metricval = @metricval
                                                                                                        + N', more...',
                                                                                                        @intval2 = 1;
                                                                                END;
                                                                                ELSE
                                                                                        SELECT
                                                                                                @metricval = @metricval
                                                                                                + CASE
                                                                                                        WHEN LEN(@metricval) > 0 THEN N', '
                                                                                                        ELSE N''
                                                                                                END + N''''
                                                                                                + @strval
                                                                                                + N'''';

                                                                                IF (@isadmin = 1)
                                                                                        INSERT INTO policyassessmentdetail (policyid,
                                                                                        assessmentid,
                                                                                        metricid,
                                                                                        snapshotid,
                                                                                        detailfinding,
                                                                                        databaseid,
                                                                                        objecttype,
                                                                                        objectid,
                                                                                        objectname)
                                                                                                VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'CONTROL SERVER permission granted to ''' + @strval + N'''', NULL, -- database ID,
                                                                                                N'iSRV', -- object type
                                                                                                NULL, @strval);

                                                                                FETCH NEXT FROM
                                                                                controlcursor INTO @strval;
                                                                        END;

                                                                        CLOSE controlcursor;
                                                                        DEALLOCATE controlcursor;

                                                                        IF (LEN(@metricval) = 0)
                                                                                SELECT
                                                                                        @sevcode = @sevcodeok,
                                                                                        @metricval = 'No CONTROL SERVER permissions found.';
                                                                        ELSE
                                                                                SELECT
                                                                                        @sevcode = @severity,
                                                                                        @metricval = N'Unauthorized logins found: '
                                                                                        + @metricval;
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = N'No list of approved logins was provided.';
                                                        END;
                                                        ELSE
                                                        BEGIN
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'The CONTROL SERVER permission does not exist on SQL Server 2000.';
                                                        END;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if the CONTROL SERVER permission has been granted to a login that is not a sysadmin and is not: '
                                                                + @severityvalues;
                                                END;
                                                -- xp_cmdshell Enabled
                                                ELSE
                                                IF (@metricid = 75)
                                                BEGIN
                                                        -- only apply this check if the version is 2005 or greater
                                                        IF (@version > N'9.'
                                                                OR @version < N'6.'
                                                                )
                                                        BEGIN
                                                                SELECT
                                                                        @strval = @xp_cmdshell,
                                                                        @severityvalues = N'N';
                                                                IF (@strval = @severityvalues)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok;
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @severity;

                                                                SELECT
                                                                        @metricval = dbo.getyesnotext(@strval);
                                                        END;
                                                        ELSE
                                                        BEGIN
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'The ability to disable xp_cmdshell does not exist on SQL Server 2000.';
                                                        END;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if the xp_cmdshell extended stored procedure is enabled.';
                                                END;

                                                -- Required Admin Accounts
                                                ELSE
                                                IF (@metricid = 76)
                                                BEGIN
                                                        IF (LEN(@severityvalues) > 0)
                                                        BEGIN
                                                                -- convert the severityvalues into a table for reverse selection
                                                                SELECT
                                                                        @intval = 1;
                                                                SELECT
                                                                        @intval2 = CHARINDEX(''',''',
                                                                        @severityvalues);
                                                                WHILE @intval2 > 0
                                                                BEGIN
                                                                        INSERT INTO @tblval
                                                                                VALUES (SUBSTRING(@severityvalues, @intval + 1, @intval2 - @intval - 1));
                                                                        SELECT
                                                                                @intval = @intval2
                                                                                + 2,
                                                                                @intval2 = CHARINDEX(''',''',
                                                                                @severityvalues,
                                                                                @intval + 1);
                                                                END;
                                                                INSERT INTO @tblval
                                                                        VALUES (SUBSTRING(@severityvalues, @intval + 1, LEN(@severityvalues) - @intval - 1));

                                                                DECLARE reqadmincursor CURSOR FOR
                                                                SELECT
                                                                        val
                                                                FROM @tblval
                                                                WHERE LOWER(val) NOT IN (SELECT
                                                                        LOWER(name)
                                                                FROM #sysadminstbl)
                                                                ORDER BY val;

                                                                OPEN reqadmincursor;
                                                                FETCH NEXT FROM
                                                                reqadmincursor INTO @strval;

                                                                SELECT
                                                                        @intval2 = 0;
                                                                WHILE @@fetch_status = 0
                                                                BEGIN
                                                                        IF (@intval2 = 1
                                                                                OR LEN(@metricval)
                                                                                + LEN(@strval) > 1400
                                                                                )
                                                                        BEGIN
                                                                                IF @intval2 = 0
                                                                                        SELECT
                                                                                                @metricval = @metricval
                                                                                                + N', more...',
                                                                                                @intval2 = 1;
                                                                        END;
                                                                        ELSE
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + CASE
                                                                                                WHEN LEN(@metricval) > 0 THEN N', '
                                                                                                ELSE N''
                                                                                        END + N''''
                                                                                        + @strval
                                                                                        + N'''';

                                                                        IF (@isadmin = 1)
                                                                                INSERT INTO policyassessmentdetail (policyid,
                                                                                assessmentid,
                                                                                metricid,
                                                                                snapshotid,
                                                                                detailfinding,
                                                                                databaseid,
                                                                                objecttype,
                                                                                objectid,
                                                                                objectname)
                                                                                        VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Required administrative login not found: ''' + @strval + N'''', NULL, -- database ID,
                                                                                        N'iSRV', -- object type
                                                                                        NULL, @strval);

                                                                        FETCH NEXT FROM
                                                                        reqadmincursor INTO @strval;
                                                                END;

                                                                CLOSE reqadmincursor;
                                                                DEALLOCATE reqadmincursor;

                                                                IF (LEN(@metricval) = 0)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = 'All required administrative logins were found.';
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @severity,
                                                                                @metricval = N'These administrative logins are missing or are not sysadmin members:  '
                                                                                + @metricval;
                                                        END;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'No required administrative login account was provided.';

                                                        SELECT
                                                                @metricthreshold = N'Server is not standardized if the sysadmin server role does not include these accounts: '
                                                                + @severityvalues;
                                                END;

                                                -- Password Policy Enabled for sa
                                                ELSE
                                                IF (@metricid = 77)
                                                BEGIN
                                                        SELECT
                                                                @severityvalues = N'N';
                                                        SELECT
                                                                @strval = ispolicychecked
                                                        FROM serverprincipal
                                                        WHERE snapshotid = @snapshotid
                                                        AND sid = 0x01;

                                                        IF (@strval <> @severityvalues)
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = 'The password policy is enforced on the sa account.';
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity,
                                                                        @metricval = N'Password policy not enforced on the sa account.';

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if password policy is not enforced on the sa account.';
                                                END;

                                                -- Database files missing required administrative permissions
                                                ELSE
                                                IF (@metricid = 78)
                                                BEGIN
                                                        IF (LEN(@severityvalues) > 0)
                                                        BEGIN
                                                                SELECT
                                                                        @intval = 1;
                                                                SELECT
                                                                        @intval2 = CHARINDEX(''',''',
                                                                        @severityvalues);
                                                                WHILE @intval2 > 0
                                                                BEGIN
                                                                        INSERT INTO @tblval
                                                                                VALUES (SUBSTRING(@severityvalues, @intval + 1, @intval2 - @intval - 1));
                                                                        SELECT
                                                                                @intval = @intval2
                                                                                + 2,
                                                                                @intval2 = CHARINDEX(''',''',
                                                                                @severityvalues,
                                                                                @intval + 1);
                                                                END;
                                                                INSERT INTO @tblval
                                                                        VALUES (SUBSTRING(@severityvalues, @intval + 1, LEN(@severityvalues) - @intval - 1));

                                                                DECLARE filecursor CURSOR STATIC FOR
                                                                SELECT
                                                                        o.objectname,
                                                                        a.val
                                                                FROM (vwfilesystemobject o
                                                                CROSS JOIN @tblval a
                                                                )
                                                                WHERE o.snapshotid = @snapshotid
                                                                AND o.objecttype = N'DB'
                                                                AND NOT EXISTS (SELECT
                                                                        1
                                                                FROM vwfilesystemobjectpermission p
                                                                WHERE p.snapshotid = @snapshotid
                                                                AND p.osobjectid = o.osobjectid
                                                                AND LOWER(p.grantee) LIKE LOWER(a.val)
                                                                AND p.filesystemrights = 2032127)	-- make sure they have full control
                                                                ORDER BY o.objectname,
                                                                a.val;

                                                                OPEN filecursor;

                                                                SELECT
                                                                        @intval = 0,
                                                                        @intval2 = 0;
                                                                FETCH NEXT FROM
                                                                filecursor INTO @strval,
                                                                @strval2;
                                                                WHILE @@fetch_status = 0
                                                                BEGIN
                                                                        SELECT
                                                                                @intval = @intval
                                                                                + 1;
                                                                        IF (LEN(@metricval)
                                                                                + LEN(@strval)
                                                                                + LEN(@strval2) > 1400)
                                                                        BEGIN
                                                                                IF @intval2 = 0
                                                                                        SELECT
                                                                                                @metricval = @metricval
                                                                                                + N', more...',
                                                                                                @intval2 = 1;
                                                                        END;
                                                                        ELSE
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + CASE
                                                                                                WHEN LEN(@metricval) > 0 THEN N', '
                                                                                                ELSE N''
                                                                                        END + N''''
                                                                                        + @strval2
                                                                                        + N''' on '''
                                                                                        + @strval
                                                                                        + N'''';

                                                                        IF (@isadmin = 1)
                                                                                INSERT INTO policyassessmentdetail (policyid,
                                                                                assessmentid,
                                                                                metricid,
                                                                                snapshotid,
                                                                                detailfinding,
                                                                                databaseid,
                                                                                objecttype,
                                                                                objectid,
                                                                                objectname)
                                                                                        VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Database file missing required permission found: ''' + @strval + N''' is missing Full Control for ''' + @strval2 + N'''', NULL, -- database ID,
                                                                                        N'FI', -- no file object type
                                                                                        NULL, @strval);

                                                                        FETCH NEXT FROM
                                                                        filecursor INTO @strval,
                                                                        @strval2;
                                                                END;

                                                                CLOSE filecursor;
                                                                DEALLOCATE filecursor;

                                                                IF (LEN(@metricval) = 0)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = 'All database files have the required administrative account permissions.';
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @severity,
                                                                                @metricval = N'Administrative accounts are missing Full Control permission on database files: '
                                                                                + @metricval;
                                                        END;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'No list of required administrative accounts was provided.';

                                                        SELECT
                                                                @metricthreshold = N'Server is not standardized if database files do not include Full Control permission for '
                                                                + @severityvalues;
                                                END;

                                                -- Executable files missing required administrative permissions
                                                ELSE
                                                IF (@metricid = 79)
                                                BEGIN
                                                        IF (LEN(@severityvalues) > 0)
                                                        BEGIN
                                                                SELECT
                                                                        @intval = 1;
                                                                SELECT
                                                                        @intval2 = CHARINDEX(''',''',
                                                                        @severityvalues);
                                                                WHILE @intval2 > 0
                                                                BEGIN
                                                                        INSERT INTO @tblval
                                                                                VALUES (SUBSTRING(@severityvalues, @intval + 1, @intval2 - @intval - 1));
                                                                        SELECT
                                                                                @intval = @intval2
                                                                                + 2,
                                                                                @intval2 = CHARINDEX(''',''',
                                                                                @severityvalues,
                                                                                @intval + 1);
                                                                END;
                                                                INSERT INTO @tblval
                                                                        VALUES (SUBSTRING(@severityvalues, @intval + 1, LEN(@severityvalues) - @intval - 1));

                                                                DECLARE filecursor CURSOR STATIC FOR
                                                                SELECT
                                                                        o.objectname,
                                                                        a.val
                                                                FROM (vwfilesystemobject o
                                                                CROSS JOIN @tblval a
                                                                )
                                                                WHERE o.snapshotid = @snapshotid
                                                                AND o.objecttype = N'File'
                                                                AND (o.objectname LIKE '%.exe'
                                                                OR o.objectname LIKE '%.dll'
                                                                )
                                                                AND NOT EXISTS (SELECT
                                                                        1
                                                                FROM vwfilesystemobjectpermission p
                                                                WHERE p.snapshotid = @snapshotid
                                                                AND p.osobjectid = o.osobjectid
                                                                AND LOWER(p.grantee) LIKE LOWER(a.val)
                                                                AND p.filesystemrights = 2032127)	-- make sure they have full control
                                                                ORDER BY o.objectname,
                                                                a.val;

                                                                OPEN filecursor;

                                                                SELECT
                                                                        @intval = 0,
                                                                        @intval2 = 0;
                                                                FETCH NEXT FROM
                                                                filecursor INTO @strval,
                                                                @strval2;
                                                                WHILE @@fetch_status = 0
                                                                BEGIN
                                                                        SELECT
                                                                                @intval = @intval
                                                                                + 1;
                                                                        IF (LEN(@metricval)
                                                                                + LEN(@strval)
                                                                                + LEN(@strval2) > 1400)
                                                                        BEGIN
                                                                                IF @intval2 = 0
                                                                                        SELECT
                                                                                                @metricval = @metricval
                                                                                                + N', more...',
                                                                                                @intval2 = 1;
                                                                        END;
                                                                        ELSE
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + CASE
                                                                                                WHEN LEN(@metricval) > 0 THEN N', '
                                                                                                ELSE N''
                                                                                        END + N''''
                                                                                        + @strval2
                                                                                        + N''' on '''
                                                                                        + @strval
                                                                                        + N'''';

                                                                        IF (@isadmin = 1)
                                                                                INSERT INTO policyassessmentdetail (policyid,
                                                                                assessmentid,
                                                                                metricid,
                                                                                snapshotid,
                                                                                detailfinding,
                                                                                databaseid,
                                                                                objecttype,
                                                                                objectid,
                                                                                objectname)
                                                                                        VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Executable file missing required permission found: ''' + @strval + N''' is missing Full Control for ''' + @strval2 + N'''', NULL, -- database ID,
                                                                                        N'FI', -- no file object type
                                                                                        NULL, @strval);

                                                                        FETCH NEXT FROM
                                                                        filecursor INTO @strval,
                                                                        @strval2;
                                                                END;

                                                                CLOSE filecursor;
                                                                DEALLOCATE filecursor;

                                                                IF (LEN(@metricval) = 0)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = 'All executable files have the required administrative account permissions.';
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @severity,
                                                                                @metricval = N'Administrative accounts are missing Full Control permission on executable files: '
                                                                                + @metricval;
                                                        END;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'No list of required administrative accounts was provided.';

                                                        SELECT
                                                                @metricthreshold = N'Server is not standardized if executable files do not include Full Control permissions for '
                                                                + @severityvalues;
                                                END;

                                                -- Registry Keys missing required administrative permissions
                                                ELSE
                                                IF (@metricid = 80)
                                                BEGIN
                                                        IF (LEN(@severityvalues) > 0)
                                                        BEGIN
                                                                SELECT
                                                                        @intval = 1;
                                                                SELECT
                                                                        @intval2 = CHARINDEX(''',''',
                                                                        @severityvalues);
                                                                WHILE @intval2 > 0
                                                                BEGIN
                                                                        INSERT INTO @tblval
                                                                                VALUES (SUBSTRING(@severityvalues, @intval + 1, @intval2 - @intval - 1));
                                                                        SELECT
                                                                                @intval = @intval2
                                                                                + 2,
                                                                                @intval2 = CHARINDEX(''',''',
                                                                                @severityvalues,
                                                                                @intval + 1);
                                                                END;
                                                                INSERT INTO @tblval
                                                                        VALUES (SUBSTRING(@severityvalues, @intval + 1, LEN(@severityvalues) - @intval - 1));

                                                                DECLARE filecursor CURSOR STATIC FOR
                                                                SELECT
                                                                        o.objectname,
                                                                        a.val
                                                                FROM (vwregistrykey o
                                                                CROSS JOIN @tblval a
                                                                )
                                                                WHERE o.snapshotid = @snapshotid
                                                                AND NOT EXISTS (SELECT
                                                                        1
                                                                FROM vwregistrykeypermission p
                                                                WHERE p.snapshotid = @snapshotid
                                                                AND p.osobjectid = o.osobjectid
                                                                AND LOWER(p.grantee) LIKE LOWER(a.val)
                                                                AND p.filesystemrights = 983103)	-- make sure they have registry rights full control
                                                                ORDER BY o.objectname,
                                                                a.val;

                                                                OPEN filecursor;

                                                                SELECT
                                                                        @intval = 0,
                                                                        @intval2 = 0;
                                                                FETCH NEXT FROM
                                                                filecursor INTO @strval,
                                                                @strval2;
                                                                WHILE @@fetch_status = 0
                                                                BEGIN
                                                                        SELECT
                                                                                @intval = @intval
                                                                                + 1;
                                                                        IF (LEN(@metricval)
                                                                                + LEN(@strval)
                                                                                + LEN(@strval2) > 1400)
                                                                        BEGIN
                                                                                IF @intval2 = 0
                                                                                        SELECT
                                                                                                @metricval = @metricval
                                                                                                + N', more...',
                                                                                                @intval2 = 1;
                                                                        END;
                                                                        ELSE
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + CASE
                                                                                                WHEN LEN(@metricval) > 0 THEN N', '
                                                                                                ELSE N''
                                                                                        END + N''''
                                                                                        + @strval2
                                                                                        + N''' on '''
                                                                                        + @strval
                                                                                        + N'''';

                                                                        IF (@isadmin = 1)
                                                                                INSERT INTO policyassessmentdetail (policyid,
                                                                                assessmentid,
                                                                                metricid,
                                                                                snapshotid,
                                                                                detailfinding,
                                                                                databaseid,
                                                                                objecttype,
                                                                                objectid,
                                                                                objectname)
                                                                                        VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Registry key missing required administrative permission found: ''' + @strval + N''' is missing Full Control for ''' + @strval2 + N'''', NULL, -- database ID,
                                                                                        N'Reg', -- no registry object type
                                                                                        NULL, @strval);

                                                                        FETCH NEXT FROM
                                                                        filecursor INTO @strval,
                                                                        @strval2;
                                                                END;

                                                                CLOSE filecursor;
                                                                DEALLOCATE filecursor;

                                                                IF (LEN(@metricval) = 0)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = 'All registry keys have the required administrative account permissions.';
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @severity,
                                                                                @metricval = N'Administrative accounts are missing Full Control permission on registry keys: '
                                                                                + @metricval;
                                                        END;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'No list of required administrative accounts was provided.';

                                                        SELECT
                                                                @metricthreshold = N'Server is not standardized if registry keys do not include Full Control permission for '
                                                                + @severityvalues;
                                                END;

                                                -- Data Files on System Drive
                                                ELSE
                                                IF (@metricid = 81)
                                                BEGIN
                                                        SELECT
                                                                @sql = N'declare filecursor cursor static for
												select objectname
														from vwfilesystemobject
															where snapshotid = '
                                                                + CONVERT(nvarchar, @snapshotid)
                                                                + N'
																and objecttype= N''DB''
																and lower(left(objectname,2)) = '''
                                                                + LOWER(@systemdrive)
                                                                + N'''';
                                                        -- This one is ok to process with no values although there isn't really a way to configure it like that
                                                        -- It will just produce findings on any data file
                                                        IF (LEN(@severityvalues) > 0)
                                                        BEGIN
                                                                SELECT
                                                                        @sql = @sql
                                                                        + N'				and lower(objectname) not in ('
                                                                        + LOWER(@severityvalues)
                                                                        + N')';
                                                                IF (CHARINDEX('%',
                                                                        @severityvalues) > 0)
                                                                BEGIN
                                                                        SELECT
                                                                                @strval = LOWER(@severityvalues),
                                                                                @intval = CHARINDEX(''',''',
                                                                                @strval);
                                                                        WHILE (@intval > 0)
                                                                        BEGIN
                                                                                SELECT
                                                                                        @intval2 = CHARINDEX('%',
                                                                                        @strval);
                                                                                IF (@intval2 < @intval)		-- this item contains a wildcard
                                                                                        SELECT
                                                                                                @sql = @sql
                                                                                                + N' and lower(objectname) not like '
                                                                                                + SUBSTRING(@strval,
                                                                                                1, @intval);
                                                                                SELECT
                                                                                        @strval = SUBSTRING(@strval,
                                                                                        @intval + 2,
                                                                                        LEN(@strval)
                                                                                        - (@intval + 1));
                                                                                SELECT
                                                                                        @intval = CHARINDEX(''',''',
                                                                                        @strval);
                                                                        END;
                                                                        IF (LEN(@strval) > 0)
                                                                        BEGIN
                                                                                SELECT
                                                                                        @intval2 = CHARINDEX('%',
                                                                                        @strval);
                                                                                IF (@intval2 > 0)		-- this item contains a wildcard
                                                                                        SELECT
                                                                                                @sql = @sql
                                                                                                + N' and lower(objectname) not like '
                                                                                                + @strval;
                                                                        END;
                                                                END;
                                                        END;

                                                        SELECT
                                                                @sql = @sql
                                                                + N' order by objectname';

                                                        EXEC (@sql);

                                                        OPEN filecursor;

                                                        SELECT
                                                                @intval = 0,
                                                                @intval2 = 0;
                                                        FETCH NEXT FROM
                                                        filecursor INTO @strval;
                                                        WHILE @@fetch_status = 0
                                                        BEGIN
                                                                SELECT
                                                                        @intval = @intval
                                                                        + 1;
                                                                IF (LEN(@metricval)
                                                                        + LEN(@strval)
                                                                        + LEN(@strval2) > 1400)
                                                                BEGIN
                                                                        IF @intval2 = 0
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + N', more...',
                                                                                        @intval2 = 1;
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @metricval = @metricval
                                                                                + CASE
                                                                                        WHEN LEN(@metricval) > 0 THEN N', '
                                                                                        ELSE N''
                                                                                END + N''''
                                                                                + @strval
                                                                                + N'''';

                                                                IF (@isadmin = 1)
                                                                        INSERT INTO policyassessmentdetail (policyid,
                                                                        assessmentid,
                                                                        metricid,
                                                                        snapshotid,
                                                                        detailfinding,
                                                                        databaseid,
                                                                        objecttype,
                                                                        objectid,
                                                                        objectname)
                                                                                VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Data file found on system drive: ''' + @strval + N'''', NULL, -- database ID,
                                                                                N'FI', -- no file object type
                                                                                NULL, @strval);

                                                                FETCH NEXT FROM
                                                                filecursor INTO @strval;
                                                        END;

                                                        CLOSE filecursor;
                                                        DEALLOCATE filecursor;

                                                        IF (LEN(@metricval) = 0)
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'There are no data files on the system drive.';
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity,
                                                                        @metricval = N'Data files found on the system drive: '
                                                                        + @metricval;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if any data files are located on the '
                                                                + @systemdrive
                                                                + N' system drive and are not in '
                                                                + @severityvalues;
                                                END;

                                                -- SQL Server Installation on System Drive
                                                ELSE
                                                IF (@metricid = 82)
                                                BEGIN
                                                        IF (LEN(RTRIM(@systemdrive)) > 0)
                                                        BEGIN
                                                                SELECT
                                                                        @sql = N'declare sysfilecursor cursor static for
													select objectname
														from vwfilesystemobject
															where snapshotid = '
                                                                        + CONVERT(nvarchar, @snapshotid)
                                                                        + N'
																and objecttype= N''IDir''
																and lower(left(objectname,2)) = '''
                                                                        + LOWER(@systemdrive)
                                                                        + N'''';
                                                                -- This one is ok to process with no values although there isn't really a way to configure it like that
                                                                -- It will just produce findings on any data file
                                                                IF (LEN(@severityvalues) > 0)
                                                                BEGIN
                                                                        SELECT
                                                                                @sql = @sql
                                                                                + N'				and lower(objectname) not in ('
                                                                                + LOWER(@severityvalues)
                                                                                + N')';
                                                                        IF (CHARINDEX('%',
                                                                                @severityvalues) > 0)
                                                                        BEGIN
                                                                                SELECT
                                                                                        @strval = LOWER(@severityvalues),
                                                                                        @intval = CHARINDEX(''',''',
                                                                                        @strval);
                                                                                WHILE (@intval > 0)
                                                                                BEGIN
                                                                                        SELECT
                                                                                                @intval2 = CHARINDEX('%',
                                                                                                @strval);
                                                                                        IF (@intval2 < @intval)		-- this item contains a wildcard
                                                                                                SELECT
                                                                                                        @sql = @sql
                                                                                                        + N' and lower(objectname) not like '
                                                                                                        + SUBSTRING(@strval,
                                                                                                        1, @intval);
                                                                                        SELECT
                                                                                                @strval = SUBSTRING(@strval,
                                                                                                @intval + 2,
                                                                                                LEN(@strval)
                                                                                                - (@intval + 1));
                                                                                        SELECT
                                                                                                @intval = CHARINDEX(''',''',
                                                                                                @strval);
                                                                                END;
                                                                                IF (LEN(@strval) > 0)
                                                                                BEGIN
                                                                                        SELECT
                                                                                                @intval2 = CHARINDEX('%',
                                                                                                @strval);
                                                                                        IF (@intval2 > 0)		-- this item contains a wildcard
                                                                                                SELECT
                                                                                                        @sql = @sql
                                                                                                        + N' and lower(objectname) not like '
                                                                                                        + @strval;
                                                                                END;
                                                                        END;
                                                                END;

                                                                SELECT
                                                                        @sql = @sql
                                                                        + N' order by objectname';

                                                                EXEC (@sql);

                                                                OPEN sysfilecursor;

                                                                SELECT
                                                                        @intval = 0,
                                                                        @intval2 = 0;
                                                                FETCH NEXT FROM
                                                                sysfilecursor INTO @strval;
                                                                WHILE @@fetch_status = 0
                                                                BEGIN
                                                                        SELECT
                                                                                @intval = @intval
                                                                                + 1;
                                                                        IF (LEN(@metricval)
                                                                                + LEN(@strval)
                                                                                + LEN(@strval2) > 1400)
                                                                        BEGIN
                                                                                IF @intval2 = 0
                                                                                        SELECT
                                                                                                @metricval = @metricval
                                                                                                + N', more...',
                                                                                                @intval2 = 1;
                                                                        END;
                                                                        ELSE
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + CASE
                                                                                                WHEN LEN(@metricval) > 0 THEN N', '
                                                                                                ELSE N''
                                                                                        END + N''''
                                                                                        + @strval
                                                                                        + N'''';

                                                                        IF (@isadmin = 1)
                                                                                INSERT INTO policyassessmentdetail (policyid,
                                                                                assessmentid,
                                                                                metricid,
                                                                                snapshotid,
                                                                                detailfinding,
                                                                                databaseid,
                                                                                objecttype,
                                                                                objectid,
                                                                                objectname)
                                                                                        VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Installation directory found on system drive: ''' + @strval + N'''', NULL, -- database ID,
                                                                                        N'IDir', -- no file object type
                                                                                        NULL, @strval);

                                                                        FETCH NEXT FROM
                                                                        sysfilecursor INTO @strval;
                                                                END;

                                                                CLOSE sysfilecursor;
                                                                DEALLOCATE sysfilecursor;

                                                                IF (LEN(@metricval) = 0)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = N'No unapproved SQL Server installation directories found on the system drive.';
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @severity,
                                                                                @metricval = N'Unapproved SQL Server installation directories found on the system drive: '
                                                                                + @metricval;
                                                        END;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'The system drive is not known. No check is performed.';

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if the '
                                                                + CASE
                                                                        WHEN LEN(RTRIM(@systemdrive)) = 0 THEN N''
                                                                        ELSE @systemdrive
                                                                                + N' '
                                                                END
                                                                + N' system drive hosts SQL Server installation directories other than: '
                                                                + @severityvalues;
                                                END;

                                                -- Ad Hoc Distributed Queries Enabled
                                                ELSE
                                                IF (@metricid = 83)
                                                BEGIN
                                                        -- only apply this check if the version is 2005 or greater
                                                        IF (@version > N'9.'
                                                                OR @version < N'6.'
                                                                )
                                                        BEGIN
                                                                SELECT
                                                                        @strval = @adhocqueries,
                                                                        @severityvalues = N'Y';
                                                                IF (@strval <> @severityvalues)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = N'Ad Hoc Distributed Queries are not enabled.';
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @severity,
                                                                                @metricval = N'Ad Hoc Distributed Queries are enabled.';
                                                        END;
                                                        ELSE
                                                        BEGIN
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'The ability to disable Ad Hoc Distributed Queries does not exist on SQL Server 2000.';
                                                        END;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if Ad Hoc Distributed Queries are enabled.';
                                                END;

                                                -- Unauthorized SQL Logins exist
                                                ELSE
                                                IF (@metricid = 84)
                                                BEGIN
                                                        SELECT
                                                                @sql = N'declare logincursor cursor for
												select name
													from vwserverprincipal
													where snapshotid = '
                                                                + CONVERT(nvarchar, @snapshotid)
                                                                + N'
														and type = N''S''';
                                                        -- This is ok to process with no values even though there is no way to configure it that way
                                                        -- It will just produce findings on anything although I guess there will always be an sa account and a finding.
                                                        IF (LEN(@severityvalues) > 0)
                                                                SELECT
                                                                        @sql = @sql
                                                                        + N'		and name not in ('
                                                                        + @severityvalues
                                                                        + N')';

                                                        SELECT
                                                                @sql = @sql
                                                                + N' order by name';

                                                        EXEC (@sql);

                                                        OPEN logincursor;
                                                        FETCH NEXT FROM
                                                        logincursor INTO @strval;

                                                        SELECT
                                                                @intval2 = 0;
                                                        WHILE @@fetch_status = 0
                                                        BEGIN
                                                                IF (@intval2 = 1
                                                                        OR LEN(@metricval)
                                                                        + LEN(@strval) > 1010
                                                                        )
                                                                BEGIN
                                                                        IF @intval2 = 0
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + N', more...',
                                                                                        @intval2 = 1;
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @metricval = @metricval
                                                                                + CASE
                                                                                        WHEN LEN(@metricval) > 0 THEN N', '
                                                                                        ELSE N''
                                                                                END + N''''
                                                                                + @strval
                                                                                + N'''';

                                                                IF (@isadmin = 1)
                                                                        INSERT INTO policyassessmentdetail (policyid,
                                                                        assessmentid,
                                                                        metricid,
                                                                        snapshotid,
                                                                        detailfinding,
                                                                        databaseid,
                                                                        objecttype,
                                                                        objectid,
                                                                        objectname)
                                                                                VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Unauthorized SQL Login found: ''' + @strval + N'''', NULL, -- database ID,
                                                                                N'iLOGN', -- object type
                                                                                NULL, @strval);

                                                                FETCH NEXT FROM
                                                                logincursor INTO @strval;
                                                        END;

                                                        CLOSE logincursor;
                                                        DEALLOCATE logincursor;

                                                        IF (LEN(@metricval) = 0)
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = 'No unauthorized SQL Logins exist.';
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity,
                                                                        @metricval = N'Unauthorized SQL Logins exist: '
                                                                        + @metricval;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if SQL Logins exist that are not '
                                                                + @severityvalues;
                                                END;

                                                -- Public Server Role Has Permissions
                                                ELSE
                                                IF (@metricid = 85)
                                                BEGIN
                                                        -- only apply this check if the version is 2005 or greater
                                                        IF (@version > N'9.'
                                                                OR @version < N'6.'
                                                                )
                                                        BEGIN
                                                                DECLARE permissioncursor CURSOR FOR
                                                                SELECT
                                                                        a.permission,
                                                                        -- note this matches the functionality of isp_sqlsecure_getserverprincipalpermission for returning the object name
                                                                        CASE
                                                                                WHEN classid = 101 THEN dbo.getclasstype(a.classid)
                                                                                        + N' '''
                                                                                        + (SELECT
                                                                                                name
                                                                                        FROM serverprincipal
                                                                                        WHERE snapshotid = @snapshotid
                                                                                        AND principalid = a.majorid)
                                                                                        + N''''
                                                                                WHEN classid = 105 THEN dbo.getclasstype(a.classid)
                                                                                        + N' '''
                                                                                        + (SELECT
                                                                                                name
                                                                                        FROM endpoint
                                                                                        WHERE snapshotid = @snapshotid
                                                                                        AND endpointid = a.majorid)
                                                                                        + N''''
                                                                                ELSE 'Server'
                                                                        END
                                                                FROM serverpermission a,
                                                                     serverprincipal b
                                                                WHERE a.snapshotid = @snapshotid
                                                                AND a.snapshotid = b.snapshotid
                                                                AND a.grantee = b.principalid
                                                                AND a.grantee = 2		--	principalid of public server role is 2
                                                                ORDER BY a.permission;

                                                                EXEC (@sql);
                                                                OPEN permissioncursor;
                                                                FETCH NEXT FROM
                                                                permissioncursor INTO @strval,
                                                                @strval2;

                                                                SELECT
                                                                        @intval2 = 0;
                                                                WHILE @@fetch_status = 0
                                                                BEGIN
                                                                        IF (@intval2 = 1
                                                                                OR LEN(@metricval)
                                                                                + LEN(@strval) > 1400
                                                                                )
                                                                        BEGIN
                                                                                IF @intval2 = 0
                                                                                        SELECT
                                                                                                @metricval = @metricval
                                                                                                + N', more...',
                                                                                                @intval2 = 1;
                                                                        END;
                                                                        ELSE
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + CASE
                                                                                                WHEN LEN(@metricval) > 0 THEN N', '
                                                                                                ELSE N''
                                                                                        END + N''
                                                                                        + @strval
                                                                                        + N' on '
                                                                                        + @strval2;

                                                                        IF (@isadmin = 1)
                                                                                INSERT INTO policyassessmentdetail (policyid,
                                                                                assessmentid,
                                                                                metricid,
                                                                                snapshotid,
                                                                                detailfinding,
                                                                                databaseid,
                                                                                objecttype,
                                                                                objectid,
                                                                                objectname)
                                                                                        VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Public Permission found: ''' + @strval + N''' on ''' + @strval2 + N'''', NULL, -- database ID,
                                                                                        N'iSRV', -- object type
                                                                                        NULL, @strval);

                                                                        FETCH NEXT FROM
                                                                        permissioncursor INTO @strval,
                                                                        @strval2;
                                                                END;

                                                                CLOSE permissioncursor;
                                                                DEALLOCATE permissioncursor;

                                                                IF (LEN(@metricval) = 0)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = 'No permissions found.';
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @severity,
                                                                                @metricval = N'Public has permissions: '
                                                                                + @metricval;
                                                        END;
                                                        ELSE
                                                        BEGIN
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'The public server role does not exist on SQL Server 2000.';
                                                        END;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if the public server role has been granted permissions.';
                                                END;

                                                -- Databases Are Trustworthy
                                                ELSE
                                                IF (@metricid = 86)
                                                BEGIN
                                                        SELECT
                                                                @sql = N'declare dbcursor cursor static for
												select databasename
													from sqldatabase
													where snapshotid = '
                                                                + CONVERT(nvarchar, @snapshotid)
                                                                + N'
														and trustworthy = ''Y''
														and databasename not in ('
                                                                + @severityvalues
                                                                + N')
													order by databasename';
                                                        EXEC (@sql);
                                                        OPEN dbcursor;
                                                        SELECT
                                                                @intval2 = 0;
                                                        FETCH NEXT FROM
                                                        dbcursor INTO @strval;
                                                        WHILE @@fetch_status = 0
                                                        BEGIN
                                                                IF (@intval2 = 1
                                                                        OR LEN(@metricval)
                                                                        + LEN(@strval) > 1010
                                                                        )
                                                                BEGIN
                                                                        IF @intval2 = 0
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + N', more...',
                                                                                        @intval2 = 1;
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @metricval = @metricval
                                                                                + CASE
                                                                                        WHEN LEN(@metricval) > 0 THEN N', '
                                                                                        ELSE N''
                                                                                END + N''''
                                                                                + @strval
                                                                                + N'''';

                                                                IF (@isadmin = 1)
                                                                        INSERT INTO policyassessmentdetail (policyid,
                                                                        assessmentid,
                                                                        metricid,
                                                                        snapshotid,
                                                                        detailfinding,
                                                                        databaseid,
                                                                        objecttype,
                                                                        objectid,
                                                                        objectname)
                                                                                VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Database with Trustworthy bit enabled: ''' + @strval + N'''', NULL, -- database ID,
                                                                                N'DB', -- object type
                                                                                NULL, -- object id
                                                                                @strval);

                                                                FETCH NEXT FROM
                                                                dbcursor INTO @strval;
                                                        END;

                                                        IF (LEN(@metricval) = 0)
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'None found.';
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity,
                                                                        @metricval = N'Trustworthy databases: '
                                                                        + @metricval;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if any SQL Server 2005 or later databases are trustworthy other than: '
                                                                + @severityvalues;

                                                        CLOSE dbcursor;
                                                        DEALLOCATE dbcursor;
                                                END;

                                                -- Sysadmins Own Databases
                                                ELSE
                                                IF (@metricid = 87)
                                                BEGIN
                                                        SELECT
                                                                @sql = N'declare dbcursor cursor static for
												select databasename
													from sqldatabase
													where snapshotid = '
                                                                + CONVERT(nvarchar, @snapshotid)
                                                                + N'
														and owner in (select name from #sysadminstbl)
														and databasename not in ('
                                                                + @severityvalues
                                                                + N')
													order by databasename';
                                                        EXEC (@sql);
                                                        OPEN dbcursor;
                                                        SELECT
                                                                @intval2 = 0;
                                                        FETCH NEXT FROM
                                                        dbcursor INTO @strval;
                                                        WHILE @@fetch_status = 0
                                                        BEGIN
                                                                IF (@intval2 = 1
                                                                        OR LEN(@metricval)
                                                                        + LEN(@strval) > 1010
                                                                        )
                                                                BEGIN
                                                                        IF @intval2 = 0
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + N', more...',
                                                                                        @intval2 = 1;
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @metricval = @metricval
                                                                                + CASE
                                                                                        WHEN LEN(@metricval) > 0 THEN N', '
                                                                                        ELSE N''
                                                                                END + N''''
                                                                                + @strval
                                                                                + N'''';

                                                                IF (@isadmin = 1)
                                                                        INSERT INTO policyassessmentdetail (policyid,
                                                                        assessmentid,
                                                                        metricid,
                                                                        snapshotid,
                                                                        detailfinding,
                                                                        databaseid,
                                                                        objecttype,
                                                                        objectid,
                                                                        objectname)
                                                                                VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Database owned by sysadmin: ''' + @strval + N'''', NULL, -- database ID,
                                                                                N'DB', -- object type
                                                                                NULL, -- object id
                                                                                @strval);

                                                                FETCH NEXT FROM
                                                                dbcursor INTO @strval;
                                                        END;

                                                        IF (LEN(@metricval) = 0)
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'None found.';
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity,
                                                                        @metricval = N'Databases owned by system administrators: '
                                                                        + @metricval;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if a login who is a member of sysadmin or has the CONTROL SERVER permission owns any databases other than: '
                                                                + @severityvalues;

                                                        CLOSE dbcursor;
                                                        DEALLOCATE dbcursor;
                                                END;

                                                -- Sysadmins Own Trustworthy Databases
                                                ELSE
                                                IF (@metricid = 88)
                                                BEGIN
                                                        SELECT
                                                                @sql = N'declare dbcursor cursor static for
												select databasename
													from sqldatabase
													where snapshotid = '
                                                                + CONVERT(nvarchar, @snapshotid)
                                                                + N'
														and trustworthy = ''Y''
														and owner in (select name from #sysadminstbl)
														and databasename not in ('
                                                                + @severityvalues
                                                                + N')
													order by databasename';
                                                        EXEC (@sql);
                                                        OPEN dbcursor;
                                                        SELECT
                                                                @intval2 = 0;
                                                        FETCH NEXT FROM
                                                        dbcursor INTO @strval;
                                                        WHILE @@fetch_status = 0
                                                        BEGIN
                                                                IF (@intval2 = 1
                                                                        OR LEN(@metricval)
                                                                        + LEN(@strval) > 1010
                                                                        )
                                                                BEGIN
                                                                        IF @intval2 = 0
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + N', more...',
                                                                                        @intval2 = 1;
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @metricval = @metricval
                                                                                + CASE
                                                                                        WHEN LEN(@metricval) > 0 THEN N', '
                                                                                        ELSE N''
                                                                                END + N''''
                                                                                + @strval
                                                                                + N'''';

                                                                IF (@isadmin = 1)
                                                                        INSERT INTO policyassessmentdetail (policyid,
                                                                        assessmentid,
                                                                        metricid,
                                                                        snapshotid,
                                                                        detailfinding,
                                                                        databaseid,
                                                                        objecttype,
                                                                        objectid,
                                                                        objectname)
                                                                                VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Trustworthy Database owned by sysadmin: ''' + @strval + N'''', NULL, -- database ID,
                                                                                N'DB', -- object type
                                                                                NULL, -- object id
                                                                                @strval);

                                                                FETCH NEXT FROM
                                                                dbcursor INTO @strval;
                                                        END;

                                                        IF (LEN(@metricval) = 0)
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'None found.';
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity,
                                                                        @metricval = N'Databases owned by system administrators: '
                                                                        + @metricval;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if a login who is a member of sysadmin or has the CONTROL SERVER permission owns any trustworthy databases other than: '
                                                                + @severityvalues;

                                                        CLOSE dbcursor;
                                                        DEALLOCATE dbcursor;
                                                END;

                                                -- Public Roles Have Permissions on User Databases
                                                ELSE
                                                IF (@metricid = 89)
                                                BEGIN
                                                        SELECT
                                                                @severityvalues = N'Y';
                                                        DECLARE databasecursor CURSOR FOR
                                                        SELECT DISTINCT
                                                                b.databasename
                                                        FROM vwdatabaseobjectpermission a
                                                        INNER JOIN sqldatabase b
                                                                ON ((a.dbid = b.dbid)
                                                                AND (a.snapshotid = b.snapshotid)
                                                                )
                                                                INNER JOIN databaseschema c
                                                                        ON ((a.schemaid = c.schemaid)
                                                                        AND (a.dbid = c.dbid)
                                                                        AND (a.snapshotid = c.snapshotid)
                                                                        )
                                                        WHERE a.snapshotid = @snapshotid
                                                        AND a.grantee = 0			--public uid is 0
                                                        AND (a.isgrant = N'Y'
                                                        OR a.isgrantwith = N'Y'
                                                        )
                                                        AND (b.databasename NOT IN (
                                                        N'master',
                                                        N'msdb',
                                                        N'distribution',
                                                        N'tempdb'))
                                                        AND (c.schemaname <> N'sys');
                                                        OPEN databasecursor;
                                                        FETCH NEXT FROM
                                                        databasecursor INTO @strval;

                                                        SELECT
                                                                @intval2 = 0;
                                                        WHILE @@fetch_status = 0
                                                        BEGIN
                                                                IF (@intval2 = 1
                                                                        OR LEN(@metricval)
                                                                        + LEN(@strval) > 1010
                                                                        )
                                                                BEGIN
                                                                        IF @intval2 = 0
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + N', more...',
                                                                                        @intval2 = 1;
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @metricval = @metricval
                                                                                + CASE
                                                                                        WHEN LEN(@metricval) > 0 THEN N', '
                                                                                        ELSE N''
                                                                                END + N''''
                                                                                + @strval
                                                                                + N'''';

                                                                IF (@isadmin = 1)
                                                                        INSERT INTO policyassessmentdetail (policyid,
                                                                        assessmentid,
                                                                        metricid,
                                                                        snapshotid,
                                                                        detailfinding,
                                                                        databaseid,
                                                                        objecttype,
                                                                        objectid,
                                                                        objectname)
                                                                                VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Public has permissions on ''' + @strval + N'''', NULL, -- database ID,
                                                                                N'DB', -- object type
                                                                                NULL, @strval);

                                                                FETCH NEXT FROM
                                                                databasecursor INTO @strval;
                                                        END;

                                                        CLOSE databasecursor;
                                                        DEALLOCATE databasecursor;

                                                        IF (LEN(@metricval) = 0)
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = 'No user databases found with public permissions.';
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity,
                                                                        @metricval = N'Public has permissions on '
                                                                        + @metricval;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if the public role has been granted permissions on user databases.';
                                                END;

                                                -- Dangerous Security Principals
                                                ELSE
                                                IF (@metricid = 90)
                                                BEGIN
                                                        DECLARE @memberid int;
                                                        SELECT
                                                                @sql = N'declare databasecursor cursor for
								select dpmember.uid, dpmember.name, dpgroup.name
								from databaserolemember drm
								inner join databaseprincipal dpgroup 
									on ((dpgroup.snapshotid = drm.snapshotid) and (dpgroup.uid = drm.groupuid) and (dpgroup.dbid = drm.dbid))
								inner join databaseprincipal dpmember 
									on ((dpmember.snapshotid = drm.snapshotid) and (dpmember.uid = drm.rolememberuid) and (dpmember.dbid = drm.dbid))
								where 
								(
									drm.snapshotid = '
                                                                + CONVERT(nvarchar, @snapshotid)
                                                                + N' 
									and drm.dbid = 4				-- 4 = msdb
									and dpgroup.name in (''db_ssisadmin'', ''db_ssisltduser'', ''db_ssisoperator'', ''db_dtsadmin'', ''db_dtsltduser'', ''db_dtsoperator'')
									and dpmember.name in (' + @severityvalues
                                                                + N')
								) 
								order by dpgroup.name';
                                                        EXEC (@sql);
                                                        OPEN databasecursor;
                                                        FETCH NEXT FROM
                                                        databasecursor INTO @memberid,
                                                        @strval2,
                                                        @strval3;

                                                        SELECT
                                                                @intval2 = 0;
                                                        WHILE @@fetch_status = 0
                                                        BEGIN
                                                                SELECT
                                                                        @strval = @strval2
                                                                        + ' in '
                                                                        + @strval3;
                                                                IF (@intval2 = 1
                                                                        OR LEN(@metricval)
                                                                        + LEN(@strval) > 1010
                                                                        )
                                                                BEGIN
                                                                        IF @intval2 = 0
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + N', more...',
                                                                                        @intval2 = 1;
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @metricval = @metricval
                                                                                + CASE
                                                                                        WHEN LEN(@metricval) > 0 THEN N', '
                                                                                        ELSE N''
                                                                                END + N''''
                                                                                + @strval
                                                                                + N'''';

                                                                IF (@isadmin = 1)
                                                                        INSERT INTO policyassessmentdetail (policyid,
                                                                        assessmentid,
                                                                        metricid,
                                                                        snapshotid,
                                                                        detailfinding,
                                                                        databaseid,
                                                                        objecttype,
                                                                        objectid,
                                                                        objectname)
                                                                                VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Dangerous security principals found in SSIS database roles: ''' + @strval + N'''', NULL, -- database ID,
                                                                                N'iDUSR', -- object type
                                                                                @memberid, @strval);

                                                                FETCH NEXT FROM
                                                                databasecursor INTO @memberid,
                                                                @strval2,
                                                                @strval3;
                                                        END;

                                                        CLOSE databasecursor;
                                                        DEALLOCATE databasecursor;

                                                        IF (LEN(@metricval) = 0)
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = 'No dangerous members found in SSIS security roles.';
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity,
                                                                        @metricval = N'Dangerous security principals found in SSIS database roles: '
                                                                        + @metricval;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if dangerous security principals have been added to SSIS database roles.';
                                                END;

                                                -- Integration Services Roles Permissions Not Acceptable
                                                ELSE
                                                IF (@metricid = 91)
                                                BEGIN
                                                        SELECT
                                                                @sql = N'declare databasecursor cursor for
											select vdop.objectname, dp.name
											from [dbo].[vwdatabaseobjectpermission] vdop
											inner join databaseprincipal dp 
												on ((vdop.snapshotid = dp.snapshotid) and (vdop.dbid = dp.dbid) and (vdop.grantee = dp.uid))
											where 
											(
												(vdop.snapshotid = '
                                                                + CONVERT(nvarchar, @snapshotid)
                                                                + N') 
												and ((vdop.isgrant = N''Y'') or (vdop.isgrantwith = N''Y''))
												and (vdop.objectname in ('
                                                                + @severityvalues
                                                                + N'))
												and dp.type IN (''R'', ''A'')
												and (dp.name not in (''db_dtsadmin'', ''db_dtsltduser'', ''db_dtsoperator'', ''db_ssisadmin'', ''db_ssisltduser'', ''db_ssisoperator''))
											)';
                                                        EXEC (@sql);
                                                        OPEN databasecursor;
                                                        FETCH NEXT FROM
                                                        databasecursor INTO @strval2,
                                                        @strval3;

                                                        SELECT
                                                                @intval2 = 0;
                                                        WHILE @@fetch_status = 0
                                                        BEGIN
                                                                SELECT
                                                                        @strval = @strval3
                                                                        + ' on '
                                                                        + @strval2;
                                                                IF (@intval2 = 1
                                                                        OR LEN(@metricval)
                                                                        + LEN(@strval) > 1010
                                                                        )
                                                                BEGIN
                                                                        IF @intval2 = 0
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + N', more...',
                                                                                        @intval2 = 1;
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @metricval = @metricval
                                                                                + CASE
                                                                                        WHEN LEN(@metricval) > 0 THEN N', '
                                                                                        ELSE N''
                                                                                END + N''''
                                                                                + @strval
                                                                                + N'''';

                                                                IF (@isadmin = 1)
                                                                        INSERT INTO policyassessmentdetail (policyid,
                                                                        assessmentid,
                                                                        metricid,
                                                                        snapshotid,
                                                                        detailfinding,
                                                                        databaseid,
                                                                        objecttype,
                                                                        objectid,
                                                                        objectname)
                                                                                VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Permissions on stored procedures found: ''' + @strval + N'''', NULL, -- database ID,
                                                                                N'DB', -- object type
                                                                                NULL, @strval);

                                                                FETCH NEXT FROM
                                                                databasecursor INTO @strval2,
                                                                @strval3;
                                                        END;

                                                        CLOSE databasecursor;
                                                        DEALLOCATE databasecursor;

                                                        IF (LEN(@metricval) = 0)
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = 'No unacceptable permissions found.';
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity,
                                                                        @metricval = N'Permissions on stored procedures found: '
                                                                        + @metricval;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if users other than the default SSIS database roles have been granted permissions on an Integration Services stored procedure.';
                                                END;
                                                --Weak Passwords
                                                ELSE
                                                IF (@metricid = 92)
                                                BEGIN
                                                        SELECT
                                                                @severityvalues = N'Y';
                                                        DECLARE logincursor CURSOR FOR
                                                        SELECT
                                                                name
                                                        FROM vwserverprincipal
                                                        WHERE snapshotid = @snapshotid
                                                        AND type = N'S'
                                                        AND passwordstatus > 0;

                                                        OPEN logincursor;
                                                        FETCH NEXT FROM
                                                        logincursor INTO @strval;

                                                        SELECT
                                                                @intval2 = 0;
                                                        WHILE @@fetch_status = 0
                                                        BEGIN
                                                                IF (@intval2 = 1
                                                                        OR LEN(@metricval)
                                                                        + LEN(@strval) > 1010
                                                                        )
                                                                BEGIN
                                                                        IF @intval2 = 0
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + N', more...',
                                                                                        @intval2 = 1;
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @metricval = @metricval
                                                                                + CASE
                                                                                        WHEN LEN(@metricval) > 0 THEN N', '
                                                                                        ELSE N''
                                                                                END + N''''
                                                                                + @strval
                                                                                + N'''';

                                                                IF (@isadmin = 1)
                                                                        INSERT INTO policyassessmentdetail (policyid,
                                                                        assessmentid,
                                                                        metricid,
                                                                        snapshotid,
                                                                        detailfinding,
                                                                        databaseid,
                                                                        objecttype,
                                                                        objectid,
                                                                        objectname)
                                                                                VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Login with weak password found: ''' + @strval + N'''', NULL, -- database ID,
                                                                                N'iLOGN', -- object type
                                                                                NULL, @strval);

                                                                FETCH NEXT FROM
                                                                logincursor INTO @strval;
                                                        END;

                                                        CLOSE logincursor;
                                                        DEALLOCATE logincursor;

                                                        IF (LEN(@metricval) = 0)
                                                        BEGIN
                                                                IF (@weakpasswordenabled = 'N')
                                                                BEGIN
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = @metricval
                                                                                + N'   Weak password detection was disabled during data collection.';
                                                                END;
                                                                ELSE
                                                                BEGIN
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = N'No SQL Logins have weak passwords.';
                                                                END;
                                                        END;
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity,
                                                                        @metricval = N'These SQL Logins have weak passwords: '
                                                                        + @metricval;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if SQL Logins are found that have a weak password.';
                                                END;
                                                IF (@metricid = 93)
                                                BEGIN
                                                        TRUNCATE TABLE #tempdetails;

                                                        IF (@isadmin = 1)
                                                        BEGIN
                                                                INSERT INTO #tempdetails
                                                                        SELECT
                                                                                @policyid,
                                                                                @assessmentid,
                                                                                @metricid,
                                                                                @snapshotid,
                                                                                N'Symmetric key '''
                                                                                + db.name
                                                                                + N''' was found in system database ',
                                                                                db.dbid,
                                                                                db.type,
                                                                                db.objectid,
                                                                                db.name
                                                                        FROM dbo.databaseobject db
                                                                        JOIN dbo.sqldatabase sd
                                                                                ON db.snapshotid = sd.snapshotid
                                                                                AND db.dbid = sd.dbid
                                                                        WHERE db.type = 'isk'
                                                                        AND db.snapshotid = @snapshotid
                                                                        AND sd.databasename IN (
                                                                        'msdb', 'master',
                                                                        'model',
                                                                        'tempdb');

                                                                IF NOT EXISTS (SELECT
                                                                                *
                                                                        FROM #tempdetails)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = N'None found.';
                                                                ELSE
                                                                BEGIN
                                                                        SELECT
                                                                                @metricval = @metricval
                                                                                + objectname
                                                                                + ', '
                                                                        FROM #tempdetails;

                                                                        SET @metricval = SUBSTRING(@metricval,
                                                                        0,
                                                                        LEN(@metricval));

                                                                        INSERT INTO policyassessmentdetail
                                                                                SELECT
                                                                                        policyid,
                                                                                        assessmentid,
                                                                                        metricid,
                                                                                        snapshotid,
                                                                                        detailfinding,
                                                                                        databaseid,
                                                                                        objecttype,
                                                                                        objectid,
                                                                                        objectname
                                                                                FROM #tempdetails;

                                                                        SELECT
                                                                                @sevcode = @severity,
                                                                                @metricval = N'Next symmetric keys found in system databases : '
                                                                                + @metricval;
                                                                END;
                                                        END;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if system database have symmetric key ';
                                                END;

                                                ELSE
                                                IF (@metricid = 94)
                                                BEGIN

                                                        TRUNCATE TABLE #tempdetails;

                                                        IF (@isadmin = 1)
                                                        BEGIN
                                                                INSERT INTO #tempdetails
                                                                        SELECT
                                                                                @policyid,
                                                                                @assessmentid,
                                                                                @metricid,
                                                                                @snapshotid,
                                                                                N'User defined assembly with unsafe access  '''
                                                                                + db.name
                                                                                + N''' was found in '
                                                                                + sd.databasename,
                                                                                db.dbid,
                                                                                db.type,
                                                                                db.objectid,
                                                                                db.name
                                                                        FROM dbo.databaseobject db
                                                                        JOIN dbo.sqldatabase sd
                                                                                ON db.snapshotid = sd.snapshotid
                                                                                AND db.dbid = sd.dbid
                                                                        WHERE db.type = 'iasm'
                                                                        AND db.snapshotid = @snapshotid
                                                                        AND db.userdefined = 'y'
                                                                        AND permission_set <> 1;

                                                                IF NOT EXISTS (SELECT
                                                                                *
                                                                        FROM #tempdetails)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = N'None found.';
                                                                ELSE
                                                                BEGIN
                                                                        SELECT
                                                                                @metricval = @metricval
                                                                                + objectname
                                                                                + ', '
                                                                        FROM #tempdetails;

                                                                        SET @metricval = SUBSTRING(@metricval,
                                                                        0,
                                                                        LEN(@metricval));

                                                                        INSERT INTO policyassessmentdetail
                                                                                SELECT
                                                                                        policyid,
                                                                                        assessmentid,
                                                                                        metricid,
                                                                                        snapshotid,
                                                                                        detailfinding,
                                                                                        databaseid,
                                                                                        objecttype,
                                                                                        objectid,
                                                                                        objectname
                                                                                FROM #tempdetails;

                                                                        SELECT
                                                                                @sevcode = @severity,
                                                                                @metricval = N'Next user defined assemblies with host policy other then safe was found : '
                                                                                + @metricval;
                                                                END;
                                                        END;


                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if there are user defined assemblies with host policy other than SAFE ';
                                                END;

                                                ELSE
                                                IF (@metricid = 95)
                                                BEGIN

                                                        TRUNCATE TABLE #tempdetails;

                                                        IF (@isadmin = 1)
                                                        BEGIN

                                                                DECLARE @currServerAuthMode nvarchar(1);

                                                                SELECT
                                                                        @currServerAuthMode = authenticationmode
                                                                FROM dbo.registeredserver
                                                                WHERE registeredserverid = @registeredserverid;

                                                                INSERT INTO #tempdetails
                                                                        SELECT
                                                                                @policyid,
                                                                                @assessmentid,
                                                                                @metricid,
                                                                                @snapshotid,
                                                                                N'Next contained databases was found  '''
                                                                                + db.name,
                                                                                db.dbid,
                                                                                db.type,
                                                                                db.objectid,
                                                                                db.name
                                                                        FROM dbo.databaseobject db
                                                                        JOIN dbo.sqldatabase sd
                                                                                ON db.snapshotid = sd.snapshotid
                                                                                AND db.dbid = sd.dbid
                                                                        WHERE @currServerAuthMode = 'M'
                                                                        AND db.type = 'DB'
                                                                        AND db.snapshotid = @snapshotid
                                                                        AND ISNULL(sd.IsContained,
                                                                        0) = 1;

                                                                IF NOT EXISTS (SELECT
                                                                                *
                                                                        FROM #tempdetails)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = N'None found.';
                                                                ELSE
                                                                BEGIN
                                                                        SET @metricval = 'Sql server authentication mode set to Mixed but contained databases exist on instance';

                                                                        INSERT INTO policyassessmentdetail
                                                                                SELECT
                                                                                        policyid,
                                                                                        assessmentid,
                                                                                        metricid,
                                                                                        snapshotid,
                                                                                        detailfinding,
                                                                                        databaseid,
                                                                                        objecttype,
                                                                                        objectid,
                                                                                        objectname
                                                                                FROM #tempdetails;

                                                                        SELECT
                                                                                @sevcode = @severity;


                                                                END;
                                                        END;


                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if there are contained databases and authentication mode set to Mixed ';
                                                END;


                                                ELSE
                                                IF (@metricid = 96)
                                                BEGIN

                                                        TRUNCATE TABLE #tempdetails;

                                                        IF (@isadmin = 1)
                                                        BEGIN

                                                                ;
                                                                WITH users (objectid, name, isOwner, snapshotid, userid, dbid, type)
                                                                AS (SELECT
                                                                        db.objectid,
                                                                        db.name,
                                                                        0 AS IsOwner,
                                                                        db.snapshotid,
                                                                        dp.grantee,
                                                                        db.dbid,
                                                                        db.type
                                                                FROM dbo.databaseobject db
                                                                JOIN dbo.databaseobjectpermission dp
                                                                        ON db.snapshotid = dp.snapshotid
                                                                        AND db.dbid = dp.dbid
                                                                        AND db.classid = dp.classid
                                                                        AND db.parentobjectid = dp.parentobjectid
                                                                        AND db.objectid = dp.objectid
                                                                        AND dp.permission = 'EXECUTE'
                                                                WHERE db.type IN ('P',
                                                                'X')
                                                                AND runatstartup IS NOT NULL
                                                                AND runatstartup = 'y'
                                                                AND db.snapshotid = @snapshotid
                                                                UNION
                                                                SELECT
                                                                        db.objectid,
                                                                        db.name,
                                                                        1 AS IsOwner,
                                                                        db.snapshotid,
                                                                        db.owner,
                                                                        db.dbid,
                                                                        db.type
                                                                FROM dbo.databaseobject db
                                                                WHERE db.type IN ('P',
                                                                'X')
                                                                AND runatstartup IS NOT NULL
                                                                AND runatstartup = 'y'
                                                                AND db.snapshotid = @snapshotid),
                                                                UserRoles (userId, dbid, userDBname, userDBRole, userLogin, userServerRole, snapshotid)
                                                                AS (SELECT
                                                                        m.uid,
                                                                        m.dbid,
                                                                        m.name,
                                                                        r.name,
                                                                        sm.name,
                                                                        sr.name,
                                                                        m.snapshotid
                                                                FROM databaseprincipal m
                                                                JOIN databaserolemember
                                                                AS rm
                                                                        ON m.snapshotid = rm.snapshotid
                                                                        AND m.dbid = rm.dbid
                                                                        AND m.uid = rm.rolememberuid
                                                                        JOIN databaseprincipal
                                                                        AS r
                                                                                ON rm.snapshotid = r.snapshotid
                                                                                AND rm.dbid = r.dbid
                                                                                AND rm.groupuid = r.uid
                                                                        JOIN dbo.serverprincipal sm
                                                                                ON m.snapshotid = sm.snapshotid
                                                                                AND m.usersid = sm.sid
                                                                        JOIN dbo.serverrolemember srm
                                                                                ON sm.snapshotid = srm.snapshotid
                                                                                AND sm.principalid = srm.memberprincipalid
                                                                        JOIN dbo.serverprincipal sr
                                                                                ON rm.snapshotid = sr.snapshotid
                                                                                AND srm.principalid = sr.principalid
                                                                WHERE sr.sid = 0x03
                                                                AND m.snapshotid = @snapshotid)
                                                                INSERT INTO #tempdetails
                                                                        SELECT
                                                                                @policyid,
                                                                                @assessmentid,
                                                                                @metricid,
                                                                                @snapshotid,
                                                                                CASE u.isOwner
                                                                                        WHEN 1 THEN N'Startup stored procedure '
                                                                                                + u.name
                                                                                                + 'are owned by user without sysadmin permissions '
                                                                                        WHEN 0 THEN N'Startup stored procedure '
                                                                                                + u.name
                                                                                                + 'can be executed by user without sysadmin permissions '
                                                                                END,
                                                                                u.dbid,
                                                                                u.type,
                                                                                u.objectid,
                                                                                u.name
                                                                        FROM users u
                                                                        WHERE u.userid NOT IN (SELECT
                                                                                userId
                                                                        FROM UserRoles us
                                                                        WHERE us.dbid = u.dbid)
                                                                        GROUP BY u.isOwner,
                                                                                 u.dbid,
                                                                                 u.type,
                                                                                 u.objectid,
                                                                                 u.name;



                                                                IF NOT EXISTS (SELECT
                                                                                *
                                                                        FROM #tempdetails)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = N'None found.';
                                                                ELSE
                                                                BEGIN
                                                                        SET @metricval = 'Next stored procedure can be run or are owned by accounts without sysadmin permissions ';

                                                                        SELECT
                                                                                @metricval = @metricval
                                                                                + objectname
                                                                                + ', '
                                                                        FROM #tempdetails;

                                                                        SET @metricval = SUBSTRING(@metricval,
                                                                        0,
                                                                        LEN(@metricval));

                                                                        INSERT INTO policyassessmentdetail
                                                                                SELECT
                                                                                        policyid,
                                                                                        assessmentid,
                                                                                        metricid,
                                                                                        snapshotid,
                                                                                        detailfinding,
                                                                                        databaseid,
                                                                                        objecttype,
                                                                                        objectid,
                                                                                        objectname
                                                                                FROM #tempdetails;
                                                                        SELECT
                                                                                @sevcode = @severity;


                                                                END;
                                                        END;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if startup stored procedures can be run or are owned by accounts without sysadmin permissions ';
                                                END;


                                                ELSE
                                                IF (@metricid = 97)
                                                BEGIN

                                                        TRUNCATE TABLE #tempdetails;
                                                        IF OBJECT_ID('tempdb..#sevrVals') IS NOT NULL
                                                        BEGIN
                                                                DROP TABLE #sevrVals;
                                                        END;

                                                        SELECT
                                                                Value INTO #sevrVals
                                                        FROM dbo.splitbydelimiter(@severityvalues,
                                                        ',');

                                                        IF EXISTS (SELECT
                                                                        1
                                                                FROM dbo.sqljob
                                                                WHERE SubSystem IN (SELECT
                                                                        Value
                                                                FROM #sevrVals)
                                                                AND SnapshotId = @snapshotid)
                                                        BEGIN
                                                                IF (@isadmin = 1)
                                                                BEGIN
                                                                        DECLARE @lname AS varchar(200);

                                                                        SELECT
                                                                                @lname = (SELECT
                                                                                        loginname
                                                                                FROM dbo.serverservice
                                                                                WHERE snapshotid = @snapshotid
                                                                                AND servicename = 'SQLSERVERAGENT');

                                                                        INSERT INTO #tempdetails
                                                                                SELECT
                                                                                        @policyid,
                                                                                        @assessmentid,
                                                                                        @metricid,
                                                                                        @snapshotid,
                                                                                        N'SQL Server Agent account '
                                                                                        + swa.name
                                                                                        + ' is a member of '
                                                                                        + smm.name ' group',
                                                                                        NULL,
                                                                                        'Acc',
                                                                                        NULL,
                                                                                        swa.name
                                                                                FROM dbo.serveroswindowsaccount swa
                                                                                JOIN dbo.serveroswindowsgroupmember ss
                                                                                        ON swa.snapshotid = ss.snapshotid
                                                                                        AND swa.sid = ss.groupmember
                                                                                        JOIN dbo.serveroswindowsaccount smm
                                                                                                ON ss.snapshotid = smm.snapshotid
                                                                                                AND ss.groupsid = smm.sid
                                                                                WHERE SUBSTRING(swa.name,
                                                                                CHARINDEX('\',
                                                                                swa.name) + 1,
                                                                                LEN(swa.name)) = SUBSTRING(@lname,
                                                                                CHARINDEX('\',
                                                                                @lname) + 1,
                                                                                LEN(@lname))
                                                                                AND swa.snapshotid = @snapshotid
                                                                                AND smm.name LIKE '%\Administrators';


                                                                        INSERT INTO #tempdetails
                                                                                SELECT
                                                                                        @policyid,
                                                                                        @assessmentid,
                                                                                        @metricid,
                                                                                        @snapshotid,
                                                                                        N'SQL Server Job proxy  '
                                                                                        + swa.name
                                                                                        + ' is a member of '
                                                                                        + smm.name ' group',
                                                                                        NULL,
                                                                                        'Acc',
                                                                                        NULL,
                                                                                        swa.name
                                                                                FROM dbo.sqljobproxy p
                                                                                JOIN dbo.serveroswindowsaccount swa
                                                                                        ON p.snapshotid = swa.snapshotid
                                                                                        AND swa.sid = p.usersid
                                                                                        JOIN dbo.serveroswindowsgroupmember ss
                                                                                                ON swa.snapshotid = ss.snapshotid
                                                                                                AND swa.sid = ss.groupmember
                                                                                        JOIN dbo.serveroswindowsaccount smm
                                                                                                ON ss.snapshotid = smm.snapshotid
                                                                                                AND ss.groupsid = smm.sid
                                                                                WHERE p.subsystem IN (SELECT
                                                                                        Value
                                                                                FROM #sevrVals)
                                                                                AND p.snapshotid = @snapshotid
                                                                                AND smm.name LIKE '%\Administrators'
                                                                                GROUP BY swa.name,
                                                                                         smm.name;


                                                                        IF NOT EXISTS (SELECT
                                                                                        *
                                                                                FROM #tempdetails)
                                                                                SELECT
                                                                                        @sevcode = @sevcodeok,
                                                                                        @metricval = N'None found.';
                                                                        ELSE
                                                                        BEGIN
                                                                                SET @metricval = 'Next accounts are in Administrators role  ';

                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + objectname
                                                                                        + ', '
                                                                                FROM #tempdetails
                                                                                GROUP BY objectname;

                                                                                SET @metricval = SUBSTRING(@metricval,
                                                                                0,
                                                                                LEN(@metricval));

                                                                                SET @metricval = @metricval
                                                                                + ' and can run sql job steps in '
                                                                                + @severityvalues
                                                                                + ' subsystems';

                                                                                INSERT INTO policyassessmentdetail
                                                                                        SELECT
                                                                                                policyid,
                                                                                                assessmentid,
                                                                                                metricid,
                                                                                                snapshotid,
                                                                                                detailfinding,
                                                                                                databaseid,
                                                                                                objecttype,
                                                                                                objectid,
                                                                                                objectname
                                                                                        FROM #tempdetails
                                                                                        GROUP BY policyid,
                                                                                                 assessmentid,
                                                                                                 metricid,
                                                                                                 snapshotid,
                                                                                                 detailfinding,
                                                                                                 databaseid,
                                                                                                 objecttype,
                                                                                                 objectid,
                                                                                                 objectname;
                                                                                SELECT
                                                                                        @sevcode = @severity;


                                                                        END;
                                                                END;
                                                        END;

                                                        ELSE
                                                        BEGIN
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'None found.';
                                                        END;
                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if sql job steps in  '
                                                                + @severityvalues
                                                                + ' subsystems are run by Administrators role members';
                                                END;

                                                --  DISTRIBUTOR_ADMIN account
                                                ELSE
                                                IF (@metricid = 98)
                                                BEGIN
                                                        SET @metricval = N'None found.';
                                                        SET @metricthreshold = N'Server is vulnerable if DISTRIBUTOR_ADMIN account exists when server is not distributor or DISTRIBUTOR_ADMIN account doesn''t follow password control standards when distributor server has a remote publisher.';
                                                        SET @sevcode = @sevcodeok;
                                                        IF EXISTS (SELECT
                                                                        SPU.name
                                                                FROM dbo.serverrolemember
                                                                AS SRM
                                                                INNER JOIN dbo.serverprincipal
                                                                AS SPU
                                                                        ON SRM.snapshotid = SPU.snapshotid
                                                                        AND SRM.memberprincipalid = SPU.principalid
                                                                        AND SPU.type <> 'R'
                                                                        INNER JOIN dbo.serverprincipal
                                                                        AS SPR
                                                                                ON SRM.snapshotid = SPR.snapshotid
                                                                                AND SRM.principalid = SPR.principalid
                                                                                AND SPR.type = 'R'
                                                                                AND SPR.name = 'sysadmin'
                                                                WHERE SRM.snapshotid = @snapshotid
                                                                AND SPU.name = 'distributor_admin')
                                                        BEGIN

                                                                DECLARE @IsDistributer nchar(1);
                                                                DECLARE @IsPublisher nchar(1);
                                                                DECLARE @HasRemotePublisher nchar(1);

                                                                SELECT
                                                                        @IsDistributer = SS.isdistributor,
                                                                        @IsPublisher = SS.ispublisher,
                                                                        @HasRemotePublisher = SS.hasremotepublisher
                                                                FROM dbo.serversnapshot
                                                                AS SS
                                                                WHERE SS.snapshotid = @snapshotid;

                                                                IF (@IsDistributer = 'N')
                                                                BEGIN
                                                                        SET @sevcode = @severity;
                                                                        SET @metricval = N'The DISTRIBUTOR_ADMIN account should be deleted as it is only needed at the distributor.';
                                                                END;
                                                                ELSE
                                                                IF (@IsPublisher = 'N'
                                                                        AND @HasRemotePublisher = 'Y'
                                                                        )
                                                                BEGIN
                                                                        SET @sevcode = @severity;
                                                                        SET @metricval = N'The password of DISTRIBUTOR_ADMIN login must be set according to password control standards using the "sp_changedistributor_password" stored procedure.';
                                                                END;
                                                        END;
                                                END;

                                                -- sysadmin accounts with local administrator role
                                                ELSE
                                                IF (@metricid = 99)
                                                BEGIN
                                                        DECLARE @output varchar(max);
                                                        DECLARE @output_delim varchar(2);
                                                        DECLARE @max_output_str_length int;
                                                        DECLARE @char_index_for_trim int;
                                                        SET @max_output_str_length = 1010;
                                                        SET @output = NULL;
                                                        SET @output_delim = ', ';
                                                        SET @char_index_for_trim = 0;

                                                        WITH SuppressedAccounts
                                                        AS (SELECT
                                                                Value
                                                        FROM dbo.splitbydelimiter(@severityvalues,
                                                        ','))
                                                        SELECT
                                                                @output = ISNULL(@output
                                                                + @output_delim,
                                                                '') + ''''
                                                                + SPU.name
                                                                + '''',
                                                                @char_index_for_trim =
                                                                                              CASE
                                                                                                      WHEN LEN(@output) < @max_output_str_length THEN LEN(@output)
                                                                                                      ELSE @char_index_for_trim
                                                                                              END
                                                        FROM dbo.serverrolemember
                                                        AS SRM
                                                        INNER JOIN dbo.serverprincipal
                                                        AS SPU
                                                                ON SRM.snapshotid = SPU.snapshotid
                                                                AND SRM.snapshotid = @snapshotid
                                                                AND SRM.memberprincipalid = SPU.principalid
                                                                AND SPU.type <> 'R'
                                                                INNER JOIN dbo.serverprincipal
                                                                AS SPR
                                                                        ON SRM.snapshotid = SPR.snapshotid
                                                                        AND SRM.principalid = SPR.principalid
                                                                        AND SPR.type = 'R'
                                                                        AND SPR.name = 'sysadmin'
                                                                INNER JOIN dbo.serveroswindowsgroupmember
                                                                AS WGM
                                                                        ON SPU.snapshotid = WGM.snapshotid
                                                                        AND SPU.sid = WGM.groupmember
                                                                INNER JOIN dbo.serveroswindowsaccount
                                                                AS WG
                                                                        ON WGM.snapshotid = WG.snapshotid
                                                                        AND WGM.groupsid = WG.sid
                                                                        AND WG.name LIKE '%\Administrators'
                                                        WHERE NOT EXISTS (SELECT
                                                                1
                                                        FROM SuppressedAccounts
                                                        AS sa
                                                        WHERE SPU.name LIKE sa.Value);

                                                        IF (LEN(@output) > @max_output_str_length)
                                                                SELECT
                                                                        @output = SUBSTRING(@output,
                                                                        1,
                                                                        @char_index_for_trim)
                                                                        + N', more...';


                                                        IF (@isadmin = 1)
                                                                INSERT INTO policyassessmentdetail (policyid,
                                                                assessmentid,
                                                                metricid,
                                                                snapshotid,
                                                                detailfinding,
                                                                databaseid,
                                                                objecttype,
                                                                objectid,
                                                                objectname)
                                                                        SELECT
                                                                                @policyid,
                                                                                @assessmentid,
                                                                                @metricid,
                                                                                @snapshotid,
                                                                                N'SQL SYSADMIN accounts that are in the local Administrator role: '''
                                                                                + Value + N'''',
                                                                                NULL, -- database ID,
                                                                                N'DB', -- object type
                                                                                NULL, -- object id
                                                                                Value
                                                                        FROM dbo.splitbydelimiter(@output,
                                                                        @output_delim);

                                                        IF (@output IS NULL)
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'None found.';
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity,
                                                                        @metricval = N'SQL SYSADMIN accounts that are in the local Administrator role: '
                                                                        + @output;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if SQL SYSADMIN accounts that are in the local Administrator role for the physical server other than: '
                                                                + @severityvalues;
                                                END;

                                                --information about database roles
                                                ELSE
                                                IF (@metricid = 100)
                                                BEGIN
                                                        SELECT
                                                                @severityvalues = N'Y';
                                                        DECLARE @RolePermissions TABLE (
                                                                rolename
                                                                nvarchar(256)
                                                                NULL,
                                                                rolepermission
                                                                nvarchar(max)
                                                                NULL
                                                        );

                                                        WITH RolePermisions
                                                        AS (SELECT
                                                                rolename,
                                                                (SELECT
                                                                        T1.rolepermission
                                                                        + ', ' AS [text()]
                                                                FROM dbo.fixedrolepermission
                                                                AS T1
                                                                WHERE T1.rolename = T2.rolename
                                                                ORDER BY T1.rolename
                                                                FOR
                                                                xml PATH (''))
                                                                AS rolepermissions
                                                        FROM (SELECT
                                                                rolename
                                                        FROM dbo.fixedrolepermission
                                                        GROUP BY rolename) AS T2)
                                                        INSERT INTO @RolePermissions
                                                                SELECT
                                                                        rolename,
                                                                        LEFT(rolepermissions,
                                                                        LEN(rolepermissions)
                                                                        - 1) AS rolepermissions
                                                                FROM RolePermisions;


                                                        DECLARE @DatabaseRoleUsers TABLE (
                                                                snapshotid int
                                                                NOT NULL,
                                                                dbid int
                                                                NOT NULL,
                                                                roleid
                                                                varbinary(85)
                                                                NULL,
                                                                username
                                                                nvarchar(128)
                                                                NOT NULL,
                                                                usertype
                                                                nvarchar(20)
                                                                NOT NULL,
                                                                groupname
                                                                nvarchar(200)
                                                                NULL,
                                                                rolepermissions
                                                                nvarchar(max)
                                                        );

                                                        INSERT INTO @DatabaseRoleUsers
                                                                SELECT
                                                                        DRM.snapshotid,
                                                                        DRM.dbid,
                                                                        DPR.uid AS roleid,
                                                                        DPU.name AS username,
                                                                        CASE
                                                                                WHEN DPU.type = 'U' THEN 'Windows User'
                                                                                WHEN DPU.type = 'G' THEN 'Windows Group'
                                                                                WHEN DPU.type = 'S' THEN 'SQL login'
																				--START(Barkha Khatri) For azure SQL DB -adding 2 new types
																				WHEN DPU.type = 'E' THEN 'External User'
																				WHEN DPU.type = 'X' THEN 'External Group'
																				--END(Barkha Khatri) For azure SQL DB -adding 2 new types
                                                                        END AS usertype,
                                                                        WG.name AS groupname,
                                                                        RP.rolepermission
                                                                FROM dbo.databaserolemember
                                                                AS DRM
                                                                INNER JOIN dbo.databaseprincipal
                                                                AS DPU
                                                                        ON DRM.snapshotid = DPU.snapshotid
                                                                        AND DRM.dbid = DPU.dbid
                                                                        AND DRM.rolememberuid = DPU.uid
                                                                        AND DPU.type <> 'R'
                                                                        INNER JOIN dbo.databaseprincipal
                                                                        AS DPR
                                                                                ON DRM.snapshotid = DPR.snapshotid
                                                                                AND DRM.dbid = DPR.dbid
                                                                                AND DRM.groupuid = DPR.uid
                                                                                AND DPR.type = 'R'
                                                                        LEFT JOIN dbo.serveroswindowsgroupmember
                                                                        AS WGM
                                                                                ON DPU.snapshotid = WGM.snapshotid
                                                                                AND DPU.usersid = WGM.groupmember
                                                                        LEFT JOIN dbo.serveroswindowsaccount
                                                                        AS WG
                                                                                ON WGM.snapshotid = WG.snapshotid
                                                                                AND WGM.groupsid = WG.sid
                                                                        LEFT JOIN @RolePermissions
                                                                        AS RP
                                                                                ON DPR.name = RP.rolename
                                                                ORDER BY DRM.snapshotid,
                                                                DRM.dbid;

                                                        DECLARE @DatabaseRolesInfo TABLE (
                                                                databasename
                                                                nvarchar(128),
                                                                rolename
                                                                nvarchar(128),
                                                                username
                                                                nvarchar(128),
                                                                usertype
                                                                nvarchar(20),
                                                                groupname
                                                                nvarchar(200),
                                                                rolepermissions
                                                                nvarchar(max)
                                                        );

                                                        INSERT INTO @DatabaseRolesInfo
                                                                SELECT
                                                                        DB.databasename,
                                                                        DPR.name,
                                                                        DRU.username,
                                                                        DRU.usertype,
                                                                        DRU.groupname,
                                                                        rolepermissions
                                                                FROM dbo.databaseprincipal
                                                                AS DPR
                                                                INNER JOIN dbo.sqldatabase
                                                                AS DB
                                                                        ON DPR.snapshotid = DB.snapshotid
                                                                        AND DPR.dbid = DB.dbid
                                                                        AND DPR.type = 'R'
                                                                        LEFT JOIN @DatabaseRoleUsers
                                                                        AS DRU
                                                                                ON DPR.snapshotid = DRU.snapshotid
                                                                                AND DPR.dbid = DRU.dbid
                                                                                AND DPR.uid = DRU.roleid
                                                                WHERE DPR.snapshotid = @snapshotid
                                                                ORDER BY DB.databasename,
                                                                DPR.name;

                                                        DECLARE @databasename nvarchar(128);
                                                        DECLARE @rolename nvarchar(128);
                                                        DECLARE @username nvarchar(128);
                                                        DECLARE @usertype nvarchar(20);
                                                        DECLARE @groupname nvarchar(200);
                                                        DECLARE @permissions nvarchar(max);
                                                        DECLARE @Delimiter nvarchar(10);

                                                        SELECT
                                                                @Delimiter = ', ';

                                                        DECLARE DatabaseRolesInfoCursor CURSOR FOR
                                                        SELECT
                                                                DRI.databasename,
                                                                DRI.rolename,
                                                                DRI.username,
                                                                DRI.usertype,
                                                                DRI.groupname,
                                                                DRI.rolepermissions
                                                        FROM @DatabaseRolesInfo
                                                        AS DRI;

                                                        OPEN DatabaseRolesInfoCursor;
                                                        FETCH NEXT FROM
                                                        DatabaseRolesInfoCursor INTO @databasename,
                                                        @rolename,
                                                        @username,
                                                        @usertype,
                                                        @groupname,
                                                        @permissions;

                                                        SELECT
                                                                @intval2 = 0;
                                                        WHILE @@fetch_status = 0
                                                        BEGIN

                                                                SET @strval = 'Database: '
                                                                + @databasename
                                                                + '; Role: '
                                                                + @rolename;

                                                                IF LEN(ISNULL(@username,
                                                                        '')) <> 0
                                                                BEGIN
                                                                        SET @strval = @strval
                                                                        + '; User: '
                                                                        + @username;
                                                                END;

                                                                IF LEN(ISNULL(@usertype,
                                                                        '')) <> 0
                                                                BEGIN
                                                                        SET @strval = @strval
                                                                        + '; Type: '
                                                                        + @usertype;
                                                                END;

                                                                IF LEN(ISNULL(@groupname,
                                                                        '')) <> 0
                                                                BEGIN
                                                                        SET @strval = @strval
                                                                        + '; Windows Group: '
                                                                        + @groupname;
                                                                END;

                                                                IF LEN(ISNULL(@permissions,
                                                                        '')) <> 0
                                                                BEGIN
                                                                        SET @strval = @strval
                                                                        + '; Permissions: '
                                                                        + @permissions;
                                                                END;

                                                                IF (@intval2 = 1
                                                                        OR LEN(@metricval)
                                                                        + LEN(@strval) > 1010
                                                                        )
                                                                BEGIN
                                                                        IF @intval2 = 0
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + N', more...',
                                                                                        @intval2 = 1;
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @metricval = @metricval
                                                                                + CASE
                                                                                        WHEN LEN(@metricval) > 0 THEN N', '
                                                                                        ELSE N''
                                                                                END + N''''
                                                                                + @strval
                                                                                + N'''';

                                                                IF (@isadmin = 1)
                                                                        INSERT INTO policyassessmentdetail (policyid,
                                                                        assessmentid,
                                                                        metricid,
                                                                        snapshotid,
                                                                        detailfinding,
                                                                        databaseid,
                                                                        objecttype,
                                                                        objectid,
                                                                        objectname)
                                                                                VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Database Role Info found: ''' + @strval + N'''', NULL, -- database ID,
                                                                                N'DB', -- object type
                                                                                NULL, N'Database Role Metric details');

                                                                FETCH NEXT FROM
                                                                DatabaseRolesInfoCursor INTO @databasename,
                                                                @rolename,
                                                                @username,
                                                                @usertype,
                                                                @groupname,
                                                                @permissions;
                                                        END;

                                                        CLOSE DatabaseRolesInfoCursor;
                                                        DEALLOCATE DatabaseRolesInfoCursor;
                                                        SELECT
                                                                @sevcode = @severity,
                                                                @metricval = N'The following database roles were found: '
                                                                + @metricval;
                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if database role info hasn''t been checked.';
                                                END;
                                                --						--information about server roles
                                                ELSE
                                                IF (@metricid = 101)
                                                BEGIN
                                                        SELECT
                                                                @severityvalues = N'Y';
                                                        DECLARE @ServerRoleUsers TABLE (
                                                                snapshotid int
                                                                NOT NULL,
                                                                roleprincipalid
                                                                varbinary(85)
                                                                NULL,
                                                                username
                                                                nvarchar(128)
                                                                NOT NULL,
                                                                usertype
                                                                nvarchar(20)
                                                                NOT NULL,
                                                                groupname
                                                                nvarchar(200)
                                                                NULL,
                                                                disabled
                                                                nvarchar(3)
                                                        );

                                                        INSERT INTO @ServerRoleUsers
                                                                SELECT
                                                                        --*
                                                                        SRM.snapshotid,
                                                                        SPR.principalid,
                                                                        SPU.name AS username,
                                                                        CASE
                                                                                WHEN SPU.type = 'U' THEN 'Windows User'
                                                                                WHEN SPU.type = 'G' THEN 'Windows Group'
                                                                                WHEN SPU.type = 'S' THEN 'SQL login'
																				--START(Barkha Khatri) For azure SQL DB -adding 2 new types
																				WHEN SPU.type = 'E' THEN 'External User'
																				WHEN SPU.type = 'X' THEN 'External Group'
																				--END(Barkha Khatri) For azure SQL DB -adding 2 new types
                                                                        END AS usertype,
                                                                        WG.name AS groupname,
                                                                        CASE
                                                                                WHEN SPU.disabled = 'Y' THEN 'Yes'
                                                                                ELSE 'No'
                                                                        END AS disabled
                                                                FROM dbo.serverrolemember
                                                                AS SRM
                                                                INNER JOIN dbo.serverprincipal
                                                                AS SPU
                                                                        ON SRM.snapshotid = SPU.snapshotid
                                                                        AND SRM.memberprincipalid = SPU.principalid
                                                                        AND SPU.type <> 'R'
                                                                        INNER JOIN dbo.serverprincipal
                                                                        AS SPR
                                                                                ON SRM.snapshotid = SPR.snapshotid
                                                                                AND SRM.principalid = SPR.principalid
                                                                                AND SPR.type = 'R'
                                                                        LEFT JOIN dbo.serveroswindowsgroupmember
                                                                        AS WGM
                                                                                ON SPU.snapshotid = WGM.snapshotid
                                                                                AND SPU.sid = WGM.groupmember
                                                                        LEFT JOIN dbo.serveroswindowsaccount
                                                                        AS WG
                                                                                ON WGM.snapshotid = WG.snapshotid
                                                                                AND WGM.groupsid = WG.sid
                                                                WHERE SRM.snapshotid = @snapshotid
                                                                ORDER BY SRM.snapshotid;

                                                        DECLARE @ServerRolesInfo TABLE (
                                                                instancename
                                                                nvarchar(400),
                                                                rolename
                                                                nvarchar(128),
                                                                username
                                                                nvarchar(128),
                                                                usertype
                                                                nvarchar(20),
                                                                groupname
                                                                nvarchar(200),
                                                                disabled
                                                                nvarchar(3)
                                                        );

                                                        INSERT INTO @ServerRolesInfo
                                                                SELECT
                                                                        CASE
                                                                                WHEN LEN(ISNULL(ST.instancename,
                                                                                        '')) <> 0 THEN ST.instancename
                                                                                WHEN LEN(ISNULL(ST.servername,
                                                                                        '')) <> 0 THEN ST.servername
                                                                                ELSE ST.connectionname
                                                                        END AS instancename,
                                                                        SP.name AS rolename,
                                                                        SRU.username,
                                                                        SRU.usertype,
                                                                        SRU.groupname,
                                                                        SRU.disabled
                                                                FROM dbo.serverprincipal
                                                                AS SP
                                                                INNER JOIN dbo.serversnapshot
                                                                AS ST
                                                                        ON SP.snapshotid = ST.snapshotid
                                                                        AND SP.type = 'R'
                                                                        LEFT JOIN @ServerRoleUsers
                                                                        AS SRU
                                                                                ON SP.snapshotid = SRU.snapshotid
                                                                                AND SP.principalid = SRU.roleprincipalid
                                                                WHERE SP.snapshotid = @snapshotid
                                                                ORDER BY instancename,
                                                                rolename;

                                                        DECLARE @Instancename nvarchar(400);
                                                        DECLARE @ServerRolename nvarchar(128);
                                                        DECLARE @ServerUsername nvarchar(128);
                                                        DECLARE @ServerUserType nvarchar(20);
                                                        DECLARE @WindowsGroupname nvarchar(200);
                                                        DECLARE @ServerUserDisabled nvarchar(3);

                                                        DECLARE ServerRolesInfoCursor CURSOR FOR
                                                        SELECT
                                                                SRI.instancename,
                                                                SRI.rolename,
                                                                SRI.username,
                                                                SRI.usertype,
                                                                SRI.groupname,
                                                                SRI.disabled
                                                        FROM @ServerRolesInfo
                                                        AS SRI;

                                                        OPEN ServerRolesInfoCursor;
                                                        FETCH NEXT FROM
                                                        ServerRolesInfoCursor INTO @Instancename,
                                                        @ServerRolename,
                                                        @ServerUsername,
                                                        @ServerUserType,
                                                        @WindowsGroupname,
                                                        @ServerUserDisabled;

                                                        SELECT
                                                                @intval2 = 0;
                                                        WHILE @@fetch_status = 0
                                                        BEGIN

                                                                SET @strval = 'SQL Instance: '
                                                                + @Instancename
                                                                + '; Server Role: '
                                                                + @ServerRolename;

                                                                IF LEN(ISNULL(@ServerUsername,
                                                                        '')) <> 0
                                                                BEGIN
                                                                        SET @strval = @strval
                                                                        + '; User: '
                                                                        + @ServerUsername;
                                                                END;

                                                                IF LEN(ISNULL(@ServerUserType,
                                                                        '')) <> 0
                                                                BEGIN
                                                                        SET @strval = @strval
                                                                        + '; Type: '
                                                                        + @ServerUserType;
                                                                END;

                                                                IF LEN(ISNULL(@WindowsGroupname,
                                                                        '')) <> 0
                                                                BEGIN
                                                                        SET @strval = @strval
                                                                        + '; Windows Group: '
                                                                        + @WindowsGroupname;
                                                                END;

                                                                IF LEN(ISNULL(@ServerUserDisabled,
                                                                        '')) <> 0
                                                                BEGIN
                                                                        SET @strval = @strval
                                                                        + '; Disabled: '
                                                                        + @ServerUserDisabled;
                                                                END;

                                                                IF (@intval2 = 1
                                                                        OR LEN(@metricval)
                                                                        + LEN(@strval) > 1010
                                                                        )
                                                                BEGIN
                                                                        IF @intval2 = 0
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + N', more...',
                                                                                        @intval2 = 1;
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @metricval = @metricval
                                                                                + CASE
                                                                                        WHEN LEN(@metricval) > 0 THEN N', '
                                                                                        ELSE N''
                                                                                END + N''''
                                                                                + @strval
                                                                                + N'''';

                                                                IF (@isadmin = 1)
                                                                        INSERT INTO policyassessmentdetail (policyid,
                                                                        assessmentid,
                                                                        metricid,
                                                                        snapshotid,
                                                                        detailfinding,
                                                                        databaseid,
                                                                        objecttype,
                                                                        objectid,
                                                                        objectname)
                                                                                VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Server Role Info found: ''' + @strval + N'''', NULL, -- database ID,
                                                                                N'DB', -- object type
                                                                                NULL, N'Server Role Metric details');

                                                                FETCH NEXT FROM
                                                                ServerRolesInfoCursor INTO @Instancename,
                                                                @ServerRolename,
                                                                @ServerUsername,
                                                                @ServerUserType,
                                                                @WindowsGroupname,
                                                                @ServerUserDisabled;
                                                        END;

                                                        CLOSE ServerRolesInfoCursor;
                                                        DEALLOCATE ServerRolesInfoCursor;
                                                        SELECT
                                                                @sevcode = @severity,
                                                                @metricval = N'The following server roles were found: '
                                                                + @metricval;
                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if server roles haven''t been checked.';
                                                END;
                                                ELSE
                                                IF (@metricid = 102)
                                                BEGIN

                                                        TRUNCATE TABLE #tempdetails;

                                                        IF EXISTS (SELECT
                                                                        1
                                                                FROM dbo.splitbydelimiter(@severityvalues,
                                                                ','))
                                                        BEGIN
                                                                IF (@isadmin = 1)
                                                                BEGIN

                                                                        INSERT INTO #tempdetails
                                                                                SELECT
                                                                                        @policyid,
                                                                                        @assessmentid,
                                                                                        @metricid,
                                                                                        @snapshotid,
                                                                                        N'Database   '
                                                                                        + sdb.databasename
                                                                                        + 'has next unacceptable  '
                                                                                        + sdb.owner,
                                                                                        sdb.dbid,
                                                                                        'DB',
                                                                                        sdb.dbid,
                                                                                        sdb.databasename
                                                                                FROM dbo.sqldatabase sdb
                                                                                JOIN dbo.splitbydelimiter(@severityvalues,
                                                                                ',') sp
                                                                                        ON sdb.owner LIKE '%'
                                                                                        + sp.Value
                                                                                WHERE sdb.snapshotid = @snapshotid;




                                                                        IF NOT EXISTS (SELECT
                                                                                        *
                                                                                FROM #tempdetails)
                                                                                SELECT
                                                                                        @sevcode = @sevcodeok,
                                                                                        @metricval = N'None found.';
                                                                        ELSE
                                                                        BEGIN
                                                                                SET @metricval = 'Next databases are owned by unacceptable accounts :';

                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + objectname
                                                                                        + ', '
                                                                                FROM #tempdetails
                                                                                GROUP BY objectname;

                                                                                SET @metricval = SUBSTRING(@metricval,
                                                                                0,
                                                                                LEN(@metricval));



                                                                                INSERT INTO policyassessmentdetail
                                                                                        SELECT
                                                                                                policyid,
                                                                                                assessmentid,
                                                                                                metricid,
                                                                                                snapshotid,
                                                                                                detailfinding,
                                                                                                databaseid,
                                                                                                objecttype,
                                                                                                objectid,
                                                                                                objectname
                                                                                        FROM #tempdetails
                                                                                        GROUP BY policyid,
                                                                                                 assessmentid,
                                                                                                 metricid,
                                                                                                 snapshotid,
                                                                                                 detailfinding,
                                                                                                 databaseid,
                                                                                                 objecttype,
                                                                                                 objectid,
                                                                                                 objectname;
                                                                                SELECT
                                                                                        @sevcode = @severity;


                                                                        END;
                                                                END;
                                                        END;

                                                        ELSE
                                                        BEGIN
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'Blacklist of database ownership was not provided';
                                                        END;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if  databases are owned by one of the next accounts:  '
                                                                + @severityvalues;
                                                END;

                                                ELSE
                                                IF (@metricid = 103)
                                                BEGIN

                                                        TRUNCATE TABLE #tempdetails;

                                                        IF (@isadmin = 1)
                                                        BEGIN

                                                                INSERT INTO #tempdetails
                                                                        SELECT
                                                                                @policyid,
                                                                                @assessmentid,
                                                                                @metricid,
                                                                                @snapshotid,
                                                                                N'Public role has access to  '
                                                                                + dd.name
                                                                                + ' object ',
                                                                                dd.dbid,
                                                                                dd.type,
                                                                                dd.objectid,
                                                                                dd.name
                                                                        FROM dbo.databaseobject dd
                                                                        JOIN dbo.sqldatabase sdb
                                                                                ON dd.snapshotid = sdb.snapshotid
                                                                                AND dd.dbid = sdb.dbid
                                                                                JOIN dbo.databaseschema ds
                                                                                        ON dd.snapshotid = ds.snapshotid
                                                                                        AND dd.dbid = ds.dbid
                                                                                        AND dd.schemaid = ds.schemaid
                                                                                JOIN dbo.databaseobjectpermission dp
                                                                                        ON dd.snapshotid = dp.snapshotid
                                                                                        AND dd.dbid = dp.dbid
                                                                                        AND dd.classid = dp.classid
                                                                                        AND dd.parentobjectid = dp.parentobjectid
                                                                                        AND dd.objectid = dp.objectid
                                                                        WHERE dp.grantee = 0
                                                                        AND ds.schemaname <> 'sys'
                                                                        AND sdb.databasename NOT IN (
                                                                        'master', 'msdb',
                                                                        'tempdb')
                                                                        AND dp.isdeny = 'N'
                                                                        AND dd.snapshotid = @snapshotid
                                                                        GROUP BY dd.name,
                                                                                 dd.dbid,
                                                                                 dd.type,
                                                                                 dd.objectid,
                                                                                 sdb.databasename;




                                                                IF NOT EXISTS (SELECT
                                                                                *
                                                                        FROM #tempdetails)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = N'None found.';
                                                                ELSE
                                                                BEGIN
                                                                        SET @metricval = 'Public roles have access to next objects :';

                                                                        SELECT
                                                                                @metricval = @metricval
                                                                                + objectname
                                                                                + ', '
                                                                        FROM #tempdetails
                                                                        GROUP BY objectname;

                                                                        SET @metricval = SUBSTRING(@metricval,
                                                                        0,
                                                                        LEN(@metricval));



                                                                        INSERT INTO policyassessmentdetail
                                                                                SELECT
                                                                                        policyid,
                                                                                        assessmentid,
                                                                                        metricid,
                                                                                        snapshotid,
                                                                                        detailfinding,
                                                                                        databaseid,
                                                                                        objecttype,
                                                                                        objectid,
                                                                                        objectname
                                                                                FROM #tempdetails
                                                                                GROUP BY policyid,
                                                                                         assessmentid,
                                                                                         metricid,
                                                                                         snapshotid,
                                                                                         detailfinding,
                                                                                         databaseid,
                                                                                         objecttype,
                                                                                         objectid,
                                                                                         objectname;
                                                                        SELECT
                                                                                @sevcode = @severity;


                                                                END;
                                                        END;


                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if public roles has access to user defined objects';

                                                END;

                                                ELSE
                                                IF (@metricid = 104)
                                                BEGIN

                                                        IF NOT EXISTS (SELECT
                                                                        *
                                                                FROM serversnapshot
                                                                WHERE snapshotid = @snapshotid
                                                                AND isclrenabled = N'Y')
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'CLR is turned off.';
                                                        ELSE
                                                        BEGIN
                                                                SET @metricval = 'CLR is turned on.';

                                                                INSERT INTO policyassessmentdetail (policyid,
                                                                assessmentid,
                                                                metricid,
                                                                snapshotid,
                                                                detailfinding,
                                                                databaseid,
                                                                objecttype,
                                                                objectid,
                                                                objectname)
                                                                        VALUES (@policyid, @assessmentid, @metricid, @snapshotid, @metricval, NULL, -- database ID,
                                                                        N'iSRV', -- object type
                                                                        NULL, @strval);
                                                                SET @sevcode = @severity;


                                                        END;
                                                END;


                                                ELSE
                                                IF (@metricid = 105)
                                                BEGIN

                                                        IF EXISTS (SELECT
                                                                        *
                                                                FROM serversnapshot
                                                                WHERE snapshotid = @snapshotid
                                                                AND isdefaulttraceenabled = N'Y')
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'Default Trace is enabled.';
                                                        ELSE
                                                        BEGIN
                                                                SET @metricval = N'Default Trace is disabled.';

                                                                INSERT INTO policyassessmentdetail (policyid,
                                                                assessmentid,
                                                                metricid,
                                                                snapshotid,
                                                                detailfinding,
                                                                databaseid,
                                                                objecttype,
                                                                objectid,
                                                                objectname)
                                                                        VALUES (@policyid, @assessmentid, @metricid, @snapshotid, @metricval, NULL, -- database ID,
                                                                        N'iSRV', -- object type
                                                                        NULL, @strval);
                                                                SET @sevcode = @severity;


                                                        END;
                                                END;

                                                ELSE
                                                IF (@metricid = 106)
                                                BEGIN
                                                        DECLARE @numerrorlogs smallint;
                                                        SELECT
                                                                @numerrorlogs = ISNULL(numerrorlogs,
                                                                0)
                                                        FROM serversnapshot
                                                        WHERE snapshotid = @snapshotid;
                                                        IF (@numerrorlogs > 11)
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'Maximum number of error log files is '
                                                                        + CONVERT(varchar, @numerrorlogs)
                                                                        + '.';
                                                        ELSE
                                                        BEGIN
                                                                SET @metricval = N'Maximum number of error log files is less than recommended (12+). Current value is '
                                                                + CONVERT(varchar, @numerrorlogs)
                                                                + '.';

                                                                INSERT INTO policyassessmentdetail (policyid,
                                                                assessmentid,
                                                                metricid,
                                                                snapshotid,
                                                                detailfinding,
                                                                databaseid,
                                                                objecttype,
                                                                objectid,
                                                                objectname)
                                                                        VALUES (@policyid, @assessmentid, @metricid, @snapshotid, @metricval, NULL, -- database ID,
                                                                        N'iSRV', -- object type
                                                                        NULL, @strval);
                                                                SET @sevcode = @severity;


                                                        END;
                                                END;

                                                ELSE
                                                IF (@metricid = 107)
                                                BEGIN
                                                        TRUNCATE TABLE #tempdetails;

                                                        IF OBJECT_ID('tempdb..#sevrVal') IS NOT NULL
                                                        BEGIN
                                                                DROP TABLE #sevrVal;
                                                        END;

                                                        SELECT
                                                                Value INTO #sevrVal
                                                        FROM dbo.splitbydelimiter(@severityvalues,
                                                        ',');

                                                        DECLARE @orphanedUsersCount AS int;
                                                        DECLARE @orphanedUsersnames AS varchar(max);
                                                        WITH OrphanedUsers (dbid, name, uid, type)
                                                        AS (SELECT
                                                                dbid,
                                                                name,
                                                                uid,
                                                                type
                                                        FROM dbo.databaseprincipal
                                                        WHERE usersid NOT IN (SELECT
                                                                sid
                                                        FROM dbo.serverprincipal
                                                        WHERE sid IS NOT NULL
                                                        AND snapshotid = @snapshotid)
                                                        AND type = N'S'
                                                        AND usersid <> 0x00
                                                        AND usersid IS NOT NULL
                                                        AND IsContainedUser = 0
                                                        AND snapshotid = @snapshotid
                                                        AND NOT EXISTS (SELECT
                                                                *
                                                        FROM #sevrVal
                                                        WHERE Value = name))
                                                        INSERT INTO #tempdetails
                                                                SELECT
                                                                        @policyid,
                                                                        @assessmentid,
                                                                        @metricid,
                                                                        @snapshotid,
                                                                        N'Orphaned user found - '
                                                                        + name,
                                                                        dbid,
                                                                        type,
                                                                        uid,
                                                                        name
                                                                FROM OrphanedUsers;
                                                        WITH OrphanedUsersForDb (orphanedUsersCountForDb, orphanedUsersnamesForDb, dbname, dbid)
                                                        AS (SELECT
                                                                COUNT(*) AS orphanedUsersCount,
                                                                STUFF((SELECT
                                                                        N', '
                                                                        + objectname
                                                                FROM #tempdetails c2
                                                                WHERE c.databaseid = c2.databaseid
                                                                FOR
                                                                xml PATH (N'')), 1, 2, N'') AS orphanedUsersnamesForDb,
                                                                sdb.databasename,
                                                                c.databaseid
                                                        FROM #tempdetails c
                                                        JOIN dbo.sqldatabase sdb
                                                                ON c.databaseid = sdb.dbid
                                                                AND c.snapshotid = sdb.snapshotid
                                                        GROUP BY c.databaseid,
                                                                 sdb.databasename)
                                                        SELECT
                                                                @orphanedUsersCount = SUM(orphanedUsersCountForDb),
                                                                @orphanedUsersnames = STUFF((SELECT
                                                                        N'; '
                                                                        + orphanedUsersnamesForDb
                                                                        + N' in '
                                                                        + dbname
                                                                FROM OrphanedUsersForDb
                                                                FOR
                                                                xml PATH (N'')), 1, 2, N'')
                                                        FROM OrphanedUsersForDb;

                                                        IF (@orphanedUsersCount IS NULL
                                                                OR @orphanedUsersCount = 0
                                                                )
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'There is no Orphaned users.';
                                                        ELSE
                                                        BEGIN
                                                                SET @metricval = CAST(@orphanedUsersCount AS nvarchar)
                                                                + N' orphaned users found: '
                                                                + @orphanedUsersnames
                                                                + '.';
                                                                INSERT INTO policyassessmentdetail
                                                                        SELECT
                                                                                policyid,
                                                                                assessmentid,
                                                                                metricid,
                                                                                snapshotid,
                                                                                detailfinding,
                                                                                databaseid,
                                                                                objecttype,
                                                                                objectid,
                                                                                objectname
                                                                        FROM #tempdetails;

                                                                SET @sevcode = @severity;
                                                        END;
                                                END;

                                                ELSE
                                                IF (@metricid = 108)
                                                BEGIN
                                                        IF NOT EXISTS (SELECT
                                                                        *
                                                                FROM serversnapshot
                                                                WHERE snapshotid = @snapshotid
                                                                AND oleautomationproceduresenabled = N'Y')
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'Ole automation procedures are disabled.';
                                                        ELSE
                                                        BEGIN
                                                                SET @metricval = N'Ole automation procedures are enabled.';

                                                                INSERT INTO policyassessmentdetail (policyid,
                                                                assessmentid,
                                                                metricid,
                                                                snapshotid,
                                                                detailfinding,
                                                                databaseid,
                                                                objecttype,
                                                                objectid,
                                                                objectname)
                                                                        VALUES (@policyid, @assessmentid, @metricid, @snapshotid, @metricval, NULL, -- database ID,
                                                                        N'DB', -- object type
                                                                        NULL, @strval);
                                                                SET @sevcode = @severity;
                                                        END;
                                                END;

                                                ELSE
                                                IF (@metricid = 109)
                                                BEGIN

                                                        IF EXISTS (SELECT
                                                                        1
                                                                FROM serversnapshot
                                                                WHERE snapshotid = @snapshotid
                                                                AND iscommoncriteriacomplianceenabled = N'Y')
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'Common criteria compliance is enabled.';
                                                        ELSE
                                                        BEGIN
                                                                SET @metricval = 'Common criteria compliance is disabled.';

                                                                INSERT INTO policyassessmentdetail (policyid,
                                                                assessmentid,
                                                                metricid,
                                                                snapshotid,
                                                                detailfinding,
                                                                databaseid,
                                                                objecttype,
                                                                objectid,
                                                                objectname)
                                                                        VALUES (@policyid, @assessmentid, @metricid, @snapshotid, @metricval, NULL, -- database ID,
                                                                        N'DB', -- object type
                                                                        NULL, @strval);
                                                                SET @sevcode = @severity;


                                                        END;
                                                END;

                                                -- Integration Services Users Permissions Not Acceptable
                                                ELSE
                                                IF (@metricid = 110)
                                                BEGIN
                                                        SELECT
                                                                @sql = N'declare databasecursor cursor for
											select vdop.objectname, dp.name
											from [dbo].[vwdatabaseobjectpermission] vdop
											inner join databaseprincipal dp 
												on ((vdop.snapshotid = dp.snapshotid) and (vdop.dbid = dp.dbid) and (vdop.grantee = dp.uid))
											where 
											(
												(vdop.snapshotid = '
                                                                + CONVERT(nvarchar, @snapshotid)
                                                                + N') 
												and ((vdop.isgrant = N''Y'') or (vdop.isgrantwith = N''Y''))
												and (vdop.objectname in ('
                                                                + @severityvalues
                                                                + N'))
												and dp.type IN (''S'', ''U'', ''G'')
											)';
                                                        EXEC (@sql);
                                                        OPEN databasecursor;
                                                        FETCH NEXT FROM
                                                        databasecursor INTO @strval2,
                                                        @strval3;

                                                        SELECT
                                                                @intval2 = 0;
                                                        WHILE @@fetch_status = 0
                                                        BEGIN
                                                                SELECT
                                                                        @strval = @strval3
                                                                        + ' on '
                                                                        + @strval2;
                                                                IF (@intval2 = 1
                                                                        OR LEN(@metricval)
                                                                        + LEN(@strval) > 1010
                                                                        )
                                                                BEGIN
                                                                        IF @intval2 = 0
                                                                                SELECT
                                                                                        @metricval = @metricval
                                                                                        + N', more...',
                                                                                        @intval2 = 1;
                                                                END;
                                                                ELSE
                                                                        SELECT
                                                                                @metricval = @metricval
                                                                                + CASE
                                                                                        WHEN LEN(@metricval) > 0 THEN N', '
                                                                                        ELSE N''
                                                                                END + N''''
                                                                                + @strval
                                                                                + N'''';

                                                                IF (@isadmin = 1)
                                                                        INSERT INTO policyassessmentdetail (policyid,
                                                                        assessmentid,
                                                                        metricid,
                                                                        snapshotid,
                                                                        detailfinding,
                                                                        databaseid,
                                                                        objecttype,
                                                                        objectid,
                                                                        objectname)
                                                                                VALUES (@policyid, @assessmentid, @metricid, @snapshotid, N'Permissions on stored procedures found: ''' + @strval + N'''', NULL, -- database ID,
                                                                                N'DB', -- object type
                                                                                NULL, @strval);

                                                                FETCH NEXT FROM
                                                                databasecursor INTO @strval2,
                                                                @strval3;
                                                        END;

                                                        CLOSE databasecursor;
                                                        DEALLOCATE databasecursor;

                                                        IF (LEN(@metricval) = 0)
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = 'No unacceptable permissions found.';
                                                        ELSE
                                                                SELECT
                                                                        @sevcode = @severity,
                                                                        @metricval = N'Permissions on stored procedures found: '
                                                                        + @metricval;

                                                        SELECT
                                                                @metricthreshold = N'Server is vulnerable if users other than the default SSIS database roles have been granted permissions on an Integration Services stored procedure.';
                                                END;

                                                -- Other General Domain Accounts Check
                                                ELSE
                                                IF (@metricid = 111)
                                                BEGIN

                                                        -- Well-known security identifiers in Windows operating systems: https://support.microsoft.com/en-us/kb/243330
                                                        DECLARE @domainUsersSidPattern AS nvarchar(35);
                                                        DECLARE @everyoneSidString AS nvarchar(27);
                                                        DECLARE @authenticatedUsersSidString AS nvarchar(27);

                                                        SET @domainUsersSidPattern = '0x010500000000000515000000%01020000';
                                                        SET @everyoneSidString = '0x010100000000000100000000';
                                                        SET @authenticatedUsersSidString = '0x01010000000000050b000000';

                                                        DECLARE @generalUserPrincipals TABLE (
                                                                name
                                                                nvarchar(128)
                                                                NOT NULL
                                                        );

                                                        ;
                                                        WITH ConvertedSidsForCurrentSnapshot ([sid], name)
                                                        AS (SELECT
                                                                CONVERT([VARCHAR](512), sid, 1) AS [sid],
                                                                [name]
                                                        FROM [serverprincipal]
                                                        WHERE snapshotid = @snapshotid)
                                                        INSERT INTO @generalUserPrincipals
                                                                SELECT
                                                                        [name]
                                                                FROM ConvertedSidsForCurrentSnapshot
                                                                WHERE [sid] LIKE @domainUsersSidPattern
                                                                OR [sid] IN (
                                                                @everyoneSidString,
                                                                @authenticatedUsersSidString);

                                                        IF NOT EXISTS (SELECT
                                                                        1
                                                                FROM @generalUserPrincipals)
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'There is no general domain accounts on the instance.';
                                                        ELSE
                                                        BEGIN

                                                                DECLARE @userNamesList varchar(max);
                                                                SET @userNamesList = '';
                                                                SELECT
                                                                        @userNamesList = @userNamesList
                                                                        + name + ','
                                                                FROM @generalUserPrincipals;
                                                                SET @userNamesList = SUBSTRING(@userNamesList,
                                                                1,
                                                                LEN(@userNamesList)
                                                                - 1);

                                                                SET @metricval = 'General domain accounts are added to the instance: '
                                                                + @userNamesList
                                                                + '.';

                                                                INSERT INTO policyassessmentdetail (policyid,
                                                                assessmentid,
                                                                metricid,
                                                                snapshotid,
                                                                detailfinding,
                                                                databaseid,
                                                                objecttype,
                                                                objectid,
                                                                objectname)
                                                                        VALUES (@policyid, @assessmentid, @metricid, @snapshotid, @metricval, NULL, -- database ID,
                                                                        N'iSRV', -- object type
                                                                        NULL, @strval);
                                                                SET @sevcode = @severity;

                                                        END;
                                                END;

                                                -- SQL Jobs and Agent Check
                                                ELSE
                                                IF (@metricid = 112)
                                                BEGIN

                                                        DECLARE @jobsStepsWithoutProxy TABLE (
                                                                name
                                                                nvarchar(128)
                                                                NOT NULL,
                                                                step
                                                                nvarchar(128)
                                                                NULL
                                                        );

                                                        INSERT INTO @jobsStepsWithoutProxy
                                                                SELECT
																	j.Name,
																	j.Step
                                                                FROM [dbo].[sqljob] j
																JOIN serversnapshot ss ON ss.snapshotid =  j.SnapshotId
                                                                WHERE 
																	j.ProxyId IS NULL AND 
																	j.SnapshotId = @snapshotid
                                                                ORDER BY 
																	j.Name,
																	j.JobId;

                                                        IF NOT EXISTS (SELECT
                                                                        1
                                                                FROM @jobsStepsWithoutProxy)
                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'There is no job step without proxy account.';
														ELSE IF(dbo.fn_getversionasdecimal(dbo.fn_normalizeversion(@version)) < dbo.fn_getversionasdecimal(dbo.fn_normalizeversion(N'9.')))
																SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'SQL server 2000 does not support proxies.';
                                                        ELSE
                                                        BEGIN

                                                                DECLARE @jobStepsList varchar(max);
                                                                SET @jobStepsList = '';
                                                                SELECT
                                                                        @jobStepsList = @jobStepsList
                                                                        + 'Job: "' + name + '" Step: "'
                                                                        + step + '"; '
                                                                FROM @jobsStepsWithoutProxy;

                                                                SET @metricval = 'There are jobs that have no proxy account: '
                                                                + CHAR(13)
                                                                + CHAR(10)
                                                                + @jobStepsList
                                                                + '';

                                                                INSERT INTO policyassessmentdetail (policyid,
                                                                assessmentid,
                                                                metricid,
                                                                snapshotid,
                                                                detailfinding,
                                                                databaseid,
                                                                objecttype,
                                                                objectid,
                                                                objectname)
                                                                        VALUES (@policyid, @assessmentid, @metricid, @snapshotid, @metricval, NULL, -- database ID,
                                                                        N'iSRV', -- object type
                                                                        NULL, @strval);
                                                                SET @sevcode = @severity;

                                                        END;
														DELETE FROM @jobsStepsWithoutProxy
                                                END;

                                                ---Encryption methods 
                                                ELSE
                                                IF (@metricid = 113)
                                                BEGIN

												IF(dbo.fn_getversionasdecimal(dbo.fn_normalizeversion(@version)) < dbo.fn_getversionasdecimal(dbo.fn_normalizeversion(N'9.')))
													BEGIN
															SELECT  @sevcode = @sevcodeok, @metricval = N'Check can''t be applied to SQL Server 2000.';
													END
													ELSE
													BEGIN
                                                        SELECT
                                                                e.name,
                                                                e.algorithmdesc,
                                                                s.databasename INTO #keys
                                                        FROM dbo.encryptionkey e
                                                        JOIN dbo.sqldatabase s
                                                                ON s.dbid = e.databaseid
                                                                AND e.snapshotid = s.snapshotid
                                                        WHERE e.snapshotid = @snapshotid
                                                        AND e.algorithmdesc NOT IN (SELECT
                                                                Value
                                                        FROM dbo.splitbydelimiter(@severityvalues,
                                                        ','));
														set @metricthreshold = 'Server is vulnerable if there are encryption methods other than : '+@severityvalues;
                                                        IF NOT EXISTS (SELECT
                                                                        1
                                                                FROM #keys)

                                                                SELECT
                                                                        @sevcode = @sevcodeok,
                                                                        @metricval = N'There is no keys with not allowed encryption methods.';
																		
                                                        ELSE
                                                        BEGIN

                                                                DECLARE @foundKeys varchar(max);
                                                                SET @foundKeys = '';
                                                                SELECT
                                                                        @foundKeys = @foundKeys
                                                                        + name + '( '
                                                                        + algorithmdesc
                                                                        + ') - '
                                                                        + databasename
                                                                        + ',' + CHAR(13)
                                                                        + CHAR(10)
                                                                FROM #keys;
                                                                SET @foundKeys = SUBSTRING(@foundKeys,1,LEN(@foundKeys)- 1);
                                                                SET @metricval = 'There are keys that have not supported encryption method: '+ CHAR(13)+ CHAR(10)+ @foundKeys+ '';
																
																 SET @sevcode = @severity;
														END;
                                                                INSERT INTO policyassessmentdetail (policyid,
                                                                assessmentid,
                                                                metricid,
                                                                snapshotid,
                                                                detailfinding,
                                                                databaseid,
                                                                objecttype,
                                                                objectid,
                                                                objectname)
                                                                        VALUES (@policyid, @assessmentid, @metricid, @snapshotid, @metricval, NULL, -- database ID,
                                                                        N'DB', -- object type
                                                                        NULL, @strval);
                                                               

                                                        
														IF OBJECT_ID('tempdb..#keys') IS NOT NULL DROP TABLE #keys;
														END;
                                                END;
                                                ELSE
                                                IF (@metricid = 114)
                                                BEGIN
                                                        IF(dbo.fn_getversionasdecimal(dbo.fn_normalizeversion(@version)) > dbo.fn_getversionasdecimal(dbo.fn_normalizeversion(N'9.')))
                                                        BEGIN
                                                                SELECT
                                                                        c.name,
                                                                        s.databasename INTO #certs
                                                                FROM dbo.databasecertificates c
                                                                JOIN dbo.sqldatabase s
                                                                        ON s.dbid = c.dbid
                                                                        AND c.snapshotid = s.snapshotid
                                                                WHERE c.snapshotid = @snapshotid
                                                                AND c.pvt_key_last_backup_date IS NULL


                                                                IF NOT EXISTS (SELECT
                                                                                1
                                                                        FROM #certs)
																		BEGIN
                                                                        SELECT
                                                                                @sevcode = @sevcodeok,
                                                                                @metricval = N'All certificate private keys were exported.';
																				END
                                                                ELSE
                                                                BEGIN

                                                                        DECLARE @foundCerts varchar(max);
                                                                        SET @foundCerts = '';
                                                                        SELECT
                                                                                @foundCerts = @foundCerts
                                                                                + name + ' - '
                                                                                + databasename
                                                                                + ', ' + CHAR(13)
                                                                                + CHAR(10)
                                                                        FROM #certs;
                                                                        SET @foundCerts = SUBSTRING(@foundCerts,1,LEN(@foundCerts)- 2);
                                                                        SET @metricval = 'There are certificate private keys that have not been exported: '+ CHAR(13)+ CHAR(10)+ @foundCerts+ '';
																		 SET @sevcode = @severity;
																END;
                                                                        INSERT INTO policyassessmentdetail (policyid,
                                                                        assessmentid,
                                                                        metricid,
                                                                        snapshotid,
                                                                        detailfinding,
                                                                        databaseid,
                                                                        objecttype,
                                                                        objectid,
                                                                        objectname)
                                                                                VALUES (@policyid, @assessmentid, @metricid, @snapshotid, @metricval, NULL, -- database ID,
                                                                                N'DB', -- object type
                                                                                NULL, @strval);
                                                                       

                                                              IF OBJECT_ID('tempdb..#certs') IS NOT NULL DROP TABLE #certs;  
                                                        END;
														
                                                END;
												ELSE
                                              IF ( @metricid = 115 )
													BEGIN

													--Check if LinkedServers Used
													SELECT servername 
													INTO #linkedsrv
													FROM linkedserver ls
													WHERE ls.snapshotid = @snapshotid

													IF NOT EXISTS (SELECT 1 FROM #linkedsrv)
														BEGIN
														--Linked servers not used

														SELECT
																		@sevcode = @sevcodeok ,
																		@metricval = N'Linked Servers are not configured.';
														END
													ELSE
														BEGIN
														--Linked servers used add warning
														DECLARE @foundLinkedServer VARCHAR(MAX);
																SET @foundLinkedServer = '';
																SELECT
																	@foundLinkedServer = @foundLinkedServer + servername
																	+ ', ' + CHAR(13) + CHAR(10)
																FROM
																	#linkedsrv;
																SET @foundLinkedServer = SUBSTRING(@foundLinkedServer, 1,
																							LEN(@foundLinkedServer) - 2);
																SET @metricval = 'There are linked servers configured: '
																	+ CHAR(13) + CHAR(10) + @foundLinkedServer + '';
																SET @sevcode = @severity;
														END;

													INSERT  INTO policyassessmentdetail
																		(
																		  policyid ,
																		  assessmentid ,
																		  metricid ,
																		  snapshotid ,
																		  detailfinding ,
																		  databaseid ,
																		  objecttype ,
																		  objectid ,
																		  objectname
																		)
																VALUES
																		(
																		  @policyid ,
																		  @assessmentid ,
																		  @metricid ,
																		  @snapshotid ,
																		  @metricval ,
																		  NULL , -- database ID,
																		  N'iLOGN' , -- object type
																		  NULL ,
																		  @strval
																		);

													IF OBJECT_ID('tempdb..#linkedsrv') IS NOT NULL DROP TABLE #linkedsrv;
													
													--End Check if Linked Servers Used

													END
													ELSE
													--Check if configured linked servers has security holes
													 IF ( @metricid = 116 )
													BEGIN
                                                        
														SELECT
															ls.servername ,
															lsp.principal
														INTO
															#linksrvusr
														FROM
															dbo.linkedserver ls
														JOIN dbo.linkedserverprincipal lsp
														ON  ls.serverid = lsp.serverid
															AND ls.snapshotid = lsp.snapshotid
														WHERE
															ls.snapshotid = @snapshotid
            


														IF NOT EXISTS ( SELECT
																			1
																		FROM
																			#linksrvusr )
															BEGIN
																SELECT
																	@sevcode = @sevcodeok ,
																	@metricval = N'There are no linked servers that are running as a member of sysadmin group.';
															END
														ELSE
															BEGIN

																DECLARE @foundLinkedServerUser VARCHAR(MAX);
																SET @foundLinkedServerUser = '';
																SELECT
																	@foundLinkedServerUser = @foundLinkedServerUser + servername + ' - ' + principal
																	+ ', ' + CHAR(13) + CHAR(10)
																FROM
																	#linksrvusr;
																SET @foundLinkedServerUser = SUBSTRING(@foundLinkedServerUser, 1,
																							LEN(@foundLinkedServerUser) - 2);
																SET @metricval = 'There are linked servers that are running as a member of sysadmin group: '
																	+ CHAR(13) + CHAR(10) + @foundLinkedServerUser + '';
																SET @sevcode = @severity;
																	
															END
																INSERT  INTO policyassessmentdetail
																		(
																		  policyid ,
																		  assessmentid ,
																		  metricid ,
																		  snapshotid ,
																		  detailfinding ,
																		  databaseid ,
																		  objecttype ,
																		  objectid ,
																		  objectname
																		)
																VALUES
																		(
																		  @policyid ,
																		  @assessmentid ,
																		  @metricid ,
																		  @snapshotid ,
																		  @metricval ,
																		  NULL , -- database ID,
																		  N'iLOGN' , -- object type
																		  NULL ,
																		  @strval
																		);
																
																   IF OBJECT_ID('tempdb..#linksrvusr') IS NOT NULL DROP TABLE #linksrvusr;
																  
															END;
                                                        
     
                                                
                                                --**************************** code added to handle user defined security checks, but never used (first added in version 2.5)
                                                -- User implemented
                                                ELSE
                                                IF (@metricid >= 1000)
                                                BEGIN
                                                        IF EXISTS (SELECT
                                                                        *
                                                                FROM dbo.sysobjects
                                                                WHERE name = @severityvalues
                                                                AND type = N'P')
                                                        BEGIN
                                                                SELECT
                                                                        @strval = @severityvalues;
                                                        -- this currently will accept no parameters and cannot return any list of exceptions
                                                        -- it should read the sysobjects table to find predefined output parameters and process them appropriately
                                                        --
                                                        BEGIN TRY
                                                                EXEC @intval = @strval;

                                                                IF (@intval = 0)
                                                                        SELECT
                                                                                @sevcode = @sevcodeok;
                                                                ELSE
                                                                        SELECT
                                                                                @sevcode = @severity;

                                                                SELECT
                                                                        @metricval = dbo.getyesnotext(N'U');
                                                        END TRY
                                                        BEGIN CATCH
                                                                SELECT
                                                                        @sevcode = @severity;
                                                                SELECT
                                                                        @metricval = N'Error '
                                                                        + CAST(ERROR_NUMBER() AS nvarchar)
                                                                        + N' encountered while executing custom stored procedure '
                                                                        + @severityvalues
                                                                        + N': '
                                                                        + ERROR_MESSAGE();
                                                                SELECT
                                                                        @metricthreshold = N'The security check could not be verified.';
                                                        END CATCH;

                                                                SELECT
                                                                        @metricthreshold = N'Server is vulnerable if the stored procedure '
                                                                        + @severityvalues
                                                                        + ' returns true.';
                                                        END;
                                                        ELSE
                                                                SELECT
                                                                        @metricval = dbo.getyesnotext(N'U');
                                                        SELECT
                                                                @metricthreshold = N'The stored procedure '
                                                                + @severityvalues
                                                                + ' was not found in the SQLsecure database.';
                                                END;
                                        END TRY
                                        BEGIN CATCH
                                                SELECT
                                                        @sevcode = @severity;
                                                SELECT
                                                        @metricval = N'Error '
                                                        + CAST(ERROR_NUMBER() AS nvarchar)
                                                        + N' encountered on line '
                                                        + CAST(ERROR_LINE() AS nvarchar)
                                                        + N' while processing security check: '
                                                        + ERROR_MESSAGE();
                                                SELECT
                                                        @metricthreshold = N'The security check could not be verified.';
                                        END CATCH;

                                                --****************************** done processing the security check. Now write out the valid metric data ***********************************
                                                IF (@err = 0
                                                        AND (@alertsonly = 0
                                                        OR @sevcode > 0
                                                        )
                                                        )
                                                BEGIN
                                                        -- handle unexpected null values more gracefully
                                                        SELECT
                                                                @metricval = ISNULL(@metricval,
                                                                'The selected snapshot does not contain a value. Check the snapshot status and the activity log for possible causes.');
                                                        IF (EXISTS (SELECT
                                                                        *
                                                                FROM @returnservertbl
                                                                WHERE registeredserverid = @registeredserverid)
                                                                )
                                                                INSERT INTO @outtbl (snapshotid,
                                                                registeredserverid,
                                                                connectionname,
                                                                collectiontime,
                                                                metricid,
                                                                metricname,
                                                                metrictype,
                                                                metricseveritycode,
                                                                metricseverity,
                                                                metricseverityvalues,
                                                                metricdescription,
                                                                metricreportkey,
                                                                metricreporttext,
                                                                severitycode,
                                                                severity,
                                                                currentvalue,
                                                                thresholdvalue)
                                                                        VALUES (@snapshotid, @registeredserverid, @connection, @snapshottime, @metricid, @metricname, @metrictype, @severity, dbo.getpolicyseverityname(@severity), @configuredvalues, @metricdescription, @metricreportkey, @metricreporttext, @sevcode, dbo.getpolicyseverityname(@sevcode), @metricval, @metricthreshold);
                                                        IF (@isadmin = 1)
                                                                INSERT INTO policyassessment (policyid,
                                                                assessmentid,
                                                                snapshotid,
                                                                registeredserverid,
                                                                connectionname,
                                                                collectiontime,
                                                                metricid,
                                                                metricname,
                                                                metrictype,
                                                                metricseveritycode,
                                                                metricseverity,
                                                                metricseverityvalues,
                                                                metricdescription,
                                                                metricreportkey,
                                                                metricreporttext,
                                                                severitycode,
                                                                severity,
                                                                currentvalue,
                                                                thresholdvalue)
                                                                        VALUES (@policyid, @assessmentid, @snapshotid, @registeredserverid, @connection, @snapshottime, @metricid, @metricname, @metrictype, @severity, dbo.getpolicyseverityname(@severity), @configuredvalues, @metricdescription, @metricreportkey, @metricreporttext, @sevcode, dbo.getpolicyseverityname(@sevcode), @metricval, @metricthreshold);
                                                END;

                                                IF (@debug = 1)
                                                        PRINT 'metric execution took '
                                                        + CONVERT(nvarchar, DATEDIFF(SECOND,
                                                        @runtime,
                                                        GETDATE()))
                                                        + ' seconds';

                                                FETCH NEXT FROM metriccursor INTO @metricid,
                                                @metricname, @metrictype,
                                                @metricdescription, @metricreportkey,
                                                @metricreporttext, @severity,
                                                @severityvalues;
                                        END;

                                        -- delete from the server table after processing
                                        DELETE #servertbl
                                        WHERE registeredserverid = @registeredserverid;

                                        IF (@debug = 1)
                                                PRINT 'server execution took '
                                                + CONVERT(nvarchar, DATEDIFF(SECOND,
                                                @serverruntime,
                                                GETDATE()))
                                                + ' seconds';
										CLOSE metriccursor;
										DEALLOCATE metriccursor;
												
                                        FETCH NEXT FROM snapcursor INTO @snapshotid,
                                        @registeredserverid, @connection,
                                        @snapshottime, @status, @baseline,
                                        @collectorversion, @version, @os,
                                        @authentication, @loginauditmode,
                                        @c2audittrace, @crossdb, @proxy, @remotedac,
                                        @remoteaccess, @startupprocs, @sqlmail,
                                        @databasemail, @ole, @webassistant,
                                        @xp_cmdshell, @agentmailprofile, @hide,
                                        @agentsysadmin, @dc, @replication, @sapassword,
                                        @systemtables, @systemdrive, @adhocqueries,
                                        @weakpasswordenabled;
                                END;

                                -- drop saved temp table after all snapshot processing is done
                                DROP TABLE #sysadminstbl;

                                CLOSE snapcursor;
                                DEALLOCATE snapcursor;

                                --			-- now process the non-server related metrics
                                --			fetch first from metriccursor into @metricid, @metricname, @metrictype,
                                --												@metricdescription, @metricreportkey, @metricreporttext,
                                --												@severity, @severityvalues
                                --			while (@@fetch_status = 0)
                                --			begin
                                --				-- This sets the metric so it will not be displayed if no value is found
                                --				--     each metric should handle this situation appropriately
                                --				select @err=0, @sevcode=-1, @metricval=N'', @metricthreshold=N'', @configuredvalues=@severityvalues
                                --				-- clean up old values
                                --				select @intval=0, @intval2=0, @strval=N'', @strval2=N'', @strval3=N'', @sql=N''
                                --				delete from @tblval
                                --
                                --	--************************************************* version 2.5 security checks
                                --				-- Security Check Settings are different (Assessment Comparison)
                                --				if (@metricid = 64)
                                --				begin
                                --					select @severityvalues = N'Y'
                                --					if (N'Y' <> @severityvalues)
                                --						select @sevcode=@sevcodeok
                                --					else
                                --						select @sevcode=@severity
                                --
                                --					select @metricval = dbo.getyesnotext(@severityvalues)
                                --					select @metricthreshold = N'Servers are vulnerable if security check settings are different from the most recent ' + dbo.getassessmentstatename(@severityvalues) + N' assessment.'
                                --				end
                                --
                                --				-- Assessment Findings are different (Assessment Comparison)
                                --				else if (@metricid = 65)
                                --				begin
                                --					select @severityvalues = N'Y'
                                --					if (N'Y' <> @severityvalues)
                                --						select @sevcode=@sevcodeok
                                --					else
                                --						select @sevcode=@severity
                                --
                                --					select @metricval = dbo.getyesnotext(@severityvalues)
                                --					select @metricthreshold = N'Servers are vulnerable if assessment results are different from the most recent ' + dbo.getassessmentstatename(@severityvalues) + N' assessment.'
                                --				end
                                --
                                --				-- Policy Servers are different (Assessment Comparison)
                                --				else if (@metricid = 66)
                                --				begin
                                --					-- get the assessmentid of the selected policy for comparison
                                --					select @strval = N'declare assessmentcursor cursor for 
                                --											select top 1 assessmentid 
                                --												from assessment 
                                --												where policyid = ' + convert(nvarchar,@policyid) + N'
                                --													and assessmentstate in (' + @severityvalues + N') 
                                --												order by assessmentid desc'
                                --					exec (@strval)
                                --
                                --					open assessmentcursor
                                --					fetch next from assessmentcursor into @intval
                                --					close assessmentcursor
                                --					deallocate assessmentcursor	
                                --
                                --					-- get the list of servers for the selected policy
                                --					if (@intval is not null and @intval > 0)
                                --					begin
                                --						declare policyservercursor cursor for
                                --							select a1.registeredserverid, b1.connectionname + N' added'
                                --								from (policymember a1 left join registeredserver b1 on a1.registeredserverid = b1.registeredserverid)
                                --										left join (policymember a2 left join registeredserver b2 on a2.registeredserverid = b2.registeredserverid)
                                --											on a2.policyid = @policyid and a2.assessmentid = @intval and a1.registeredserverid = a2.registeredserverid
                                --								where a1.policyid = @policyid and a1.assessmentid = @assessmentid and a2.registeredserverid is null
                                --							union
                                --							select a1.registeredserverid, b1.connectionname + N' missing'
                                --								from (policymember a1 left join registeredserver b1 on a1.registeredserverid = b1.registeredserverid)
                                --										left join (policymember a2 left join registeredserver b2 on a2.registeredserverid = b2.registeredserverid)
                                --											on a2.policyid = @policyid and a2.assessmentid = @assessmentid and a1.registeredserverid = a2.registeredserverid
                                --								where a1.policyid = @policyid and a1.assessmentid = @intval and a2.registeredserverid is null
                                --
                                --						open policyservercursor
                                --						fetch next from policyservercursor into @intval, @strval
                                --						select @intval2 = 0
                                --						while @@fetch_status = 0
                                --						begin
                                --							if (@intval2 = 1 or len(@metricval) + len(@strval) > 1010)
                                --							begin
                                --								if @intval2 = 0
                                --									select @metricval = @metricval + N', more...',
                                --											@intval2 = 1
                                --							end
                                --							else
                                --								select @metricval = @metricval + case when len(@metricval) > 0 then N', ' else N'' end + N'''' + @strval + N''''
                                --
                                --							if (@isadmin = 1)
                                --							insert into policyassessmentdetail ( policyid,
                                --																 assessmentid,
                                --																 metricid,
                                --																 snapshotid,
                                --																 detailfinding,
                                --																 databaseid,
                                --																 objecttype,
                                --																 objectid,
                                --																 objectname )
                                --														values ( @policyid,
                                --																 @assessmentid,
                                --																 @metricid,
                                --																 @snapshotid,
                                --																 @strval,
                                --																 null, -- database ID,
                                --																 N'SVR', -- object type
                                --																 @intval,
                                --																 @strval )
                                --
                                --							fetch next from policyservercursor into @intval, @strval
                                --						end
                                --
                                --						close policyservercursor
                                --						deallocate policyservercursor	
                                --
                                --						if (len(@metricval) = 0)
                                --							select @sevcode=@sevcodeok,
                                --									@metricval = 'No servers are different on the compared assessments.'
                                --						else
                                --							select @sevcode=@severity,
                                --									@metricval = N'These servers are different: ' + @metricval
                                --					end
                                --					else
                                --					begin
                                --						select @sevcode=@severity,
                                --									@metricval = N'The selected assessment was not found and all servers are different'
                                --					end
                                --
                                --					select @metricthreshold = N'Servers are vulnerable if the policy server list is different from the most recent ' + dbo.getassessmentstatename(substring(@severityvalues,2,1)) + N' assessment.'
                                --				end
                                --
                                --				-- write out the valid metric data
                                --				if (@err = 0 and (@alertsonly = 0 or @sevcode > 0))
                                --				begin
                                --					-- handle unexpected null values more gracefully
                                --					if (exists (select * from @returnservertbl where registeredserverid = @registeredserverid))
                                --						insert into @outtbl (
                                --									snapshotid,
                                --									registeredserverid,
                                --									connectionname,
                                --									collectiontime,
                                --									metricid,
                                --									metricname,
                                --									metrictype,
                                --									metricseveritycode,
                                --									metricseverity,
                                --									metricseverityvalues,
                                --									metricdescription,
                                --									metricreportkey,
                                --									metricreporttext,
                                --									severitycode,
                                --									severity,
                                --									currentvalue,
                                --									thresholdvalue)
                                --							values (
                                --									null,
                                --									null,
                                --									N'',
                                --									null,
                                --									@metricid,
                                --									@metricname,
                                --									@metrictype,
                                --									@severity,
                                --									dbo.getpolicyseverityname(@severity),
                                --									@configuredvalues,
                                --									@metricdescription,
                                --									@metricreportkey,
                                --									@metricreporttext,
                                --									@sevcode,
                                --									dbo.getpolicyseverityname(@sevcode),
                                --									@metricval,
                                --									@metricthreshold
                                --									)
                                --					if (@isadmin = 1)
                                --					insert into policyassessment (
                                --									policyid,
                                --									assessmentid,
                                --									snapshotid,
                                --									registeredserverid,
                                --									connectionname,
                                --									collectiontime,
                                --									metricid,
                                --									metricname,
                                --									metrictype,
                                --									metricseveritycode,
                                --									metricseverity,
                                --									metricseverityvalues,
                                --									metricdescription,
                                --									metricreportkey,
                                --									metricreporttext,
                                --									severitycode,
                                --									severity,
                                --									currentvalue,
                                --									thresholdvalue)
                                --							values (
                                --									@policyid,
                                --									@assessmentid,
                                --									null,
                                --									null,
                                --									N'',
                                --									null,
                                --									@metricid,
                                --									@metricname,
                                --									@metrictype,
                                --									@severity,
                                --									dbo.getpolicyseverityname(@severity),
                                --									@configuredvalues,
                                --									@metricdescription,
                                --									@metricreportkey,
                                --									@metricreporttext,
                                --									@sevcode,
                                --									dbo.getpolicyseverityname(@sevcode),
                                --									@metricval, 
                                --									@metricthreshold
                                --									)
                                --				end
                                --
                                --				fetch next from metriccursor into @metricid, @metricname, @metrictype,
                                --													@metricdescription, @metricreportkey, @metricreporttext,
                                --													@severity, @severityvalues
                                --			end

                                

                                -- if any servers are left in the server table, there was no audit data and this is a finding
                                IF EXISTS (SELECT
                                                *
                                        FROM #servertbl)
                                BEGIN
                                        -- Audited Servers
                                        SELECT
                                                @metricid = metricid,
                                                @metricname = metricname,
                                                @metrictype = metrictype,
                                                @severity = severity,
                                                @metricdescription = metricdescription,
                                                @metricreportkey = reportkey,
                                                @metricreporttext = reporttext
                                        FROM vwpolicymetric
                                        WHERE policyid = @policyid
                                        AND assessmentid = @assessmentid
                                        AND metricid = 54
                                        AND isenabled = 1;

                                        IF (@metricid = 54)	--	only process if the metric was enabled
                                        BEGIN
                                                DECLARE servercursor CURSOR STATIC FOR
                                                SELECT
                                                        registeredserverid,
                                                        connectionname
                                                FROM registeredserver
                                                WHERE registeredserverid IN (SELECT
                                                        registeredserverid
                                                FROM #servertbl);
                                                OPEN servercursor;

                                                FETCH NEXT FROM servercursor INTO @registeredserverid,
                                                @connection;
                                                WHILE (@@fetch_status = 0)
                                                BEGIN
                                                        SELECT
                                                                @metricval = N'Server has no audit data for the selections.';

                                                        SELECT
                                                                @metricthreshold = N'Assessment may not be valid if all servers do not have audit data.';

                                                        IF (EXISTS (SELECT
                                                                        *
                                                                FROM @returnservertbl
                                                                WHERE registeredserverid = @registeredserverid)
                                                                )
                                                                INSERT INTO @outtbl (registeredserverid,
                                                                connectionname,
                                                                metricid,
                                                                metricname,
                                                                metrictype,
                                                                metricseveritycode,
                                                                metricseverity,
                                                                metricseverityvalues,
                                                                metricdescription,
                                                                metricreportkey,
                                                                metricreporttext,
                                                                severitycode,
                                                                severity,
                                                                currentvalue,
                                                                thresholdvalue)
                                                                        VALUES (@registeredserverid, @connection, @metricid, @metricname, @metrictype, @severity, dbo.getpolicyseverityname(@severity), N'', @metricdescription, @metricreportkey, @metricreporttext, @severity, dbo.getpolicyseverityname(@severity), @metricval, @metricthreshold);

                                                        IF (@isadmin = 1)
                                                                INSERT INTO policyassessment (policyid,
                                                                assessmentid,
                                                                snapshotid,
                                                                registeredserverid,
                                                                connectionname,
                                                                collectiontime,
                                                                metricid,
                                                                metricname,
                                                                metrictype,
                                                                metricseveritycode,
                                                                metricseverity,
                                                                metricseverityvalues,
                                                                metricdescription,
                                                                metricreportkey,
                                                                metricreporttext,
                                                                severitycode,
                                                                severity,
                                                                currentvalue,
                                                                thresholdvalue)
                                                                        VALUES (@policyid, @assessmentid, NULL, @registeredserverid, @connection, NULL, @metricid, @metricname, @metrictype, @severity, dbo.getpolicyseverityname(@severity), N'', @metricdescription, @metricreportkey, @metricreporttext, @severity, dbo.getpolicyseverityname(@severity), @metricval, @metricthreshold);
                                                        FETCH NEXT FROM servercursor INTO @registeredserverid,
                                                        @connection;
                                                END;

                                                CLOSE servercursor;
                                                DEALLOCATE servercursor;
                                        END;
                                END;
                        END;

                        DROP TABLE #servertbl;

                        IF (@isadmin = 1)
                        BEGIN
                                -- log the changes
                                DECLARE @msg nvarchar(128),
                                        @assessmentstate nchar(1);
                                SELECT
                                        @assessmentstate = assessmentstate,
                                        @msg = N'Refreshed assessment findings from audit data'
                                FROM assessment
                                WHERE policyid = @policyid
                                AND assessmentid = @assessmentid;
                                EXEC isp_sqlsecure_addpolicychangelog @policyid = @policyid,
                                                                      @assessmentid = @assessmentid,
                                                                      @state = @assessmentstate,
                                                                      @description = @msg;
                        END;

                COMMIT TRANSACTION;
        END;

        -- return the results and add the explanations and the updated severitycode with an explanation
        -- severity codes will have 10 added if they are findings otherwise, if ok, then they are < 10
        SELECT
                a.*,
                isexplained = ISNULL(b.isexplained, 0),
                severitycodeexplained = severitycode
                + CASE
                        WHEN severitycode > 0 AND
                                ISNULL(b.isexplained, 0) = 0 THEN 10
                        ELSE 0
                END,
                notes = ISNULL(b.notes, N'')
        FROM @outtbl a
        LEFT JOIN policyassessmentnotes b
                ON b.policyid = @policyid
                AND b.assessmentid = @assessmentid
                AND a.metricid = b.metricid
                AND a.snapshotid = b.snapshotid
        ORDER BY a.snapshotid,
        a.metricid;

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_getpolicyassessment] TO [SQLSecureView];

GO
SET QUOTED_IDENTIFIER OFF;
GO
SET ANSI_NULLS ON;
GO