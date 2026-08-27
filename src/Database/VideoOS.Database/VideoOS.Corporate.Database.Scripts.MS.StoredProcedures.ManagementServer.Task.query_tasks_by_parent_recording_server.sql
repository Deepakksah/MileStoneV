-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_tasks_by_parent_recording_server]') AND type = 'P')
DROP PROCEDURE [query_tasks_by_parent_recording_server]
GO

CREATE PROCEDURE [query_tasks_by_parent_recording_server]
	@recordingServerId [GuidType]
AS
BEGIN
  SET NOCOUNT ON

  SELECT * FROM
  (SELECT
		t.[pkid],
		t.[type],
		t.[target],
		t.[data_a],
		t.[data_b],
		t.[percent_completed],
		t.[status_details],
		t.[task_status],
		t.[start_time],
		t.[end_time],
		coalesce( t.[data_a].value('(task/idRecorder)[1]','VARCHAR(36)'), t.[target]) as idParent
  FROM
		[Task] t 
  WHERE t.[type] NOT IN (
		'diffgram_insert',
		'diffgram_update',
		'diffgram_delete',
		'diffgram_security_insert',
		'diffgram_security_update',
		'diffgram_security_delete'
  )
  ) t2 INNER JOIN [RecordingServerServiceRecorder] rssr ON rssr.[recording_server_service_id] = t2.idParent AND rssr.[type] = 0
  WHERE rssr.[recorder_id] = @recordingServerId
  ORDER BY t2.[pkid] ASC
END
GO
