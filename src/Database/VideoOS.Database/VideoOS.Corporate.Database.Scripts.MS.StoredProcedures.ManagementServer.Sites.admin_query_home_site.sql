-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_query_home_site]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_query_home_site]
GO

CREATE PROCEDURE [admin_query_home_site]
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @homeSiteID GuidType
	SET @homeSiteID = (SELECT [IDSite] FROM [Sites] WHERE [IDSiteType] = 1) -- Only one home site
	
	EXEC admin_query_site @IDSite = @homeSiteID
END
GO
