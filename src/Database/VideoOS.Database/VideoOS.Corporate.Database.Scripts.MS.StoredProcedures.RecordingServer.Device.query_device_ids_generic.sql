-- This header is used by the dbupdate tool]
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on] The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes]
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_ids_generic]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_ids_generic]
GO

CREATE PROCEDURE [query_device_ids_generic]
	@IDRecorder [GuidType],
	@IDHardware [GuidType],
	@DeviceTypeSearch VARCHAR(128)
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		d.[IDDevice]
	FROM 
		[Devices] d
		INNER JOIN [Hardware] h ON h.[IDHardware] = d.[IDHardware]
	WHERE 
		(@IDRecorder IS NULL OR @IDRecorder = '' OR h.[IDRecorder] = @IDRecorder)
		AND 
		(@IDHardware IS NULL OR @IDHardware = '' OR h.[IDHardware] = @IDHardware)
		AND 
		(
			(@DeviceTypeSearch = 'All' OR @DeviceTypeSearch = '' OR @DeviceTypeSearch IS NULL)
			OR
			(@DeviceTypeSearch = 'RecordingDevice' AND d.[DeviceType] IN ('Camera', 'AudioInput', 'AudioOutput', 'Metadata')) 
			OR
			(@DeviceTypeSearch = 'Camera' AND d.[DeviceType] = 'Camera')
			OR
			(@DeviceTypeSearch = 'Microphone' AND d.[DeviceType] = 'AudioInput')
			OR
			(@DeviceTypeSearch = 'Metadata' AND d.[DeviceType] = 'Metadata')
			OR
			(@DeviceTypeSearch = 'Speaker' AND d.[DeviceType] = 'AudioOutput')
			OR
			(@DeviceTypeSearch = 'Input' AND d.[DeviceType] = 'Input')
			OR
			(@DeviceTypeSearch = 'Output' AND d.[DeviceType] = 'Output')
		)
END
GO
