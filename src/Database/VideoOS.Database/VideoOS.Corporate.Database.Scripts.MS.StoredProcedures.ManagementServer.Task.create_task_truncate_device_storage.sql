-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_task_truncate_device_storage]') AND type = 'P')
DROP PROCEDURE [create_task_truncate_device_storage]
GO

CREATE PROCEDURE [create_task_truncate_device_storage]
	@device_id [GuidType],
	@recording_server_id [GuidType],
	@session_id [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		DECLARE @task_type VARCHAR(50)
		SET @task_type = 'Database.TruncateDeviceStorage'

		BEGIN TRANSACTION
	
		DECLARE @recorder_service_id [GuidType]
		DECLARE @device_name [NameType]
		DECLARE @device_type VARCHAR(100)

		-- Lookup device name and validate that device exists
		SELECT
			@device_name = d.[Name],
			@device_type = d.[DeviceType]
		FROM
			[Devices] d
		WHERE
			d.[IDDevice] = @device_id

		IF(@device_name IS NULL)
		BEGIN
			;THROW 60309, @procedureName, 1;
		END

		IF(@recording_server_id IS NULL OR @recording_server_id = '')
		BEGIN
			-- Recording server id is not specified, look it up from device id
			SELECT
				@recorder_service_id = rssr.[recording_server_service_id],
				@recording_server_id = hw.[IDRecorder],
				@device_name = d.[Name]
			FROM
				[Hardware] hw
				INNER JOIN [Devices] d ON d.[IDHardware] = hw.[IDHardware]
				INNER JOIN [RecordingServerServiceRecorder] rssr ON rssr.[recorder_id] = hw.[IDRecorder] AND rssr.[type] = 0
			WHERE
				d.[IDDevice] = @device_id

			IF(@recording_server_id IS NULL)
			BEGIN
				;THROW 60310, @procedureName, 1;
			END
		END
		ELSE
		BEGIN
			-- Recording server id is specified, validate that it exists
			SELECT
				@recorder_service_id = rssr.[recording_server_service_id]
			FROM
				[RecordingServerServiceRecorder] rssr
			WHERE
				rssr.[recorder_id] = @recording_server_id
				AND rssr.[type] = 0 -- Recording server

			IF(@recorder_service_id IS NULL)
			BEGIN
				;THROW 60310, @procedureName, 1;
			END
		END

		DECLARE @recording_server_name [NameType]
		SELECT
			@recording_server_name = rs.[Name]
		FROM
			[Recorders] rs
		WHERE
			rs.[IDRecorder] = @recording_server_id

		IF(@recording_server_name IS NULL)
		BEGIN
			;THROW 60310, @procedureName, 1;
		END

		-- Task property needed for compatibility with vmo clients before version 10
		DECLARE @recordingstorage_id as [GuidType]
		SET @recordingstorage_id = '00000000-0000-0000-0000-000000000000' 

		-- Find the tracks that are expected to be truncated
		DECLARE @tracks_xml XML
		SELECT @tracks_xml = 
		(
			SELECT * FROM
				(
				SELECT dt.IDTrack as track_Id FROM [Devices] AS d
					INNER JOIN [Hardware] AS h ON d.[IDHardware] = h.[IDHardware]
					INNER JOIN [DeviceTrack] AS dt ON d.[IDDevice] = dt.[IDDevice]
				WHERE (h.[IDRecorder] = @recording_server_id AND d.[IDDevice] = @device_id)
				UNION ALL
				SELECT [track_id] FROM [DeviceDataDistribution]
				WHERE ([recorder_id] = @recording_server_id AND [device_id] = @device_id)
				) t(track)
			FOR XML PATH (''), ROOT ('tracks'), TYPE
		)
		DECLARE @tracks_xml_string NVARCHAR(4000)
		SET @tracks_xml_string = CAST(@tracks_xml AS NVARCHAR(4000))

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
			@task_type,
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
	
		-- Now we update the task data with correct values
		DECLARE @data_a_xml XML
		SET @data_a_xml = '<task/>'
	
		SET @data_a_xml.modify('insert attribute type {sql:variable("@task_type")} as first into (/task)[1]')
		SET @data_a_xml.modify('insert attribute id {sql:variable("@task_id")} as last into (/task)[1]')

		SET @data_a_xml.modify('insert element target {sql:variable("@recorder_service_id")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element idRecorder {sql:variable("@recording_server_id")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element idStorage {sql:variable("@recordingstorage_id")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element idDevice {sql:variable("@device_id")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element deviceName {sql:variable("@device_name")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element tracksXml {sql:variable("@tracks_xml_string")} as last into (/task)[1]')

		-- Update the task xml in the table
		UPDATE [Task]
		SET [data_a] = @data_a_xml
		WHERE [pkid] = @task_id

		SELECT @task_id AS task_id
	
		COMMIT TRANSACTION

		EXEC create_pending_log @session_id, 4032, @device_id, @device_name, @recording_server_id, @recording_server_name, @device_type
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