-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @CurrentTime DateTime
SET @CurrentTime = GETUTCDATE()

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'ENTERPRISESERVERWINDOWSACCOUNTPASSWORD')
INSERT INTO SecurityAttributes(LastModified,SecurityAttributeID, [Name], Description, ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, Definition)
VALUES(@CurrentTime,'ENTERPRISESERVERWINDOWSACCOUNTPASSWORD', 'Windows account password', 'Windows account password', 'ENTERPRISESERVERWINDOWS', 'EnterpriseServer', 1, 6, '<attributedefinition><type>passwordstring</type><data><datatype>string</datatype><value><name/><displayname>Password</displayname></value><defaultvalue/><administratorvalue/></data></attributedefinition>')
GO
