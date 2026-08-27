IF OBJECT_ID ( '[LogServer].[ExpandMessageInstancePage]', 'IF' ) IS NOT NULL	
    DROP FUNCTION [LogServer].[ExpandMessageInstancePage];
GO

CREATE FUNCTION [LogServer].[ExpandMessageInstancePage](
@MessageInstancePage [LogServer].[MessageInstancePageTable] readonly,
@LanguageId smallint,
@DefaultLanguageId smallint
)
RETURNS TABLE AS
RETURN

SELECT
	MIP.[Id], 
	MIP.[ClientTimestampUtc],
	COALESCE(MText.[Text], MTextDef.[Text]) AS [Text],
	COALESCE(MIA.[SourceName], MII.[SourceName]) AS [SourceName],
	COALESCE(MIA.[User], MII.[User]) AS [User],
	COALESCE(MIA.[UserLocation], MII.[UserLocation]) AS [UserLocation],
	COALESCE(MIA.[RuleName], MII.[RuleName]) AS [RuleName],
	COALESCE(MIA.[ServiceName], MII.[ServiceName]) AS [ServiceName],
	COALESCE(MIA.[EventType], MII.[EventType]) AS [EventType],
	COALESCE(MIA.[Loglevel], MII.[Loglevel]) AS [Loglevel],
	COALESCE(MIA.[Permission], MII.[Permission]) AS [Permission],
	COALESCE(SText.[Text], STextDef.[Text]) as [STText],
	COALESCE(CText.[Text], CTextDef.[Text]) as [CTText],
	COALESCE(P1.[Name], P0.[Name]) AS [Name],
	COALESCE(P1.[Value], P0.[Value]) AS [Value]
	FROM 
	@MessageInstancePage AS MIP
	LEFT JOIN [LogServer].[MessageInstance_Active] MIA ON Active = 1 AND MIP.[Id] = MIA.[Id]
	LEFT JOIN [LogServer].[MessageInstance_Inactive] MII ON Active = 0 AND MIP.[Id] = MII.[Id]
	LEFT JOIN [LogServer].[MessageText] MText ON MText.[MessageTypeId] = COALESCE(MIA.[MessageTypeId], MII.[MessageTypeId]) AND MText.[LanguageId] = @LanguageId 
	LEFT JOIN [LogServer].[MessageText] MTextDef ON MTextDef.[MessageTypeId] = COALESCE(MIA.[MessageTypeId], MII.[MessageTypeId]) AND MTextDef.[LanguageId] = @DefaultLanguageId
	INNER JOIN [LogServer].[SourceType] SType ON MIP.[SourceTypeId] = SType.[Id]
	LEFT JOIN [LogServer].[SourceTypeText] SText ON SType.[Id] = SText.[SourceTypeId] AND SText.[LanguageId] = @LanguageId
	LEFT JOIN [LogServer].[SourceTypeText] STextDef ON SType.[Id] = STextDef.[SourceTypeId] AND STextDef.[LanguageId] = @DefaultLanguageId
	INNER JOIN [LogServer].[Category] C ON MIP.[CategoryId] = C.[Id]
	LEFT JOIN [LogServer].[CategoryText] CText ON C.[Id] = CText.[CategoryId] AND CText.[LanguageId] = @LanguageId
	LEFT JOIN [LogServer].[CategoryText] CTextDef ON C.[Id] = CTextDef.[CategoryId] AND CTextDef.[LanguageId] = @DefaultLanguageId
	LEFT JOIN [LogServer].[MessageInstance_Active_Parameter] P1 ON Active = 1 AND MIA.[Id] = P1.[MessageInstanceId]
	LEFT JOIN [LogServer].[MessageInstance_Inactive_Parameter] P0 ON Active = 0 AND MII.[Id] = P0.[MessageInstanceId]
	ORDER BY MIP.[ClientTimestampUtc] DESC
	OFFSET 0 ROWS
GO
