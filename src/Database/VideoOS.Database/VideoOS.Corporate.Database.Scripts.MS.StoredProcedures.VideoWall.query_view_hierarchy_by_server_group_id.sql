-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_view_hierarchy_by_server_group_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_view_hierarchy_by_server_group_id]
GO

CREATE PROCEDURE [query_view_hierarchy_by_server_group_id]
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
		v.[pkid],
		v.[name],
		v.[description],
		v.[created_date],
		v.[last_modified],
		v.[definition_xml],
		sgm.IDServerGroup as [view_group_id],
		sg.[RelationIds] as [view_group_relation_ids],
		'' as monitor_id,
		'' as video_wall_id
	FROM [VideoWallView] v
	INNER JOIN [ServerGroupMembership] sgm ON v.[pkid] = sgm.[IDObject]
	INNER JOIN [ServerGroup] sg ON sgm.[IDServerGroup] = sg.[IDServerGroup]
	INNER JOIN CTEServerGroupHierarchy cte ON cte.ID = sg.[IDServerGroup]
END
GO