-- This header is used by the dbupdate tool]
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on] The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes]
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_perfcounter_state_rule_items]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_perfcounter_state_rule_items]
GO

CREATE PROCEDURE [query_perfcounter_state_rule_items]
	@Ids tvp_StringTableGuidType READONLY,
	@DataCollectorId INT
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @StateRuleIds TABLE(ID GuidType)

	IF ((SELECT COUNT(ID) FROM @Ids) = 0)
	BEGIN
		INSERT INTO @StateRuleIds(ID)
		SELECT PerformanceCounterStateRule.counter_state_rule_id
		FROM PerformanceCounterStateRule
	END
	ELSE
	BEGIN
		INSERT INTO @StateRuleIds(ID)
		SELECT [ID]
		FROM @Ids
	END

	SELECT b.[counter_state_rule_id]
		, b.[critical_threshold]
		, b.[warning_threshold]  
		, b.[time_window]
		, b.[rule_name]
		, b.[rule_type]
		, b.[disabled]	
		, (SELECT [PerformanceCounterType].[counter_direction] 
		   FROM [PerformanceCounterType]
		   WHERE [PerformanceCounterType].[counter_type] = b.[rule_type]) as direction	
		,  unit = CAST(CASE b.[rule_type]
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
		   				   
		FROM [PerformanceCounterStateRule] b
		INNER JOIN @StateRuleIds ids
		ON b.counter_state_rule_id = ids.[ID] 
		INNER JOIN [PerformanceCounterType] as pt
		ON pt.counter_type = b.[rule_type]
END
GO