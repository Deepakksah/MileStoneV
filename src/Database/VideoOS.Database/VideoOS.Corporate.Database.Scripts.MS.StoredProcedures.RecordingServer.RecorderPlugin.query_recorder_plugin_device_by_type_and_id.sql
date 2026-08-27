-- This header is used by the dbupdate tool.
-- The TARGET poperty is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_recorder_plugin_device_by_type_and_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_recorder_plugin_device_by_type_and_id]
GO

CREATE PROCEDURE [dbo].[query_recorder_plugin_device_by_type_and_id]
	@IDRecorderPlugin [GuidType],
	@IDDevice [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	SELECT 
		[IDRecorderPlugin], [IDDevice], [Settings]  --, [ProcessOrder], [Enabled], [LastModified] 
	FROM
		 [RecorderPluginDevice]
	WHERE [IDRecorderPlugin] = @IDRecorderPlugin AND [IDDevice] = @IDDevice
END
GO
