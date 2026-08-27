-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_device_privacy_mask]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_device_privacy_mask]
GO

CREATE PROCEDURE [update_device_privacy_mask]
	@device_id [GuidType],
	@privacy_mask_xml XML,
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
		DECLARE @dataSetAfter XML
	
		DECLARE @device_name [NameType]
		DECLARE @device_type [InternalIdType]
		DECLARE @recordingserver_id [GuidType]
		DECLARE @recordingserver_name [NameType]
		DECLARE @modifiedTime DATETIME

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		BEGIN TRAN

		-- Make sure that device exists, and get properties for logging
		SELECT	@recordingserver_id = h.[IDRecorder],
				@recordingserver_name = r.[Name],
				@device_name = d.[Name],
				@device_type = d.[DeviceType],
				@modifiedTime = dpm.[LastModified]
		FROM [DevicePrivacyMask] dpm WITH(UPDLOCK) 
			INNER JOIN [Devices] d  ON dpm.[IDDevice] = d.[IDDevice]
			INNER JOIN [Hardware] h ON h.[IDHardware] = d.[IDHardware]
			INNER JOIN [Recorders] r ON r.[IDRecorder] = h.[IDRecorder]
		WHERE dpm.[IDDevice] = @device_id AND dpm.[LastModified] = (SELECT MAX([LastModified]) FROM [DevicePrivacyMask] WHERE [IDDevice] = @device_id)
	
		SELECT @rowCount = @@ROWCOUNT
		IF(@rowCount = 0)
		BEGIN
			;THROW 60071, @procedureName, 1;
		END

		IF (@privacy_mask_xml IS NULL AND @enabled IS NULL)
		BEGIN
			ROLLBACK TRAN
			RETURN 0
		END

		-- Update table 'DevicePrivacyMask' with new row to preserve history
		INSERT INTO	[DevicePrivacyMask] ([LastModified], [IDDevice], [PrivacyMask], [Enabled])
		VALUES (@CurrentUtcTime,
				@device_id,
				COALESCE(@privacy_mask_xml, (SELECT [PrivacyMask] FROM [DevicePrivacyMask] WHERE [IDDevice] = @device_id AND [LastModified] = @modifiedTime)),
				COALESCE(@enabled, (SELECT [Enabled] FROM [DevicePrivacyMask] WHERE [IDDevice] = @device_id AND [LastModified] = @modifiedTime)))

		-- Get DataSet after changes
		SET @dataSetAfter = dbo.devicePrivacyMaskAsDataSet(@device_id, @CurrentUtcTime)

		-- Register diffgrams for management server
		EXEC [create_task_transaction]
							@type = 'diffgram_insert',
							@target = '909749AE-88C4-4780-A381-383D691E98FA',
							@data_a = @dataSetAfter,
							@current_time = @CurrentUtcTime

		-- AuditDevicePrivacyMaskSettingsChanged: 4040; Log an entry similar to "Privacy mask settings changed on '{device name}'."
		EXEC create_pending_log @session_id, 4040, @device_id, @device_name, @recordingserver_id, @recordingserver_name, @device_type

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
