
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE PROCEDURE [dbo].[isp_AssignTagToServers]
    @tag_id INT ,
    @servers NVARCHAR(MAX)
AS
    BEGIN
        DECLARE @delimiter AS NVARCHAR(1);
        SET @delimiter = ',';
        INSERT  INTO dbo.server_tags
                SELECT  registeredserverid ,
                        @tag_id
                FROM    dbo.registeredserver r
                WHERE   r.connectionname COLLATE SQL_Latin1_General_CP1_CI_AS IN (
                        SELECT  Value
                        FROM    dbo.splitbydelimiter(@servers, @delimiter) )
                        AND r.registeredserverid NOT IN ( SELECT
                                                              server_id
                                                          FROM
                                                              server_tags
                                                          WHERE
                                                              tag_id = @tag_id );

      
    END;
