-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

UPDATE [PerformanceCounterType]
SET [category_name] = 'VideoOS Recording Server Storage', [counter_name] = 'Used Bytes'
WHERE [counter_type] = 7
GO
