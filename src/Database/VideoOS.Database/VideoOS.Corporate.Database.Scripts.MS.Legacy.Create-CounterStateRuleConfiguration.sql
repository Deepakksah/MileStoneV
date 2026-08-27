-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[PerfomanceCounterStateRule]') AND type = 'U')
CREATE TABLE [PerfomanceCounterStateRule]
(
	[counter_state_rule_id] [UNIQUEIDENTIFIER] NOT NULL,
	[critical_threshold] FLOAT NOT NULL,
	[warning_threshold] FLOAT NOT NULL,
	[time_window] int NOT NULL,
	[rule_name] NVARCHAR(50) NULL

	CONSTRAINT [PK_PerfomanceCounterStateRule] PRIMARY KEY NONCLUSTERED
	(
		[counter_state_rule_id] ASC
	),

) ON [PRIMARY]

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[PerfomanceCounterStateSource]') AND type in (N'U'))
CREATE TABLE [PerfomanceCounterStateSource]
(
	[counter_state_rule_id] [UNIQUEIDENTIFIER] NOT NULL,
	[counter_id] int NOT NULL,
	[data_collector_id] int NOT NULL,
	[source_id] [UNIQUEIDENTIFIER] NOT NULL

	CONSTRAINT [PK_PerfomanceCounterStateSource] PRIMARY KEY NONCLUSTERED
	(
		[source_id] ASC
	),
	
	CONSTRAINT [FK_PerfomanceCounterStateSource_PerfomanceCounterStateRule] FOREIGN KEY([counter_state_rule_id]) REFERENCES [PerfomanceCounterStateRule] ([counter_state_rule_id]),
	
) ON [PRIMARY]


IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[PerfomanceCounterStateOverride]') AND type in (N'U'))
CREATE TABLE [PerfomanceCounterStateOverride]
(
	[source_id] [UNIQUEIDENTIFIER] NOT NULL,
	[critical_threshold] FLOAT NOT NULL,
	[warning_threshold] FLOAT NOT NULL,
	[time_window] int NOT NULL,

	CONSTRAINT [FK_PerfomanceCounterStateOverride_PerfomanceCounterStateSource] FOREIGN KEY([source_id]) REFERENCES [PerfomanceCounterStateSource] ([source_id]),
	
) ON [PRIMARY]

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'counter_direction' and Object_ID = Object_ID(N'DataCollectorConfiguration'))
BEGIN
	ALTER TABLE [DataCollectorConfiguration]
	ADD [counter_direction] BIT NOT NULL DEFAULT 1

END
GO
	
UPDATE [DataCollectorConfiguration]
SET [DataCollectorConfiguration].[counter_direction] = 0	
WHERE [counter_name] = 'Available MBytes' OR [counter_name] = 'Free Bytes'
GO
