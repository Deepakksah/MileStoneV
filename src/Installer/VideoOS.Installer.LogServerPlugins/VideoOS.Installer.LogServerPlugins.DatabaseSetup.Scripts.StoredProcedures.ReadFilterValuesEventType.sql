IF OBJECT_ID ( '[LogServer].[ReadFilterValuesEventType]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[ReadFilterValuesEventType];
GO

CREATE PROCEDURE [LogServer].[ReadFilterValuesEventType]
@Language nvarchar(10),
@LogTypeId tinyint
AS
BEGIN

	SELECT MI.[EventType] AS [FilterId], MI.[EventType] AS [DisplayValue]
	FROM
	[LogServer].[MessageType] MType
	INNER JOIN [LogServer].[MessageInstance_Active] MI ON MI.[MessageTypeId] = MType.[Id]
	WHERE MType.[LogTypeId] = @LogTypeId AND MI.[EventType] IS NOT NULL

	UNION

	SELECT MI.[EventType] AS [FilterId], MI.[EventType] AS [DisplayValue]
	FROM
	[LogServer].[MessageType] MType
	INNER JOIN [LogServer].[MessageInstance_Inactive] MI ON MI.[MessageTypeId] = MType.[Id]
	WHERE MType.[LogTypeId] = @LogTypeId AND MI.[EventType] IS NOT NULL

END
GO