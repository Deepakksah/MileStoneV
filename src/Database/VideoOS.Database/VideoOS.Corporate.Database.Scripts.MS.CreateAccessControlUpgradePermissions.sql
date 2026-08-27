-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @UngroupId NVARCHAR(300) = '9EA76B67-E0FF-4A8F-A904-F4DB1DB6A9CF'
DECLARE @AccessControlUngroupedSecurityObjectId [GuidType] = 'B6B820C2-F1CE-4FCE-881A-1413F6CA1FD3'
DECLARE @AccessControlSecurityNamespace [GuidType] = '6d3bdb33-d913-470f-8784-3dd4ee94db8c'
DECLARE @AdminRole [GuidType] = '1f30d14b-ba91-4e8a-8838-eda17931f8fc'

-- Create all SecurityAccessEntries for all roles - except admin role. Also exclude ADMIN_SECURITY on all roles
INSERT [SecurityAccessEntry] ([security_object_id], [user_sid], [action_name], [access_deny])
SELECT 
	@AccessControlUngroupedSecurityObjectId AS [security_object_id],
	si.[SID] AS [user_sid],
	sa.[name] as [action_name],
	0 AS [access_deny]
FROM [SecurityIdentity] si, [SecurityAction] sa 
WHERE [namespace_id] = @AccessControlSecurityNamespace AND sa.[name] <> 'ADMIN_SECURITY' AND si.[SID] <> @AdminRole AND si.[Type] = 4
GO
