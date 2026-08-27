-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

UPDATE [Devices]
SET [gispoint] = geography::STPointFromText('POINT EMPTY', 4326)
WHERE [gispoint] IS NULL
GO

IF NOT EXISTS(SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[DF_EmptyPoint]') AND parent_object_id = OBJECT_ID(N'[Devices]'))
ALTER TABLE [Devices]
ADD CONSTRAINT DF_EmptyPoint DEFAULT geography::STPointFromText('POINT EMPTY', 4326) FOR [gispoint]
GO

ALTER TABLE [Devices]
ALTER COLUMN [GisPoint] GEOGRAPHY NOT NULL
GO 