-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'APPLICATIONACCESSSMARTCLIENTBROWSEMODE')
UPDATE SecurityAttributes
SET [Name] = 'Play back',
	[Description] = 'Play back'
WHERE SecurityAttributeID = 'APPLICATIONACCESSSMARTCLIENTBROWSEMODE'

IF EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICERECORDEDBROWSE')
UPDATE SecurityAttributes
SET [Name] = 'Play back video',
	[Description] = 'Play back video'
WHERE SecurityAttributeID = 'DEVICERECORDEDBROWSE'

IF EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICESPEAKERRECORDEDBROWSE')
UPDATE SecurityAttributes
SET [Name] = 'Play back audio',
	[Description] = 'Play back audio'
WHERE SecurityAttributeID = 'DEVICESPEAKERRECORDEDBROWSE'

IF EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEMICRECORDEDBROWSE')
UPDATE SecurityAttributes
SET [Name] = 'Play back audio',
	[Description] = 'Play back audio'
WHERE SecurityAttributeID = 'DEVICEMICRECORDEDBROWSE'

GO

