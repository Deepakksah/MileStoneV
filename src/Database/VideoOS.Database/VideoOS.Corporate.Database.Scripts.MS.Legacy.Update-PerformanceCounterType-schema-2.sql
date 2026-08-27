-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[PerfomanceCounterType]') AND type = 'U')
BEGIN
IF (SELECT COUNT([counter_type]) FROM [PerfomanceCounterType] WHERE counter_type = 7) = 0
BEGIN
	INSERT [PerfomanceCounterType]([counter_direction], [counter_type], [category_name], [counter_name])
	SELECT 0, 7, 'Retention Time', 'Days'
END
	
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[PerfomanceCounterType]') AND type = 'U')
BEGIN
IF (SELECT COUNT([counter_type]) FROM [PerfomanceCounterType] WHERE category_name = 'VideoOS Recording Server Storage') = 0
BEGIN
	INSERT [PerfomanceCounterType]([counter_direction], [counter_type], [category_name], [counter_name])
	SELECT 1, 0, 'VideoOS Recording Server Storage', 'Used Bytes'
END
	
END
GO