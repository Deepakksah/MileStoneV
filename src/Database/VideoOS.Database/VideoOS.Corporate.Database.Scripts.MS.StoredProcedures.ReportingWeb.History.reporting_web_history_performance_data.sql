-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[reporting_web_history_performance_data]') AND type in (N'P', N'PC'))
DROP PROCEDURE [reporting_web_history_performance_data]
GO

CREATE PROCEDURE [reporting_web_history_performance_data]
	@time_scale VARCHAR(128),
	@instance_id INT,
	@offset_minutes INT
AS
BEGIN
	DECLARE @utc_start_time DATETIME
	DECLARE @utc_end_time DATETIME
	DECLARE @interval INT
	DECLARE @number_of_values INT
	
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
	
	IF EXISTS (SELECT TOP 1 * FROM PerformanceCounterValue WHERE instance_id = @instance_id)
	BEGIN
		SELECT calculated_value, collection_time
		FROM ufn_query_performance_data(@utc_start_time, @utc_end_time, @number_of_values)
		WHERE instance_id = @instance_id
		OPTION(MAXRECURSION 10000);
	END
	ELSE
	BEGIN
		SELECT CAST(0 AS FLOAT) as calculated_value, @utc_start_time as collection_time
		UNION
		SELECT CAST(0 AS FLOAT) as calculated_value, @utc_end_time as collection_time
	END
END
GO