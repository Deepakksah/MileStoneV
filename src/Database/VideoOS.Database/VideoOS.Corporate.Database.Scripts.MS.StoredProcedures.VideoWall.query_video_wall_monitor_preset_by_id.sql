-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_video_wall_monitor_preset_by_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_video_wall_monitor_preset_by_id]
GO

CREATE PROCEDURE [query_video_wall_monitor_preset_by_id]
	@pkid [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	SELECT 
		vwmp.[pkid],
		vwp.[name],
		vwmp.[last_modified],
		vwmp.[definition_xml],
		vwp.[pkid] AS [videowall_preset_id],
		vwmp.[monitor_id],
		vwp.[videowall_id]
	FROM [VideoWallMonitorPreset] vwmp
	INNER JOIN [VideoWallPreset] vwp ON vwp.[pkid] = vwmp.[videowallpreset_id]
	WHERE vwmp.[pkid] = @pkid
END
GO