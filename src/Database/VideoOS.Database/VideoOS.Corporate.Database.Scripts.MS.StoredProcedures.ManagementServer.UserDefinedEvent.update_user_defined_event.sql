-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_user_defined_event]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_user_defined_event]
GO

CREATE PROCEDURE [update_user_defined_event]
	@IDEventType GuidType,
	@Name NameType,
	@Definition NTEXT,
	@custom_properties XML
AS
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @dataSetBefore XML
		DECLARE @dataSetAfter XML

		BEGIN TRANSACTION

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		DECLARE @user_defined_event_id [GuidType]

		SELECT @user_defined_event_id = [IDEventType]
		FROM [EventTypes] WITH(UPDLOCK)
		WHERE [IDEventType] = @IDEventType

		-- Make sure the user defined event exists
		IF(@user_defined_event_id IS NULL)
		BEGIN
			;THROW 60099, @procedureName, 1;
		END
		
		-- Get DataSet before changes
		SET @dataSetBefore = dbo.[userDefinedEventAsDataSet](@IDEventType)

		BEGIN TRY
			UPDATE [EventTypes]
			SET [Name] = @Name,
				[Definition] = @Definition,
				[LastModified] = @CurrentUtcTime
			WHERE [IDEventType] = @IDEventType AND GeneratorType = 'External'
		END TRY
		BEGIN CATCH
			IF(ERROR_NUMBER() = 2601 OR ERROR_NUMBER() = 2627) -- CONSTRAINT VIOLATION
			BEGIN
				-- The name already exists for an external event
				SET @ErrorArgs = @procedureName + N': %%Name=' + @Name + N'%%'
				;THROW 60015, @ErrorArgs, 1;
			END
			;THROW;
		END CATCH

		-- Get DataSet after changes
		SET @dataSetAfter = dbo.[userDefinedEventAsDataSet](@IDEventType)

		-- Call the create task procedure to create diffgram update task
		EXEC [create_task_transaction]
			@type = 'diffgram_update',
			@target = '909749AE-88C4-4780-A381-383D691E98FA',
			@data_a = @dataSetBefore,
			@data_b = @dataSetAfter,
			@current_time = @CurrentUtcTime

		-- Update custom properties
		EXEC [update_custom_properties_transaction]
			@object_id = @IDEventType,
			@custom_properties = @custom_properties
		
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