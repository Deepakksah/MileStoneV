-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_server_groups]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_server_groups]
GO

CREATE PROCEDURE [query_server_groups]
	@StructureType VARCHAR(64)
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
		FROM [ServerGroup] WHERE [StructureType] = @StructureType
END
GO