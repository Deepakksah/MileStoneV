-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_NAME = 'Devices' AND CONSTRAINT_NAME = 'UQ_DeviceHwid')
BEGIN
  ALTER TABLE [Devices] DROP CONSTRAINT [UQ_DeviceHwid]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE OBJECT_ID = OBJECT_ID(N'[Devices]') AND [name] = N'IX_Devices_Hwid')
BEGIN
  CREATE UNIQUE NONCLUSTERED INDEX [IX_Devices_Hwid] ON [Devices]
  (
    [IDHardware] ASC,
    [DeviceType] ASC,
    [DeviceIndex] ASC
  )
  WHERE ([DeviceIndex] >= 0)
END
GO