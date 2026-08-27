-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_query_client_login_legacy_permission]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_query_client_login_legacy_permission]
GO

CREATE PROCEDURE [security_query_client_login_legacy_permission]
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

	SELECT @IDTimeProfile = [IDTimeProfile], @IsDefaultTimeProfile = [IsDefaultTimeProfile]
	FROM [SecurityPermissions]
	WHERE [IDRole] = @IDRole
	
	-- If Role is Administrator
	IF(@IsDefaultTimeProfile IS NULL)
	BEGIN
		SELECT 'Always'
		RETURN
	END
	
	-- If Time Profile is not default
	IF(@IDTimeProfile IS NOT NULL)
	BEGIN
		SELECT [name]
		FROM [TimeProfile]
		WHERE [pkid] = @IDTimeProfile
		RETURN
	END

	-- There is no time profile specified and the default is not used - use 'Always'
	IF(@IsDefaultTimeProfile = 0)
	BEGIN
		SELECT 'Always'
		RETURN
	END

	-- Search for the default time profile
	SELECT @IDTimeProfile = [IDTimeProfile]
	FROM [TimeProfileDefaultRoleRelation]
	WHERE [IDRole] = @IDRole

	-- There is no default time profile entry - default is 'Always'
	IF (@IDTimeProfile IS NULL)
	BEGIN
		SELECT 'Always'
		RETURN
	END		

	-- Query the name of the default time profile
	SELECT [name]
	FROM [TimeProfile] 
	WHERE [pkid] = @IDTimeProfile
END
GO