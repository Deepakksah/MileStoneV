-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_recording_servers_by_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_recording_servers_by_ids]
GO

CREATE PROCEDURE [query_recording_servers_by_ids]
  @IDRecorderIds tvp_StringTableGuidType READONLY
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	BEGIN TRY
		INSERT [ServerGroupMembership] ([IDServerGroup], [IDObject], [ObjectType], [LastModified])
		SELECT 
			'909749AE-88C4-4780-A381-383D691E98FA',
			r.IDRecorder,
			'RECORDING_SERVER',
			GETUTCDATE()
		FROM [Recorders] r
		LEFT JOIN [ServerGroupMembership] sgm ON sgm.[IDObject] = r.[IDRecorder]
		WHERE sgm.[IDObject] IS NULL
	END TRY
	BEGIN CATCH
		;THROW 60000, @procedureName, 1;
	END CATCH

	SELECT
		r.[IDRecorder],
		r.[Name],
		r.[Description],
		r.[SynchronizationTime],
		rss.[host_name],
		r.[Enabled],
		rss.[certificate_configured],
		rss.[webserver_hostname],
		rss.[webserver_port],
		rssc.[PublicAccessEnabled] AS [ActivePublicAccessEnabled],
		rssc.[PublicWebServerHostName] AS [ActivePublicWebServerHostName],
		rssc.[PublicWebServerPort] AS [ActivePublicWebServerPort],
		rssc2.[PublicAccessEnabled],
		rssc2.[PublicWebServerHostName],
		rssc2.[PublicWebServerPort],
		rssc2.[MulticastServerAddress],
		rssc2.[ShutdownOnStorageFailure],
		rss.[scan_ranges],
		rss.[time_zone_name],
		r.[IDFailoverPrimaryGroup],
		r.[IDFailoverSecondaryGroup],
		rss2.[failover_udp_port],
		rss2.[host_name] AS [failover_host_name],
		r.[MulticastSettings],
		rss.[devicepack_info],
		rss.[x64],
		COALESCE(fs.[IDFailoverServer], '00000000-0000-0000-0000-000000000000') AS [failover_server_id],
		(SELECT MAX(d) FROM 
			(SELECT r.[LastModified] as d
			UNION 
			SELECT rssr.[last_modified]
			UNION 
			SELECT rss.[last_modified]
			UNION 
			SELECT rssc.[LastModified]) AS dates
		) AS [last_modified],
		rss.[version]
	FROM
		[Recorders] r 
		INNER JOIN [RecordingServerServiceRecorder] rssr ON r.[IDRecorder] = rssr.[recorder_id] AND rssr.[active]= 1
		INNER JOIN [RecordingServerService] rss ON rss.pkid = rssr.[recording_server_service_id]
		INNER JOIN [RecordingServerServiceConfigurable] rssc ON rssc.pkid = rssr.[recording_server_service_id]
		-- The two joins below are needed to get the failover host name (host name of the main recording server service)
		INNER JOIN [RecordingServerServiceRecorder] rssr2 ON rssr2.[recorder_id] = r.[IDRecorder] AND rssr2.[type] = 0
		INNER JOIN [RecordingServerService] rss2 ON rss2.[pkid] = rssr2.[recording_server_service_id]
		INNER JOIN [RecordingServerServiceConfigurable] rssc2 ON rssc2.pkid = rssr2.[recording_server_service_id]
		-- Join for getting the failover id
		LEFT JOIN [FailoverServers] fs ON fs.[IDRecordingServerService] = rssr.[recording_server_service_id]
		INNER JOIN @IDRecorderIds ids ON r.[IDRecorder] = ids.[ID]
END
GO