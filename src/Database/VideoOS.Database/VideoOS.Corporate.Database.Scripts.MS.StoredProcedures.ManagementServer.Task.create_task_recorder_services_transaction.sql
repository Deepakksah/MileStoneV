-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_task_recorder_services_transaction]') AND type = 'P')
DROP PROCEDURE [create_task_recorder_services_transaction]
GO

CREATE PROCEDURE [create_task_recorder_services_transaction]
	@type VARCHAR(50),
	@data_a XML,
	@data_b XML = NULL,
	@current_time DATETIME,
	@recorder_id [GuidType] = NULL,
	@active_only BIT = 0
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	
	-- Make sure that a current transaction is active
	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END

	-- Get recording server service ids for task creation
	DECLARE @recorder_service_id [GuidType]

	DECLARE recording_server_service CURSOR FORWARD_ONLY READ_ONLY FOR
		SELECT	rssr.[recording_server_service_id]
		FROM	[RecordingServerServiceRecorder] rssr
		WHERE	(@recorder_id IS NULL OR rssr.[recorder_id] = @recorder_id)
				AND (@active_only = 0 OR rssr.[active] = 1)


	OPEN recording_server_service
	BEGIN TRY
		FETCH NEXT FROM recording_server_service
		INTO @recorder_service_id

		WHILE(@@FETCH_STATUS = 0)
		BEGIN
			-- Call the create task procedure to create diffgram update task
			EXEC [create_task_transaction] @type, @recorder_service_id, @data_a, @data_b, @current_time

			FETCH NEXT FROM recording_server_service
			INTO @recorder_service_id
		END
	END TRY
	BEGIN CATCH
		CLOSE recording_server_service
		DEALLOCATE recording_server_service
		;THROW
	END CATCH

	CLOSE recording_server_service
	DEALLOCATE recording_server_service
END
GO