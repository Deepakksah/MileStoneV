-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dataset_recorder_delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dataset_recorder_delete]
GO

CREATE PROCEDURE [dataset_recorder_delete]
	@Original_IDRecorder [GuidType]
AS
BEGIN
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	DECLARE @dataSetBefore XML
	DECLARE @recordingstorage_id [GuidType]
	DECLARE @serviceId as [GuidType]

	DECLARE @CurrentUtcTime DATETIME
	SET @CurrentUtcTime = GETUTCDATE()

	-- Make sure that a current transaction is active
	IF (@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END

	-- Check for possible evidence locked data on this recorder
	IF (EXISTS(
		SELECT	1
		FROM	[DeviceTrackEvidenceLock]
		WHERE	[IDRecorder] = @Original_IDRecorder
	))
	BEGIN
		;THROW 60334, @procedureName, 1;
	END

	-- Get before-info for diffgram task
	SET @dataSetBefore = dbo.recorderInfoAsDataSet(@Original_IDRecorder)

	BEGIN TRY
		-- Delete archive storages
		DELETE	a
		FROM	[ArchiveStorage] a 
				INNER JOIN [RecordingStorage] r ON a.[recordingstorage_id] = r.[pkid]
		WHERE	r.[recordingserver_id]= @Original_IDRecorder

		-- Delete recording storage devices
		DELETE	rsd
		FROM	[RecordingStorageDevice] rsd 
				INNER JOIN [RecordingStorage] r ON rsd.[recordingstorage_id] = r.[pkid]
		WHERE	r.[recordingserver_id]= @Original_IDRecorder
	
		-- Delete recording storage
		DELETE [RecordingStorage] WHERE [recordingserver_id] = @Original_IDRecorder
	
		-- Delete recorder status data
		DELETE [RecorderStatusData] WHERE [IDRecorder] = @Original_IDRecorder
	
		-- Lookup the service id for the master recording server
		SELECT	@serviceId = [pkid]
		FROM	[RecordingServerService] rss
				INNER JOIN [RecordingServerServiceRecorder] rssr ON rssr.[recording_server_service_id] = rss.pkid
		WHERE	rssr.[type] = 0 -- RECORDER
				AND rssr.recorder_id = @Original_IDRecorder

		-- Delete tasks for the master recording server service
		IF (@serviceId IS NOT NULL)
		BEGIN
			DELETE [Task] WHERE [target] = @serviceId
		END

		-- Delete relations for all services that are using the recorder configuration (RS, FS or HS)
		DELETE [RecordingServerServiceRecorder] WHERE [recorder_id] = @Original_IDRecorder

		-- Delete the one and only master recorder service associated with this recorder id
		-- and the recording server configurable row associated with that.
		IF (@serviceId IS NOT NULL)
		BEGIN
			DELETE [RecordingServerServiceConfigurable] WHERE [pkid] = @serviceId

			DELETE [RecordingServerService] WHERE [pkid] = @serviceId
		END
	END TRY
	BEGIN CATCH
		;THROW 60000, @procedureName, 1;
	END CATCH
	
	DECLARE @namespace NVARCHAR(260) = 'RecordingServer'

	-- Unregister security object
	BEGIN TRY
		EXEC [security_unregister_object_transaction]
			@namespace_name = @namespace,
			@object_id = @Original_IDRecorder,
			@CurrentUtcTime = @CurrentUtcTime
	END TRY
	BEGIN CATCH
		SET @ErrorArgs = dbo.[FormatErrorMessage](ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE()) + N'%%; %%Namespace=' + @namespace + N'%%; %%ID=' + CAST(@Original_IDRecorder AS NVARCHAR(36)) + N'%%'
		;THROW 65001, @ErrorArgs, 1;
	END CATCH

	BEGIN TRY
		-- Delete the recorder configuration
		DELETE [Recorders] WHERE [IDRecorder] = @Original_IDRecorder
	
		DELETE [ServerGroupMembership] WHERE [IDObject] = @Original_IDRecorder
	END TRY
	BEGIN CATCH
		;THROW 60000, @procedureName, 1;
	END CATCH

	-- Create task for all other recorders
	EXEC [create_task_recorder_services_transaction]
		@type = 'diffgram_delete',
		@data_a = @dataSetBefore,
		@current_time = @CurrentUtcTime
END
GO