-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[PerfomanceCounterType]') AND type = 'U')
BEGIN
CREATE TABLE [PerfomanceCounterType]
(
	[counter_type_id] INT IDENTITY(1,1) PRIMARY KEY,
	[counter_direction] BIT NOT NULL,
	[counter_type] INT NOT NULL,
	[category_name] NVARCHAR(50) NULL,
	[counter_name] NVARCHAR(50) NULL

) ON [PRIMARY]

	INSERT [PerfomanceCounterType]([counter_direction], [counter_type], [category_name], [counter_name])

	SELECT 1, 1, 'Processor', '% Processor Time' UNION ALL
	SELECT 0, 2, 'Memory', 'Available MBytes' UNION ALL
	
	SELECT 0, 3, 'VideoOS Recording Server Device', 'Media/sec perc' UNION ALL
	SELECT 0, 4, 'VideoOS Recording Server Drivers', 'Media/sec perc' UNION ALL
				
	SELECT 1, 5, 'VideoOS Recording Server Device Storage', 'Used Bytes' UNION ALL
	SELECT 0, 6, 'VideoOS Recording Server Database Disk', 'Free Bytes' UNION ALL

	--underfined types. not used for rule configurations 
	SELECT 1, 0, 'LogicalDisk', 'Disk Read Bytes/sec' UNION ALL
	SELECT 1, 0, 'LogicalDisk', 'Disk Write Bytes/sec' UNION ALL

	SELECT 1, 0, 'Network Interface', 'Bytes Received/sec' UNION ALL
	SELECT 1, 0, 'Network Interface', 'Bytes Sent/sec' UNION ALL
	SELECT 1, 0, 'Network Interface', 'Bytes Total/sec' UNION ALL
	SELECT 1, 0, 'VideoOS Recording Server Database Disk', 'Media Data Bytes' UNION ALL
	SELECT 1, 0, 'VideoOS Recording Server Database Disk', 'Other Data Bytes' UNION ALL
	SELECT 1, 0, 'VideoOS Recording Server Drivers', 'Bytes/sec'
	
	-- add performance counter for Recording FPS ('VideoOS Recording Server Database', 'Media/sec') and 
	--                             Live FPS ('VideoOS Recording Server Drivers', 'Media/sec')
	DECLARE @id INT
	DECLARE @getid CURSOR

	SET @getid = CURSOR FOR
	SELECT [DataCollector].[pkid]
	FROM   [DataCollector]

	OPEN @getid
	FETCH NEXT
	FROM @getid INTO @id
	WHILE @@FETCH_STATUS = 0
	BEGIN
		INSERT [DataCollectorConfiguration]([data_collector_id], [category_name], [counter_name], [instance_name], [sample_frequency])
		VALUES(@id,'VideoOS Recording Server Device', 'Media/sec perc', '', 300), 
				(@id,'VideoOS Recording Server Drivers', 'Media/sec perc', '', 300)

		FETCH NEXT
		FROM @getid INTO @id
	END

	IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'rule_type' and Object_ID = Object_ID(N'PerfomanceCounterStateRule'))
BEGIN
	ALTER TABLE [PerfomanceCounterStateRule]
	ADD [rule_type] INT NOT NULL DEFAULT 0

END
END
GO
