-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[reporting_web_recording_server_web_address]') AND type in (N'P', N'PC'))
DROP PROCEDURE [reporting_web_recording_server_web_address]
GO

CREATE PROCEDURE [reporting_web_recording_server_web_address]
	@recorder_id GuidType
AS
BEGIN
	SELECT IIF(rss.[certificate_configured] = 1, 'https://', 'http://') + rss.[webserver_hostname] + ':' + cast(rss.[webserver_port] as varchar(256)) + '/' AS [RecorderWebServerUri]
	FROM   [RecordingServerService] rss
	INNER JOIN [RecordingServerServiceRecorder] rssr 
		ON rss.[pkid] = rssr.[recording_server_service_id] 
		AND rssr.[type] = 0
	WHERE  rssr.[recorder_id] = @recorder_id
	AND rssr.[active] = 1
END
GO