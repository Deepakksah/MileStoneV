-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @CurrentTime DateTime
SET @CurrentTime = GETUTCDATE()

IF NOT EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'DEVICEEDGE')
BEGIN
	INSERT INTO SecurityAttributes(LastModified,SecurityAttributeID,[Name],Description,ParentSecurityAttributeName,SecurityType,HasObject,DisplayOrder,Definition)
	VALUES(@CurrentTime,'DEVICEEDGE','Camera','Camera','','EdgeCamera',0,1,'')
END

IF NOT EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'DEVICERETRIEVEEDGERECORDINGS')
BEGIN
	INSERT INTO SecurityAttributes(LastModified,SecurityAttributeID,[Name],Description,ParentSecurityAttributeName,SecurityType,HasObject,DisplayOrder,Definition,TimeProfileBased,TimeBackwardBased) 
	VALUES(@CurrentTime,'DEVICERETRIEVEEDGERECORDINGS','Retrieve edge recordings','Retrieve edge recordings','DEVICEEDGE','EdgeCamera',0,10,'',0,0)
END

IF NOT EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'DEVICEMICEDGE')
BEGIN
	INSERT INTO SecurityAttributes(LastModified,SecurityAttributeID,[Name],Description,ParentSecurityAttributeName,SecurityType,HasObject,DisplayOrder,Definition)
	VALUES(@CurrentTime,'DEVICEMICEDGE','Microphone','Microphone','','EdgeAudioInput',0,1,'')
END

IF NOT EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'DEVICEMICRETRIEVEEDGERECORDINGS')
BEGIN
	INSERT INTO SecurityAttributes(LastModified,SecurityAttributeID,[Name],Description,ParentSecurityAttributeName,SecurityType,HasObject,DisplayOrder,Definition,TimeProfileBased,TimeBackwardBased) 
	VALUES(@CurrentTime,'DEVICEMICRETRIEVEEDGERECORDINGS','Retrieve edge recordings','Retrieve edge recordings','DEVICEMICEDGE','EdgeAudioInput',0,10,'',0,0)
END

IF NOT EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'DEVICESPEAKEREDGE')
BEGIN
	INSERT INTO SecurityAttributes(LastModified,SecurityAttributeID,[Name],Description,ParentSecurityAttributeName,SecurityType,HasObject,DisplayOrder,Definition)
	VALUES(@CurrentTime,'DEVICESPEAKEREDGE','Speaker','Speaker','','EdgeAudioOutput',0,1,'')
END

IF NOT EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'DEVICESPEAKERRETRIEVEEDGERECORDINGS')
BEGIN
	INSERT INTO SecurityAttributes(LastModified,SecurityAttributeID,[Name],Description,ParentSecurityAttributeName,SecurityType,HasObject,DisplayOrder,Definition,TimeProfileBased,TimeBackwardBased) 
	VALUES(@CurrentTime,'DEVICESPEAKERRETRIEVEEDGERECORDINGS','Retrieve edge recordings','Retrieve edge recordings','DEVICESPEAKEREDGE','EdgeAudioOutput',0,10,'',0,0)
END

GO
