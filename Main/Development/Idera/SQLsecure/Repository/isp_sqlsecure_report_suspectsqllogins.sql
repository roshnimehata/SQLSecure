USE [SQLsecure];
GO
/****** Object:  StoredProcedure [dbo].[isp_sqlsecure_report_suspectsqllogins]    Script Date: 7/25/2016 1:04:27 AM ******/
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO


CREATE PROCEDURE [dbo].[isp_sqlsecure_report_suspectsqllogins]
    (
      @rundate DATETIME = NULL ,	--defaults to all
      @policyid INT = 1 ,			--defaults to all
      @usebaseline BIT = 0 ,		--defaults to false
      @serverName NVARCHAR(400)
    )
AS -- <Idera SQLsecure version and copyright>
   --
  
    CREATE TABLE #tmpservers
        (
          registeredserverid INT
        );
    INSERT  #tmpservers
            EXEC [dbo].[isp_sqlsecure_getpolicymemberlist] @policyid = @policyid;  

    SELECT  snapshotid ,
            connectionname
    INTO    #ids
    FROM    serversnapshot d
    WHERE   d.snapshotid IN (
            SELECT  snapshotid
            FROM    dbo.getsnapshotlist(@rundate, @usebaseline) )
            AND d.registeredserverid IN ( SELECT    registeredserverid
                                          FROM      #tmpservers )
            AND UPPER(d.connectionname) LIKE UPPER(@serverName);
            
            
	            
    SELECT  d.connectionname ,
            a.name ,
            [type] = CASE WHEN a.type = 'G' THEN 'Group'
                          ELSE 'User'
                     END ,
		-- if there are any resolved accounts in the domain
		--		then this account is likely an orphan
		--		otherwise the entire domain is suspect
            '' [state]
    FROM    vwserverprincipal a
            INNER JOIN #ids d ON a.snapshotid = d.snapshotid
    WHERE   NOT EXISTS ( SELECT 1
                         FROM   serverpermission perm
                         WHERE  perm.grantee = a.principalid
                                AND perm.isgrant = 'Y'
                                 AND perm.snapshotid = d.snapshotid  )
            AND NOT EXISTS ( SELECT 1
                             FROM   databaseobjectpermission dbp
                             WHERE  dbp.grantee = a.principalid
                                    AND dbp.isgrant = 'Y'
                                    AND dbp.snapshotid = d.snapshotid  )
            AND NOT EXISTS ( SELECT 1
                             FROM   databaseprincipalpermission dbp
                             WHERE  dbp.grantee = a.principalid
                                    AND dbp.isgrant = 'Y'
                                    AND dbp.snapshotid = d.snapshotid  )
            AND NOT EXISTS ( SELECT 1
                             FROM   databaseschemapermission dbp
                             WHERE  dbp.grantee = a.principalid
                                    AND dbp.isgrant = 'Y'
                                    AND dbp.snapshotid = d.snapshotid  )
    GROUP BY d.connectionname ,
            a.name ,
            a.type
    ORDER BY d.connectionname ,
            a.name;

    DROP TABLE #tmpservers;
    DROP TABLE #ids;

