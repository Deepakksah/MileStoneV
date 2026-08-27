-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_rule]') AND type in (N'P', N'PC'))
DROP PROCEDURE [create_rule]
GO

CREATE PROCEDURE [create_rule]
	@Id [GuidType],
	@Name [NameType],
	@Description [DescriptionType],
	@Definition XML,
	@Enabled BIT
AS
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @dataSetAfter XML

		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		-- Insert the row
		INSERT INTO [Rules]
		(
			[IDRule],
			[Name],
			[Description],
			[LastModified],
			[Definition],
			[Enabled],
			[Priority]
		)
		VALUES 
		(
			@Id,
			@Name,
			@Description,
			@CurrentUtcTime,
			@Definition,
			@Enabled,
			100
		)

		-- Register the object in the security system
		EXEC security_register_object_transaction
			@namespace_name = 'Rule',
			@object_id = @Id,
			@parent_path = '/',
			@CurrentUtcTime = @CurrentUtcTime

		-- Get DataSet after changes
		SET @dataSetAfter = dbo.ruleAsDataSet(@Id)

		-- Create diffgram task for server service
		EXEC [create_task_server_services_transaction]
			@type = 'diffgram_insert',
			@data_a = @dataSetAfter,
			@current_time = @CurrentUtcTime

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