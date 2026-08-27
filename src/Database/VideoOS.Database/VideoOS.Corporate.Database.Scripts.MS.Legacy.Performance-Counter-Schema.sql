-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[DataCollector]') AND type in (N'U'))
BEGIN
	CREATE TABLE [DataCollector] (
		[pkid] INT IDENTITY(1,1) NOT NULL,
		[host_name] NVARCHAR(200) NOT NULL,
		
		CONSTRAINT [PK_DataCollector] PRIMARY KEY CLUSTERED
		(
			[pkid] ASC
		),
		
		CONSTRAINT [UN_DataCollector_HostName] UNIQUE NONCLUSTERED
		(
			[host_name] ASC
		)
	)
END
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[DataCollectorConfiguration]') AND type in (N'U'))
BEGIN
	CREATE TABLE [DataCollectorConfiguration] (
		[data_collector_id] INT NOT NULL,
		[category_name] NVARCHAR(120) NOT NULL,
		[counter_name] NVARCHAR(120) NOT NULL,
		[instance_name] NVARCHAR(120) NOT NULL,
		[sample_frequency] INT NOT NULL,
		
		CONSTRAINT [PK_DataCollectorConfiguration] PRIMARY KEY CLUSTERED
		(
			[data_collector_id] ASC,
			[category_name] ASC,
			[counter_name] ASC,
			[instance_name] ASC
		),

		CONSTRAINT [FK_DataCollectorConfiguration_DataCollector] FOREIGN KEY
		(
			[data_collector_id]
		)
		REFERENCES [DataCollector] ( [pkid] )
	)
END
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[PerformanceCounterInstance]') AND type in (N'U'))
BEGIN
	CREATE TABLE [PerformanceCounterInstance] (
		[pkid] INT IDENTITY(1,1) NOT NULL,
		[data_collector_id] INT NOT NULL,
		[path] NVARCHAR(400) NOT NULL,
		[category_name] NVARCHAR(120) NOT NULL,
		[counter_name] NVARCHAR(120) NOT NULL,
		[instance_name] NVARCHAR(120) NULL,
		[counter_type] INT NOT NULL,

		CONSTRAINT [PK_PerformanceCounterInstance] PRIMARY KEY NONCLUSTERED
		(
			[pkid] ASC
		),

		CONSTRAINT [FK_PerformanceCounterInstance_DataCollector] FOREIGN KEY
		(
			[data_collector_id]
		)
		REFERENCES [DataCollector] ( [pkid] )
	)
END
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[PerformanceCounterValue]') AND type in (N'U'))
BEGIN
	CREATE TABLE [PerformanceCounterValue] (
		[instance_id] INT NOT NULL,
		[collection_time] DATETIME NOT NULL,
		[raw_value] BIGINT NOT NULL,
		[calculated_value] FLOAT NOT NULL,

		CONSTRAINT [PK_performance_counter_value] PRIMARY KEY CLUSTERED 
		(
			[collection_time] ASC,
			[instance_id] ASC
		),

		CONSTRAINT [FK_PerformanceCounterValue_PerformanceCounterInstance] FOREIGN KEY
		(
			[instance_id]
		)
		REFERENCES [PerformanceCounterInstance] ( [pkid] )
	)
END
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[TempPerformanceCounterValue]') AND type in (N'U'))
BEGIN
	CREATE TABLE [TempPerformanceCounterValue] (
		[SPID] INT NOT NULL,
		[category_name] NVARCHAR(120) NOT NULL,
		[counter_name] NVARCHAR(120) NOT NULL,
		[instance_name] NVARCHAR(120) NULL,
		[collection_time] DATETIME NOT NULL,
		[raw_value] BIGINT NOT NULL,
		[calculated_value] FLOAT NOT NULL,
	)
END
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ServerType]') AND type in (N'U'))
BEGIN
	CREATE TABLE [ServerType] (
		[pkid] INT NOT NULL,
		[name] NVARCHAR(50) NOT NULL,
		
		CONSTRAINT [PK_server_type] PRIMARY KEY NONCLUSTERED
		(
			[pkid] ASC
		),

		CONSTRAINT [UN_server_type_name] UNIQUE CLUSTERED
		(
			[name] ASC
		)
	)
END
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[DataCollectorInstalledServices]') AND type in (N'U'))
BEGIN
	CREATE TABLE [DataCollectorInstalledServices]
	(
		[data_collector_id] INT NOT NULL,
		[server_type_id] INT NOT NULL,
		
		CONSTRAINT [PK_data_collector_installed_services] PRIMARY KEY CLUSTERED
		(
			[data_collector_id] ASC,
			[server_type_id] ASC
		),
		
		CONSTRAINT [FK_DataCollectorInstalledServices_DataCollector] FOREIGN KEY
		(
			[data_collector_id]
		)
		REFERENCES [DataCollector] ( [pkid] ),
		
		CONSTRAINT [FK_DataCollectorInstalledServices_ServerType] FOREIGN KEY
		(
			[server_type_id]
		)
		REFERENCES [ServerType] ( [pkid] )
	)
END
GO
