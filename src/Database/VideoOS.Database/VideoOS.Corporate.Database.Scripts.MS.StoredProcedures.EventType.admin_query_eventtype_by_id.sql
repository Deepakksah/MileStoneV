-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_query_eventtype_by_id]') AND type = 'P')
DROP PROCEDURE [admin_query_eventtype_by_id]
GO

CREATE PROCEDURE [dbo].[admin_query_eventtype_by_id]
	@idEventType [UniqueIdentifier]
AS
BEGIN
	SET NOCOUNT ON

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
	WHERE [IDEventType] = @idEventType
END
GO