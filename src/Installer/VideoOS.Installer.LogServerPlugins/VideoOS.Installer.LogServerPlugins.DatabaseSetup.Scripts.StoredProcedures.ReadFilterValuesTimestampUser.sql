IF OBJECT_ID ( '[LogServer].[ReadFilterValuesTimestampUser]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[ReadFilterValuesTimestampUser];
GO

CREATE PROCEDURE [LogServer].[ReadFilterValuesTimestampUser]
@Language nvarchar(10),
@LogTypeId tinyint,
@ClientTimestampUtcFrom datetime,
@ClientTimestampUtcTo datetime
AS
BEGIN

	SELECT MI.[User] AS [FilterId], MI.[User] AS [DisplayValue]
	FROM
	[LogServer].[MessageType] MType
	INNER JOIN [LogServer].[MessageInstance_Active] MI ON MI.[MessageTypeId] = MType.[Id]
	WHERE 
	MType.[LogTypeId] = @LogTypeId AND 
	MI.[User] IS NOT NULL AND
	MI.[ClientTimestampUtc] >= @ClientTimestampUtcFrom AND 
	MI.[ClientTimestampUtc] <= @ClientTimestampUtcTo

	UNION

	SELECT MI.[User] AS [FilterId], MI.[User] AS [DisplayValue]
	FROM
	[LogServer].[MessageType] MType
	INNER JOIN [LogServer].[MessageInstance_Inactive] MI ON MI.[MessageTypeId] = MType.[Id]
	WHERE 
	MType.[LogTypeId] = @LogTypeId AND 
	MI.[User] IS NOT NULL AND
	MI.[ClientTimestampUtc] >= @ClientTimestampUtcFrom AND 
	MI.[ClientTimestampUtc] <= @ClientTimestampUtcTo

END
GO