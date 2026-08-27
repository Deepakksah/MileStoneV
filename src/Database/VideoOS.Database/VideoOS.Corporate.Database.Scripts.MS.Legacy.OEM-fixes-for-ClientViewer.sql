-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'APPLICATIONACCESSSMARTCLIENT')
UPDATE [SecurityAttributes]
SET [Name] = '{ViewerClientApplicationName}',
		[Description] = '{ViewerClientApplicationName}'
WHERE [SecurityAttributeID] = 'APPLICATIONACCESSSMARTCLIENT'

IF EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'APPLICATIONACCESSSMARTCLIENTLIVEMODE')
UPDATE [SecurityAttributes]
SET [Description] = '{ViewerClientApplicationName}'
WHERE [SecurityAttributeID] = 'APPLICATIONACCESSSMARTCLIENTLIVEMODE'

IF EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'APPLICATIONACCESSSMARTCLIENTBROWSEMODE')
UPDATE [SecurityAttributes]
SET [Description] = '{ViewerClientApplicationName}'
WHERE [SecurityAttributeID] = 'APPLICATIONACCESSSMARTCLIENTBROWSEMODE'

IF EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'APPLICATIONACCESSSMARTCLIENTSETUPMODE')
UPDATE [SecurityAttributes]
SET [Description] = '{ViewerClientApplicationName}'
WHERE [SecurityAttributeID] = 'APPLICATIONACCESSSMARTCLIENTSETUPMODE'
GO
