-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_query_client_login_legacy_permission_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_query_client_login_legacy_permission_id]
GO

CREATE PROCEDURE [security_query_client_login_legacy_permission_id]
	@IDRole GuidType
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @IDTimeProfile GuidType
	DECLARE @IsDefaultTimeProfile BIT

	IF NOT EXISTS(
		SELECT @IDRole
		FROM [SecurityRoles]
		WHERE [IDRole] = @IDRole)
	BEGIN
		RETURN
	END

	SELECT [IDTimeProfile] as "time_profile_id", [IsDefaultTimeProfile] as "isDefault"
	FROM [SecurityPermissions]
	WHERE [IDRole] = @IDRole
END
GO