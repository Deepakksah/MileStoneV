-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_query_license_modified]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_query_license_modified]
GO

CREATE PROCEDURE [admin_query_license_modified]
AS
BEGIN
	SET NOCOUNT ON

	SELECT  l.[LastModified]
	FROM License l
	WHERE l.[IDLicense] = '94F58776-84BD-44DB-B19B-851F1D53A832'
END
GO