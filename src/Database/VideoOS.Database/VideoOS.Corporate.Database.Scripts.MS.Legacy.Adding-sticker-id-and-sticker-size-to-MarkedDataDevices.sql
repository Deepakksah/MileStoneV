-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Adding Size column as nullable
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[MarkedDataDevices]') AND name = N'Size')
ALTER TABLE [MarkedDataDevices]
ADD
    [Size] BIGINT NULL
GO

-- Put 0 where Size is null (most likely everywhere)
UPDATE [MarkedDataDevices] SET Size=0
WHERE Size IS NULL
GO

-- Make Size not null
ALTER TABLE [MarkedDataDevices]
ALTER COLUMN
    [Size] BIGINT NOT NULL
GO
