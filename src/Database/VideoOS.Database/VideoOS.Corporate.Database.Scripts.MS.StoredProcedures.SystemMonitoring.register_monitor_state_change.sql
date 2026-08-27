-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[register_monitor_state_change]') AND type = 'P')
DROP PROCEDURE [register_monitor_state_change]
GO

CREATE PROCEDURE [register_monitor_state_change]
	@RuleId NVARCHAR(100),
	@CounterPath NVARCHAR(364), 
	@DataCollectorId INT,
	@State INT,
	@TriggerValue FLOAT,
	@IsMonitored BIT,
	@ExistingState INT OUTPUT,
	@ExistingValue FLOAT OUTPUT,
	@ValueBasedState BIT OUTPUT
AS
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRAN
				
		SET @ExistingState = -1 --underfined unless counter rule is enabled 
		SET @ExistingValue = NULL --underfined
		
		-- set ValueBasedState
		SELECT @ValueBasedState = pt.[trigger_per_value]
		FROM [PerformanceCounterStateRule] as pr 
		INNER JOIN [PerformanceCounterType] as pt
		ON pr.[rule_type] = pt.[counter_type] AND pr.[counter_state_rule_id] = @RuleId COLLATE SQL_Latin1_General_CP1_CI_AS 
		
		IF ((SELECT [disabled] 
			FROM [PerformanceCounterStateRule] 
			WHERE [counter_state_rule_id] = @RuleId) = 0)
		BEGIN 
			SELECT @ExistingState = [state], 
				@ExistingValue = [trigger_value]
			FROM [MonitorStates]
			WHERE [counter_state_rule_id] = @RuleId AND [counter_path] = @CounterPath AND [data_collector_id] = @DataCollectorId

			IF(@ExistingState = -1)
			BEGIN	
				SET @ExistingState = NULL
				SET @ExistingValue = NULL
				
				-- The data collector has not been registered before
				INSERT [MonitorStates] ([counter_state_rule_id], [counter_path], [data_collector_id], [state], [is_monitored], [trigger_value])
				VALUES (@RuleId, @CounterPath, @DataCollectorId, @State, @IsMonitored, @TriggerValue)
			END
			ELSE
			BEGIN
				-- An existing monitor state already exists 
				UPDATE [MonitorStates]
				SET [state] = @State, 
					[trigger_value] = @TriggerValue
				WHERE [counter_state_rule_id] = @RuleId AND 
					[counter_path] = @CounterPath AND 
					[data_collector_id] = @DataCollectorId AND 
					[is_monitored] = @IsMonitored
			END
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