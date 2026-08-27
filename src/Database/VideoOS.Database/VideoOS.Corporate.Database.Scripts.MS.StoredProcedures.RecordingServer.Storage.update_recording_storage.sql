-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_recording_storage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_recording_storage]
GO

CREATE PROCEDURE [update_recording_storage]
	@pkid [GuidType],
	@name [NameType],
	@description NVARCHAR(MAX),
	@retain_minutes INT,
	@max_size_mb BIGINT,
	@signing BIT,
	@session_id [GuidType],
	@custom_properties XML = NULL
 AS
BEGIN
	SET NOCOUNT ON

		-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @rowCount INT
		DECLARE @dataSetBefore XML
		DECLARE @dataSetAfter XML
		DECLARE @recorder_service_id [GuidType]
		DECLARE @recordingserver_id [GuidType]
		DECLARE @recordingServerName [NameType]

		DECLARE @retain_minutes_high_limit AS INT

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		BEGIN TRAN

		-- Ensure that we lock the recording storage row first (avoid potential dead-lock)
		SELECT @retain_minutes_high_limit = [retain_minutes]
		FROM [RecordingStorage] WITH(UPDLOCK)
		WHERE  [pkid] = @pkid
	
		-- Extract info for logging
		SELECT @recordingServerName = r.[Name],
			   @recordingserver_id = rs.[recordingserver_id],
			   @recorder_service_id = rssr.[recording_server_service_id]
		FROM
			[RecordingStorage] rs
			INNER JOIN [Recorders] r ON rs.[recordingserver_id] = r.[IDRecorder]
			INNER JOIN [RecordingServerServiceRecorder] rssr ON rssr.[recorder_id] = r.[IDRecorder] AND rssr.[type] = 0
		WHERE rs.[pkid] = @pkid
			
		---- Ensure that retain_minutes is not being changed to a value larger than any archive storage retain_minutes
		SELECT @retain_minutes_high_limit = MIN(a.[retain_minutes])
		FROM [ArchiveStorage] a WITH(UPDLOCK)
		WHERE a.[recordingstorage_id] = @pkid
		
		IF (@retain_minutes >= @retain_minutes_high_limit) -- if @retain_minutes_high_limit is null then condition is false and there is no problem
		BEGIN
			;THROW 60147, @procedureName, 1;
		END

		-- Get DataSet before changes
		set @dataSetBefore = dbo.recordingStorageAsDataSet(@pkid)

		BEGIN TRY
			UPDATE [RecordingStorage]
			SET
				[name] = @name,
				[description] = @description,
				[retain_minutes] = @retain_minutes,
				[max_size_mb] = @max_size_mb,
				[signing] = @signing,
				[last_modified] = @CurrentUtcTime
			WHERE [pkid] = @pkid

			SELECT @rowCount = @@ROWCOUNT
		END TRY
		BEGIN CATCH
			IF(ERROR_NUMBER() = 2601 OR ERROR_NUMBER() = 2627) -- CONSTRAINT VIOLATION
			BEGIN
				-- The name already exists on the recorder
				SET @ErrorArgs = @procedureName + N': %%Name=' + @name + N'%%; %%recordingServerName=' + @recordingServerName + N'%%'
				;THROW 60026, @ErrorArgs, 1;
			END
			;THROW
		END CATCH

		IF(@rowCount = 0)
		BEGIN
			;THROW 60027, @procedureName, 1;
		END
	
		-- Update custom properties
		EXEC [update_custom_properties_transaction]
			@object_id = @pkid,
			@custom_properties = @custom_properties

		-- Get DataSet after changes
		set @dataSetAfter = dbo.recordingStorageAsDataSet(@pkid)

		-- Call the create task procedure to create diffgram update task
		EXEC [create_task_transaction]
								'diffgram_update',
								@recorder_service_id,
								@dataSetBefore,
								@dataSetAfter,
								@CurrentUtcTime
	
		EXEC create_pending_log @session_id, 20002, @pkid, @name, @recordingserver_id, @recordingServerName

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
