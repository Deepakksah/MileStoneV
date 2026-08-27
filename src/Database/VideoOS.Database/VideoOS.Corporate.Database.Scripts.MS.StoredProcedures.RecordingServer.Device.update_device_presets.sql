-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_device_presets]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_device_presets]
GO

CREATE PROCEDURE [update_device_presets]
	@device_id [GuidType],
	@last_modified DATETIME,
	@presets_xml XML,
	@session_id [GuidType],
	@preset_id [GuidType],
	@create_preset BIT
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
		DECLARE @name [NameType]

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		BEGIN TRAN

		-- Make sure that device exists, and get properties for logging
		SELECT	@recordingserver_id = h.[IDRecorder],
				@recordingserver_name = r.[Name],
				@device_name = d.[Name],
				@device_type = d.[DeviceType],
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


		-- Check that last modified doesn't prevent user from updating object
		IF (@last_modified_current <> @last_modified)
		BEGIN
			;THROW 600198, @procedureName, 1;
		END


		-- Get DataSet before changes (for DiffGram)
		SET @dataSetBefore = dbo.devicesAsDataSet(@device_id)


		-- Update table 'Devices' with new settings (and last modified)
		UPDATE [Devices]
		SET [LastModified] = @CurrentUtcTime,
			[Presets] = @presets_xml
		WHERE [IDDevice] = @device_id


		SELECT @name = presetNodes.value('name[1]', 'nvarchar(max)')
		FROM Devices
		CROSS APPLY Presets.nodes('presets/preset') PresetNodes(presetNodes)
		WHERE [IDDevice] = @device_id		
		AND presetNodes.value('@id[1]', 'nvarchar(50)') = @preset_id

		IF (@create_preset = 1)
		BEGIN
			-- DeviceSettingsChanged: 13022; Log an entry similar to "PTZ preset position 'preset_name' created."
			EXEC create_pending_log @session_id, 13022, @device_id, @device_name, @recordingserver_id, @recordingserver_name, @name
		END
		ELSE
		BEGIN
			-- DeviceSettingsChanged: 13023; Log an entry similar to "PTZ preset position 'preset_name' changed."
			EXEC create_pending_log @session_id, 13023, @device_id, @device_name, @recordingserver_id, @recordingserver_name, @name
		END

		-- Get DataSet after changes
		SET @dataSetAfter = dbo.devicesAsDataSet(@device_id)

		-- Register diffgrams for management server
		EXEC [create_task_transaction]
							@type = 'diffgram_update',
							@target = '909749AE-88C4-4780-A381-383D691E98FA',
							@data_a = @dataSetBefore,
							@data_b = @dataSetAfter,
							@current_time = @CurrentUtcTime
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
