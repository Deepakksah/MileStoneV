-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[reporting_web_query_failover_incidence]') AND type in (N'P', N'PC'))
DROP PROCEDURE [reporting_web_query_failover_incidence]
GO

CREATE PROCEDURE [reporting_web_query_failover_incidence]
	@time_scale NVARCHAR(100),
	@IDFailover GuidType,
	@time_offset FLOAT
AS
BEGIN
	DECLARE @start_time DATETIME
	DECLARE @timeInMinutes INT 
	SET @timeInMinutes = @time_offset * 60
	
	IF(@time_scale = 'Day')
	BEGIN
		SET @start_time = GETUTCDATE() - DAY(0)
	END
	ELSE IF (@time_scale = 'Week')
	BEGIN
		SET @start_time = GETUTCDATE() - DAY(6)
	END
	ELSE IF (@time_scale = 'Month')
	BEGIN
		SET @start_time = GETUTCDATE() - DAY(29)
	END
	
	SELECT rec.Name as RecorderName
	  ,DATEADD(minute, @timeInMinutes, fh.[StartTime]) AS StartTime
	  ,DATEADD(minute, @timeInMinutes, fh.[EndTime]) AS EndTime
	FROM [FailoverHistory] fh
	INNER JOIN Recorders rec ON fh.IDRecorder = rec.IDRecorder 
	WHERE IDFailover = @IDFailover
	AND (EndTime > @start_time OR EndTime IS NULL)
	ORDER BY StartTime DESC
END
GO