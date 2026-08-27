-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

INSERT [PerformanceCounterType]([counter_direction], [enable_history], [trigger_per_value], [counter_type], [category_name], [counter_name])
SELECT 0, 0, 1, 11, 'VideoOS Recording Server Storage Archive', 'ArchiveAvailable'

INSERT [PerformanceCounterType]([counter_direction], [enable_history], [trigger_per_value], [counter_type], [category_name], [counter_name])
SELECT 0, 0, 1, 12, 'VideoOS Recording Server Storage Database', 'DatabaseAvailable'

GO