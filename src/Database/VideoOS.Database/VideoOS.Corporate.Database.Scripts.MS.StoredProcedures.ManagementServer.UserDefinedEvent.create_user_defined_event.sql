-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_user_defined_event]') AND type = 'P')
DROP PROCEDURE [create_user_defined_event]
GO

CREATE PROCEDURE [create_user_defined_event]
	@UserSID [SidType],
	@IDEventType GuidType,
	@GeneratorSubType VARCHAR(36),
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
		BEGIN TRANSACTION

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		BEGIN TRY
			INSERT [EventTypes] (
				[IDEventType],
				[Name],
				[Definition],
				[GeneratorType],
				[GeneratorSubType],
				[IDGenerator],
				[Builtin],
				[LastModified]
			)
			VALUES (
				@IDEventType,
				@Name,
				@Definition,
				'External',
				@GeneratorSubType,
				'',
				0,
				@CurrentUtcTime
			)
		END TRY
		BEGIN CATCH
			IF(ERROR_NUMBER() = 2601 OR ERROR_NUMBER() = 2627) -- CONSTRAINT VIOLATION
			BEGIN
				-- The name already exists for an external event
				SET @ErrorArgs = @procedureName + N': %%Name=' + @Name + N'%%'
				;THROW 60014, @ErrorArgs, 1;
			END
			;THROW;
		END CATCH

		-- Register the object in the security system
		EXEC security_register_object_transaction
			@namespace_name = 'UserDefinedEvent',
			@object_id = @IDEventType,
			@parent_path = '/',
			@CurrentUtcTime = @CurrentUtcTime

		DECLARE @diffgram XML
		SELECT @diffgram = dbo.[userDefinedEventAsDataSet](@IDEventType)

		-- Create diffgram
		EXEC [create_task_transaction]
			@type = 'diffgram_insert',
			@target = '909749AE-88C4-4780-A381-383D691E98FA',
			@data_a = @diffgram,
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