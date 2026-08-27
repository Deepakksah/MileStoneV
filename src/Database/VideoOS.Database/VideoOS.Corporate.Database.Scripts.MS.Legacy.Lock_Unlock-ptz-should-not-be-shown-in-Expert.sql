-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Lock/Unlock PTZ preset should not be visible in Expert
UPDATE [SecurityAction]
SET [visible] = 0
WHERE [namespace_id] = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85'
AND [name] = 'MANAGE_LOCK_PTZ_PRESET'
GO


