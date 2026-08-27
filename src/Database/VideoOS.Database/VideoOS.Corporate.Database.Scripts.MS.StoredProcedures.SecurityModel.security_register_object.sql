-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_register_object]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_register_object]
GO

CREATE PROCEDURE [security_register_object]
	@namespace_id NVARCHAR(260),
	@user_sid [SIDType] = NULL,
	@object_id NVARCHAR(300),
	@object_name NVARCHAR(100) = '',
	@object_type TINYINT = 2,
	@parent_path NVARCHAR(400) = '/',
	@parent_object_id NVARCHAR(260) = NULL
AS
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRAN

		-- Lock tables for update
		EXEC [security_table_lock] @LockLevel = 0

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		-- Get the name of the namespace from the namespace id
		DECLARE @namespace_name NVARCHAR(260)
		SELECT @namespace_name = [name] FROM [SecurityNamespace] WHERE [namespace_id] = @namespace_id

		IF(@parent_object_id IS NOT NULL AND @parent_object_id <> '/')
		BEGIN
			SELECT @parent_path = [full_path] + '/' FROM [SecurityObject] WHERE [object_id] = @parent_object_id AND [namespace_id] = @namespace_id
		END
		ELSE
		BEGIN
			SELECT @parent_path = '/', @parent_object_id = '/'
		END
	
		EXEC [security_register_object_transaction]
			@namespace_name,
			@object_id,
			@object_type,
			@object_name,
			@parent_path,
			@CurrentUtcTime,
			@parent_object_id

		-- TODO: We need to handle if security changed
		SELECT CAST(1 AS BIT) AS security_changed

		COMMIT TRAN
	END TRY
	BEGIN CATCH 
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION
		END;
		DECLARE @shouldReThrowError BIT = dbo.[ShouldReThrowError](ERROR_NUMBER(), ERROR_SEVERITY())
		IF(@shouldReThrowError = 1)
			THROW;
		SET @ErrorArgs = dbo.[FormatErrorMessage](ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE())
		;THROW 60000, @ErrorArgs, 1;
	END CATCH;
END
GO