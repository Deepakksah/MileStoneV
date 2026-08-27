-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HardwareTemplates]') AND type in (N'U'))
CREATE TABLE [HardwareTemplates]
(
	[LastModified] DATETIME NOT NULL ,
	[IDHardwareTemplate] [GuidType] ,
	[IDHardwareDriver] [GuidType] ,
	[Name] [NameType] ,
	[Description] [DescriptionType] ,
	[Definition] NVARCHAR(MAX) ,
	[Settings] NVARCHAR(MAX) ,
	[EventSettings] NVARCHAR(MAX) ,
	[CustomSettings] NVARCHAR(MAX) ,
	[VendorProductId] [NameType]

	CONSTRAINT [PK_HardwareTemplate] PRIMARY KEY CLUSTERED 
	(
		[IDHardwareTemplate]
	)
) ON [PRIMARY]
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[DeviceTemplates]') AND type in (N'U'))
CREATE TABLE [DeviceTemplates]
(
	[LastModified] DATETIME NOT NULL ,
	[IDDeviceTemplate] [GuidType] ,
	[IDHardwareTemplate] [GuidType] ,
	[DeviceType][InternalIdType] ,
	[Settings] NVARCHAR(MAX) ,
	[Presets] NVARCHAR(MAX) ,
	[DbSettings] NVARCHAR(MAX) ,
	[MulticastSettings] NVARCHAR(MAX) ,
	[Streams] NVARCHAR(MAX)

	CONSTRAINT [PK_DeviceTemplate] PRIMARY KEY CLUSTERED 
	(
		[IDDeviceTemplate]
	)
) ON [PRIMARY]
GO
