-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_role_claim_remove]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_role_claim_remove]
GO

CREATE PROCEDURE [security_role_claim_remove]
	@IDRole				[GuidType],
	@LoginProviderId	UNIQUEIDENTIFIER,
	@ClaimName			NVARCHAR(128),
	@ClaimValue			NVARCHAR(512),
	@session_id			[GuidType]
AS
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @roleName [NameType]

		DECLARE @rowCount INT

		DECLARE @LoginProviderIdChar CHAR(36) = @LoginProviderId

		BEGIN TRAN
		
		-- Lock tables for update
		EXEC [security_table_lock] @LockLevel = 0

		SELECT  @roleName = sr.[Name]
		FROM    [SecurityRoles] sr WITH(ROWLOCK)
		WHERE   [IDRole] = @IDRole

		IF @roleName IS NULL
		BEGIN
			-- Role does not exist
			SET @ErrorArgs = @procedureName + N': %%IDRole=' + @IDRole + N'%%'
			;THROW 60002, @ErrorArgs, 1;
		END

		DELETE  rc
		FROM    [SecurityRoleClaim] rc
				INNER JOIN [SecurityClaim] sc ON sc.[IDClaim] = rc.[IDClaim]
		WHERE   sc.[LoginProviderId] = @LoginProviderId
				AND sc.[ClaimName] = @ClaimName COLLATE Latin1_General_100_BIN -- Case sensitive compare
				AND (
						-- Do an 'Ordinal' string compare
						(sc.[CaseSensitive] = 1 AND rc.[ClaimValue] COLLATE Latin1_General_100_BIN = @ClaimValue COLLATE Latin1_General_100_BIN)
						OR
						-- Do an 'OrdinalIgnoreCase' string compare
						(sc.[CaseSensitive] = 0 AND UPPER(rc.[ClaimValue] COLLATE Latin1_General_100_BIN) = UPPER(@ClaimValue COLLATE Latin1_General_100_BIN))
					)
				AND rc.[IDRole] = @IDRole

		SELECT @rowCount = @@ROWCOUNT

		IF (@rowCount = 0)
		BEGIN
			SET @ErrorArgs = @procedureName + N': %%LoginProviderId=' + @LoginProviderIdChar + N'%%; %%ClaimName=' + @ClaimName + N'%%'
			;THROW 60340, @ErrorArgs, 1;
		END
  
		COMMIT TRAN

		-- Register audit log: Claim '{2}' with value '{3}' from provider '{1}' was removed from the role '{0}'
		EXEC create_pending_log @session_id, 9099, @IDRole, @roleName, @LoginProviderIdChar, @ClaimName, @ClaimValue
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