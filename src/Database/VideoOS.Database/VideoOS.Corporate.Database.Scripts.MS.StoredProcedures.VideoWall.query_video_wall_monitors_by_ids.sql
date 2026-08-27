-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_video_wall_monitors_by_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_video_wall_monitors_by_ids]
GO

CREATE PROCEDURE [query_video_wall_monitors_by_ids]
	@monitorIds tvp_StringTableGuidType READONLY
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		m.[pkid],
		m.[name],
		m.[description],
		m.[aspect_ratio],
		m.[insertion_method],
		m.[no_layout_method],
		m.[empty_viewitems_method],
		m.[monitor_size],
		m.[location_x],
		m.[location_y],
		m.[view_id],
		m.[created_date],
		m.[last_modified],
		m.[videowall_id],
		m.[monitor_state]
	FROM [VideoWallMonitor] m
	INNER JOIN @monitorIds ids
	ON m.[pkid] = ids.[ID]
END
GO