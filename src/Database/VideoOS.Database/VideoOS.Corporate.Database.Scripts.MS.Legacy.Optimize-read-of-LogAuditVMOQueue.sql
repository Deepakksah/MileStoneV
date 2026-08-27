-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Add index on TimeStampUTC column in [LogAuditVMOQueue] to improve read count, CPU usage and execution time


IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[LogAuditVMOQueue]') AND name = 'IX_LogAuditVMOQueue_TimeStampUTC')
CREATE CLUSTERED INDEX [IX_LogAuditVMOQueue_TimeStampUTC] ON [dbo].[LogAuditVMOQueue] ([TimeStampUTC] ASC)
GO