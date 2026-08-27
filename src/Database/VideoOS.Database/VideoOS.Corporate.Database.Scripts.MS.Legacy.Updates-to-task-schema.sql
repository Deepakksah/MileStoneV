-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Task]') AND type in (N'U'))
DROP TABLE [Task]
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Task]') AND type in (N'U'))
CREATE TABLE [Task] (
	[pkid] [bigint] IDENTITY(1,1) NOT NULL,
	[type] VARCHAR(50) NOT NULL,
	[target] [GuidType] NOT NULL,
	[data_a] XML NOT NULL,
	[data_b] XML NULL,
	[percent_completed] INT NOT NULL, -- 0-100
	[status_details] NVARCHAR(MAX) NOT NULL, -- Details e.g. in case of failure e.g. enum/type/string defining the details
	[task_status] INT NOT NULL, -- 0: Not Started, 1: Running, 2: Failed, 3: Completed, 4: Completed with errors
	[start_time] DATETIME NOT NULL,
	[end_time] DATETIME NULL,

	CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED
	(
		[pkid]
	)
)
GO

-- Type:
--  diffgram_insert
--  diffgram_update
--  diffgram_delete
--  task
