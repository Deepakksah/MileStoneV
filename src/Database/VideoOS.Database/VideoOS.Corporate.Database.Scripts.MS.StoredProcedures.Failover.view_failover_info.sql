-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[FailoverInfo]'))
DROP VIEW [dbo].[FailoverInfo]
GO

CREATE VIEW [dbo].[FailoverInfo]
AS
	SELECT
		rssr.[recording_server_service_id]
		, rss.[host_name]
		, rss.[database_port]
	FROM
		[RecordingServerServiceRecorder] rssr
		INNER JOIN [RecordingServerService] rss ON rss.[pkid] = rssr.[recording_server_service_id]
	WHERE
		rssr.[type] <> 0
GO