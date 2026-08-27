-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[reporting_web_recorder_overview]') AND type in (N'P', N'PC'))
DROP PROCEDURE [reporting_web_recorder_overview]
GO

CREATE PROCEDURE [reporting_web_recorder_overview]
	@recorder_id NVARCHAR(100) 
AS
BEGIN
	SELECT TOP 1
		rs.[Name],
		rs.[IDRecorder],
		dc.[pkid] AS [CollectorId],
		dc.[host_name] AS [HostName],
		rs.[Enabled] AS [Authorized],
		CPU.CpuInstanceId AS CpuInstanceId,
		Memory.MemoryInstanceId AS MemoryInstanceId
	FROM 
		[Recorders] rs
		INNER JOIN [RecordingServerServiceRecorder] rssr ON rssr.[recorder_id] = rs.[IDRecorder]
		INNER JOIN [RecordingServerService] rss ON rss.[pkid] = rssr.[recording_server_service_id] AND rssr.[type] = 0
		INNER JOIN [DataCollector] dc ON dc.[host_name] = rss.[host_name]
		INNER JOIN [DataCollectorInstalledServices] dis ON dc.[pkid] = dis.[data_collector_id]
		OUTER APPLY 
			(
				SELECT pkid as CpuInstanceId
				FROM PerformanceCounterInstance 
				WHERE data_collector_id = dc.[pkid]
				AND category_name = 'Processor' AND counter_name = '% Processor Time' AND instance_name = '_Total'
			) AS CPU
			OUTER APPLY 
			(
				SELECT pkid as MemoryInstanceId
				FROM PerformanceCounterInstance 
				WHERE data_collector_id = dc.[pkid]
				AND category_name = 'Memory' AND counter_name = 'Available MBytes'
			) AS Memory
	WHERE
		dis.[service_name] = 'Recorder' AND rs.[IDRecorder] = @recorder_id
END
GO