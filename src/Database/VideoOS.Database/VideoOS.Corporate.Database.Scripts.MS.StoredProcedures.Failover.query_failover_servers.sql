-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_failover_servers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_failover_servers]
GO

CREATE PROCEDURE [query_failover_servers]
	@FilterOnServerGroup [bit],
	@IDServerGroup [GuidType] = ''
AS
BEGIN
	SET NOCOUNT ON

	IF (@FilterOnServerGroup = 0)
	BEGIN
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
			rssc.PublicAccessEnabled AS [PublicAccessEnabled],
			rssc.PublicWebServerHostname AS [PublicWebServerHostname],
			rssc.PublicWebServerPort AS [PublicWebServerPort],
			rssc.MulticastServerAddress AS [MulticastServerAddress],
			f.[IDRecordingServerService],
			COALESCE( sg.[RelationIds], sgm.IDServerGroup) AS [RelationIds]
		FROM 
			[FailoverServers] f
			INNER JOIN [RecordingServerService] rss ON rss.[pkid] = f.[IDRecordingServerService]
			INNER JOIN [RecordingServerServiceConfigurable] rssc ON rssc.[pkid] = f.[IDRecordingServerService]
			LEFT JOIN [ServerGroupMembership] sgm ON sgm.IDObject = f.IDFailoverServer
			LEFT JOIN [RecordingServerServiceRecorder] rssr ON rssr.[recording_server_service_id] = f.[IDRecordingServerService] AND [type] = 2
			LEFT JOIN [ServerGroup] sg ON sg.IDServerGroup = sgm.IDServerGroup
	END
	ELSE
	BEGIN
		IF(@IDServerGroup IS NULL OR @IDServerGroup = '')
		BEGIN
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
				rssc.PublicAccessEnabled AS [PublicAccessEnabled],
				rssc.PublicWebServerHostname AS [PublicWebServerHostname],
				rssc.PublicWebServerPort AS [PublicWebServerPort],
				rssc.MulticastServerAddress AS [MulticastServerAddress],
				f.[IDRecordingServerService],
				'' AS [RelationIds]
			FROM 
				[FailoverServers] f
				INNER JOIN [RecordingServerService] rss ON rss.[pkid] = f.[IDRecordingServerService]
				INNER JOIN [RecordingServerServiceConfigurable] rssc ON rssc.[pkid] = f.[IDRecordingServerService]
				LEFT JOIN [ServerGroupMembership] sgm ON sgm.IDObject = f.IDFailoverServer
				LEFT JOIN [RecordingServerServiceRecorder] rssr ON rssr.[recording_server_service_id] = f.[IDRecordingServerService] AND [type] = 2
			WHERE
				sgm.IDServerGroup IS NULL
		END
		ELSE
		BEGIN
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
				rssc.PublicAccessEnabled AS [PublicAccessEnabled],
				rssc.PublicWebServerHostname AS [PublicWebServerHostname],
				rssc.PublicWebServerPort AS [PublicWebServerPort],
				rssc.MulticastServerAddress AS [MulticastServerAddress],
				f.[IDRecordingServerService],
				COALESCE( sg.[RelationIds], sgm.IDServerGroup) AS [RelationIds]
			FROM
				[FailoverServers] f
				INNER JOIN [RecordingServerService] rss ON rss.[pkid] = f.[IDRecordingServerService]
				INNER JOIN [RecordingServerServiceConfigurable] rssc ON rssc.[pkid] = f.[IDRecordingServerService]
				INNER JOIN [ServerGroupMembership] sgm ON sgm.[IDObject] = f.[IDFailoverServer]
				LEFT JOIN [RecordingServerServiceRecorder] rssr ON rssr.[recording_server_service_id] = f.[IDRecordingServerService] AND [type] = 2
				LEFT JOIN [ServerGroup] sg ON sg.IDServerGroup = sgm.IDServerGroup
			WHERE
				sgm.IDServerGroup = @IDServerGroup
		END
	END
END
GO