-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
DELETE FROM [RegisteredServers]
WHERE [Type] = '517AFD85-CDEE-4d50-A9EA-991C0801DA21'
GO

DELETE FROM [DataCollectorInstalledServices]
WHERE [service_name] = 'ServiceChannel'
GO

DELETE FROM [ServerType] 
WHERE [name] = 'ServiceChannel'
GO