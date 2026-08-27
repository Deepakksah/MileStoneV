-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[request_edge_storage_retrieval]') AND type in (N'P', N'PC'))
DROP PROCEDURE [request_edge_storage_retrieval]
GO

CREATE PROCEDURE [request_edge_storage_retrieval]
	@deviceId [GuidType],
	@edgeStartTime NVARCHAR(MAX),
	@edgeEndTime NVARCHAR(MAX),
	@metadata NVARCHAR(MAX),
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
		DECLARE @device_name [NameType]
		DECLARE @device_type VARCHAR(100)
		DECLARE @recordingserver_id [GuidType]
		DECLARE @recorder_service_id [GuidType]
		DECLARE @recordingserver_name [NameType]
	
		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		-- Make sure that hardware/recording server exists
		SELECT @recordingserver_id = h.[IDRecorder],
			   @recorder_service_id = rssr.[recording_server_service_id],
			   @recordingserver_name = r.[Name],
			   @device_name = d.[Name],
			   @device_type = d.[DeviceType]
		FROM 
			[Devices] d 
			INNER JOIN [Hardware] h ON d.IDHardware = h.IDHardware
			INNER JOIN [Recorders] r ON r.IDRecorder = h.[IDRecorder]
			INNER JOIN [RecordingServerServiceRecorder] rssr ON rssr.[recorder_id] = r.[IDRecorder] AND rssr.[type] = 0
		WHERE 
			d.[IDDevice] = @deviceId
		
		SELECT @rowCount = @@ROWCOUNT
		IF(@rowCount = 0)
		BEGIN
			;THROW 60064, @procedureName, 1;
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
			'Edge.StorageRetrieval',
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
		SET @task_type = 'Edge.StorageRetrieval'
	
		-- Now we update the task data with correct values
		DECLARE @data_a_xml AS XML
		SET @data_a_xml = '<task/>'
	
		SET @data_a_xml.modify('insert attribute type {sql:variable("@task_type")} as first into (/task)[1]')
		SET @data_a_xml.modify('insert attribute id {sql:variable("@task_id")} as last into (/task)[1]')

		SET @data_a_xml.modify('insert element target {sql:variable("@recorder_service_id")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element idRecorder {sql:variable("@recordingserver_id")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element idDevice {sql:variable("@deviceId")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element deviceName {sql:variable("@device_name")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element startTime {sql:variable("@edgeStartTime")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element endTime {sql:variable("@edgeEndTime")} as last into (/task)[1]')
		SET @data_a_xml.modify('insert element metadata {sql:variable("@metadata")} as last into (/task)[1]')

		-- Update the task xml in the table
		UPDATE [Task]
		SET [data_a] = @data_a_xml
		WHERE [pkid] = @task_id
	
		EXEC create_pending_log @session_id, 20109, @deviceId, @device_name, @recordingserver_id, @recordingserver_name, @device_type

		SELECT @task_id AS task_id

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
