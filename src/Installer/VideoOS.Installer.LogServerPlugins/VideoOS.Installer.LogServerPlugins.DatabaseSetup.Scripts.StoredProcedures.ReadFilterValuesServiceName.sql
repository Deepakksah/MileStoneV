IF OBJECT_ID ( '[LogServer].[ReadFilterValuesServiceName]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[ReadFilterValuesServiceName];
GO

CREATE PROCEDURE [LogServer].[ReadFilterValuesServiceName]
@Language nvarchar(10),
@LogTypeId tinyint
AS
BEGIN

	SELECT MI.[ServiceName] AS [FilterId], MI.[ServiceName] AS [DisplayValue]
	FROM
	[LogServer].[MessageType] MType
	INNER JOIN [LogServer].[MessageInstance_Active] MI ON MI.[MessageTypeId] = MType.[Id]
	WHERE MType.[LogTypeId] = @LogTypeId AND MI.[ServiceName] IS NOT NULL

	UNION

	SELECT MI.[ServiceName] AS [FilterId], MI.[ServiceName] AS [DisplayValue]
	FROM
	[LogServer].[MessageType] MType
	INNER JOIN [LogServer].[MessageInstance_Inactive] MI ON MI.[MessageTypeId] = MType.[Id]
	WHERE MType.[LogTypeId] = @LogTypeId AND MI.[ServiceName] IS NOT NULL

END
GO