
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_DeleteTag]'))
drop procedure [dbo].[isp_DeleteTag]
go

CREATE PROCEDURE [dbo].[isp_DeleteTag] @tag_id INT
AS
    BEGIN
        DELETE  FROM tags
        WHERE   tag_id = @tag_id
                AND ISNULL(is_default, 0) = 0;

      
    END;