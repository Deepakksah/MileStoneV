-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_userupdatedcache_create_or_update]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_userupdatedcache_create_or_update]
GO

CREATE PROCEDURE [security_userupdatedcache_create_or_update]
	@IDUser UNIQUEIDENTIFIER,
	@LastModified DATETIME2
AS
BEGIN
	SET NOCOUNT ON

	IF EXISTS(SELECT IDUser FROM [SecurityUserUpdatedCache] WHERE IDUser = @IDUser)
	BEGIN
		UPDATE [SecurityUserUpdatedCache]
		SET [LastModified] = @LastModified
		WHERE IDUser = @IDUser
	END
	ELSE
	BEGIN
		INSERT INTO [SecurityUserUpdatedCache] (IDUser, LastModified)
		VALUES(@IDUser, @LastModified) 
	END
END
GO