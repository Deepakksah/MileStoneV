-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @recorderId GuidType

DECLARE recorder_storage CURSOR FOR 
SELECT DISTINCT [recordingserver_id]
FROM [RecordingStorage];

OPEN recorder_storage;

FETCH NEXT FROM recorder_storage
INTO @recorderId

DECLARE @storageCount int
WHILE @@FETCH_STATUS = 0
BEGIN
	SELECT @storageCount = COUNT([pkid]) FROM [RecordingStorage] WHERE [recordingserver_id] = @recorderId AND is_default = 1
	
	IF @storageCount = 0
	BEGIN
		PRINT @recorderId;

		UPDATE TOP (1) [RecordingStorage]
		SET is_default = 1
		WHERE [recordingserver_id] = @recorderId
	END
	
	FETCH NEXT FROM recorder_storage
	INTO @recorderId
END
CLOSE recorder_storage;
DEALLOCATE recorder_storage;
GO
