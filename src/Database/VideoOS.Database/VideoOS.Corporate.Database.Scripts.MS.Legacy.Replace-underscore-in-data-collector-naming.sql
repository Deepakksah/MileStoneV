-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Replace '_' underscore in the DataCollector table to '$$' so the LIKE statments will not interpret it as a wildcard character

UPDATE [DataCollector]
SET host_name = REPLACE(host_name, '_', '$$')
GO