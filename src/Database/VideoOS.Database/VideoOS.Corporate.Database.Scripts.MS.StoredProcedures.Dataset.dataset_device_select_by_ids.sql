
-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dataset_device_select_by_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dataset_device_select_by_ids]
GO

CREATE PROCEDURE [dataset_device_select_by_ids]
	@ids XML
 AS
BEGIN
	SELECT [LastModified], [IDDevice], [IDHardware], [Name], [Description], [Location], [DeviceType], [Enabled], [Settings], [Presets], [Shortcut], [MulticastEnabled], [Streams], [DeviceIndex], [SubType], [FailoverLiveOnly], [DisableOnFailover], [PtzManualSessionTimeout], [PtzReservedSessionTimeout], [PtzExtendedManualSessionTimeout], GisPoint.AsTextZM() AS [GisPointString], [ShortName], [Icon], [CoverageDirection], [CoverageDepth], [CoverageFieldOfView]
	FROM [Devices] d
	INNER JOIN XmlToIdTable(@ids) ids ON d.[IDDevice] = ids.[ID]
END
GO