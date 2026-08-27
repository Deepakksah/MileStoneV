-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_metadata_search_paths_by_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_metadata_search_paths_by_ids]
GO

CREATE PROCEDURE [query_metadata_search_paths_by_ids]
	@Ids tvp_StringTableGuidType READONLY
AS
BEGIN
	SET NOCOUNT ON
	SELECT SearchPaths.[Id]
		,SearchPaths.[Path]
		,SearchPaths.[PropertyId]
	FROM [dbo].[MetadataSearchPaths] SearchPaths
	INNER JOIN @Ids Ids
	ON SearchPaths.Id = Ids.ID
END
GO