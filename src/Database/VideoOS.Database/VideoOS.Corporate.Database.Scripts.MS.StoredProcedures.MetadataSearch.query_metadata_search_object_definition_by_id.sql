-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_metadata_search_object_definition_by_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_metadata_search_object_definition_by_id]
GO

CREATE PROCEDURE [query_metadata_search_object_definition_by_id]
	@Id [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	SELECT [Id],
		[ObjectRoot]
	FROM [dbo].[MetadataSearchObjectDefinitions]
	WHERE @Id = [Id]
END
GO