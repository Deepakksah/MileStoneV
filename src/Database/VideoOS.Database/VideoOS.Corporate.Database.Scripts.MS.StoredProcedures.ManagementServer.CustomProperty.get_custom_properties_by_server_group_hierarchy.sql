-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[get_custom_properties_by_server_group_hierarchy]') AND type in (N'P', N'PC'))
DROP PROCEDURE [get_custom_properties_by_server_group_hierarchy]
GO

CREATE PROCEDURE [get_custom_properties_by_server_group_hierarchy]
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
		cp.[name],
		cp.[value],
		cp.[object_id]
	FROM [CustomProperty] cp
	INNER JOIN CTEServerGroupHierarchy cte ON cte.ID = cp.[object_id]
END
GO