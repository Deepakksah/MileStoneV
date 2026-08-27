-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF OBJECT_ID (N'ufn_query_performance_data', N'IF') IS NOT NULL
DROP FUNCTION ufn_query_performance_data;
GO

CREATE FUNCTION ufn_query_performance_data(@start_time DATETIME, @end_time DATETIME, @number_of_values INT)
RETURNS TABLE
AS
RETURN
(
	WITH TimeCTE
	AS
	(
		SELECT @start_time AS [collection_time]

		UNION ALL

		SELECT DATEADD(mi, DATEDIFF(MINUTE, @start_time, @end_time) / @number_of_values, [collection_time])
		FROM TimeCTE
		WHERE [collection_time] <= DATEADD(mi, -DATEDIFF(MINUTE, @start_time, @end_time) / @number_of_values, @end_time)
	)
	SELECT 
		c.[collection_time],
		pci.[pkid] as [instance_id],
		AVG(CASE WHEN pct.[calculated_value] IS NULL THEN 0 ELSE pct.[calculated_value] END) AS [calculated_value],
		MAX(CASE WHEN pct.[calculated_value] IS NULL THEN 0 ELSE pct.[calculated_value] END) AS [calculated_value_max],
		COUNT(pct.[calculated_value]) AS [number_of_values]
	FROM TimeCTE c
	CROSS JOIN [PerformanceCounterInstance] pci
	LEFT JOIN [PerformanceCounterValue] pct ON 
					pct.[collection_time] > DATEADD(mi, -DATEDIFF(MINUTE, @start_time, @end_time) / @number_of_values, c.[collection_time]) 
			AND pct.[collection_time] <= c.[collection_time]
			AND pci.[pkid] = pct.instance_id
	GROUP BY pci.[pkid], c.[collection_time]
	--OPTION(MAXRECURSION 10000);
)
GO
