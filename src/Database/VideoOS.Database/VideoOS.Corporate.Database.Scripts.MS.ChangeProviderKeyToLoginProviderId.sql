-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================


IF EXISTS(SELECT 1 FROM sys.columns WHERE name = N'ProviderKey'AND [object_id] = OBJECT_ID(N'SecurityClaim'))
BEGIN
    EXEC sp_RENAME 'SecurityClaim.ProviderKey' , 'LoginProviderId', 'COLUMN'
END
GO

