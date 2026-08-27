-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_active_recorders_datacollector_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_active_recorders_datacollector_ids]
GO

CREATE PROCEDURE [query_active_recorders_datacollector_ids]
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		dc.[pkid] as Id
	FROM
		[Recorders] r 
		INNER JOIN [RecordingServerServiceRecorder] rssr ON r.[IDRecorder] = rssr.[recorder_id]
		INNER JOIN [RecordingServerService] rss ON rss.[pkid] = rssr.[recording_server_service_id]
		INNER JOIN [DataCollector] dc ON dc.[host_name] = rss.[host_name]
		INNER JOIN [DataCollectorInstalledServices] dis ON dc.[pkid] = dis.[data_collector_id]
	WHERE
		rssr.[active] = 1
END
GO