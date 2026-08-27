-- This header is used by the dbupdate tool]
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on] The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes]
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[device_tracks_query_by_device]') AND type in (N'P', N'PC'))
DROP PROCEDURE [device_tracks_query_by_device]
GO

CREATE PROCEDURE [device_tracks_query_by_device]
	@IDDevice [GuidType]
 AS
BEGIN
	SET NOCOUNT ON

	SELECT 
		d.[IDDevice],
		d.[IDTrack],
		d.[IDTrackDefinition],
		d.[MergeEdge],
		d.[DefaultPlayback],
		d.[InUse]
		from [DeviceTrack] d
	WHERE d.[IDDevice] = @IDDevice

END
GO
