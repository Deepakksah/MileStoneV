-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_MarkedData_Retention' AND object_id = OBJECT_ID('MarkedData'))
BEGIN
            CREATE INDEX IX_MarkedData_Retention ON MarkedData (RetentionType, RetentionExpire) INCLUDE (IDMarkedData)
END

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_IDDevice' AND object_id = OBJECT_ID('MarkedDataDevices'))
BEGIN
            CREATE NONCLUSTERED INDEX [IX_IDDevice] ON [MarkedDataDevices] ([IDDevice]) INCLUDE ([IDMarkedData])
END

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_ObjectId' AND object_id = OBJECT_ID('SecurityObject'))
BEGIN
            CREATE NONCLUSTERED INDEX IX_ObjectId ON [SecurityObject] ([object_id]) 
END
GO


