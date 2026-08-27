IF OBJECT_ID ( '[LogServer].[RegisterCategoryText]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[RegisterCategoryText];
GO

CREATE PROCEDURE [LogServer].[RegisterCategoryText]
@CategoryId nvarchar(400),
@LanguageCode nvarchar(10),
@CategoryText nvarchar(400)
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

	UPDATE [LogServer].[CategoryText]
	SET [Text] = @CategoryText
	WHERE [CategoryId] = @CategoryId AND [LanguageId] = @LanguageId

	IF @@ROWCOUNT = 0
	BEGIN

		BEGIN TRY
			INSERT INTO [LogServer].[CategoryText]([CategoryId], [LanguageId], [Text])
			VALUES (@CategoryId, @LanguageId, @CategoryText)
		END TRY
		BEGIN CATCH
			IF ERROR_NUMBER() IN (2601, 2627)	-- unique constraint violation
			BEGIN
				UPDATE [LogServer].[CategoryText]
				SET [Text] = @CategoryText
				WHERE [CategoryId] = @CategoryId AND [LanguageId] = @LanguageId
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

IF OBJECT_ID ( '[LogServer].[RegisterCategory]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[RegisterCategory];
GO

CREATE PROCEDURE [LogServer].[RegisterCategory]
@CategoryId nvarchar(400),
@CategoryTexts CategoryTextTable READONLY
AS
BEGIN
	BEGIN TRANSACTION

	-- make sure category id is uppercase
	SET @CategoryId = UPPER(@CategoryId)

	BEGIN TRY
		INSERT INTO [LogServer].[Category]([Id])
		VALUES (@CategoryId)
	END TRY
	BEGIN CATCH
		IF ERROR_NUMBER() NOT IN (2601, 2627)	-- unique constraint violation
		BEGIN
			ROLLBACK;
			THROW
		END

	END CATCH

	DECLARE @LanguageCode nvarchar(10)
	DECLARE @CategoryText nvarchar(400)
	DECLARE CategoryTextCursor CURSOR FOR
	SELECT [LanguageCode], [CategoryText]
	FROM @CategoryTexts
	OPEN CategoryTextCursor
	FETCH NEXT FROM CategoryTextCursor INTO @LanguageCode, @CategoryText
	WHILE (@@FETCH_STATUS = 0)
	BEGIN
		EXEC [LogServer].[RegisterCategoryText] @CategoryId, @LanguageCode, @CategoryText

		FETCH NEXT FROM CategoryTextCursor INTO @LanguageCode, @CategoryText
	END
	CLOSE CategoryTextCursor
	DEALLOCATE CategoryTextCursor

	COMMIT TRANSACTION
END
GO


IF OBJECT_ID ( '[LogServer].[RegisterCategories]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[RegisterCategories];
GO

CREATE PROCEDURE [LogServer].[RegisterCategories]
@Categories CategoryTable READONLY
AS
BEGIN
	BEGIN TRANSACTION

	DECLARE @CategoryId nvarchar(400)
	DECLARE @CategoryTexts CategoryTextTable
	DECLARE CategoriesCursor CURSOR FOR
	(SELECT DISTINCT [CategoryId]
	FROM @Categories)
	OPEN CategoriesCursor
	FETCH NEXT FROM CategoriesCursor INTO @CategoryId
	WHILE (@@FETCH_STATUS = 0)
	BEGIN
		INSERT INTO @CategoryTexts([LanguageCode], [CategoryText])
		SELECT [LanguageCode], [CategoryText]
		FROM @Categories
		WHERE [CategoryId] = @CategoryId
		
		EXEC [LogServer].[RegisterCategory] @CategoryId, @CategoryTexts

		DELETE FROM @CategoryTexts

		FETCH NEXT FROM CategoriesCursor INTO @CategoryId
	END
	CLOSE CategoriesCursor
	DEALLOCATE CategoriesCursor

	COMMIT TRANSACTION
END
GO