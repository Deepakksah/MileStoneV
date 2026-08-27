-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_old_scheme_recorders]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_old_scheme_recorders]
GO

CREATE PROCEDURE [query_old_scheme_recorders]
AS	
BEGIN
	SET NOCOUNT ON

	SELECT DISTINCT r.[IDRecorder]
	FROM	[Recorders] r
			INNER JOIN [RecordingServerServiceRecorder] rssr ON r.[IDRecorder] = rssr.[recorder_id]
			INNER JOIN (SELECT	[pkid],
								CONVERT(INT, LEFT([version], CHARINDEX('.', [version]) - 1)) AS [Major],
								CONVERT(INT, SUBSTRING([version], CHARINDEX('.', [version]) + 1, CHARINDEX('.', [version], CHARINDEX('.', [version]) + 1) - CHARINDEX('.', [version]) - 1)) AS [Minor]
						FROM	[RecordingServerService]
						) rss ON rss.[pkid] = rssr.[recording_server_service_id]
	WHERE	(rss.[Major] < 20 OR (rss.[Major] = 20 AND rss.[Minor] < 2)) AND -- Check version
			(rssr.[active] = 1 or rssr.[type] = 0 or rssr.[type] = 2) -- Check Recorder, HotStandby or active failover
END
GO