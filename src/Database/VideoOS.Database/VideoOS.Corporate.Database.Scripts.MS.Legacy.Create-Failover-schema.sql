-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FailoverRecorderGroups]') AND type in (N'U'))
CREATE TABLE [FailoverRecorderGroups]
(
	[LastModified] DATETIME NOT NULL CONSTRAINT [DF_FailoverRecorderGroups_LastModified]  DEFAULT (getdate()),
	[IDFailoverGroup] [GuidType] NOT NULL,
	[Name] [NameType] NOT NULL,
	[Description] [DescriptionType] NOT NULL CONSTRAINT [DF_FailoverRecorderGroups_Description]  DEFAULT (''),
	[IDParentFailoverGroup] [GuidType] NULL,

	CONSTRAINT [PK_FailoverRecorderGroups] PRIMARY KEY CLUSTERED 
	(
		[IDFailoverGroup] ASC
	)
) ON [PRIMARY]
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FailoverRecorders]') AND type in (N'U'))
CREATE TABLE [FailoverRecorders]
(
	[LastModified] [datetime] NOT NULL CONSTRAINT [DF_FailoverRecorders_LastModified]  DEFAULT (getdate()),
	[IDFailoverRecorder] [GuidType] NOT NULL,
	[Name] [NameType] NOT NULL,
	[Description] [DescriptionType] NOT NULL CONSTRAINT [DF_FailoverRecorders_Description]  DEFAULT (''),
	[HostName] [UriType] NOT NULL,
	[UdpPort] [smallint] NOT NULL,
	[Enabled] [bit] NOT NULL,
	[GroupSequence] [smallint] NOT NULL,
	[IDFailoverGroup] [GuidType] NOT NULL,

	CONSTRAINT [PK_FailoverRecorders] PRIMARY KEY CLUSTERED 
	(
		[IDFailoverRecorder] ASC
	)
) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT syscolumns.* FROM sysobjects INNER JOIN syscolumns ON sysobjects.id = syscolumns.id WHERE sysobjects.name = 'Recorders' AND syscolumns.name= 'IDFailoverPrimaryGroup')
BEGIN
	ALTER TABLE Recorders ADD
	[IDFailoverPrimaryGroup] [GuidType] NULL
END
GO

IF NOT EXISTS (SELECT syscolumns.*, sysobjects.name FROM sysobjects INNER JOIN syscolumns ON sysobjects.id = syscolumns.id WHERE sysobjects.name = 'Recorders' AND syscolumns.name= 'IDFailoverSecondaryGroup')
BEGIN
	ALTER TABLE Recorders ADD
	[IDFailoverSecondaryGroup] [GuidType] NULL
END
GO

IF NOT EXISTS (SELECT syscolumns.*, sysobjects.name FROM sysobjects INNER JOIN syscolumns ON sysobjects.id = syscolumns.id WHERE sysobjects.name = 'Recorders' AND syscolumns.name= 'FailoverUdpPort')
BEGIN
	ALTER TABLE Recorders ADD
	[FailoverUdpPort] [smallint] NULL
END
GO

UPDATE [Recorders] 
SET [FailoverUdpPort] = 11000
WHERE IsNull(FailoverUdpPort, 0) = 0
GO
