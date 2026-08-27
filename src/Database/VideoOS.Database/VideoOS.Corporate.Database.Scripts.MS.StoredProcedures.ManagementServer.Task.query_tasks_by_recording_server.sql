-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_tasks_by_recording_server]') AND type = 'P')
DROP PROCEDURE [query_tasks_by_recording_server]
GO

CREATE PROCEDURE [query_tasks_by_recording_server]
	@recordingServerId [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		t.[pkid],
		t.[type],
		t.[target],
		t.[data_a],
		t.[data_b],
		t.[percent_completed],
		t.[status_details],
		t.[task_status],
		t.[start_time],
		t.[end_time]
	FROM
		[Task] t INNER JOIN [RecordingServerServiceRecorder] rssr ON rssr.[recording_server_service_id] = t.[target] AND rssr.[type] = 0
	WHERE t.[type] NOT IN (
		'diffgram_insert',
		'diffgram_update',
		'diffgram_delete',
		'diffgram_security_insert',
		'diffgram_security_update',
		'diffgram_security_delete'
	)
	AND rssr.[recorder_id] = @recordingServerId
	ORDER BY t.[pkid] ASC
END
GO
