-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_server_group_hierarchy_by_parent_group_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_server_group_hierarchy_by_parent_group_id]
GO

CREATE PROCEDURE [query_server_group_hierarchy_by_parent_group_id]
	@IDParentGroup [GuidType],
	@StructureType VARCHAR(128)
AS
BEGIN
	;WITH CTEServerGroupHierarchy AS
   (
		SELECT @IDParentGroup AS ID, 1 as [Level]
		UNION ALL
                         
		SELECT sg.[IDServerGroup]  AS ID, cte.[Level] + 1 As Level
		FROM [ServerGroup] sg
		INNER JOIN CTEServerGroupHierarchy cte ON cte.ID = sg.[IDParentGroup]
		WHERE cte.[Level] + 1 <= 12
	)
	SELECT
		[IDServerGroup],
		[Name],
		[Description],
		[IDParentGroup],
		[CreatedDate],
		[LastModified],
		[StructureType],
		[RelationIds]
	FROM [ServerGroup] sg
	INNER JOIN CTEServerGroupHierarchy cte ON cte.ID = sg.[IDServerGroup]
	WHERE [StructureType] = @StructureType
END
GO
