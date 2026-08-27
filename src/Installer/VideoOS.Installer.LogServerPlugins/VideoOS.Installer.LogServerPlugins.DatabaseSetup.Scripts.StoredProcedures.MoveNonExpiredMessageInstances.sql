-- Delete MoveNonExpiredMessageInstances procedure if it exists
IF OBJECTPROPERTY(OBJECT_ID('[LogServer].[MoveNonExpiredMessageInstances]'), 'IsProcedure') = 1
	DROP PROCEDURE [LogServer].[MoveNonExpiredMessageInstances]
GO

-- Recreate MoveNonExpiredMessageInstances procedure
CREATE PROCEDURE [LogServer].[MoveNonExpiredMessageInstances]
AS
BEGIN
	INSERT INTO [LogServer].[MessageInstance_0]
	(
		[Id],
		[MessageTypeId],
		[ClientTimestampUtc],
		[ServerTimestampUtc],
		[UtcExpires],
		[SourceName],
		[User],
		[UserLocation],
		[RuleName],
		[ServiceName],
		[EventType],
		[LogLevel],
		[Permission]
	)
	SELECT	
		[Id],
		[MessageTypeId],
		[ClientTimestampUtc],
		[ServerTimestampUtc],
		[UtcExpires],
		[SourceName],
		[User],
		[UserLocation],
		[RuleName],
		[ServiceName],
		[EventType],
		[LogLevel],
		[Permission]
	FROM [LogServer].[MessageInstance_Inactive]
	WHERE [UtcExpires] > GETUTCDATE()
END
GO