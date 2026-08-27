-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Add new column for  servertype 
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [object_id] = OBJECT_ID(N'ServerType') AND name = N'service_type')
ALTER TABLE [ServerType]
ADD [service_type] [GuidType] NULL
GO

UPDATE [ServerType]
SET [service_type] = '2D275C3A-50BC-4435-B0AD-ED3381FEDC1D'
WHERE [name] = 'ManagementServer'

UPDATE [ServerType]
SET [service_type] = '2B2D3601-5857-44A3-880F-BE7957B5B27E'
WHERE [name] = 'Recorder'

UPDATE [ServerType]
SET [service_type] = '3C944CF2-668F-4956-9EBE-A1C3F6BD6CD4'
WHERE [name] = 'FailoverServer'

UPDATE [ServerType]
SET [service_type] = '002B1B2F-0817-4592-8C62-2FE5107C160C'
WHERE [name] = 'LogService'

UPDATE [ServerType]
SET [service_type] = '517AFD85-CDEE-4d50-A9EA-991C0801DA21'
WHERE [name] = 'ServiceChannel'

UPDATE [ServerType]
SET [service_type] = '08AB8F23-9AEF-4298-9CAA-F39259FE7CDA'
WHERE [name] = 'EventServer'

UPDATE [ServerType]
SET [service_type] = '7510BBAE-DBA5-4C2E-B037-74A0222484C6'
WHERE [name] = 'MobileServer'

UPDATE [ServerType]
SET [service_type] = 'BA177CB7-5A9B-42B8-988D-CFBA91CA44E3'
WHERE [name] = 'LPRServer'

GO
