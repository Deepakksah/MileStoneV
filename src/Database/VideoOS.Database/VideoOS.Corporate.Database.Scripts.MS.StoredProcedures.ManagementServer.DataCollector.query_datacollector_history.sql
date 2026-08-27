-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_datacollector_history]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_datacollector_history]
GO

CREATE PROCEDURE [query_datacollector_history]
	@datacollector_id INT,
	@utc_start_time DATETIME,
	@utc_end_time DATETIME,
	@max_number_of_values INT,
	@counter_path NVARCHAR(100)
	
AS
BEGIN
	
	DECLARE @instance_id INT
	DECLARE @timeInMinutes INT 
	
	SELECT @instance_id = instance.pkid
	FROM [PerformanceCounterInstance] as instance
	WHERE instance.[data_collector_id] = @datacollector_id AND 
		  instance.[path] = @counter_path
	
	BEGIN
			SELECT calculated_value_max as calculated_value
				, collection_time
			FROM ufn_query_performance_data(@utc_start_time, @utc_end_time, @max_number_of_values)
			WHERE instance_id = @instance_id
			OPTION(MAXRECURSION 10000);
	
	END
	
END
GO