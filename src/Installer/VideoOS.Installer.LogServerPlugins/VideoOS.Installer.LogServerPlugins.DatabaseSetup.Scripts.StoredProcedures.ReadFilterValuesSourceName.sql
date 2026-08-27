IF OBJECT_ID ( '[LogServer].[ReadFilterValuesSourceName]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[ReadFilterValuesSourceName];
GO

CREATE PROCEDURE [LogServer].[ReadFilterValuesSourceName]
@Language nvarchar(10),
@LogTypeId tinyint
AS
BEGIN

	SELECT MI.[SourceName] AS [FilterId], MI.[SourceName] AS [DisplayValue]
	FROM
	[LogServer].[MessageType] MType
	INNER JOIN [LogServer].[MessageInstance_Active] MI ON MI.[MessageTypeId] = MType.[Id]
	WHERE MType.[LogTypeId] = @LogTypeId AND MI.[SourceName] IS NOT NULL

	UNION

	SELECT MI.[SourceName] AS [FilterId], MI.[SourceName] AS [DisplayValue]
	FROM
	[LogServer].[MessageType] MType
	INNER JOIN [LogServer].[MessageInstance_Inactive] MI ON MI.[MessageTypeId] = MType.[Id]
	WHERE MType.[LogTypeId] = @LogTypeId AND MI.[SourceName] IS NOT NULL

END
GO