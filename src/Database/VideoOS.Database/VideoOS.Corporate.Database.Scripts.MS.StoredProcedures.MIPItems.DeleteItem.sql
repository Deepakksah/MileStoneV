-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- DeleteItem - removes a MIP item from the table
IF OBJECT_ID ( '[Mip].[DeleteItem]', 'P' ) IS NOT NULL 
	DROP PROCEDURE [Mip].[DeleteItem];
GO

CREATE PROCEDURE [Mip].[DeleteItem]
	@ItemId UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @Kind UNIQUEIDENTIFIER

		BEGIN TRAN

		-- Get the kind of this item so that we can update the kind table afterwards
		SELECT @Kind = [Kind] 
		FROM [Mip].[Items] 
		WHERE [ItemId] = @ItemId

		DELETE FROM [Mip].[Items]
		WHERE [ItemId] = @ItemId

		IF @@ROWCOUNT = 0
		BEGIN
			;THROW 60473, @procedureName, 1;
		END

		-- Update the overall versions for this kind
		UPDATE [Mip].[Kinds]
		SET [DataVersion] = [DataVersion] + 1, [LastModified] = GETUTCDATE()
		WHERE [KindId] = @Kind

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