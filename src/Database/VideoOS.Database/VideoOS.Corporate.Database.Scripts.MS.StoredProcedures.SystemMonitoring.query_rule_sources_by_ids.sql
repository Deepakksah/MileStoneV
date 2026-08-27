-- This header is used by the dbupdate tool]
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on] The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes]
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================


IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_rule_sources_by_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_rule_sources_by_ids]
GO

CREATE PROCEDURE [query_rule_sources_by_ids]
	@Ids tvp_StringTableGuidType READONLY,
	@DataCollectorId INT,
	@RuleConfigIds tvp_StringTableGuidType READONLY
AS

BEGIN
	DECLARE @SourceIds TABLE(ID GuidType)

	IF ((SELECT COUNT(ID) FROM @Ids) = 0)
	BEGIN
		INSERT INTO @SourceIds(ID)
		SELECT [PerformanceCounterStateSource].[source_id] as Id
		FROM [PerformanceCounterStateSource]
	END
	ELSE
	BEGIN
		INSERT INTO @SourceIds(ID)
		SELECT [ID]
		FROM @Ids
	END

	DECLARE @Empty NVARCHAR(5)
	SET @Empty = ''

	SELECT ps.source_id,
	   ps.counter_id, 
	   pin.path,	   
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

	LEFT JOIN [PerformanceCounterInstance] pin
	ON pin.pkid = ps.counter_id

	LEFT JOIN [PerformanceCounterStateRule] pr
	ON ps.counter_state_rule_id = pr.counter_state_rule_id

	INNER JOIN [PerformanceCounterType] pt
	ON pt.counter_type = pr.rule_type

	LEFT JOIN @SourceIds as ri
	ON ps.source_id = ri.[ID] 

	WHERE ps.data_collector_id = @DataCollectorId OR @DataCollectorId = -1 OR ps.data_collector_id = 0

	UNION ALL
	SELECT source_id = NULL, 
		  pin.pkid, 
		  pin.path, 
		  pin.data_collector_id, 
		  os.critical_threshold, 
		  os.warning_threshold, 
		  os.time_window, 
		  pr.counter_state_rule_id, 
		  pr.rule_type, 
		  pt.category_name, 
		  pt.counter_name,
		  -1 as unit
	FROM PerformanceCounterStateRule AS pr 
	INNER JOIN [PerformanceCounterType] pt
		ON pt.counter_type = pr.rule_type
	INNER JOIN @RuleConfigIds as ri
		ON ri.[ID] = pr.rule_type
	INNER JOIN [PerformanceCounterInstance] AS pin
		ON (pin.data_collector_id = @DataCollectorId OR @DataCollectorId = -1) AND 
		pt.category_name = pin.category_name AND
		(pt.counter_name = pin.[counter_name] OR pt.counter_name IS NULL) 
	LEFT JOIN [PerformanceCounterStateSource] ps
	ON ps.counter_id = pin.pkid

	LEFT JOIN [PerformanceCounterStateOverride] as os
	ON os.[source_id] = ps.[source_id] AND (@DataCollectorId = ps.[data_collector_id] OR @DataCollectorId = -1) 

	WHERE os.critical_threshold = -1 OR os.critical_threshold IS NULL 
END
GO