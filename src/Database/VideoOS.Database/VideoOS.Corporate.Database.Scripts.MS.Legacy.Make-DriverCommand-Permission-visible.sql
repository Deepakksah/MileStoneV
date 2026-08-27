-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

update [SecurityAction] set [visible] = '1' where [namespace_id] = 'F97772D8-7B6D-4EA4-B6B5-EE4EA010FA2C' and [name] = 'HARDWARECOMMAND'
GO
