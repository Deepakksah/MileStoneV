-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_counters_state_rule_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_counters_state_rule_ids]
GO

CREATE PROCEDURE [query_counters_state_rule_ids]
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		CAST(PerformanceCounterStateRule.counter_state_rule_id as nvarchar(50)) as IdConfig
	FROM PerformanceCounterStateRule
END
GO