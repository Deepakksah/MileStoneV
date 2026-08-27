IF OBJECT_ID ( '[LogServer].[RegisterMessageText]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[RegisterMessageText];
GO

CREATE PROCEDURE [LogServer].[RegisterMessageText]
@MessageTypeId uniqueidentifier,
@LanguageCode nvarchar(10),
@MessageText nvarchar(max)
AS
BEGIN

	BEGIN TRANSACTION

	-- ensure that the language code is uppercase
	SET @LanguageCode = UPPER(@LanguageCode)

	DECLARE @LanguageId smallint
	SET @LanguageId = (SELECT [Id] FROM [LogServer].[Language] with (NOLOCK) WHERE [Code] = @LanguageCode)  -- NOLOCK to prevent possible deadlock

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

	UPDATE [LogServer].[MessageText]
	SET [Text] = @MessageText
	WHERE [MessageTypeId] = @MessageTypeId AND [LanguageId] = @LanguageId

	IF @@ROWCOUNT = 0
	BEGIN

		BEGIN TRY
			INSERT INTO [LogServer].[MessageText]([MessageTypeId], [LanguageId], [Text])
			VALUES (@MessageTypeId, @LanguageId, @MessageText)
		END TRY
		BEGIN CATCH
			IF ERROR_NUMBER() IN (2601, 2627)	-- unique constraint violation
			BEGIN
				UPDATE [LogServer].[MessageText]
				SET [Text] = @MessageText
				WHERE [MessageTypeId] = @MessageTypeId AND [LanguageId] = @LanguageId
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

IF OBJECT_ID ( '[LogServer].[RegisterMessageType]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[RegisterMessageType];
GO

CREATE PROCEDURE [LogServer].[RegisterMessageType]
@MessageTypeId uniqueidentifier,
@LogTypeId tinyint,
@SourceTypeId nvarchar(400),
@CategoryId nvarchar(400),
@MessageTexts MessageTextTable READONLY
AS
BEGIN
	BEGIN TRANSACTION

	-- First, ensure that category and source type ids are uppercase
	SET @CategoryId = UPPER(@CategoryId)
	SET @SourceTypeId = UPPER(@SourceTypeId)

	-- The old Log Server allowed ad-hoc categories and source types, 
	-- so check that @SourceTypeId and @CategoryId actually represent ids for well-known source types and categories.
	-- If not, change them to use the "Unknown" source type / category

	IF NOT EXISTS (SELECT [Id] FROM [LogServer].[Category] WHERE [Id] = @CategoryId)
	BEGIN
		SET @CategoryId = 'UNKNOWN'
	END
	
	IF NOT EXISTS (SELECT [Id] FROM [LogServer].[SourceType] WHERE [Id] = @SourceTypeId)
	BEGIN
		SET @SourceTypeId = 'UNKNOWN'
	END
	
	UPDATE [LogServer].[MessageType]
	SET [LogTypeId] = @LogTypeId, 
		[SourceTypeId] = @SourceTypeId, 
		[CategoryId] = @CategoryId
	WHERE [Id] = @MessageTypeId
	
	IF @@ROWCOUNT = 0
	BEGIN
		
		BEGIN TRY
			INSERT INTO [LogServer].[MessageType]([Id], [LogTypeId], [SourceTypeId], [CategoryId])
			VALUES (@MessageTypeId, @LogTypeId, @SourceTypeId, @CategoryId)
		END TRY
		BEGIN CATCH
			IF ERROR_NUMBER() IN (2601, 2627)	-- unique constraint violation
			BEGIN
				UPDATE [LogServer].[MessageType]
				SET [LogTypeId] = @LogTypeId,
					[SourceTypeId] = @SourceTypeId, 
					[CategoryId] = @CategoryId
				WHERE [Id] = @MessageTypeId
			END
			ELSE
			BEGIN
				ROLLBACK;
				THROW
			END

		END CATCH
	END

	DECLARE @LanguageCode nvarchar(10)
	DECLARE @MessageText nvarchar(max)
	DECLARE MessageTextCursor CURSOR FOR
	SELECT [LanguageCode], [MessageText]
	FROM @MessageTexts
	OPEN MessageTextCursor
	FETCH NEXT FROM MessageTextCursor INTO @LanguageCode, @MessageText
	WHILE (@@FETCH_STATUS = 0)
	BEGIN
		EXEC [LogServer].[RegisterMessageText] @MessageTypeId, @LanguageCode, @MessageText

		FETCH NEXT FROM MessageTextCursor INTO @LanguageCode, @MessageText
	END
	CLOSE MessageTextCursor
	DEALLOCATE MessageTextCursor

	COMMIT TRANSACTION
END
GO


IF OBJECT_ID ( '[LogServer].[RegisterMessages]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[RegisterMessages];
GO

CREATE PROCEDURE [LogServer].[RegisterMessages]
@Messages MessageTypeTable READONLY
AS
BEGIN
	BEGIN TRANSACTION

	DECLARE @MessageTypeId uniqueidentifier
	DECLARE @LogTypeId tinyint
	DECLARE @SourceTypeId nvarchar(400)
	DECLARE @CategoryId nvarchar(400)
	DECLARE @MessageTexts MessageTextTable
	DECLARE MessagesCursor CURSOR FOR
	(SELECT DISTINCT [MessageTypeId]
	FROM @Messages)
	OPEN MessagesCursor
	FETCH NEXT FROM MessagesCursor INTO @MessageTypeId
	WHILE (@@FETCH_STATUS = 0)
	BEGIN
		-- assume that data is consistent for all rows with same MessageTypeId, so only read the first one
		SELECT TOP(1) @LogTypeId = [LogTypeId], @SourceTypeId = [SourceTypeId], @CategoryId = [CategoryId]
		FROM @Messages
		WHERE [MessageTypeId] = @MessageTypeId

		INSERT INTO @MessageTexts([LanguageCode], [MessageText])
		SELECT [LanguageCode], [MessageText]
		FROM @Messages
		WHERE [MessageTypeId] = @MessageTypeId
		
		EXEC [LogServer].[RegisterMessageType] @MessageTypeId, @LogTypeId, @SourceTypeId, @CategoryId, @MessageTexts

		DELETE FROM @MessageTexts

		FETCH NEXT FROM MessagesCursor INTO @MessageTypeId
	END
	CLOSE MessagesCursor
	DEALLOCATE MessagesCursor

	COMMIT TRANSACTION
END
GO


IF OBJECT_ID ( '[LogServer].[RegisterMessagesLegacy]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[RegisterMessagesLegacy];
GO

CREATE PROCEDURE [LogServer].[RegisterMessagesLegacy]
@LegacyMessages LegacyMessageTypeTable READONLY
AS
BEGIN
	BEGIN TRANSACTION

	DECLARE @StringIds TABLE (StringId nvarchar(400), LogLevel smallint)

	INSERT INTO @StringIds (StringId, LogLevel)
	SELECT DISTINCT [MessageTypeId], [LogLevel]
	FROM @LegacyMessages

	DECLARE @IdMap TABLE (StringId nvarchar(400), Id uniqueidentifier, LogLevel smallint)

	INSERT INTO @IdMap (StringId, Id, LogLevel)
	SELECT [StringId], [Id], SI.[LogLevel]
	FROM
	@StringIds SI
	LEFT JOIN [LogServer].[MessageTypeIdMapping] with (NOLOCK) ON [LegacyId] = [StringId]  -- NOLOCK to prevent possible deadlock
	
	DECLARE @UnknownStringIds TABLE (StringId nvarchar(400), LogLevel smallint)

	INSERT INTO @UnknownStringIds
	SELECT StringId, LogLevel
	FROM @IdMap
	WHERE Id IS NULL

	IF @@ROWCOUNT <> 0
	BEGIN

		DECLARE @UnknownStringId nvarchar(400)
		DECLARE @MessageTypeId uniqueidentifier
		DECLARE @LogLevel smallint

		DECLARE UnknownStringIdCursor CURSOR FOR
		(SELECT StringId, LogLevel FROM @UnknownStringIds)
		OPEN UnknownStringIdCursor
		FETCH NEXT FROM UnknownStringIdCursor INTO @UnknownStringId, @LogLevel
		WHILE (@@FETCH_STATUS = 0)
		BEGIN
			SET @MessageTypeId = NEWID()

			BEGIN TRY
				INSERT INTO [LogServer].[MessageTypeIdMapping]([LegacyId], [Id], [LogLevel])
				VALUES (@UnknownStringId, @MessageTypeId, @LogLevel)
			END TRY
			BEGIN CATCH
				IF ERROR_NUMBER() IN (2601, 2627)	-- unique constraint violation
				BEGIN
					SET @MessageTypeId = 
					(SELECT [Id] FROM [LogServer].[MessageTypeIdMapping]
					WHERE [LegacyId] = @UnknownStringId)
				END
				ELSE
				BEGIN
					ROLLBACK;
					THROW
				END
			END CATCH

			UPDATE @IdMap
			SET [Id] = @MessageTypeId
			WHERE [StringId] = @UnknownStringId
		
			FETCH NEXT FROM UnknownStringIdCursor INTO @UnknownStringId, @LogLevel
		END
		CLOSE UnknownStringIdCursor
		DEALLOCATE UnknownStringIdCursor
	END
	
	DECLARE @Messages MessageTypeTable

	INSERT INTO @Messages
	SELECT IM.[Id], LM.[LogTypeId], LM.[LanguageCode], LM.[MessageText], LM.[SourceTypeId], LM.[CategoryId]
	FROM 
	@LegacyMessages LM
	INNER JOIN @IdMap IM ON LM.[MessageTypeId] = IM.StringId
	ORDER BY [Id]

	EXEC [LogServer].[RegisterMessages] @Messages

	COMMIT TRANSACTION
END
GO