-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_views_by_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_views_by_ids]
GO

CREATE PROCEDURE [query_views_by_ids]
	@viewIds tvp_StringTableGuidType READONLY
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		v.[pkid],
		v.[name],
		v.[description],
		v.[created_date],
		v.[last_modified],
		v.[definition_xml],
		[view_group_id] = 
		CASE
			WHEN sgm.[IDServerGroup] IS NULL THEN ''
			ELSE sgm.[IDServerGroup]
		END,
		COALESCE( sg.[RelationIds], sgm.IDServerGroup) AS [view_group_relation_ids],
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
	FROM [VideoWallView] v
	LEFT JOIN [ServerGroupMembership] sgm ON v.[pkid] = sgm.[IDObject]
	LEFT JOIN [ServerGroup] sg ON sg.IDServerGroup = sgm.IDServerGroup
	LEFT JOIN [VideoWallMonitor] vwm ON vwm.[view_id] = v.[pkid]
	INNER JOIN @viewIds ids ON v.[pkid] = ids.[ID]
END
GO