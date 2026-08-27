-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_query_plugin_event_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_query_plugin_event_ids]
GO

CREATE PROCEDURE [admin_query_plugin_event_ids]
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		[IDEventType]
	FROM [EventTypes]
	WHERE [GeneratorType] = 'MIP' OR [GeneratorType] = 'MIPDevice'
END
GO
