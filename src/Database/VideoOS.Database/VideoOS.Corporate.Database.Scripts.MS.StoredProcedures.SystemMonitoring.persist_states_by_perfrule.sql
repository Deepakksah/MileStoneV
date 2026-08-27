-- This header is used by the dbupdate tool]
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on] The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes]
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================


IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[persist_states_by_perfrule]') AND type in (N'P', N'PC'))
DROP PROCEDURE [persist_states_by_perfrule]
GO

CREATE PROCEDURE [persist_states_by_perfrule]
	@NewState int,
	@CounterRuleId [UNIQUEIDENTIFIER]
AS
BEGIN
	SET NOCOUNT ON
	
	UPDATE [MonitorStates]
	SET state = @NewState 
	WHERE counter_state_rule_id = @CounterRuleId
		
	SELECT
		[CounterPath] = m.counter_path,
		[CounterRuleId] = m.counter_state_rule_id,
		[DataCollectorId] = m.[data_collector_id],
		[State] = m.[state],
		[TriggerValue] = m.[trigger_value]
	FROM [MonitorStates] m
	WHERE [counter_state_rule_id] = @CounterRuleId	
END
GO