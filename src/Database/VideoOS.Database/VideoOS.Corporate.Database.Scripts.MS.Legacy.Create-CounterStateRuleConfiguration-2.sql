-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[PerformanceCounterStateRule]') AND type = 'U')
CREATE TABLE [PerformanceCounterStateRule]
(
	[counter_state_rule_id] [UNIQUEIDENTIFIER] NOT NULL,
	[critical_threshold] FLOAT NOT NULL,
	[warning_threshold] FLOAT NOT NULL,
	[time_window] int NOT NULL,
	[rule_name] NVARCHAR(50) NULL

	CONSTRAINT [PK_PerformanceCounterStateRule] PRIMARY KEY NONCLUSTERED
	(
		[counter_state_rule_id] ASC
	),

) ON [PRIMARY]

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[PerformanceCounterStateSource]') AND type in (N'U'))
CREATE TABLE [PerformanceCounterStateSource]
(
	[counter_state_rule_id] [UNIQUEIDENTIFIER] NOT NULL,
	[counter_id] int NOT NULL,
	[data_collector_id] int NOT NULL,
	[source_id] [UNIQUEIDENTIFIER] NOT NULL

	CONSTRAINT [PK_PerformanceCounterStateSource] PRIMARY KEY NONCLUSTERED
	(
		[source_id] ASC
	),
	
	CONSTRAINT [FK_PerformanceCounterStateSource_PerformanceCounterStateRule] FOREIGN KEY([counter_state_rule_id]) REFERENCES [PerformanceCounterStateRule] ([counter_state_rule_id]),
	
) ON [PRIMARY]


IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[PerformanceCounterStateOverride]') AND type in (N'U'))
CREATE TABLE [PerformanceCounterStateOverride]
(
	[source_id] [UNIQUEIDENTIFIER] NOT NULL,
	[critical_threshold] FLOAT NOT NULL,
	[warning_threshold] FLOAT NOT NULL,
	[time_window] int NOT NULL,

	CONSTRAINT [FK_PerformanceCounterStateOverride_PerformanceCounterStateSource] FOREIGN KEY([source_id]) REFERENCES [PerformanceCounterStateSource] ([source_id]),
	
) ON [PRIMARY]

GO
