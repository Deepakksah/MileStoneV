-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @CurrentTime DateTime
SET @CurrentTime = GETUTCDATE()

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEMETADATAMARKEDDATA')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEMETADATAMARKEDDATA', 'Evidence Lock', 'Evidence Lock', 'DEVICEMETADATA', 'Metadata', 0, 99, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEMETADATAMARKEDDATAVIEW')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEMETADATAMARKEDDATAVIEW', 'View', 'View', 'DEVICEMETADATAMARKEDDATA', 'Metadata', 0, 99, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEMETADATAMARKEDDATACREATE')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEMETADATAMARKEDDATACREATE', 'Create / Expand time span', 'Create / Expand time span', 'DEVICEMETADATAMARKEDDATA', 'Metadata', 0, 99, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEMETADATAMARKEDDATAREMOVE')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEMETADATAMARKEDDATAREMOVE', 'Remove / Reduce time span', 'Remove / Reduce time span', 'DEVICEMETADATAMARKEDDATA', 'Metadata', 0, 99, '')

IF EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICESPEAKERMARKEDDATA')
	BEGIN 
		UPDATE SecurityAttributes
		SET [ParentSecurityAttributeName] = 'DEVICESPEAKER'
		WHERE SecurityAttributeID = 'DEVICESPEAKERMARKEDDATA'
	END
ELSE 
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
