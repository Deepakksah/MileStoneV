-- Delete MoveMessageInstancesAndUpdateExpiration procedure if it exists
IF OBJECTPROPERTY(OBJECT_ID('[LogServer].[MoveMessageInstancesAndUpdateExpiration]'), 'IsProcedure') = 1
	DROP PROCEDURE [LogServer].[MoveMessageInstancesAndUpdateExpiration]
GO

-- Recreate MoveMessageInstancesAndUpdateExpiration procedure
CREATE PROCEDURE [LogServer].[MoveMessageInstancesAndUpdateExpiration]
@SystemLogRetention SMALLINT,
@AuditLogRetention SMALLINT,
@RuleLogRetention SMALLINT
AS
BEGIN

	DECLARE @messageInstanceExpirationThreshold TABLE (
		[LogTypeId] TINYINT PRIMARY KEY,
		[ExpirationThreshold] DATETIME
	)

	INSERT INTO @messageInstanceExpirationThreshold 
	([LogTypeId], [ExpirationThreshold])
	VALUES 
	(1, DATEADD(day, -@SystemLogRetention, GETUTCDATE())),
	(2, DATEADD(day, -@AuditLogRetention, GETUTCDATE())),
	(3, DATEADD(day, -@RuleLogRetention, GETUTCDATE()))

	-- Copy live messages instances (based on the new retention times) from [MessageInstance_Inactive] to [MessageInstance_0]
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
		MI.[Id],
		MI.[MessageTypeId],
		MI.[ClientTimestampUtc],
		MI.[ServerTimestampUtc],
		MI.[UtcExpires],
		MI.[SourceName],
		MI.[User],
		MI.[UserLocation],
		MI.[RuleName],
		MI.[ServiceName],
		MI.[EventType],
		MI.[LogLevel],
		MI.[Permission]
	FROM [LogServer].[MessageInstance_Inactive] AS MI
	JOIN [LogServer].[MessageType] AS MT ON MI.[MessageTypeId] = MT.[Id]
	JOIN @messageInstanceExpirationThreshold AS MIET ON MT.[LogTypeId] = MIET.[LogTypeId]
	WHERE MIET.[ExpirationThreshold] < MI.[ClientTimestampUtc]

	-- Update [UtcExpires] on all moved message instances based on the new retention times
	DECLARE @messageInstanceRetention TABLE (
		[LogTypeId] TINYINT PRIMARY KEY,
		[RetentionDays] SMALLINT
	)

	INSERT INTO @messageInstanceRetention
	([LogTypeId], [RetentionDays])
	VALUES 
	(1, @SystemLogRetention),
	(2, @AuditLogRetention),
	(3, @RuleLogRetention)

	UPDATE MI
	SET [UtcExpires] = DATEADD(day, MIR.[RetentionDays], [ClientTimestampUtc])
	FROM [LogServer].[MessageInstance_0] AS MI
	JOIN [LogServer].[MessageType] AS MT ON MI.[MessageTypeId] = MT.[Id]
	JOIN @messageInstanceRetention AS MIR ON MT.[LogTypeId] = MIR.[LogTypeId]

END 
GO