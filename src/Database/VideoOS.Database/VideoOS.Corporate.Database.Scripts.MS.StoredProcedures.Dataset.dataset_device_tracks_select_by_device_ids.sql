-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dataset_device_tracks_select_by_device_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dataset_device_tracks_select_by_device_ids]
GO

CREATE PROCEDURE [dataset_device_tracks_select_by_device_ids]
	@ids XML
AS
BEGIN
	SELECT [IDDevice], [IDTrack], [IDTrackDefinition], [MergeEdge], [DefaultPlayback], [InUse], [OnlyUseMediaAfter], [CreatedDate]
	FROM [DeviceTrack] dt
	INNER JOIN XmlToIdTable(@ids) ids ON dt.[IDDevice] = ids.[ID]
END
GO