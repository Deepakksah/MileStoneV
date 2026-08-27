-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_external_smart_client_plugin_by_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_external_smart_client_plugin_by_id]
GO

CREATE PROCEDURE [query_external_smart_client_plugin_by_id]
	@IDPlugin [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		[PluginID], [Data]
	FROM
		[ExternalPlugins]
	WHERE
		([PluginType] = 'SMARTCLIENT' OR [PluginType] = 'CLIENT') AND [PluginID] = @IDPlugin
END
GO