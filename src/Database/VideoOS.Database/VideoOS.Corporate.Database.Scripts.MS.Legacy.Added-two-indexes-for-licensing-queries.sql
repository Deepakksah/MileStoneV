-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE [object_id] = OBJECT_ID(N'Devices') AND name = N'IX_Device_Hardware_Enabled')
CREATE NONCLUSTERED INDEX [IX_Device_Hardware_Enabled]
ON [Devices] ([IDHardware], [Enabled])
INCLUDE ([IDDevice], [DeviceType], [DeviceIndex])
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE [object_id] = OBJECT_ID(N'Hardware') AND name = N'IX_Hardware_Enabled')
CREATE NONCLUSTERED INDEX [IX_Hardware_Enabled]
ON [Hardware] ([IDRecorder], [Enabled])
GO
