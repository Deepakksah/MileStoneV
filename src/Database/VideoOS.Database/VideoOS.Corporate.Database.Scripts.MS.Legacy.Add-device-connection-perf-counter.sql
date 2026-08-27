-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
	
IF NOT EXISTS ( SELECT *  FROM sys.columns  WHERE object_id = OBJECT_ID(N'[PerformanceCounterType]') AND name = N'enable_history')
BEGIN
	ALTER TABLE [PerformanceCounterType]
	ADD [enable_history] BIT NOT NULL DEFAULT 1
END

IF NOT EXISTS ( SELECT *  FROM sys.columns  WHERE object_id = OBJECT_ID(N'[PerformanceCounterType]') AND name = N'trigger_per_value')
BEGIN
	ALTER TABLE [PerformanceCounterType]
	ADD [trigger_per_value] BIT NOT NULL DEFAULT 0
END

IF NOT EXISTS ( SELECT *  FROM sys.columns  WHERE object_id = OBJECT_ID(N'[MonitorStates]') AND name = N'trigger_value')
BEGIN
	ALTER TABLE [MonitorStates]
	ADD [trigger_value] FLOAT NOT NULL DEFAULT 0
END
GO
