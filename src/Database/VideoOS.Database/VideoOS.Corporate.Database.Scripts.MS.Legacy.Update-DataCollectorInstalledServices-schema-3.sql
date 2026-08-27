-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

UPDATE [ServerType]
SET upgrade_code = '{D5CE8192-FCE4-4DC9-8473-66320C318700}'
WHERE [name] = 'ManagementServer'
GO
