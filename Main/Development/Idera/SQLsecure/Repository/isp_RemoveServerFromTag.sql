
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE PROCEDURE [dbo].[isp_RemoveServerFromTag]
    @tag_id INT ,
    @server_id INT
AS
    BEGIN
        DELETE  FROM dbo.server_tags
        WHERE   server_id = @server_id
                AND tag_id = @tag_id;

    END;
