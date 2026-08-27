IF OBJECT_ID ( '[LogServer].[ReadFilterValuesRuleName]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[ReadFilterValuesRuleName];
GO

CREATE PROCEDURE [LogServer].[ReadFilterValuesRuleName]
@Language nvarchar(10),
@LogTypeId tinyint
AS
BEGIN

	SELECT MI.[RuleName] AS [FilterId], MI.[RuleName] AS [DisplayValue]
	FROM
	[LogServer].[MessageType] MType
	INNER JOIN [LogServer].[MessageInstance_Active] MI ON MI.[MessageTypeId] = MType.[Id]
	WHERE MType.[LogTypeId] = @LogTypeId AND MI.[RuleName] IS NOT NULL

	UNION

	SELECT MI.[RuleName] AS [FilterId], MI.[RuleName] AS [DisplayValue]
	FROM
	[LogServer].[MessageType] MType
	INNER JOIN [LogServer].[MessageInstance_Inactive] MI ON MI.[MessageTypeId] = MType.[Id]
	WHERE MType.[LogTypeId] = @LogTypeId AND MI.[RuleName] IS NOT NULL

END
GO