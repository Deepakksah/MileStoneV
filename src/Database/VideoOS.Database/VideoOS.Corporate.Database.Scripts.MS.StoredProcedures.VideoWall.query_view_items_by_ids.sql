-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_view_items_by_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_view_items_by_ids]
GO

CREATE PROCEDURE [query_view_items_by_ids]
	@viewItemIds tvp_StringTableGuidType READONLY
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		vi.[pkid],
		vi.[position],
		vi.[definition_xml],
		vi.[view_id],
		[view_group_id] = 
		CASE
			WHEN sgm.[IDServerGroup] IS NULL THEN ''
			ELSE sgm.[IDServerGroup]
		END,
		[monitor_id] = 
		CASE
			WHEN vwm.[pkid] IS NULL THEN ''
			ELSE vwm.[pkid]
		END,
		[video_wall_id] = 
		CASE
			WHEN vwm.[videowall_id] IS NULL THEN ''
			ELSE vwm.[videowall_id]
		END
	FROM [VideoWallViewItem] vi
	INNER JOIN [VideoWallView] v ON vi.[view_id] = v.[pkid]
	LEFT JOIN [ServerGroupMembership] sgm ON v.[pkid] = sgm.[IDObject]
	LEFT JOIN [VideoWallMonitor] vwm ON vwm.[view_id] = v.[pkid]
	INNER JOIN @viewItemIds ids ON vi.[pkid] = ids.[ID]
	ORDER BY [position] ASC
END
GO