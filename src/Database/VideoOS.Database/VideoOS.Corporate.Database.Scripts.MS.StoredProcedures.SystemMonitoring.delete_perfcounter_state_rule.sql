-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_perfcounter_state_rule]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_perfcounter_state_rule]
GO

CREATE PROCEDURE [delete_perfcounter_state_rule]
	@CounterStateRuleId [UNIQUEIDENTIFIER],
	@session_id [GuidType]
AS	
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRAN

		-- Does the PerformanceCounterStateRule exist?
		DECLARE @Found INT
		SELECT @Found = Count([counter_state_rule_id]) FROM [PerformanceCounterStateRule]
		WHERE [counter_state_rule_id] = @CounterStateRuleId
		IF (@Found = 0)
		BEGIN
			;THROW 65003, @procedureName, 1;
		END

		DECLARE @name LongNameType
		SELECT @name = [PerformanceCounterStateRule].[rule_name]
		FROM [PerformanceCounterStateRule]
		WHERE [PerformanceCounterStateRule].[counter_state_rule_id] = @CounterStateRuleId

		-- clean up of PerformanceCounterStateOverride table
		DECLARE @id [UNIQUEIDENTIFIER]
		DECLARE @getid CURSOR

		SET @getid = CURSOR FOR
		SELECT [PerformanceCounterStateSource].[source_id]
		FROM   [PerformanceCounterStateSource]
		WHERE [PerformanceCounterStateSource].[counter_state_rule_id] = @CounterStateRuleId

		OPEN @getid
		BEGIN TRY
			FETCH NEXT
			FROM @getid INTO @id
			WHILE @@FETCH_STATUS = 0
			BEGIN
				DELETE FROM PerformanceCounterStateOverride
				WHERE [source_id] = @id
				FETCH NEXT
				FROM @getid INTO @id
			END
		END TRY
		BEGIN CATCH
			CLOSE @getid;
			DEALLOCATE @getid;
			;THROW
		END CATCH

		CLOSE @getid
		DEALLOCATE @getid

		DELETE FROM [PerformanceCounterStateSource]
		WHERE [counter_state_rule_id] = @CounterStateRuleId

		DELETE FROM [PerformanceCounterStateRule]
		WHERE [counter_state_rule_id] = @CounterStateRuleId
	
		COMMIT TRAN

		EXEC create_pending_log @session_id, 22020, @CounterStateRuleId, @name
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