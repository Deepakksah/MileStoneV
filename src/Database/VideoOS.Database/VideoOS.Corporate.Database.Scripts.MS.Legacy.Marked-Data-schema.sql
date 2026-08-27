-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[MarkedData]') AND type in (N'U'))
CREATE TABLE [MarkedData]
(
	[IDMarkedData] [UNIQUEIDENTIFIER] NOT NULL,
	[LastModified] [DATETIME] NOT NULL,
	[CreatedDate] [DATETIME] NOT NULL,
	[Reference] [NameType] NOT NULL,
	[UserName] [NameType] NOT NULL,
	[Header] [LongNameType] NOT NULL,
	[Description] NVARCHAR(MAX) NOT NULL,
	[MarkDataType] [int] NOT NULL,
	[StartTime] BigInt NOT NULL,
	[TagTime] BigInt NOT NULL,
	[EndTime] BigInt NOT NULL,
	[Protected] BIT NOT NULL,
	[RetentionType] int NOT NULL,
	[RetentionUnits] int NOT NULL,
	[RetentionExpire] BigInt NOT NULL,
	[ProtectionSize] [BigInt] NOT NULL,

	CONSTRAINT [PK_MarkedData] PRIMARY KEY NONCLUSTERED
	(
		[IDMarkedData] ASC
	),
) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[MarkedData]') AND name = N'IX_MarkedData_TagTime')
CREATE CLUSTERED INDEX [IX_MarkedData_TagTime] ON [dbo].[MarkedData] ( [TagTime] ASC ) 
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[MarkedDataDevices]') AND type in (N'U'))
CREATE TABLE [MarkedDataDevices]
(
	[IDMarkedData] [UNIQUEIDENTIFIER] NOT NULL,
	[IDDevice] [GuidType] NOT NULL,

	CONSTRAINT [PK_MarkedDataDevices] PRIMARY KEY NONCLUSTERED
	(
		[IDMarkedData], [IDDevice] ASC
	),
	
	CONSTRAINT [FK_MarkedDataDevices_MarkedData] FOREIGN KEY([IDMarkedData]) REFERENCES [MarkedData] ([IDMarkedData]),
	CONSTRAINT [FK_MarkedDataDevices_Devices] FOREIGN KEY([IDDevice]) REFERENCES [Devices] ([IDDevice])

) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[MarkedDataDevices]') AND name = N'IX_MarkedDataDevices_IDDevice')
CREATE NONCLUSTERED INDEX [IX_MarkedDataDevices_IDDevice] ON [dbo].[MarkedDataDevices] ( [IDDevice] ASC ) 
GO




