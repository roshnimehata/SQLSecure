SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists ( select
                *
            from
                dbo.sysobjects
            where
                id = object_id(N'[dbo].[splitbydelimiter]')
                and xtype in ( N'FN', N'IF', N'TF' ) ) 
   drop function [dbo].[splitbydelimiter]
GO

create function [dbo].[splitbydelimiter]
       (
         @String nvarchar(max),
         @Delimiter nvarchar(10)
       )
returns @Result table ( Value nvarchar(max) )
as -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --             splits string into table by delimiter,
   --
   -- Returns: split value table
    begin 
        declare @Values xml  
        select
            @Values = cast('<Value>' + replace(@String, @Delimiter,
                                               '</Value><Value>') + '</Value>' as xml) 
        insert  into @Result
                select
                    replace(rtrim(ltrim(t.value('.', 'nvarchar(max)'))),'''','') as Value
                from
                    @Values.nodes('/Value') as V ( t ) 
        return 
    end
GO 

grant select on [dbo].[splitbydelimiter] to [SQLSecureView]

GO
set QUOTED_IDENTIFIER off 
GO
set ANSI_NULLS on 
GO