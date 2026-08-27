-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[SecurityAction]') AND name = N'security_bit')
BEGIN
	ALTER TABLE [SecurityAction]
	ADD [security_bit] INT NULL
END
GO


UPDATE [SecurityAction]
SET security_bit = secBit.securityBit
FROM
(
	SELECT 
		namespace_id
		,name
		,CASE
			WHEN RANK() OVER (PARTITION by namespace_id ORDER BY order_no) = 1 THEN 1
			ELSE POWER(2, RANK() OVER (PARTITION by namespace_id ORDER BY order_no) - 1)
		 END as securityBit
	FROM SecurityAction 
) as secbit
WHERE [SecurityAction].namespace_id = secbit.namespace_id
AND [SecurityAction].name = secbit.name

ALTER TABLE [SecurityAction]
ALTER COLUMN [security_bit] INT NOT NULL


IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[SecurityAction]') AND name = N'legacy_security_name')
BEGIN
	ALTER TABLE [SecurityAction]
	ADD [legacy_security_name] NVARCHAR(250) NULL
END
GO

DECLARE @namespace_id UNIQUEIDENTIFIER

-- CAMERA
SET @namespace_id = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEVISIBLE'
WHERE namespace_id = @namespace_id
AND name = 'GENERIC_READ'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICELIVEVIEW'
WHERE namespace_id = @namespace_id
AND name = 'VIEW_LIVE'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICERECORDEDBROWSE'
WHERE namespace_id = @namespace_id
AND name = 'PLAYBACK'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICERECORDEDEXPORT'
WHERE namespace_id = @namespace_id
AND name = 'EXPORT'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICERECORDEDSMARTSEARCH'
WHERE namespace_id = @namespace_id
AND name = 'SMARTSEARCH'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICERECORDEDGETSEQUENCES'
WHERE namespace_id = @namespace_id
AND name = 'READ_SEQUENCES'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEBOOKMARKVIEW'
WHERE namespace_id = @namespace_id
AND name = 'READ_BOOKMARKS'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEBOOKMARKADD'
WHERE namespace_id = @namespace_id
AND name = 'CREATE_BOOKMARKS'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEBOOKMARKEDIT'
WHERE namespace_id = @namespace_id
AND name = 'EDIT_BOOKMARKS'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEBOOKMARKDELETE'
WHERE namespace_id = @namespace_id
AND name = 'DELETE_BOOKMARKS'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEMARKEDDATAVIEW'
WHERE namespace_id = @namespace_id
AND name = 'READ_EVIDENCE_LOCK'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEMARKEDDATACREATE'
WHERE namespace_id = @namespace_id
AND name = 'CREATE_EVIDENCE_LOCK'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEMARKEDDATAREMOVE'
WHERE namespace_id = @namespace_id
AND name = 'DELETE_EVIDENCE_LOCK'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEPTZAUX'
WHERE namespace_id = @namespace_id
AND name = 'AUX_COMMANDS'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICERETRIEVEEDGERECORDINGS'
WHERE namespace_id = @namespace_id
AND name = 'READ_REMOTE_RECORDINGS'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEMANUALRECORDINGSTART'
WHERE namespace_id = @namespace_id
AND name = 'START_MANUAL_RECORDING'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEMANUALRECORDINGSTOP'
WHERE namespace_id = @namespace_id
AND name = 'STOP_MANUAL_RECORDING'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEPTZCONTROL'
WHERE namespace_id = @namespace_id
AND name = 'PTZ_CONTROL'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEPTZPRESETS'
WHERE namespace_id = @namespace_id
AND name = 'ACTIVATE_PTZ_PRESET'

-- MICROPHONE
SET @namespace_id = '15F48F88-CA89-4926-9A84-2B02864EC77A'
	
UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEMICVISIBLE'
WHERE namespace_id = @namespace_id
AND name = 'GENERIC_READ'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEMICLIVEVIEW'
WHERE namespace_id = @namespace_id
AND name = 'LISTEN'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEMICRECORDEDBROWSE'
WHERE namespace_id = @namespace_id
AND name = 'PLAYBACK'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEMICRECORDEDEXPORT'
WHERE namespace_id = @namespace_id
AND name = 'EXPORT'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEMICRECORDEDGETSEQUENCES'
WHERE namespace_id = @namespace_id
AND name = 'READ_SEQUENCES'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEMICBOOKMARKVIEW'
WHERE namespace_id = @namespace_id
AND name = 'READ_BOOKMARKS'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEMICBOOKMARKADD'
WHERE namespace_id = @namespace_id
AND name = 'CREATE_BOOKMARKS'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEMICBOOKMARKEDIT'
WHERE namespace_id = @namespace_id
AND name = 'EDIT_BOOKMARKS'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEMICBOOKMARKDELETE'
WHERE namespace_id = @namespace_id
AND name = 'DELETE_BOOKMARKS'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEMICMARKEDDATAVIEW'
WHERE namespace_id = @namespace_id
AND name = 'READ_EVIDENCE_LOCK'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEMICMARKEDDATACREATE'
WHERE namespace_id = @namespace_id
AND name = 'CREATE_EVIDENCE_LOCK'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEMICMARKEDDATAREMOVE'
WHERE namespace_id = @namespace_id
AND name = 'DELETE_EVIDENCE_LOCK'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEMICRETRIEVEEDGERECORDINGS'
WHERE namespace_id = @namespace_id
AND name = 'READ_REMOTE_RECORDINGS'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEMICMANUALRECORDINGSTART'
WHERE namespace_id = @namespace_id
AND name = 'START_MANUAL_RECORDING'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEMICMANUALRECORDINGSTOP'
WHERE namespace_id = @namespace_id
AND name = 'STOP_MANUAL_RECORDING'


-- INPUT
SET @namespace_id = '3ED79EF0-EE2F-40B8-A49A-17E9380C5932'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEINPUTVISIBLE'
WHERE namespace_id = @namespace_id
AND name = 'GENERIC_READ'


-- OUTPUT

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEOUTPUTVISIBLE'
WHERE namespace_id = @namespace_id
AND name = 'GENERIC_READ'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEACTIVEOUTPUT'
WHERE namespace_id = @namespace_id
AND name = 'ACTIVATE'

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

-- MATRIX
SET @namespace_id = '988D493D-1597-4BE6-AFF0-396BFD5D047A'

UPDATE [SecurityAction]
SET [legacy_security_name] = 'MATRIXVISIBLE'
WHERE namespace_id = @namespace_id
AND name = 'GENERIC_READ'
GO
