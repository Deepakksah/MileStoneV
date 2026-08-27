-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_server_group_kind_by_parent_group_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_server_group_kind_by_parent_group_id]
GO

CREATE PROCEDURE [query_server_group_kind_by_parent_group_id]
	@IDParentServerGroup [GuidType],
	@IDServerGroup [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		SG.[IDServerGroup],
		SG.[Name],
		SG.[Description],
		SG.[IDParentGroup],
		SG.[CreatedDate],
		SG.[LastModified],
		SG.[StructureType],
		SGK.[Kind],
		SGK.[PluginId],
		SGK.[CustomData],
		SG.[RelationIds]
	FROM [ServerGroup] SG
	INNER JOIN [ServerGroupKind] SGK on SGK.[IDServerGroup] = SG.[IDServerGroup]
	WHERE [IDParentGroup] = @IDParentServerGroup
	AND SG.[IDServerGroup] = @IDServerGroup
END
GO