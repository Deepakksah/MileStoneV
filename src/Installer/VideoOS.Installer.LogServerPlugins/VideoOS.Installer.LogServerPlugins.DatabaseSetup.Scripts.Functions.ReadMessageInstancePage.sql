IF EXISTS(SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[LogServer].[MessageInstance_Combined]'))
	DROP VIEW [LogServer].[MessageInstance_Combined] 
GO

CREATE VIEW [LogServer].[MessageInstance_Combined] 
AS
	SELECT MI.[Id], MType.[LogTypeId], MI.[ClientTimestampUtc], MI.[UtcExpires], MType.[SourceTypeId], MType.[CategoryId], 1 as Active
	FROM
	[LogServer].[MessageType] MType
	INNER JOIN [LogServer].[MessageInstance_Active] MI ON MI.[MessageTypeId] = MType.[Id]

	UNION
		
	SELECT MI.[Id], MType.[LogTypeId], MI.[ClientTimestampUtc], MI.[UtcExpires], MType.[SourceTypeId], MType.[CategoryId], 0 as Active
	FROM
	[LogServer].[MessageType] MType
	INNER JOIN [LogServer].[MessageInstance_Inactive] MI ON MI.[MessageTypeId] = MType.[Id]
GO

IF OBJECT_ID ( '[LogServer].[ReadMessageInstancePage]', 'IF' ) IS NOT NULL	
    DROP FUNCTION [LogServer].[ReadMessageInstancePage];
GO

CREATE FUNCTION [LogServer].[ReadMessageInstancePage](
@LogTypeId tinyint,
@ClientTimestampUtcFrom datetime,
@ClientTimestampUtcTo datetime,
@BatchSize int,
@Offset int,
@NowUtc datetime
)
RETURNS TABLE AS
RETURN

	SELECT MI.[Id], MI.[ClientTimestampUtc], MI.[SourceTypeId], MI.[CategoryId], MI.[Active]
	FROM [LogServer].[MessageInstance_Combined] AS MI
	WHERE
		MI.[LogTypeId] = @LogTypeId AND
		MI.[ClientTimestampUtc] >= @ClientTimestampUtcFrom AND
		MI.[ClientTimestampUtc] <= @ClientTimestampUtcTo AND
		@NowUtc <= MI.[UtcExpires]
	ORDER BY MI.[ClientTimestampUtc] DESC
	OFFSET @Offset ROWS FETCH NEXT @BatchSize ROWS ONLY

GO
