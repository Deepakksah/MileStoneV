IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[LogServer].[MessageInstance_Active]') AND name = N'IX_MessageInstance_Active_ClientTimestampUtc_UtcExpires')
BEGIN
	CREATE NONCLUSTERED INDEX [IX_MessageInstance_Active_ClientTimestampUtc_UtcExpires] ON [LogServer].[MessageInstance_Active]
	(
		[ClientTimestampUtc] DESC,	-- queries typically ask for instances sorted by time descending
		[UtcExpires]
	)
	INCLUDE 
	(
		[Id], 
		[MessageTypeId]
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[LogServer].[MessageInstance_Inactive]') AND name = N'IX_MessageInstance_Inactive_ClientTimestampUtc_UtcExpires')
BEGIN
	CREATE NONCLUSTERED INDEX [IX_MessageInstance_Inactive_ClientTimestampUtc_UtcExpires] ON [LogServer].[MessageInstance_Inactive]
	(
		[ClientTimestampUtc] DESC,	-- queries typically ask for instances sorted by time descending
		[UtcExpires]
	)
	INCLUDE 
	(
		[Id], 
		[MessageTypeId]
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
END