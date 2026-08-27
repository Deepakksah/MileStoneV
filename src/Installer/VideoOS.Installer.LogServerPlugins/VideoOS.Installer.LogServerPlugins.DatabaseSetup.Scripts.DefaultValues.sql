-- ensure that the en-US language is present
INSERT INTO [LogServer].[Language]([Code])
VALUES ('EN-US')	-- uppercase as the uniform representation of language codes

GO

-- ensure that the unknown source type is present (in English)
DECLARE @LanguageId smallint
SELECT @LanguageId = [Id] FROM [LogServer].[Language] WHERE [Code] = 'EN-US'

INSERT INTO [LogServer].[SourceType]([Id])
VALUES ('UNKNOWN')	-- uppercase as the uniform representation of identifiers for source types

INSERT INTO [LogServer].[SourceTypeText]([SourceTypeId], [LanguageId], [Text]) 
VALUES ('UNKNOWN', @LanguageId, 'Unknown')
-- TODO: Handle other languages

GO

-- ensure that the unknown category is present (in English)
DECLARE @LanguageId smallint
SELECT @LanguageId = [Id] FROM [LogServer].[Language] WHERE [Code] = 'EN-US'

INSERT INTO [LogServer].[Category]([Id])
VALUES ('UNKNOWN')	-- uppercase as the uniform representation of identifiers for categories

INSERT INTO [LogServer].[CategoryText]([CategoryId], [LanguageId], [Text]) 
VALUES ('UNKNOWN', @LanguageId, 'Unknown')
-- TODO: Handle other languages

GO

-- ensure that default retention values exist
INSERT INTO [LogServer].[RetentionConfiguration]([LogTypeId], [RetentionDays])
VALUES 
	(0,30), 	--Unknown
	(1,30), 	--System
	(2,30), 	--Audit
	(3,30) 		--Rule
GO

-- ensure that default anonymous logging enabled option exists
INSERT INTO [LogServer].[Configuration]([Name], [Value])
VALUES 
	('AnonymousLogging','false')
GO