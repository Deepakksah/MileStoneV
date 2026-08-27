-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DELETE MS FROM [dbo].[MonitorStates] MS
JOIN
(
	SELECT [counter_path], [data_collector_id], [counter_state_rule_id]
	FROM [dbo].[MonitorStates] 
	GROUP BY [counter_path], [data_collector_id], [counter_state_rule_id]
	HAVING count(state) > 1
) DT
ON DT.counter_path = MS.counter_path
AND DT.counter_state_rule_id = MS.counter_state_rule_id
AND DT.data_collector_id = MS.data_collector_id

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[MonitorStates]') AND name = N'PRIMARY_MonitorStates')
BEGIN
	CREATE NONCLUSTERED INDEX [PRIMARY_MonitorStates] ON [dbo].[MonitorStates]
	(
		[counter_state_rule_id] ASC,
		[counter_path] ASC,
		[data_collector_id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[MonitorStates]') AND name = N'IX_MonitorStates_IsMonitored')
BEGIN
	CREATE NONCLUSTERED INDEX [IX_MonitorStates_IsMonitored] ON [dbo].[MonitorStates]
	(
		[counter_path] ASC,
		[data_collector_id] ASC,
		[is_monitored] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END
GO
