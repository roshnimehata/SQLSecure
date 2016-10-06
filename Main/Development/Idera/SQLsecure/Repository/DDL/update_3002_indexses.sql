IF NOT EXISTS ( SELECT  *
                FROM    sys.indexes
                WHERE   object_id = OBJECT_ID(N'dbo.serversnapshot')
                        AND name = N'IX_connectionname' )
    BEGIN
        CREATE NONCLUSTERED INDEX IX_connectionname
        ON dbo.serversnapshot(connectionname)
        INCLUDE(snapshotid,starttime)
        WITH (
        PAD_INDEX = OFF
        )
    END
GO


IF NOT EXISTS ( SELECT  *
                FROM    sys.indexes
                WHERE   object_id = OBJECT_ID(N'dbo.policyassessmentdetail')
                        AND name = N'IX_policyassessmentdetail_policyid' )
    BEGIN
        CREATE NONCLUSTERED INDEX IX_policyassessmentdetail_policyid
        ON dbo.policyassessmentdetail(policyid,assessmentid)
        INCLUDE(policyassessmentdetailid)
        WITH (
        PAD_INDEX = OFF
        )
    END
GO
IF NOT EXISTS ( SELECT  *
                FROM    sys.indexes
                WHERE   object_id = OBJECT_ID(N'dbo.databaseobjectpermission')
                        AND name = N'IX_snapshot_dbid' )
    BEGIN
        CREATE NONCLUSTERED INDEX IX_snapshot_dbid
        ON dbo.databaseobjectpermission(snapshotid,dbid,grantee)
        INCLUDE(objectid,permission,isgrant,isgrantwith)
        WITH (
        PAD_INDEX = OFF
        )
    END
GO
IF NOT EXISTS ( SELECT  *
                FROM    sys.indexes
                WHERE   object_id = OBJECT_ID(N'dbo.databaseobject')
                        AND name = N'IX_snapshotid_type' )
    BEGIN
        CREATE NONCLUSTERED INDEX IX_snapshotid_type
        ON dbo.databaseobject(snapshotid,type,userdefined,name)
        WITH (
        PAD_INDEX = OFF
        )
    END
GO
IF NOT EXISTS ( SELECT  *
                FROM    sys.indexes
                WHERE   object_id = OBJECT_ID(N'dbo.databaseobject')
                        AND name = N'IX_snapshotid_runatstartup' )
    BEGIN
        CREATE NONCLUSTERED INDEX IX_snapshotid_runatstartup
        ON dbo.databaseobject(snapshotid,runatstartup,type)
        INCLUDE(dbid,classid,parentobjectid,objectid,name)
        WITH (
        PAD_INDEX = OFF
        )
    END
GO


