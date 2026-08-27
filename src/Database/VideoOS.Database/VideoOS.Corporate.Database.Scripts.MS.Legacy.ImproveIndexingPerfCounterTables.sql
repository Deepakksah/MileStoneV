-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_IsObsolete' AND object_id = OBJECT_ID('PerformanceCounterInstance'))
    BEGIN
		CREATE NONCLUSTERED INDEX [IX_IsObsolete] ON [dbo].[PerformanceCounterInstance]
		([is_obsolete] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
    END
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_InstanceId' AND object_id = OBJECT_ID('PerformanceCounterValue'))
    BEGIN
		CREATE NONCLUSTERED INDEX [IX_InstanceId] ON [dbo].[PerformanceCounterValue] 
		( instance_id ASC) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
    END
GO