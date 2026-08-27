-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Remove 'Edit' action from Site
DELETE [SecurityAction]
WHERE [namespace_id] = '875F4A4E-4D78-4A8D-A978-290227253475'
	AND [name] IN ('GENERIC_WRITE')
GO
