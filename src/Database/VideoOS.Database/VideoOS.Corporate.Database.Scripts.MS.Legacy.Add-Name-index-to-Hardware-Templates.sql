-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[HardwareTemplates]') AND name = N'IX_HardwareTemplateName')
CREATE UNIQUE NONCLUSTERED INDEX [IX_HardwareTemplateName] ON [HardwareTemplates] 
(
	[Name] ASC
) ON [PRIMARY]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[DeviceTemplates]') AND type in (N'U'))
DROP TABLE [DeviceTemplates]
GO

CREATE TABLE [DeviceTemplates](
	[LastModified] DATETIME NOT NULL,
	[IDDeviceTemplate] [GuidType] NOT NULL,
	[IDHardwareTemplate] [GuidType] NOT NULL,
	[DeviceType] [InternalIdType] NOT NULL,
	[Settings] XML NULL,
	[Presets] XML NULL,
	[DbSettings] XML NULL,
	[MulticastSettings] XML NULL,
	[Streams] XML NOT NULL,
 CONSTRAINT [PK_DeviceTemplate] PRIMARY KEY CLUSTERED 
(
	[IDHardwareTemplate],
	[IDDeviceTemplate]
)
) ON [PRIMARY]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[DeviceLinkTemplates]') AND type in (N'U'))
DROP TABLE [DeviceLinkTemplates]
GO

CREATE TABLE [DeviceLinkTemplates](
	[LastModified] DATETIME NOT NULL,
	[IDHardwareTemplate] [GuidType] NOT NULL,
	[IDDeviceTemplate] [GuidType] NOT NULL,
	[IDDeviceLinkedTemplate] [GuidType] NOT NULL,
	[Priority] INT NULL,
	CONSTRAINT [PK_DeviceLinkTemplate] PRIMARY KEY CLUSTERED 
	(
		[IDHardwareTemplate],
		[IDDeviceTemplate],
		[IDDeviceLinkedTemplate]
	)
) ON [PRIMARY]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RecorderPluginDeviceTemplates]') AND type in (N'U'))
DROP TABLE [RecorderPluginDeviceTemplates]
GO

CREATE TABLE [RecorderPluginDeviceTemplates](
	[LastModified] DATETIME NOT NULL,
	[IDHardwareTemplate] [GuidType] NOT NULL,
	[IDDeviceTemplate] [GuidType] NOT NULL,
	[ProcessOrder] INT NULL,
	[IDRecorderPlugin] [GuidType] NOT NULL,
	[Settings] XML NULL,
	[UISettings] XML NULL,
	CONSTRAINT [PK_RecorderPluginDeviceTemplate] PRIMARY KEY CLUSTERED 
	(
		[IDHardwareTemplate],
		[IDDeviceTemplate],
		[IDRecorderPlugin]
	)
) ON [PRIMARY]
GO
