-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[add_service_account]') AND type in (N'P', N'PC'))
DROP PROCEDURE [add_service_account]
GO

CREATE PROCEDURE add_service_account
	@accountName sysname
AS
BEGIN

	DECLARE @sqlString NVARCHAR(500)

	DECLARE @currentOwner sysname
	SELECT @currentOwner = SUSER_SNAME(owner_sid)
	FROM sys.databases 
	WHERE name = DB_NAME()

	SET @sqlString = 'ALTER AUTHORIZATION ON DATABASE::[' + DB_NAME() + '] TO [' + @currentOwner + ']'
    EXEC sp_executesql @sqlString

	IF(@currentOwner = @accountName)
	RETURN 0

	IF NOT EXISTS(SELECT * FROM sys.database_principals WHERE name = @accountName)
	BEGIN
		SET @sqlString = 'CREATE USER [' + @accountName + '] FOR LOGIN [' + @accountName + '] WITH DEFAULT_SCHEMA = [dbo]'
		EXEC sp_executesql @sqlString	
	END
	
	SET @sqlString = 'EXEC sp_addrolemember N''db_owner'', ''' + @accountName + ''''
	EXEC sp_executesql @sqlString

	RETURN 0
END
GO
