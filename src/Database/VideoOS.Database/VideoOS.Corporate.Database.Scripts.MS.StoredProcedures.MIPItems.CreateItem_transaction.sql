-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- CreateItem - adds a MIP item to the table
IF OBJECT_ID ( '[Mip].[CreateItem_transaction]', 'P' ) IS NOT NULL 
	DROP PROCEDURE [Mip].[CreateItem_transaction];
GO

CREATE PROCEDURE [Mip].[CreateItem_transaction]
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

	DECLARE @gis_point GEOGRAPHY
	IF (@GisPoint IS NOT NULL)
		SET @gis_point = geography::STPointFromText(@GisPoint, 4326)
	ELSE
		SET @gis_point = geography::STPointFromText('POINT EMPTY', 4326)

	-- Check that kind exists
	IF NOT EXISTS (SELECT * FROM [Mip].[Kinds] WHERE [KindId] = @Kind)
	BEGIN
		;THROW 60474, @procedureName, 1;
	END

	DECLARE @currentTime DATETIME
	SET @currentTime = GETUTCDATE()

	INSERT INTO [Mip].[Items]([ItemId], [Name], [Kind], [ParentId], [ServerId], [ServerScheme], [ServerHost], [ServerPort], [ServerType], [Enabled], [Properties], [GisPoint], [LastModified], [Encrypted])
	VALUES (@ItemId, @Name, @Kind, @ParentId, @ServerId, @ServerScheme, @ServerHost, @ServerPort, @ServerType, @Enabled, @Properties, @gis_point, @currentTime, @Encrypted)
	
	-- Update the overall versions for this kind
	UPDATE [Mip].[Kinds]
	SET [DataVersion] = [DataVersion] + 1, [LastModified] = @currentTime, [ModifiedUser] = @ModifiedUser
	WHERE [KindId] = @Kind
END
GO