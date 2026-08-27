IF OBJECT_ID ( '[LogServer].[WriteMessageInstance]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[WriteMessageInstance];
GO

CREATE PROCEDURE [LogServer].[WriteMessageInstance]
@MessageTypeId uniqueidentifier,
@ClientTimestampUtc datetime,
@SourceName nvarchar(400),
@User nvarchar(400),
@UserLocation nvarchar(400),
@RuleName nvarchar(400),
@ServiceName nvarchar(400),
@EventType nvarchar(400),
@LogLevel smallint,
@Permission smallint,
@MessageParameters MessageParameterTable READONLY
AS
BEGIN

	BEGIN TRANSACTION

	DECLARE @MessageInstanceId bigint
	DECLARE @RetentionDays int
	SET @RetentionDays = 
	(
		SELECT RC.RetentionDays 
		FROM [LogServer].[RetentionConfiguration] RC
		INNER JOIN [LogServer].[MessageType] MT ON RC.[LogTypeId] = MT.[LogTypeId] 
		WHERE MT.[Id] = @MessageTypeId
	); 

	INSERT INTO [LogServer].[MessageInstance_Active] ([MessageTypeId], [ClientTimestampUtc], [ServerTimestampUtc],
	[UtcExpires], [SourceName], [User], [UserLocation], [RuleName], [ServiceName], [EventType], [LogLevel], [Permission])
	VALUES (@MessageTypeId, @ClientTimestampUtc, SYSUTCDATETIME(), DATEADD(day, @RetentionDays, @ClientTimestampUtc),
			@SourceName, @User, @UserLocation, @RuleName, @ServiceName, @EventType, @LogLevel, @Permission)

	SET @MessageInstanceId = SCOPE_IDENTITY()

	INSERT INTO [LogServer].[MessageInstance_Active_Parameter]([MessageInstanceId], [Name], [Value])
	SELECT @MessageInstanceId, [Name], [Value]
	FROM @MessageParameters

	COMMIT TRANSACTION
END
GO

IF OBJECT_ID ( '[LogServer].[WriteMessageInstanceLegacy]', 'P' ) IS NOT NULL 
    DROP PROCEDURE [LogServer].[WriteMessageInstanceLegacy];
GO

CREATE PROCEDURE [LogServer].[WriteMessageInstanceLegacy]
@LegacyMessageTypeId nvarchar(400),
@ClientTimestampUtc datetime,
@SourceName nvarchar(400),
@User nvarchar(400),
@UserLocation nvarchar(400),
@RuleName nvarchar(400),
@ServiceName nvarchar(400),
@EventType nvarchar(400),
@LogLevel smallint,
@Permission smallint, 
@MessageParameters MessageParameterTable READONLY
AS
BEGIN

	DECLARE @MessageTypeId uniqueidentifier
	DECLARE @LogLevelToUse smallint

	SELECT @MessageTypeId = [Id], @LogLevelToUse = COALESCE([LogLevel], @LogLevel) 
	FROM [LogServer].[MessageTypeIdMapping] 
	WHERE [LegacyId] = @LegacyMessageTypeId
	-- @MessageTypeId will be null in case of an unknown message type, in which case the next call will fail

	EXEC [LogServer].[WriteMessageInstance] 
		@MessageTypeId, @ClientTimestampUtc, @SourceName, @User, @UserLocation, @RuleName, @ServiceName, 
		@EventType, @LogLevelToUse, @Permission, @MessageParameters
END
GO