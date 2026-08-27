-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- The SUPPORTS_TRANSACTION property specifies if the scripts supports
-- running in multi-statement transaction or if it should be running
-- using implicit transactions
-- =========================================================================
-- TARGET: Surveillance
-- SUPPORTS_TRANSACTION: TRUE
-- =========================================================================

-- Add column to devices table for device index
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[DeviceDataDistribution]') AND name = N'track_id')
ALTER TABLE [DeviceDataDistribution]
  ADD [track_id] [GuidType] NULL
GO


ALTER TABLE dbo.DeviceDataDistribution
	DROP CONSTRAINT PK_DeviceDataDistribution
GO


UPDATE	[DeviceDataDistribution]
SET		[track_id] = [device_id]
WHERE	[track_id] IS NULL
GO


ALTER TABLE [DeviceDataDistribution]
  ALTER COLUMN [track_id] [GuidType] NOT NULL
GO


ALTER TABLE dbo.DeviceDataDistribution ADD CONSTRAINT
	PK_DeviceDataDistribution PRIMARY KEY CLUSTERED 
	(
		recorder_id,
		track_id
	)
GO
