-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[PerformanceCounterType]') AND type = 'U')
BEGIN
	IF (SELECT COUNT([counter_type]) FROM [PerformanceCounterType] WHERE counter_type = 8) = 0
	BEGIN
		INSERT [PerformanceCounterType]([counter_direction], [counter_type], [category_name], [counter_name])
		SELECT 0, 8, 'Windows Services', NULL	
	END
	ELSE 
	BEGIN
		UPDATE [PerformanceCounterType]
		SET [counter_name] = NULL
		WHERE counter_type = 8
	END
		
END
GO
