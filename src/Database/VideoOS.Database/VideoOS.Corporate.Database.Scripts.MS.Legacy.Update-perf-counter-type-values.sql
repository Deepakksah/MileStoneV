-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

INSERT [PerformanceCounterType]([counter_direction], [enable_history], [trigger_per_value], [counter_type], [category_name], [counter_name])
SELECT 1, 0, 1, 10, 'VideoOS Recording Server Device', 'State'
	
UPDATE [PerformanceCounterType] 
SET [enable_history] = 0
WHERE 
		[PerformanceCounterType].[counter_type] = 3 OR -- live fps 47AD1879-4F04-4E71-8EE8-6F9D2DAC9148
		[PerformanceCounterType].[counter_type] = 4 OR  -- recording fps 2917B174-7E8E-490E-A402-6F5A76E4F9DD
		[PerformanceCounterType].[counter_type] = 8 -- service available 063EC923-9E6B-4A95-B949-AE5A76DB8B5C
GO
