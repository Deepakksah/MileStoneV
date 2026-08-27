IF OBJECT_ID ( '[LogServer].[FilterMessageInstances]', 'TF' ) IS NOT NULL	
    DROP FUNCTION [LogServer].[FilterMessageInstances];
GO

CREATE FUNCTION [LogServer].[FilterMessageInstances](
@MessageInstances [LogServer].[MessageInstancePageTable] readonly,
@Filters [LogServer].[MessageFilterTable] readonly
)
RETURNS @FilteredMessageInstances table(
	[Id] [bigint],
	[ClientTimestampUtc] [datetime],
	[SourceTypeId] [nvarchar](400),
	[CategoryId] [nvarchar](400),
	[Active] [bit]
)
AS
BEGIN

	DECLARE @WorkingTable [LogServer].[MessageInstancePageTable]

	DECLARE @UseWorkingTable bit
	SET @UseWorkingTable = 0

	DECLARE @SourceTypeFilters [LogServer].[MessageFilterTable]
	DECLARE @CategoryFilters [LogServer].[MessageFilterTable]
	DECLARE @LogLevelFilters [LogServer].[MessageFilterTable]
	DECLARE @SourceNameFilters [LogServer].[MessageFilterTable]
	DECLARE @UserFilters [LogServer].[MessageFilterTable]
	DECLARE @UserLocationFilters [LogServer].[MessageFilterTable]
	DECLARE @RuleNameFilters [LogServer].[MessageFilterTable]
	DECLARE @ServiceNameFilters [LogServer].[MessageFilterTable]
	DECLARE @EventTypeFilters [LogServer].[MessageFilterTable]
	DECLARE @PermissionTypeFilters [LogServer].[MessageFilterTable]

	INSERT INTO @SourceTypeFilters SELECT * FROM @Filters WHERE [Name] = 'SourceType'
	INSERT INTO @CategoryFilters SELECT * FROM @Filters WHERE [Name] = 'Category'
	INSERT INTO @LogLevelFilters SELECT * FROM @Filters WHERE [Name] = 'LogLevel'
	INSERT INTO @SourceNameFilters SELECT * FROM @Filters WHERE [Name] = 'SourceName'
	INSERT INTO @UserFilters SELECT * FROM @Filters WHERE [Name] = 'User'
	INSERT INTO @UserLocationFilters SELECT * FROM @Filters WHERE [Name] = 'UserLocation'
	INSERT INTO @RuleNameFilters SELECT * FROM @Filters WHERE [Name] = 'RuleName'
	INSERT INTO @ServiceNameFilters SELECT * FROM @Filters WHERE [Name] = 'ServiceName'
	INSERT INTO @EventTypeFilters SELECT * FROM @Filters WHERE [Name] = 'EventType'
	INSERT INTO @PermissionTypeFilters SELECT * FROM @Filters WHERE [Name] = 'PermissionType'

	/*	It is expected that the first filter applied will reduce set so much that it is "good enough" to remove from same table in the following filters.
		This reduces the code not having to handle further swapping/truncating or 9 tables */

	/* Filter on SourceType if specified */
	IF EXISTS(SELECT 1 FROM @SourceTypeFilters)
	BEGIN
		INSERT INTO @WorkingTable SELECT * FROM @MessageInstances WHERE [Id] IN 
		(SELECT [Id] FROM [LogServer].[FilterMessageInstancesOnSourceType](@MessageInstances, @SourceTypeFilters))
		SET @UseWorkingTable = 1
	END

	/* Filter on Category if specified */
	IF EXISTS(SELECT 1 FROM @CategoryFilters)
	BEGIN
		IF @UseWorkingTable = 0
			INSERT INTO @WorkingTable SELECT * FROM @MessageInstances WHERE [Id] IN
			(SELECT [Id] FROM [LogServer].[FilterMessageInstancesOnCategory](@MessageInstances, @CategoryFilters))
		ELSE
			DELETE FROM @WorkingTable WHERE [Id] NOT IN 
			(SELECT [Id] FROM [LogServer].[FilterMessageInstancesOnCategory](@WorkingTable, @CategoryFilters))
		SET @UseWorkingTable = 1
	END

	/* Filter on LogLevel if specified */
	IF EXISTS(SELECT 1 FROM @LogLevelFilters)
	BEGIN
		IF @UseWorkingTable = 0
			INSERT INTO @WorkingTable SELECT * FROM @MessageInstances WHERE [Id] IN 
			(SELECT [Id] FROM [LogServer].[FilterMessageInstancesOnLogLevel](@MessageInstances, @LogLevelFilters))
		ELSE
			DELETE FROM @WorkingTable WHERE [Id] NOT IN 
			(SELECT [Id] FROM [LogServer].[FilterMessageInstancesOnLogLevel](@WorkingTable, @LogLevelFilters))
		SET @UseWorkingTable = 1
	END

	/* Filter on SourceName if specified */
	IF EXISTS(SELECT 1 FROM @SourceNameFilters)
	BEGIN
		IF @UseWorkingTable = 0
			INSERT INTO @WorkingTable SELECT * FROM @MessageInstances WHERE [Id] IN 
			(SELECT [Id] FROM [LogServer].[FilterMessageInstancesOnSourceName](@MessageInstances, @SourceNameFilters))
		ELSE
			DELETE FROM @WorkingTable WHERE [Id] NOT IN 
			(SELECT [Id] FROM [LogServer].[FilterMessageInstancesOnSourceName](@WorkingTable, @SourceNameFilters))
		SET @UseWorkingTable = 1
	END

	/* Filter on User if specified */
	IF EXISTS(SELECT 1 FROM @UserFilters)
	BEGIN
		IF @UseWorkingTable = 0
			INSERT INTO @WorkingTable SELECT * FROM @MessageInstances WHERE [Id] IN 
			(SELECT [Id] FROM [LogServer].[FilterMessageInstancesOnUser](@MessageInstances, @UserFilters))
		ELSE
			DELETE FROM @WorkingTable WHERE [Id] NOT IN 
			(SELECT [Id] FROM [LogServer].[FilterMessageInstancesOnUser](@WorkingTable, @UserFilters))
		SET @UseWorkingTable = 1
	END

	/* Filter on UserLocation if specified */
	IF EXISTS(SELECT 1 FROM @UserLocationFilters)
	BEGIN
		IF @UseWorkingTable = 0
			INSERT INTO @WorkingTable SELECT * FROM @MessageInstances WHERE [Id] IN 
			(SELECT [Id] FROM [LogServer].[FilterMessageInstancesOnUserLocation](@MessageInstances, @UserLocationFilters))
		ELSE
			DELETE FROM @WorkingTable WHERE [Id] NOT IN 
			(SELECT [Id] FROM [LogServer].[FilterMessageInstancesOnUserLocation](@WorkingTable, @UserLocationFilters))
		SET @UseWorkingTable = 1
	END

	/* Filter on RuleName if specified */
	IF EXISTS(SELECT 1 FROM @RuleNameFilters)
	BEGIN
		IF @UseWorkingTable = 0
			INSERT INTO @WorkingTable SELECT * FROM @MessageInstances WHERE [Id] IN 
			(SELECT [Id] FROM [LogServer].[FilterMessageInstancesOnRuleName](@MessageInstances, @RuleNameFilters))
		ELSE
			DELETE FROM @WorkingTable WHERE [Id] NOT IN 
			(SELECT [Id] FROM [LogServer].[FilterMessageInstancesOnRuleName](@WorkingTable, @RuleNameFilters))
		SET @UseWorkingTable = 1
	END

	/* Filter on ServiceName if specified */
	IF EXISTS(SELECT 1 FROM @ServiceNameFilters)
	BEGIN
		IF @UseWorkingTable = 0
			INSERT INTO @WorkingTable SELECT * FROM @MessageInstances WHERE [Id] IN 
			(SELECT [Id] FROM [LogServer].[FilterMessageInstancesOnServiceName](@MessageInstances, @ServiceNameFilters))
		ELSE
			DELETE FROM @WorkingTable WHERE [Id] NOT IN 
			(SELECT [Id] FROM [LogServer].[FilterMessageInstancesOnServiceName](@WorkingTable, @ServiceNameFilters))
		SET @UseWorkingTable = 1
	END

	/* Filter on EventType if specified */
	IF EXISTS(SELECT 1 FROM @EventTypeFilters)
	BEGIN
		IF @UseWorkingTable = 0
			INSERT INTO @WorkingTable SELECT * FROM @MessageInstances WHERE [Id] IN 
			(SELECT [Id] FROM [LogServer].[FilterMessageInstancesOnEventType](@MessageInstances, @EventTypeFilters))
		ELSE
			DELETE FROM @WorkingTable WHERE [Id] NOT IN 
			(SELECT [Id] FROM [LogServer].[FilterMessageInstancesOnEventType](@WorkingTable, @EventTypeFilters))
		SET @UseWorkingTable = 1
	END

	/* Filter on PermissionType if specified */
	IF EXISTS(SELECT 1 FROM @PermissionTypeFilters)
	BEGIN
		IF @UseWorkingTable = 0
			INSERT INTO @WorkingTable SELECT * FROM @MessageInstances WHERE [Id] IN 
			(SELECT [Id] FROM [LogServer].[FilterMessageInstancesOnPermission](@MessageInstances, @PermissionTypeFilters))
		ELSE
			DELETE FROM @WorkingTable WHERE [Id] NOT IN 
			(SELECT [Id] FROM [LogServer].[FilterMessageInstancesOnPermission](@WorkingTable, @PermissionTypeFilters))
		SET @UseWorkingTable = 1
	END

	INSERT INTO @FilteredMessageInstances SELECT * FROM @WorkingTable

