-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_query_all_slave_sites]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_query_all_slave_sites]
GO

CREATE PROCEDURE [admin_query_all_slave_sites]
AS
BEGIN
	SET NOCOUNT ON

	SELECT  s.[IDSite],
	  	s.[IDSiteType],
		s.[Name],
		s.[Description],
		s.[IDSiteState],
		s.[LastModified],
		s.[LastStatusHandshake],
		s.[Addresses],
		s.[MarkInconsistent],
		sr.[IDSiteMaster],
		s.[Version],
		s.[ServiceAccount],
		s.[SynchronizationStatus],
		dbo.GetSitesRelations(s.[IDSite]) AS SlaveSiteRelations
	FROM Sites s
	LEFT JOIN SitesRelation sr ON s.IDSite = sr.IDSiteSlave
	WHERE IDSiteType = 3
END
GO