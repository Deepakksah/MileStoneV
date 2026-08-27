-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_hosted_hardware]') AND type = 'P')
DROP PROCEDURE [delete_hosted_hardware]
GO

CREATE PROCEDURE [delete_hosted_hardware]
	@pkid INT,
	@session_id [GuidType]
AS
BEGIN
	SET NOCOUNT ON;

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRANSACTION

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		-- Extract info for logging
		DECLARE @name [NameType]

		SELECT @name = [name]
		FROM [HostedHardware] WITH(UPDLOCK)
		WHERE [pkid] = @pkid

		-- Does the hosted hardware exist?
		IF NOT EXISTS(
			SELECT *
			FROM [HostedHardware] WITH(UPDLOCK)
			WHERE [pkid] = @pkid
		)
		BEGIN
			SET @ErrorArgs = @procedureName + N': %%pkid=' + CAST(@pkid AS NVARCHAR(10)) + N'%%'
			;THROW 60614, @ErrorArgs, 1;
		END

		DECLARE @diffgram XML
		SELECT @diffgram = dbo.[hostedHardwareAsDataSet](@pkid)

		DELETE [HostedHardware]
		WHERE [pkid] = @pkid

		-- Create diffgram
		EXEC [create_task_transaction]
				@type = 'diffgram_delete',
				@target = '909749AE-88C4-4780-A381-383D691E98FA',
				@data_a = @diffgram,
				@current_time = @CurrentUtcTime
	
		COMMIT TRANSACTION

		-- AudithostedHardwareDeleted = 20215
		EXEC create_pending_log @session_id, 20215, @pkid, @name
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
