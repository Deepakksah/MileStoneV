-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_unregister_object_transaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_unregister_object_transaction]
GO

CREATE PROCEDURE [security_unregister_object_transaction]
	@namespace_name NVARCHAR(260),
	@object_id NVARCHAR(300),
	@CurrentUtcTime DATETIME
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	DECLARE @rowCount INT

	DECLARE @security_object_id UNIQUEIDENTIFIER

	-- Make sure that a current transaction is active
	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END

	DECLARE @namespace_id UNIQUEIDENTIFIER
	SELECT @namespace_id = [namespace_id] FROM [SecurityNamespace] WHERE [name] = @namespace_name

	SELECT @security_object_id = [security_object_id]
	FROM [SecurityObject]
	WHERE [namespace_id] = @namespace_id
		AND [object_id] = @object_id

	-- Verify that the object exists
	IF(@security_object_id IS NULL)
	BEGIN
		SET @ErrorArgs = @procedureName + N': %%IDObject=' + @object_id + N'%%'
		;THROW 60012, @ErrorArgs, 1;
	END
	
	DECLARE @currentLevel INT
	SET @currentLevel = 0

	DECLARE @objectList AS TABLE (
		[security_object_id] UNIQUEIDENTIFIER,
		[level] INT
	)

	INSERT @objectList ([security_object_id], [level])
	VALUES (@security_object_id, @currentLevel)

	-- Delete all access control entries on the object
	DELETE aces
	FROM [SecurityAccessEntry] aces
	INNER JOIN @objectList objCache ON aces.[security_object_id] = objCache.[security_object_id]

	-- Delete the security object and its children
	DELETE so
	FROM [SecurityObject] so
	INNER JOIN @objectList objCache ON so.[security_object_id] = objCache.[security_object_id]

	-- Now that the security permissions are changed, we need to clear the permission cache
	-- This will be populated again when needed.
	DELETE FROM [SecurityAccessCache2]
END
GO