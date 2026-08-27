-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_active_mip_drivers]') AND type in (N'P', N'PC'))
DROP PROCEDURE query_active_mip_drivers
GO

-- This stored procedure will return all MIP drivers in use by active recording server. 
-- A MIP driver is defined as having a driver number that is either between 50000 and 59999 or is equal to 30005 (Arcules)
CREATE PROCEDURE query_active_mip_drivers
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
		hdr.[DriverVersion]
	FROM [HardwareDriverRecorder] hdr 
		INNER JOIN [RecordingServerService] rss ON rss.[pkid] = hdr.[IDRecordingServerService]
		INNER JOIN [RecordingServerServiceRecorder] rssr ON rssr.[recording_server_service_id] = rss.[pkid] AND rssr.[active] = 1
		LEFT JOIN Hardware h ON hdr.[IDHardwareDriver] = h.[IDHardwareDriver] AND h.[IDRecorder] = rssr.[recorder_id]
	WHERE 
		hdr.[DriverNumber] = 30005 OR hdr.[DriverNumber] BETWEEN 50000 AND 59999
	GROUP BY hdr.[IDHardwareDriver], hdr.[DriverNumber], hdr.[DriverName], hdr.[GroupName], hdr.[DriverVersion]
END
GO
