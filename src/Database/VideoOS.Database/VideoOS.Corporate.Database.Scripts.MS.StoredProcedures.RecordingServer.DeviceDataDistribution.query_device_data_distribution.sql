-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_data_distribution]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_data_distribution]
GO

CREATE PROCEDURE [query_device_data_distribution]
	@device_id [GuidType] = NULL,
	@recorder_id [GuidType] = NULL,
	@track_id [UNIQUEIDENTIFIER] = NULL
 AS
BEGIN
	SET NOCOUNT ON

	SELECT	[device_id], [recorder_id], [track_id]
	FROM	[DeviceDataDistribution]
	WHERE	(@device_id IS NULL OR [device_id] = @device_id) AND
			(@track_id IS NULL OR [track_id] = @track_id) AND
			(@recorder_id IS NULL OR [recorder_id] = @recorder_id)
END
GO
