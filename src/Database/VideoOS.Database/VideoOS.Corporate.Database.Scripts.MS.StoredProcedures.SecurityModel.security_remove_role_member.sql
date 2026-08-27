-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_remove_role_member]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_remove_role_member]
GO

CREATE PROCEDURE [security_remove_role_member]
	@IDRole GuidType,
	@MemberSID SidType,
	@RemoveDynamic BIT,
	@session_id [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @domain NVARCHAR(256)
		DECLARE @accountName NVARCHAR(256)
		DECLARE @roleName [NameType]
		DECLARE @type TINYINT

		BEGIN TRAN

		SELECT @roleName = sr.[Name]
		FROM [SecurityRoles] sr
		WHERE [IDRole] = @IDRole
	
		SELECT @type = [Type], @domain = [Domain], @accountName = [AccountName]
		FROM [SecurityIdentity]
		WHERE [SID] = @MemberSID
	
		-- Lock tables for update
		EXEC [security_table_lock] @LockLevel = 0

		DECLARE @LastModified DATETIME
		SET @LastModified = GETUTCDATE()

		-- Do the real work
		EXEC [security_remove_role_member_transaction]
			@IDRole,
			@MemberSID,
			@LastModified,
			@RemoveDynamic
	
		COMMIT TRAN

		IF(@RemoveDynamic IS NULL OR @RemoveDynamic = 0)
		BEGIN
			DECLARE @userName NVARCHAR(512)
			SELECT @userName = @domain + '\' + @accountName

			IF(@type = 5)
			BEGIN
				SELECT @userName = @AccountName
			END
		
			IF(@type = 2 OR @type = 5)
			BEGIN
				EXEC create_pending_log @session_id, 9006, @IDRole, @roleName, @userName
			END
			ELSE IF(@type = 3)
			BEGIN
				EXEC create_pending_log @session_id, 9008, @IDRole, @roleName, @userName
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