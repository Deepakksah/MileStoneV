-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_legacy_HasMatrixViewPermission]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_legacy_HasMatrixViewPermission]
GO

CREATE PROCEDURE [security_legacy_HasMatrixViewPermission]
	@IDRole [GuidType],
	@IDMatrixView [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @is_permitted BIT

	SET @is_permitted = 0

	IF(@IDRole = '1f30d14b-ba91-4e8a-8838-eda17931f8fc')
	BEGIN
		SET @is_permitted = 1
		SELECT @is_permitted AS is_permitted
		RETURN
	END

  -- Initialize the ProcedureName for error messages.
  DECLARE @procedureName SYSNAME
  SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	SELECT @is_permitted = CAST(spm.[Value] AS BIT)
	FROM [SecurityPermissionsMatrix] spm
	WHERE spm.[IDRole] = @IDRole
				AND spm.[SecurityAttributeID] = 'MATRIXVISIBLE'
				AND spm.[IDMatrix] = @IDMatrixView

	SELECT @is_permitted AS is_permitted

	RETURN
END
GO
