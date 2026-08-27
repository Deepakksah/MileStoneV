-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

UPDATE [SecurityAction]
SET [legacy_security_name] = 'DEVICEPTZMANAGEPATROLLING'
WHERE [namespace_id] = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85'
AND [name] = 'MANAGE_PATROLLING_PROFILE'
GO

IF NOT EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'DEVICEPTZMANAGEPATROLLING')
BEGIN
	UPDATE [SecurityAttributes]
	SET [DisplayOrder] = [DisplayOrder] + 1
	WHERE [ParentSecurityAttributeName] = 'DEVICEPTZ'
		AND [DisplayOrder] > 16

	INSERT INTO [SecurityAttributes] ([LastModified], [SecurityAttributeID], [Name], [Description], [ParentSecurityAttributeName], [SecurityType], [HasObject], [DisplayOrder], [Definition])
	VALUES(GETUTCDATE(), 'DEVICEPTZMANAGEPATROLLING', 'Manage Patrolling Profiles', 'Manage PTZ patrolling profiles with create/update/delete', 'DEVICEPTZ', 'PTZ', 0, 16, '')
END
GO
