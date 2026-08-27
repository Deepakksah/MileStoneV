-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_server_monitor_states_by_datacollectors]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_server_monitor_states_by_datacollectors]
GO

CREATE PROCEDURE [query_server_monitor_states_by_datacollectors]
	@DataCollectors tvp_StringTableGuidType READONLY,
	@EventTypes tvp_StringTableGuidType READONLY
AS
BEGIN
	SELECT ms.[data_collector_id] AS DataCollectorId, 
		pcsr.[rule_type] AS EventType, 
		ms.[state] AS CounterState,
		ms.[counter_id] AS InstanceId,
		ms.[trigger_value] as TriggerValue
	FROM MonitorStates ms 
	INNER JOIN @DataCollectors dc ON dc.[ID] = ms.[data_collector_id]
	INNER JOIN dbo.PerformanceCounterStateRule pcsr ON pcsr.[counter_state_rule_id] = ms.[counter_state_rule_id]
	INNER JOIN @EventTypes et ON  et.[ID] = pcsr.[rule_type]
	WHERE  ms.[is_monitored] = 1
END
GO