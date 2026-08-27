-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_views]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_views]
GO

CREATE PROCEDURE [query_views]
	@parentId [GuidType]
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
		@parentId as [view_group_id],
		COALESCE( sg.[RelationIds], sgm.IDServerGroup) AS [view_group_relation_ids],
		'' as monitor_id,
		'' as video_wall_id
	FROM [VideoWallView] v
	INNER JOIN [ServerGroupMembership] sgm ON v.[pkid] = sgm.[IDObject]
	LEFT JOIN [ServerGroup] sg ON sg.IDServerGroup = sgm.IDServerGroup
	WHERE sgm.[IDServerGroup] = @parentId
END
GO