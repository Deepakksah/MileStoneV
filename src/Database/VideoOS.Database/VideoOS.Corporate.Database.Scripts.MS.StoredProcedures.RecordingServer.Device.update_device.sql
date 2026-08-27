-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_device]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_device]
GO

CREATE PROCEDURE [update_device]
	@IDDevice [GuidType],
	@name [NameType],
	@description [DescriptionType],
	@enabled BIT,
	@recording_enabled BIT,
	@recording_framerate FLOAT,
	@prebuffer_enabled BIT,
	@prebuffer_in_memory BIT,
	@prebuffer_seconds INT,
	@edgestorage_enabled BIT,
	@edgestorage_playback_enabled BIT,
	@record_keyframes BIT,
	@manual_timeout_enabled BIT,
	@manual_timeout INT,
	@record_on_related_devices BIT,
	@gis_point_string NVARCHAR(MAX),
	@short_name NVARCHAR(128),
	@icon INT,
	@coverage_direction FLOAT,
	@coverage_depth FLOAT,
	@coverage_field_of_view FLOAT,
	@shortcut NVARCHAR(128),
	@custom_properties XML,
	@session_id [GuidType],
	@multicastEnabled BIT,
	@ptzManualSessionTimeout INT,
	@ptzReservedSessionTimeout INT,
	@ptzExtendedManualSessionTimeout INT,
	@failoverLiveOnly BIT,
	@failoverDisabled BIT
