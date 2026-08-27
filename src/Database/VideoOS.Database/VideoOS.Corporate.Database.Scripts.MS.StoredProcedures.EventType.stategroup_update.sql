-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF OBJECT_ID ( '[dbo].[stategroup_update]', 'P') IS NOT NULL
	DROP PROCEDURE [stategroup_update]
GO

CREATE PROCEDURE [dbo].[stategroup_update]
	@Id [UniqueIdentifier],
	@Name [NameType],
	@States NVarchar(1024)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @status INT

	  -- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY

		BEGIN TRY

			BEGIN TRAN

			DECLARE @CurrentUtcTime DATETIME
			SET @CurrentUtcTime = GETUTCDATE()

			UPDATE [StateGroup] SET 
				[Name] = @Name,
				[States] = @States,
				[last_modified] = @CurrentUtcTime
			WHERE [Id] = @Id

			COMMIT TRAN

		END TRY
		BEGIN CATCH

			IF(ERROR_NUMBER() = 2601 OR ERROR_NUMBER() = 2627) -- CONSTRAINT VIOLATION
			BEGIN
				-- The name already exists on the server
				SET @ErrorArgs = @procedureName + N': %%name=' + @name + N'%%'
				;THROW 60701, @ErrorArgs, 1;
			END
			ELSE IF(ERROR_NUMBER() <> 0)
			BEGIN
				;THROW 60000, @procedureName, 1;
			END
			;THROW;
		END CATCH

	END TRY

	BEGIN CATCH 
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION
		END;
		IF(ERROR_NUMBER() < 60000 AND ERROR_NUMBER() <> 1205 AND ERROR_NUMBER() <> 8628 AND ERROR_NUMBER() <> 8645 AND ERROR_NUMBER() <> 2801)
		BEGIN
			SET @ErrorArgs = dbo.[FormatErrorMessage](ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE())
			;THROW 60000, @ErrorArgs, 1;
		END
		;THROW
	END CATCH;

	RETURN 0
	END
GO