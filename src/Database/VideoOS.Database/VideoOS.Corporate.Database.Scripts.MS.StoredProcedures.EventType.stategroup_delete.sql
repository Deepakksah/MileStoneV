-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF OBJECT_ID ( '[dbo].[stategroup_delete]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[stategroup_delete];	
GO

CREATE PROCEDURE [dbo].[stategroup_delete]
	@Id UniqueIdentifier
AS
BEGIN
	SET NOCOUNT ON

	  -- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY

		BEGIN TRY

			BEGIN TRAN		
			DELETE FROM [StateGroup] WHERE [Id] = @Id
			COMMIT TRAN

		END TRY
		BEGIN CATCH

			IF(@@ERROR <> 0)			-- Any difference between @@ERROR and ERROR_NUMBER()  ?
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

END

GO