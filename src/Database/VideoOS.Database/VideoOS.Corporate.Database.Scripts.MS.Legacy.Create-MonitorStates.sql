-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[MonitorStates]') AND type = 'U')
CREATE TABLE [MonitorStates]
(
	[counter_state_rule_id] NVARCHAR(100) NULL,
	[counter_path] NVARCHAR(364) NULL,
	[data_collector_id] int NOT NULL,
	[state] int NOT NULL


) ON [PRIMARY]

GO
