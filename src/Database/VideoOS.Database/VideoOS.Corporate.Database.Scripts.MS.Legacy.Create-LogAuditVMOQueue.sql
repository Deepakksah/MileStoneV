---- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LogAuditVMOQueue]') AND type in (N'U'))
DROP TABLE [dbo].[LogAuditVMOQueue]
GO

CREATE TABLE [dbo].[LogAuditVMOQueue](
	[pkid] [bigint] IDENTITY(1,1) NOT NULL,
	[TimeStampUTC] [datetime] NOT NULL,
	[TimeStamp] [datetime] NOT NULL,
	[LogID] [int] NOT NULL,
	[LogLevel] [varchar](128) NOT NULL,
	[User] [dbo].[UserIdType] NOT NULL,
	[AuditType] [varchar](128) NOT NULL,
	[AccessGranted] [varchar](128) NOT NULL,
	[UserLocation] [dbo].[UriType] NOT NULL,
	[ResourceType] [varchar](128) NOT NULL,
	[ResourceName] [nvarchar](512) NOT NULL,
	[ResourceLocation] [nvarchar](512) NOT NULL,
	[ResourceID] [dbo].[GuidType] NOT NULL,
	[Argument0] [nvarchar](512) NULL,
	[Argument1] [nvarchar](512) NULL,
	[Argument2] [nvarchar](512) NULL,
	[Argument3] [nvarchar](512) NULL,
	[Argument4] [nvarchar](512) NULL,
	[Argument5] [nvarchar](512) NULL,
	[Argument6] [nvarchar](512) NULL,
 CONSTRAINT [PK_LogAuditVMOQueue] PRIMARY KEY NONCLUSTERED 
(
	[pkid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

