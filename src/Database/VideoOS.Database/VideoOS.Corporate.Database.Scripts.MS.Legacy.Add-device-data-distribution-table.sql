-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.tables WHERE [object_id] = OBJECT_ID(N'DeviceDataDistribution'))
BEGIN
	CREATE TABLE [dbo].[DeviceDataDistribution]
	(
		[device_id] [GuidType] NOT NULL,
		[recorder_id] [GuidType] NOT NULL,

		CONSTRAINT [PK_DeviceDataDistribution] PRIMARY KEY CLUSTERED ([device_id], [recorder_id]),

		CONSTRAINT [FK_DeviceDataDistribution_Devices] FOREIGN KEY ([device_id]) REFERENCES [Devices] ([IDDevice]),
		CONSTRAINT [FK_DeviceDataDistribution_Recorders] FOREIGN KEY ([recorder_id]) REFERENCES [Recorders] ([IDRecorder]),
	)
END
GO
