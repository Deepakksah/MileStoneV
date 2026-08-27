-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Search].[query_search_template_by_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [Search].[query_search_template_by_id]
GO

CREATE PROCEDURE [Search].[query_search_template_by_id]
	@id uniqueidentifier,
	@owner [SIDType]
AS
BEGIN
	SET NOCOUNT ON

	SELECT [Id], [Name], [Description], [Availability], [IncludesScopeItems], [SearchQuery]
	FROM [Search].[SearchTemplates]
	WHERE [Id] = @id AND (([Availability] = 'Private' AND [Owner] = @owner) OR ([Availability] != 'Private'))
END
GO