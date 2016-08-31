SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_addcollectorinfo]'))
drop procedure [dbo].[isp_sqlsecure_addcollectorinfo]
GO

CREATE procedure [dbo].[isp_sqlsecure_addcollectorinfo] (@infoname nvarchar(64), @infovalue nvarchar(1000))
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Adds new collector information to database table for job scheduler processing

	declare @errmsg nvarchar(500)

	if (@infoname IS NULL) 
	begin
		set @errmsg = 'Error: Info name cannot be null'
		RAISERROR (@errmsg, 16, 1)
		return -1
	end

	if (@infovalue IS NULL) 
	begin
		set @errmsg = 'Error: Info value cannot be null'
		RAISERROR (@errmsg, 16, 1)
		return -1
	end	

	if exists (select * from collectorinfo where UPPER(name) = UPPER(@infoname))
	begin
		update collectorinfo set value = @infovalue,  lastmodifiedby = SYSTEM_USER, lastmodifiedtm=GETUTCDATE() where UPPER(name) = UPPER(@infoname)
	end
	else
	begin
		insert into collectorinfo (name, value, lastmodifiedby, lastmodifiedtm) values (@infoname, @infovalue, SYSTEM_USER, GETUTCDATE())
	end

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_addcollectorinfo] TO [SQLSecureView]

GO

