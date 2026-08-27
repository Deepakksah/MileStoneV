-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dataset_recorder_plugin_device_select_by_id_and_devices]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dataset_recorder_plugin_device_select_by_id_and_devices]
GO

CREATE PROCEDURE [dataset_recorder_plugin_device_select_by_id_and_devices]
	@deviceIds XML,
	@recorderPluginId [GuidType]
AS
BEGIN
	SELECT [LastModified], [IDDevice], [ProcessOrder], [IDRecorderPlugin], [Settings], [Enabled]
	FROM [RecorderPluginDevice] rpd
	INNER JOIN XmlToIdTable(@deviceIds) ids ON rpd.[IDDevice] = ids.[ID]
	WHERE [IDRecorderPlugin] = @recorderPluginId
END
GO