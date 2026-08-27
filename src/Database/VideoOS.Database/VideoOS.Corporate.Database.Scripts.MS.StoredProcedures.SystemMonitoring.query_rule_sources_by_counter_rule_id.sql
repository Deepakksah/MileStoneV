-- This header is used by the dbupdate tool]
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on] The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes]
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================


IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_rule_sources_by_counter_rule_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_rule_sources_by_counter_rule_id]
GO

CREATE PROCEDURE [query_rule_sources_by_counter_rule_id]
	@CounterRuleId [UNIQUEIDENTIFIER]
AS
BEGIN
	SET NOCOUNT ON

	SELECT ps.source_id,
	   ps.counter_id, 
	   pinst.path,
	   ps.data_collector_id,
	   os.[critical_threshold] as critical_threshold,
	   os.[warning_threshold] as warning_threshold,
	   os.time_window,
	   ps.counter_state_rule_id,
	   pr.rule_type,
	   pt.category_name,
	   pt.counter_name,
	   unit = CAST(CASE pr.[rule_type]
						WHEN 1 THEN  100
						WHEN 2 THEN (SELECT MAX(MachineProperties.value) / (1024 * 1024)
										FROM MachineProperties
										WHERE name = 'ullTotalPhys')
						WHEN 3 THEN 100
						WHEN 4 THEN 100
						WHEN 5 THEN (SELECT MAX(storages.ts) 
									 FROM (
											SELECT rs.max_size_mb * 1024 * 1024 as ts
											FROM [RecordingStorage] rs) as storages)
						WHEN 6 THEN (SELECT MAX(disks.ts) 
									 FROM (
											SELECT disk.value('@Size', 'bigint') as ts
											FROM [RecorderStatusData] rsd
											CROSS apply rsd.Data.nodes('DiskUsage/Disk') Disks(disk)
											WHERE rsd.Type = 'DiskUsage') as disks)
						WHEN 7 THEN 30
						WHEN 8 THEN 0
					END AS BIGINT)
	FROM [PerformanceCounterStateSource] ps
	LEFT JOIN [PerformanceCounterStateOverride] as os
	ON os.[source_id] = ps.[source_id]
	INNER JOIN PerformanceCounterInstance pinst
	ON pinst.pkid = ps.counter_id
	INNER JOIN PerformanceCounterStateRule pr
	ON pr.counter_state_rule_id = ps.counter_state_rule_id
	INNER JOIN [PerformanceCounterType] pt
	ON pt.counter_type = pr.rule_type
	WHERE pr.counter_state_rule_id = @CounterRuleId		
END
GO