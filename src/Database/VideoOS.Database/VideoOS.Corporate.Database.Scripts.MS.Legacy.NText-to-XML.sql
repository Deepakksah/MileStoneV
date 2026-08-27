-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Change SQL type of several columns to Xml from NText. 

-- Devices
ALTER TABLE Devices
ALTER COLUMN Settings XML NULL
GO

ALTER TABLE Devices
ALTER COLUMN Presets XML NULL
GO

ALTER TABLE Devices
ALTER COLUMN DbSettings XML NULL
GO

ALTER TABLE Devices
ALTER COLUMN MulticastSettings XML NOT NULL
GO

-- DeviceTemplates
ALTER TABLE DeviceTemplates
ALTER COLUMN Settings XML NULL
GO

ALTER TABLE DeviceTemplates
ALTER COLUMN Presets XML NULL
GO

ALTER TABLE DeviceTemplates
ALTER COLUMN DbSettings XML NULL
GO

ALTER TABLE DeviceTemplates
ALTER COLUMN MulticastSettings XML NULL
GO

-- Hardware
ALTER TABLE Hardware
ALTER COLUMN Definition XML NULL
GO

ALTER TABLE Hardware
ALTER COLUMN Settings XML NULL
GO

ALTER TABLE Hardware
ALTER COLUMN EventSettings XML NULL
GO

ALTER TABLE Hardware
ALTER COLUMN CustomSettings XML NULL
GO

-- HardwareTemplate
ALTER TABLE HardwareTemplates
ALTER COLUMN Definition XML NULL
GO

ALTER TABLE HardwareTemplates
ALTER COLUMN Settings XML NULL
GO

ALTER TABLE HardwareTemplates
ALTER COLUMN EventSettings XML NULL
GO

ALTER TABLE HardwareTemplates
ALTER COLUMN CustomSettings XML NULL
GO