-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[PerformanceCounterInstance]') AND name = N'IX_PerformanceCounterInstanceCategory')
DROP INDEX [IX_PerformanceCounterInstanceCategory] ON [dbo].[PerformanceCounterInstance]
GO

CREATE NONCLUSTERED INDEX [IX_PerformanceCounterInstanceCategory] ON [dbo].[PerformanceCounterInstance]
(
	[category_name] ASC,
	[counter_name] ASC,
	[instance_name] ASC
)
INCLUDE ([pkid])
GO
