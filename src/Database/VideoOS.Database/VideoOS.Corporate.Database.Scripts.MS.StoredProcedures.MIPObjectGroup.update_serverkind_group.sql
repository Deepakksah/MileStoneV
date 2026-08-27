-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_serverkind_group]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_serverkind_group]
GO

CREATE PROCEDURE [update_serverkind_group]
	@IDServerGroup [GuidType],
	@Name [NameType],
	@Description NVARCHAR(MAX),
	@CustomData NVARCHAR(MAX),
	@custom_properties XML = NULL
AS
BEGIN
	SET NOCOUNT ON
 
	DECLARE @CurrentUtcTime DATETIME
	SET @CurrentUtcTime = GETUTCDATE()

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRAN

		-- Make sure the server group exists
		IF NOT EXISTS (SELECT [IDServerGroup] FROM [ServerGroup] WITH(UPDLOCK) WHERE [IDServerGroup] = @IDServerGroup)
		BEGIN
			;THROW 60060, @procedureName, 1;
		END

		DECLARE @dataSetBefore XML
		SET @dataSetBefore = dbo.serverGroupAsDataSet(@IDServerGroup)

		BEGIN TRY
			UPDATE [ServerGroup]
			SET	[Name] = @Name,
				[Description] = @Description,
				[LastModified] = @CurrentUtcTime
			WHERE [IDServerGroup] = @IDServerGroup
		END TRY
		BEGIN CATCH
			IF(ERROR_NUMBER() = 2601 OR ERROR_NUMBER() = 2627) -- CONSTRAINT VIOLATION
			BEGIN
				-- The name already exists in the server group
				SET @ErrorArgs = @procedureName + N': %%Name=' + @Name + N'%%'
				;THROW 60061, @ErrorArgs, 1;
			END
			;THROW
		END CATCH

		UPDATE [ServerGroupKind]
		SET	[CustomData] = @CustomData
		WHERE [IDServerGroup] = @IDServerGroup

		-- Update custom properties
		EXEC [update_custom_properties_transaction]
			@object_id = @IDServerGroup,
			@custom_properties = @custom_properties

		-- Get DataSet after changes
		DECLARE @dataSetAfter XML
		SET @dataSetAfter = dbo.serverGroupAsDataSet(@IDServerGroup)

		EXEC [create_task_transaction]
			@type = 'diffgram_update',
			@target = '909749AE-88C4-4780-A381-383D691E98FA',
			@data_a = @dataSetBefore,
			@data_b = @dataSetAfter,
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