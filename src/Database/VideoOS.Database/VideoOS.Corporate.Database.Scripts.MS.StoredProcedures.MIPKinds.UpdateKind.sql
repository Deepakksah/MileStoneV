-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- UpdateKind - updates a MIP kind
IF OBJECT_ID ( '[Mip].[UpdateKind]', 'P' ) IS NOT NULL 
	DROP PROCEDURE [Mip].[UpdateKind];
GO

CREATE PROCEDURE [Mip].[UpdateKind]
	@KindId UNIQUEIDENTIFIER,
	@Name NVARCHAR(256),
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
		EXEC [Mip].[UpdateKind_transaction] @KindId, @Name, @PluginName, @DataVersion, @KindType, @SecurityAction, @DisplayOnGisMap, @ParentKindId, @LastModifiedBefore, @ModifiedUser
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