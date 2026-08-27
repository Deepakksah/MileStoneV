-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @LicenseId [GuidType]	=	'E145597A-ECAF-40A1-99E6-14BB9D45A09E';

IF EXISTS (SELECT * FROM [Mip].[Items] WHERE [ItemId] = @LicenseId)
	DELETE FROM [Mip].[Items] WHERE [ItemId] = @LicenseId
GO