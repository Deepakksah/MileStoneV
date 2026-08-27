-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[recording_server_create_or_update]') AND type = 'P')
DROP PROCEDURE [recording_server_create_or_update]
GO

CREATE PROCEDURE [recording_server_create_or_update]
	@RecorderId GuidType,
	@ServiceId GuidType,
	@Type int,
	@Name NameType,
	@HostName NameType,
	@CertificateConfigured BIT,
	@IsMulticastLicensed BIT,
	@WebServerHostName NameType,
	@WebServerPort INT,
	@ScanRanges nText,
	@TimeZoneName NameType,
	@DevicePackInfo nText,
	@FailoverUdpPort int,
	@authorized bit,
	@DatabasePort int,
	@FailoverId GuidType,
	@Version NameType = "0.0.0.0",
	@FailOnUpdate bit = 0
 AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @dataSetBefore XML
		DECLARE @dataSetAfter XML
		DECLARE @recorderAsDataSetAfter XML

		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()
	
		DECLARE @IsCreated BIT
		SET @IsCreated = 0
		DECLARE @diffgram_created BIT
		SET @diffgram_created = 0

		-- Prepare values for task creation
		SET @dataSetBefore = NULL
		IF (@Type = 0)
		BEGIN
			IF EXISTS (SELECT 1 FROM [RecorderInfo] WHERE [recorder_id] = @RecorderId)
				SET @dataSetBefore = dbo.recorderInfoAsDataSet(@RecorderId)
		END
		ELSE
		BEGIN
			IF EXISTS (SELECT 1 FROM [FailoverInfo] WHERE [recording_server_service_id] = @ServiceId)
				SET @dataSetBefore = dbo.failoverInfoAsDataSet(@ServiceId)
		END

		-- Make sure the recorder exists
		IF NOT EXISTS (
			SELECT [IDRecorder]
			FROM [Recorders] WITH(UPDLOCK)
			WHERE [IDRecorder] = @RecorderId)
		BEGIN
			DECLARE @namespace NVARCHAR(260) = 'RecordingServer'

			BEGIN TRY
				-- Register the object in the security system
				EXEC security_register_object_transaction
					@namespace_name = @namespace,
					@object_id = @RecorderId,
					@parent_path = '/',
					@CurrentUtcTime = @CurrentUtcTime
			END TRY
			BEGIN CATCH
				SET @ErrorArgs = dbo.[FormatErrorMessage](ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE()) + N'%%; %%Namespace=' + @namespace + N'%%; %%ID=' + CAST(@RecorderId AS NVARCHAR(36)) + N'%%'
				;THROW 65000, @ErrorArgs, 1;
			END CATCH
		
			-- Calculate the multicast ip-range
			DECLARE @NextIPRange VARCHAR(15)
			SELECT @NextIPRange = dbo.ConvertIntegerToIP(MAX(dbo.ConvertIPToInteger(r.[MulticastSettings].value('(/properties/settings/setting[name="ClientIPRangeEnd"]/value)[1]', 'VARCHAR(15)'))) + 1)
			FROM 
			(
				SELECT CAST([MulticastSettings] AS XML) AS MulticastSettings FROM recorders
			) AS r

			IF(@NextIPRange IS NULL)
			BEGIN
				SET @NextIPRange = '232.0.1.0'
			END

			DECLARE @multicastSettings NVARCHAR(MAX)

			-- If the calculated IP-range is outside scope i.e larger than 232.255.255.255 then Multicast must be disabled.
			IF(dbo.ConvertIPToInteger(@NextIPRange) > dbo.ConvertIPToInteger('232.255.255.255') OR @IsMulticastLicensed = 0) 
			BEGIN
			  -- Multicast disabled
				SET @multicastSettings = '<properties><settings><setting><name>MulticastEnabled</name><value>False</value></setting><setting><name>ClientIPRangeStart</name><value>232.0.1.0</value></setting><setting><name>ClientIPRangeEnd</name><value>232.0.1.0</value></setting><setting><name>ClientPortRangeStart</name><value>6000</value></setting><setting><name>ClientPortRangeEnd</name><value>7000</value></setting><setting><name>ServerAddress</name><value>0.0.0.0</value></setting><setting><name>ServerPort</name><value>0</value></setting><setting><name>MTU</name><value>1500</value></setting><setting><name>TTL</name><value>32</value></setting></settings></properties>'
			END
			ELSE
			BEGIN
			  -- Multicates enabled
				SET @multicastSettings = '<properties><settings><setting><name>MulticastEnabled</name><value>True</value></setting><setting><name>ClientIPRangeStart</name><value>XXX.XXX.XXX.XXX</value></setting><setting><name>ClientIPRangeEnd</name><value>XXX.XXX.XXX.XXX</value></setting><setting><name>ClientPortRangeStart</name><value>6000</value></setting><setting><name>ClientPortRangeEnd</name><value>7000</value></setting><setting><name>ServerAddress</name><value>0.0.0.0</value></setting><setting><name>ServerPort</name><value>0</value></setting><setting><name>MTU</name><value>1500</value></setting><setting><name>TTL</name><value>32</value></setting></settings></properties>'
				SELECT @multicastSettings = REPLACE(@multicastSettings, 'XXX.XXX.XXX.XXX', @NextIPRange)
			END

			INSERT [Recorders] (
				[IDRecorder],
				[Name],
				[Description],
				[SynchronizationTime],
				[Enabled],
				[IDFailoverPrimaryGroup],
				[IDFailoverSecondaryGroup],
				[MulticastSettings],
				[LastModified]
			)
			VALUES (
				@RecorderId,
				@Name,
				'',
				-1,
				@authorized,
				'00000000-0000-0000-0000-000000000000',
				'00000000-0000-0000-0000-000000000000',
				@multicastSettings,
				@CurrentUtcTime
			)

			EXEC insert_server_group_member @RecorderId, '909749AE-88C4-4780-A381-383D691E98FA', 'RECORDING_SERVER'
		 
			SET @IsCreated = 1

			-- Send diffgram task (Recorders) to server
			SET @recorderAsDataSetAfter = dbo.recordersAsDataSet(@RecorderId)

			EXEC [create_task_server_services_transaction]
				@type = 'diffgram_insert',
				@data_a = @recorderAsDataSetAfter,
				@current_time = @CurrentUtcTime
		END
	
		IF (@FailOnUpdate = 1 AND @IsCreated = 0)
		BEGIN
			;THROW 65022, @procedureName, 1;
		END

		--Increment the failover udp port number if it is already taken by another recorder instance(Multi instance recorders).
		DECLARE @CanContinue bit 
		SET @CanContinue=1

		WHILE (@CanContinue = 1)
		BEGIN
			IF EXISTS (
				SELECT [failover_udp_port]
				FROM [RecordingServerService] WITH(UPDLOCK)
				WHERE [host_name] = @HostName AND [pkid] != @ServiceId AND [failover_udp_port] = @FailoverUdpPort)
			BEGIN
				SET @FailoverUdpPort  = @FailoverUdpPort + 1
			END
			ELSE 
				SET @CanContinue = 0
		END

		-- Make sure the recorder service exists
		IF NOT EXISTS (
			SELECT [pkid]
			FROM [RecordingServerService] WITH(UPDLOCK)
			WHERE [pkid] = @ServiceId)
		BEGIN
			INSERT [RecordingServerService] (
				[pkid],
				[host_name],
				[certificate_configured],
				[webserver_hostname],
				[webserver_port],
				[scan_ranges],
				[time_zone_name],
				[failover_udp_port],
				[devicepack_info],
				[x64],
				[database_port],
				[last_modified],
				[version]
			)
			VALUES (
				@ServiceId,
				@HostName,
				@CertificateConfigured,
				@WebServerHostName,
				@WebServerPort,
				@ScanRanges,
				@TimeZoneName,
				@FailoverUdpPort,
				@DevicePackInfo,
				1,
				@DatabasePort,
				@CurrentUtcTime,
				@Version
			)
			-- Insert a default row in configurable. 
			-- If you change the default, see admin_create_failover_server.sql where the same row is inserted.
			INSERT [RecordingServerServiceConfigurable] (
				[LastModified],
				[PKID],
				[MulticastServerAddress],
				[ShutdownOnStorageFailure],
				[PublicAccessEnabled],
				[PublicWebServerHostName],
				[PublicWebServerPort]
				)
			VALUES (
				@CurrentUtcTime,
				@ServiceId,
				'0.0.0.0',
				0,
				0,
				'',
				0
			)
		END
		ELSE
		BEGIN
			-- If there is any change in the data, update it and set the time stamp
			UPDATE [RecordingServerService]
			SET 	[host_name] = @HostName,
					[certificate_configured] = @CertificateConfigured,
					[webserver_hostname] = @WebServerHostName,
					[webserver_port] = @WebServerPort,
					[scan_ranges] = @ScanRanges,
					[time_zone_name] = @TimeZoneName,
					[failover_udp_port] = @FailoverUdpPort,
					[devicepack_info] = @DevicePackInfo,
					[database_port] = @DatabasePort,
					[last_modified] = @CurrentUtcTime,
					[version] = @Version
			WHERE [pkid] = @ServiceId 
					AND (
					[host_name] <> @HostName
					OR [certificate_configured] <> @CertificateConfigured
					OR [webserver_hostname] <> @WebServerHostName
					OR [webserver_port] <> @WebServerPort
					OR [scan_ranges] <> CAST(@ScanRanges AS NVARCHAR(MAX))
					OR [time_zone_name] <> @TimeZoneName
					OR [failover_udp_port] <> @FailoverUdpPort
					OR [devicepack_info] <> CAST(@DevicePackInfo AS NVARCHAR(MAX))
					OR [database_port] <> @DatabasePort
					OR [version] <> @Version
					)
		END
		
		-- Make sure the relation exists between the two
		IF NOT EXISTS (
			SELECT [recording_server_service_id]
			FROM [RecordingServerServiceRecorder] WITH(UPDLOCK)
			WHERE [recording_server_service_id] = @ServiceId)
		BEGIN
			INSERT [RecordingServerServiceRecorder] (
				[recording_server_service_id],
				[recorder_id],
				[type],
				[active],
				[last_modified]
			)
			VALUES (
				@ServiceId,
				@RecorderId,
				@Type,
				0,
				@CurrentUtcTime
			)
		END
		ELSE
		BEGIN
			DECLARE @old_recorder_id as [GuidType]
			-- select the service's current recorder id only if it is about to change and this service was the active one
			SET @old_recorder_id = (SELECT TOP 1 [recorder_id] FROM [RecordingServerServiceRecorder] rssr WHERE rssr.[active] = 1 AND rssr.[recording_server_service_id] = @ServiceId AND rssr.[recorder_id] <> @RecorderId)

			IF (@old_recorder_id IS NOT NULL)
			BEGIN
				UPDATE [RecordingServerServiceRecorder]
				SET [active] = 1,
					[last_modified] = @CurrentUtcTime
				WHERE [recorder_id] = @old_recorder_id
						AND [type] = 0
			END

			UPDATE [RecordingServerServiceRecorder]
			SET [recorder_id] = @RecorderId,
				[type] = @Type
			WHERE [recording_server_service_id] = @ServiceId
		END
	
		-- Set the current relation to active and all the others to inactive
		UPDATE	[RecordingServerServiceRecorder]
		SET		[active] = (CASE WHEN [recording_server_service_id] = @ServiceId THEN 1 ELSE 0 END),
				[last_modified] = @CurrentUtcTime
		WHERE	[recorder_id] = @RecorderId
			
	
		-- Send diffgram task (RecorderInfo) to all recorders
		IF (@Type = 0)
		BEGIN
			SET @dataSetAfter = dbo.recorderInfoAsDataSet(@RecorderId)
		END
		ELSE
		BEGIN
			SET @dataSetAfter = dbo.failoverInfoAsDataSet(@ServiceId)
		END

		IF (@dataSetBefore IS NULL)
		BEGIN
			EXEC [create_task_recorder_services_transaction]
				@type = 'diffgram_insert',
				@data_a = @dataSetAfter,
				@current_time = @CurrentUtcTime
		 
			SET @diffgram_created = 1
		END
		ELSE -- (@dataSetBefore IS NOT NULL)
		BEGIN
			-- Only send updates if row is actually changed
			DECLARE @xmlValuesDifferent BIT
			SET @xmlValuesDifferent = dbo.StringValueChanging(CAST(@dataSetBefore AS NVARCHAR(MAX)), CAST(@dataSetAfter AS NVARCHAR(MAX)))
			IF (@xmlValuesDifferent = 1)
			BEGIN
				EXEC [create_task_recorder_services_transaction]
					@type = 'diffgram_update',
					@data_a = @dataSetBefore,
					@data_b = @dataSetAfter,
					@current_time = @CurrentUtcTime
				SET @diffgram_created = 1
			END
		END

		-- Update the failover history table
		IF	(@Type = 0)
		BEGIN
			-- We are receiving the main recorder info
		
			-- Update the failover history with the end time
			UPDATE	[FailoverHistory]
			SET		[EndTime] = GETUTCDATE(),
					[LastModified] = GETUTCDATE()
			WHERE	[IDRecorder] = @RecorderId
			AND		ISNULL([StartTime], 0) <> 0
			AND		ISNULL([EndTime], 0) = 0
		END
		ELSE
		BEGIN
			-- We are either in failover mode or hot standby mode
			IF NOT EXISTS (SELECT [IDRecorder] FROM [FailoverHistory]
			WHERE [IDRecorder] = @RecorderId
				AND [IDFailover] = @FailoverId
				AND ISNULL([StartTime], 0) <> 0
				AND ISNULL([EndTime], 0) = 0)
			BEGIN
				-- Failover started, insert into failover history
				INSERT [FailoverHistory] (IDFailoverHistoryEntry, IDFailover, Description, IDRecorder, StartTime, LastModified)
				VALUES (NEWID(), @FailoverId, '', @RecorderId, GETUTCDATE(), GETUTCDATE())
			END
		END
	
		-- Delete entries in failover history older than 30 days
		DELETE [FailoverHistory]
		WHERE [LastModified] < DATEADD(dd, -30, GETUTCDATE())

		COMMIT TRAN

		SELECT @IsCreated as 'is_created', @diffgram_created as 'diffgram_created'
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