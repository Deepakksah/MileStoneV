-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Removed Manage security and Create View group in Expert mode
UPDATE [SecurityAction]
SET Visible = 0
WHERE (name = 'ADMIN_SECURITY' OR name = 'CREATE_VIEWGROUP') AND namespace_id = 'EC543C56-ADF7-4600-B9AD-97E166552BE9'

GO