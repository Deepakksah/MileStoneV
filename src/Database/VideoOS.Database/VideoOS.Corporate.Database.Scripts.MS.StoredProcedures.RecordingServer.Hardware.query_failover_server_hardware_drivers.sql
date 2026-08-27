-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_failover_server_hardware_drivers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_failover_server_hardware_drivers]
GO

CREATE PROCEDURE [query_failover_server_hardware_drivers]
	@IDFailoverServer [GuidType]
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
		0 AS DriverCount,
		hdr.[DriverVersion],
		hdr.[XmlVersion]
	FROM [HardwareDriverRecorder] hdr 
		INNER JOIN [FailoverServers] fs ON fs.[IDRecordingServerService] = hdr.[IDRecordingServerService]
	WHERE 
		fs.[IDFailoverServer] = @IDFailoverServer
END
GO
