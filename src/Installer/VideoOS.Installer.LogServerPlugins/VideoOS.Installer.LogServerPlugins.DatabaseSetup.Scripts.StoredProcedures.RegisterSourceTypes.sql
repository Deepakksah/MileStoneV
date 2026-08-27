IF OBJECT_ID ( '[LogServer].[RegisterSourceTypeText]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[RegisterSourceTypeText];
GO

CREATE PROCEDURE [LogServer].[RegisterSourceTypeText]
@SourceTypeId nvarchar(400),
@LanguageCode nvarchar(10),
@SourceTypeText nvarchar(400)
AS
BEGIN

	BEGIN TRANSACTION

	DECLARE @LanguageId smallint
	SET @LanguageId = (SELECT [Id] FROM [LogServer].[Language] WHERE [Code] = @LanguageCode)

	IF @LanguageId IS NULL
	BEGIN 
		BEGIN TRY

			INSERT INTO [LogServer].[Language] ([Code])
			VALUES (@LanguageCode)

			SET @LanguageId = SCOPE_IDENTITY()

		END TRY
		BEGIN CATCH

			IF ERROR_NUMBER() IN (2601, 2627)	-- unique constraint violation
			BEGIN
				SET @LanguageId = (SELECT [Id] FROM [LogServer].[Language] WHERE [Code] = @LanguageCode)
			END
			ELSE
			BEGIN
				ROLLBACK;
				THROW
			END
		END CATCH
	END

	UPDATE [LogServer].[SourceTypeText]
	SET [Text] = @SourceTypeText
	WHERE [SourceTypeId] = @SourceTypeId AND [LanguageId] = @LanguageId

	IF @@ROWCOUNT = 0
	BEGIN

		BEGIN TRY
			INSERT INTO [LogServer].[SourceTypeText]([SourceTypeId], [LanguageId], [Text])
			VALUES (@SourceTypeId, @LanguageId, @SourceTypeText)
		END TRY
		BEGIN CATCH
			IF ERROR_NUMBER() IN (2601, 2627)	-- unique constraint violation
			BEGIN
				UPDATE [LogServer].[SourceTypeText]
				SET [Text] = @SourceTypeText
				WHERE [SourceTypeId] = @SourceTypeId AND [LanguageId] = @LanguageId
			END
			ELSE
			BEGIN
				ROLLBACK;
				THROW
			END

		END CATCH

	END

	COMMIT TRANSACTION
END
GO

IF OBJECT_ID ( '[LogServer].[RegisterSourceType]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[RegisterSourceType];
GO

CREATE PROCEDURE [LogServer].[RegisterSourceType]
@SourceTypeId nvarchar(400),
@SourceTypeTexts SourceTypeTextTable READONLY
AS
BEGIN
	BEGIN TRANSACTION

	-- make sure source type id is uppercase
	SET @SourceTypeId = UPPER(@SourceTypeId)

	BEGIN TRY
		INSERT INTO [LogServer].[SourceType]([Id])
		VALUES (@SourceTypeId)
	END TRY
	BEGIN CATCH
		IF ERROR_NUMBER() NOT IN (2601, 2627)	-- unique constraint violation
		BEGIN
			ROLLBACK;
			THROW
		END

	END CATCH

	DECLARE @LanguageCode nvarchar(10)
	DECLARE @SourceTypeText nvarchar(400)
	DECLARE SourceTypeTextCursor CURSOR FOR
	SELECT [LanguageCode], [SourceTypeText]
	FROM @SourceTypeTexts
	OPEN SourceTypeTextCursor
	FETCH NEXT FROM SourceTypeTextCursor INTO @LanguageCode, @SourceTypeText
	WHILE (@@FETCH_STATUS = 0)
	BEGIN
		EXEC [LogServer].[RegisterSourceTypeText] @SourceTypeId, @LanguageCode, @SourceTypeText

		FETCH NEXT FROM SourceTypeTextCursor INTO @LanguageCode, @SourceTypeText
	END
	CLOSE SourceTypeTextCursor
	DEALLOCATE SourceTypeTextCursor

	COMMIT TRANSACTION
END
GO


IF OBJECT_ID ( '[LogServer].[RegisterSourceTypes]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[RegisterSourceTypes];
GO

CREATE PROCEDURE [LogServer].[RegisterSourceTypes]
@SourceTypes SourceTypeTable READONLY
AS
BEGIN
	BEGIN TRANSACTION

	DECLARE @SourceTypeId nvarchar(400)
	DECLARE @SourceTypeTexts SourceTypeTextTable
	DECLARE SourceTypesCursor CURSOR FOR
	(SELECT DISTINCT [SourceTypeId]
	FROM @SourceTypes)
	OPEN SourceTypesCursor
	FETCH NEXT FROM SourceTypesCursor INTO @SourceTypeId
	WHILE (@@FETCH_STATUS = 0)
	BEGIN
		INSERT INTO @SourceTypeTexts([LanguageCode], [SourceTypeText])
		SELECT [LanguageCode], [SourceTypeText]
		FROM @SourceTypes
		WHERE [SourceTypeId] = @SourceTypeId
		
		EXEC [LogServer].[RegisterSourceType] @SourceTypeId, @SourceTypeTexts

		DELETE FROM @SourceTypeTexts

		FETCH NEXT FROM SourceTypesCursor INTO @SourceTypeId
	END
	CLOSE SourceTypesCursor
	DEALLOCATE SourceTypesCursor

	COMMIT TRANSACTION
END
GO