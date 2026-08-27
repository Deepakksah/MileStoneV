-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

ALTER TABLE RecorderPluginDevice 
ALTER COLUMN [Settings] XML NOT NULL

ALTER TABLE RecorderPluginDevice 
ALTER COLUMN [UISettings] XML NOT NULL

GO
