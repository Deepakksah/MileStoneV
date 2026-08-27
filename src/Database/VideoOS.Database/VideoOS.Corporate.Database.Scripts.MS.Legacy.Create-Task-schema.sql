-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Task]') AND type = 'U')
CREATE TABLE [Task]
(
	[SequenceNumber] [bigint] IDENTITY(1,1) NOT NULL,
	[pkid] [dbo].[GuidType] NOT NULL,
	[Type] [dbo].[InternalIdType] NOT NULL,
	[Target] [dbo].[GuidType] NOT NULL,
	[DataA] [XML] NOT NULL,
	[DataB] [XML] NULL,
	[StatusProgress] [tinyint] NOT NULL, -- 0-100
	[StatusDetails] [nvarchar](max) NOT NULL, -- Details e.g. in case of failure e.g. enum/type/string defining the details
	[LastModified] [datetime] NOT NULL

	CONSTRAINT [PK_Task] PRIMARY KEY NONCLUSTERED
	(
		[pkid]
	),
) ON [PRIMARY]

GO