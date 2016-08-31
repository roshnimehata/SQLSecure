SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fn_normalizeversion]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[fn_normalizeversion]
GO

CREATE FUNCTION [dbo].[fn_normalizeversion] 
(
	@version    nvarchar(50)
)  
returns nvarchar(50)
as  
begin
	if(@version is null) return null;
	if(@version = '') return '';

	declare @first as int;
	declare @second as int;
	declare @third as int;
	declare @fourth as int;

	declare @dotIndex as int
	declare @currentVersionPartEndPosition as int
	declare @remainingVersionStartPosition as int

	select @dotIndex = charindex('.',@version)
	select @currentVersionPartEndPosition = isnull(nullif(@dotIndex -1, -1), 1) 
	select @remainingVersionStartPosition = isnull(nullif(@dotIndex, 0), 1) + 1
	set @first = CAST(ISNULL(NULLIF(substring(@version,1, @currentVersionPartEndPosition), ''), '0') AS INT)

	set @version = substring(@version, @remainingVersionStartPosition, len(@version))
	---------------------------------------------------------------
	select @dotIndex = charindex('.',@version)
	select @currentVersionPartEndPosition = isnull(nullif(@dotIndex -1, -1), 1)
	select @remainingVersionStartPosition = isnull(nullif(@dotIndex, 0), 1) + 1
	set @second = CAST(ISNULL(NULLIF(substring(@version,1, @currentVersionPartEndPosition), ''), '0') AS INT)

	set @version = substring(@version,@remainingVersionStartPosition, len(@version))
	---------------------------------------------------------------
	--select @version
	select @dotIndex = charindex('.',@version)
	select @currentVersionPartEndPosition = isnull(nullif(@dotIndex -1, -1), LEN(@version))
	select @remainingVersionStartPosition = isnull(nullif(@dotIndex, 0), LEN(@version))+1
	
	set @third = CAST(ISNULL(NULLIF(substring(@version,1, @currentVersionPartEndPosition), ''), '0') AS INT)
	
--select @third,@currentVersionPartEndPosition,@remainingVersionStartPosition,@dotIndex

	set @version = substring(@version,@remainingVersionStartPosition, len(@version))
	---------------------------------------------------------------
	set @fourth = CAST(ISNULL(NULLIF(@version, ''), '0') AS INT) 
	---------------------------------------------------------------
	
	return CAST(@first AS NVARCHAR) + '.' + CAST(@second AS NVARCHAR) + '.' + CAST(@third AS NVARCHAR) + '.' + CAST(@fourth AS NVARCHAR)

end;

/*
if(dbo.f_NormalizeVersion('') <> '') SELECT 'FAIL'
if(dbo.f_NormalizeVersion(null) IS NOT NULL) SELECT 'FAIL null'
if(dbo.f_NormalizeVersion('...') <> '0.0.0.0') SELECT 'FAIL ...'
if(dbo.f_NormalizeVersion('1...') <> '1.0.0.0') SELECT 'FAIL 1...'
if(dbo.f_NormalizeVersion('1') <> '1.0.0.0') SELECT 'FAIL 1'
if(dbo.f_NormalizeVersion('1.') <> '1.0.0.0') SELECT 'FAIL 1.'
if(dbo.f_NormalizeVersion('1.2') <> '1.2.0.0') SELECT 'FAIL 1.2'
if( dbo.f_NormalizeVersion('1.2.') <> '1.2.0.0') SELECT 'FAIL 1.2.'
if( dbo.f_NormalizeVersion('1.2.3') <> '1.2.3.0') SELECT 'FAIL 1.2.3'
if( dbo.f_NormalizeVersion('8.00.194') <> '8.0.194.0') SELECT 'FAIL 8.00.194'
if( dbo.f_NormalizeVersion('1.2.3.') <> '1.2.3.0') SELECT 'FAIL 1.2.3.'
if( dbo.f_NormalizeVersion('1.2.3.4') <> '1.2.3.4') SELECT 'FAIL 1.2.3.4'
if( dbo.f_NormalizeVersion('1.2..4') <> '1.2.0.4') SELECT 'FAIL 1.2..4'
if( dbo.f_NormalizeVersion('1...4') <> '1.0.0.4') SELECT 'FAIL 1...4'
if( dbo.f_NormalizeVersion('...4') <> '0.0.0.4') SELECT 'FAIL ...4'
if( dbo.f_NormalizeVersion('.') <> '0.0.0.0') SELECT 'FAIL .'
if( dbo.f_NormalizeVersion('..') <> '0.0.0.0') SELECT 'FAIL ..'
if( dbo.f_NormalizeVersion('...') <> '0.0.0.0') SELECT 'FAIL ...'
if( dbo.f_NormalizeVersion('1.2..') <> '1.2.0.0') SELECT 'FAIL 1.2..'
if( dbo.f_NormalizeVersion('1.002..') <> '1.2.0.0') SELECT 'FAIL 1.002..'
if( dbo.f_NormalizeVersion('1..00.') <> '1.0.0.0') SELECT 'FAIL 1..00.'
if( dbo.f_NormalizeVersion('1.002..00000') <> '1.2.0.0') SELECT 'FAIL 1.002..00000'
if( dbo.f_NormalizeVersion('1.200..004000') <> '1.200.0.4000') SELECT 'FAIL 1.200..004000'

Performs 5 times slower that select without normalizing. (1:36 vs 8:32 on 16435150 items)
*/

GO

GRANT EXECUTE ON [dbo].[fn_normalizeversion]   TO [SQLSecureView]

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


