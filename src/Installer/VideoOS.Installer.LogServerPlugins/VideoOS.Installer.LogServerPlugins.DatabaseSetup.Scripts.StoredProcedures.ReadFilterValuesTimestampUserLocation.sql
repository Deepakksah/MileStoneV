IF OBJECT_ID ( '[LogServer].[ReadFilterValuesTimestampUserLocation]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[ReadFilterValuesTimestampUserLocation];
GO

CREATE PROCEDURE [LogServer].[ReadFilterValuesTimestampUserLocation]
@Language nvarchar(10),
@LogTypeId tinyint,
@ClientTimestampUtcFrom datetime,
@ClientTimestampUtcTo datetime
AS
BEGIN

	SELECT MI.[UserLocation] AS [FilterId], MI.[UserLocation] AS [DisplayValue]
	FROM
	[LogServer].[MessageType] MType
	INNER JOIN [LogServer].[MessageInstance_Active] MI ON MI.[MessageTypeId] = MType.[Id]
	WHERE 
	MType.[LogTypeId] = @LogTypeId AND 
	MI.[UserLocation] IS NOT NULL AND
	MI.[ClientTimestampUtc] >= @ClientTimestampUtcFrom AND 
	MI.[ClientTimestampUtc] <= @ClientTimestampUtcTo

	UNION

	SELECT MI.[UserLocation] AS [FilterId], MI.[UserLocation] AS [DisplayValue]
	FROM
	[LogServer].[MessageType] MType
	INNER JOIN [LogServer].[MessageInstance_Inactive] MI ON MI.[MessageTypeId] = MType.[Id]
	WHERE 
	MType.[LogTypeId] = @LogTypeId AND 
	MI.[UserLocation] IS NOT NULL AND
	MI.[ClientTimestampUtc] >= @ClientTimestampUtcFrom AND 
	MI.[ClientTimestampUtc] <= @ClientTimestampUtcTo

END
GO