declare @ver int

SELECT @ver=schemaversion FROM currentversion
if (isnull(@ver, 900) <= 2799)
BEGIN
	print 'test'
END

GO
