-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Tabel TrustedServer is overtaken by new table RegisteredServers with the implementation
-- of Server Registration interface.

DECLARE @CurrentTime DateTime
SET @CurrentTime = GETUTCDATE()

-- Adding new RegisteredServers tabel to replace TrustedServers tabel
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RegisteredServers]') AND type in (N'U'))
BEGIN
	CREATE TABLE [RegisteredServers](
		[LastModified] [datetime] NOT NULL,
		[IDServer] [GuidType] NOT NULL,
		[Type] [GuidType] NOT NULL,
		[Name] [NameType] NOT NULL,
		[Description] ntext NOT NULL,
		[Addresses] [UriType] NOT NULL,
		[Trusted] BIT NOT NULL,
		[CustomData] ntext NOT NULL,
		CONSTRAINT [PK_RegisteredServers] PRIMARY KEY CLUSTERED 
		(
			[IDServer] ASC
		)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]

	-- Copy data from TrustedServers tabel to RegisteredServers tabel
	INSERT INTO [RegisteredServers] (
		[LastModified],
		[IDServer],
		[Type],
		[Name],
		[Description],
		[Addresses],
		[Trusted],
		[CustomData])
		SELECT 
			[LastModified], 
			[IDServer], 
			'Type' = 'DC4E7BB6-D8C9-4179-80B9-37ED9715EE9A', 
			[Name], 
			'Description' = '', 
			[Address],
			'Trusted' = 1,
			'CustomData' = ''
		FROM [TrustedServers]
END

-- Drop TrustedServers tabel
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[TrustedServers]') AND type in (N'U'))
DROP TABLE [TrustedServers]

-- Drop SecurityPermissionsTrustedServer tabel as it is not used
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SecurityPermissionsTrustedServer]') AND type in (N'U'))
DROP TABLE [SecurityPermissionsTrustedServer]

-- Add new security attribute
IF NOT EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'APPLICATIONACCESSSERVICEREGAPI')
INSERT INTO SecurityAttributes(LastModified,SecurityAttributeID,[Name],Description,ParentSecurityAttributeName,SecurityType,HasObject,DisplayOrder,Definition) 
VALUES(@CurrentTime,'APPLICATIONACCESSSERVICEREGAPI','Service Registration API','Service Registration API Access','APPLICATIONACCESS','Application',0,6,'')

GO
