
/****** Object:  StoredProcedure [dbo].[isp_AssignTagsToServer]    Script Date: 7/21/2016 5:48:13 AM ******/
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_AssignTagsToServer]'))
drop procedure [dbo].[isp_AssignTagsToServer]
go

CREATE PROCEDURE [dbo].[isp_AssignTagsToServer]
    @tag_ids NVARCHAR(MAX) = NULL ,
    @server_id INT
AS
    BEGIN
        DECLARE @delimiter AS NVARCHAR(1);
        SET @delimiter = ',';
        IF ISNULL(@tag_ids, '') = ''
            BEGIN 
    
                INSERT  INTO dbo.server_tags
                        SELECT  @server_id ,
                                tag_id
                        FROM    tags t
                        WHERE   is_default = 1
                                AND NOT EXISTS ( SELECT 1
                                                 FROM   server_tags st
                                                 WHERE  st.tag_id = t.tag_id
                                                        AND st.server_id = @server_id );
            END;
        ELSE
            BEGIN 
                INSERT  INTO dbo.server_tags
                        SELECT  @server_id ,
                                Value
                        FROM    dbo.splitbydelimiter(@tag_ids, @delimiter)
                        WHERE   NOT EXISTS ( SELECT 1
                                             FROM   server_tags st
                                             WHERE  st.tag_id = Value
                                                    AND st.server_id = @server_id );
	

            END;
    END;

	