-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_recording_server_hardware_drivers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_recording_server_hardware_drivers]
GO

CREATE PROCEDURE [query_recording_server_hardware_drivers]
	@IDRecorder [GuidType],
	@UsedOnly BIT
 AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	SELECT 
		hdr.[IDHardwareDriver],
		hdr.[DriverName], 
		hdr.[DriverNumber], 
		hdr.[GroupName], 
		COUNT(h.[DriverNumber]) AS DriverCount,
		hdr.[DriverVersion],
		hdr.[XmlVersion]
	FROM [HardwareDriverRecorder] hdr 
		INNER JOIN [RecordingServerService] rss ON rss.[pkid] = hdr.[IDRecordingServerService]
		INNER JOIN [RecordingServerServiceRecorder] rssr ON rssr.[recording_server_service_id] = rss.[pkid] AND rssr.[type] = 0
		LEFT JOIN Hardware h ON hdr.[IDHardwareDriver] = h.[IDHardwareDriver] AND h.[IDRecorder] = rssr.[recorder_id]
	WHERE 
		rssr.[recorder_id] = @IDRecorder
	GROUP BY hdr.[IDHardwareDriver], hdr.[DriverNumber],hdr.[DriverName], hdr.[GroupName], hdr.[DriverVersion], hdr.[XmlVersion]
	HAVING @UsedOnly = 0 OR COUNT(h.[DriverNumber]) > 0
END
GO
