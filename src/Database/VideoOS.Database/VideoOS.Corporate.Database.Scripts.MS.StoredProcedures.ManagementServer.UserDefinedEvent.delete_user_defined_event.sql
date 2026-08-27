-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_user_defined_event]') AND type = 'P')
DROP PROCEDURE [delete_user_defined_event]
GO

CREATE PROCEDURE [delete_user_defined_event]
	@IDEventType GuidType
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRANSACTION

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		-- Does the user-defined event exist?
		IF NOT EXISTS(
			SELECT *
			FROM [EventTypes] WITH(UPDLOCK)
			WHERE [IDEventType] = @IDEventType AND [GeneratorType] = 'External')
		BEGIN
			SET @ErrorArgs = @procedureName + N': %%IDEventType=' + CAST(@IDEventType AS NVARCHAR(36)) + N'%%'
			;THROW 60044, @ErrorArgs, 1;
		END

		DECLARE @diffgram XML
		SELECT @diffgram = dbo.[userDefinedEventAsDataSet](@IDEventType)

		DELETE [EventTypes]
		WHERE [IDEventType] = @IDEventType AND [GeneratorType] = 'External'

		-- Create diffgram
		EXEC [create_task_transaction]
			@type = 'diffgram_delete',
			@target = '909749AE-88C4-4780-A381-383D691E98FA',
			@data_a = @diffgram,
			@current_time = @CurrentUtcTime

		EXEC [delete_custom_properties_transaction]
			@object_id = @IDEventType
	
		DECLARE @namespace NVARCHAR(260) = 'UserDefinedEvent'

		-- Unregister security object
		EXEC [security_unregister_object_transaction]
			@namespace_name = @namespace,
			@object_id = @IDEventType,
			@CurrentUtcTime = @CurrentUtcTime

		COMMIT TRANSACTION
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