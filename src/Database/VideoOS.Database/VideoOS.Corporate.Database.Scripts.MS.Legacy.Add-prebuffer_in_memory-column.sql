-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS ( SELECT *  FROM sys.columns  WHERE object_id = OBJECT_ID(N'[RecordingStorageDevice]') AND name = N'prebuffer_in_memory')
BEGIN
	ALTER TABLE [RecordingStorageDevice]
	ADD [prebuffer_in_memory] BIT NULL
END
GO

DECLARE @TimeLimitForPrebufferInRam AS INT
SELECT @TimeLimitForPrebufferInRam = CAST([Setting] AS INT)
FROM [SystemSettings]
WHERE [Name] = 'TimeLimitForPrebufferInRam'

UPDATE [RecordingStorageDevice]
SET [prebuffer_in_memory] = 0
WHERE [prebuffer_seconds] > @TimeLimitForPrebufferInRam

UPDATE [RecordingStorageDevice]
SET [prebuffer_in_memory] = 1
WHERE [prebuffer_seconds] <= @TimeLimitForPrebufferInRam
GO

ALTER TABLE [RecordingStorageDevice]
ALTER COLUMN [prebuffer_in_memory] BIT NOT NULL
GO
