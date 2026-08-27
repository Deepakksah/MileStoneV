-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[DeviceTrackInfo]'))
DROP VIEW [dbo].[DeviceTrackInfo]
GO

CREATE VIEW [DeviceTrackInfo]
AS
  SELECT	CAST(DD.[recorder_id] AS UNIQUEIDENTIFIER) AS [IDRecorder],
			CAST(DD.[track_id] AS UNIQUEIDENTIFIER) AS [IDTrack],
			CAST(DD.[device_id] AS UNIQUEIDENTIFIER) AS [IDDevice],
			D.[Name],
			D.[DeviceType],
			DT.[IDTrackDefinition]
	FROM	[DeviceDataDistribution] DD
			INNER JOIN [Devices] D ON D.[IDDevice] = DD.[device_id]
			INNER JOIN [DeviceTrack] DT ON DT.[IDTrack] = DD.[track_id]
GO


