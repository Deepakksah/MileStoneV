-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_server_group_structure_type]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_server_group_structure_type]
GO

CREATE PROCEDURE [query_server_group_structure_type]
	@IDServerGroup [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	BEGIN
		SELECT
			[StructureType]
		FROM [ServerGroup]
		WHERE [IDServerGroup] = @IDServerGroup
	END
END
GO
