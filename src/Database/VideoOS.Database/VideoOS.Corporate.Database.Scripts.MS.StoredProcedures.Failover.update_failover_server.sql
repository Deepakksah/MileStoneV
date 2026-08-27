-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_failover_server]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_failover_server]
GO

CREATE PROCEDURE [update_failover_server]
	@IDFailoverServer [GuidType],
	@Name [NameType],
	@Description [DescriptionType],
	@HotStandbyRecorderId [GuidType],
	@HostName NameType,
	@UdpPort INT,
	@DatabaseLocation [DescriptionType],
	@PublicAccessEnabled BIT,
	@PublicWebServerHostname NameType,
	@PublicWebServerPort INT,
	@MulticastServerAddress NameType,
	@Enabled BIT
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @taskTarget [GuidType]
		DECLARE @dataSetBefore XML
		DECLARE @dataSetAfter XML

		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		DECLARE @recording_server_service_id AS [GuidType]
		DECLARE @recorder_id AS [GuidType]


		-- Make sure the failover service exists and look up recording server service id
		SELECT @recording_server_service_id = fs.[IDRecordingServerService]
		FROM [FailoverServers] fs WITH(UPDLOCK)
		WHERE fs.[IDFailoverServer] = @IDFailoverServer
		IF (@recording_server_service_id IS NULL)
		BEGIN
			SET @ErrorArgs = @procedureName + N': %%IDFailoverServer=' + CAST(@IDFailoverServer AS NVARCHAR(36)) + N'%%'
			;THROW 60039, @ErrorArgs, 1;
		END

		-- Make sure that the associated recording server service exists
		IF NOT EXISTS (
			SELECT [pkid]
			FROM [RecordingServerServiceConfigurable] WITH(UPDLOCK)
			WHERE [pkid] = @recording_server_service_id)
		BEGIN
			SET @ErrorArgs = @procedureName + N': %%IDFailoverServer=' + CAST(@IDFailoverServer AS NVARCHAR(36)) + N'%%'
			;THROW 60039, @ErrorArgs, 1;
		END

		-- Get primary recorder service id (if any) for task creation
		SELECT	@taskTarget = rssr.[recording_server_service_id],
				@recorder_id = rssr.[recorder_id]
		FROM	[RecordingServerServiceRecorder] rssr
				INNER JOIN [RecordingServerServiceRecorder] rssr2 ON rssr2.[recorder_id] = rssr.[recorder_id] AND rssr.[type] = 0
		WHERE	rssr2.[recording_server_service_id] = @recording_server_service_id
	
		-- Set or remove hot standby
		IF (@HotStandbyRecorderId = '00000000-0000-0000-0000-000000000000')
		BEGIN
			-- Not hot standby
			SET @dataSetBefore = dbo.recordingServerServiceRecorderAsDataSet(@recording_server_service_id)

			UPDATE [RecordingServerServiceRecorder]
			SET [last_modified] = @CurrentUtcTime
				,[type] = 1
				,[active] = 0
			WHERE [recording_server_service_id] = @recording_server_service_id AND [type] = 2

			IF (@@ROWCOUNT > 0)
			BEGIN
				-- Generate task for the primary recorder
				IF (@taskTarget IS NOT NULL)
				BEGIN
					SET @dataSetAfter = dbo.recordingServerServiceRecorderAsDataSet(@recording_server_service_id)
					EXEC [create_task_transaction]
						'diffgram_update',
						@taskTarget,
						@dataSetBefore,
						@dataSetAfter,
						@CurrentUtcTime
				END

				-- In case we changed an active hot standby to an ordinary failover, and made in inactive - we need to make sure that there is an active service for this recorder
				IF NOT EXISTS (SELECT [recorder_id] FROM [RecordingServerServiceRecorder] WHERE [recorder_id] = @recorder_id AND [active] = 1)
				BEGIN
					UPDATE [RecordingServerServiceRecorder]
					SET [active] = 1,
						[last_modified] = @CurrentUtcTime
					WHERE [recorder_id] = @recorder_id AND [type] = 0
				END
			END
		END
		ELSE
		BEGIN
			-- Hot standby

			-- Check if there is already another hot standby in database and if so throw
			IF EXISTS (SELECT [recorder_id] 
				FROM [RecordingServerServiceRecorder]
				WHERE [type] = 2 AND [recorder_id] = @HotStandbyRecorderId AND [recording_server_service_id] != @recording_server_service_id)
			BEGIN
				;THROW 60460, @procedureName, 1; 
			END

			IF EXISTS (SELECT [recording_server_service_id] 
				FROM [RecordingServerServiceRecorder]
				WHERE [recording_server_service_id] = @recording_server_service_id)				

			BEGIN
				SET @dataSetBefore = dbo.recordingServerServiceRecorderAsDataSet(@recording_server_service_id)

				UPDATE [RecordingServerServiceRecorder]
				SET [last_modified] = @CurrentUtcTime
					,[recorder_id] = @HotStandbyRecorderId
					,[type] = 2
				WHERE [recording_server_service_id] = @recording_server_service_id

				IF (@taskTarget IS NOT NULL)
				BEGIN
					SET @dataSetAfter = dbo.recordingServerServiceRecorderAsDataSet(@recording_server_service_id)
					EXEC [create_task_transaction]
						'diffgram_update',
						@taskTarget,
						@dataSetBefore,
						@dataSetAfter,
						@CurrentUtcTime
				END
			END
			ELSE
			BEGIN
				INSERT INTO [RecordingServerServiceRecorder]
					([last_modified]
					,[recording_server_service_id]
					,[recorder_id]
					,[type]
					,[active])
				VALUES
					(@CurrentUtcTime
					,@recording_server_service_id
					,@HotStandbyRecorderId
					,2
					,0)

				IF (@taskTarget IS NOT NULL)
				BEGIN
					SET @dataSetAfter = dbo.recordingServerServiceRecorderAsDataSet(@recording_server_service_id)
					EXEC [create_task_transaction]
						'diffgram_insert',
						@taskTarget,
						@dataSetAfter,
						NULL,
						@CurrentUtcTime
				END
			END

			-- Clear failover groups on recorder
			SET @dataSetBefore = dbo.recordersAsDataSet(@HotStandbyRecorderId)

			UPDATE [Recorders]
			SET [LastModified] = @CurrentUtcTime,
				[IDFailoverPrimaryGroup] = '00000000-0000-0000-0000-000000000000',
				[IDFailoverSecondaryGroup] = '00000000-0000-0000-0000-000000000000'
			WHERE [IDRecorder] = @HotStandbyRecorderId
				AND ([IDFailoverPrimaryGroup] <> '00000000-0000-0000-0000-000000000000'
				OR [IDFailoverSecondaryGroup] <> '00000000-0000-0000-0000-000000000000')

			-- Create a diffgram_update task for all recorders containing the above changes
			IF (@@ROWCOUNT > 0 AND @taskTarget IS NOT NULL)
			BEGIN
				SET @dataSetAfter = dbo.recordersAsDataSet(@HotStandbyRecorderId)
				EXEC [create_task_transaction]
					'diffgram_update',
					@taskTarget,
					@dataSetBefore,
					@dataSetAfter,
					@CurrentUtcTime
			END
		END

		UPDATE [FailoverServers]
		SET [Name] = @Name,
			[Description] = @Description,
			[HostName] = @HostName,
			[UdpPort] = @UdpPort,
			[Enabled] = @Enabled,
			[DatabaseLocation] = @DatabaseLocation,
			[LastModified] = @CurrentUtcTime
		WHERE [IDFailoverServer] = @IDFailoverServer

		-- If the @MulticastServerAddress is passed as null, blank or spaces, 
		-- it's considered 'no value', so pick up the existing value.
		IF (IsNull(LTrim(@MulticastServerAddress), N'') = N'')
		BEGIN
			SELECT @MulticastServerAddress = [MulticastServerAddress]
			FROM [RecordingServerServiceConfigurable]
			WHERE [pkid] = @recording_server_service_id
		END

		SET @dataSetBefore = dbo.recordingServerServiceConfigurableAsDataSet(@recording_server_service_id)

		UPDATE [RecordingServerServiceConfigurable]
		SET [PublicAccessEnabled] = @PublicAccessEnabled,
			[PublicWebServerHostname] = @PublicWebServerHostname,
			[PublicWebServerPort] = @PublicWebServerPort,
			[MulticastServerAddress] = @MulticastServerAddress,
			[LastModified] = @CurrentUtcTime
		WHERE 
			[pkid] = @recording_server_service_id
			AND ([PublicAccessEnabled] <> @PublicAccessEnabled OR 
					[PublicWebServerHostname] <> @PublicWebServerHostname OR 
					[PublicWebServerPort] <> @PublicWebServerPort OR
					[MulticastServerAddress] <> @MulticastServerAddress)

		-- Create a diffgram_update task for the failover recorder.
		IF (@@ROWCOUNT > 0)
		BEGIN
			SET @dataSetAfter = dbo.recordingServerServiceConfigurableAsDataSet(@recording_server_service_id)
			EXEC [create_task_transaction]
				'diffgram_update',
				@recording_server_service_id,
				@dataSetBefore,
				@dataSetAfter,
				@CurrentUtcTime
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