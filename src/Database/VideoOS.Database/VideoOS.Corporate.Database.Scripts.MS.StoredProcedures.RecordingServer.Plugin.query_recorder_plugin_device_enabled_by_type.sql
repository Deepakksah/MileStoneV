-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_recorder_plugin_device_enabled_by_type]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_recorder_plugin_device_enabled_by_type]
GO

CREATE PROCEDURE [query_recorder_plugin_device_enabled_by_type]
	@IDRecorderPlugin [GuidType]
 AS
BEGIN
	SET NOCOUNT ON

	SELECT
		[IDDevice], [IDRecorderPlugin], [Settings]
	FROM
		[RecorderPluginDevice]
	WHERE
		([IDRecorderPlugin] = @IDRecorderPlugin) AND ([Enabled] = 1)
END
GO
