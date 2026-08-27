-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @CurrentTime DateTime
SET @CurrentTime = GETUTCDATE()

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEMARKEDDATA')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEMARKEDDATA', 'Evidence Lock', 'Evidence Lock', 'DEVICE', 'Camera', 0, 99, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEMARKEDDATAVIEW')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEMARKEDDATAVIEW', 'View', 'View', 'DEVICEMARKEDDATA', 'Camera', 0, 99, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEMARKEDDATACREATE')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEMARKEDDATACREATE', 'Create / Expand time span', 'Create / Expand time span', 'DEVICEMARKEDDATA', 'Camera', 0, 99, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEMARKEDDATAREMOVE')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEMARKEDDATAREMOVE', 'Remove / Reduce time span', 'Remove / Reduce time span', 'DEVICEMARKEDDATA', 'Camera', 0, 99, '')


IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEMICMARKEDDATA')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEMICMARKEDDATA', 'Evidence Lock', 'Evidence Lock', 'DEVICEMIC', 'AudioInput', 0, 99, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEMICMARKEDDATAVIEW')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEMICMARKEDDATAVIEW', 'View', 'View', 'DEVICEMICMARKEDDATA', 'AudioInput', 0, 99, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEMICMARKEDDATACREATE')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEMICMARKEDDATACREATE', 'Create / Expand time span', 'Create / Expand time span', 'DEVICEMICMARKEDDATA', 'AudioInput', 0, 99, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEMICMARKEDDATAREMOVE')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEMICMARKEDDATAREMOVE', 'Remove / Reduce time span', 'Remove / Reduce time span', 'DEVICEMICMARKEDDATA', 'AudioInput', 0, 99, '')


IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICESPEAKERMARKEDDATA')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICESPEAKERMARKEDDATA', 'Evidence Lock', 'Evidence Lock', 'DEVICESPEAKER', 'AudioOutput', 0, 99, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICESPEAKERMARKEDDATAVIEW')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICESPEAKERMARKEDDATAVIEW', 'View', 'View', 'DEVICESPEAKERMARKEDDATA', 'AudioOutput', 0, 99, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICESPEAKERMARKEDDATACREATE')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICESPEAKERMARKEDDATACREATE', 'Create / Expand time span', 'Create / Expand time span', 'DEVICESPEAKERMARKEDDATA', 'AudioOutput', 0, 99, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICESPEAKERMARKEDDATAREMOVE')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICESPEAKERMARKEDDATAREMOVE', 'Remove / Reduce time span', 'Remove / Reduce time span', 'DEVICESPEAKERMARKEDDATA', 'AudioOutput', 0, 99, '')

GO
