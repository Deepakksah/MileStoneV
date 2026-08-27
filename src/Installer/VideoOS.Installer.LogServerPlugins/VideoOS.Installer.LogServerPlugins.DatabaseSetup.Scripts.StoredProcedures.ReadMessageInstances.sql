IF OBJECT_ID ( '[LogServer].[ReadMessageInstancesByClientTimestamp]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[ReadMessageInstancesByClientTimestamp];
GO

CREATE PROCEDURE [LogServer].[ReadMessageInstancesByClientTimestamp]
@LogTypeId tinyint,
@LanguageCode nvarchar(10),
@ClientTimestampUtcFrom datetime,
@ClientTimestampUtcTo datetime,
@BatchSize int,
@Offset int
AS
BEGIN

	SET @LanguageCode = UPPER(@LanguageCode)

	DECLARE @LanguageId smallint
	SET @LanguageId =
	(SELECT TOP(1) [Id] FROM [LogServer].[Language] WHERE [Code] = @LanguageCode);

	DECLARE @DefaultLanguageId smallint
	SET @DefaultLanguageId =
	(SELECT TOP(1) [Id] FROM [LogServer].[Language] WHERE [Code] = 'EN-US');

	DECLARE @NowUtc datetime
	SET @NowUtc = GETUTCDATE();

	DECLARE @Page [LogServer].[MessageInstancePageTable]

	INSERT INTO @Page SELECT * FROM
	[LogServer].[ReadMessageInstancePage]
	(@LogTypeId, @ClientTimestampUtcFrom, @ClientTimestampUtcTo, @BatchSize, @Offset, @NowUtc)

	SELECT * FROM
	[LogServer].[ExpandMessageInstancePage]
	(@Page, @LanguageId, @DefaultLanguageId)

END
GO


IF OBJECT_ID ( '[LogServer].[ReadMessageInstancesByClientTimestampWithFilters]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[ReadMessageInstancesByClientTimestampWithFilters];
GO

CREATE PROCEDURE [LogServer].[ReadMessageInstancesByClientTimestampWithFilters]
@LogTypeId tinyint,
@LanguageCode nvarchar(10),
@ClientTimestampUtcFrom datetime,
@ClientTimestampUtcTo datetime,
@BatchSize int,
@Offset int,
@Filters [LogServer].[MessageFilterTable] readonly
AS
BEGIN

	SET @LanguageCode = UPPER(@LanguageCode)

	DECLARE @LanguageId smallint
	SET @LanguageId =
	(SELECT TOP(1) [Id] FROM [LogServer].[Language] WHERE [Code] = @LanguageCode);

	DECLARE @DefaultLanguageId smallint
	SET @DefaultLanguageId =
	(SELECT TOP(1) [Id] FROM [LogServer].[Language] WHERE [Code] = 'EN-US');

	DECLARE @NowUtc datetime
	SET @NowUtc = GETUTCDATE();

	DECLARE @FullTable [LogServer].[MessageInstancePageTable]
	DECLARE @Page [LogServer].[MessageInstancePageTable]

	/* Get all instances that match time and type */
	INSERT INTO @FullTable SELECT * FROM
	[LogServer].[ReadMessageInstancePage]
	(@LogTypeId, @ClientTimestampUtcFrom, @ClientTimestampUtcTo, 2147483647, 0, @NowUtc)

	/* Filter and extract the requested page */
	INSERT INTO @Page SELECT * FROM
	[LogServer].[FilterMessageInstances]
	(@FullTable, @Filters)
	ORDER BY [ClientTimestampUtc] DESC
	OFFSET @Offset ROWS FETCH NEXT @BatchSize ROWS ONLY

	/* Expand page with all fields and parameters */
	SELECT * FROM
	[LogServer].[ExpandMessageInstancePage]
	(@Page, @LanguageId, @DefaultLanguageId)

END
GO

IF OBJECT_ID ( '[LogServer].[ReadMessageInstanceCountByClientTimestamp]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[ReadMessageInstanceCountByClientTimestamp];
GO

CREATE PROCEDURE [LogServer].[ReadMessageInstanceCountByClientTimestamp]
@LogTypeId tinyint,
@ClientTimestampUtcFrom datetime,
@ClientTimestampUtcTo datetime
AS
BEGIN

	DECLARE @NowUtc datetime
	SET @NowUtc = GETUTCDATE()

	SELECT 
	(
		(SELECT COUNT(*)
		FROM 
		[LogServer].[MessageType] MType
		INNER JOIN [LogServer].[MessageInstance_Active] MI ON MI.[MessageTypeId] = MType.[Id]
		WHERE
		MType.[LogTypeId] = @LogTypeId AND
		MI.[ClientTimestampUtc] >= @ClientTimestampUtcFrom AND 
		MI.[ClientTimestampUtc] <= @ClientTimestampUtcTo AND
		@NowUtc < MI.[UtcExpires]) 
		
		+ 
		
		(SELECT COUNT(*)
		FROM 
		[LogServer].[MessageType] MType
		INNER JOIN [LogServer].[MessageInstance_Inactive] MI ON MI.[MessageTypeId] = MType.[Id]
		WHERE
		MType.[LogTypeId] = @LogTypeId AND
		MI.[ClientTimestampUtc] >= @ClientTimestampUtcFrom AND 
		MI.[ClientTimestampUtc] <= @ClientTimestampUtcTo AND
		@NowUtc < MI.[UtcExpires])
	)

END
GO
IF OBJECT_ID ( '[LogServer].[ReadMessageInstanceCountByClientTimestampWithFilters]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[ReadMessageInstanceCountByClientTimestampWithFilters];
GO

CREATE PROCEDURE [LogServer].[ReadMessageInstanceCountByClientTimestampWithFilters]
@LogTypeId tinyint,
@ClientTimestampUtcFrom datetime,
@ClientTimestampUtcTo datetime,
@Filters [LogServer].[MessageFilterTable] readonly
AS
BEGIN

	DECLARE @NowUtc datetime
	SET @NowUtc = GETUTCDATE()

	DECLARE @FullTable [LogServer].[MessageInstancePageTable]

	/* Get all instances that matches time and type */
	INSERT INTO @FullTable SELECT * FROM
	[LogServer].[ReadMessageInstancePage]
	(@LogTypeId, @ClientTimestampUtcFrom, @ClientTimestampUtcTo, 2147483647, 0, @NowUtc)

	/* Filter and get count */
	SELECT COUNT(*) FROM
	[LogServer].[FilterMessageInstances]
	(@FullTable, @Filters)

END
GO