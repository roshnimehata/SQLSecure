/* Version Management

	This script will update the version information in the SQLsecure database.
	It is designed to work for either a new install or an upgrade install and will
	either insert or update the version information as well as remove compatibility
	for versions not listed in the version info.
*/

DECLARE @schematype nvarchar(20), @daltype nvarchar(20),
		@schemaversion int, @dalversion int,
		@schemacompat nvarchar(500), @dalcompat nvarchar(500),
		@idx smallint, @str nvarchar(20), @sql nvarchar(2000)

SELECT	@schematype = 'schema', @daltype = 'dal'

-- **************************** SET THE CURRENT VERSION INFO HERE

-- The current schema version (int)
SELECT	@schemaversion = 3001
-- The current dal version (int)
SELECT	@dalversion = 3001
-- Compatible schema versions (nvarchar - comma separated list of integers with no delimiters)
SELECT	@schemacompat = '3001'
-- Compatible dal versions (nvarchar - comma separated list of integers with no delimiters)
SELECT	@dalcompat = '3001'

-- **************************** END SET CURRENT VERSION INFO

-- Begin update

/*		I'm keeping this code in case we need it in the future
-- Remove versions that are no longer compatible
--		Delete schema versions
@sql = 'DELETE compatibleversion WHERE objecttype = ' + @schematype + 
			' AND compatibleversion IN (' + @schemacompat + ')'
exec @sql
--		Delete dal versions
@sql = 'DELETE compatibleversion WHERE objecttype = ' + @daltype + 
			' AND compatibleversion IN (' + @dalcompat + ')'
exec @sql
*/

-- Remove all old compatibility info
DELETE compatibleversion 



-- Set the current version information
IF ( EXISTS ( SELECT dalversion FROM currentversion ) )
	UPDATE currentversion SET schemaversion=@schemaversion, dalversion=@dalversion
ELSE
	INSERT INTO currentversion (schemaversion, dalversion) VALUES (@schemaversion, @dalversion)


-- Update compatibility (assumes the table has been cleared and does not check for dups)

-- Update schema compatibility
WHILE @schemacompat <> ''
BEGIN
    SET @idx = CHARINDEX(',', @schemacompat)
    IF ( @idx > 0 )
        BEGIN
            SET @str = CAST(LEFT(@schemacompat, @idx-1) AS INT)
            SET @schemacompat = RIGHT(@schemacompat, LEN(@schemacompat)-@idx)
        END
    ELSE
        BEGIN
            SET @str = CAST(@schemacompat AS INT)
            SET @schemacompat = ''
        END
    SELECT @sql = 'INSERT INTO compatibleversion (objecttype, compatibleversion)' +
							' VALUES(''' + @schematype + ''',' + @str +')'
    EXEC(@sql)
END

-- Update dal compatibility
WHILE @dalcompat <> ''
BEGIN
    SET @idx = CHARINDEX(',', @dalcompat)
    IF ( @idx > 0 )
        BEGIN
            SET @str = CAST(LEFT(@dalcompat, @idx-1) AS INT)
            SET @dalcompat = RIGHT(@dalcompat, LEN(@dalcompat)-@idx)
        END
    ELSE
        BEGIN
            SET @str = CAST(@dalcompat AS INT)
            SET @dalcompat = ''
        END
    SELECT @sql = 'INSERT INTO compatibleversion (objecttype, compatibleversion)' +
							' VALUES(''' + @daltype + ''',' + @str +')'
    EXEC(@sql)
END


GO

