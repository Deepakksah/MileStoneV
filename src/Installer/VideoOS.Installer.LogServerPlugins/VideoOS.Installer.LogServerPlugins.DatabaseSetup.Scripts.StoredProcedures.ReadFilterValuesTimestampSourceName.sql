IF OBJECT_ID ( '[LogServer].[ReadFilterValuesTimestampSourceName]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[ReadFilterValuesTimestampSourceName];
GO

CREATE PROCEDURE [LogServer].[ReadFilterValuesTimestampSourceName]
@Language nvarchar(10),
@LogTypeId tinyint,
@ClientTimestampUtcFrom datetime,
@ClientTimestampUtcTo datetime
AS
BEGIN

	SELECT MI.[SourceName] AS [FilterId], MI.[SourceName] AS [DisplayValue]
	FROM
	[LogServer].[MessageType] MType
	INNER JOIN [LogServer].[MessageInstance_Active] MI ON MI.[MessageTypeId] = MType.[Id]
	WHERE 
	MType.[LogTypeId] = @LogTypeId AND 
	MI.[SourceName] IS NOT NULL AND
	MI.[ClientTimestampUtc] >= @ClientTimestampUtcFrom AND 
	MI.[ClientTimestampUtc] <= @ClientTimestampUtcTo

	UNION

	SELECT MI.[SourceName] AS [FilterId], MI.[SourceName] AS [DisplayValue]
	FROM
	[LogServer].[MessageType] MType
	INNER JOIN [LogServer].[MessageInstance_Inactive] MI ON MI.[MessageTypeId] = MType.[Id]
	WHERE 
	MType.[LogTypeId] = @LogTypeId AND 
	MI.[SourceName] IS NOT NULL AND
	MI.[ClientTimestampUtc] >= @ClientTimestampUtcFrom AND 
	MI.[ClientTimestampUtc] <= @ClientTimestampUtcTo

END
GO

