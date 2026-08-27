-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_query_master_site]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_query_master_site]
GO

CREATE PROCEDURE [admin_query_master_site]
AS
BEGIN
	SET NOCOUNT ON
	
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	
	DECLARE @masterSiteID GuidType
	SET @masterSiteID = (SELECT [IDSite] FROM [Sites] WHERE [IDSiteType] = 2) -- Only one master site
	
	-- Does the master site exist?
	IF NOT EXISTS(SELECT * FROM [Sites] WHERE [IDSiteType] = 2)
	BEGIN
		RETURN 60400
	END

	SELECT
		s.[IDSite],
		s.[IDSiteType],
		s.[Name],
		s.[Description],
		s.[IDSiteState],
		s.[LastModified],
		s.[LastStatusHandshake],
		s.[Addresses],
		s.[MarkInconsistent],
		@masterSiteID as [IDSiteMaster],
		s.[Version],
		s.[ServiceAccount],
		s.[SynchronizationStatus],
		NULL AS SlaveSiteRelations
	FROM [Sites] s
	WHERE [IDSiteType] = 2
END
GO