RETURN
END
GO


IF OBJECT_ID ( '[LogServer].[FilterMessageInstancesOnSourceType]', 'IF' ) IS NOT NULL	
    DROP FUNCTION [LogServer].[FilterMessageInstancesOnSourceType];
GO

CREATE FUNCTION [LogServer].[FilterMessageInstancesOnSourceType](
@Messages [LogServer].[MessageInstancePageTable] readonly,
@Filters [LogServer].[MessageFilterTable] readonly
)
RETURNS TABLE AS
RETURN

SELECT
	MI.[Id]
FROM
	@Messages MI
	INNER JOIN [LogServer].[MessageInstance_Active] MIA ON MI.[Active] = 1 AND MI.[Id] = MIA.[Id]
	INNER JOIN [LogServer].[MessageType] MType ON MType.[Id] = MIA.[MessageTypeId]
	INNER JOIN @Filters F ON F.[Value] = MType.[SourceTypeId]

UNION

SELECT
	MI.[Id]
FROM
	@Messages MI
	INNER JOIN [LogServer].[MessageInstance_Inactive] MII ON MI.[Active] = 0 AND MI.[Id] = MII.[Id]
	INNER JOIN [LogServer].[MessageType] MType ON MType.[Id] = MII.[MessageTypeId]
	INNER JOIN @Filters F ON F.[Value] = MType.[SourceTypeId]

