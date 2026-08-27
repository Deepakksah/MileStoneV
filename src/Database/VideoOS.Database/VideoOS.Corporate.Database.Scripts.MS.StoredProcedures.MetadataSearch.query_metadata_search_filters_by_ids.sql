-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_metadata_search_filters_by_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_metadata_search_filters_by_ids]
GO

CREATE PROCEDURE [query_metadata_search_filters_by_ids]
	@Ids tvp_StringTableGuidType READONLY
AS
BEGIN
	SET NOCOUNT ON

	SELECT Filters.[Id]
		,Filters.[Visibility]
	FROM [dbo].[MetadataSearchFilters] Filters
	INNER JOIN @Ids Ids
	ON Filters.Id = Ids.ID
END
GO