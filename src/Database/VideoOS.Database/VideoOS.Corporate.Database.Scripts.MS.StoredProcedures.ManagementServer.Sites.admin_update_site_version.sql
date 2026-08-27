-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_update_site_version]') AND type = 'P')
DROP PROCEDURE [admin_update_site_version]
GO

CREATE PROCEDURE [admin_update_site_version]
	@IDSite GuidType,
	@Version VARCHAR(15)
AS
BEGIN
	SET NOCOUNT ON
	
	UPDATE [Sites] 
	SET [Version] = @Version
	WHERE IDSite = @IDSite
END
GO
