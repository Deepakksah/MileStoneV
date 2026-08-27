-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_video_wall_monitor_by_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_video_wall_monitor_by_id]
GO

CREATE PROCEDURE [query_video_wall_monitor_by_id]
	@pkid [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		[pkid],
		[name],
		[description],
		[aspect_ratio],
		[insertion_method],
		[no_layout_method],
		[empty_viewitems_method],
		[monitor_size],
		[location_x],
		[location_y],
		[view_id],
		[created_date],
		[last_modified],
		[videowall_id],
		[monitor_state]
	FROM [VideoWallMonitor]
	WHERE [pkid] = @pkid
END
GO