GO


IF OBJECT_ID ( '[LogServer].[FilterMessageInstancesOnCategory]', 'IF' ) IS NOT NULL	
    DROP FUNCTION [LogServer].[FilterMessageInstancesOnCategory];
GO

CREATE FUNCTION [LogServer].[FilterMessageInstancesOnCategory](
@Messages [LogServer].[MessageInstancePageTable] readonly,
@Filters [LogServer].[MessageFilterTable] readonly
)
RETURNS TABLE AS
RETURN

SELECT
	MI.[Id]
FROM
	@Messages MI
	INNER JOIN [LogServer].[MessageInstance_Active] MIA ON MI.[Active] = 1 AND MI.[Id] = MIA.[Id]
	INNER JOIN [LogServer].[MessageType] MType ON MType.[Id] = MIA.[MessageTypeId]
	INNER JOIN @Filters F ON F.[Value] = MType.[CategoryId]

UNION

SELECT
	MI.[Id]
FROM
	@Messages MI
	INNER JOIN [LogServer].[MessageInstance_Inactive] MII ON MI.[Active] = 0 AND MI.[Id] = MII.[Id]
	INNER JOIN [LogServer].[MessageType] MType ON MType.[Id] = MII.[MessageTypeId]
	INNER JOIN @Filters F ON F.[Value] = MType.[CategoryId]

GO


IF OBJECT_ID ( '[LogServer].[FilterMessageInstancesOnLogLevel]', 'IF' ) IS NOT NULL	
    DROP FUNCTION [LogServer].[FilterMessageInstancesOnLogLevel];
GO

