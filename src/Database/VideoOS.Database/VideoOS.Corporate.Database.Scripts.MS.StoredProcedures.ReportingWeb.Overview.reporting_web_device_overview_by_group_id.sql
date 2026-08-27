-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[reporting_web_device_overview_by_group_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [reporting_web_device_overview_by_group_id]
GO

CREATE PROCEDURE [reporting_web_device_overview_by_group_id]
	@device_group_id GuidType
 AS
BEGIN
	SELECT
		d.[IDDevice] AS DeviceId,
		d.[Name] AS DeviceName,
		IIF(rss.[certificate_configured] = 1, 'https://', 'http://') + rss.[webserver_hostname] + ':' + cast(rss.[webserver_port] as varchar(256)) + '/' AS [RecorderWebServerUri],
		rs.[Name] AS RecordingStorageName,
		r.[Name] AS RecorderName,
		 CameraHostName =
		CASE
			WHEN hs.internal_address IS NOT NULL THEN hs.internal_address
			ELSE h.URI
		END,
		r.[IDRecorder] as RecorderId,
		dc.pkid as CollectorId
	FROM
		[DeviceGroupMembers] dgm
		INNER JOIN [Devices] d ON d.[IDDevice] = dgm.[IDDevice] AND d.[Enabled] = 1
		INNER JOIN [Hardware] h ON h.[IDHardware] = d.[IDHardware] AND h.[Enabled] = 1
		INNER JOIN [Recorders] r ON r.[IDRecorder] = h.[IDRecorder]
		INNER JOIN [RecordingStorageDevice] rsd ON rsd.[device_id] = d.[IDDevice]
		INNER JOIN [RecordingStorage] rs ON rs.[pkid] = rsd.[recordingstorage_id]
		-- Two joins for getting the active recording server service
		INNER JOIN [RecordingServerServiceRecorder] rssr ON r.[IDRecorder] = rssr.[recorder_id] AND rssr.[active] = 1
		INNER JOIN [RecordingServerService] rss ON rss.pkid = rssr.[recording_server_service_id]
		-- Three joins for getting the collector id of the main recording server (identified by the host name of the main recording server service)
		INNER JOIN [RecordingServerServiceRecorder] rssr2 ON rssr2.[recorder_id] = r.[IDRecorder] AND rssr2.[type] = 0
		INNER JOIN [RecordingServerService] rss2 ON rss2.[pkid] = rssr2.[recording_server_service_id]
		INNER JOIN [DataCollector] dc ON dc.[host_name] = rss2.[host_name]
		LEFT JOIN [HostedHardware] hh ON h.HostedHardware_id = hh.pkid
		LEFT JOIN [HostingServer] hs ON hs.pkid = hh.hosting_server_id
	WHERE
		dgm.[IDDeviceGroup] = @device_group_id
		
		
END
GO
