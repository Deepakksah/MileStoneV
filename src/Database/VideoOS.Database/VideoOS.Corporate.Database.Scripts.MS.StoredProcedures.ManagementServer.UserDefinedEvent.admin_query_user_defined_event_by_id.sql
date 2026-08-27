-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_query_user_defined_event_by_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_query_user_defined_event_by_id]
GO

CREATE PROCEDURE [admin_query_user_defined_event_by_id]
@IDEventType [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		[IDEventType],
		[Name],
		[Definition],
		[GeneratorSubType],
		[LastModified]
	FROM [EventTypes]
	WHERE [IDEventType] = @IDEventType 
				AND [GeneratorType] = 'External'
END
GO
