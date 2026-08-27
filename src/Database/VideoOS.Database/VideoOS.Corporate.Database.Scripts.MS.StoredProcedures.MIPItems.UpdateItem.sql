-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- UpdateItem - updates a MIP item
IF OBJECT_ID ( '[Mip].[UpdateItem]', 'P' ) IS NOT NULL 
	DROP PROCEDURE [Mip].[UpdateItem];
GO

CREATE PROCEDURE [Mip].[UpdateItem]
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
		DECLARE @lastModifiedCurrent DATETIME
		DECLARE @rowCount INT
		DECLARE @gis_point GEOGRAPHY

		IF (@GisPoint IS NOT NULL)
			SET @gis_point = geography::STPointFromText(@GisPoint, 4326)
		ELSE
			SET @gis_point = geography::STPointFromText('POINT EMPTY', 4326)

		BEGIN TRAN

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