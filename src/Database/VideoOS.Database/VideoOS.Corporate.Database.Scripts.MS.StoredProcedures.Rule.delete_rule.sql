-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_rule]') AND type = 'P')
DROP PROCEDURE [delete_rule]
GO

CREATE PROCEDURE [delete_rule]
	@Id [GuidType],
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

		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		-- Get name for logging
		DECLARE @name [NameType]
		SELECT @name = [Name]
		FROM [Rules] WITH(UPDLOCK)
		WHERE [IDRule] = @Id

		-- Ensure that rule exists
		IF (@name IS NULL)
		BEGIN
			;THROW 60661, @procedureName, 1;
		END

		-- Get data set before changes
		SET @dataSetBefore = dbo.ruleAsDataSet(@Id)
		
		-- Delete the row
		DELETE  [Rules]
		WHERE   [IDRule] = @Id

		-- Create diffgram task for server service
		EXEC [create_task_server_services_transaction]
			@type = 'diffgram_delete',
			@data_a = @dataSetBefore,
			@current_time = @CurrentUtcTime

		-- Unregister security object
		EXEC [security_unregister_object_transaction]
			@namespace_name = 'Rule',
			@object_id = @Id,
			@CurrentUtcTime = @CurrentUtcTime

		COMMIT TRAN

		EXEC create_pending_log @session_id, 7102, @Id, @name -- E.g. Rule '{0}' deleted.
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