CREATE FUNCTION [LogServer].[FilterMessageInstancesOnLogLevel](
@Messages [LogServer].[MessageInstancePageTable] readonly,
@Filters [LogServer].[MessageFilterTable] readonly
)
RETURNS TABLE AS
RETURN

SELECT
	MI.[Id]
FROM
	@Messages MI
	INNER JOIN [LogServer].[MessageInstance_Active] MIA ON MI.[Active] = 1 AND MI.[Id] = MIA.[Id]
	INNER JOIN @Filters F ON TRY_CONVERT(smallint, F.[Value]) = MIA.[LogLevel]

UNION

SELECT
	MI.[Id]
FROM
	@Messages MI
	INNER JOIN [LogServer].[MessageInstance_Inactive] MII ON MI.[Active] = 0 AND MI.[Id] = MII.[Id]
	INNER JOIN @Filters F ON TRY_CONVERT(smallint, F.[Value]) = MII.[LogLevel]

GO


IF OBJECT_ID ( '[LogServer].[FilterMessageInstancesOnSourceName]', 'IF' ) IS NOT NULL	
    DROP FUNCTION [LogServer].[FilterMessageInstancesOnSourceName];
GO

CREATE FUNCTION [LogServer].[FilterMessageInstancesOnSourceName](
@Messages [LogServer].[MessageInstancePageTable] readonly,
@Filters [LogServer].[MessageFilterTable] readonly
)
RETURNS TABLE AS
RETURN

SELECT
	MI.[Id]
FROM
	@Messages MI
	INNER JOIN [LogServer].[MessageInstance_Active] MIA ON MI.[Active] = 1 AND MI.[Id] = MIA.[Id]
	INNER JOIN @Filters F ON F.[Value] = MIA.[SourceName]

UNION

SELECT
	MI.[Id]
FROM
	@Messages MI
	INNER JOIN [LogServer].[MessageInstance_Inactive] MII ON MI.[Active] = 0 AND MI.[Id] = MII.[Id]
	INNER JOIN @Filters F ON F.[Value] = MII.[SourceName]

GO


IF OBJECT_ID ( '[LogServer].[FilterMessageInstancesOnUser]', 'IF' ) IS NOT NULL	
    DROP FUNCTION [LogServer].[FilterMessageInstancesOnUser];
GO

CREATE FUNCTION [LogServer].[FilterMessageInstancesOnUser](
@Messages [LogServer].[MessageInstancePageTable] readonly,
@Filters [LogServer].[MessageFilterTable] readonly
)
RETURNS TABLE AS
RETURN

SELECT
	MI.[Id]
FROM
	@Messages MI
	INNER JOIN [LogServer].[MessageInstance_Active] MIA ON MI.[Active] = 1 AND MI.[Id] = MIA.[Id]
	INNER JOIN @Filters F ON F.[Value] = MIA.[User]

UNION

SELECT
	MI.[Id]
FROM
	@Messages MI
	INNER JOIN [LogServer].[MessageInstance_Inactive] MII ON MI.[Active] = 0 AND MI.[Id] = MII.[Id]
	INNER JOIN @Filters F ON F.[Value] = MII.[User]

GO


IF OBJECT_ID ( '[LogServer].[FilterMessageInstancesOnUserLocation]', 'IF' ) IS NOT NULL	
    DROP FUNCTION [LogServer].[FilterMessageInstancesOnUserLocation];
GO

CREATE FUNCTION [LogServer].[FilterMessageInstancesOnUserLocation](
@Messages [LogServer].[MessageInstancePageTable] readonly,
@Filters [LogServer].[MessageFilterTable] readonly
)
RETURNS TABLE AS
RETURN

SELECT
	MI.[Id]
FROM
	@Messages MI
	INNER JOIN [LogServer].[MessageInstance_Active] MIA ON MI.[Active] = 1 AND MI.[Id] = MIA.[Id]
	INNER JOIN @Filters F ON F.[Value] = MIA.[UserLocation]

UNION

SELECT
	MI.[Id]
FROM
	@Messages MI
	INNER JOIN [LogServer].[MessageInstance_Inactive] MII ON MI.[Active] = 0 AND MI.[Id] = MII.[Id]
	INNER JOIN @Filters F ON F.[Value] = MII.[UserLocation]

GO


IF OBJECT_ID ( '[LogServer].[FilterMessageInstancesOnRuleName]', 'IF' ) IS NOT NULL	
    DROP FUNCTION [LogServer].[FilterMessageInstancesOnRuleName];
