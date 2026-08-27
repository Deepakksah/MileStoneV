-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @CurrentUtcTime DATETIME
SET @CurrentUtcTime = GETUTCDATE()

UPDATE ClientProfile
SET Settings = CAST(REPLACE(CAST(Settings as nvarchar(max)), '<Value>DES56</Value>', '<Value>AES256</Value>') AS XML),
	LastModified = @CurrentUtcTime
WHERE profile_type = 'SmartClientProfile';

UPDATE ClientProfile
SET Settings = CAST(REPLACE(CAST(Settings as nvarchar(max)), '<Value>AES128</Value>', '<Value>AES256</Value>') AS XML),
	LastModified = @CurrentUtcTime
WHERE profile_type = 'SmartClientProfile';

UPDATE ClientProfile
SET Settings = CAST(REPLACE(CAST(Settings as nvarchar(max)), '<Value>AES192</Value>', '<Value>AES256</Value>') AS XML),
	LastModified = @CurrentUtcTime
WHERE profile_type = 'SmartClientProfile';
GO