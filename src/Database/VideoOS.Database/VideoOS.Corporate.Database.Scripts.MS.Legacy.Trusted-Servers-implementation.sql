-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Creates new tables 'TrustedServers' and 'SecurityPermissionsTrustedServer'

/****** Object:  Table [dbo].[TrustedServers]    Script Date: 07/18/2008 11:56:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TrustedServers]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TrustedServers](
	[LastModified] [datetime] NOT NULL,
	[IDServer] [dbo].[GuidType] NOT NULL,
	[Name] [dbo].[NameType] NOT NULL,
	[Address] [dbo].[UriType] NOT NULL,
 CONSTRAINT [PK_TrustedServers] PRIMARY KEY CLUSTERED 
(
	[IDServer] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[SecurityPermissionsTrustedServer]    Script Date: 07/18/2008 11:56:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SecurityPermissionsTrustedServer]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SecurityPermissionsTrustedServer](
	[LastModified] [datetime] NOT NULL,
	[SecurityAttributeID] [dbo].[InternalIdType] NOT NULL,
	[IDRole] [dbo].[GuidType] NOT NULL,
	[IDServer] [dbo].[GuidType] NOT NULL,
	[Value] [dbo].[SecurityValueType] NULL,
 CONSTRAINT [PK_SecurityPermissionsTrustedServer] PRIMARY KEY CLUSTERED 
(
	[SecurityAttributeID] ASC,
	[IDRole] ASC,
	[IDServer] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
