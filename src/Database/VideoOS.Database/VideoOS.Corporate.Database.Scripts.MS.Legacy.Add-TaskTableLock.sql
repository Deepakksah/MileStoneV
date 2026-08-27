-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[TaskTableLock]') AND type = 'U')
BEGIN
	CREATE TABLE [TaskTableLock]
	(
		[LockLevel] [int] NOT NULL,
		[LastUpdated] [datetime] NOT NULL,

		CONSTRAINT [PK_TaskTableLock] PRIMARY KEY CLUSTERED 
		(
			[LockLevel] ASC
		)
	) ON [PRIMARY]
END
GO

IF NOT EXISTS(SELECT * FROM [TaskTableLock] WHERE [LockLevel] = 0)
BEGIN
	INSERT INTO [TaskTableLock] ([LockLevel], [LastUpdated])
	VALUES(0, GETUTCDATE())
END
GO
