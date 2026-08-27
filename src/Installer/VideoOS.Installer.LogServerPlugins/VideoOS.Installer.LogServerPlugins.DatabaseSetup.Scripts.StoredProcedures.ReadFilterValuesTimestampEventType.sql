IF OBJECT_ID ( '[LogServer].[ReadFilterValuesTimestampEventType]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[ReadFilterValuesTimestampEventType];
GO

CREATE PROCEDURE [LogServer].[ReadFilterValuesTimestampEventType]
@Language nvarchar(10),
@LogTypeId tinyint,
@ClientTimestampUtcFrom datetime,
@ClientTimestampUtcTo datetime
AS
BEGIN

	SELECT MI.[EventType] AS [FilterId], MI.[EventType] AS [DisplayValue]
	FROM
	[LogServer].[MessageType] MType
	INNER JOIN [LogServer].[MessageInstance_Active] MI ON MI.[MessageTypeId] = MType.[Id]
	WHERE 
	MType.[LogTypeId] = @LogTypeId AND 
	MI.[EventType] IS NOT NULL AND
	MI.[ClientTimestampUtc] >= @ClientTimestampUtcFrom AND 
	MI.[ClientTimestampUtc] <= @ClientTimestampUtcTo

	UNION

	SELECT MI.[EventType] AS [FilterId], MI.[EventType] AS [DisplayValue]
	FROM
	[LogServer].[MessageType] MType
	INNER JOIN [LogServer].[MessageInstance_Inactive] MI ON MI.[MessageTypeId] = MType.[Id]
	WHERE 
	MType.[LogTypeId] = @LogTypeId AND 
	MI.[EventType] IS NOT NULL AND
	MI.[ClientTimestampUtc] >= @ClientTimestampUtcFrom AND 
	MI.[ClientTimestampUtc] <= @ClientTimestampUtcTo

END
GO