AS
BEGIN
	SET NOCOUNT ON

		-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY

		DECLARE @rowCount INT
		DECLARE @devicesDataSetBefore XML
		DECLARE @devicesDataSetAfter XML
		DECLARE @recordingstorageDeviceDataSetBefore XML
		DECLARE @recordingstorageDeviceDataSetAfter XML
		DECLARE @device_name [NameType]
		DECLARE @device_type VARCHAR(100)
		DECLARE @recordingserver_id [GuidType]
		DECLARE @recordingserver_name [NameType]
		DECLARE @recordingStorageDeviceChanged BIT
		DECLARE @is_recording_device BIT


		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()
	
		BEGIN TRAN

		-- Make sure that hardware/recording server exists
		SELECT @recordingserver_id = h.[IDRecorder],
			   @recordingserver_name = r.[Name],
			   @device_name = d.[Name],
			   @device_type = d.[DeviceType],
			   @is_recording_device = rd.[IsRecordingDevice]
		FROM [Devices] d 
		INNER JOIN [RegisteredDeviceTypes] rd ON d.[DeviceType] = rd.[DeviceType]
		INNER JOIN [Hardware] h ON d.IDHardware = h.IDHardware
		INNER JOIN [Recorders] r ON r.IDRecorder = h.[IDRecorder] 
		WHERE d.[IDDevice] = @IDDevice
	
		SELECT @rowCount = @@ROWCOUNT
		IF(@rowCount = 0)
		BEGIN
			;THROW 60064, @procedureName, 1;
		END


		IF (@name IS NULL AND @description IS NULL AND @enabled IS NULL AND 
			@recording_enabled IS NULL AND @recording_framerate IS NULL AND 
			@prebuffer_enabled IS NULL AND @prebuffer_in_memory IS NULL AND @prebuffer_seconds IS NULL AND 
			@edgestorage_enabled IS NULL AND @edgestorage_playback_enabled IS NULL AND 
			@record_keyframes IS NULL AND @custom_properties IS NULL AND
			@manual_timeout_enabled IS NULL AND @manual_timeout IS NULL AND
			@record_on_related_devices IS NULL AND @gis_point_string IS NULL AND
			@short_name IS NULL AND @icon IS NULL AND @coverage_direction IS NULL AND
			@coverage_depth IS NULL AND @coverage_field_of_view IS NULL AND @shortcut IS NULL AND @multicastEnabled IS NULL AND
			@ptzManualSessionTimeout IS NULL AND @ptzReservedSessionTimeout IS NULL AND @ptzExtendedManualSessionTimeout IS NULL AND
			@failoverLiveOnly IS NULL AND @failoverDisabled IS NULL)
		BEGIN
			ROLLBACK TRAN
			RETURN 0
		END


		-- TODO : Check that last modified doesn't prevent user from updating object


		-- Verify that that prebuffer settings are valid

		DECLARE @prebuffer_in_memory_final BIT
		DECLARE @prebuffer_seconds_final INT
		DECLARE @edgestorage_playback_before BIT

		SELECT @prebuffer_in_memory_final = COALESCE(@prebuffer_in_memory, [prebuffer_in_memory]), 
			   @prebuffer_seconds_final = COALESCE(@prebuffer_seconds, [prebuffer_seconds]),
			   @edgestorage_playback_before = [edgestorage_playback]
		FROM RecordingStorageDevice WITH(UPDLOCK)
		WHERE device_id = @IDDevice

		DECLARE @TimeLimitForPrebufferInRam AS INT
		SELECT @TimeLimitForPrebufferInRam = CAST([Setting] AS INT)
		FROM [SystemSettings]
		WHERE [Name] = 'TimeLimitForPrebufferInRam'

		IF (@prebuffer_in_memory_final = 1 AND @prebuffer_seconds_final > @TimeLimitForPrebufferInRam)
		BEGIN
			;THROW 60302, @procedureName, 1;
		END

		-- If device is a recording device, then also update table 'RecordingStorageDevice'
		IF (@is_recording_device = 1)
		BEGIN
			SET @recordingStorageDeviceChanged = 0

			-- Get DataSet before changes (for DiffGram)
			SET @recordingstorageDeviceDataSetBefore = dbo.recordingStorageDeviceAsDataSet(@IDDevice)

		
			-- Update device recording properties
			IF (@recording_framerate IS NOT NULL OR @prebuffer_seconds IS NOT NULL OR @prebuffer_in_memory IS NOT NULL
			OR @record_keyframes IS NOT NULL OR @manual_timeout IS NOT NULL
			OR @manual_timeout_enabled IS NOT NULL
			OR @record_on_related_devices IS NOT NULL)
			BEGIN
				UPDATE [RecordingStorageDevice]
				SET
					[recording_framerate] = COALESCE(@recording_framerate, [recording_framerate]),
					[prebuffer_seconds] = COALESCE(@prebuffer_seconds, [prebuffer_seconds]),
					[prebuffer_in_memory] = COALESCE(@prebuffer_in_memory, [prebuffer_in_memory]),
					[record_keyframes] = COALESCE(@record_keyframes, [record_keyframes]),
					[manual_timeout_enabled] = COALESCE(@manual_timeout_enabled, [manual_timeout_enabled]),
					[manual_timeout] = COALESCE(@manual_timeout, [manual_timeout]),
					[record_on_related_devices] = COALESCE(@record_on_related_devices, [record_on_related_devices])
				WHERE
					[device_id] = @IDDevice
					AND ([recording_framerate] != @recording_framerate 
					OR [prebuffer_seconds] != @prebuffer_seconds OR [prebuffer_in_memory] != @prebuffer_in_memory 
					OR [record_keyframes] != @record_keyframes OR [manual_timeout_enabled] != @manual_timeout_enabled 
					OR [manual_timeout] != @manual_timeout
					OR [record_on_related_devices] != @record_on_related_devices)

				SELECT @rowcount = @@ROWCOUNT
				
				IF (@rowcount > 0)
				BEGIN
					SET @recordingStorageDeviceChanged = 1

					--	Log an entry similar to "Recording settings changed on '{device name}'"
					EXEC create_pending_log @session_id, 4028, @IDDevice, @device_name, @recordingserver_id, @recordingserver_name, @device_type
				END
			END

			-- Enable/disable recording
			IF (@recording_enabled IS NOT NULL)
			BEGIN
				UPDATE [RecordingStorageDevice]
				SET
					[recording_enabled] = @recording_enabled
				WHERE 
					[device_id] = @IDDevice
					AND [recording_enabled] != @recording_enabled

				SELECT @rowcount = @@ROWCOUNT
				
				IF (@rowcount > 0)
				BEGIN
					SET @recordingStorageDeviceChanged = 1

					IF (@recording_enabled = 1)
					BEGIN
						--	Log an entry similar to "Recording enabled on '{device name}'."
						EXEC create_pending_log @session_id, 20101, @IDDevice, @device_name, @recordingserver_id, @recordingserver_name, @device_type
					END
					ELSE
					BEGIN
						--	Log an entry similar to "Recording disabled on '{device name}'."
						EXEC create_pending_log @session_id, 20102, @IDDevice, @device_name, @recordingserver_id, @recordingserver_name, @device_type
					END
				END
			END

			-- Enable/disable prebuffer
			IF (@prebuffer_enabled IS NOT NULL)
			BEGIN
				UPDATE [RecordingStorageDevice]
				SET
					[prebuffer_enabled] = @prebuffer_enabled
				WHERE
					[device_id] = @IDDevice
					AND [prebuffer_enabled] != @prebuffer_enabled

				SELECT @rowcount = @@ROWCOUNT
				
				IF (@rowcount > 0)
				BEGIN
					SET @recordingStorageDeviceChanged = 1

					IF (@prebuffer_enabled = 1)
					BEGIN
						--	Log an entry similar to "Pre-buffer enabled on '{device name}'."
						EXEC create_pending_log @session_id, 20103, @IDDevice, @device_name, @recordingserver_id, @recordingserver_name, @device_type
					END
					ELSE
					BEGIN
						--	Log an entry similar to "Pre-buffer disabled on '{device name}'."
						EXEC create_pending_log @session_id, 20104, @IDDevice, @device_name, @recordingserver_id, @recordingserver_name, @device_type
					END
				END
			END

			-- Update device recording properties
			IF (@edgestorage_enabled IS NOT NULL)
			BEGIN
				UPDATE [RecordingStorageDevice]
				SET
					[edgestorage_enabled] = @edgestorage_enabled
				WHERE
					[device_id] = @IDDevice
					AND [edgestorage_enabled] != @edgestorage_enabled

				SELECT @rowcount = @@ROWCOUNT
				
				IF (@rowcount > 0)
				BEGIN
					SET @recordingStorageDeviceChanged = 1

					IF (@edgestorage_enabled = 1)
					BEGIN
						--	Log an entry similar to "Remote recording enabled on '{device name}'."
						EXEC create_pending_log @session_id, 20105, @IDDevice, @device_name, @recordingserver_id, @recordingserver_name, @device_type
					END
					ELSE
					BEGIN
						--	Log an entry similar to "Remote recording disabled on '{device name}'."
						EXEC create_pending_log @session_id, 20106, @IDDevice, @device_name, @recordingserver_id, @recordingserver_name, @device_type
					END
				END
			END
			

			-- Update device recording properties
			IF (@edgestorage_playback_enabled IS NOT NULL)
			BEGIN
				IF(@edgestorage_playback_before != @edgestorage_playback_enabled)
				BEGIN
					DECLARE @MarkedDataType_EvidenceLock INT = 2
	
					-- Check for evidence locks before shift to edgestorage playback
					IF(@edgestorage_playback_enabled = 1 AND EXISTS(
						SELECT	mdd.[IDDevice]
						FROM	[MarkedDataDevices] mdd
								INNER JOIN [MarkedData] md ON md.[IDMarkedData] = mdd.[IDMarkedData]
						WHERE	mdd.[IDDevice] = @IDDevice AND
								md.[MarkDataType] = @MarkedDataType_EvidenceLock))
					BEGIN
						SET @ErrorArgs = @procedureName + N': %%Name=' + @device_name + N'%%'
						;THROW 60335, @ErrorArgs, 1;
					END

					UPDATE [RecordingStorageDevice]
					SET
						[edgestorage_playback] = @edgestorage_playback_enabled
					WHERE
						[device_id] = @IDDevice


					SET @recordingStorageDeviceChanged = 1

					IF (@edgestorage_playback_enabled = 1)
					BEGIN
						--	Log an entry similar to "Playback of remote recording enabled on '{device name}'."
						EXEC create_pending_log @session_id, 20107, @IDDevice, @device_name, @recordingserver_id, @recordingserver_name, @device_type
					END
					ELSE
					BEGIN
						--	Log an entry similar to "Playback of remote recording disabled on '{device name}'."
						EXEC create_pending_log @session_id, 20108, @IDDevice, @device_name, @recordingserver_id, @recordingserver_name, @device_type
					END
				END
			END

			IF (@recordingStorageDeviceChanged = 1)
			BEGIN
				UPDATE [RecordingStorageDevice]
				SET [last_modified] = @CurrentUtcTime
				WHERE  [device_id] = @IDDevice
			END

			-- Get DataSet after changes
			SET @recordingstorageDeviceDataSetAfter = dbo.recordingStorageDeviceAsDataSet(@IDDevice)
		END
	
	
		-- Update table 'Devices'

		-- Get DataSet before changes (for DiffGram)
		SET @devicesDataSetBefore = dbo.devicesAsDataSet(@IDDevice)

		-- Update device name
		IF (@name IS NOT NULL)
		BEGIN
			UPDATE [Devices]
			SET
				[LastModified] = @CurrentUtcTime,
				[Name] = @name
			WHERE 
				[IDDevice] = @IDDevice
				AND dbo.StringValueChanging([Name], @name) = 1

			SELECT @rowcount = @@ROWCOUNT

			IF (@rowcount > 0)
			BEGIN
				--	Log an entry similar to "Device '{old device name}' renamed to '{new device name}'."
				EXEC create_pending_log @session_id, 4027, @IDDevice, @name, @recordingserver_id, @recordingserver_name, @device_type, @device_name
			END
		END


		-- Enable/disable device
		IF (@enabled IS NOT NULL)
		BEGIN
			UPDATE [Devices]
			SET
				[LastModified] = @CurrentUtcTime,
				[Enabled] = @enabled
			WHERE 
				[IDDevice] = @IDDevice
				AND [Enabled] != @enabled

			SELECT @rowcount = @@ROWCOUNT

			IF (@rowcount > 0)
			BEGIN
				IF (@enabled = 1)
				BEGIN
					--	Log an entry similar to "Device '{device name}' enabled."
					EXEC create_pending_log @session_id, 4025, @IDDevice, @device_name, @recordingserver_id, @recordingserver_name, @device_type
				END
				ELSE
				BEGIN
					--	Log an entry similar to "Device '{device name}' disabled."
					EXEC create_pending_log @session_id, 4026, @IDDevice, @device_name, @recordingserver_id, @recordingserver_name, @device_type
				END
			END
		END

		-- GisPoint
		IF (@gis_point_string IS NOT NULL)
		BEGIN
			DECLARE @gis_point geography
			SET @gis_point = geography::STPointFromText(@gis_point_string, 4326)
	
			UPDATE [Devices]
			SET
				[LastModified] = @CurrentUtcTime,
				[GisPoint] = @gis_point 
			WHERE 
				[IDDevice] = @IDDevice AND [GisPoint].AsTextZM() != @gis_point.AsTextZM()

			SELECT @rowcount = @@ROWCOUNT

			DECLARE @gis_point_audit_string NVARCHAR(MAX) = ''
			IF @gis_point.STIsEmpty() = '0'
			BEGIN
				SET @gis_point_audit_string = LTRIM(STR(@gis_point.Lat, 255, 100)) + ', ' + LTRIM(STR(@gis_point.Long, 255, 100))
			END
		
			IF (@rowcount > 0)
			BEGIN
				--Log an entry similar to "Device GisPoint changed to '{new gis point}'."
				EXEC create_pending_log @session_id, 4044, @IDDevice, @device_name, @recordingserver_id, @recordingserver_name, @device_type, @gis_point_audit_string
			END
		END

		-- ShortName
		IF (@short_name IS NOT NULL)
		BEGIN
			UPDATE [Devices]
			SET
				[LastModified] = @CurrentUtcTime,
				[ShortName] = @short_name 
			WHERE 
				[IDDevice] = @IDDevice
				AND dbo.StringValueChanging([ShortName], @short_name) = 1

			SELECT @rowcount = @@ROWCOUNT

			IF (@rowcount > 0)
			BEGIN
				--Log an entry similar to "Device short name changed to '{new short name}'."
				EXEC create_pending_log @session_id, 4045, @IDDevice, @device_name, @recordingserver_id, @recordingserver_name, @device_type, @short_name
			END
		END

		-- Shortcut
		IF (@shortcut IS NOT NULL)
		BEGIN
			UPDATE [Devices]
			SET
				[LastModified] = @CurrentUtcTime,
				[Shortcut] = @shortcut
			WHERE 
				[IDDevice] = @IDDevice
				AND dbo.StringValueChanging([Shortcut], @shortcut) = 1

			SELECT @rowcount = @@ROWCOUNT

			IF (@rowcount > 0)
			BEGIN
				--Log an entry similar to "Device shortcut changed to '{new shortcut}'."
				EXEC create_pending_log @session_id, 4050, @IDDevice, @device_name, @recordingserver_id, @recordingserver_name, @device_type, @shortcut
			END
		END

		-- MulticastEnabled
		IF (@multicastEnabled IS NOT NULL)
		BEGIN
			UPDATE [Devices]
			SET
				[LastModified] = @CurrentUtcTime,
				[MulticastEnabled] = @multicastEnabled
			WHERE 
				[IDDevice] = @IDDevice
				AND [MulticastEnabled] !=  @multicastEnabled

			SELECT @rowcount = @@ROWCOUNT

			IF (@rowcount > 0)
			BEGIN
				--Log an entry similar to "Device multicast changed on {deviceId} to {multicastEnabled}."
				EXEC create_pending_log @session_id, 2101, @IDDevice, @device_name, @recordingserver_id, @recordingserver_name, @device_type, @multicastEnabled
			END
		END

		-- PtzManualSessionTimeout
		IF (@ptzManualSessionTimeout IS NOT NULL)
		BEGIN
			UPDATE [Devices]
			SET
				[LastModified] = @CurrentUtcTime,
				[PtzManualSessionTimeout] = @ptzManualSessionTimeout
			WHERE 
				[IDDevice] = @IDDevice
				AND [PtzManualSessionTimeout] !=  @ptzManualSessionTimeout

			SELECT @rowcount = @@ROWCOUNT

			IF (@rowcount > 0)
			BEGIN
				--Log an entry similar to "Device ptzManuelSessionTimeout changed on {0} to {1}."
				EXEC create_pending_log @session_id, 2102, @IDDevice, @device_name, @recordingserver_id, @recordingserver_name, @device_type, @ptzManualSessionTimeout
			END
		END

		-- ptzReservedSessionTimeout
		IF (@ptzReservedSessionTimeout IS NOT NULL)
		BEGIN
			UPDATE [Devices]
			SET
				[LastModified] = @CurrentUtcTime,
				[PtzReservedSessionTimeout] = @ptzReservedSessionTimeout
			WHERE 
				[IDDevice] = @IDDevice
				AND [PtzReservedSessionTimeout] !=  @ptzReservedSessionTimeout

			SELECT @rowcount = @@ROWCOUNT

			IF (@rowcount > 0)
			BEGIN
				--Log an entry similar to "Device ptzReservedSessionTimeout changed on {0} to {1}"
				EXEC create_pending_log @session_id, 2103, @IDDevice, @device_name, @recordingserver_id, @recordingserver_name, @device_type, @ptzReservedSessionTimeout
			END
		END

		-- ptzExtendedManualSessionTimeout
		IF (@ptzExtendedManualSessionTimeout IS NOT NULL)
		BEGIN
			UPDATE [Devices]
			SET
				[LastModified] = @CurrentUtcTime,
				[PtzExtendedManualSessionTimeout] = @ptzExtendedManualSessionTimeout
			WHERE 
				[IDDevice] = @IDDevice
				AND [PtzExtendedManualSessionTimeout] !=  @ptzExtendedManualSessionTimeout

			SELECT @rowcount = @@ROWCOUNT

			IF (@rowcount > 0)
			BEGIN
				--Log an entry similar to "Device ptzExtendedManualSessionTimeout changed on {0} to {1}."
				EXEC create_pending_log @session_id, 2104, @IDDevice, @device_name, @recordingserver_id, @recordingserver_name, @device_type, @ptzExtendedManualSessionTimeout
			END
		END

		-- Icon
		IF (@icon IS NOT NULL)
		BEGIN
			UPDATE [Devices]
			SET
				[LastModified] = @CurrentUtcTime,
				[Icon] = @icon 
			WHERE 
				[IDDevice] = @IDDevice
				AND [Icon] != @icon

			SELECT @rowcount = @@ROWCOUNT

			IF (@rowcount > 0)
			BEGIN
				--Log an entry similar to "Device icon changed to '{new icon}'."
				EXEC create_pending_log @session_id, 4046, @IDDevice, @device_name, @recordingserver_id, @recordingserver_name, @device_type, @icon
			END
		END

		-- CoverageDirection
		IF (@coverage_direction IS NOT NULL)
		BEGIN
			UPDATE [Devices]
			SET
				[LastModified] = @CurrentUtcTime,
				[CoverageDirection] = @coverage_direction 
			WHERE 
				[IDDevice] = @IDDevice
				AND [CoverageDirection] != @coverage_direction

			SELECT @rowcount = @@ROWCOUNT

			DECLARE @coverage_direction_degree float
			SET @coverage_direction_degree =
				CASE
					WHEN @coverage_direction < 0 THEN ((@coverage_direction + 1) * 360)
					ELSE (@coverage_direction * 360)
				END
		
			IF (@rowcount > 0)
			BEGIN
				--Log an entry similar to "Device coverage direction changed to '{new coverage direction}'."
				EXEC create_pending_log @session_id, 4047, @IDDevice, @device_name, @recordingserver_id, @recordingserver_name, @device_type, @coverage_direction_degree
			END
		END

		-- CoverageDepth
		IF (@coverage_depth IS NOT NULL)
		BEGIN
			UPDATE [Devices]
			SET
				[LastModified] = @CurrentUtcTime,
				[CoverageDepth] = @coverage_depth 
			WHERE 
				[IDDevice] = @IDDevice
				AND [CoverageDepth] != @coverage_depth

			SELECT @rowcount = @@ROWCOUNT

			IF (@rowcount > 0)
			BEGIN
				--Log an entry similar to "Device coverage depth changed to '{new coverage depth}'."
				EXEC create_pending_log @session_id, 4048, @IDDevice, @device_name, @recordingserver_id, @recordingserver_name, @device_type, @coverage_depth
			END
		END

		-- CoverageFieldOfView
		IF (@coverage_field_of_view IS NOT NULL)
		BEGIN
			UPDATE [Devices]
			SET
				[LastModified] = @CurrentUtcTime,
				[CoverageFieldOfView] = @coverage_field_of_view 
			WHERE 
				[IDDevice] = @IDDevice
				AND [CoverageFieldOfView] != @coverage_field_of_view

			SELECT @rowcount = @@ROWCOUNT

			DECLARE @coverage_field_of_view_degree FLOAT
			SET @coverage_field_of_view_degree = @coverage_field_of_view * 360
		
			IF (@rowcount > 0)
			BEGIN
				--Log an entry similar to "Device coverage field of view changed to '{new coverage field of view}'."
				EXEC create_pending_log @session_id, 4049, @IDDevice, @device_name, @recordingserver_id, @recordingserver_name, @device_type, @coverage_field_of_view_degree
			END
		END

		-- Update remaining device properties
		IF (@description IS NOT NULL)
		BEGIN
			UPDATE [Devices]
			SET
				[LastModified] = @CurrentUtcTime,
				[Description] = @description
			WHERE 
				[IDDevice] = @IDDevice
				AND dbo.StringValueChanging([Description], @description) = 1
		END

		-- Update failover settings (Comes in pair)
		IF (@failoverLiveOnly IS NOT NULL AND @failoverDisabled IS NOT NULL)
		BEGIN
			UPDATE [Devices]
			SET
				[LastModified] = @CurrentUtcTime,
				[FailoverLiveOnly] = @failoverLiveOnly,
				[DisableOnFailover] = @failoverDisabled
			WHERE
				[IDDevice] = @IDDevice AND ([FailoverLiveOnly] != @failoverLiveOnly OR [DisableOnFailover] != @failoverDisabled)

			SELECT @rowcount = @@ROWCOUNT
			IF (@rowcount > 0)
			BEGIN
				DECLARE @failoverSetting VARCHAR(20)
				IF (@failoverLiveOnly = 1)
				BEGIN
					SET @failoverSetting = 'LiveOnly'
				END 
				ELSE 
				IF (@failoverDisabled = 1)
				BEGIN
					SET @failoverSetting = 'Disabled'
				END
				ELSE
				BEGIN
					SET @failoverSetting = 'FullSupport'
				END
				--Log an entry similar to "Failover settings of device '{0}' changed to '{1}'."
				EXEC create_pending_log @session_id, 4051, @IDDevice, @device_name, @recordingserver_id, @recordingserver_name, @device_type, @failoverSetting
			END

		END

		-- Get DataSet after changes
		SET @devicesDataSetAfter = dbo.devicesAsDataSet(@IDDevice)

		-- Update custom properties
		IF (@custom_properties IS NOT NULL)
		BEGIN
			EXEC [update_custom_properties_transaction]
				@object_id = @IDDevice,
				@custom_properties = @custom_properties
		END

		-- Call the create task procedure to create diffgram update task
		EXEC [create_task_transaction] 'diffgram_update', '909749AE-88C4-4780-A381-383D691E98FA', @devicesDataSetBefore, @devicesDataSetAfter, @CurrentUtcTime
		
		IF (@recordingStorageDeviceChanged = 1)
		BEGIN
			-- Call the create task procedure to create diffgram update task
			EXEC [create_task_transaction] 'diffgram_update', '909749AE-88C4-4780-A381-383D691E98FA', @recordingstorageDeviceDataSetBefore, @recordingstorageDeviceDataSetAfter, @CurrentUtcTime
		END

		COMMIT TRAN
		RETURN 0
	END TRY
	BEGIN CATCH 
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION
		END;
		IF(ERROR_NUMBER() < 60000 AND ERROR_NUMBER() <> 1205 AND ERROR_NUMBER() <> 8628 AND ERROR_NUMBER() <> 8645 AND ERROR_NUMBER() <> 2801)
		BEGIN
			SET @ErrorArgs = dbo.[FormatErrorMessage](ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE())
			;THROW 60000, @ErrorArgs, 1;
		END

		;THROW

    END CATCH;
END
GO
