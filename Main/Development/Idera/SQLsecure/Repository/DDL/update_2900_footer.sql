--put here some changes
/*
declare @ver int
SELECT @ver=schemaversion FROM currentversion
if (isnull(@ver, 900) <= 2899)	-- Check to prevent this from running in future upgrades, but run for all prior versions at any version level because the version gets updated later
Begin

end
go*/


