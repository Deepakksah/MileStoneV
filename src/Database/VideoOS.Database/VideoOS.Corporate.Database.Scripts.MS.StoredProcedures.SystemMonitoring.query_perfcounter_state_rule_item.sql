-- This header is used by the dbupdate tool]
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on] The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes]
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================


IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_perfcounter_state_rule_item]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_perfcounter_state_rule_item]
GO

CREATE PROCEDURE [query_perfcounter_state_rule_item]
	@CounterStateRuleId [UNIQUEIDENTIFIER]
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @categoryName nvarchar(50)
	DECLARE @counterName nvarchar(50)

	SELECT @categoryName = pt.[category_name],
		@counterName = pt.[counter_name]			
	FROM [PerformanceCounterType] as pt
	WHERE pt.[counter_type] = (SELECT p.[rule_type] 		
								  FROM [PerformanceCounterStateRule] as p 		
								  WHERE p.counter_state_rule_id = @CounterStateRuleId)


	SELECT
		[PerformanceCounterStateRule].[counter_state_rule_id] 
	    , [PerformanceCounterStateRule].[critical_threshold]
		, [PerformanceCounterStateRule].[warning_threshold]
		, [PerformanceCounterStateRule].[time_window]
		, [PerformanceCounterStateRule].[rule_name]
		, [PerformanceCounterStateRule].[rule_type]
		, [PerformanceCounterStateRule].[disabled]
		, (SELECT [PerformanceCounterType].[counter_direction] 
		   FROM [PerformanceCounterType] 
		   WHERE [PerformanceCounterType].[counter_type] = [PerformanceCounterStateRule].[rule_type]) as direction
		,  unit = CAST(CASE [PerformanceCounterStateRule].[rule_type]
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
		, pt.[category_name]	
		, pt.[counter_name]

	FROM [PerformanceCounterStateRule]
	INNER JOIN [dbo].[PerformanceCounterType] as pt
	ON pt.[counter_type] = [PerformanceCounterStateRule].[rule_type]
	WHERE 
		[PerformanceCounterStateRule].[counter_state_rule_id] = @CounterStateRuleId
END
GO