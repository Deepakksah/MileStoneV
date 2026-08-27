IF OBJECT_ID ( '[LogServer].[ReadFilterValuesSourceType]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[ReadFilterValuesSourceType];
GO

CREATE PROCEDURE [LogServer].[ReadFilterValuesSourceType]
@Language nvarchar(10),
@LogTypeId tinyint
AS
BEGIN

	SELECT DISTINCT STT.[SourceTypeId] AS [FilterId], STT.[Text] AS [DisplayValue]
	
	FROM
	
	[LogServer].[MessageType] MType 
	INNER JOIN [LogServer].[SourceTypeText] STT ON MType.SourceTypeId = STT.SourceTypeId 
	INNER JOIN [LogServer].[Language] L ON L.Id = STT.LanguageId
	WHERE L.[Code] = @Language AND MType.LogTypeId = @LogTypeId

END
GO