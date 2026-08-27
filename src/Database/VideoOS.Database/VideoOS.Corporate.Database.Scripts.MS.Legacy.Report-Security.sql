-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @CurrentTime DateTime
SET @CurrentTime = GETUTCDATE()

IF NOT EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'APPLICATIONACCESSREPORTS')
INSERT INTO [SecurityAttributes]([LastModified], [SecurityAttributeID], [Name], [Description], [ParentSecurityAttributeName], [SecurityType], [HasObject], [DisplayOrder], [Definition], [TimeProfileBased], [TimeBackwardBased]) 
VALUES(@CurrentTime, 'APPLICATIONACCESSREPORTS', 'Reports', 'Reports Access', 'APPLICATIONACCESS', 'Application', 0, 16, '', 0, 0)

GO
