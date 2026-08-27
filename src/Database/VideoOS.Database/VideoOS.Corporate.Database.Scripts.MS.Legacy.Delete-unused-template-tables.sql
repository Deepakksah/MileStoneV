-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================



IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[DeviceLinkTemplates]') AND type = 'U')
DROP TABLE [DeviceLinkTemplates]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[DeviceTemplates]') AND type = 'U')
DROP TABLE [DeviceTemplates]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HardwareTemplates]') AND type = 'U')
DROP TABLE [HardwareTemplates]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RecorderPluginDeviceTemplates]') AND type = 'U')
DROP TABLE [RecorderPluginDeviceTemplates]
GO
