-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================



IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'PK_DeviceDataDistribution') AND parent_object_id = OBJECT_ID(N'[DeviceDataDistribution]'))
BEGIN
	ALTER TABLE [DeviceDataDistribution] DROP CONSTRAINT [PK_DeviceDataDistribution]
END
GO


IF EXISTS (SELECT * FROM sys.foreign_keys  WHERE object_id = OBJECT_ID(N'FK_DeviceDataDistribution_DeviceTrack') AND parent_object_id = OBJECT_ID(N'[DeviceDataDistribution]'))
BEGIN
	ALTER TABLE [DeviceDataDistribution] DROP CONSTRAINT [FK_DeviceDataDistribution_DeviceTrack]
END
GO


ALTER TABLE [DeviceDataDistribution] ALTER COLUMN [track_id] UNIQUEIDENTIFIER NOT NULL
GO


ALTER TABLE [DeviceDataDistribution] ADD CONSTRAINT
	[PK_DeviceDataDistribution] PRIMARY KEY CLUSTERED 
	(
		recorder_id,
		track_id
	)
GO


ALTER TABLE [DeviceDataDistribution] ADD CONSTRAINT
	[FK_DeviceDataDistribution_DeviceTrack] FOREIGN KEY ([track_id]) REFERENCES [DeviceTrack] ([IDTrack])

GO
