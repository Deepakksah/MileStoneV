-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_recorder_ids_changed_after]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_recorder_ids_changed_after]
GO

CREATE PROCEDURE [query_recorder_ids_changed_after]
	@changedAfter DATETIME
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		r.[IDRecorder],
		(SELECT MAX(d) FROM 
			(SELECT r.[LastModified] as d
			UNION 
			SELECT rssr.[last_modified]
			UNION 
			SELECT rss.[last_modified]
			UNION 
			SELECT rssc.[LastModified]) AS dates
		) AS [last_modified]
	FROM 
		[Recorders] r
		INNER JOIN [RecordingServerServiceRecorder] rssr ON r.IDRecorder = rssr.recorder_id
		INNER JOIN [RecordingServerService] rss ON rss.pkid = rssr.recording_server_service_id
		INNER JOIN [RecordingServerServiceConfigurable] rssc ON rssc.pkid = rssr.recording_server_service_id
	WHERE
		r.[LastModified] > @changedAfter
		OR rssr.[last_modified] > @changedAfter
		OR rss.[last_modified] > @changedAfter
		OR rssc.[LastModified] > @changedAfter
END
GO