-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @CurrentTime DateTime
SET @CurrentTime = GETUTCDATE()

-- Camera
IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEMANUALRECORDING')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEMANUALRECORDING', 'Manual recording', 'Manual recording', 'DEVICE', 'Camera', 0, 15, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEMANUALRECORDINGSTART')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEMANUALRECORDINGSTART', 'Start', 'Start', 'DEVICEMANUALRECORDING', 'Camera', 0, 60, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEMANUALRECORDINGSTOP')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEMANUALRECORDINGSTOP', 'Stop', 'Stop', 'DEVICEMANUALRECORDING', 'Camera', 0, 62, '')

-- Microphone
IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEMICMANUALRECORDING')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEMICMANUALRECORDING', 'Manual recording', 'Manual recording', 'DEVICEMIC', 'AudioInput', 0, 15, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEMICMANUALRECORDINGSTART')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEMICMANUALRECORDINGSTART', 'Start', 'Start', 'DEVICEMICMANUALRECORDING', 'AudioInput', 0, 60, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEMICMANUALRECORDINGSTOP')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEMICMANUALRECORDINGSTOP', 'Stop', 'Stop', 'DEVICEMICMANUALRECORDING', 'AudioInput', 0, 62, '')

-- Speaker
IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICESPEAKERMANUALRECORDING')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICESPEAKERMANUALRECORDING', 'Manual recording', 'Manual recording', 'DEVICESPEAKER', 'AudioOutput', 0, 15, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICESPEAKERMANUALRECORDINGSTART')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICESPEAKERMANUALRECORDINGSTART', 'Start', 'Start', 'DEVICESPEAKERMANUALRECORDING', 'AudioOutput', 0, 60, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICESPEAKERMANUALRECORDINGSTOP')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICESPEAKERMANUALRECORDINGSTOP', 'Stop', 'Stop', 'DEVICESPEAKERMANUALRECORDING', 'AudioOutput', 0, 62, '')

-- Metadata
IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEMETADATAMANUALRECORDING')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEMETADATAMANUALRECORDING', 'Manual recording', 'Manual recording', 'DEVICEMETADATA', 'Metadata', 0, 15, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEMETADATAMANUALRECORDINGSTART')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEMETADATAMANUALRECORDINGSTART', 'Start', 'Start', 'DEVICEMETADATAMANUALRECORDING', 'Metadata', 0, 60, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEMETADATAMANUALRECORDINGSTOP')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEMETADATAMANUALRECORDINGSTOP', 'Stop', 'Stop', 'DEVICEMETADATAMANUALRECORDING', 'Metadata', 0, 62, '')

GO
