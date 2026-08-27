-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Create new index on PerformanceCounterInstance.
-- This is required for the following updates to complete in reasonable time
IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[PerformanceCounterInstance]') AND name = N'IX_PerformanceCounterInstanceCategory')
CREATE NONCLUSTERED INDEX [IX_PerformanceCounterInstanceCategory] ON [PerformanceCounterInstance]
(
	[category_name] ASC,
	[counter_name] ASC
)
INCLUDE
(
	[pkid],
	[instance_name]
)
GO

-- Create new index to support finding instances in PerformanceCounterValue
-- This is required for the following updates to complete in reasonable time
IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[PerformanceCounterValue]') AND name = N'IX_PerformanceCounterValueInstanceId')
CREATE NONCLUSTERED INDEX [IX_PerformanceCounterValueInstanceId] ON [PerformanceCounterValue]
(
	[instance_id] ASC,
	[collection_time] ASC
)
INCLUDE
(
	[calculated_value]
)
GO

-- Create a new index to increase performance counter value processing
IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[TempPerformanceCounterValue]') AND name = N'IX_SPID')
CREATE NONCLUSTERED INDEX [IX_SPID] ON [TempPerformanceCounterValue]
(
	[SPID] ASC
)
GO

SET DEADLOCK_PRIORITY HIGH;
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
GO

-- *************************************************
-- Now we're ready to remove the duplicate instances
-- *************************************************
BEGIN TRAN

	-- First ensure that we have an update lock on all duplicate counter instances we're going to delete
	SELECT * FROM TempPerformanceCounterValue WITH(TABLOCKX, HOLDLOCK)
	SELECT * FROM PerformanceCounterInstance WITH(TABLOCKX, HOLDLOCK)

	-- Delete performance counter values related to duplicate counters
	-- With the previous update lock we ensure that no counter values for the duplicate instances are inserted
	;WITH dup_values AS
	(
		SELECT
			pci.[pkid],
			SUBSTRING(pci.[instance_name], LEN(pci.[instance_name]) - 36, 36) AS oid,
			ROW_NUMBER() OVER (PARTITION BY pci.[data_collector_id], pci.[category_name], pci.[counter_name], SUBSTRING(pci.[instance_name], LEN(pci.[instance_name]) - 36, 36) ORDER BY pci.[pkid] DESC) AS lowest_id
		FROM [PerformanceCounterInstance] pci
		WHERE SUBSTRING([instance_name], LEN([instance_name]) - 37, 1) = '[' -- Ensure we have a ']' as the last character
			AND SUBSTRING([instance_name], LEN([instance_name]), 1) = ']' -- Ensure we have a '[' 37 characters before enclosing ']'
	)
	DELETE [PerformanceCounterValue]
	WHERE instance_id IN (SELECT [pkid] FROM dup_values WHERE lowest_id > 1)

	-- Now delete the duplicate performance counter instances
	;WITH dup_instances AS
	(
		SELECT
			pci.[pkid],
			SUBSTRING(pci.[instance_name], LEN(pci.[instance_name]) - 36, 36) AS oid,
			ROW_NUMBER() OVER (PARTITION BY pci.[data_collector_id], pci.[category_name], pci.[counter_name], SUBSTRING(pci.[instance_name], LEN(pci.[instance_name]) - 36, 36) ORDER BY pci.[pkid] DESC) AS lowest_id
		FROM [PerformanceCounterInstance] pci
		WHERE SUBSTRING([instance_name], LEN([instance_name]) - 37, 1) = '[' -- Ensure we have a ']' as the last character
			AND SUBSTRING([instance_name], LEN([instance_name]), 1) = ']' -- Ensure we have a '[' 37 characters before enclosing ']'
	)
	DELETE [PerformanceCounterInstance]
	WHERE pkid IN (SELECT pkid FROM dup_instances WHERE lowest_id > 1)

COMMIT TRAN
GO
