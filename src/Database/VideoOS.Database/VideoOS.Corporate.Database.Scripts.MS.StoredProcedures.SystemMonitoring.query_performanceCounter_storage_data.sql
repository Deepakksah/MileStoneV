-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_performanceCounter_storage_data]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_performanceCounter_storage_data]
GO

CREATE PROCEDURE [query_performanceCounter_storage_data]
	@instance_id INT,
	@time datetime,
	@days INT,
	@samples INT
AS	
BEGIN
  SET NOCOUNT ON
 
  DECLARE @utc_start_time DATETIME
  SET @utc_start_time = @time - ( DAY(@days) - 1 )
  
  IF EXISTS (SELECT TOP 1 * FROM PerformanceCounterValue WHERE instance_id = @instance_id)
	BEGIN
		SELECT calculated_value as calculated_value
			, collection_time as collection_time
		FROM ufn_query_performance_data(@utc_start_time, @time, @samples)
		WHERE instance_id = @instance_id
		OPTION(MAXRECURSION 10000);
	END
	ELSE
	BEGIN
		SELECT null as calculated_value, @utc_start_time as collection_time
		UNION
		SELECT null as calculated_value, @time as collection_time
	END
END
GO