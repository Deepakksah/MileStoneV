-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_query_eventtypes]') AND type = 'P')
DROP PROCEDURE [admin_query_eventtypes]
GO

CREATE PROCEDURE [dbo].[admin_query_eventtypes]
	@GeneratorTypes tvp_StringTableGuidType READONLY,
	@idGenerator dbo.GuidType
AS
BEGIN
	SET NOCOUNT ON

	IF @idGenerator IS NULL
		BEGIN
		SELECT
			[IDEventType],
			[Name],
			[Definition],
			[LastModified],
			[GeneratorType],
			[Builtin],
			[GeneratorSubType],
			[IDGenerator]
		FROM [EventTypes]
		INNER JOIN @GeneratorTypes ids ON [EventTypes].[GeneratorType] = ids.[ID]
		END
	ELSE
	BEGIN
		SELECT * FROM
		(SELECT
			[IDEventType],
			[Name],
			[Definition],
			[LastModified],
			[GeneratorType],
			[Builtin],
			[GeneratorSubType],
			[IDGenerator]
		FROM [EventTypes]
		WHERE [IDGenerator] = @idGenerator) et
		INNER JOIN @GeneratorTypes ids ON et.[GeneratorType] = ids.[ID]
	END
END
GO