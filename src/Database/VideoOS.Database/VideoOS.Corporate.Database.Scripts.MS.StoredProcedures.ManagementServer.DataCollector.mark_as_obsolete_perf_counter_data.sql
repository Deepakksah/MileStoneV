-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[mark_as_obsolete_perf_counter_data]') AND type in (N'P', N'PC'))
DROP PROCEDURE [mark_as_obsolete_perf_counter_data]
GO

CREATE PROCEDURE [mark_as_obsolete_perf_counter_data]
    @Instances tvp_StringTableNvarchar READONLY
 AS
BEGIN
	UPDATE [PerformanceCounterInstance] 
	SET [is_obsolete] = 1
	WHERE EXISTS
	(
		SELECT TOP 1 instances.Id
		FROM @Instances instances 
		WHERE instances.[Id] = [PerformanceCounterInstance].path
	) AND 
	(NOT EXISTS (
			SELECT TOP 1 d.IDDevice
			FROM Devices d
			WHERE d.DeviceType = 'Camera' AND d.IDDevice = [PerformanceCounterInstance].counter_id
	)) AND
	[PerformanceCounterInstance].counter_id is not null
END
GO