---- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

UPDATE [ServerGroup]
SET [Name] = '4BF8A8B0-54AB-4E82-994F-98D8E9584176'
WHERE [IDServerGroup] = '4BF8A8B0-54AB-4E82-994F-98D8E9584176'
GO