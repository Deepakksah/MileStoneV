-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_audio_message]') AND type = 'P')
DROP PROCEDURE [delete_audio_message]
GO

CREATE PROCEDURE [delete_audio_message]
	@Id [GuidType],
	@session_id [GuidType]
AS
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @dataSetBefore XML
		
		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		-- Get name for logging
		DECLARE @name [NameType]
		SELECT @name = [Name]
		FROM [AudioMessage] WITH(UPDLOCK)
		WHERE [ID] = @Id

		-- Ensure that audio message exists
		IF (@name IS NULL)
		BEGIN
			;THROW 60305, @procedureName, 1;
		END

		-- Get data set before changes
		SET @dataSetBefore = dbo.audioMessageAsDataSet(@Id)

		-- Delet the row
		DELETE  [AudioMessage]
		WHERE   [Id] = @Id
		
		-- Create diffgram task for server service
		EXEC [create_task_server_services_transaction]
			@type = 'diffgram_delete',
			@data_a = @dataSetBefore,
			@current_time = @CurrentUtcTime
		
		COMMIT TRAN

		EXEC create_pending_log @session_id, 23018, @Id, @name -- E.g. Audio message '{0}' deleted.
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