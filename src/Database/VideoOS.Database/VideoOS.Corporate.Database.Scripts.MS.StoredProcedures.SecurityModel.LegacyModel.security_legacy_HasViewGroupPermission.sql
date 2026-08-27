-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_legacy_HasViewGroupPermission]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_legacy_HasViewGroupPermission]
GO

CREATE PROCEDURE [security_legacy_HasViewGroupPermission]
	@IDRole [GuidType],
	@IDSecurityAttribute [NameType],
	@IDViewGroup [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @is_permitted BIT

  -- Initialize the ProcedureName for error messages.
  DECLARE @procedureName SYSNAME
  SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	IF(@IDRole = '1f30d14b-ba91-4e8a-8838-eda17931f8fc')
	BEGIN
		IF(@IDSecurityAttribute = 'VIEWGROUPREADONLY')
		BEGIN
			SET @is_permitted = 0
		END
		ELSE
		BEGIN
			SET @is_permitted = 1
		END
		
		SELECT @is_permitted AS is_permitted

		RETURN
	END

	SELECT @is_permitted = CAST(spv.[Value] AS BIT)
	FROM [SecurityPermissionsViewGroup] spv
	WHERE spv.[IDRole] = @IDRole
				AND spv.[SecurityAttributeID] = @IDSecurityAttribute
				AND spv.[IDViewGroup] = @IDViewGroup
				
	IF(@is_permitted IS NULL)
	BEGIN
	  IF(@IDSecurityAttribute = 'VIEWGROUPREADONLY')
	  BEGIN
	    SET @is_permitted = 0
	  END
	  ELSE IF(@IDSecurityAttribute = 'VIEWGROUPVISIBLE')
	  BEGIN
	    SET @is_permitted = 0
	  END
	END

	SELECT @is_permitted AS is_permitted

	RETURN
END
GO
