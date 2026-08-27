-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @ManagementServerNamespaceId GuidType
SET @ManagementServerNamespaceId = 'E9914E39-A67A-496C-A3C1-5B4AAAB28F15'

INSERT INTO [SecurityAccessEntry]([security_object_id], [user_sid], [action_name], [access_deny], [deleted])
SELECT so.[security_object_id], sr.[IDRole], 'CONNECT', 0, 0
FROM [SecurityObject] so, [SecurityRoles] sr
WHERE so.[namespace_id] = @ManagementServerNamespaceId 
AND so.[object_id] = '/'
AND sr.[Builtin] = 0
AND NOT EXISTS
(
	SELECT *
	FROM [SecurityAccessEntry] sae
	WHERE sae.[action_name] = 'CONNECT'
	AND sae.[security_object_id] = so.[security_object_id]
	AND sae.[user_sid] =  sr.[IDRole]
)
GO