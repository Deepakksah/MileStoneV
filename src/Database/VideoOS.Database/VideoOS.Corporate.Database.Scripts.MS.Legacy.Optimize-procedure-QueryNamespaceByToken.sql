-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE name = 'IX_VideoWallMonitorPreset_MonitorId' AND object_id = OBJECT_ID('VideoWallMonitorPreset'))
BEGIN
	CREATE INDEX IX_VideoWallMonitorPreset_MonitorId
	ON VideoWallMonitorPreset (monitor_id)
END

GO
