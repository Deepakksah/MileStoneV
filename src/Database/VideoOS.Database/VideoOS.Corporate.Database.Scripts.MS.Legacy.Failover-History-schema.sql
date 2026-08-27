-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FailoverHistory]') AND type = 'U')
CREATE TABLE [FailoverHistory] (
	[IDFailoverHistoryEntry] [GuidType] NOT NULL,
	[IDFailover] [GuidType] NOT NULL,
	[Description] [DescriptionType],
	[IDRecorder] [GuidType] NULL,
	[StartTime] [datetime],
	[EndTime] [datetime],
	[LastModified] [datetime] NOT NULL

	CONSTRAINT [PK_FailoverHistory] PRIMARY KEY CLUSTERED
	(
		[IDFailoverHistoryEntry]
	)
) ON [PRIMARY]
GO
