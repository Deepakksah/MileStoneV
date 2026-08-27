-- This header is used by the dbupdate tool]
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on] The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes]
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[recording_server_has_locked_device_data_distribution]') AND type in (N'P', N'PC'))
DROP PROCEDURE [recording_server_has_locked_device_data_distribution]
GO

CREATE PROCEDURE [recording_server_has_locked_device_data_distribution]
	@recorder_id [GuidType]
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT 
		CASE WHEN EXISTS
		(
			SELECT	1
			FROM	[DeviceDataDistribution] ddd
					INNER JOIN [MarkedDataDevices] mdd ON mdd.[IDDevice] = ddd.[device_id]
					INNER JOIN [MarkedData] md ON md.[IDMarkedData] = mdd.[IDMarkedData]
			WHERE	ddd.recorder_id = @recorder_id
					AND md.[MarkDataType] = 2
		)
		THEN CAST(1 AS BIT)
		ELSE CAST(0 AS BIT)
		END
END
GO