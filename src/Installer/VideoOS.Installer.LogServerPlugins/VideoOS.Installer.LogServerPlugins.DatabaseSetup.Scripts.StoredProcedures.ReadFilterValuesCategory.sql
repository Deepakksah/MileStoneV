IF OBJECT_ID ( '[LogServer].[ReadFilterValuesCategory]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[ReadFilterValuesCategory];
GO

CREATE PROCEDURE [LogServer].[ReadFilterValuesCategory]
@Language nvarchar(10),
@LogTypeId tinyint
AS
BEGIN

	SELECT DISTINCT CT.[CategoryId] AS [FilterId], CT.[Text] AS [DisplayValue]
	FROM 
	[LogServer].[MessageType] MType
	INNER JOIN [LogServer].[CategoryText] CT ON MType.CategoryId = CT.CategoryId 
	INNER JOIN [LogServer].[Language] L ON CT.LanguageId = L.Id
	WHERE L.[Code] = @Language AND MType.LogTypeId = @LogTypeId

END
GO