---- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

--Delete Smart Client
DELETE SecurityAttributes
WHERE SecurityAttributeID = 'APPLICATIONACCESSSMARTCLIENT'
GO

--Delete Smart Client browse mode from SecurityAttributes
DELETE SecurityAttributes
WHERE SecurityAttributeID = 'APPLICATIONACCESSSMARTCLIENTBROWSEMODE'
GO

--Delete Smart Client live mode from SecurityAttributes
DELETE SecurityAttributes
WHERE SecurityAttributeID = 'APPLICATIONACCESSSMARTCLIENTLIVEMODE'
GO

--Delete Smart Client setup mode from SecurityAttributes
DELETE SecurityAttributes
WHERE SecurityAttributeID = 'APPLICATIONACCESSSMARTCLIENTSETUPMODE'
GO

--Delete Smart Client browse mode from SecurityPermissions
DELETE SecurityPermissions
WHERE SecurityAttributeID = 'APPLICATIONACCESSSMARTCLIENTBROWSEMODE'
GO

--Delete Smart Client live mode from SecurityPermissions
DELETE SecurityPermissions
WHERE SecurityAttributeID = 'APPLICATIONACCESSSMARTCLIENTLIVEMODE'
GO

--Delete Smart Client setup mode from SecurityPermissions
DELETE SecurityPermissions
WHERE SecurityAttributeID = 'APPLICATIONACCESSSMARTCLIENTSETUPMODE'
GO
