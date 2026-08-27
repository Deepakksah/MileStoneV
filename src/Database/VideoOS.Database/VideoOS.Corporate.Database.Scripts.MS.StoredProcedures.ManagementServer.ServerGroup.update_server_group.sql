-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_server_group]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_server_group]
GO

CREATE PROCEDURE [update_server_group]
	@IDServerGroup [GuidType],
	@Name [NameType],
	@Description NVARCHAR(MAX),
	@custom_properties XML = NULL
AS
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRANSACTION

		-- Make sure the server group exists
		IF NOT EXISTS (SELECT [IDServerGroup] FROM [ServerGroup] WITH(UPDLOCK) WHERE [IDServerGroup] = @IDServerGroup)
		BEGIN
			;THROW 60060, @procedureName, 1;
		END

		BEGIN TRY
			UPDATE [ServerGroup]
			SET	[Name] = @Name,
				[Description] = @Description,
				[LastModified] = GETUTCDATE()
			WHERE [IDServerGroup] = @IDServerGroup
		END TRY
		BEGIN CATCH
			IF(ERROR_NUMBER() = 2601 OR ERROR_NUMBER() = 2627) -- CONSTRAINT VIOLATION
			BEGIN
				-- The name already exists in the server group
				SET @ErrorArgs = @procedureName + N': %%Name=' + @Name + N'%%'
				;THROW 60061, @ErrorArgs, 1;
			END
	
			;THROW;
		END CATCH

		-- Update custom properties
		EXEC [update_custom_properties_transaction]
			@object_id = @IDServerGroup,
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