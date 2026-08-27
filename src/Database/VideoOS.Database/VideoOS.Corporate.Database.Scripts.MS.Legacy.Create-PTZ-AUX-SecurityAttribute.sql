-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'DEVICEPTZAUX')
BEGIN
	INSERT INTO SecurityAttributes(LastModified,SecurityAttributeID,[Name],Description,ParentSecurityAttributeName,SecurityType,HasObject,DisplayOrder,Definition,TimeProfileBased,TimeBackwardBased) 
	VALUES(GETUTCDATE(),'DEVICEPTZAUX','AUX Commands','AUX Commands','DEVICE','Camera',0,10,'',0,0)
END
GO
