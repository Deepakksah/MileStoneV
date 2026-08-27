-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Update namespace display name 
UPDATE [SecurityAction]
SET [display_name] = 'Manage LPR'
WHERE [name] = 'GENERIC_WRITE'
AND [namespace_id] = '6F714690-E09F-4D6A-A0CA-9F913CA125E8'

UPDATE [SecurityAction]
SET [display_name] = 'View the LPR node in Management Client'
WHERE [name] = 'GENERIC_READ'
AND [namespace_id] = '6F714690-E09F-4D6A-A0CA-9F913CA125E8'

UPDATE [SecurityAction]
SET [display_name] = 'View the LPR tab in client applications'
WHERE [name] = 'LPR_READ'
AND [namespace_id] = '6F714690-E09F-4D6A-A0CA-9F913CA125E8'

GO