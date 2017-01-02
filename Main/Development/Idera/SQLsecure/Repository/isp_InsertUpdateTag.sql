
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_InsertUpdateTag]'))
drop procedure [dbo].[isp_InsertUpdateTag]
go

CREATE PROCEDURE [dbo].[isp_InsertUpdateTag]
    @tag_id INT = NULL ,
    @tag_name NVARCHAR(250) ,
    @description NVARCHAR(500)
AS
    BEGIN
        IF @tag_id IS NOT NULL
            BEGIN
                UPDATE  dbo.tags
                SET     name = @tag_name ,
                        description = @description
                WHERE   tag_id = @tag_id;
            END;
        ELSE
            BEGIN
                INSERT  INTO dbo.tags
                        ( name, description )
                VALUES  ( @tag_name, -- name - nvarchar(250)
                          @description-- description - nvarchar(500)
                          );
            END;
    
    END;