-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_rule]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_rule]
GO

CREATE PROCEDURE [update_rule]
	@Id [GuidType],
	@Name [NameType],
	@Description [DescriptionType],
	@Definition XML,
	@Enabled BIT,
	@session_id [GuidType]
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
	
		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

	    DECLARE @rule_id [GuidType]
		DECLARE @currentRuleName [NameType]
		DECLARE @currentEnabled BIT
		DECLARE @currentDefinition XML
	
		-- Make sure the audio message exists
		SELECT @rule_id = [IDRule], 
		@currentRuleName = [Name], 
		@currentEnabled = [Enabled],
		@currentDefinition = [Definition]
		FROM [Rules] WITH(UPDLOCK) 
		WHERE [IDRule] = @Id

		IF(@rule_id IS NULL)
		BEGIN
			;THROW 60660, @procedureName, 1;
		END

		-- Get DataSet before changes (for DiffGram)
		SET @dataSetBefore = dbo.ruleAsDataSet(@Id)

		-- Update the row
		UPDATE  [Rules]
		SET     [Name] = @Name,
				[Description] = @Description,
				[LastModified] = @CurrentUtcTime,
				[Definition] = @Definition,
				[Enabled] = @Enabled
		WHERE   [IDRule] = @Id

		-- Get DataSet after changes
		SET @dataSetAfter = dbo.ruleAsDataSet(@Id)

		-- Create diffgram task for server service
		EXEC [create_task_server_services_transaction]
			@type = 'diffgram_update',
			@data_a = @dataSetBefore,
			@data_b = @dataSetAfter,
			@current_time = @CurrentUtcTime

		IF(@currentRuleName <> @Name)
		BEGIN
		    --- Audit log similar to "Rule '{0}' renamed to '{1}'"
			EXEC create_pending_log @session_id, 7106, @Id, @Name, @currentRuleName
		END

		IF(@currentEnabled <> @Enabled)
		BEGIN
			IF(@Enabled = 1)
			BEGIN
				  --- Audit log similar to "Rule '{0}' enabled"
				EXEC create_pending_log @session_id, 7105, @Id, @Name
			END
			ELSE
			BEGIN
				  --- Audit log similar to "Rule '{0}' disabled"
				EXEC create_pending_log @session_id, 7104, @Id, @Name
			END
		END

		IF(CAST(@currentDefinition AS NVARCHAR(MAX)) <> CAST(@Definition AS NVARCHAR(MAX)))
		BEGIN
			--- Audit log similar to "Rule '{0}' edited"
			EXEC create_pending_log @session_id, 7103, @Id, @Name
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