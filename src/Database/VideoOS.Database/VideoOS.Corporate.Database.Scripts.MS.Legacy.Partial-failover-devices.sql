-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.columns WHERE [object_id] = OBJECT_ID(N'Devices') AND name = N'FailoverLiveOnly')
BEGIN
	ALTER TABLE [Devices]
	ADD [FailoverLiveOnly] BIT NULL
END
GO

UPDATE [Devices]
SET [FailoverLiveOnly] = 0
WHERE [FailoverLiveOnly] IS NULL
GO

ALTER TABLE [Devices]
ALTER COLUMN [FailoverLiveOnly] BIT NOT NULL
GO



IF NOT EXISTS (SELECT * FROM sys.columns WHERE [object_id] = OBJECT_ID(N'Devices') AND name = N'DisableOnFailover')
BEGIN
	ALTER TABLE [Devices]
	ADD [DisableOnFailover] BIT NOT NULL DEFAULT 0
END
GO

UPDATE [Devices]
SET [DisableOnFailover] = 0
WHERE [DisableOnFailover] IS NULL
GO

ALTER TABLE [Devices]
ALTER COLUMN [DisableOnFailover] BIT NOT NULL
GO
