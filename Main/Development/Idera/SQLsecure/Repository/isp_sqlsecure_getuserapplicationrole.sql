SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[isp_sqlsecure_getuserapplicationrole]'))
drop procedure [dbo].[isp_sqlsecure_getuserapplicationrole]
GO

CREATE procedure [dbo].[isp_sqlsecure_getuserapplicationrole]
as
   -- <Idera SQLsecure version and copyright>
   --
   -- Description :
   --              Check if connecting user has sqlsecure admin, loader or view priviilege. 
   --              If sysadmin then return admin and loader
   --              If db_owner then return loader and view
   -- 	           If belongs to SQLSecureView role then return view
   -- 	           All others return no access

	declare @result nvarchar(16)
	declare @err int
	declare @errmsg nvarchar(500)

	if object_id('#tmpdata') is null 
	begin
		create table #tmpdata (data nvarchar(16))
	end

	delete from #tmpdata

	select @result = CASE WHEN IS_SRVROLEMEMBER('sysadmin') = 1 THEN 'admin' ELSE 'no access' END
	
	if (@result = 'admin')
	begin

		BEGIN TRAN
	
			insert into #tmpdata (data) values (@result)
			insert into #tmpdata (data) values ('loader')
	
			select @err = @@error
	
			if @err <> 0
			begin
				set @errmsg = 'Error: Failed to insert into temp table'
				RAISERROR (@errmsg, 16, 1)
				ROLLBACK TRAN
				return -1
			end
	
		COMMIT TRAN
	end 
	else
	begin

		select @result = CASE WHEN IS_MEMBER('db_owner') = 1 THEN 'view' WHEN IS_MEMBER('dbo') = 1 THEN 'view' ELSE 'no access' END
		
		if (@result = 'view')
		begin
	
			BEGIN TRAN
				
				insert into #tmpdata (data) values (@result)
				insert into #tmpdata (data) values ('loader')
		
				select @err = @@error
		
				if @err <> 0
				begin
					set @errmsg = 'Error: Failed to insert into temp table'
					RAISERROR (@errmsg, 16, 1)
					ROLLBACK TRAN
					return -1
				end
		
			COMMIT TRAN
		end 
		else 
		begin
			select @result = CASE WHEN IS_MEMBER('db_datawriter') = 1 THEN 'loader' ELSE 'no access' END

			if (@result = 'loader')
			begin
		
				BEGIN TRAN
					
					insert into #tmpdata (data) values (@result)
			
					select @err = @@error
			
					if @err <> 0
					begin
						set @errmsg = 'Error: Failed to insert into temp table'
						RAISERROR (@errmsg, 16, 1)
						ROLLBACK TRAN
						return -1
					end
			
				COMMIT TRAN
			end 
			else
			begin
				select @result = CASE WHEN IS_MEMBER('SQLSecureView')= 1 THEN 'view' ELSE 'no access' END
				
				BEGIN TRAN
					
					insert into #tmpdata (data) values (@result)
			
					select @err = @@error
			
					if @err <> 0
					begin
						set @errmsg = 'Error: Failed to insert into temp table'
						RAISERROR (@errmsg, 16, 1)
						ROLLBACK TRAN
						return -1
					end
			
				COMMIT TRAN

			end
			
		end
	end
	
	exec('select * from #tmpdata')

GO

GRANT EXECUTE ON [dbo].[isp_sqlsecure_getuserapplicationrole] TO [SQLSecureView]

GO

