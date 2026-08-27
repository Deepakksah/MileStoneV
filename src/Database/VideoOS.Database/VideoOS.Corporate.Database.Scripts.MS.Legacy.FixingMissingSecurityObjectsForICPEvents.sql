-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- The SUPPORTS_TRANSACTION property specifies if the scripts supports
-- running in multi-statement transaction or if it should be running
-- using implicit transactions
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

INSERT SecurityObjects(IDSecurityObject, IDObject, IDClass)
SELECT NEWID(), et.IDEventType, 'USER_DEFINED_EVENT'
FROM EventTypes et
LEFT JOIN SecurityObjects so ON so.IDObject = et.IDEventType
WHERE et.GeneratorSubType = 'ICP' AND so.IDObject IS NULL
GO
