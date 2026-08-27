-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

UPDATE [SecurityAction]
SET [visible] = 1
WHERE [namespace_id] = 'E9914E39-A67A-496C-A3C1-5B4AAAB28F15' AND [name] = 'EXEC_CONFIG_REPORT'
GO