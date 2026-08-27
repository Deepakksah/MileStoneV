-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- UpdateItem - updates a MIP item
IF OBJECT_ID ( '[Mip].[UpdateItem_transaction]', 'P' ) IS NOT NULL 
	DROP PROCEDURE [Mip].[UpdateItem_transaction];
GO

CREATE PROCEDURE [Mip].[UpdateItem_transaction]
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
	
	DECLARE @lastModifiedCurrent DATETIME
	DECLARE @rowCount INT
	DECLARE @gis_point GEOGRAPHY

	 -- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	-- Make sure that a current transaction is active
	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END

	IF (@GisPoint IS NOT NULL)
		SET @gis_point = geography::STPointFromText(@GisPoint, 4326)
	ELSE
		SET @gis_point = geography::STPointFromText('POINT EMPTY', 4326)

	SELECT @lastModifiedCurrent = [LastModified]
	FROM [Mip].[Items]
	WHERE [ItemId] = @ItemId AND [Kind] = @Kind

		-- Check that last modified doesn't prevent user from updating object
	IF (@lastModifiedCurrent <> @LastModifiedBefore)
	BEGIN
		;THROW 60198, @procedureName, 1;
	END

	DECLARE @currentTime DATETIME
	SET @currentTime = GETUTCDATE()

	UPDATE [Mip].[Items]
	SET [Name] = @Name, [ServerId] = @ServerId, [ParentId] = @ParentId, [ServerScheme] = @ServerScheme, [ServerHost] = @ServerHost, [ServerPort] = @ServerPort, [ServerType] = @ServerType, [Enabled] = @Enabled, [Properties] = @Properties, 
		[GisPoint] = @gis_point, [Encrypted] = @Encrypted, [LastModified] = @currentTime
	WHERE [ItemId] = @ItemId AND [Kind] = @Kind
	
	SELECT @rowCount = @@ROWCOUNT
	IF(@rowCount = 0)
	BEGIN
		;THROW 60071, @procedureName, 1;
	END

	-- Update the overall versions for this kind
	UPDATE [Mip].[Kinds]
	SET [DataVersion] = [DataVersion] + 1, [LastModified] = @currentTime, [ModifiedUser] = @ModifiedUser
	WHERE [KindId] = @Kind
END
GO