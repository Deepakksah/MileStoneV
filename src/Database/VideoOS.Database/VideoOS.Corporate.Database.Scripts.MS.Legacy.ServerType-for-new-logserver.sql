-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

UPDATE [ServerType]
SET service_type = '3D6F1153-92AD-43F1-B467-9482FFD291B2'
WHERE service_type = '002B1B2F-0817-4592-8C62-2FE5107C160C'

GO