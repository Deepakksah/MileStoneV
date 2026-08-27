-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================


-- Add new calculated column for id of the instance
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'PtzManualSessionTimeout' and OBJECT_ID = OBJECT_ID(N'Devices'))
ALTER TABLE [Devices]
ADD [PtzManualSessionTimeout] INT NOT NULL DEFAULT -1
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'PtzReservedSessionTimeout' and OBJECT_ID = OBJECT_ID(N'Devices'))
ALTER TABLE [Devices]
ADD [PtzReservedSessionTimeout] INT NOT NULL DEFAULT -1
GO
