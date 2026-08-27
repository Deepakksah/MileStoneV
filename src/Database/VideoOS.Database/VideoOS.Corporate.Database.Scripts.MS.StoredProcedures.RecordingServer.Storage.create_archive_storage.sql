-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_archive_storage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [create_archive_storage]
GO

CREATE PROCEDURE [create_archive_storage]
	@pkid [GuidType],
	@recordingstorage_id [GuidType],
	@name [NameType],
	@description NVARCHAR(MAX),
	@path NVARCHAR(512),
	@retain_minutes INT,
	@max_size_mb BIGINT,
	@framerate_reduction_enabled BIT,
	@target_framerate FLOAT,
	@schedule XML,
	@session_id [GuidType],
	@custom_properties XML = NULL,
	@archives_per_storage_limit INT = 2147483647
 AS
BEGIN
	SET NOCOUNT ON

		-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY

		BEGIN TRAN
		DECLARE @archives_on_storage AS INT
		SELECT @archives_on_storage = COUNT([pkid])
		FROM [ArchiveStorage]
		WHERE [recordingstorage_id] = @recordingstorage_id
		IF (@archives_on_storage >= @archives_per_storage_limit)
		BEGIN
			;THROW 64002, @procedureName, 1;
		END

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		DECLARE @retain_minutes_low_limit AS INT
		DECLARE @recordingstorage_name [NameType]
		DECLARE @recordingserver_id [GuidType]
		DECLARE @recorder_service_id [GuidType]
		DECLARE @recordingserver_name [NameType]

		-- Ensure that retain_minutes is not <= RecordingStorage.retain_minutes
		SELECT @retain_minutes_low_limit = [retain_minutes]
		FROM [RecordingStorage] WITH(UPDLOCK)
		WHERE [pkid] = @recordingstorage_id
		
		IF (@retain_minutes <= @retain_minutes_low_limit) -- if @retain_minutes_low_limit is null then condition is false and there is no problem
		BEGIN
			;THROW 60145, @procedureName, 1;
		END
	
		-- Ensure that the retain_minutes are unique for archives in this storage
		IF EXISTS (	SELECT [pkid] 
					FROM [ArchiveStorage] WITH(UPDLOCK)
					WHERE 
						[recordingstorage_id] = @recordingstorage_id
						AND [retain_minutes] = @retain_minutes)
		BEGIN
			;THROW 60146, @procedureName, 1;
		END
	
		-- Extract info for logging and task creation
		SELECT @recordingserver_name = r.[Name],
				@recordingserver_id = rs.[recordingserver_id],
				@recorder_service_id = rssr.[recording_server_service_id],
				@recordingstorage_name = rs.[Name]
		FROM [RecordingStorage] rs
		INNER JOIN [Recorders] r ON rs.[recordingserver_id] = r.[IDRecorder]
		INNER JOIN [RecordingServerServiceRecorder] rssr ON rssr.[recorder_id] = r.[IDRecorder] AND rssr.[type] = 0
		WHERE rs.[pkid] = @recordingstorage_id
	
		BEGIN TRY
			INSERT [ArchiveStorage] (
				[pkid],
				[recordingstorage_id],
				[name],
				[description],
				[path],
				[retain_minutes],
				[max_size_mb],
				[target_framerate],
				[schedule],
				[created_date],
				[framerate_reduction_enabled],
				[folder],
				[last_modified],
				[bank_id],
				[mounted]
			)
			VALUES (
				@pkid,
				@recordingstorage_id,
				@name,
				@description,
				@path,
				@retain_minutes,
				@max_size_mb,
				@target_framerate,
				@schedule,
				@CurrentUtcTime,
				@framerate_reduction_enabled,
				@pkid,
				@CurrentUtcTime,
				NEWID(),
				1
			)
		END TRY
		BEGIN CATCH
			IF(ERROR_NUMBER() = 2601 OR ERROR_NUMBER() = 2627) -- CONSTRAINT VIOLATION
			BEGIN
				-- The name already exists on the recording storage
				SET @ErrorArgs = @procedureName + N': %%Name=' + @name + N'%%; %%recordingServerName=' + @recordingstorage_name + N'%%'
				;THROW 60049, @ErrorArgs, 1;
			END
			;THROW
		END CATCH

		DECLARE @diffgram XML
		SELECT @diffgram = dbo.[archiveStorageAsDataSet](@pkid)

		-- Create diffgram
		EXEC [create_task_transaction]
			@type = 'diffgram_insert',
			@target = @recorder_service_id,
			@data_a = @diffgram,
			@current_time = @CurrentUtcTime

		-- Update custom properties
		EXEC [update_custom_properties_transaction]
			@object_id = @pkid,
			@custom_properties = @custom_properties
	
		EXEC create_pending_log @session_id, 20005, @recordingstorage_id, @recordingstorage_name, @recordingserver_id, @recordingserver_name, @name
		COMMIT TRAN

		RETURN 0

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
