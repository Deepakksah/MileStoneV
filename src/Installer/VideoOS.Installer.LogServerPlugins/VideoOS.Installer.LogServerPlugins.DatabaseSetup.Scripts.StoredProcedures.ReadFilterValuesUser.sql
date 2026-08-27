IF OBJECT_ID ( '[LogServer].[ReadFilterValuesUser]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[ReadFilterValuesUser];
GO

CREATE PROCEDURE [LogServer].[ReadFilterValuesUser]
@Language nvarchar(10),
@LogTypeId tinyint
AS
BEGIN

	SELECT MI.[User] AS [FilterId], MI.[User] AS [DisplayValue]
	FROM
	[LogServer].[MessageType] MType
	INNER JOIN [LogServer].[MessageInstance_Active] MI ON MI.[MessageTypeId] = MType.[Id]
	WHERE MType.[LogTypeId] = @LogTypeId AND MI.[User] IS NOT NULL

	UNION

	SELECT MI.[User] AS [FilterId], MI.[User] AS [DisplayValue]
	FROM
	[LogServer].[MessageType] MType
	INNER JOIN [LogServer].[MessageInstance_Inactive] MI ON MI.[MessageTypeId] = MType.[Id]
	WHERE MType.[LogTypeId] = @LogTypeId AND MI.[User] IS NOT NULL

END
GO