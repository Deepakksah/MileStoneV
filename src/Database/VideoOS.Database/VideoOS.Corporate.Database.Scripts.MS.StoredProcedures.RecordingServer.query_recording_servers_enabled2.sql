-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_recording_servers_enabled2]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_recording_servers_enabled2]
GO

CREATE PROCEDURE [query_recording_servers_enabled2]
  @IDRecorders tvp_StringTableGuidType READONLY
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @AllRecorders BIT
	SET @AllRecorders = 0

	IF((SELECT COUNT(ID) FROM @IDRecorders) = 0)
	BEGIN
		SET @AllRecorders = 1
	END

	SELECT
		(SELECT MAX(d) FROM 
			(SELECT r.[LastModified] as d
			UNION 
			SELECT rssr.[last_modified]
			UNION 
			SELECT rss.[last_modified]
			UNION 
			SELECT rssc.[LastModified]) AS dates
		) AS [last_modified],
		r.[IDRecorder], 
		rssr.recording_server_service_id as [IDService],
		r.[Name], 
		r.[Description], 
		r.[SynchronizationTime],
		rss.[host_name], 
		rss.[certificate_configured],
		rss.[webserver_hostname],
		rss.[webserver_port],
		rssc.[PublicAccessEnabled],
		rssc.[PublicWebServerHostName],
		rssc.[PublicWebServerPort],
		rss.[time_zone_name]
	FROM [Recorders] r 
	INNER JOIN [RecordingServerServiceRecorder] rssr ON r.[IDRecorder] = rssr.[recorder_id]
	INNER JOIN [RecordingServerService] rss ON rss.pkid = rssr.[recording_server_service_id]
	INNER JOIN [RecordingServerServiceConfigurable] rssc ON rssc.pkid = rssr.[recording_server_service_id]
	WHERE
		rssr.[active] = 1 AND
		r.[Enabled] = 1 AND
		(@AllRecorders = 1 OR r.[IDRecorder] IN (SELECT Ids.[ID] FROM @IDRecorders Ids))
END
GO