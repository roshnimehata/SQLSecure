
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE PROCEDURE [dbo].[isp_GetTags]
    @tag_name NVARCHAR(250) = NULL ,
    @tag_id INT = NULL
AS
    BEGIN
        SELECT  t.tag_id ,
                t.name ,
                t.description ,
                ISNULL(t.is_default, 0) AS is_default
        FROM    dbo.tags t
        WHERE   ( @tag_name IS NULL
                  AND @tag_id = t.tag_id
                )
                OR ( @tag_id IS NULL
                     AND t.name COLLATE SQL_Latin1_General_CP1_CI_AS = @tag_name COLLATE SQL_Latin1_General_CP1_CI_AS
                   )
                OR ( @tag_name IS NULL
                     AND @tag_id IS NULL
                   );

        SELECT  st.tag_id ,
                rs.servername ,
                rs.registeredserverid
        FROM    dbo.registeredserver rs
                JOIN dbo.server_tags st ON st.server_id = rs.registeredserverid
                JOIN dbo.tags t ON t.tag_id = st.tag_id
        WHERE   ( @tag_name IS NULL
                  AND @tag_id = t.tag_id
                )
                OR ( @tag_id IS NULL
                     AND t.name COLLATE SQL_Latin1_General_CP1_CI_AS = @tag_name COLLATE SQL_Latin1_General_CP1_CI_AS
                   );

    END;
