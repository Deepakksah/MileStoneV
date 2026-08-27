-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- The SUPPORTS_TRANSACTION property specifies if the scripts supports
-- running in multi-statement transaction or if it should be running
-- using implicit transactions
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Add new calculated column for id of the instance
IF NOT EXISTS (SELECT * FROM sys.columns WHERE [object_id] = OBJECT_ID(N'PerformanceCounterInstance') AND name = N'counter_id')
ALTER TABLE [PerformanceCounterInstance]
ADD	[counter_id] AS (CASE WHEN 
														SUBSTRING(REVERSE([instance_name]), 1, 1) = ']' -- Ensure we have a ']' as the last character
												AND SUBSTRING(REVERSE([instance_name]), 38, 1) = '[' -- Ensure we have a '[' 37 characters before enclosing ']'
												AND SUBSTRING([instance_name], LEN([instance_name]) - 36, 36) LIKE '[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]-[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]-[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]-[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]-[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]'
											THEN SUBSTRING([instance_name], LEN([instance_name]) - 36, 36)
											ELSE NULL
											END)
GO

-- Create an index on the counter id column
IF NOT EXISTS(SELECT * FROM sys.indexes WHERE [object_id] = OBJECT_ID(N'[PerformanceCounterInstance]') AND name = N'IX_Counter_ID')
CREATE NONCLUSTERED INDEX [IX_Counter_ID] ON [PerformanceCounterInstance]
(
	[counter_id] ASC
)
GO

-- Drop the existing index on path
IF EXISTS(SELECT * FROM sys.indexes WHERE [object_id] = OBJECT_ID(N'[PerformanceCounterInstance]') AND name = N'IX_PerformanceCounterInstancePath')
ALTER TABLE [PerformanceCounterInstance]
DROP CONSTRAINT [IX_PerformanceCounterInstancePath]
GO

-- Drop the path column - will be created as a computed column
IF EXISTS(SELECT * FROM sys.columns WHERE [object_id] = OBJECT_ID(N'PerformanceCounterInstance') AND name = N'path' AND is_computed = 0)
ALTER TABLE [PerformanceCounterInstance]
DROP COLUMN [path]

-- Add new calculated column for path of the counter
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [object_id] = OBJECT_ID(N'PerformanceCounterInstance') AND name = N'path' AND is_computed = 1)
ALTER TABLE [PerformanceCounterInstance]
ADD	[path] AS (CASE 
								WHEN [instance_name] = ''
									THEN '\' + [category_name] + '\' + [counter_name]
									ELSE '\' + [category_name] + '(' + [instance_name] + ')\' + [counter_name]
								END)
GO

-- We don't need this index anymore
IF EXISTS(SELECT * FROM sys.indexes WHERE [object_id] = OBJECT_ID(N'[PerformanceCounterInstance]') AND name = N'IX_PerformanceCounterInstanceNames')
ALTER TABLE [PerformanceCounterInstance] 
DROP CONSTRAINT [IX_PerformanceCounterInstanceNames]
GO

-- Create new clustered primary key on PerformanceCounterInstance
IF NOT EXISTS(SELECT * FROM sys.indexes WHERE [object_id] = OBJECT_ID(N'[PerformanceCounterInstance]') AND name = N'IX_DataCollectorId')
CREATE CLUSTERED INDEX [IX_DataCollectorId] ON [PerformanceCounterInstance]
(
	[data_collector_id]
)
GO

-- Create non-clustered index to identify instances including the primary key
IF NOT EXISTS(SELECT * FROM sys.indexes WHERE [object_id] = OBJECT_ID(N'[PerformanceCounterInstance]') AND name = N'IX_DataCollectorData')
CREATE NONCLUSTERED INDEX [IX_DataCollectorData] ON [dbo].[PerformanceCounterInstance]
(
	[data_collector_id],
	[category_name],
	[counter_name],
	[instance_name]
)
INCLUDE ([pkid])
GO

IF EXISTS(SELECT * FROM sys.tables WHERE [object_id] = OBJECT_ID(N'[TempPerformanceCounterValue]') AND type IN ('U'))
DROP TABLE [dbo].[TempPerformanceCounterValue]
GO
