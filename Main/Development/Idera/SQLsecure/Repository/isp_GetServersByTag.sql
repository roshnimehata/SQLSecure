SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_GetServersByTag]'))
drop procedure [dbo].[isp_GetServersByTag]
go

CREATE PROCEDURE [dbo].[isp_GetServersByTag]
    @tag_name NVARCHAR(250) = NULL ,
    @tag_id INT = NULL
AS
    BEGIN
        SELECT  rs.connectionname  ,
                rs.registeredserverid ,
                st.tag_id
        FROM    dbo.registeredserver rs
                JOIN dbo.server_tags st ON st.server_id = rs.registeredserverid
                JOIN dbo.tags t ON t.tag_id = st.tag_id
        WHERE   ( @tag_name IS NULL
                  AND @tag_id = t.tag_id
                )
                OR ( @tag_id IS NULL
                     AND t.name = @tag_name
                   );
    END;
