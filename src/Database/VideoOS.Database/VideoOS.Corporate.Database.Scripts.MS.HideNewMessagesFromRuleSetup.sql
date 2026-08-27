-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

UPDATE [EventTypes]	SET [GeneratorType] = 'System' WHERE IDEventType = '06C5010D-11F2-4D70-BD79-4CEC3A20D589'
UPDATE [EventTypes]	SET [GeneratorType] = 'System' WHERE IDEventType = '629405da-f906-44d6-a3d1-57d3de1d1eb8'