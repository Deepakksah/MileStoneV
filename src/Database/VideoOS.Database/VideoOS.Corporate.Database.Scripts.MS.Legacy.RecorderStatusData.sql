-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RecorderStatusData]') AND type = 'U')
CREATE TABLE [RecorderStatusData]
(
	[IDRecorder] [dbo].[GuidType] NOT NULL,
	[Type] [varchar](256) NOT NULL,
	[Data] [xml] NOT NULL,
	[LastModified] [datetime] NOT NULL,
	
	CONSTRAINT [PK_RecorderStatusData] PRIMARY KEY NONCLUSTERED 
	(
		[IDRecorder],
		[Type]
	),
	
	CONSTRAINT [FK_RecorderStatusData_Recorders] FOREIGN KEY
	(
		[IDRecorder]
	)
	REFERENCES [Recorders] ([IDRecorder]) ON DELETE CASCADE,
	
) ON [PRIMARY]

GO
