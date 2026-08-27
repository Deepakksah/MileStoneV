-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_driver_by_service_and_number]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_driver_by_service_and_number]
GO

CREATE PROCEDURE [query_driver_by_service_and_number]
  @recording_server_service_id [GuidType],
  @driver_number INT
AS
BEGIN
	SET NOCOUNT ON

	SELECT [IDHardwareDriver]
		, [IDRecordingServerService]
		, [DriverName]
		, [DriverNumber]
		, [DriverVersion]
		, [XmlVersion]
		, [LastModified]
	FROM
		[HardwareDriverRecorder]
	WHERE
		[IDRecordingServerService] = @recording_server_service_id
		AND [DriverNumber] = @driver_number
END
GO