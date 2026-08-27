-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Rename 'IX_Device_Hardware_Enabled' to 'IX_Devices_IDHardware_Enabled' to follow general index naming
IF EXISTS(SELECT 1 FROM sys.indexes WHERE [object_id] = OBJECT_ID(N'Devices') AND [name] = N'IX_Device_Hardware_Enabled')
	EXEC sp_rename N'dbo.Devices.IX_Device_Hardware_Enabled', N'IX_Devices_IDHardware_Enabled', N'INDEX'
GO

-- Remove index that is a duplicate of 'IX_Devices_IDHardware_Enabled' above
IF EXISTS(SELECT 1 FROM sys.indexes WHERE [object_id] = OBJECT_ID(N'Devices') AND [name] = N'IX_Devices_IDHardware')
	DROP INDEX [IX_Devices_IDHardware] ON [dbo].[Devices]
GO
