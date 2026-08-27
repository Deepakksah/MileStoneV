-- This header is used by the dbupdate tool]
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on] The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes]
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_ids_with_media_on_recorder]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_ids_with_media_on_recorder]
GO

CREATE PROCEDURE [query_device_ids_with_media_on_recorder]
	@IDRecorder [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		d.[IDDevice]
	FROM 
		[Devices] d
		INNER JOIN [Hardware] h ON h.[IDHardware] = d.[IDHardware]
		INNER JOIN [RegisteredDeviceTypes] rdt ON rdt.[DeviceType] = d.[DeviceType]
	WHERE 
		(h.[IDRecorder] = @IDRecorder AND rdt.IsRecordingDevice = 1)
	UNION ALL
	SELECT DISTINCT
	   ddd.[device_id] AS IDDevice
	FROM
		[DeviceDataDistribution] ddd
	WHERE 
		(ddd.[recorder_id] = @IDRecorder)
END
GO
