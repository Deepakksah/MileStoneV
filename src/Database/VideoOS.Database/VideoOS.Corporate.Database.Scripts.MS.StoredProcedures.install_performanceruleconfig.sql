-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_perfcounter_state_rule]') AND type in (N'P', N'PC'))
DROP PROCEDURE [create_perfcounter_state_rule]
GO

CREATE PROCEDURE [create_perfcounter_state_rule]
	@CounterStateRuleId [UNIQUEIDENTIFIER],
	@CriticalThreshold FLOAT,
	@WarningThreshold FLOAT,
	@TimeWindow [int],
	@Name NVARCHAR(50),
	@RuleType [int],
	@session_id [GuidType]
AS	
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRANSACTION

		INSERT [PerformanceCounterStateRule] (
		[counter_state_rule_id],
		[critical_threshold],
		[warning_threshold],
		[time_window],
		[rule_name],
		[rule_type],
		[disabled]
		)
		VALUES (
			@CounterStateRuleId,
			@CriticalThreshold,
			@WarningThreshold,
			@TimeWindow,
			@Name,
			@RuleType,
			0
		)

		COMMIT TRANSACTION

		IF (@session_id IS NOT NULL)
		BEGIN 
			EXEC create_pending_log @session_id, 22018, @CounterStateRuleId, @Name
		END
	END TRY
	BEGIN CATCH 
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION
		END;
		DECLARE @shouldReThrowError BIT = dbo.[ShouldReThrowError](ERROR_NUMBER(), ERROR_SEVERITY())
		IF(@shouldReThrowError = 1)
			THROW;
		SET @ErrorArgs = dbo.[FormatErrorMessage](ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE())
		;THROW 60000, @ErrorArgs, 1;
	END CATCH;
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_rule_source]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [create_rule_source]
END
GO
			
CREATE PROCEDURE [create_rule_source]
	@CounterRuleId [UNIQUEIDENTIFIER],
	@SourceId [UNIQUEIDENTIFIER],
	@DataCollectorId INT,
	@CounterId INT,
	@DeviceId NVARCHAR(50),
	@CriticalThreshold FLOAT,
	@WarningThreshold FLOAT,
	@TimeWindow [int]
AS	
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRANSACTION
	
		--device performance counter
		IF (@DataCollectorId IS NULL OR @DataCollectorId = 0)
		BEGIN
			SELECT  @CounterId = p.pkid, 
					@DataCollectorId = p.data_collector_id
			FROM [PerformanceCounterInstance] p 
			INNER JOIN [PerformanceCounterStateRule] pr
			ON pr.counter_state_rule_id = @CounterRuleId
			INNER JOIN [PerformanceCounterType] pt
			ON pr.rule_type = pt.counter_type
			AND p.category_name = pt.category_name 
			AND p.counter_name = pt.counter_name
			WHERE  p.is_obsolete = 0 AND p.[counter_id] = @DeviceId
		END
	
		IF (@DataCollectorId IS NULL)
			SET @DataCollectorId = 0

		IF (@CounterId IS NULL)
			SET @CounterId = 0

		-- if non default values were set then we need to make sure that counter id and datacollector id are set
		IF (@TimeWindow != -1 AND (@CounterId = 0 OR @DataCollectorId = 0))
		BEGIN
			;THROW 60000, @procedureName, 1;
		END
			
		--insert to PerformanceCounterStateSource
		INSERT [PerformanceCounterStateSource] ([counter_state_rule_id], [counter_id], [data_collector_id], [source_id])
		VALUES (@CounterRuleId, @CounterId, @DataCollectorId, @SourceId)
	
		--insert to PerformanceCounterStateOverride table
		INSERT [PerformanceCounterStateOverride] ([source_id], [critical_threshold], [warning_threshold], [time_window])
		VALUES (@SourceId, @CriticalThreshold, @WarningThreshold, @TimeWindow)

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH 
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION
		END;
		DECLARE @shouldReThrowError BIT = dbo.[ShouldReThrowError](ERROR_NUMBER(), ERROR_SEVERITY())
		IF(@shouldReThrowError = 1)
			THROW;
		SET @ErrorArgs = dbo.[FormatErrorMessage](ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE())
		;THROW 60000, @ErrorArgs, 1;
	END CATCH;
