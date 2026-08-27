SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CUDAPlugin].[Message](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReceiveTime] [datetime2](7) NOT NULL,
	[CUDAInfo] [xml] NOT NULL,
	[SourceId] [xml] NOT NULL,
	[EventId] [uniqueidentifier] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [CUDAPlugin].[RefreshInterval](
	[IntervalSecs] [int] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArchiveStorage](
	[pkid] [dbo].[GuidType] NOT NULL,
	[recordingstorage_id] [dbo].[GuidType] NOT NULL,
	[name] [dbo].[NameType] NOT NULL,
	[description] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[path] [nvarchar](512) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[retain_minutes] [int] NOT NULL,
	[max_size_mb] [bigint] NOT NULL,
	[target_framerate] [float] NOT NULL,
	[schedule] [xml] NOT NULL,
	[created_date] [datetime] NOT NULL,
	[last_modified] [datetime] NOT NULL,
	[folder] [dbo].[GuidType] NOT NULL,
	[framerate_reduction_enabled] [bit] NOT NULL,
	[bank_id] [uniqueidentifier] NOT NULL,
	[mounted] [bit] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AudioMessage](
	[LastModified] [datetime] NOT NULL,
	[ID] [dbo].[GuidType] NOT NULL,
	[Name] [dbo].[NameType] NOT NULL,
	[Description] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[OriginalFileName] [dbo].[LongNameType] NOT NULL,
	[Content] [varbinary](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuthorizationCookie](
	[AuthorizationId] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[SID] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ValidUntil] [datetime] NOT NULL,
	[ActionType] [nvarchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BasicInfo](
	[id] [dbo].[GuidType] NOT NULL,
	[name] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[property] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[order] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClientProfile](
	[IDClientProfile] [dbo].[GuidType] NOT NULL,
	[Name] [dbo].[NameType] NOT NULL,
	[Description] [dbo].[DescriptionType] NOT NULL,
	[LastModified] [datetime] NOT NULL,
	[Priority] [int] NOT NULL,
	[DefaultProfile] [bit] NOT NULL,
	[Settings] [xml] NOT NULL,
	[profile_type] [nvarchar](200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClientProfileRoleRelation](
	[IDClientProfile] [dbo].[GuidType] NOT NULL,
	[IDRole] [dbo].[GuidType] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Compressors](
	[LastModified] [datetime] NOT NULL,
	[IDCodec] [dbo].[GuidType] NOT NULL,
	[Handler] [int] NOT NULL,
	[Flags] [int] NULL,
	[Quality] [int] NULL,
	[KeyFrames] [int] NULL,
	[DataRate] [int] NULL,
	[ConfigData] [varbinary](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomProperty](
	[object_id] [varchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[name] [dbo].[NameType] NOT NULL,
	[value] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomSettings](
	[LastModified] [datetime] NOT NULL,
	[IDCustomSetting] [dbo].[GuidType] NOT NULL,
	[OwnerSID] [dbo].[SIDType] NOT NULL,
	[DataVersion] [int] NOT NULL,
	[SettingDataXml] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[ModifiedUser] [dbo].[UserIdType] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DataCollector](
	[pkid] [int] IDENTITY(1,1) NOT NULL,
	[host_name] [nvarchar](200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[application_id] [uniqueidentifier] NOT NULL,
	[version] [int] NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DataCollectorConfiguration](
	[data_collector_id] [int] NOT NULL,
	[category_name] [nvarchar](120) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[counter_name] [nvarchar](120) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[instance_name] [nvarchar](120) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[counter_direction] [bit] NOT NULL
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[DataCollectorConfiguration] ADD  DEFAULT ((1)) FOR [counter_direction]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DataCollectorInstalledServices](
	[data_collector_id] [int] NOT NULL,
	[service_name] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeviceDataDistribution](
	[device_id] [dbo].[GuidType] NOT NULL,
	[recorder_id] [dbo].[GuidType] NOT NULL,
	[track_id] [uniqueidentifier] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeviceGroupMembers](
	[LastModified] [datetime] NOT NULL,
	[IDDeviceGroup] [dbo].[GuidType] NOT NULL,
	[IDDevice] [dbo].[GuidType] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeviceGroups](
	[LastModified] [datetime] NOT NULL,
	[IDDeviceGroup] [dbo].[GuidType] NOT NULL,
	[IDParentDeviceGroup] [dbo].[GuidType] NULL,
	[DeviceType] [dbo].[InternalIdType] NOT NULL,
	[Name] [dbo].[NameType] NOT NULL,
	[Description] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[BuiltIn] [bit] NOT NULL,
	[AutoInsertNew] [bit] NOT NULL,
	[IsUserControlled] [bit] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeviceLinks](
	[LastModified] [datetime] NOT NULL,
	[IDDevice] [dbo].[GuidType] NOT NULL,
	[IDDeviceLinked] [dbo].[GuidType] NOT NULL,
	[Priority] [int] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeviceMetadataInfo](
	[IDDevice] [dbo].[GuidType] NOT NULL,
	[ItemIndex] [int] NOT NULL,
	[IDReference] [uniqueidentifier] NOT NULL,
	[Name] [dbo].[NameType] NOT NULL,
	[IDDisplayReference] [uniqueidentifier] NOT NULL,
	[DisplayName] [dbo].[NameType] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DevicePrivacyMask](
	[LastModified] [datetime] NOT NULL,
	[IDDevice] [dbo].[GuidType] NOT NULL,
	[PrivacyMask] [xml] NOT NULL,
	[Enabled] [bit] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DevicePtzSettingData](
	[IDDevice] [dbo].[GuidType] NOT NULL,
	[Hwid] [varchar](32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[TypeId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Value] [nvarchar](2048) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Devices](
	[LastModified] [datetime] NOT NULL,
	[IDDevice] [dbo].[GuidType] NOT NULL,
	[IDHardware] [dbo].[GuidType] NOT NULL,
	[Name] [dbo].[NameType] NOT NULL,
	[Description] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Location] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[DeviceType] [dbo].[InternalIdType] NOT NULL,
	[Enabled] [bit] NOT NULL,
	[Settings] [xml] NULL,
	[Presets] [xml] NULL,
	[Shortcut] [dbo].[ShortNameType] NOT NULL,
	[MulticastEnabled] [bit] NOT NULL,
	[Streams] [xml] NULL,
	[DeviceIndex] [int] NOT NULL,
	[SubType] [int] NOT NULL,
	[FailoverLiveOnly] [bit] NOT NULL,
	[DisableOnFailover] [bit] NOT NULL,
	[PtzManualSessionTimeout] [int] NOT NULL,
	[PtzReservedSessionTimeout] [int] NOT NULL,
	[PtzExtendedManualSessionTimeout] [int] NOT NULL,
	[GisPoint] [geography] NULL,
	[ShortName] [nvarchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Icon] [int] NOT NULL,
	[CoverageDirection] [float] NOT NULL,
	[CoverageDepth] [float] NOT NULL,
	[CoverageFieldOfView] [float] NOT NULL,
	[OriginalIdDevice]  AS ([dbo].[GetOriginalDeviceID]([Settings],[IdDevice])) PERSISTED
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[Devices] ADD  DEFAULT ((0)) FOR [DisableOnFailover]
GO
ALTER TABLE [dbo].[Devices] ADD  DEFAULT ((-1)) FOR [PtzManualSessionTimeout]
GO
ALTER TABLE [dbo].[Devices] ADD  DEFAULT ((-1)) FOR [PtzReservedSessionTimeout]
GO
ALTER TABLE [dbo].[Devices] ADD  DEFAULT ((-1)) FOR [PtzExtendedManualSessionTimeout]
GO
ALTER TABLE [dbo].[Devices] ADD  CONSTRAINT [DF_EmptyPoint]  DEFAULT ([geography]::STPointFromText('POINT EMPTY',(4326))) FOR [GisPoint]
GO
ALTER TABLE [dbo].[Devices] ADD  CONSTRAINT [DF_ShortName]  DEFAULT ('') FOR [ShortName]
GO
ALTER TABLE [dbo].[Devices] ADD  CONSTRAINT [DF_Icon]  DEFAULT ((0)) FOR [Icon]
GO
ALTER TABLE [dbo].[Devices] ADD  CONSTRAINT [DF_CoverageDirection]  DEFAULT ((0.0)) FOR [CoverageDirection]
GO
ALTER TABLE [dbo].[Devices] ADD  CONSTRAINT [DF_CoverageDepth]  DEFAULT ((0.0)) FOR [CoverageDepth]
GO
ALTER TABLE [dbo].[Devices] ADD  CONSTRAINT [DF_CoverageFieldOfView]  DEFAULT ((0.0)) FOR [CoverageFieldOfView]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeviceTrack](
	[IDDevice] [dbo].[GuidType] NOT NULL,
	[IDTrack] [uniqueidentifier] NOT NULL,
	[IDTrackDefinition] [uniqueidentifier] NOT NULL,
	[MergeEdge] [bit] NOT NULL,
	[DefaultPlayback] [bit] NOT NULL,
	[InUse] [bit] NOT NULL,
	[OnlyUseMediaAfter] [datetime2](7) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeviceTrackDefinition](
	[IDTrackDefinition] [uniqueidentifier] NOT NULL,
	[IsPrimary] [bit] NOT NULL,
	[IsEdge] [bit] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DriverNumbers](
	[IDDriver] [uniqueidentifier] NOT NULL,
	[DriverNumber] [int] IDENTITY(50001,1) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventTypes](
	[LastModified] [datetime] NOT NULL,
	[IDEventType] [dbo].[GuidType] NOT NULL,
	[Definition] [xml] NOT NULL,
	[GeneratorType] [dbo].[InternalIdType] NOT NULL,
	[IDGenerator] [dbo].[GuidType] NULL,
	[Builtin] [bit] NOT NULL,
	[Name] [dbo].[NameType] NOT NULL,
	[GeneratorSubType] [varchar](36) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExternalPlugins](
	[LastModified] [datetime] NOT NULL,
	[PluginID] [dbo].[GuidType] NOT NULL,
	[PluginType] [dbo].[InternalIdType] NOT NULL,
	[Data] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FailoverHistory](
	[IDFailoverHistoryEntry] [dbo].[GuidType] NOT NULL,
	[IDFailover] [dbo].[GuidType] NOT NULL,
	[Description] [dbo].[DescriptionType] NOT NULL,
	[IDRecorder] [dbo].[GuidType] NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[LastModified] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FailoverServers](
	[LastModified] [datetime] NOT NULL,
	[IDFailoverServer] [dbo].[GuidType] NOT NULL,
	[Name] [dbo].[NameType] NOT NULL,
	[Description] [dbo].[DescriptionType] NOT NULL,
	[HostName] [dbo].[UriType] NULL,
	[UdpPort] [int] NOT NULL,
	[Enabled] [bit] NOT NULL,
	[GroupSequence] [smallint] NOT NULL,
	[DatabaseLocation] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[x64] [bit] NOT NULL,
	[IDRecordingServerService] [dbo].[GuidType] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[FailoverServers] ADD  CONSTRAINT [DF_FailoverServer_x64]  DEFAULT ((0)) FOR [x64]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hardware](
	[LastModified] [datetime] NOT NULL,
	[IDHardware] [dbo].[GuidType] NOT NULL,
	[IDHardwareDriver] [dbo].[GuidType] NOT NULL,
	[Name] [dbo].[NameType] NOT NULL,
	[Description] [dbo].[DescriptionType] NOT NULL,
	[URI] [dbo].[UriTypeShort] NOT NULL,
	[LoginID] [dbo].[UserIdType] NOT NULL,
	[EncryptedPassword] [dbo].[PasswordType] NOT NULL,
	[IDRecorder] [dbo].[GuidType] NOT NULL,
	[Enabled] [bit] NOT NULL,
	[Settings] [xml] NULL,
	[EventSettings] [xml] NULL,
	[VendorProductID] [dbo].[NameType] NULL,
	[C18NDigest] [char](44) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[DriverNumber] [int] NOT NULL,
	[DriverProxyName] [dbo].[NameType] NOT NULL,
	[HostedHardware_id] [int] NULL,
	[CameraData] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[EdgeSettings] [xml] NULL,
	[SubType] [int] NOT NULL,
	[MacAddress]  AS ([dbo].[ExtractMacAddress]([Settings])) PERSISTED,
	[SerialNumber]  AS ([dbo].[ExtractSerialNumber]([Settings])) PERSISTED,
	[DetectedModel]  AS ([dbo].[ExtractDetectedModel]([Settings])) PERSISTED,
	[CredentialsLastModified] [datetime] NULL,
	[HardwareDataLastUpdated] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HardwareBlock](
	[pkid] [int] IDENTITY(1,1) NOT NULL,
	[hardware_id] [dbo].[GuidType] NOT NULL,
	[expires] [datetime] NOT NULL,
	[owner] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[data] [nvarchar](4000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HardwareDefinition](
	[C18NDigest] [char](44) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Definition] [xml] NOT NULL,
	[DriverName]  AS ([dbo].[ExtractDriverName]([Definition])) PERSISTED,
	[DriverVersion]  AS ([dbo].[ExtractDriverVersion]([Definition])) PERSISTED
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HardwareDriverRecorder](
	[LastModified] [datetime] NOT NULL,
	[IDHardwareDriver] [dbo].[GuidType] NOT NULL,
	[IDRecordingServerService] [dbo].[GuidType] NOT NULL,
	[DriverName] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[DriverNumber] [int] NOT NULL,
	[DriverVersion] [nvarchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[XmlVersion] [nvarchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[DllName] [nvarchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[GroupName] [nvarchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HostedHardware](
	[pkid] [int] IDENTITY(1,1) NOT NULL,
	[created_date] [datetime] NOT NULL,
	[last_modified] [datetime] NOT NULL,
	[type] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[name] [dbo].[NameType] NOT NULL,
	[description] [dbo].[DescriptionType] NOT NULL,
	[user_name] [dbo].[UserIdType] NOT NULL,
	[password] [dbo].[PasswordType] NOT NULL,
	[hosting_info] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[manufacturer_id] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[custom_data] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[hosting_server_id] [int] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HostingServer](
	[pkid] [int] IDENTITY(1,1) NOT NULL,
	[created_date] [datetime] NOT NULL,
	[last_modified] [datetime] NOT NULL,
	[type] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[name] [dbo].[NameType] NOT NULL,
	[description] [dbo].[DescriptionType] NOT NULL,
	[user_name] [dbo].[UserIdType] NOT NULL,
	[password] [dbo].[PasswordType] NOT NULL,
	[internal_address] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[external_address] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[License](
	[LastModified] [datetime] NOT NULL,
	[IDLicense] [uniqueidentifier] NOT NULL,
	[Data] [varbinary](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LicenseDevice](
	[LastModified] [datetime] NOT NULL,
	[IDDevice] [uniqueidentifier] NOT NULL,
	[License] [bigint] NOT NULL,
	[UniqueId] [varchar](700) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LicenseHardware](
	[LastModified] [datetime] NOT NULL,
	[IDHardware] [uniqueidentifier] NOT NULL,
	[License] [bigint] NOT NULL,
	[UniqueId] [varchar](700) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LicenseRecorder](
	[LastModified] [datetime] NOT NULL,
	[IDRecorder] [uniqueidentifier] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogAuditVMOQueue](
	[pkid] [bigint] IDENTITY(1,1) NOT NULL,
	[TimeStampUTC] [datetime] NOT NULL,
	[TimeStamp] [datetime] NOT NULL,
	[LogID] [int] NOT NULL,
	[LogLevel] [varchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[User] [dbo].[UserIdType] NOT NULL,
	[AuditType] [varchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[AccessGranted] [varchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[UserLocation] [dbo].[UriType] NOT NULL,
	[ResourceType] [varchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ResourceName] [nvarchar](512) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ResourceLocation] [nvarchar](512) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ResourceID] [dbo].[GuidType] NOT NULL,
	[Argument0] [nvarchar](512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Argument1] [nvarchar](512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Argument2] [nvarchar](512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Argument3] [nvarchar](512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Argument4] [nvarchar](512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Argument5] [nvarchar](512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Argument6] [nvarchar](512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogPending](
	[session_id] [dbo].[GuidType] NOT NULL,
	[log_id] [int] NOT NULL,
	[params] [xml] NOT NULL,
	[TimeStamp] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[LogPending] ADD  DEFAULT (getdate()) FOR [TimeStamp]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogSettings](
	[LastModified] [datetime] NOT NULL,
	[IDLogType] [dbo].[GuidType] NOT NULL,
	[TableName] [dbo].[NameType] NOT NULL,
	[DateTimeColumn] [nvarchar](64) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[TimeBetweenDatabaseWrites] [int] NOT NULL,
	[Name] [dbo].[NameType] NOT NULL,
	[LogHandlerAssemblyName] [dbo].[LongNameType] NULL,
	[LogHandlerClassName] [dbo].[LongNameType] NULL,
	[PrimaryLog] [bit] NULL,
	[Enabled] [bit] NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MachineProperties](
	[data_collector_id] [int] NOT NULL,
	[name] [nvarchar](200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[value] [bigint] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MarkedData](
	[IDMarkedData] [uniqueidentifier] NOT NULL,
	[LastModified] [datetime] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[Reference] [dbo].[NameType] NOT NULL,
	[UserName] [dbo].[NameType] NOT NULL,
	[Header] [dbo].[LongNameType] NOT NULL,
	[Description] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[MarkDataType] [int] NOT NULL,
	[StartTime] [bigint] NOT NULL,
	[TagTime] [bigint] NOT NULL,
	[EndTime] [bigint] NOT NULL,
	[RetentionType] [int] NOT NULL,
	[RetentionUnits] [int] NOT NULL,
	[RetentionExpire] [bigint] NOT NULL,
	[ProtectionSize] [bigint] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MarkedDataDevices](
	[IDMarkedData] [uniqueidentifier] NOT NULL,
	[IDDevice] [dbo].[GuidType] NOT NULL,
	[Size] [bigint] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Matrix](
	[IDMatrix] [dbo].[GuidType] NOT NULL,
	[Name] [dbo].[NameType] NOT NULL,
	[Description] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Address] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Port] [int] NOT NULL,
	[Password] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[LastUpdated] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MetadataSearchCategories](
	[Id] [dbo].[GuidType] NOT NULL,
	[Visibility] [bit] NOT NULL,
	[NonLocName] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MetadataSearchClassCandidates](
	[Id] [dbo].[GuidType] NOT NULL,
	[Name] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[CategoryId] [dbo].[GuidType] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MetadataSearchFilters](
	[Id] [dbo].[GuidType] NOT NULL,
	[Visibility] [bit] NOT NULL,
	[NonLocName] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[CategoryId] [dbo].[GuidType] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MetadataSearchObjectDefinitions](
	[Id] [dbo].[GuidType] NOT NULL,
	[ObjectRoot] [nvarchar](1024) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MetadataSearchPaths](
	[Id] [dbo].[GuidType] NOT NULL,
	[Path] [nvarchar](1024) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ObjectDefinitionId] [dbo].[GuidType] NOT NULL,
	[PropertyId] [dbo].[GuidType] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MetadataSearchProperties](
	[Id] [dbo].[GuidType] NOT NULL,
	[PropertyName] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[FilterId] [dbo].[GuidType] NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MIPAction](
	[IDMIPAction] [uniqueidentifier] NOT NULL,
	[LastModified] [datetime] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[Name] [dbo].[NameType] NOT NULL,
	[DefiningPluginId] [uniqueidentifier] NOT NULL,
	[DefiningPluginName] [dbo].[NameType] NOT NULL,
	[SelectionText] [dbo].[NameType] NOT NULL,
	[DescriptionText] [dbo].[NameType] NOT NULL,
	[PickerText] [dbo].[NameType] NOT NULL,
	[MIPKinds] [xml] NOT NULL,
	[PickerText2] [dbo].[NameType] NOT NULL,
	[MIPKinds2] [xml] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[MIPAction] ADD  CONSTRAINT [DF_PickerText2]  DEFAULT ('') FOR [PickerText2]
GO
ALTER TABLE [dbo].[MIPAction] ADD  CONSTRAINT [DF_MIPKinds2]  DEFAULT ('<MIPKinds />') FOR [MIPKinds2]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MIPObject](
	[IDMIPObject] [uniqueidentifier] NOT NULL,
	[LastModified] [datetime] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[Name] [dbo].[NameType] NOT NULL,
	[Kind] [uniqueidentifier] NOT NULL,
	[SubKind] [uniqueidentifier] NOT NULL,
	[PluginId] [uniqueidentifier] NOT NULL,
	[CustomData] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MonitorStates](
	[counter_state_rule_id] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[counter_path] [nvarchar](364) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[data_collector_id] [int] NOT NULL,
	[state] [int] NOT NULL,
	[is_monitored] [bit] NOT NULL,
	[counter_id]  AS (case when charindex('[',[counter_path])<>(0) AND substring([counter_path],charindex('[',[counter_path]),(1))='[' AND substring([counter_path],charindex('[',[counter_path])+(1),(36)) like '[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]-[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]-[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]-[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]-[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]' then substring([counter_path],charindex('[',[counter_path])+(1),(36)) else case when charindex('(',[counter_path])<>(0) AND substring([counter_path],charindex('(',[counter_path]),(1))='(' AND substring([counter_path],charindex('(',[counter_path])+(1),(36)) like '[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]-[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]-[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]-[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]-[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]' then substring([counter_path],charindex('(',[counter_path])+(1),(36))  end end) PERSISTED,
	[trigger_value] [float] NOT NULL
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[MonitorStates] ADD  DEFAULT ((0)) FOR [is_monitored]
GO
ALTER TABLE [dbo].[MonitorStates] ADD  DEFAULT ((0)) FOR [trigger_value]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotificationProfiles](
	[LastModified] [datetime] NOT NULL,
	[IDNotificationProfile] [dbo].[GuidType] NOT NULL,
	[Name] [dbo].[NameType] NOT NULL,
	[Description] [dbo].[DescriptionType] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotificationSettings](
	[LastModified] [datetime] NOT NULL,
	[IDNotificationSetting] [dbo].[GuidType] NOT NULL,
	[Settings] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[IDNotificationType] [dbo].[GuidType] NOT NULL,
	[IDNotificationProfile] [dbo].[GuidType] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotificationTypes](
	[LastModified] [datetime] NOT NULL,
	[IDNotificationType] [dbo].[GuidType] NOT NULL,
	[Name] [dbo].[NameType] NOT NULL,
	[PageName] [dbo].[NameType] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PatrollingProfiles](
	[LastModified] [datetime] NOT NULL,
	[IDPatrollingProfile] [dbo].[GuidType] NOT NULL,
	[IDDevice] [dbo].[GuidType] NOT NULL,
	[Name] [dbo].[NameType] NOT NULL,
	[Description] [dbo].[DescriptionType] NOT NULL,
	[Definition] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PerformanceCounterInstance](
	[pkid] [int] IDENTITY(1,1) NOT NULL,
	[data_collector_id] [int] NOT NULL,
	[category_name] [nvarchar](120) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[counter_name] [nvarchar](120) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[instance_name] [nvarchar](120) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[counter_type] [int] NOT NULL,
	[counter_id]  AS (case when substring(reverse([instance_name]),(1),(1))=']' AND substring(reverse([instance_name]),(38),(1))='[' AND substring([instance_name],len([instance_name])-(36),(36)) like '[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]-[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]-[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]-[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]-[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]' then substring([instance_name],len([instance_name])-(36),(36))  end),
	[path]  AS (case when [instance_name]='' then (('\'+[category_name])+'\')+[counter_name] else (((('\'+[category_name])+'(')+[instance_name])+')\')+[counter_name] end),
	[is_obsolete] [bit] NOT NULL
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[PerformanceCounterInstance] ADD  DEFAULT ((0)) FOR [is_obsolete]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PerformanceCounterStateOverride](
	[source_id] [uniqueidentifier] NOT NULL,
	[critical_threshold] [float] NOT NULL,
	[warning_threshold] [float] NOT NULL,
	[time_window] [int] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PerformanceCounterStateRule](
	[counter_state_rule_id] [uniqueidentifier] NOT NULL,
	[critical_threshold] [float] NOT NULL,
	[warning_threshold] [float] NOT NULL,
	[time_window] [int] NOT NULL,
	[rule_name] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[disabled] [bit] NOT NULL,
	[rule_type] [int] NOT NULL
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[PerformanceCounterStateRule] ADD  DEFAULT ((0)) FOR [disabled]
GO
ALTER TABLE [dbo].[PerformanceCounterStateRule] ADD  DEFAULT ((0)) FOR [rule_type]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PerformanceCounterStateSource](
	[counter_state_rule_id] [uniqueidentifier] NOT NULL,
	[counter_id] [int] NOT NULL,
	[data_collector_id] [int] NOT NULL,
	[source_id] [uniqueidentifier] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PerformanceCounterType](
	[counter_type_id] [int] IDENTITY(1,1) NOT NULL,
	[counter_direction] [bit] NOT NULL,
	[counter_type] [int] NOT NULL,
	[category_name] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[counter_name] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[enable_history] [bit] NOT NULL,
	[trigger_per_value] [bit] NOT NULL
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[PerformanceCounterType] ADD  DEFAULT ((1)) FOR [enable_history]
GO
ALTER TABLE [dbo].[PerformanceCounterType] ADD  DEFAULT ((0)) FOR [trigger_per_value]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PerformanceCounterValue](
	[instance_id] [int] NOT NULL,
	[collection_time] [datetime] NOT NULL,
	[raw_value] [bigint] NOT NULL,
	[calculated_value] [float] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecorderPluginDevice](
	[LastModified] [datetime] NOT NULL,
	[IDDevice] [dbo].[GuidType] NOT NULL,
	[ProcessOrder] [int] NOT NULL,
	[IDRecorderPlugin] [dbo].[GuidType] NOT NULL,
	[Settings] [xml] NOT NULL,
	[Enabled] [bit] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecorderPlugins](
	[LastModified] [datetime] NOT NULL,
	[IDRecorderPlugin] [dbo].[GuidType] NOT NULL,
	[Name] [dbo].[NameType] NOT NULL,
	[Description] [dbo].[DescriptionType] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Recorders](
	[LastModified] [datetime] NOT NULL,
	[IDRecorder] [dbo].[GuidType] NOT NULL,
	[Enabled] [bit] NOT NULL,
	[Name] [dbo].[NameType] NOT NULL,
	[Description] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[IDFailoverPrimaryGroup] [dbo].[GuidType] NULL,
	[IDFailoverSecondaryGroup] [dbo].[GuidType] NULL,
	[MulticastSettings] [xml] NOT NULL,
	[SynchronizationTime] [int] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[Recorders] ADD  DEFAULT ((-1)) FOR [SynchronizationTime]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecorderStatusData](
	[IDRecorder] [dbo].[GuidType] NOT NULL,
	[Type] [varchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Data] [xml] NOT NULL,
	[LastModified] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecordingServerService](
	[last_modified] [datetime] NOT NULL,
	[pkid] [dbo].[GuidType] NOT NULL,
	[host_name] [dbo].[NameType] NOT NULL,
	[scan_ranges] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[time_zone_name] [dbo].[NameType] NOT NULL,
	[failover_udp_port] [int] NULL,
	[devicepack_info] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[x64] [bit] NOT NULL,
	[database_port] [int] NOT NULL,
	[certificate_configured] [bit] NOT NULL,
	[webserver_hostname] [dbo].[NameType] NOT NULL,
	[webserver_port] [int] NOT NULL,
	[version] [dbo].[NameType] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecordingServerServiceConfigurable](
	[LastModified] [datetime] NOT NULL,
	[PKID] [dbo].[GuidType] NOT NULL,
	[MulticastServerAddress] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ShutdownOnStorageFailure] [bit] NOT NULL,
	[PublicAccessEnabled] [bit] NOT NULL,
	[PublicWebServerHostname] [dbo].[NameType] NOT NULL,
	[PublicWebServerPort] [int] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecordingServerServiceRecorder](
	[last_modified] [datetime] NOT NULL,
	[recording_server_service_id] [dbo].[GuidType] NOT NULL,
	[recorder_id] [dbo].[GuidType] NOT NULL,
	[type] [int] NOT NULL,
	[active] [bit] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecordingServerServiceType](
	[pkid] [int] NOT NULL,
	[type] [varchar](25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecordingStorage](
	[pkid] [dbo].[GuidType] NOT NULL,
	[recordingserver_id] [dbo].[GuidType] NOT NULL,
	[name] [dbo].[NameType] NOT NULL,
	[description] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[path] [nvarchar](512) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[is_default] [bit] NOT NULL,
	[retain_minutes] [int] NOT NULL,
	[max_size_mb] [bigint] NOT NULL,
	[created_date] [datetime] NOT NULL,
	[last_modified] [datetime] NOT NULL,
	[encryption_method] [dbo].[InternalIdType] NOT NULL,
	[encryption_password] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[encryption_password_new] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[signing] [bit] NOT NULL,
	[bank_id] [uniqueidentifier] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[RecordingStorage] ADD  DEFAULT ('None') FOR [encryption_method]
GO
ALTER TABLE [dbo].[RecordingStorage] ADD  DEFAULT ('') FOR [encryption_password]
GO
ALTER TABLE [dbo].[RecordingStorage] ADD  DEFAULT ('') FOR [encryption_password_new]
GO
ALTER TABLE [dbo].[RecordingStorage] ADD  DEFAULT ((0)) FOR [signing]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecordingStorageDevice](
	[recordingstorage_id] [dbo].[GuidType] NOT NULL,
	[device_id] [dbo].[GuidType] NOT NULL,
	[recording_enabled] [bit] NOT NULL,
	[recording_framerate] [float] NOT NULL,
	[prebuffer_enabled] [bit] NOT NULL,
	[prebuffer_seconds] [int] NOT NULL,
	[edgestorage_enabled] [bit] NOT NULL,
	[edgestorage_playback] [bit] NOT NULL,
	[record_keyframes] [bit] NOT NULL,
	[last_modified] [datetime] NOT NULL,
	[manual_timeout_enabled] [bit] NOT NULL,
	[manual_timeout] [int] NOT NULL,
	[record_on_related_devices] [bit] NOT NULL,
	[prebuffer_in_memory] [bit] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RegisteredDeviceTypes](
	[LastModified] [datetime] NOT NULL,
	[DeviceType] [dbo].[InternalIdType] NOT NULL,
	[Description] [dbo].[DescriptionType] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[PrimaryIconNumber] [int] NOT NULL,
	[IsRecordingDevice] [bit] NOT NULL,
	[SecurityNamespaceName] [nvarchar](260) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RegisteredServers](
	[LastModified] [datetime] NOT NULL,
	[IDServer] [dbo].[GuidType] NOT NULL,
	[Type] [dbo].[GuidType] NOT NULL,
	[Name] [dbo].[NameType] NOT NULL,
	[Description] [dbo].[DescriptionType] NOT NULL,
	[Addresses] [dbo].[UriType] NOT NULL,
	[Trusted] [bit] NOT NULL,
	[CustomData] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Enabled] [bit] NULL,
	[EndpointDefinitions] [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[DataCollectorId] [int] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[RegisteredServers] ADD  DEFAULT (N'<endpoints></endpoints>') FOR [EndpointDefinitions]
GO
ALTER TABLE [dbo].[RegisteredServers] ADD  DEFAULT ((0)) FOR [DataCollectorId]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Report](
	[pkid] [int] IDENTITY(1,1) NOT NULL,
	[name] [dbo].[NameType] NOT NULL,
	[description] [dbo].[DescriptionType] NOT NULL,
	[url] [varchar](200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[created_date] [datetime] NOT NULL,
	[last_modified] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestLog](
	[RequestId] [bigint] IDENTITY(1,1) NOT NULL,
	[WebMethodName] [varchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[ExecutionTime] [bigint] NOT NULL,
	[IdentityName] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[IPAddress] [varchar](40) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ExecutionCount] [int] NOT NULL,
	[TempId] [bigint] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestLogParameter](
	[RequestId] [bigint] NOT NULL,
	[ParameterName] [varchar](64) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ParameterIndex] [int] NOT NULL,
	[ParameterValue] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RestrictedMedia](
	[LastModified] [datetime2](7) NOT NULL,
	[IDRestrictedMedia] [uniqueidentifier] NOT NULL,
	[Header] [dbo].[LongNameType] NOT NULL,
	[Description] [dbo].[DescriptionType2] NOT NULL,
	[UserName] [dbo].[NameType] NOT NULL,
	[StartTime] [datetime2](7) NOT NULL,
	[EndTime] [datetime2](7) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RestrictedMediaDevice](
	[IDRestrictedMedia] [uniqueidentifier] NOT NULL,
	[IDDevice] [dbo].[GuidType] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RestrictedMediaLive](
	[LastModified] [datetime2](7) NOT NULL,
	[IDDevice] [dbo].[GuidType] NOT NULL,
	[UserName] [dbo].[NameType] NOT NULL,
	[StartTime] [datetime2](7) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RetentionOptionRoleRelation](
	[IdRetentionOption] [dbo].[GuidType] NOT NULL,
	[IdRole] [dbo].[GuidType] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rules](
	[LastModified] [datetime] NOT NULL,
	[IDRule] [dbo].[GuidType] NOT NULL,
	[Name] [dbo].[NameType] NOT NULL,
	[Description] [dbo].[DescriptionType] NOT NULL,
	[Definition] [xml] NOT NULL,
	[Enabled] [bit] NOT NULL,
	[Priority] [int] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RuleVariables](
	[LastModified] [datetime] NOT NULL,
	[IDVariable] [dbo].[GuidType] NOT NULL,
	[Name] [dbo].[NameType] NOT NULL,
	[Description] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Value] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Reset] [bit] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SecurityAccessCache2](
	[namespace_id] [uniqueidentifier] NOT NULL,
	[user_sid] [dbo].[SIDType] NOT NULL,
	[action_name] [nvarchar](260) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[object_id] [nvarchar](300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[allowed_access] [bit] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SecurityAccessEntry](
	[access_entry_id] [int] IDENTITY(1,1) NOT NULL,
	[security_object_id] [uniqueidentifier] NOT NULL,
	[user_sid] [dbo].[SIDType] NOT NULL,
	[action_name] [nvarchar](260) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[access_deny] [bit] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SecurityAccessEntryTimeProfile](
	[access_entry_id] [int] NOT NULL,
	[timeprofile_id] [dbo].[GuidType] NULL,
	[is_default] [bit] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SecurityAction](
	[namespace_id] [uniqueidentifier] NOT NULL,
	[name] [nvarchar](260) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[display_name] [nvarchar](260) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[order_no] [int] NOT NULL,
	[visible] [bit] NOT NULL,
	[security_bit] [decimal](20, 0) NOT NULL,
	[legacy_security_name] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SecurityAttributes](
	[LastModified] [datetime] NOT NULL,
	[SecurityAttributeID] [dbo].[InternalIdType] NOT NULL,
	[Name] [dbo].[NameType] NOT NULL,
	[Description] [dbo].[DescriptionType] NOT NULL,
	[ParentSecurityAttributeName] [dbo].[InternalIdType] NOT NULL,
	[SecurityType] [dbo].[InternalIdType] NOT NULL,
	[HasObject] [bit] NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[Definition] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TimeProfileBased] [bit] NOT NULL,
	[TimeBackwardBased] [bit] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[SecurityAttributes] ADD  CONSTRAINT [TimeProfileBasedDefault]  DEFAULT ((0)) FOR [TimeProfileBased]
GO
ALTER TABLE [dbo].[SecurityAttributes] ADD  CONSTRAINT [TimeBackwardBasedDefault]  DEFAULT ((0)) FOR [TimeBackwardBased]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SecurityClaim](
	[IDClaim] [int] IDENTITY(1,1) NOT NULL,
	[LoginProviderId] [uniqueidentifier] NOT NULL,
	[ClaimName] [nvarchar](450) COLLATE Latin1_General_100_BIN NOT NULL,
	[CaseSensitive] [bit] NOT NULL,
	[DisplayName] [nvarchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SecurityIdentity](
	[SID] [varchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Type] [tinyint] NOT NULL,
	[DisplayName] [nvarchar](512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Description] [nvarchar](2048) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Domain] [nvarchar](512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[AccountName] [nvarchar](512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[DistinguishedName] [nvarchar](2044) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Deleted] [bit] NOT NULL,
	[LastModified] [datetime] NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SecurityMembership](
	[Container] [varchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[MemberSID] [dbo].[SIDType] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SecurityMembershipCache](
	[MemberSID] [dbo].[SIDType] NOT NULL,
	[GroupSID] [dbo].[SIDType] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SecurityNamespace](
	[namespace_id] [uniqueidentifier] NOT NULL,
	[name] [nvarchar](260) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[display_name] [nvarchar](260) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[separator] [char](1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[structure] [int] NOT NULL,
	[order_no] [int] NOT NULL,
	[user_defined] [bit] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SecurityObject](
	[namespace_id] [uniqueidentifier] NOT NULL,
	[parent_path] [nvarchar](400) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[object_id] [nvarchar](300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[object_type] [tinyint] NOT NULL,
	[object_name] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[security_object_id] [uniqueidentifier] NOT NULL,
	[inherits_from_id] [nvarchar](300) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[full_path]  AS ([parent_path]+[object_id])
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SecurityPermissions](
	[LastModified] [datetime] NOT NULL,
	[IDRole] [dbo].[GuidType] NOT NULL,
	[SecurityAttributeID] [dbo].[InternalIdType] NOT NULL,
	[Value] [dbo].[SecurityValueType] NULL,
	[IDTimeProfile] [dbo].[GuidType] NULL,
	[IsDefaultTimeProfile] [bit] NOT NULL
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[SecurityPermissions] ADD  CONSTRAINT [SecurityPermissions_IsDefaultTimeProfileConstraint]  DEFAULT ((1)) FOR [IsDefaultTimeProfile]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SecurityPermissionsDevice](
	[LastModified] [datetime] NOT NULL,
	[SecurityAttributeID] [dbo].[InternalIdType] NOT NULL,
	[IDRole] [dbo].[GuidType] NOT NULL,
	[IDDevice] [dbo].[GuidType] NOT NULL,
	[Value] [dbo].[SecurityValueType] NULL,
	[IDTimeProfile] [dbo].[GuidType] NULL,
	[TimeBackward] [int] NOT NULL,
	[IsDefaultTimeProfile] [bit] NOT NULL
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[SecurityPermissionsDevice] ADD  CONSTRAINT [TimeBackwardDefault]  DEFAULT ((0)) FOR [TimeBackward]
GO
ALTER TABLE [dbo].[SecurityPermissionsDevice] ADD  CONSTRAINT [SecurityPermissionsDevice_IsDefaultTimeProfileConstraint]  DEFAULT ((1)) FOR [IsDefaultTimeProfile]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SecurityPermissionsMatrix](
	[LastModified] [datetime] NOT NULL,
	[SecurityAttributeID] [dbo].[InternalIdType] NOT NULL,
	[IDRole] [dbo].[GuidType] NOT NULL,
	[IDMatrix] [dbo].[GuidType] NOT NULL,
	[Value] [dbo].[SecurityValueType] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SecurityPermissionsViewGroup](
	[LastModified] [datetime] NOT NULL,
	[SecurityAttributeID] [dbo].[InternalIdType] NOT NULL,
	[IDRole] [dbo].[GuidType] NOT NULL,
	[IDViewGroup] [dbo].[GuidType] NOT NULL,
	[Value] [dbo].[SecurityValueType] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SecurityRoleClaim](
	[IDRoleClaim] [int] IDENTITY(1,1) NOT NULL,
	[IDRole] [dbo].[GuidType] NOT NULL,
	[IDClaim] [int] NOT NULL,
	[ClaimValue] [nvarchar](512) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SecurityRoles](
	[LastModified] [datetime] NOT NULL,
	[IDRole] [dbo].[GuidType] NOT NULL,
	[Name] [dbo].[NameType] NOT NULL,
	[Description] [dbo].[DescriptionType] NOT NULL,
	[Builtin] [bit] NOT NULL,
	[Enabled] [bit] NOT NULL,
	[Type] [tinyint] NOT NULL,
	[DualRequired] [bit] NOT NULL,
	[PtzReservedAsAnonymous] [bit] NOT NULL,
	[AllowMobileClientLogOn] [bit] NOT NULL,
	[AllowSmartClientLogOn] [bit] NOT NULL,
	[AllowWebClientLogOn] [bit] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[SecurityRoles] ADD  CONSTRAINT [BuiltinDefault]  DEFAULT ((0)) FOR [Builtin]
GO
ALTER TABLE [dbo].[SecurityRoles] ADD  CONSTRAINT [EnabledDefault]  DEFAULT ((1)) FOR [Enabled]
GO
ALTER TABLE [dbo].[SecurityRoles] ADD  DEFAULT ((0)) FOR [PtzReservedAsAnonymous]
GO
ALTER TABLE [dbo].[SecurityRoles] ADD  DEFAULT ((1)) FOR [AllowMobileClientLogOn]
GO
ALTER TABLE [dbo].[SecurityRoles] ADD  DEFAULT ((1)) FOR [AllowSmartClientLogOn]
GO
ALTER TABLE [dbo].[SecurityRoles] ADD  DEFAULT ((1)) FOR [AllowWebClientLogOn]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SecurityRolesIdentity](
	[IDRole] [dbo].[GuidType] NOT NULL,
	[SID] [dbo].[SIDType] NOT NULL,
	[LastModified] [datetime] NOT NULL,
	[IsStatic] [bit] NOT NULL,
	[IsDynamic] [bit] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SecurityTableLock](
	[LockLevel] [int] NOT NULL,
	[LastUpdated] [datetime] NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SecurityUserUpdatedCache](
	[IDUser] [uniqueidentifier] NOT NULL,
	[LastModified] [datetime2](7) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SequenceCounter](
	[Name] [nvarchar](30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Value] [int] NULL,
	[MaxValue] [int] NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sequences](
	[Name] [dbo].[NameType] NOT NULL,
	[Sequence] [bigint] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServerGroup](
	[IDServerGroup] [dbo].[GuidType] NOT NULL,
	[Name] [dbo].[NameType] NOT NULL,
	[Description] [nvarchar](1024) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[IDParentGroup] [dbo].[GuidType] NOT NULL,
	[StructureType] [varchar](64) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[OrderNo] [int] NOT NULL,
	[Level] [int] NOT NULL,
	[Path] [nvarchar](4000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModified] [datetime] NOT NULL,
	[RelationIds] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[ServerGroup] ADD  CONSTRAINT [SG_RELATIONIDS_DEFAULT]  DEFAULT ('') FOR [RelationIds]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServerGroupKind](
	[IDServerGroup] [dbo].[GuidType] NOT NULL,
	[Kind] [dbo].[GuidType] NOT NULL,
	[PluginId] [dbo].[GuidType] NOT NULL,
	[CustomData] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServerGroupMembership](
	[IDServerGroup] [dbo].[GuidType] NOT NULL,
	[IDObject] [dbo].[GuidType] NOT NULL,
	[ObjectType] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[LastModified] [datetime] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServerType](
	[name] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[upgrade_code] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[service_type] [dbo].[GuidType] NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sites](
	[IDSite] [dbo].[GuidType] NOT NULL,
	[IDSiteType] [int] NOT NULL,
	[Name] [dbo].[NameType] NOT NULL,
	[Description] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[IDSiteState] [int] NOT NULL,
	[LastModified] [datetime] NOT NULL,
	[LastStatusHandshake] [datetime] NOT NULL,
	[Addresses] [xml] NOT NULL,
	[C14NSlaveTree] [char](24) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[MarkInconsistent] [datetime] NULL,
	[Version] [nvarchar](15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ServiceAccount] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[SynchronizationStatus] [int] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[Sites] ADD  CONSTRAINT [SITES_VERISON_DEFAULT]  DEFAULT ('') FOR [Version]
GO
ALTER TABLE [dbo].[Sites] ADD  CONSTRAINT [SITES_SERVICE_ACCOUNT_DEFAULT]  DEFAULT ('') FOR [ServiceAccount]
GO
ALTER TABLE [dbo].[Sites] ADD  CONSTRAINT [SITES_SYNC_STATUS_DEFAULT]  DEFAULT ((0)) FOR [SynchronizationStatus]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SitesRelation](
	[IDSiteMaster] [dbo].[GuidType] NOT NULL,
	[IDSiteSlave] [dbo].[GuidType] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SiteState](
	[IDSiteState] [int] NOT NULL,
	[State] [char](30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SiteType](
	[IDSiteType] [int] NOT NULL,
	[TypeName] [char](20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StateGroup](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[States] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[last_modified] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[System](
	[version] [varchar](30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemDashboardConfiguration](
	[id] [int] NOT NULL,
	[name] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[state_source_type] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[single_state_source_id] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[event_types] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[position] [int] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[SystemDashboardConfiguration] ADD  DEFAULT ((2147483647)) FOR [position]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemSettings](
	[LastModified] [datetime] NOT NULL,
	[Name] [dbo].[NameType] NOT NULL,
	[Setting] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Task](
	[pkid] [bigint] IDENTITY(1,1) NOT NULL,
	[type] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[target] [dbo].[GuidType] NOT NULL,
	[data_a] [xml] NOT NULL,
	[data_b] [xml] NULL,
	[percent_completed] [int] NOT NULL,
	[status_details] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[task_status] [int] NOT NULL,
	[start_time] [datetime] NOT NULL,
	[end_time] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaskTableLock](
	[LockLevel] [int] NOT NULL,
	[LastUpdated] [datetime] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempCustomProperty](
	[SPID] [int] NOT NULL,
	[name] [dbo].[NameType] NOT NULL,
	[value] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempCustomPropertyDelete](
	[SPID] [int] NOT NULL,
	[object_id] [varchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempSecurityAccessEntry](
	[SPID] [int] NOT NULL,
	[user_sid] [dbo].[SIDType] NOT NULL,
	[action_name] [nvarchar](260) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[access_deny] [bit] NOT NULL,
	[change] [tinyint] NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempSecurityDeletedIdentities](
	[SPID] [int] NOT NULL,
	[SID] [dbo].[SIDType] NOT NULL,
	[used] [bit] NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TempSecurityIdentities](
	[SPID] [int] NOT NULL,
	[SID] [dbo].[SIDType] NOT NULL,
	[Type] [tinyint] NULL,
	[DisplayName] [nvarchar](512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Description] [nvarchar](2048) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Domain] [nvarchar](512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[AccountName] [nvarchar](512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[DistinguishedName] [nvarchar](2044) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TimeProfile](
	[pkid] [dbo].[GuidType] NOT NULL,
	[name] [dbo].[NameType] NOT NULL,
	[description] [dbo].[DescriptionType] NOT NULL,
	[type] [nvarchar](15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[xml_data] [xml] NOT NULL,
	[created_date] [datetime] NOT NULL,
	[last_modified] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TimeProfileDefaultRoleRelation](
	[IDTimeProfile] [dbo].[GuidType] NOT NULL,
	[IDRole] [dbo].[GuidType] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Token](
	[TokenId] [nvarchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[SID] [dbo].[SIDType] NOT NULL,
	[Expires] [bigint] NOT NULL,
	[RegistrationTime] [datetime] NOT NULL,
	[ManagementClientToken] [bit] NOT NULL
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Token] ADD  DEFAULT ((0)) FOR [ManagementClientToken]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VideoWall](
	[pkid] [dbo].[GuidType] NOT NULL,
	[name] [dbo].[NameType] NOT NULL,
	[description] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[width] [float] NOT NULL,
	[height] [float] NOT NULL,
	[created_date] [datetime] NOT NULL,
	[last_modified] [datetime] NOT NULL,
	[statustext] [bit] NOT NULL,
	[titlebar] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VideoWallMonitor](
	[pkid] [dbo].[GuidType] NOT NULL,
	[videowall_id] [dbo].[GuidType] NOT NULL,
	[view_id] [dbo].[GuidType] NOT NULL,
	[name] [dbo].[NameType] NOT NULL,
	[description] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[monitor_size] [real] NOT NULL,
	[aspect_ratio] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[insertion_method] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[no_layout_method] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[empty_viewitems_method] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[location_x] [float] NOT NULL,
	[location_y] [float] NOT NULL,
	[created_date] [datetime] NOT NULL,
	[last_modified] [datetime] NOT NULL,
	[monitor_state] [varchar](2000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[VideoWallMonitor] ADD  DEFAULT ('') FOR [monitor_state]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VideoWallMonitorPreset](
	[pkid] [dbo].[GuidType] NOT NULL,
	[monitor_id] [dbo].[GuidType] NOT NULL,
	[videowallpreset_id] [dbo].[GuidType] NOT NULL,
	[last_modified] [datetime] NOT NULL,
	[definition_xml] [xml] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VideoWallPreset](
	[pkid] [dbo].[GuidType] NOT NULL,
	[videowall_id] [dbo].[GuidType] NOT NULL,
	[name] [dbo].[NameType] NOT NULL,
	[description] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[created_date] [datetime] NOT NULL,
	[last_modified] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VideoWallView](
	[pkid] [dbo].[GuidType] NOT NULL,
	[name] [dbo].[NameType] NOT NULL,
	[description] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[created_date] [datetime] NOT NULL,
	[last_modified] [datetime] NOT NULL,
	[definition_xml] [xml] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VideoWallViewItem](
	[pkid] [dbo].[GuidType] NOT NULL,
	[view_id] [dbo].[GuidType] NOT NULL,
	[position] [int] NOT NULL,
	[definition_xml] [xml] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VideoWallViewLayout](
	[pkid] [dbo].[GuidType] NOT NULL,
	[name] [dbo].[NameType] NOT NULL,
	[description] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[created_date] [datetime] NOT NULL,
	[last_modified] [datetime] NOT NULL,
	[definition_xml] [xml] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Mip].[IntegrationRegistrations](
	[InstanceId] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[IntegrationId] [uniqueidentifier] NOT NULL,
	[Type] [nvarchar](32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[IntegrationVersion] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[IntegrationName] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ManufacturerName] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[RegistrationTime] [datetime] NOT NULL,
	[AllowIntegrationRegistration] [bit] NOT NULL
) ON [PRIMARY]

GO
ALTER TABLE [Mip].[IntegrationRegistrations] ADD  DEFAULT ((0)) FOR [AllowIntegrationRegistration]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Mip].[Items](
	[ItemId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Kind] [uniqueidentifier] NOT NULL,
	[ParentId] [uniqueidentifier] NOT NULL,
	[ServerId] [uniqueidentifier] NOT NULL,
	[ServerHost] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ServerPort] [int] NOT NULL,
	[ServerType] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Enabled] [bit] NOT NULL,
	[Properties] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[GisPoint] [geography] NOT NULL,
	[LastModified] [datetime] NOT NULL,
	[ServerScheme] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Encrypted] [bit] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [Mip].[Items] ADD  DEFAULT ((0)) FOR [Encrypted]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Mip].[Kinds](
	[KindId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[PluginId] [uniqueidentifier] NOT NULL,
	[PluginName] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[DataVersion] [int] NOT NULL,
	[KindType] [nvarchar](16) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[SecurityAction] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[DisplayOnGisMap] [bit] NOT NULL,
	[ParentKindId] [uniqueidentifier] NOT NULL,
	[LastModified] [datetime] NOT NULL,
	[ModifiedUser] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Search].[SearchTemplates](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Description] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Availability] [nvarchar](40) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[IncludesScopeItems] [bit] NOT NULL,
	[SearchQuery] [varbinary](max) NOT NULL,
	[Owner] [dbo].[SIDType] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Search].[SearchTemplatesFind](
	[Id] [uniqueidentifier] NOT NULL,
	[Count] [int] NOT NULL,
	[StartIndex] [int] NOT NULL,
	[KeywordFilter] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[PrivateOnly] [bit] NOT NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[Owner] [dbo].[SIDType] NOT NULL
) ON [PRIMARY]

GO
