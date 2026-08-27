-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF OBJECT_ID ( '[Mip].[UpdateKind_transaction]', 'P' ) IS NOT NULL 
	DROP PROCEDURE [Mip].[UpdateKind_transaction];
GO

CREATE PROCEDURE [Mip].[UpdateKind_transaction]
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
	
	DECLARE @lastModifiedCurrent DATETIME
	DECLARE @rowCount INT

	 -- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	-- Make sure that a current transaction is active
	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END

	SELECT @lastModifiedCurrent = [LastModified]
	FROM [Mip].[Kinds]
	WHERE [KindId] = @KindId

		-- Check that last modified doesn't prevent user from updating object
	IF (@lastModifiedCurrent <> @LastModifiedBefore)
	BEGIN
		;THROW 60198, @procedureName, 1;
	END

	UPDATE [Mip].[Kinds]
	SET [Name] = @Name, [PluginName] = @PluginName, [DataVersion] = @DataVersion, [KindType] = @KindType, [SecurityAction] = @SecurityAction, [DisplayOnGisMap] = @DisplayOnGisMap, [ParentKindId] = @ParentKindId, [LastModified] = GETUTCDATE(), [ModifiedUser] = [ModifiedUser]
	WHERE [KindId] = @KindId
	
	SELECT @rowCount = @@ROWCOUNT
	IF(@rowCount = 0)
	BEGIN
		;THROW 60071, @procedureName, 1;
	END
END
GO