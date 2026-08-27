-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_set_inheritance]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_set_inheritance]
GO

CREATE PROCEDURE [security_set_inheritance]
	@namespace_id UNIQUEIDENTIFIER,
	@object_id NVARCHAR(300),
	@parent_object_id NVARCHAR(400)
AS
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @security_object_id UNIQUEIDENTIFIER
		DECLARE @parent_security_object_id UNIQUEIDENTIFIER

		BEGIN TRAN

		-- Lock tables for update
		EXEC [security_table_lock] @LockLevel = 0

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()
	
		-- Ensure objectId exists
		SELECT @security_object_id = [security_object_id]
		FROM [SecurityObject]
		WHERE [namespace_id] = @namespace_id AND [object_id] = @object_id
	
		IF(@security_object_id IS NULL)
		BEGIN
			SET @ErrorArgs = @procedureName + N': %%IDObject=' + @object_id + N'%%'
			;THROW 60012, @ErrorArgs, 1;
		END
	
		SET @parent_security_object_id = NULL

		-- Ensure ParentObjectId is valid (existing or not specified)
		IF(@parent_object_id IS NOT NULL)
		BEGIN
			SELECT @parent_security_object_id = [security_object_id]
			FROM [SecurityObject]
			WHERE [namespace_id] = @namespace_id AND [object_id] = @parent_object_id
		
			IF(@parent_security_object_id IS NULL)
			BEGIN
				SET @ErrorArgs = @procedureName + N': %%IDObject=' + @parent_object_id + N'%%'
				;THROW 60012, @ErrorArgs, 1;
			END
		
			-- TODO: Check for circular reference
		END
	
		UPDATE [SecurityObject]
		SET [inherits_from_id] = @parent_security_object_id
		WHERE [namespace_id] = @namespace_id AND [object_id] = @object_id

		-- Now that the security permissions are changed, we need to clear the permission cache
		-- This will be populated again when needed.
		DELETE FROM [SecurityAccessCache2]

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