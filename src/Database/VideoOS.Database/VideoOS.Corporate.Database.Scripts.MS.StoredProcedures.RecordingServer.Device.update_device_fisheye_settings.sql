-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_device_fisheye_settings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_device_fisheye_settings]
GO

CREATE PROCEDURE [update_device_fisheye_settings]
	@IDDevice [GuidType],
	@order int,
	@settings NVARCHAR(MAX),
	@enabled bit,
	@session_id [GuidType]
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

		DECLARE @ImmerVisionPlugInGuid [GuidType] = '4807A4B7-6F6B-456E-A062-141586ED733C'
		DECLARE @ExistingEnabled BIT = 0
		DECLARE @device_name [NameType]
		DECLARE @device_type VARCHAR(100)
		DECLARE @recordingserver_id [GuidType]
		DECLARE @recordingserver_name [NameType]

		SELECT @recordingserver_id = h.[IDRecorder],
			   @recordingserver_name = r.[Name],
			   @device_name = d.[Name],
			   @device_type = d.[DeviceType]
		FROM [Devices] d 
		INNER JOIN [RegisteredDeviceTypes] rd ON d.[DeviceType] = rd.[DeviceType]
		INNER JOIN [Hardware] h ON d.IDHardware = h.IDHardware
		INNER JOIN [Recorders] r ON r.IDRecorder = h.[IDRecorder] 
		WHERE d.[IDDevice] = @IDDevice

		IF EXISTS(SELECT * FROM [RecorderPluginDevice] WHERE [IDRecorderPlugin] = @ImmerVisionPlugInGuid AND [IDDevice] = @IDDevice)
		BEGIN
			SELECT @ExistingEnabled = [Enabled] 
			FROM [RecorderPluginDevice] 
			WHERE [IDRecorderPlugin] = @ImmerVisionPlugInGuid AND [IDDevice] = @IDDevice

			UPDATE [RecorderPluginDevice]
			SET [Settings] = @settings,
				[Enabled] = @enabled,
				[LastModified] = @CurrentUtcTime
			WHERE [IDRecorderPlugin] = @ImmerVisionPlugInGuid AND [IDDevice] = @IDDevice
		END
		ELSE
		BEGIN
			INSERT  [RecorderPluginDevice] 
			(
				[IDRecorderPlugin],
				[IDDevice],
				[ProcessOrder],
				[Settings],
				[Enabled],
				[LastModified]) 
			VALUES
				(
				@ImmerVisionPlugInGuid,
				@IDDevice,
				@order,
				@settings,
				@enabled,
				@CurrentUtcTime
				);			
		END

		IF(@ExistingEnabled <> @enabled)
		BEGIN
			IF(@enabled = 1)
			BEGIN
				-- Log an entry similar to 'Fisheye lens settings enabled on '{0}'.'
				EXEC create_pending_log @session_id, 20120, @IDDevice, @device_name, @recordingserver_id, @recordingserver_name, @device_type
			END
			ELSE
			BEGIN
				-- Log an entry similar to 'Fisheye lens settings disabled on '{0}'.'
				EXEC create_pending_log @session_id, 20121, @IDDevice, @device_name, @recordingserver_id, @recordingserver_name, @device_type
			END
        END
					
		IF(@enabled = 1)
		BEGIN
			-- Log an entry similar to 'Fisheye lens settings changed on '{0}'.'
			EXEC create_pending_log @session_id, 20122, @IDDevice, @device_name, @recordingserver_id, @recordingserver_name, @device_type
		END
		

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
