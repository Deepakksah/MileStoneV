-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_by_ids_sorted]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_by_ids_sorted]
GO

CREATE PROCEDURE [query_device_by_ids_sorted]
	@ids tvp_StringTableGuidType READONLY
 AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	SELECT
		d.[IDDevice],
		d.[Name],
		d.[Description],
		d.[DeviceType],
		d.[Enabled],
		d.[DeviceIndex],
		d.[Settings],
		d.[LastModified],
		d.[GisPoint].AsTextZM() AS [GisPointString],
		d.[ShortName],
		d.[Shortcut],
		d.[Icon],
		d.[CoverageDirection],
		d.[CoverageDepth],
		d.[CoverageFieldOfView],
		h.[IDRecorder],
		h.[IDHardware],
		d.[MulticastEnabled],
		d.[PtzManualSessionTimeout],
		d.[PtzReservedSessionTimeout],
		d.[PtzExtendedManualSessionTimeout],
		dpsd.[Value] AS [PtzEnabled],
		d.[FailoverLiveOnly],
		d.[DisableOnFailover],
		COALESCE(rsd.[recordingstorage_id], CAST('00000000-0000-0000-0000-000000000000' AS CHAR(36))) AS [recordingstorage_id],
		COALESCE(rsd.[recording_enabled], CAST(0 AS BIT)) AS [recording_enabled],
		COALESCE(rsd.[recording_framerate], CAST(5 AS FLOAT)) AS [recording_framerate],
		COALESCE(rsd.[prebuffer_enabled], CAST(0 AS BIT)) AS [prebuffer_enabled],
		COALESCE(rsd.[prebuffer_in_memory], CAST(0 AS BIT)) AS [prebuffer_in_memory],
		COALESCE(rsd.[prebuffer_seconds], 3) AS [prebuffer_seconds],
		COALESCE(rsd.[edgestorage_enabled], CAST(0 AS BIT)) AS [edgestorage_enabled],
		COALESCE(rsd.[edgestorage_playback], CAST(0 AS BIT)) AS [edgestorage_playback],
		COALESCE(rsd.[record_keyframes], CAST(0 AS BIT)) AS [record_keyframes],
		COALESCE(rsd.[manual_timeout_enabled], CAST(0 AS BIT)) AS [manual_timeout_enabled],
		COALESCE(rsd.[manual_timeout], 5) AS [manual_timeout],
		COALESCE(rsd.[record_on_related_devices], CAST(0 AS BIT)) AS [record_on_related_devices]
	FROM [Devices] d
	INNER JOIN [Hardware] h ON h.[IDHardware] = d.[IDHardware]
	LEFT JOIN [RecordingStorageDevice] rsd ON rsd.[device_id] = d.[IDDevice]
	LEFT JOIN [DevicePtzSettingData] dpsd ON d.[IDDevice] = dpsd.[IDDevice] and dpsd.[TypeId] = '338D6DB1-160E-4CAC-A5D4-3FCF8068A955'
	INNER JOIN @ids ids ON d.[IDDevice] = ids.[ID] ORDER BY d.[Name]
END
GO
