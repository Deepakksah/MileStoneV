-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_device_motion_detection]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_device_motion_detection]
GO

CREATE PROCEDURE [update_device_motion_detection]
	@device_id [GuidType],
	@last_modified DATETIME,
	@motion_detection_xml XML,
	@enabled BIT,
	@session_id [GuidType]
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
	
		DECLARE @device_name [NameType]
		DECLARE @device_type [InternalIdType]
		DECLARE @recordingserver_id [GuidType]
		DECLARE @recordingserver_name [NameType]

		DECLARE @last_modified_current DATETIME

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		DECLARE @motionPluginId [GuidType] = 'f020c881-72bf-4014-8504-2b4a5a658f1a'

		BEGIN TRAN

		-- Make sure that device exists, and get properties for logging
		SELECT	@recordingserver_id = h.[IDRecorder],
				@recordingserver_name = r.[Name],
				@device_name = d.[Name],
				@device_type = d.[DeviceType],
				@last_modified_current = rpd.[LastModified]
		FROM [RecorderPluginDevice] rpd WITH(UPDLOCK) 
		INNER JOIN [Devices] d  ON rpd.[IDDevice] = d.[IDDevice]
		INNER JOIN [Hardware] h ON h.[IDHardware] = d.[IDHardware]
		INNER JOIN [Recorders] r ON r.[IDRecorder] = h.[IDRecorder]
		WHERE d.[IDDevice] = @device_id
		AND [IDRecorderPlugin] = @motionPluginId
	
	
		SELECT @rowCount = @@ROWCOUNT
		IF(@rowCount = 0)
		BEGIN
			;THROW 60071, @procedureName, 1;
		END
	
		-- Check that last modified doesn't prevent user from updating object
		IF (@last_modified_current <> @last_modified)
		BEGIN
			;THROW 60198, @procedureName, 1;
		END
		
		-- Get DataSet before changes (for DiffGram)
		SET @dataSetBefore = dbo.recorderPluginDeviceAsDataSet(@device_id)
		
		-- Update table 'RecorderPluginDevice' with new settings (and last modified)
		UPDATE [RecorderPluginDevice]
		SET [LastModified] = @CurrentUtcTime,
			[Settings] = @motion_detection_xml,
			[Enabled] = @enabled
		WHERE [IDDevice] = @device_id
		AND [IDRecorderPlugin] = @motionPluginId
		
		-- Get DataSet after changes
		SET @dataSetAfter = dbo.recorderPluginDeviceAsDataSet(@device_id)


		-- Register diffgrams for management server
		EXEC [create_task_server_services_transaction]
						@type = 'diffgram_update',
						@data_a = @dataSetBefore,
						@data_b = @dataSetAfter,
						@current_time = @CurrentUtcTime

		-- DeviceMotionDetectionSettingsChanged: 4031; Log an entry similar to "Motion detection settings changed on '{device name}'."
		EXEC create_pending_log @session_id, 4031, @device_id, @device_name, @recordingserver_id, @recordingserver_name, @device_type

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
