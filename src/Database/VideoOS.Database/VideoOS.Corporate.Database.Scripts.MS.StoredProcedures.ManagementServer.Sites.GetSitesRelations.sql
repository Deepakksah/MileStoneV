-- This header is used by the dbupdate7 tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[GetSitesRelations]') AND type in (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [GetSitesRelations]
GO

CREATE FUNCTION [GetSitesRelations] (@IDSite GuidType)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	 DECLARE @Ids NVARCHAR(MAX)
	;WITH CTERelations AS
	(
		SELECT sr.IDSiteMaster AS ID
		FROM SitesRelation sr
		INNER JOIN Sites s ON s.IDSite = sr.IDSiteMaster
		WHERE sr.IDSiteSlave = @IDSite
		AND s.IDSiteType = 3

		UNION ALL
		
		SELECT sr.IDSiteMaster AS ID
		FROM SitesRelation sr
		INNER JOIN CTERelations cte ON cte.ID = sr.IDSiteSlave
		INNER JOIN Sites s ON s.IDSite = sr.IDSiteMaster
		WHERE s.IDSiteType = 3
	)
	SELECT @Ids = 
	STUFF(( 
		SELECT '; ' + ID 
		FROM CTERelations
		FOR XML PATH('')), 1,1, '')
	RETURN @Ids
END
GO
