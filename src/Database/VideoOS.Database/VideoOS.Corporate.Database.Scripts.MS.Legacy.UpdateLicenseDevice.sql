-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS (
 SELECT * 
 FROM sys.columns 
 WHERE object_id = OBJECT_ID(N'[LicenseDevice]') AND 
  name = N'License')
BEGIN

 DECLARE @deviceLicense_table TABLE (
  [LastModified] DATETIME NOT NULL
  , IDDevice [uniqueidentifier] NOT NULL
  , License BIGINT NOT NULL
 )

 -- populate temp license table
 INSERT @deviceLicense_table
 SELECT * FROM LicenseDevice
 
 DELETE [LicenseDevice]

 ALTER TABLE [LicenseDevice] ALTER COLUMN [License] BIGINT NOT NULL


 INSERT [LicenseDevice]
 SELECT
  [LastModified],
  [IDDevice],
  CASE WHEN [License] = 1 THEN 3155378939999999999 ELSE 0 END AS License
 FROM @deviceLicense_table
END



GO