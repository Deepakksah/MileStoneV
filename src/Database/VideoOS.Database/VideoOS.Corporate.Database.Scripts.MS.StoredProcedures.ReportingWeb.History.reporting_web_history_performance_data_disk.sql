-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[reporting_web_history_performance_data_disk]') AND type in (N'P', N'PC'))
DROP PROCEDURE [reporting_web_history_performance_data_disk]
GO

CREATE PROCEDURE [reporting_web_history_performance_data_disk]
	@time_scale NVARCHAR(100),
	@disk_name NVARCHAR(100),
	@IDRecorder GuidType,
	@offset_minutes INT,
	@max_value BIGINT
AS
BEGIN
	DECLARE @utc_start_time DATETIME
	DECLARE @utc_end_time DATETIME
	DECLARE @number_of_values INT
	DECLARE @interval INT
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
	
	DECLARE @client_local_time DATETIME
	SELECT @client_local_time = DATEADD(minute, @offset_minutes, GETUTCDATE()) 
	
	DECLARE @client_local_date DATETIME
	SELECT @client_local_date = CONVERT(DateTime, CONVERT(VARCHAR(10), @client_local_time, 103), 103)
	
	IF(@time_scale = 'Day')
	BEGIN
				-- Remove min, sec, millisecond from time
		DECLARE @tempDate DateTime
		SELECT @tempDate = DATEADD(HOUR, DATEPART(HOUR, @client_local_time), @client_local_date)
		
		-- Adjust to UTC for data layer
		SELECT @utc_end_time = DATEADD(minute, -@offset_minutes, @tempDate)
		SET @utc_start_time = @utc_end_time - DAY(0)
		SELECT @number_of_values = 48
	END
	ELSE IF (@time_scale = 'Week')
	BEGIN
		-- Adjust to UTC time
		SELECT @utc_end_time = DATEADD(minute, -@offset_minutes, @client_local_date)
		SET @utc_start_time = @utc_end_time - DAY(6) 
		SELECT @number_of_values = 84
	END
	ELSE IF (@time_scale = 'Month')
	BEGIN
			-- Adjust to UTC time
		SELECT @utc_end_time = DATEADD(minute, -@offset_minutes, @client_local_date)
		SET @utc_start_time = @utc_end_time - DAY(29) 
		SELECT @number_of_values = 90
	END
	
	SELECT MediaBytes
		, (@max_value - MediaBytes - OtherBytes) AS FreeBytes
		, dtOtherBytes.OtherBytes
		, dtMediaBytes.collection_time
	FROM
	(
		SELECT calculated_value as MediaBytes, collection_time
		FROM ufn_query_performance_data(@utc_start_time, @utc_end_time, @number_of_values) data
		WHERE data.instance_id =
		(
			SELECT pkid 
			FROM PerformanceCounterInstance pci 
			WHERE pci.instance_name like '%' + dbo.EscapeWildcards(@disk_name) + '%' ESCAPE '\'
			AND pci.instance_name like '%' + @IDRecorder + '%'
			AND counter_name = 'Media Data Bytes'
			AND data_collector_id = @CollectorId
		)
	) AS dtMediaBytes
	OUTER APPLY
	(
		SELECT calculated_value AS OtherBytes
			   , collection_time
		FROM ufn_query_performance_data(@utc_start_time, @utc_end_time, @number_of_values) data
		WHERE data.instance_id =
		(
			SELECT pkid 
			FROM PerformanceCounterInstance pci 
			WHERE pci.instance_name like '%' + dbo.EscapeWildcards(@disk_name) +'%' ESCAPE '\'
			AND pci.instance_name like '%' + @IDRecorder + '%'
			AND counter_name = 'Other Data Bytes'
			AND data_collector_id = @CollectorId
		)
		AND collection_time = dtMediaBytes.collection_time
	) AS dtOtherBytes
	ORDER by collection_time
	OPTION(MAXRECURSION 10000);
END
GO