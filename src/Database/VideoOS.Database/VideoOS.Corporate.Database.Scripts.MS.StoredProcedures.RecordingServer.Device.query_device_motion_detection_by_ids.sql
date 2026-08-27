-- This header is used by the dbupdate tool]
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on] The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes]
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_motion_detection_by_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_motion_detection_by_ids]
GO

CREATE PROCEDURE [query_device_motion_detection_by_ids]
	@IDDevices tvp_StringTableGuidType READONLY
AS
BEGIN
	SET NOCOUNT ON
    
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	SELECT
		rpd.[IDDevice],
		rpd.[Enabled],
		rpd.[Settings],
		rpd.[LastModified],
		h.[IDHardware],
		h.[IDRecorder]
	FROM [RecorderPluginDevice] rpd
	INNER JOIN [Devices] d ON d.[IDDevice] = rpd.[IDDevice]
	INNER JOIN [Hardware] h ON h.[IDHardware] = d.[IDHardware]
	INNER JOIN @IDDevices ids ON d.[IDDevice] = ids.[ID]
	AND rpd.[IDRecorderPlugin] = 'f020c881-72bf-4014-8504-2b4a5a658f1a'
END
GO