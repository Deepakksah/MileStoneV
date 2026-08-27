-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_device_storage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_device_storage]
GO

CREATE PROCEDURE [update_device_storage]
	@deviceId [GuidType],
	@targetRecordingStorageId [GuidType],
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
		DECLARE @recordingStorageId [GuidType]
		DECLARE @dataSetBefore XML
		DECLARE @dataSetAfter XML
		DECLARE @device_name [NameType]
		DECLARE @device_type VARCHAR(100)
		DECLARE @recorder_service_id [GuidType]
		DECLARE @recordingserver_id [GuidType]
		DECLARE @recordingserver_name [NameType]
		DECLARE @targetRecordingStorage_Name [NameType]

		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		-- Make sure that hardware/recording server exists
		SELECT
			@recordingserver_id = h.[IDRecorder],
			@recordingserver_name = r.[Name],
			@device_name = d.[Name],
			@device_type = d.[DeviceType],
			@recorder_service_id = rssr.[recording_server_service_id]
		FROM
			[Devices] d 
			INNER JOIN [Hardware] h ON d.[IDHardware] = h.[IDHardware]
			INNER JOIN [Recorders] r ON r.[IDRecorder] = h.[IDRecorder]
			INNER JOIN [RecordingServerServiceRecorder] rssr ON rssr.[recorder_id] = r.[IDRecorder] AND rssr.[type] = 0
		WHERE d.[IDDevice] = @deviceId
		
		SELECT @rowCount = @@ROWCOUNT
		IF(@rowCount = 0)
		BEGIN
			;THROW 60064, @procedureName, 1;
		END

		-- Find the recording storage id the device belongs to
		SELECT @recordingStorageId = [recordingstorage_id] 
		FROM [RecordingStorageDevice] 
		WHERE [device_id] = @deviceId
		
		SELECT @rowCount = @@ROWCOUNT
		IF(@rowCount = 0)
		BEGIN
			;THROW 60064, @procedureName, 1;
		END
	
		-- Make sure the target recording storage id exists and is on the recorder where the device is.
		SELECT @targetRecordingStorage_Name = [Name]
		FROM [RecordingStorage] 
		WHERE [pkid] = @targetRecordingStorageId AND [recordingserver_id] = @recordingserver_id

		SELECT @rowCount = @@ROWCOUNT
		IF(@rowCount = 0)
		BEGIN
			;THROW 60000, @procedureName, 1;
		END

		-- Get DataSet before changes (for DiffGram)
		SET @dataSetBefore = dbo.recordingStorageDeviceAsDataSet(@deviceId)

		-- Update device recording storage id
		UPDATE [RecordingStorageDevice] 
		SET [recordingstorage_id] = @targetRecordingStorageId,
		[last_modified] = @CurrentUtcTime 
		WHERE [device_id] = @deviceId

		SELECT @rowCount = @@ROWCOUNT
		IF(@rowCount = 0)
		BEGIN
			;THROW 60000, @procedureName, 1;
		END
	
		-- Get DataSet after changes
		SET @dataSetAfter = dbo.recordingStorageDeviceAsDataSet(@deviceId)
	
		-- Call the create task procedure to create diffgram update task
		EXEC [create_task_transaction] 'diffgram_update', @recorder_service_id, @dataSetBefore, @dataSetAfter, @CurrentUtcTime

		-- Validate if a new Database.MoveArchive task can be created:
		-- No other database tasks on storage with same storage id.
		-- Exceptions: EmptyDevice or MoveDevice with different DeviceId.
		IF EXISTS (	SELECT 1
					FROM   [TaskProperties] tp
					WHERE ([TaskPropertyName] = 'storage_id' AND
					       [TaskPropertyValue] = @recordingStorageId AND
						   [TaskType] NOT IN ('Database.MoveDeviceDb'))
					   OR ([TaskPropertyName] = 'device_id' AND
					       [TaskPropertyValue] = @deviceId AND
						   [TaskType] IN ('Database.MoveDeviceDb'))
				  )
		BEGIN
			;THROW 60137, @procedureName, 1;
		END


		-- First create the new task row - with default task data
		INSERT [Task] (
			[type],
			[target],
			[data_a],
			[data_b],
			[percent_completed],
			[status_details], 
			[task_status],
			[start_time],
			[end_time]
		)
		VALUES (
			'Database.MoveDeviceDb',
			@recorder_service_id,
			'',
			NULL,
			0,
			'',
			0,
			@CurrentUtcTime,
			NULL
		)

		DECLARE @task_id BIGINT
		SET @task_id = @@IDENTITY
	
		DECLARE @task_type NVARCHAR(200)
		SET @task_type = 'Database.MoveDeviceDb'
	
		DECLARE @moveDataValue NVARCHAR(10) = '0'
	
		-- Now we update the task data with correct values
		DECLARE @data_a_xml AS XML
		SET @data_a_xml = '<task/>'
	
		SET @data_a_xml.modify('insert attribute type {sql:variable("@task_type")} as first into (/task)[1]')
		SET @data_a_xml.modify('insert attribute id {sql:variable("@task_id")} as last into (/task)[1]')

		SET @data_a_xml.modify('insert element target {sql:variable("@recorder_service_id")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element idRecorder {sql:variable("@recordingserver_id")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element idStorage {sql:variable("@recordingStorageId")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element idDevice {sql:variable("@deviceId")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element deviceName {sql:variable("@device_name")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element idStorageTarget {sql:variable("@targetRecordingStorageId")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element moveData {sql:variable("@moveDataValue")} as last into (/task)[1]')

		-- Update the task xml in the table
		UPDATE [Task]
		SET [data_a] = @data_a_xml
		WHERE [pkid] = @task_id
	
		EXEC create_pending_log @session_id, 20100, @deviceId, @device_name, @recordingserver_id, @recordingserver_name, @targetRecordingStorage_Name, @device_type

		SELECT @task_id AS task_id

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
