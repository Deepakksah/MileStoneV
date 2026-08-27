-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_install]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_install]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[install_security]') AND type in (N'P', N'PC'))
DROP PROCEDURE [install_security]
GO

CREATE PROCEDURE [install_security]
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRANSACTION

		DECLARE @LastModified DATETIME
		SET @LastModified = GETUTCDATE()

		DECLARE @IDRole [GuidType]
		DECLARE @RoleType TINYINT
		DECLARE @Name [NameType]
		DECLARE @Description NVARCHAR(500)

		-- Due to an issue in v1.5 where SecurityPermissions are not correctly removed 
		-- when deleting roles we need to remove any orphaned SecurityPermissions before
		-- updating security database tables
		DELETE [SecurityPermissions]
		WHERE IDRole NOT IN ( SELECT IDRole FROM [SecurityRoles] )

		-- Add roles to SecurityIdentity
		INSERT [SecurityIdentity] (
			[SID],
			[Type],
			[DisplayName],
			[Description],
			[Domain],
			[AccountName],
			[DistinguishedName],
			[Deleted],
			[LastModified]
		)
		SELECT 
			sr.[IDRole],
			4,
			sr.[Name],
			sr.[Description],
			'', -- Roles don't have domain
			'', -- Roles don't have account name
			'', -- Roles don't have distinguished name
			0,
			sr.[LastModified]
		FROM [SecurityRoles] sr
		LEFT JOIN [SecurityIdentity] si ON
			si.[SID] = sr.[IDRole]
		WHERE si.[SID] IS NULL

		-- Add roles/identities to SecurityMembership
		INSERT [SecurityMembership] (
			[Container],
			[MemberSID]
		)
		SELECT
			sri.[IDRole],
			sri.[SID]
		FROM [SecurityRolesIdentity] sri
		LEFT JOIN [SecurityMembership] sm ON sm.[Container] = sri.[IDRole] AND sm.[MemberSID] = sri.[SID]
		WHERE sm.[Container] IS NULL AND sm.[MemberSID] IS NULL

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH 
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION
		END;
		DECLARE @shouldReThrowError BIT = dbo.[ShouldReThrowError](ERROR_NUMBER(), ERROR_SEVERITY())
		IF(@shouldReThrowError = 1)
			THROW;
		SET @ErrorArgs = dbo.[FormatErrorMessage](ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE())
		;THROW 60000, @ErrorArgs, 1;
	END CATCH;
END
GO

EXEC [install_security]
GO