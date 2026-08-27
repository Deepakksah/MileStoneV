-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_legacy_delete_role_permissions]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_legacy_delete_role_permissions]
GO

CREATE PROCEDURE [security_legacy_delete_role_permissions]
	@IDRole [GuidType]
AS
BEGIN
	SET NOCOUNT ON


	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	BEGIN TRAN

	-- Lock tables for update
	EXEC [security_table_lock] @LockLevel = 0

	BEGIN TRY
		-- Delete application security permissions
		DELETE FROM [SecurityPermissions]
		WHERE [IDRole] = @IDRole

		-- Delete device security permissions
		DELETE FROM [SecurityPermissionsDevice]
		WHERE [IDRole] = @IDRole

		-- Delete Matrix security permissions
		DELETE FROM [SecurityPermissionsMatrix]
		WHERE [IDRole] = @IDRole

		-- Delete TrustedServer security permissions
		DELETE FROM [SecurityPermissionsTrustedServer]
		WHERE [IDRole] = @IDRole

		-- Delete ViewGroup security permissions
		DELETE FROM [SecurityPermissionsViewGroup]
		WHERE [IDRole] = @IDRole
	END TRY
	BEGIN CATCH
		ROLLBACK TRAN
		;THROW 60000, @procedureName, 1;
	END CATCH

	COMMIT TRAN

	RETURN 0
END
GO
