-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- SaveKind - adds or updates a MIP kind to the table
IF OBJECT_ID ( '[Mip].[SaveKind]', 'P' ) IS NOT NULL 
	DROP PROCEDURE [Mip].[SaveKind];
GO

CREATE PROCEDURE [Mip].[SaveKind]
	@KindId UNIQUEIDENTIFIER,
	@Name NVARCHAR(256),
	@PluginId UNIQUEIDENTIFIER,
	@PluginName NVARCHAR(256),
	@DataVersion INTEGER,
	@KindType NVARCHAR(16),
	@SecurityAction NVARCHAR(256),
	@DisplayOnGisMap BIT,
	@ParentKindId UNIQUEIDENTIFIER,
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

		BEGIN TRY
			IF EXISTS (SELECT * FROM [Mip].[Kinds] WHERE [KindId] = @KindId)
			BEGIN
				EXEC [Mip].[UpdateKind_transaction] @KindId, @Name, @PluginName, @DataVersion, @KindType, @SecurityAction, @DisplayOnGisMap, @ParentKindId, @LastModifiedBefore, @ModifiedUser
			END
			ELSE
			BEGIN
				EXEC [Mip].[CreateKind_transaction] @KindId, @Name, @PluginId, @PluginName, @DataVersion, @KindType, @SecurityAction, @DisplayOnGisMap, @ParentKindId, @ModifiedUser
			END
		END TRY
		BEGIN CATCH
			;THROW 60477, @procedureName, 1;
		END CATCH

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