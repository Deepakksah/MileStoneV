-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_legacy_GetApplicationTimeProfile]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_legacy_GetApplicationTimeProfile]
GO

CREATE PROCEDURE [security_legacy_GetApplicationTimeProfile]
	@IDRole [GuidType],
	@IDSecurityAttribute [NameType]
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @IDTimeProfile GuidType
	
	 -- Initialize the ProcedureName for error messages.
    DECLARE @procedureName SYSNAME
    SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	IF(@IDRole = '1f30d14b-ba91-4e8a-8838-eda17931f8fc')
	BEGIN
		SET @IDTimeProfile = NULL
	END
	ELSE
	BEGIN
		DECLARE @IsDefaultTimeProfile BIT
		SET @IsDefaultTimeProfile = (SELECT [IsDefaultTimeProfile] FROM [SecurityPermissions]  WHERE [IDRole] = @IDRole AND [SecurityAttributeID] = @IDSecurityAttribute)
	
		IF(@IsDefaultTimeProfile = 1)
		BEGIN
			SET @IDTimeProfile = (SELECT [IDTimeProfile] FROM [TimeProfileDefaultRoleRelation] WHERE [IDRole] = @IDRole)
		END
		ELSE
		BEGIN
			SELECT @IDTimeProfile = [IDTimeProfile]
			FROM [SecurityPermissions]
			WHERE [IDRole] = @IDRole AND
				  [SecurityAttributeID] = @IDSecurityAttribute
		END
	END
	
	SELECT @IDTimeProfile AS IDTimeProfile

END
GO
