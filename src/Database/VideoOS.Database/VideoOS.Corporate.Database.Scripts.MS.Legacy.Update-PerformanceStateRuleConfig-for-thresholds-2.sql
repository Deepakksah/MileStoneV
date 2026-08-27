-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

BEGIN
	--update values for Retention time
	UPDATE [PerformanceCounterStateRule]
	SET [critical_threshold] = 3,
		[warning_threshold] = 10,
		[disabled] =  1
	WHERE [PerformanceCounterStateRule].[counter_state_rule_id] = 'E9557CFD-6CB1-40C8-822E-84E7E8CA5171'
END
	
GO