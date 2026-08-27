-- Delete MessageInstance_RecreateStructure procedure if it exists
IF OBJECTPROPERTY(OBJECT_ID('[LogServer].[MessageInstance_RecreateStructure]'), 'IsProcedure') = 1
	DROP PROCEDURE [LogServer].[MessageInstance_RecreateStructure]
GO

-- Recreate MessageInstance_RecreateStructure procedure
CREATE PROCEDURE [LogServer].[MessageInstance_RecreateStructure]
AS
BEGIN
	-- Comment: This procedure recreates brand new copy of MessageInstance tables with prefix 'MessageInstance_0_':
	--          1. Primary message instance table: MessageInstance_0
	--          2. Message parameter table: MessageInstance_0_Parameter
	--			3. After tables are recreated, all necessary indexes and foreign keys are setup

	SET NOCOUNT ON

	-- Delete table MessageInstance_0_Parameter if it exists, this will also delete foreign keys to MessageInstance_0
	IF OBJECT_ID('[LogServer].[MessageInstance_0_Parameter]') IS NOT NULL
		DROP TABLE [LogServer].[MessageInstance_0_Parameter]

	-- Recreate MessageInstance_0_Parameter table
	CREATE TABLE [LogServer].[MessageInstance_0_Parameter](
		[MessageInstanceId] bigint NOT NULL,
		[Name] nvarchar(128) NOT NULL,
		[Value] nvarchar(400) NOT NULL,
		CONSTRAINT [PK_MessageInstance_0_Parameter] PRIMARY KEY CLUSTERED
		(
			[MessageInstanceId] ASC,
			[Name] ASC
		) 
		WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	-- Delete table MessageInstance_0 if it exists
	IF OBJECT_ID('[LogServer].[MessageInstance_0]') IS NOT NULL
		DROP TABLE [LogServer].[MessageInstance_0]

	-- Recreate MessageInstance_0 table
	CREATE TABLE [LogServer].[MessageInstance_0](
		[Id] bigint IDENTITY(1,1) NOT NULL,
		[MessageTypeId] uniqueidentifier NOT NULL,
		[ClientTimestampUtc] datetime NOT NULL,
		[ServerTimestampUtc] datetime NOT NULL,
		[UtcExpires] datetime NOT NULL,
		[SourceName] [nvarchar](400),
		[User] [nvarchar](400),
		[UserLocation] [nvarchar](400),
		[RuleName] [nvarchar](400),
		[ServiceName] [nvarchar](400),
		[EventType] [nvarchar](400),
		[LogLevel] smallint,
		[Permission] smallint 
		CONSTRAINT [PK_MessageInstance_0] PRIMARY KEY CLUSTERED
		(
			[Id] ASC
		)
		WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	CREATE NONCLUSTERED INDEX [IX_MessageInstance_0_ClientTimestampUtc] ON [LogServer].[MessageInstance_0]
	(
		[ClientTimestampUtc] DESC	-- queries typically ask for instances sorted by time descending
	)
	INCLUDE 
	(
		[Id], 
		[MessageTypeId],
		[UtcExpires]
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

	CREATE NONCLUSTERED INDEX [IX_MessageInstance_0_SourceName] ON [LogServer].[MessageInstance_0]
	(
		[SourceName] ASC
	)
	INCLUDE 
	(
		[Id], 
		[MessageTypeId]
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

	CREATE NONCLUSTERED INDEX [IX_MessageInstance_0_ClientTimestampUtc_UtcExpires] ON [LogServer].[MessageInstance_0]
	(
		[ClientTimestampUtc] DESC,	-- queries typically ask for instances sorted by time descending
		[UtcExpires]
	)
	INCLUDE 
	(
		[Id], 
		[MessageTypeId]
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

	CREATE NONCLUSTERED INDEX [IX_MessageInstance_0_User] ON [LogServer].[MessageInstance_0]
	(
		[User] ASC
	)
	INCLUDE 
	(
		[Id], 
		[MessageTypeId]
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	
	CREATE NONCLUSTERED INDEX [IX_MessageInstance_0_UserLocation] ON [LogServer].[MessageInstance_0]
	(
		[UserLocation] ASC
	)
	INCLUDE 
	(
		[Id], 
		[MessageTypeId]
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

	CREATE NONCLUSTERED INDEX [IX_MessageInstance_0_RuleName] ON [LogServer].[MessageInstance_0]
	(
		[RuleName] ASC
	)
	INCLUDE 
	(
		[Id], 
		[MessageTypeId]
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

	CREATE NONCLUSTERED INDEX [IX_MessageInstance_0_ServiceName] ON [LogServer].[MessageInstance_0]
	(
		[ServiceName] ASC
	)
	INCLUDE 
	(
		[Id], 
		[MessageTypeId]
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

	CREATE NONCLUSTERED INDEX [IX_MessageInstance_0_EventType] ON [LogServer].[MessageInstance_0]
	(
		[EventType] ASC
	)
	INCLUDE 
	(
		[Id], 
		[MessageTypeId]
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

	CREATE NONCLUSTERED INDEX [IX_MessageInstance_0_LogLevel] ON [LogServer].[MessageInstance_0]
	(
		[LogLevel] ASC
	)
	INCLUDE 
	(
		[Id], 
		[MessageTypeId]
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

	CREATE NONCLUSTERED INDEX [IX_MessageInstance_0_Permission] ON [LogServer].[MessageInstance_0]
	(
		[Permission] ASC
	)
	INCLUDE 
	(
		[Id], 
		[MessageTypeId]
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]


	-- Recreate foreign key from child table MessageInstance_0_Parameter to parent table MessageInstance_0
	ALTER TABLE [LogServer].[MessageInstance_0_Parameter]  WITH CHECK 
	ADD CONSTRAINT [FK_MessageInstance_0_Parameter_MessageInstance] 
	FOREIGN KEY([MessageInstanceId]) REFERENCES [LogServer].[MessageInstance_0] ([Id])

	ALTER TABLE [LogServer].[MessageInstance_0_Parameter] CHECK CONSTRAINT [FK_MessageInstance_0_Parameter_MessageInstance]

	-- Recreate foreign key from table MessageInstance_0 to table MessageType
	ALTER TABLE [LogServer].[MessageInstance_0]  WITH CHECK 
	ADD CONSTRAINT [FK_MessageInstance_0_MessageType] 
	FOREIGN KEY ([MessageTypeId]) REFERENCES [LogServer].[MessageType] ([Id])

	ALTER TABLE [LogServer].[MessageInstance_0] CHECK CONSTRAINT [FK_MessageInstance_0_MessageType]
END
GO

-- Delete MessageInstance_RenameStructure procedure if it exists
IF OBJECTPROPERTY(OBJECT_ID('[LogServer].[MessageInstance_RenameStructure]'), 'IsProcedure') = 1
	DROP PROCEDURE [LogServer].[MessageInstance_RenameStructure]
GO

-- Recreate MessageInstance_RenameStructure procedure
CREATE PROCEDURE [LogServer].[MessageInstance_RenameStructure]
 @Find nvarchar(100)
,@Replace nvarchar(100)
AS
BEGIN
	-- Comment:        Changes name pattern in all tables, keys, indexes
	-- Param @Find:    string to be replaced in matched names
	-- Param @Replace: string to replace with in found name matches

	SET NOCOUNT ON

	-- Cursor helper variables
	DECLARE @name sysname, @replacement sysname

	-- Use cursor to iterate through all objects and indexes that have name containing @Find string
	DECLARE MY_CURSOR CURSOR 
	  LOCAL STATIC READ_ONLY FORWARD_ONLY
	FOR 
	SELECT name, replacement
	FROM (
	SELECT '[LogServer].[' + o.name + '].[' + i.name + ']' name
		 , REPLACE(i.name, @Find, @Replace) replacement
		 , 1 [order]
	FROM sys.indexes i
	JOIN sys.objects o ON i.object_id = o.object_id
	WHERE i.name LIKE '%' + @Find + '%' AND i.is_primary_key = 0
	UNION
	SELECT '[LogServer].[' + name + ']' name
		 , REPLACE(name, @Find, @Replace) replacement
		 , 2 [order]
	FROM sys.objects 
	WHERE name LIKE '%' + @Find + '%') t
	ORDER BY [order] -- Ensure indexes are renamed prior to tables

	-- For each found object change existing @name to @replacement
	OPEN MY_CURSOR
	FETCH NEXT FROM MY_CURSOR INTO @name, @replacement
	WHILE @@FETCH_STATUS = 0
	BEGIN 
		EXEC SP_RENAME @name, @replacement
		FETCH NEXT FROM MY_CURSOR INTO @name, @replacement
	END

	-- Dispose cursor
	CLOSE MY_CURSOR
	DEALLOCATE MY_CURSOR
END
GO

-- Actual creation of MessageInstance_Active and MessageInstance_Inactive

IF OBJECT_ID('[LogServer].[MessageInstance_Active]') IS NULL
BEGIN
	-- Recreate MessageInstance structures
	EXEC [LogServer].[MessageInstance_RecreateStructure]

	-- Claim new structures for Active version
	EXEC [LogServer].[MessageInstance_RenameStructure] 'MessageInstance_0', 'MessageInstance_Active'

	-- Recreate MessageInstance structures
	EXEC [LogServer].[MessageInstance_RecreateStructure]

	-- Claim new structures for Inactive version
	EXEC [LogServer].[MessageInstance_RenameStructure] 'MessageInstance_0', 'MessageInstance_Inactive'
END
GO