IF OBJECT_ID ( '[LogServer].[ReadFilterValuesTimestampRuleName]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[ReadFilterValuesTimestampRuleName];
GO

CREATE PROCEDURE [LogServer].[ReadFilterValuesTimestampRuleName]
@Language nvarchar(10),
@LogTypeId tinyint,
@ClientTimestampUtcFrom datetime,
@ClientTimestampUtcTo datetime
AS
BEGIN

	SELECT MI.[RuleName] AS [FilterId], MI.[RuleName] AS [DisplayValue]
	FROM
	[LogServer].[MessageType] MType
	INNER JOIN [LogServer].[MessageInstance_Active] MI ON MI.[MessageTypeId] = MType.[Id]
	WHERE 
	MType.[LogTypeId] = @LogTypeId AND 
	MI.[RuleName] IS NOT NULL AND
	MI.[ClientTimestampUtc] >= @ClientTimestampUtcFrom AND 
	MI.[ClientTimestampUtc] <= @ClientTimestampUtcTo

	UNION

	SELECT MI.[RuleName] AS [FilterId], MI.[RuleName] AS [DisplayValue]
	FROM
	[LogServer].[MessageType] MType
	INNER JOIN [LogServer].[MessageInstance_Inactive] MI ON MI.[MessageTypeId] = MType.[Id]
	WHERE 
	MType.[LogTypeId] = @LogTypeId AND 
	MI.[RuleName] IS NOT NULL AND
	MI.[ClientTimestampUtc] >= @ClientTimestampUtcFrom AND 
	MI.[ClientTimestampUtc] <= @ClientTimestampUtcTo

END
GO