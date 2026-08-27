-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Adds stored procedure to delete failover servers.
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_failover_server]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_failover_server]
GO

CREATE PROCEDURE [delete_failover_server]
	@IDFailoverServer [GuidType]
AS
BEGIN
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @deleted_service_service_id [GuidType]
		DECLARE @deleted_service_recorder_id [GuidType]
		DECLARE @deleted_service_active BIT

		DECLARE @taskTarget [GuidType]
		DECLARE @dataSetBefore XML

		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()
	
		-- Remove from ServerGroup
		EXEC remove_server_group_member @IDFailoverServer

		-- Get recording server service id.
		-- Also lock the failover server row to handle multi user issues
		SELECT	@deleted_service_service_id = [IDRecordingServerService]
		FROM	[FailoverServers] f WITH(UPDLOCK)
		WHERE	[IDFailoverServer] = @IDFailoverServer

		-- Get the recorder id and active state (if present) for the failover recorder service
		SELECT	@deleted_service_recorder_id = [recorder_id],
				@deleted_service_active = [active]
		FROM	[RecordingServerServiceRecorder]
		WHERE	[recording_server_service_id] = @deleted_service_service_id

		-- Lock on all RecordingServerServiceRecorder rows for the recorder id
		IF (@deleted_service_recorder_id IS NOT NULL)
		BEGIN
			DECLARE @tempValue INT
			-- Just assigning to @tempValue to avoid getting a result set
			SELECT @tempValue = COUNT([recorder_id])
			FROM [RecordingServerServiceRecorder] WITH(UPDLOCK)
			WHERE [recorder_id] = @deleted_service_recorder_id
		END

		-- Lock the RecordingServerService row (if it exists)
		IF (@deleted_service_service_id IS NOT NULL)
		BEGIN
			SELECT @deleted_service_service_id = [pkid]
			FROM [RecordingServerService] WITH(UPDLOCK)
			WHERE [pkid] = @deleted_service_service_id
		END

		-- Delete the RecordingServerService row for this failover server and a possible relation in the RecordingServerServiceRecorder
		IF (@deleted_service_service_id IS NOT NULL)
		BEGIN
			-- The failover did have a service associated with it's recording server

			IF (@deleted_service_recorder_id IS NOT NULL)
			BEGIN
				SET @dataSetBefore = dbo.recordingServerServiceRecorderAsDataSet(@deleted_service_service_id)

				DELETE [RecordingServerServiceRecorder] WHERE [recording_server_service_id] = @deleted_service_service_id

				-- Create a task for the primary recorder recorder
				SELECT @taskTarget = rssr.[recording_server_service_id]
				FROM [RecordingServerServiceRecorder] rssr
				WHERE rssr.[recorder_id] = @deleted_service_recorder_id AND rssr.[type] = 0

				IF(@taskTarget IS NULL)
				BEGIN
					;THROW 60000, @procedureName, 1;
				END

				EXEC [create_task_transaction]
					'diffgram_delete',
					@taskTarget,
					@dataSetBefore,
					NULL,
					@CurrentUtcTime
			END

			-- Get dataset before for FailoverInfo diffgram
			SET @dataSetBefore = NULL
			IF EXISTS (SELECT 1 FROM [FailoverInfo] WHERE [recording_server_service_id] = @deleted_service_service_id)
				SET @dataSetBefore = dbo.failoverInfoAsDataSet(@deleted_service_service_id)

			-- Delete RecordingServerServiceConfigurable row
			DELETE [RecordingServerServiceConfigurable] WHERE [pkid] = @deleted_service_service_id

			-- Delete RecordingServerService row
			DELETE [RecordingServerService] WHERE [pkid] = @deleted_service_service_id

			-- Create task with FailoverInfo diffgram if any row was deleted
			IF (@dataSetBefore IS NOT NULL)
			BEGIN
				EXEC [create_task_recorder_services_transaction]
					@type = 'diffgram_delete',
					@data_a = @dataSetBefore,
					@current_time = @CurrentUtcTime
			END

			-- Make sure that the main recording server service is set to active (in case we just deleted the active service)
			IF (@deleted_service_active = 1) -- if @deleted_service_active is null, this if will also be false
			BEGIN
				UPDATE [RecordingServerServiceRecorder]
				SET [active] = 1,
					[last_modified] = @CurrentUtcTime
				WHERE [recorder_id] = @deleted_service_recorder_id AND [type] = 0
			END
		END

		-- Delete the failover server	
		DELETE [FailoverServers]
		WHERE [IDFailoverServer] = @IDFailoverServer

		-- Unregister the security object
		EXEC [security_unregister_object_transaction]
			@namespace_name = 'FailoverServer',
			@object_id = @IDFailoverServer,
			@CurrentUtcTime = @CurrentUtcTime
	
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