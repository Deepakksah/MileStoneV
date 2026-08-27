-- This header is used by the dbupdate tool]
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on] The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes]
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_device_preset_order]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_device_preset_order]
GO

CREATE PROCEDURE [update_device_preset_order]
	@device_id [GuidType]
	, @ids XML
	, @session_id [GuidType]
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
		DECLARE @DevicePresetXml XML

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()
	
		BEGIN TRAN
	
		-- Make sure that device exists, and get properties for logging
		SELECT	@recordingserver_id = h.[IDRecorder],
				@recordingserver_name = r.[Name],
				@device_name = d.[Name],
				@device_type = d.[DeviceType],
				@DevicePresetXml = [Presets],
				@last_modified_current = d.[LastModified]
		FROM [Devices] d WITH(UPDLOCK)
		INNER JOIN [Hardware] h ON h.[IDHardware] = d.[IDHardware]
		INNER JOIN [Recorders] r ON r.[IDRecorder] = h.[IDRecorder]
		WHERE d.[IDDevice] = @device_id
	
		SELECT @rowCount = @@ROWCOUNT
		IF(@rowCount = 0)
		BEGIN
			;THROW 60071, @procedureName, 1;
		END

		DECLARE @PresetId GuidType

		-- Make a cursor that runs in reverse order with the id specified
		DECLARE order_id_cursor CURSOR
		FOR SELECT ID
		FROM XmlToIdTable(@ids)
		ORDER BY ROW_NUMBER() OVER (ORDER BY (SELECT 1)) DESC

		OPEN order_id_cursor
		BEGIN TRY
			FETCH NEXT FROM order_id_cursor
			INTO @PresetId

			WHILE(@@FETCH_STATUS = 0)
			BEGIN
				-- Get the preset xml
				DECLARE @PresetXml XML
				SELECT @PresetXml = @DevicePresetXml.query('(/presets/preset[@id=lower-case(sql:variable("@PresetId"))])[1]')

				IF(@PresetXml IS NOT NULL)
				BEGIN
					-- Preset exists with specifed Id.
					-- Remove the preset from the XML
					SET @DevicePresetXml.modify('delete /presets/preset[@id=lower-case(sql:variable("@PresetId"))][1]')

					-- Insert preset on first position
					SET @DevicePresetXml.modify('insert sql:variable("@PresetXml") as first into (/presets)[1]')
				END
	
				FETCH NEXT FROM order_id_cursor
				INTO @PresetId
			END
		END TRY
		BEGIN CATCH
			CLOSE order_id_cursor
			DEALLOCATE order_id_cursor
			;THROW
		END CATCH

		CLOSE order_id_cursor
		DEALLOCATE order_id_cursor
	
		-- Update table 'Devices' with new preset xml (and last modified)
		UPDATE [Devices]
		SET [Presets] = @DevicePresetXml
		, [LastModified] = @CurrentUtcTime
		WHERE [IDDevice] = @device_id
	
		COMMIT TRAN

		-- AuditDeviceSettingsChanged: 4022; Log an entry similar to "Settings changed on '{device name}'."
		EXEC create_pending_log @session_id, 4022, @device_id, @device_name, @recordingserver_id, @recordingserver_name, @device_type
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