-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
DECLARE @CurrentTime DateTime
SET @CurrentTime = GETUTCDATE()

--Playback restricted media nodes
IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICERECORDEDBROWSERESTRICTED')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICERECORDEDBROWSERESTRICTED', 'Playback restricted recordings', 'Playback restricted recordings', 'DEVICERECORDED', 'Camera', 0, 51, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEMICRECORDEDBROWSERESTRICTED')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEMICRECORDEDBROWSERESTRICTED', 'Playback restricted recordings', 'Playback restricted recordings', 'DEVICEMICRECORDED', 'AudioInput', 0, 51, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICESPEAKERRECORDEDBROWSERESTRICTED')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICESPEAKERRECORDEDBROWSERESTRICTED', 'Playback restricted recordings', 'Playback restricted recordings', 'DEVICESPEAKERRECORDED', 'AudioOutput', 0, 51, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEMETADATARECORDEDBROWSERESTRICTED')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEMETADATARECORDEDBROWSERESTRICTED', 'Playback restricted recordings', 'Playback restricted recordings', 'DEVICEMETADATARECORDED', 'Metadata', 0, 51, '')

-- Camera
IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICERESTRICTIONS')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICERESTRICTIONS', 'Playback restrictions', 'Playback restrictions', 'DEVICE', 'Camera', 0, 110, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICERESTRICTIONCREATE')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICERESTRICTIONCREATE', 'Create and extend live and playback restrictions', 'Create and extend live and playback restrictions', 'DEVICERESTRICTIONS', 'Camera', 0, 110, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICERESTRICTIONREMOVE')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICERESTRICTIONREMOVE', 'Delete and reduce live and playback restrictions', 'Delete and reduce live and playback restrictions', 'DEVICERESTRICTIONS', 'Camera', 0, 111, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEREADRESTRICTIONS')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEREADRESTRICTIONS', 'Read live and playback restrictions', 'Read live and playback restrictions', 'DEVICERESTRICTIONS', 'Camera', 0, 112, '')

-- Mic

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEMICRESTRICTIONS')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEMICRESTRICTIONS', 'Playback restrictions', 'Playback restrictions', 'DEVICEMIC', 'AudioInput', 0, 110, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEMICRESTRICTIONCREATE')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEMICRESTRICTIONCREATE', 'Create and extend live and playback restrictions', 'Create and extend live and playback restrictions', 'DEVICEMICRESTRICTIONS', 'AudioInput', 0, 110, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEMICRESTRICTIONREMOVE')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEMICRESTRICTIONREMOVE', 'Delete and reduce live and playback restrictions', 'Delete and reduce live and playback restrictions', 'DEVICEMICRESTRICTIONS', 'AudioInput', 0, 111, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEMICREADRESTRICTIONS')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEMICREADRESTRICTIONS', 'Read live and playback restrictions', 'Read live and playback restrictions', 'DEVICEMICRESTRICTIONS', 'AudioInput', 0, 112, '')

-- Speaker

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICESPEAKERRESTRICTIONS')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICESPEAKERRESTRICTIONS', 'Playback restrictions', 'Playback restrictions', 'DEVICESPEAKER', 'AudioOutput', 0, 110, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICESPEAKERRESTRICTIONCREATE')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICESPEAKERRESTRICTIONCREATE', 'Create and extend live and playback restrictions', 'Create and extend live and playback restrictions', 'DEVICESPEAKERRESTRICTIONS', 'AudioOutput', 0, 110, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICESPEAKERRESTRICTIONREMOVE')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICESPEAKERRESTRICTIONREMOVE', 'Delete and reduce live and playback restrictions', 'Delete and reduce live and playback restrictions', 'DEVICESPEAKERRESTRICTIONS', 'AudioOutput', 0, 111, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICESPEAKERREADRESTRICTIONS')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICESPEAKERREADRESTRICTIONS', 'Read live and playback restrictions', 'Read live and playback restrictions', 'DEVICESPEAKERRESTRICTIONS', 'AudioOutput', 0, 112, '')

-- Metadata

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEMETADATARESTRICTIONS')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEMETADATARESTRICTIONS', 'Playback restrictions', 'Playback restrictions', 'DEVICEMETADATA', 'Metadata', 0, 110, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEMETADATARESTRICTIONCREATE')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEMETADATARESTRICTIONCREATE', 'Create and extend live and playback restrictions', 'Create and extend live and playback restrictions', 'DEVICEMETADATARESTRICTIONS', 'Metadata', 0, 110, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEMETADATARESTRICTIONREMOVE')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEMETADATARESTRICTIONREMOVE', 'Delete and reduce live and playback restrictions', 'Delete and reduce live and playback restrictions', 'DEVICEMETADATARESTRICTIONS', 'Metadata', 0, 111, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEMETADATAREADRESTRICTIONS')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEMETADATAREADRESTRICTIONS', 'Read live and playback restrictions', 'Read live and playback restrictions', 'DEVICEMETADATARESTRICTIONS', 'Metadata', 0, 112, '')

GO