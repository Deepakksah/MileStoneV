-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[reporting_web_details_adapter]') AND type in (N'P', N'PC'))
DROP PROCEDURE [reporting_web_details_adapter]
GO

CREATE PROCEDURE [reporting_web_details_adapter]
	@IDRecorder GuidType
AS
BEGIN
	DECLARE @data_collector_id INT
	SET @data_collector_id = 
	(
		SELECT dc.[pkid]
		FROM [DataCollector] dc 
		INNER JOIN [DataCollectorInstalledServices] dis ON dc.[pkid] = dis.[data_collector_id]
		INNER JOIN [RecordingServerService] rss ON rss.[host_name] = dc.[host_name]
		INNER JOIN [RecordingServerServiceRecorder] rssr ON rss.[pkid] = rssr.[recording_server_service_id] AND rssr.[type] = 0
		WHERE dis.[service_name] = 'Recorder'
		AND rssr.[recorder_id] = @IDRecorder
	)

	SELECT 
	AdapterName,
	at AS AdapterTotal,
	at_instance_id as AdapterTotalInstanceId,
	ar AS AdapterReceived,
	ar_instance_id as AdapterReceivedInstanceId,
	asent AS AdapterSent,
	asent_instance_id as AdapterSentInstanceId,
	ROW_NUMBER() OVER(ORDER BY ar_instance_id DESC) - 1 as [RemainingRows]
	FROM
	(
		SELECT DISTINCT
			pci2.instance_name as AdapterName,
			at =
			(	
				SELECT TOP 1 ROUND(perValue.calculated_value,2)
				FROM PerformanceCounterInstance pci
				OUTER APPLY
				(
					SELECT top 1 instance_id, calculated_value
					FROM PerformanceCounterValue
					WHERE instance_id = pci.pkid
					AND collection_time > DATEADD(week, -5, GETUTCDATE()) 
					ORDER BY collection_time DESC
				) AS perValue
				WHERE category_name = 'Network Interface'
				AND counter_name = 'Bytes Total/sec'
				AND instance_name = pci2.instance_name 
				AND pci.data_collector_id = @data_collector_id
				AND pci.is_obsolete = 0
			),
			at_instance_id =
			(
				SELECT TOP 1 pkid
				FROM PerformanceCounterInstance pci
				WHERE category_name = 'Network Interface'
				AND counter_name = 'Bytes Total/sec'
				AND instance_name = pci2.instance_name 
				AND pci.data_collector_id = @data_collector_id
				AND pci.is_obsolete = 0
			),
			ar =
			(	
				SELECT TOP 1 ROUND(perValue.calculated_value,2)
				FROM PerformanceCounterInstance pci
				OUTER APPLY
				(
					SELECT top 1 instance_id, calculated_value
					FROM PerformanceCounterValue
					WHERE instance_id = pci.pkid
					AND collection_time > DATEADD(week, -5, GETUTCDATE()) 
					ORDER BY collection_time DESC
				) AS perValue
				WHERE category_name = 'Network Interface'
				AND counter_name = 'Bytes Received/sec'
				AND instance_name = pci2.instance_name 
				AND pci.data_collector_id = @data_collector_id
				AND pci.is_obsolete = 0
			),
			ar_instance_id =
			(
				SELECT TOP 1 pkid
				FROM PerformanceCounterInstance pci
				WHERE category_name = 'Network Interface'
				AND counter_name = 'Bytes Received/sec'
				AND instance_name = pci2.instance_name 
				AND pci.data_collector_id = @data_collector_id
			),
			asent =
			(	
				SELECT TOP 1 ROUND(perValue.calculated_value,2)
				FROM PerformanceCounterInstance pci
				OUTER APPLY
				(
					SELECT top 1 instance_id, calculated_value
					FROM PerformanceCounterValue
					WHERE instance_id = pci.pkid
					AND collection_time > DATEADD(week, -5, GETUTCDATE()) 
					ORDER BY collection_time DESC
				) AS perValue
				WHERE category_name = 'Network Interface'
				AND counter_name = 'Bytes Sent/sec'
				AND instance_name = pci2.instance_name 
				AND pci.data_collector_id = @data_collector_id
				AND pci.is_obsolete = 0
			),
			asent_instance_id =
			(
				SELECT TOP 1 pkid
				FROM PerformanceCounterInstance pci
				WHERE category_name = 'Network Interface'
				AND counter_name = 'Bytes Sent/sec'
				AND instance_name = pci2.instance_name 
				AND pci.data_collector_id = @data_collector_id
				AND pci.is_obsolete = 0
			)
		FROM PerformanceCounterInstance pci2
		WHERE pci2.category_name = 'Network Interface'
		AND pci2.data_collector_id = @data_collector_id
		AND pci2.is_obsolete = 0
	) as DT
	ORDER BY [RemainingRows] DESC
END
GO