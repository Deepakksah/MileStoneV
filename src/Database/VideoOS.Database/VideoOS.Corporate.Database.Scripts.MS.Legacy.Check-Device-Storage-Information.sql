-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

UPDATE rsd
SET rsd.[recordingstorage_id] = (SELECT defaultRS.pkid FROM RecordingStorage defaultRS WHERE recordingserver_id = r.IDRecorder AND is_default = 1)
FROM
	[Devices] d
	INNER JOIN [Hardware] h ON d.[IDHardware] = h.[IDHardware]
	INNER JOIN [Recorders] r ON r.[IDRecorder] = h.[IDRecorder]
	INNER JOIN [RecordingStorageDevice] rsd ON rsd.[device_id] = d.[IDDevice]
	INNER JOIN [RecordingStorage] rs ON rs.[pkid] = rsd.[recordingstorage_id]
WHERE rs.[recordingserver_id] <> r.[IDRecorder]

GO