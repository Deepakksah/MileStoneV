-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[get_data_collector_configuration]') AND type = 'P')
DROP PROCEDURE [get_data_collector_configuration]
GO

CREATE PROCEDURE [get_data_collector_configuration]
	@data_collector_id INT
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	--read sample frequency
	DECLARE @sameple_frequency BIGINT
	DECLARE @sameple_frequency_retentiontime BIGINT
	DECLARE @enable_history NVARCHAR(10)  

	SELECT @sameple_frequency = ss.[Setting]
	FROM [SystemSettings] ss
	WHERE ss.Name = 'DashboardHistorySamplingIntervalSeconds'

	SELECT @enable_history = ss.[Setting]
	FROM [SystemSettings] ss
	WHERE ss.Name = 'DashboardHistoryEnabled'

	IF (@sameple_frequency > 600) -- 10 min is max time for performance counter that is used in retention rule
	BEGIN
		SET @sameple_frequency_retentiontime = 600
	END
	ELSE 
	BEGIN 
		SET @sameple_frequency_retentiontime = @sameple_frequency
	END

	SELECT dc.[category_name], 
		 dc.[counter_name], 
		 dc.[instance_name], 
		 sample_frequency  = 
		    CAST(CASE WHEN (dc.category_name = 'VideoOS Recording Server Storage' AND dc.[counter_name] = 'Used Bytes') 
				THEN @sameple_frequency_retentiontime
				ELSE @sameple_frequency
			END as INT),
		 pt.[counter_direction], 
		 enable_history = 
			CAST(CASE WHEN (dc.category_name = 'VideoOS Recording Server Storage' AND dc.[counter_name] = 'Used Bytes') 
				THEN 1
				ELSE CASE WHEN (@enable_history = 'False') 
					      THEN 0
						  ELSE pt.[enable_history]
					END
			END AS BIT),
		 pt.[trigger_per_value] 
	FROM [DataCollectorConfiguration] as dc
	INNER JOIN [PerformanceCounterType] as pt 
	on dc.category_name = pt.[category_name] AND dc.counter_name = pt.[counter_name]
	WHERE [data_collector_id] = @data_collector_id
END
GO