-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_perfcounter_initvalues]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_perfcounter_initvalues]
GO

CREATE PROCEDURE [query_perfcounter_initvalues]
	@CounterStateRuleId [UNIQUEIDENTIFIER]
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @ruleType INT

	SELECT @ruleType = p.[rule_type] 		
	FROM [PerformanceCounterStateRule] as p 		
	WHERE p.counter_state_rule_id = @CounterStateRuleId
	
	DECLARE @CriticalValue FLOAT
	DECLARE @WarningValue FLOAT

	DECLARE @countCameras INT
	SET @countCameras = (SELECT COUNT(d.IDDevice) FROM Devices as d WHERE d.DeviceType = 'Camera') 

	DECLARE @mbPerCamera BIGINT
	SELECT @mbPerCamera = (SELECT MAX(storages.ts) FROM (SELECT rs.max_size_mb as ts FROM [RecordingStorage] rs) as storages) / 
								CASE @countCameras 
									WHEN 0 THEN 1
									ELSE @countCameras
								END
								
	DECLARE @TotalPhysMb BIGINT
	SELECT @TotalPhysMb = MAX(MachineProperties.value) / (1024 * 1024)
	FROM MachineProperties
	WHERE name = 'ullTotalPhys'

	DECLARE @MaxStorageSize BIGINT
	SELECT @MaxStorageSize = MAX(disks.ts) 
									 FROM (
											SELECT disk.value('@Size', 'bigint') as ts
											FROM [RecorderStatusData] rsd
											CROSS apply rsd.Data.nodes('DiskUsage/Disk') Disks(disk)
											WHERE rsd.Type = 'DiskUsage') as disks

	SELECT @CriticalValue = CASE @ruleType
			-- Used space
			WHEN 5 THEN  
					-- set to 90 percent from allowed per camera in butes
					COALESCE(CAST(@mbPerCamera AS FLOAT) * 1024 * 1024 * 0.9, -1)

			-- free space
			WHEN 6 THEN 
					-- set to 5 percent from max
					COALESCE(@MaxStorageSize * 0.05, -1)

			-- Memory available
			WHEN 2 THEN 
					-- set to 5 percent of total
					COALESCE(@TotalPhysMb * 0.05, -1)
			ELSE 0
	END
		
	SELECT @WarningValue = 
		CASE @ruleType
			-- Used space
			WHEN 5 THEN  
					-- set to 70 percent from allowed per camera in bytes
					COALESCE(CAST(@mbPerCamera AS FLOAT) * 1024 * 1024 * 0.7, -1)

			--Free space
			WHEN 6 THEN 
					-- set to 20 percent from max 
					COALESCE(@MaxStorageSize * 0.2, -1)

			-- Memory available
			WHEN 2 THEN 
					-- set to 10 percent of total
					COALESCE(@TotalPhysMb * 0.1, -1)
			ELSE 0
	END
	
	SELECT @CriticalValue as CriticalValue, @WarningValue as WarningValue
END
GO