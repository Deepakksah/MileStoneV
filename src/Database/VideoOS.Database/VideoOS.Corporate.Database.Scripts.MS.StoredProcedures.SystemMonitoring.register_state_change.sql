-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[register_state_change]') AND type = 'P')
DROP PROCEDURE [register_state_change]
GO

CREATE PROCEDURE [register_state_change]
	@RuleId NVARCHAR(100),
	@CounterPath NVARCHAR(364), 
	@DataCollectorId INT,
	@State INT,
	@TriggerValue FLOAT,
	@LookupSources BIT
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @rowCount INT
		DECLARE @stateChanged BIT = 0

		DECLARE @disabled BIT = 0
		DECLARE @valueBasedState BIT = 0

		SELECT @ValueBasedState = pt.[trigger_per_value], @disabled = pr.[disabled]
		FROM [PerformanceCounterStateRule] as pr 
		INNER JOIN [PerformanceCounterType] as pt
		ON pr.[rule_type] = pt.[counter_type] AND pr.[counter_state_rule_id] = @RuleId COLLATE SQL_Latin1_General_CP1_CI_AS 

		-- if rule is disabled - nothing changed and return immediately
		IF(@disabled = 1)
		BEGIN
			SELECT @stateChanged AS IsStateChanged
			RETURN
		END

		BEGIN TRAN
		-- An existing monitor state already exists
		UPDATE ms
		SET ms.[state] = @State, ms.[trigger_value] = @TriggerValue
		FROM [MonitorStates] ms
		WHERE ms.[counter_state_rule_id] = @RuleId 
			AND ms.[counter_path] = @CounterPath 
			AND ms.[data_collector_id] = @DataCollectorId 
			AND ms.[is_monitored] = 1
			AND ( ms.[state] != @State OR (@valueBasedState = 1 AND ABS(ms.[trigger_value] - @TriggerValue) > 0.0001))
			
		SELECT @rowCount = @@ROWCOUNT
		-- if the monitor state was updated, first resultset is 1
		IF (@rowCount = 1) SET @stateChanged = 1

		-- No value has been changed. This might indicate that data collector has not been registered before
		IF (@rowCount = 0)
		BEGIN
			-- Only insert if no states exists already
			IF NOT EXISTS(SELECT * FROM [MonitorStates] WHERE [counter_state_rule_id] = @RuleId AND [counter_path] = @CounterPath AND [data_collector_id] = @DataCollectorId)
			BEGIN
				INSERT [MonitorStates] ([counter_state_rule_id], [counter_path], [data_collector_id], [state], [is_monitored], [trigger_value])
				SELECT @RuleId, @CounterPath, @DataCollectorId, @State, 1, @TriggerValue
				
				SELECT @rowCount = @@ROWCOUNT

				IF(@rowCount = 1) SET @stateChanged = 1
			END
		END

		-- First resultset, to indicated whether a state has changed
		SELECT @stateChanged AS IsStateChanged

		-- Second resultset, contains affected rule sources
		IF (@stateChanged = 1 AND @LookupSources = 1)
		BEGIN
			DECLARE @ManagementServerHostName NVARCHAR(MAX)
			SET @ManagementServerHostName = (SELECT [Setting] FROM [SystemSettings] WHERE [Name] = 'ManagementServerHostName')
									 
			SELECT * FROM 
			(
				SELECT
					IDServer = 
						CASE 
							WHEN st.[service_type] = '2D275C3A-50BC-4435-B0AD-ED3381FEDC1D' AND dc.[host_name] = @ManagementServerHostName /* Management Server */ THEN (SELECT [IDSite] FROM [Sites] WHERE [IDSiteType] = 1)
							WHEN st.[service_type] = '2D275C3A-50BC-4435-B0AD-ED3381FEDC1D' AND dc.[host_name] != @ManagementServerHostName THEN NULL
							WHEN st.[service_type] = '2B2D3601-5857-44A3-880F-BE7957B5B27E' /* Recording Server */ THEN rec.[IDRecorder]
							WHEN st.[service_type] = '3C944CF2-668F-4956-9EBE-A1C3F6BD6CD4' /* Failover Server */ THEN fs.[IDFailoverServer]
							ELSE /* Other services */ rs.[IDServer]
						END,
					ServerName =
						CASE
							WHEN st.[service_type] = '2D275C3A-50BC-4435-B0AD-ED3381FEDC1D' AND dc.[host_name] = @ManagementServerHostName/* Management Server */ THEN @ManagementServerHostName
							WHEN st.[service_type] = '2B2D3601-5857-44A3-880F-BE7957B5B27E' /* Recording Server */ THEN rec.[Name]
							WHEN st.[service_type] = '3C944CF2-668F-4956-9EBE-A1C3F6BD6CD4' /* Failover Server */ THEN fs.[Name]
							ELSE /* Other services */ rs.[Name]
						END,
					dc.[pkid] AS DataCollectorId, st.[service_type] AS ServerType, st.[name] AS ServiceName
				FROM [DataCollector] dc
				INNER JOIN [DataCollectorInstalledServices] dcis ON dc.pkid = dcis.data_collector_id
				INNER JOIN [ServerType] st ON st.[name] = dcis.[service_name]
				LEFT JOIN [RegisteredServers] rs ON rs.[Type] = st.[service_type] AND rs.[DataCollectorId] = dc.pkid

				LEFT JOIN [RecordingServerService] rss ON rss.[host_name] = dc.[host_name]
				LEFT JOIN [RecordingServerServiceRecorder] rssr ON rssr.[recording_server_service_id] = rss.[pkid] AND rssr.[type] = 0
				LEFT JOIN [Recorders] rec ON rec.[IDRecorder] = rssr.[recorder_id] AND rec.[Enabled] = 1

				LEFT JOIN [FailoverServers] fs ON dc.[host_name] = fs.[HostName]
				WHERE dc.pkid = @DataCollectorId

			) AllServers
			WHERE AllServers.IDServer IS NOT NULL
		END

		COMMIT TRAN
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