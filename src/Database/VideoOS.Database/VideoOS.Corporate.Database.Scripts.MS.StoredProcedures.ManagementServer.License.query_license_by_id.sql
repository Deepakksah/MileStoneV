-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[query_license_by_id]') AND [type] IN (N'P', N'PC'))
DROP PROCEDURE [query_license_by_id]
GO

CREATE PROCEDURE [query_license_by_id]
	@IDLicense UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON

	SELECT  l.[Data], l.[LastModified]
	FROM License l
	WHERE l.[IDLicense] = @IDLicense
END
GO