-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- SaveItem - adds or updates a MIP item to the table
IF OBJECT_ID ( '[Mip].[SaveItem]', 'P' ) IS NOT NULL 
	DROP PROCEDURE [Mip].[SaveItem];
GO

CREATE PROCEDURE [Mip].[SaveItem]
	@ItemId UNIQUEIDENTIFIER,
	@Name NVARCHAR(256),
	@Kind UNIQUEIDENTIFIER,
	@ParentId UNIQUEIDENTIFIER,
	@ServerId UNIQUEIDENTIFIER,
	@ServerScheme NVARCHAR(256),
	@ServerHost NVARCHAR(256),
	@ServerPort INTEGER,
	@ServerType NVARCHAR(256),
	@Enabled BIT,
	@Properties NVARCHAR(MAX),
	@GisPoint NVARCHAR(MAX),
	@Encrypted BIT,
	@LastModifiedBefore DATETIME,
	@ModifiedUser NVARCHAR(256)
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRAN

		IF EXISTS (SELECT * FROM [Mip].[Items] WHERE [Kind] = @Kind AND [ItemId] = @ItemId)
		BEGIN
			IF @LastModifiedBefore IS NULL 
			BEGIN
				;THROW 60476, @procedureName, 1;
			END
			BEGIN TRY
				EXEC [Mip].[UpdateItem_transaction] @ItemId, @Name, @Kind, @ParentId, @ServerId, @ServerScheme, @ServerHost, @ServerPort, @ServerType, @Enabled, @Properties, @GisPoint, @Encrypted, @LastModifiedBefore, @ModifiedUser
			END TRY
			BEGIN CATCH
				;THROW 60477, @procedureName, 1;
			END CATCH
		END
		ELSE
		BEGIN
			BEGIN TRY
				EXEC [Mip].[CreateItem_transaction] @ItemId, @Name, @Kind, @ParentId, @ServerId, @ServerScheme, @ServerHost, @ServerPort, @ServerType, @Enabled, @Properties, @GisPoint, @Encrypted, @ModifiedUser
			END TRY
			BEGIN CATCH
				;THROW 60477, @procedureName, 1;
			END CATCH
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