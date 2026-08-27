-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SiteState]') AND type in (N'U'))
BEGIN
	CREATE TABLE [SiteState] (
		[IDSiteState] INT NOT NULL,
		[State] CHAR(30) NOT NULL
		
		CONSTRAINT [PK_SiteStatus] PRIMARY KEY NONCLUSTERED
		(
			[IDSiteState] ASC
		)
	) ON [PRIMARY]
END
GO

IF NOT EXISTS(SELECT * FROM SiteState WHERE IDSiteState = 1)
BEGIN
	INSERT INTO SiteState([IDSiteState],[State])
	VALUES(1,'Attached')
END

IF NOT EXISTS(SELECT * FROM SiteState WHERE IDSiteState = 2)
BEGIN
	INSERT INTO SiteState([IDSiteState],[State])
	VALUES(2,'AttachPending')
END

IF NOT EXISTS(SELECT * FROM SiteState WHERE IDSiteState = 3)
BEGIN
	INSERT INTO SiteState([IDSiteState],[State])
	VALUES(3,'Unauthorized')
END

IF NOT EXISTS(SELECT * FROM SiteState WHERE IDSiteState = 4)
BEGIN
	INSERT INTO SiteState([IDSiteState],[State])
	VALUES(4,'WaitRemoteAuthorize')
END

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SiteType]') AND type in (N'U'))
BEGIN
	CREATE TABLE [SiteType] (
		[IDSiteType] INT NOT NULL,
		[TypeName] CHAR(20) NOT NULL
		
		CONSTRAINT [PK_SitesTypes] PRIMARY KEY NONCLUSTERED
		(
			[IDSiteType] ASC
		)
	) ON [PRIMARY]
END
GO

IF NOT EXISTS(SELECT * FROM SiteType WHERE IDSiteType = 1)
BEGIN
	INSERT INTO SiteType(IDSiteType,TypeName)
	VALUES(1,'Home')
END

IF NOT EXISTS(SELECT * FROM SiteType WHERE IDSiteType = 2)
BEGIN
	INSERT INTO SiteType(IDSiteType,TypeName)
	VALUES(2,'Master')
END

IF NOT EXISTS(SELECT * FROM SiteType WHERE IDSiteType = 3)
BEGIN
	INSERT INTO SiteType(IDSiteType,TypeName)
	VALUES(3,'Slave')
END

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sites]') AND type in (N'U'))
BEGIN
	CREATE TABLE [Sites] (
		[IDSite] [GuidType] NOT NULL,
		[IDSiteType] INT NOT NULL,
		[Name] [NameType] NOT NULL,
		[Description] NVARCHAR(MAX),
		[IDSiteState] INT NOT NULL,
		[LastModified] DATETIME NOT NULL,
		[LastStatusHandshake] DATETIME NOT NULL,
		[Addresses] XML NOT NULL,
		[C14NSlaveTree] Char(24) NULL,
		[MarkInconsistent] DATETIME NULL
		
		CONSTRAINT [PK_Sites] PRIMARY KEY NONCLUSTERED
		(
			[IDSite] ASC
		),
		CONSTRAINT [FK_Sites_SiteType] FOREIGN KEY
		(
			[IDSiteType]
		)
		REFERENCES [SiteType] ([IDSiteType]), 
		
		CONSTRAINT [FK_Sites_SiteState] FOREIGN KEY
		(
			[IDSiteState]
		)
		REFERENCES [SiteState] ([IDSiteState])
	) ON [PRIMARY]
END
GO

IF NOT EXISTS(SELECT * FROM Sites WHERE IDSiteType = 1)
BEGIN
	INSERT INTO Sites([IDSite], [IDSiteType],[Name], [Description], [IDSiteState], [LastModified], [LastStatusHandshake], [Addresses])
	VALUES(newid(), 1, '', '', 1, GETUTCDATE(), GETUTCDATE(), '<Addresses></Addresses>')
END


IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SitesRelation]') AND type in (N'U'))
BEGIN
	CREATE TABLE [SitesRelation] (
		[IDSiteMaster] [GuidType] NOT NULL,
		[IDSiteSlave] [GuidType] NOT NULL
		
		CONSTRAINT [PK_SitesRelation] PRIMARY KEY NONCLUSTERED
		(
			[IDSiteSlave] ASC
		),
		CONSTRAINT [FK_SitesRelationMaster_Site] FOREIGN KEY
		(
			[IDSiteMaster]
		)
		REFERENCES [Sites] ([IDSite]), 
		
		CONSTRAINT [FK_SitesRelationSlave_Site] FOREIGN KEY
		(
			[IDSiteSlave]
		)
		REFERENCES [Sites] ([IDSite])
	) ON [PRIMARY]
END
GO
