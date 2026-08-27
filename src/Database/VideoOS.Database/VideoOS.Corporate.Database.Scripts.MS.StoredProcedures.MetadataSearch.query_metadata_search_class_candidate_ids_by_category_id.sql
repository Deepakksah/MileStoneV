-- This header is used by the dbupdate tool.
-- The TARGET poperty is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_metadata_search_class_candidate_ids_by_category_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_metadata_search_class_candidate_ids_by_category_id]
GO

CREATE PROCEDURE [query_metadata_search_class_candidate_ids_by_category_id]
	@CategoryId [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	SELECT	[Id]
	FROM [dbo].[MetadataSearchClassCandidates]
	WHERE [CategoryId] = @CategoryId
END
GO