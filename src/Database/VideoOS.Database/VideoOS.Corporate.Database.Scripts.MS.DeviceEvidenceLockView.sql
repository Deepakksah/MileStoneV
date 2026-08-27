-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================


IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[deviceEvidenceLockAsDataSet]') AND [type] IN (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [deviceEvidenceLockAsDataSet]
GO

IF EXISTS(SELECT * FROM sys.views WHERE [object_id] = OBJECT_ID(N'[dbo].[DeviceEvidenceLock]'))
DROP VIEW [dbo].[DeviceEvidenceLock]
GO


IF EXISTS(SELECT * FROM sys.views WHERE [object_id] = OBJECT_ID(N'[dbo].[DeviceTrackEvidenceLock]'))
DROP VIEW [dbo].[DeviceTrackEvidenceLock]
GO

CREATE VIEW [dbo].[DeviceTrackEvidenceLock] 
AS
SELECT 
	md.[IDMarkedData],
	mdd.[IDDevice],
	dt.[IDTrack],
	md.[StartTime], 
	md.[EndTime], 
	h.[IDRecorder]
FROM [MarkedData] md
	INNER JOIN [MarkedDataDevices] mdd ON md.[IDMarkedData] = mdd.[IDMarkedData]
	INNER JOIN [Devices] d ON mdd.[IDDevice] = d.[IDDevice]
	INNER JOIN [DeviceTrack] dt ON mdd.[IDDevice] = dt.[IDDevice]
	INNER JOIN [Hardware] h ON h.[IDHardware] = d.[IDHardware]
WHERE md.[MarkDataType] = 2
UNION ALL
SELECT 
	md.[IDMarkedData],
	mdd.[IDDevice],
	CAST(ddd.[track_id] AS UNIQUEIDENTIFIER) AS IDTrack,
	md.[StartTime], 
	md.[EndTime], 
	ddd.[recorder_id] AS IDRecorder
FROM [MarkedData] md
	INNER JOIN [MarkedDataDevices] mdd ON md.[IDMarkedData] = mdd.[IDMarkedData]
	INNER JOIN [DeviceDataDistribution] ddd ON mdd.[IDDevice] = ddd.[device_id]
WHERE md.[MarkDataType] = 2

GO
