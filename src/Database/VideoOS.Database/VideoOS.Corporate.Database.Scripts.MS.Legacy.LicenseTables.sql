-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[LicenseRecorder]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[LicenseRecorder]
	(
		[LastModified] [datetime] NOT NULL,
		[IDRecorder] [uniqueidentifier] NOT NULL,
		
		CONSTRAINT [PK_LicenseRecorder] PRIMARY KEY NONCLUSTERED 
		(
			[IDRecorder] ASC
		)	
	) ON [PRIMARY]
END
GO


IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[LicenseHardware]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[LicenseHardware]
	(
		[LastModified] [datetime] NOT NULL,
		[IDHardware] [uniqueidentifier] NOT NULL,
		[License] BIGINT NOT NULL,
		
		CONSTRAINT [PK_LicenseHardware] PRIMARY KEY NONCLUSTERED
		(
			[IDHardware] ASC
		)	
	) ON [PRIMARY]
END
GO


IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[LicenseDevice]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[LicenseDevice]
	(
		[LastModified] [datetime] NOT NULL,
		[IDDevice] [uniqueidentifier] NOT NULL,
		[License] BIT NOT NULL,
		
		CONSTRAINT [PK_LicenseDevice] PRIMARY KEY NONCLUSTERED
		(
			[IDDevice] ASC
		)	
	) ON [PRIMARY]
END
GO

-- Cleanup old license data
DELETE [dbo].[License] WHERE [IDLicense] = 'dd3ab650-ec0b-40ec-9391-c32d5388a26f'
DELETE [dbo].[License] WHERE [IDLicense] IN (SELECT [IDHardware] FROM [dbo].[Hardware])
DELETE [dbo].[License] WHERE [IDLicense] IN (SELECT [IDDevice] FROM [dbo].[Devices])
GO