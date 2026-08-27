-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_query_user_defined_events_by_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_query_user_defined_events_by_ids]
GO

CREATE PROCEDURE [admin_query_user_defined_events_by_ids]
@IDEventIds tvp_StringTableGuidType READONLY
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		et.[IDEventType],
		et.[Name],
		et.[Definition],
		et.[GeneratorSubType],
		et.[LastModified]
	FROM [EventTypes] et
	INNER JOIN @IDEventIds ids
	ON et.[IDEventType] = ids.[ID]
	WHERE et.[GeneratorType] = 'External'
END
GO
