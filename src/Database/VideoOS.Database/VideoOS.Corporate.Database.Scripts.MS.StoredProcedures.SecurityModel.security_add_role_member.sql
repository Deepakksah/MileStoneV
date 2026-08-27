-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_add_role_member]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_add_role_member]
GO

CREATE PROCEDURE [security_add_role_member]
	@IDRole [GuidType],
	@MemberSID [SidType],
	@Type TINYINT,
	@DisplayName NVARCHAR(256),
	@Description NVARCHAR(1024),
	@Domain NVARCHAR(256),
	@AccountName NVARCHAR(256),
	@DistinguishedName NVARCHAR(1024),
	@CurrentUtcTime DATETIME,
	@IsDynamic BIT = 0,
	@session_id [GuidType]
AS
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRAN

		DECLARE @roleName [NameType]
	
		SELECT @roleName = sr.[Name]
		FROM [SecurityRoles] sr
		WHERE [IDRole] = @IDRole
	
		-- Lock tables for update
		EXEC [security_table_lock] @LockLevel = 0

		-- Do the real work
		EXEC [security_add_role_member_transaction]
			@IDRole,
			@MemberSID,
			@Type,
			@DisplayName,
			@Description,
			@Domain,
			@AccountName,
			@DistinguishedName,
			@CurrentUtcTime,
			@IsDynamic

		COMMIT TRAN

		-- Only Audit log on static mappings
		IF(@IsDynamic = 0)
		BEGIN
			DECLARE @userName NVARCHAR(512)
			SELECT @userName = @Domain + '\' + @AccountName

			IF(@Type = 5)
			BEGIN
				SELECT @userName = @AccountName
			END

			IF(@Type = 2 OR @Type = 5)
			BEGIN
				EXEC create_pending_log @session_id, 9005, @IDRole, @roleName, @userName
			END
			ELSE IF(@Type = 3)
			BEGIN
				EXEC create_pending_log @session_id, 9007, @IDRole, @roleName, @userName
			END
		END
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