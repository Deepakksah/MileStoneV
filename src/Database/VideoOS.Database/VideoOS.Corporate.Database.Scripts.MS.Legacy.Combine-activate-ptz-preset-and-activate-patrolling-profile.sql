-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Remove 'Manage Patrolling Profile' security action
DELETE [SecurityAction]
WHERE [namespace_id] = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85' AND [name] = 'MANAGE_PATROLLING_PROFILE'
GO

UPDATE [SecurityAction]
SET [display_name] = 'Manage PTZ preset and patrolling profiles'
WHERE [namespace_id] = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85' AND [name] = 'MANAGE_PTZ_PRESET'
GO

DELETE [SecurityPermissionsDevice]
WHERE [SecurityAttributeID] = 'DEVICEPTZMANAGEPATROLLING'
GO

DELETE [SecurityAttributes]
WHERE [SecurityAttributeID] = 'DEVICEPTZMANAGEPATROLLING'
GO
