-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[reporting_web_device_instances]') AND type in (N'P', N'PC'))
DROP PROCEDURE [reporting_web_device_instances]
GO

CREATE PROCEDURE [reporting_web_device_instances]
	@device_id GuidType
AS
BEGIN
	DECLARE @IDRecorder GuidType
	SET @IDRecorder = 
	(
		SELECT h.IDRecorder
		FROM Devices d
		INNER JOIN Hardware h ON d.IDHardware = h.IDHardware
		WHERE d.IDDevice = @device_id
	)

	DECLARE @CollectorId INT
	SET @CollectorId = 
	(
		SELECT dc.[pkid]
		FROM [DataCollector] dc 
		INNER JOIN [DataCollectorInstalledServices] dis ON dc.[pkid] = dis.[data_collector_id]
		INNER JOIN [RecordingServerService] rss ON rss.[host_name] = dc.[host_name]
		INNER JOIN [RecordingServerServiceRecorder] rssr ON rss.[pkid] = rssr.[recording_server_service_id] AND rssr.[type] = 0
		WHERE dis.[service_name] = 'Recorder'
		AND rssr.[recorder_id] = @IDRecorder
	)
	
	SELECT NetworkLoadInstanceId = 
	(
		SELECT [pkid]
  		FROM [PerformanceCounterInstance]
  		WHERE category_name = 'VideoOS Recording Server Drivers'
  		AND counter_name = 'Bytes/sec'
  		AND counter_id = @device_id
		AND data_collector_id = @CollectorId
		AND is_obsolete = 0
	)
	, StorageUsageInstanceId =
	(
		SELECT [pkid]
  		FROM [PerformanceCounterInstance]
  		WHERE category_name = 'VideoOS Recording Server Device Storage'
  		AND counter_name = 'Used Bytes'
  		AND counter_id = @device_id 
		AND data_collector_id = @CollectorId
		AND is_obsolete = 0
	)
END
GO
