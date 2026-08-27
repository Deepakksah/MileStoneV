-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_view_by_monitor_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_view_by_monitor_id]
GO

CREATE PROCEDURE [query_view_by_monitor_id]
	@monitor_id [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		v.[pkid],
		v.[name],
		v.[description],
		v.[created_date],
		v.[last_modified],
		v.[definition_xml]
	FROM [VideoWallView] v
	WHERE v.[pkid] IN (SELECT view_id FROM VideoWallMonitor m WHERE m.[pkid] = @monitor_id)
END
GO