-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @namespace_id UNIQUEIDENTIFIER

--	INPUT
SET @namespace_id = '3ED79EF0-EE2F-40B8-A49A-17E9380C5932'
UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEINPUTVISIBLE'
WHERE namespace_id = @namespace_id
AND name = 'GENERIC_READ'

--	OUTPUT
SET @namespace_id = 'A95FCE90-753D-4CB7-86FB-F8CAD9F93B67'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEOUTPUTVISIBLE'
WHERE namespace_id = @namespace_id
AND name = 'GENERIC_READ'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEACTIVEOUTPUT'
WHERE namespace_id = @namespace_id
AND name = 'ACTIVATE'
GO
