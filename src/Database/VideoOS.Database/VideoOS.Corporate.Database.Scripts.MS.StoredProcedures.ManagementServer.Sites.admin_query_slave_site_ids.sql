-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_query_slave_site_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_query_slave_site_ids]
GO

CREATE PROCEDURE [admin_query_slave_site_ids]
	@IDSite GuidType
AS
BEGIN
	SET NOCOUNT ON

	SELECT  s.[IDSite]
	FROM Sites s
	WHERE IDSite in
	( SELECT IDSiteSlave FROM SitesRelation WHERE IDSiteMaster = @IDSite)
END
GO