-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_view_item_hierarchy_by_server_group_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_view_item_hierarchy_by_server_group_id]
GO

CREATE PROCEDURE [query_view_item_hierarchy_by_server_group_id]
	@serverGroupId [GuidType]
AS
BEGIN
	SET NOCOUNT ON

   ;WITH CTEServerGroupHierarchy AS
   (
		SELECT @serverGroupId AS ID, 1 as [Level]
		UNION ALL
						 
		SELECT sg.[IDServerGroup]  AS ID, cte.[Level] + 1 As Level
		FROM [ServerGroup] sg
		INNER JOIN CTEServerGroupHierarchy cte ON cte.ID = sg.[IDParentGroup]
		WHERE cte.[Level] + 1 <= 12
	)
	SELECT
		vi.[pkid],
		vi.[position],
		vi.[definition_xml],
		vi.[view_id],
		sgm.[IDServerGroup] AS [view_group_id], 
		[monitor_id] = '',
		[video_wall_id] = ''
	FROM [VideoWallViewItem] AS vi
	INNER JOIN [VideoWallView] AS v ON vi.[view_id] = v.[pkid]
	INNER JOIN [ServerGroupMembership] AS sgm ON vi.[view_id] = sgm.[IDObject]
	INNER JOIN CTEServerGroupHierarchy AS cte ON sgm.[IDServerGroup] = cte.ID
	ORDER BY vi.[view_id], vi.[position]
END
GO