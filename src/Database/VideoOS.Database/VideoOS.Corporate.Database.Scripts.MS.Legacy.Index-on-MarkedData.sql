-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[MarkedData]') AND name = N'IX_MarkedData_StartTime')
CREATE NONCLUSTERED INDEX [IX_MarkedData_StartTime] ON [dbo].[MarkedData] 
( 
	[StartTime] ASC 
) 
INCLUDE ([IDMarkedData], [MarkDataType], [EndTime])
GO
