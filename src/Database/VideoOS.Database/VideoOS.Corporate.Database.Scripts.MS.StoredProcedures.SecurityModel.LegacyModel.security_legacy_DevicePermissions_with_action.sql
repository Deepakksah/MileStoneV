-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_legacy_DevicePermissions_with_action]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_legacy_DevicePermissions_with_action]
GO

CREATE PROCEDURE [security_legacy_DevicePermissions_with_action]
	@SID [SidType],
	@IDAction [InternalIdType]
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @IsAdmin BIT
	EXEC security_is_member @SID, '1f30d14b-ba91-4e8a-8838-eda17931f8fc', @IsAdmin OUTPUT

	IF(@IsAdmin = 1)
	BEGIN
		SELECT [IDDevice] AS [IDDevice]
		FROM [Devices]
	END
	ELSE
	BEGIN
		SELECT DISTINCT spd.[IDDevice] AS [IDDevice]
		FROM [SecurityPermissionsDevice] spd
		INNER JOIN [RoleMembership] rm ON spd.[IDRole] = rm.[IDRole]
		WHERE rm.[MemberSID] = @SID
		AND spd.[SecurityAttributeID] = @IDAction
	END
END
GO
