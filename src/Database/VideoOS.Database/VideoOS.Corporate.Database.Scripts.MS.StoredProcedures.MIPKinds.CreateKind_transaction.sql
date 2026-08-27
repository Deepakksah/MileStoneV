-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- CreateKind - adds a MIP kind to the table
IF OBJECT_ID ( '[Mip].[CreateKind_transaction]', 'P' ) IS NOT NULL 
	DROP PROCEDURE [Mip].[CreateKind_transaction];
GO

CREATE PROCEDURE [Mip].[CreateKind_transaction]
	@KindId UNIQUEIDENTIFIER,
	@Name NVARCHAR(256),
	@PluginId UNIQUEIDENTIFIER,
	@PluginName NVARCHAR(256),
	@DataVersion INTEGER,
	@KindType NVARCHAR(16),
	@SecurityAction NVARCHAR(256),
	@DisplayOnGisMap BIT,
	@ParentKindId UNIQUEIDENTIFIER,
	@ModifiedUser NVARCHAR(256)
AS
BEGIN
	SET NOCOUNT ON

	 -- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	-- Make sure that a current transaction is active
	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END
	
	INSERT INTO [Mip].[Kinds]([KindId], [Name], [PluginId], [PluginName], [DataVersion], [KindType], [SecurityAction], [DisplayOnGisMap], [ParentKindId] ,[LastModified], [ModifiedUser])
	VALUES (@KindId, @Name, @PluginId, @PluginName, @DataVersion, @KindType, @SecurityAction, @DisplayOnGisMap, @ParentKindId, GETUTCDATE(), @ModifiedUser)
END
GO