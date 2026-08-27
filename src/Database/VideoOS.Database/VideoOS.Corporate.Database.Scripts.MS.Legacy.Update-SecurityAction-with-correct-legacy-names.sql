-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @namespace_id UNIQUEIDENTIFIER

-- SPEAKER
SET @namespace_id = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD'
	
UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICESPEAKERVISIBLE'
WHERE namespace_id = @namespace_id
AND name = 'GENERIC_READ'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICESPEAKERLIVEVIEW'
WHERE namespace_id = @namespace_id
AND name = 'LISTEN'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICESPEAKERRECORDEDBROWSE'
WHERE namespace_id = @namespace_id
AND name = 'PLAYBACK'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICESPEAKERRECORDEDEXPORT'
WHERE namespace_id = @namespace_id
AND name = 'EXPORT'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICESPEAKERRECORDEDGETSEQUENCES'
WHERE namespace_id = @namespace_id
AND name = 'READ_SEQUENCES'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICESPEAKERBOOKMARKVIEW'
WHERE namespace_id = @namespace_id
AND name = 'READ_BOOKMARKS'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICESPEAKERBOOKMARKADD'
WHERE namespace_id = @namespace_id
AND name = 'CREATE_BOOKMARKS'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICESPEAKERBOOKMARKEDIT'
WHERE namespace_id = @namespace_id
AND name = 'EDIT_BOOKMARKS'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICESPEAKERBOOKMARKDELETE'
WHERE namespace_id = @namespace_id
AND name = 'DELETE_BOOKMARKS'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICESPEAKERMARKEDDATAVIEW'
WHERE namespace_id = @namespace_id
AND name = 'READ_EVIDENCE_LOCK'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICESPEAKERMARKEDDATACREATE'
WHERE namespace_id = @namespace_id
AND name = 'CREATE_EVIDENCE_LOCK'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICESPEAKERMARKEDDATAREMOVE'
WHERE namespace_id = @namespace_id
AND name = 'DELETE_EVIDENCE_LOCK'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICESPEAKERRETRIEVEEDGERECORDINGS'
WHERE namespace_id = @namespace_id
AND name = 'READ_REMOTE_RECORDINGS'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICESPEAKERMANUALRECORDINGSTART'
WHERE namespace_id = @namespace_id
AND name = 'START_MANUAL_RECORDING'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICESPEAKERMANUALRECORDINGSTOP'
WHERE namespace_id = @namespace_id
AND name = 'STOP_MANUAL_RECORDING'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICESPEAKCONTROL'
WHERE namespace_id = @namespace_id
AND name = 'SPEAK'


-- METADATA
SET @namespace_id = 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D'
	
UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEMETADATAVISIBLE'
WHERE namespace_id = @namespace_id
AND name = 'GENERIC_READ'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEMETADATALIVEVIEW'
WHERE namespace_id = @namespace_id
AND name = 'LIVE'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEMETADATARECORDEDBROWSE'
WHERE namespace_id = @namespace_id
AND name = 'PLAYBACK'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEMETADATARECORDEDEXPORT'
WHERE namespace_id = @namespace_id
AND name = 'EXPORT'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEMETADATARECORDEDGETSEQUENCES'
WHERE namespace_id = @namespace_id
AND name = 'READ_SEQUENCES'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEMETADATARETRIEVEEDGERECORDINGS'
WHERE namespace_id = @namespace_id
AND name = 'READ_REMOTE_RECORDINGS'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEMETADATAMANUALRECORDINGSTART'
WHERE namespace_id = @namespace_id
AND name = 'START_MANUAL_RECORDING'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEMETADATAMANUALRECORDINGSTOP'
WHERE namespace_id = @namespace_id
AND name = 'STOP_MANUAL_RECORDING'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEMETADATAMARKEDDATAVIEW'
WHERE namespace_id = @namespace_id
AND name = 'READ_EVIDENCE_LOCK'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEMETADATAMARKEDDATACREATE'
WHERE namespace_id = @namespace_id
AND name = 'CREATE_EVIDENCE_LOCK'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEMETADATAMARKEDDATAREMOVE'
WHERE namespace_id = @namespace_id
AND name = 'DELETE_EVIDENCE_LOCK'
GO
