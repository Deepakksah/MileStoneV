IF OBJECT_ID ( '[LogServer].[ReadFilterValuesUserLocation]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[ReadFilterValuesUserLocation];
GO

CREATE PROCEDURE [LogServer].[ReadFilterValuesUserLocation]
@Language nvarchar(10),
@LogTypeId tinyint
AS
BEGIN

	SELECT MI.[UserLocation] AS [FilterId], MI.[UserLocation] AS [DisplayValue]
	FROM
	[LogServer].[MessageType] MType
	INNER JOIN [LogServer].[MessageInstance_Active] MI ON MI.[MessageTypeId] = MType.[Id]
	WHERE MType.[LogTypeId] = @LogTypeId AND MI.[UserLocation] IS NOT NULL

	UNION

	SELECT MI.[UserLocation] AS [FilterId], MI.[UserLocation] AS [DisplayValue]
	FROM
	[LogServer].[MessageType] MType
	INNER JOIN [LogServer].[MessageInstance_Inactive] MI ON MI.[MessageTypeId] = MType.[Id]
	WHERE MType.[LogTypeId] = @LogTypeId AND MI.[UserLocation] IS NOT NULL

END
GO