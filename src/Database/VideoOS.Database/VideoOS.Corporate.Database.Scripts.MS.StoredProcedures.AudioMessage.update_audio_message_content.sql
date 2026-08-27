-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_audio_message_content]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_audio_message_content]
GO

CREATE PROCEDURE [update_audio_message_content]
	@Id [GuidType],
	@OriginalFileName [LongNameType],
	@Content VARBINARY(MAX),
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
		DECLARE @dataSetAfter XML

		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()
	
		-- Get name for logging
		DECLARE @name [NameType]
		SELECT @name = [Name]
		FROM [AudioMessage] WITH(UPDLOCK)
		WHERE [ID] = @Id

		-- Make sure the audio message exists
		IF (@name IS NULL)
		BEGIN
			;THROW 60315, @procedureName, 1;
		END

		-- Get DataSet before changes (for DiffGram)
		SET @dataSetBefore = dbo.audioMessageAsDataSet(@Id)

		-- Update the row
		UPDATE  [AudioMessage]
		SET     [OriginalFileName] = @OriginalFileName,
				[Content] = @Content,
				[LastModified] = @CurrentUtcTime
		WHERE   [ID] = @Id
		
		-- Get DataSet after changes
		SET @dataSetAfter = dbo.audioMessageAsDataSet(@Id)

		-- Create diffgram task for server service
		EXEC [create_task_server_services_transaction]
			@type = 'diffgram_update',
			@data_a = @dataSetBefore,
			@data_b = @dataSetAfter,
			@current_time = @CurrentUtcTime
		
		COMMIT TRAN

		EXEC create_pending_log @session_id, 23017, @id, @name -- E.g. The content of the audio message '{0}' is changed.
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