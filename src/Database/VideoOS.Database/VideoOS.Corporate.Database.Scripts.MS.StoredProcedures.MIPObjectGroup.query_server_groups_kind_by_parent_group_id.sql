-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_server_groups_kind_by_parent_group_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_server_groups_kind_by_parent_group_id]
GO

CREATE PROCEDURE [query_server_groups_kind_by_parent_group_id]
	@IDParentGroup [GuidType],
	@StructureType NVARCHAR(128)
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

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
		SG.RelationIds
	FROM [ServerGroup] SG
	INNER JOIN [ServerGroupKind] SGK on SGK.[IDServerGroup] = SG.[IDServerGroup]
	WHERE	[IDParentGroup] = @IDParentGroup
		AND [StructureType] = @StructureType
END
GO