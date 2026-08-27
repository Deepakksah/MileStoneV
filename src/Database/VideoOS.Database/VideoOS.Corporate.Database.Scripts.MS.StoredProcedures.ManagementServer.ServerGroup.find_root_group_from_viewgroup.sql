-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[find_root_group_from_viewgroup]') AND type in (N'P', N'PC'))
DROP PROCEDURE [find_root_group_from_viewgroup]
GO

CREATE PROCEDURE [find_root_group_from_viewgroup]
	@servergroup_id [GuidType]
 AS
BEGIN
	SET NOCOUNT ON

	DECLARE @StuctureType VARCHAR(64)
	
	SELECT @StuctureType = [StructureType]
	FROM [ServerGroup] 
	WHERE IDServerGroup = @servergroup_id

	IF(@StuctureType <> 'VIEW_GROUP')
	BEGIN
		RETURN
	END
	
	DECLARE @RootServerGroup [GuidType]
	SET @RootServerGroup = '430C2FE2-B233-42DA-B508-1A22EF03383D'

	DECLARE @PrivateGroup [GuidType]
	SET @PrivateGroup = '4BF8A8B0-54AB-4E82-994F-98D8E9584176'

	DECLARE @temp [GuidType]

	;WITH ViewGroupCTE(IDServerGroup, IDParentGroup, [Path], RootGroupId, Level) AS
	(
		SELECT IDServerGroup, IDParentGroup, [Path], IDServerGroup AS RootGroupId, 1 AS [Level]
		FROM [ServerGroup]
		WHERE IDServerGroup = @servergroup_id

		UNION ALL

		SELECT sg.IDServerGroup, sg.IDParentGroup, sg.[Path], cte.RootGroupId, cte.[Level] + 1
		FROM [ServerGroup] sg
		INNER JOIN ViewGroupCTE cte ON sg.IDServerGroup = cte.IDParentGroup
	)
	SELECT IDServerGroup
	FROM ViewGroupCTE
	WHERE [IDServerGroup] <> @RootServerGroup -- We do not want the root server group
		AND IDServerGroup <> @PrivateGroup   -- We do not want the private fixed server group

		-- We want the child group of either the root or private fixed groups
		AND (IDParentGroup = @RootServerGroup OR IDParentGroup = @PrivateGroup)
END
GO
