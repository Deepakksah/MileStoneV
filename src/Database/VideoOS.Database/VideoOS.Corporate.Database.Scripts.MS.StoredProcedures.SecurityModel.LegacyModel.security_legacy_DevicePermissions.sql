-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_legacy_DevicePermissions]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_legacy_DevicePermissions]
GO

CREATE PROCEDURE [security_legacy_DevicePermissions]
	@SID [SidType]
AS
BEGIN
	SET NOCOUNT ON
	
	-- Find which Devices the SID(user) has access to
	SELECT DISTINCT CAST(spd.[IDDevice] AS UNIQUEIDENTIFIER) AS [IDDevice], spd.[SecurityAttributeId]
	FROM [SecurityPermissionsDevice] spd
	INNER JOIN [RoleMembership] rm ON rm.[IDRole] = spd.[IDRole]
	WHERE rm.[MemberSID] = @SID
END
GO
