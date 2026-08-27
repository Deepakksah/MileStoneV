-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_camera_monitor_states_by_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_camera_monitor_states_by_ids]
GO

CREATE PROCEDURE [query_camera_monitor_states_by_ids]
	@EventTypes tvp_StringTableGuidType READONLY,
	@CameraIds tvp_StringTableGuidType READONLY
AS
BEGIN
	SELECT pcsr.[rule_type] AS EventType, 
		ci.[Id] AS CameraId,
		ms.[state] AS CounterState, 
		ms.[data_collector_id] as DataCollectorId,
		ms.[trigger_value] as TriggerValue

	FROM dbo.PerformanceCounterStateRule pcsr 
	INNER JOIN @EventTypes et ON et.[Id] = pcsr.[rule_type]
	INNER JOIN MonitorStates as ms ON pcsr.[counter_state_rule_id] = ms.[counter_state_rule_id] 
	INNER JOIN @CameraIds ci ON ms.[counter_id] = ci.[Id] COLLATE SQL_Latin1_General_CP1_CI_AS
	WHERE ms.is_monitored = 1
END
GO