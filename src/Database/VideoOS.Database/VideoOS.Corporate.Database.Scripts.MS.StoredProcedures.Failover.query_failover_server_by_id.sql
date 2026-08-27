-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_failover_server_by_id]') AND type = 'P')
DROP PROCEDURE [query_failover_server_by_id]
GO

CREATE PROCEDURE [query_failover_server_by_id]
	@IDFailoverServer [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		f.[IDFailoverServer],
		rssr.recorder_id AS [HotStandbyRecorderId],
		f.[Name],
		f.[Description],
		f.[HostName],
		f.[UdpPort],
		f.[GroupSequence],
		f.[Enabled],
		f.[DatabaseLocation],
		f.[LastModified],
		rss.[certificate_configured] AS [CertificateConfigured], 
		rss.[webserver_hostname] AS [WebServerHostName], 
		rss.[webserver_port] AS [WebServerPort],
		rssc.PublicAccessEnabled,
		rssc.PublicWebServerHostname,
		rssc.PublicWebServerPort,
		rssc.MulticastServerAddress,
		f.[IDRecordingServerService],
		COALESCE( sg.[RelationIds], sgm.IDServerGroup) AS [RelationIds]
	FROM
		[FailoverServers] f
		INNER JOIN [RecordingServerService] rss ON rss.[pkid] = f.[IDRecordingServerService]
		INNER JOIN [RecordingServerServiceConfigurable] rssc ON rssc.[pkid] = f.[IDRecordingServerService]
		LEFT JOIN [RecordingServerServiceRecorder] rssr ON rssr.[recording_server_service_id] = f.[IDRecordingServerService] AND [type] = 2
		LEFT JOIN [ServerGroupMembership] sgm ON sgm.IDObject = f.IDFailoverServer
		LEFT JOIN [ServerGroup] sg ON sg.IDServerGroup = sgm.IDServerGroup
	WHERE
		f.[IDFailoverServer] = @IDFailoverServer
END
GO