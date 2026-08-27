-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_recording_storage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [create_recording_storage]
GO

CREATE PROCEDURE [create_recording_storage]
	@pkid [GuidType],
	@recordingserver_id [GuidType],
	@name [NameType],
	@description NVARCHAR(MAX),
	@path NVARCHAR(512),
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
		BEGIN TRAN
  
		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()
	
		-- Extract info for logging
		DECLARE @recordingServerName [NameType]
		DECLARE @recorder_service_id [GuidType]

		SELECT @recordingServerName = r.[Name],
				@recorder_service_id = rssr.[recording_server_service_id]
		FROM [Recorders] r
		INNER JOIN [RecordingServerServiceRecorder] rssr ON rssr.[recorder_id] = r.[IDRecorder] AND rssr.[type] = 0
		WHERE r.[IDRecorder] = @recordingserver_id	
		
		BEGIN TRY

			INSERT [RecordingStorage] (
				[pkid],
				[recordingserver_id],
				[name],
				[description],
				[path],
				[is_default],
				[retain_minutes],
				[max_size_mb],
				[signing],
				[created_date],
				[last_modified],
				[bank_id]
			)
			VALUES (
				@pkid,
				@recordingserver_id,
				@name,
				@description,
				@path,
				0,
				@retain_minutes,
				@max_size_mb,
				@signing,
				@CurrentUtcTime,
				@CurrentUtcTime,
				NEWID()
			)
		END TRY
		BEGIN CATCH

			IF(ERROR_NUMBER() = 2601 OR ERROR_NUMBER() = 2627) -- CONSTRAINT VIOLATION
			BEGIN
				-- The name already exists on the recorder
				SET @ErrorArgs = @procedureName + N': %%Name=' + @name + N'%%; %%recordingServerName=' + @recordingServerName + N'%%'
				;THROW 60025, @ErrorArgs, 1;
			END
			;THROW
		END CATCH

		DECLARE @diffgram XML
		SELECT @diffgram = dbo.[recordingStorageAsDataSet](@pkid)

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

		EXEC create_pending_log @session_id, 20001, @pkid, @name, @recordingserver_id, @recordingServerName

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
