-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Update ClientProfileRoleRelation with default profile relations in order to always have a profile relation
DECLARE @defaultManagementClientProfile [GuidType]

SELECT @defaultManagementClientProfile = IDClientProfile 
FROM [ClientProfile]
WHERE DefaultProfile = 1 AND profile_type = 'ManagementClientProfile'

INSERT INTO [ClientProfileRoleRelation] (IDClientProfile, IDRole)
SELECT @defaultManagementClientProfile, IDRole
FROM [SecurityRoles] as sr
WHERE NOT EXISTS (
		SELECT 1
		FROM [ClientProfileRoleRelation] as cpr
		JOIN [ClientProfile] as cp
		ON cpr.IDClientProfile = cp.IDClientProfile
		WHERE sr.IDRole = cpr.IDRole AND cp.profile_type = 'ManagementClientProfile'  
		) 
GO

DECLARE @defaultSmartClientProfile [GuidType]

SELECT @defaultSmartClientProfile = IDClientProfile 
FROM [ClientProfile]
WHERE DefaultProfile = 1 AND profile_type = 'SmartClientProfile'


INSERT INTO [ClientProfileRoleRelation] (IDClientProfile, IDRole)
SELECT @defaultSmartClientProfile, IDRole
FROM [SecurityRoles] as sr
WHERE NOT EXISTS (
		SELECT 1
		FROM [ClientProfileRoleRelation] as cpr
		JOIN [ClientProfile] as cp
		ON cpr.IDClientProfile = cp.IDClientProfile
		WHERE sr.IDRole = cpr.IDRole AND cp.profile_type = 'SmartClientProfile'  
		) 
GO