-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_server_groups_by_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_server_groups_by_ids]
GO

CREATE PROCEDURE [query_server_groups_by_ids]
	@serverGroupIds tvp_StringTableGuidType READONLY
AS
BEGIN
	SET NOCOUNT ON

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
	INNER JOIN @serverGroupIds ids ON sg.[IDServerGroup] = ids.[ID]
END
GO