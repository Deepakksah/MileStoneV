-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_audio_message]') AND type in (N'P', N'PC'))
DROP PROCEDURE [create_audio_message]
GO

CREATE PROCEDURE [create_audio_message]
	@Id [GuidType],
	@Name [NameType],
	@Description NVARCHAR(MAX),
	@OriginalFileName [LongNameType],
	@Content VARBINARY(MAX),
	@MaxMessageCount INT = -1
AS
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @dataSetAfter XML
		
		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		IF (@MaxMessageCount >= 0)
		BEGIN
			DECLARE @messageCount INT
			SELECT @messageCount = COUNT(*)
			FROM [AudioMessage]

			IF (@messageCount >= @maxMessageCount)
			BEGIN
				;THROW 60321, @procedureName, 1;
			END
		END

		BEGIN TRY
			-- Insert the row
			INSERT INTO [AudioMessage]
			(
				[ID],
				[Name],
				[Description],
				[LastModified],
				[OriginalFileName],
				[Content]
			)
			VALUES 
			(
				@Id,
				@Name,
				@Description,
				@CurrentUtcTime,
				@OriginalFileName,
				@Content
			)
		END TRY
		BEGIN CATCH
			IF(ERROR_NUMBER() = 2601 OR ERROR_NUMBER() = 2627) -- CONSTRAINT VIOLATION
			BEGIN
				-- The name already already exists
				;THROW 60304, @procedureName, 1;
			END
			;THROW
		END CATCH

		-- Get DataSet after changes
		SET @dataSetAfter = dbo.audioMessageAsDataSet(@Id)

		-- Create diffgram task for server service
		EXEC [create_task_server_services_transaction]
			@type = 'diffgram_insert',
			@data_a = @dataSetAfter,
			@current_time = @CurrentUtcTime
	
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