GO

CREATE FUNCTION [LogServer].[FilterMessageInstancesOnRuleName](
@Messages [LogServer].[MessageInstancePageTable] readonly,
@Filters [LogServer].[MessageFilterTable] readonly
)
RETURNS TABLE AS
RETURN

SELECT
	MI.[Id]
FROM
	@Messages MI
	INNER JOIN [LogServer].[MessageInstance_Active] MIA ON MI.[Active] = 1 AND MI.[Id] = MIA.[Id]
	INNER JOIN @Filters F ON F.[Value] = MIA.[RuleName]

UNION

SELECT
	MI.[Id]
FROM
	@Messages MI
	INNER JOIN [LogServer].[MessageInstance_Inactive] MII ON MI.[Active] = 0 AND MI.[Id] = MII.[Id]
	INNER JOIN @Filters F ON F.[Value] = MII.[RuleName]

GO


IF OBJECT_ID ( '[LogServer].[FilterMessageInstancesOnServiceName]', 'IF' ) IS NOT NULL	
    DROP FUNCTION [LogServer].[FilterMessageInstancesOnServiceName];
GO

CREATE FUNCTION [LogServer].[FilterMessageInstancesOnServiceName](
@Messages [LogServer].[MessageInstancePageTable] readonly,
@Filters [LogServer].[MessageFilterTable] readonly
)
RETURNS TABLE AS
RETURN

SELECT
	MI.[Id]
FROM
	@Messages MI
	INNER JOIN [LogServer].[MessageInstance_Active] MIA ON MI.[Active] = 1 AND MI.[Id] = MIA.[Id]
	INNER JOIN @Filters F ON F.[Value] = MIA.[ServiceName]

UNION

SELECT
	MI.[Id]
FROM
	@Messages MI
	INNER JOIN [LogServer].[MessageInstance_Inactive] MII ON MI.[Active] = 0 AND MI.[Id] = MII.[Id]
	INNER JOIN @Filters F ON F.[Value] = MII.[ServiceName]

GO


IF OBJECT_ID ( '[LogServer].[FilterMessageInstancesOnEventType]', 'IF' ) IS NOT NULL	
    DROP FUNCTION [LogServer].[FilterMessageInstancesOnEventType];
GO

CREATE FUNCTION [LogServer].[FilterMessageInstancesOnEventType](
@Messages [LogServer].[MessageInstancePageTable] readonly,
@Filters [LogServer].[MessageFilterTable] readonly
)
RETURNS TABLE AS
RETURN

SELECT
	MI.[Id]
FROM
	@Messages MI
	INNER JOIN [LogServer].[MessageInstance_Active] MIA ON MI.[Active] = 1 AND MI.[Id] = MIA.[Id]
	INNER JOIN @Filters F ON F.[Value] = MIA.[EventType]

UNION

SELECT
	MI.[Id]
FROM
	@Messages MI
	INNER JOIN [LogServer].[MessageInstance_Inactive] MII ON MI.[Active] = 0 AND MI.[Id] = MII.[Id]
	INNER JOIN @Filters F ON F.[Value] = MII.[EventType]

GO


IF OBJECT_ID ( '[LogServer].[FilterMessageInstancesOnPermission]', 'IF' ) IS NOT NULL	
    DROP FUNCTION [LogServer].[FilterMessageInstancesOnPermission];
GO

CREATE FUNCTION [LogServer].[FilterMessageInstancesOnPermission](
@Messages [LogServer].[MessageInstancePageTable] readonly,
@Filters [LogServer].[MessageFilterTable] readonly
)
RETURNS TABLE AS
RETURN

SELECT
	MI.[Id]
FROM
	@Messages MI
	INNER JOIN [LogServer].[MessageInstance_Active] MIA ON MI.[Active] = 1 AND MI.[Id] = MIA.[Id]
	INNER JOIN @Filters F ON F.[Value] = MIA.[Permission]

UNION

SELECT
	MI.[Id]
FROM
	@Messages MI
	INNER JOIN [LogServer].[MessageInstance_Inactive] MII ON MI.[Active] = 0 AND MI.[Id] = MII.[Id]
	INNER JOIN @Filters F ON F.[Value] = MII.[Permission]

GO
