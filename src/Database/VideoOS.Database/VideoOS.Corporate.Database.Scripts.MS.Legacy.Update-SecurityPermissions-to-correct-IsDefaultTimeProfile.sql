-- This header is used by the dbupdate7 tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

UPDATE [SecurityPermissions]
SET [IsDefaultTimeProfile] = 0
WHERE [SecurityAttributeID] IN
	(SELECT [SecurityAttributeID] FROM [SecurityAttributes] WHERE [TimeProfileBased] = 0)
GO

UPDATE [SecurityPermissionsDevice]
SET [IsDefaultTimeProfile] = 0
WHERE [SecurityAttributeID] IN
	(SELECT [SecurityAttributeID] FROM [SecurityAttributes] WHERE [TimeProfileBased] = 0)
GO