END
GO

-- Initialize the ProcedureName for error messages.
DECLARE @procedureName SYSNAME
SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + 'install_performanceruleconfig'
DECLARE @ErrorArgs NVARCHAR(500)

BEGIN TRY
	BEGIN TRANSACTION

	-- created default Performance Counter State Rule Configurations	
	DECLARE @idn nvarchar(50)
	SELECT @idn = '1E41EC7E-DA4A-44B4-BDF3-E2EB41E18B4C'

	-- CPU Usage
	IF NOT EXISTS(SELECT * FROM [PerformanceCounterStateRule] WHERE [counter_state_rule_id] = @idn)
	EXEC [create_perfcounter_state_rule] 
		@CounterStateRuleId = @idn,
		@CriticalThreshold = 80,
		@WarningThreshold = 60,
		@TimeWindow = 300,
		@Name = 'CPU Usage',
		@RuleType = 1,
		@session_id = NULL
	
	DECLARE @sourceid nvarchar(50)
	SET @sourceid = '2EAA8435-637D-4D71-8635-16C3DA06103A'

	IF NOT EXISTS(SELECT * FROM [PerformanceCounterStateSource] WHERE [counter_state_rule_id] = @idn)
	EXEC [create_rule_source]
		@CounterRuleId = @idn,
		@SourceId = @sourceid,
		@DataCollectorId = 0,
		@CounterId = 0,
		@DeviceId = NULL,
		@CriticalThreshold = -1,
		@WarningThreshold = -1,
		@TimeWindow = -1

	-- Memory, Available MBytes
	SELECT @idn = '7F9350E4-55B5-43BF-8146-78D2DC531A42';

	IF NOT EXISTS(SELECT * FROM [PerformanceCounterStateRule] WHERE [counter_state_rule_id] = @idn)
	BEGIN 
		EXEC [create_perfcounter_state_rule] 
			@CounterStateRuleId = @idn,
			@CriticalThreshold = 100,
			@WarningThreshold = 150,
			@TimeWindow = 300,
			@Name = 'Memory available',
			@RuleType = 2,
			@session_id = NULL
		
		UPDATE [PerformanceCounterStateRule]
		SET [disabled] =  1,
			[critical_threshold] = -1,
			[warning_threshold] = -1
		WHERE [PerformanceCounterStateRule].[counter_state_rule_id] = '7F9350E4-55B5-43BF-8146-78D2DC531A42'
	END 

	SET @sourceid = 'A6251066-057E-4BCF-90CB-27ABF3E8F9BA'
	IF NOT EXISTS(SELECT * FROM [PerformanceCounterStateSource] WHERE [counter_state_rule_id] = @idn)
	EXEC [create_rule_source]
		@CounterRuleId = @idn,
		@SourceId = @sourceid,
		@DataCollectorId = 0,
		@CounterId = 0,
		@DeviceId = NULL,
		@CriticalThreshold = -1,
		@WarningThreshold = -1,
		@TimeWindow = -1

	-- 	Live FPS
	SELECT @idn = '47AD1879-4F04-4E71-8EE8-6F9D2DAC9148'

	IF NOT EXISTS(SELECT * FROM [PerformanceCounterStateRule] WHERE [counter_state_rule_id] = @idn)
	EXEC [create_perfcounter_state_rule] 
		@CounterStateRuleId = @idn,
		@CriticalThreshold = 40,
		@WarningThreshold = 60,
		@TimeWindow = 30,
		@Name = 'Live FPS',
		@RuleType = 3,
		@session_id = NULL

	SET @sourceid = '6DB37E7F-C419-4D35-9C3D-000F30002187'

	IF NOT EXISTS(SELECT * FROM [PerformanceCounterStateSource] WHERE [counter_state_rule_id] = @idn)
	EXEC [create_rule_source]
		@CounterRuleId = @idn,
		@SourceId = @sourceid,
		@DataCollectorId = 0,
		@CounterId = 0,
		@DeviceId = NULL,
		@CriticalThreshold = -1,
		@WarningThreshold = -1,
		@TimeWindow = -1
	
	-- Recording FPS
	SELECT @idn = '2917B174-7E8E-490E-A402-6F5A76E4F9DD'

	IF NOT EXISTS(SELECT * FROM [PerformanceCounterStateRule] WHERE [counter_state_rule_id] = @idn)
	EXEC [create_perfcounter_state_rule] 
		@CounterStateRuleId = @idn,
		@CriticalThreshold = 40,
		@WarningThreshold = 60,
		@TimeWindow = 30,
		@Name = 'Recording FPS',
		@RuleType = 4,
		@session_id = NULL

	SET @sourceid = 'EC8B3369-EAFD-4935-9259-69A99935C90B'

	IF NOT EXISTS(SELECT * FROM [PerformanceCounterStateSource] WHERE [counter_state_rule_id] = @idn)
	EXEC [create_rule_source]
		@CounterRuleId = @idn,
		@SourceId = @sourceid,
		@DataCollectorId = 0,
		@CounterId = 0,
		@DeviceId = NULL,
		@CriticalThreshold = -1,
		@WarningThreshold = -1,
		@TimeWindow = -1
	 		
	-- Camera used space
	SELECT @idn = '7AEFC712-5251-4760-8CEB-2D95DA85E5EF'

	IF NOT EXISTS(SELECT * FROM [PerformanceCounterStateRule] WHERE [counter_state_rule_id] = @idn)
	BEGIN 
		EXEC [create_perfcounter_state_rule] 
			@CounterStateRuleId = @idn,
			@CriticalThreshold = 90,
			@WarningThreshold = 70,
			@TimeWindow = 300,
			@Name = 'Used Space',
			@RuleType = 5,
			@session_id = NULL

		UPDATE [PerformanceCounterStateRule]
		SET [disabled] =  1,
			[critical_threshold] = -1,
			[warning_threshold] = -1
		WHERE [PerformanceCounterStateRule].[counter_state_rule_id] = '7AEFC712-5251-4760-8CEB-2D95DA85E5EF'
	END

	SET @sourceid = 'F86B0CFD-0ADA-40D2-A580-6633221FFFCE'

	IF NOT EXISTS(SELECT * FROM [PerformanceCounterStateSource] WHERE [counter_state_rule_id] = @idn)
	EXEC [create_rule_source]
		@CounterRuleId = @idn,
		@SourceId = @sourceid,
		@DataCollectorId = 0,
		@CounterId = 0,
		@DeviceId = NULL,
		@CriticalThreshold = -1,
		@WarningThreshold = -1,
		@TimeWindow = -1
	 
	-- Disk free space
	SELECT @idn = '47B9163F-D9EB-43B4-BB3F-4BCCA591D5F4'

	IF NOT EXISTS(SELECT * FROM [PerformanceCounterStateRule] WHERE [counter_state_rule_id] = @idn)
	BEGIN
		EXEC [create_perfcounter_state_rule] 
			@CounterStateRuleId = @idn,
			@CriticalThreshold = 90,
			@WarningThreshold = 70,
			@TimeWindow = 300,
			@Name = 'Free Space',
			@RuleType = 6,
			@session_id = NULL

		UPDATE [PerformanceCounterStateRule]
		SET [disabled] =  1,
			[critical_threshold] = -1,
			[warning_threshold] = -1
		WHERE [PerformanceCounterStateRule].[counter_state_rule_id] = '47B9163F-D9EB-43B4-BB3F-4BCCA591D5F4'
	END

	SET @sourceid = '8E92D0DA-7E30-4790-A59D-571C36B2C293'

	IF NOT EXISTS(SELECT * FROM [PerformanceCounterStateSource] WHERE [counter_state_rule_id] = @idn)
	EXEC [create_rule_source]
		@CounterRuleId = @idn,
		@SourceId = @sourceid,
		@DataCollectorId = 0,
		@CounterId = 0,
		@DeviceId = NULL,
		@CriticalThreshold = -1,
		@WarningThreshold = -1,
		@TimeWindow = -1
	 	
	-- Service Available
	SELECT @idn = '063EC923-9E6B-4A95-B949-AE5A76DB8B5C';

	IF NOT EXISTS(SELECT * FROM [PerformanceCounterStateRule] WHERE [counter_state_rule_id] = @idn)
	EXEC [create_perfcounter_state_rule] 
		@CounterStateRuleId = @idn,
		@CriticalThreshold = 1,
		@WarningThreshold = 0,
		@TimeWindow = 5,
		@Name = 'Service Available',
		@RuleType = 8,
		@session_id = NULL

	SET @sourceid = '6DA1C766-2C6B-4AAF-9C5D-F9889C4EAF4E'

	IF NOT EXISTS(SELECT * FROM [PerformanceCounterStateSource] WHERE [counter_state_rule_id] = @idn)
	EXEC [create_rule_source]
		@CounterRuleId = @idn,
		@SourceId = @sourceid,
		@DataCollectorId = 0,
		@CounterId = 0,
		@DeviceId = NULL,
		@CriticalThreshold = -1,
		@WarningThreshold = -1,
		@TimeWindow = -1

	-- Retention time
	SELECT @idn = 'BDFC173B-43C7-4600-A483-82ED37CFD73C'

	IF NOT EXISTS(SELECT * FROM [PerformanceCounterStateRule] WHERE [counter_state_rule_id] = @idn)
	EXEC [create_perfcounter_state_rule] 
		@CounterStateRuleId = @idn,
		@CriticalThreshold = 3,
		@WarningThreshold = 10,
		@TimeWindow = 7,
		@Name = 'Retention Time',
		@RuleType = 7,
		@session_id = NULL

	SET @sourceid = '3EDFE833-543A-4AF4-9AA3-3AC44E4C0F00'

	IF NOT EXISTS(SELECT * FROM [PerformanceCounterStateSource] WHERE [counter_state_rule_id] = @idn)
	EXEC [create_rule_source]
		@CounterRuleId = @idn,
		@SourceId = @sourceid,
		@DataCollectorId = 0,
		@CounterId = 0,
		@DeviceId = NULL,
		@CriticalThreshold = -1,
		@WarningThreshold = -1,
		@TimeWindow = -1

	-- Camera status 
	SELECT @idn = '4A86F306-5A6F-43EF-9C21-F7F52FA86AC8';

	IF NOT EXISTS(SELECT * FROM [PerformanceCounterStateRule] WHERE [counter_state_rule_id] = @idn)
	EXEC [create_perfcounter_state_rule] 
		@CounterStateRuleId = @idn,
		@CriticalThreshold = 5,
		@WarningThreshold = 3,
		@TimeWindow = 1,
		@Name = 'Camera status',
		@RuleType = 10,
		@session_id = NULL

	SET @sourceid = '509CD8BA-F4CF-4F7A-A56A-0EBD52F37120'

	IF NOT EXISTS(SELECT * FROM [PerformanceCounterStateSource] WHERE [counter_state_rule_id] = @idn)
	EXEC [create_rule_source]
		@CounterRuleId = @idn,
		@SourceId = @sourceid,
		@DataCollectorId = 0,
		@CounterId = 0,
		@DeviceId = NULL,
		@CriticalThreshold = -1,
		@WarningThreshold = -1,
		@TimeWindow = -1

	-- Archive available
	SELECT @idn = 'D128E0E8-57E5-481E-8936-74423687CA82';

	IF NOT EXISTS(SELECT * FROM [PerformanceCounterStateRule] WHERE [counter_state_rule_id] = @idn)
	EXEC [create_perfcounter_state_rule] 
		@CounterStateRuleId = @idn,
		@CriticalThreshold = 0.9,
		@WarningThreshold = 0.9,
		@TimeWindow = 1,
		@Name = 'Archive available',
		@RuleType = 11,
		@session_id = NULL

	SET @sourceid = 'E1EC7B2E-0ED8-44BF-987D-05F1DC6CC0D5'

	IF NOT EXISTS(SELECT * FROM [PerformanceCounterStateSource] WHERE [counter_state_rule_id] = @idn)
	EXEC [create_rule_source]
		@CounterRuleId = @idn,
		@SourceId = @sourceid,
		@DataCollectorId = 0,
		@CounterId = 0,
		@DeviceId = NULL,
		@CriticalThreshold = -1,
		@WarningThreshold = -1,
		@TimeWindow = -1

	-- Database available
	SELECT @idn = 'E8240A18-9A93-424D-B45C-E83DCDD99EB5';

	IF NOT EXISTS(SELECT * FROM [PerformanceCounterStateRule] WHERE [counter_state_rule_id] = @idn)
	EXEC [create_perfcounter_state_rule] 
		@CounterStateRuleId = @idn,
		@CriticalThreshold = 0.9,
		@WarningThreshold = 0.9,
		@TimeWindow = 1,
		@Name = 'Database available',
		@RuleType = 12,
		@session_id = NULL

	SET @sourceid = 'B5C01EEC-C32A-40E6-8664-1B681D055813'

	IF NOT EXISTS(SELECT * FROM [PerformanceCounterStateSource] WHERE [counter_state_rule_id] = @idn)
	EXEC [create_rule_source]
		@CounterRuleId = @idn,
		@SourceId = @sourceid,
		@DataCollectorId = 0,
		@CounterId = 0,
		@DeviceId = NULL,
		@CriticalThreshold = -1,
		@WarningThreshold = -1,
		@TimeWindow = -1
			
	-- Failover started
	SELECT @idn = '009B1207-FD5F-4506-9660-4074FF04A4A3';

	IF NOT EXISTS(SELECT * FROM [PerformanceCounterStateRule] WHERE [counter_state_rule_id] = @idn)
	EXEC [create_perfcounter_state_rule] 
		@CounterStateRuleId = @idn,
		@CriticalThreshold = 0.9,
		@WarningThreshold = 0.9,
		@TimeWindow = 1,
		@Name = 'Failover started',
		@RuleType = 14,
		@session_id = NULL

	SET @sourceid = '4FE818A5-A59F-429E-A67B-A356EEF249D0'

	IF NOT EXISTS(SELECT * FROM [PerformanceCounterStateSource] WHERE [counter_state_rule_id] = @idn)
	EXEC [create_rule_source]
		@CounterRuleId = @idn,
		@SourceId = @sourceid,
		@DataCollectorId = 0,
		@CounterId = 0,
		@DeviceId = NULL,
		@CriticalThreshold = -1,
		@WarningThreshold = -1,
		@TimeWindow = -1

	-- Nvidia Decoding
	SELECT @idn = 'FC6A2188-C1AF-48B5-ACB7-E88FE093D30C';

	IF NOT EXISTS(SELECT * FROM [PerformanceCounterStateRule] WHERE [counter_state_rule_id] = @idn)
	EXEC [create_perfcounter_state_rule] 
		@CounterStateRuleId = @idn,
		@CriticalThreshold = 80,
		@WarningThreshold = 60,
		@TimeWindow = 30,
		@Name = 'Nvidia Decoding',
		@RuleType = 15,
		@session_id = NULL

	SET @sourceid = 'E67EE668-56E2-4EEC-9AFD-380A958474CE'
	IF NOT EXISTS(SELECT * FROM [PerformanceCounterStateSource] WHERE [counter_state_rule_id] = @idn)
	EXEC [create_rule_source]
		@CounterRuleId = @idn,
		@SourceId = @sourceid,
		@DataCollectorId = 0,
		@CounterId = 0,
		@DeviceId = NULL,
		@CriticalThreshold = -1,
		@WarningThreshold = -1,
		@TimeWindow = -1

	-- Nvidia Memory
	SELECT @idn = '238C1803-601B-4441-9FDE-46E7CBE2FF6E';

	IF NOT EXISTS(SELECT * FROM [PerformanceCounterStateRule] WHERE [counter_state_rule_id] = @idn)
	EXEC [create_perfcounter_state_rule] 
		@CounterStateRuleId = @idn,
		@CriticalThreshold = 80,
		@WarningThreshold = 60,
		@TimeWindow = 30,
		@Name = 'Nvidia Memory',
		@RuleType = 16,
		@session_id = NULL

	SET @sourceid = '55FB11C3-2113-4FF0-8C07-7C2DA0715E49'

	IF NOT EXISTS(SELECT * FROM [PerformanceCounterStateSource] WHERE [counter_state_rule_id] = @idn)
	EXEC [create_rule_source]
		@CounterRuleId = @idn,
		@SourceId = @sourceid,
		@DataCollectorId = 0,
		@CounterId = 0,
		@DeviceId = NULL,
		@CriticalThreshold = -1,
		@WarningThreshold = -1,
		@TimeWindow = -1

	-- Nvidia Rendering
	SELECT @idn = '3732961A-025E-4D80-AEF9-5B884E6B2BAF';

	IF NOT EXISTS(SELECT * FROM [PerformanceCounterStateRule] WHERE [counter_state_rule_id] = @idn)
	EXEC [create_perfcounter_state_rule] 
		@CounterStateRuleId = @idn,
		@CriticalThreshold = 80,
		@WarningThreshold = 60,
		@TimeWindow = 30,
		@Name = 'Nvidia Rendering',
		@RuleType = 17,
		@session_id = NULL

	SET @sourceid = 'B8DB3672-F5B2-46F8-A7F0-763888FCDE25'

	IF NOT EXISTS(SELECT * FROM [PerformanceCounterStateSource] WHERE [counter_state_rule_id] = @idn)
	EXEC [create_rule_source]
		@CounterRuleId = @idn,
		@SourceId = @sourceid,
		@DataCollectorId = 0,
		@CounterId = 0,
		@DeviceId = NULL,
		@CriticalThreshold = -1,
		@WarningThreshold = -1,
		@TimeWindow = -1

	-- Failover Error State
	SELECT @idn = 'E6330FA6-D3FE-4E35-AD50-21ECB7608931';

	IF NOT EXISTS(SELECT * FROM [PerformanceCounterStateRule] WHERE [counter_state_rule_id] = @idn)
	EXEC [create_perfcounter_state_rule] 
		@CounterStateRuleId = @idn,
		@CriticalThreshold = 0.9,
		@WarningThreshold = 0.9,
		@TimeWindow = 1,
		@Name = 'Failover Error State',
		@RuleType = 18,
		@session_id = NULL

	SET @sourceid = '79260E65-AAB2-4DC0-B809-1B44CEDC50AF'

	IF NOT EXISTS(SELECT * FROM [PerformanceCounterStateSource] WHERE [counter_state_rule_id] = @idn)
	EXEC [create_rule_source]
		@CounterRuleId = @idn,
		@SourceId = @sourceid,
		@DataCollectorId = 0,
		@CounterId = 0,
		@DeviceId = NULL,
		@CriticalThreshold = -1,
		@WarningThreshold = -1,
		@TimeWindow = -1

	COMMIT TRANSACTION
END TRY
BEGIN CATCH 
	IF @@TRANCOUNT > 0
	BEGIN
		ROLLBACK TRANSACTION
	END;
	DECLARE @shouldReThrowError BIT = dbo.[ShouldReThrowError](ERROR_NUMBER(), ERROR_SEVERITY())
		IF(@shouldReThrowError = 1)
			THROW;
		SET @ErrorArgs = dbo.[FormatErrorMessage](ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE())
		;THROW 60000, @ErrorArgs, 1;
END CATCH;
GO