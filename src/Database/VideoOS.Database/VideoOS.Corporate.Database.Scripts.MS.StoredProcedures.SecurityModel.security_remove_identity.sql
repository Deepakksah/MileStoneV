-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_remove_identity]') AND type = 'P')
DROP PROCEDURE [security_remove_identity]
GO

CREATE PROCEDURE [security_remove_identity]
	@user_sid [SIDType]
AS
BEGIN
	SET NOCOUNT ON;

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	--We only support removing users in this procedure - not roles or groups 
	IF NOT EXISTS (select 1 FROM SecurityIdentity NOLOCK WHERE [sid] = @user_sid AND [Type] IN (2, 5))
	BEGIN 
		select 0
	END

	BEGIN TRY
		BEGIN TRANSACTION

			-- Lock tables for update
	EXEC [security_table_lock] @LockLevel = 0

	DECLARE @CurrentUtcTime DATETIME
	SET @CurrentUtcTime = GETUTCDATE()

	DECLARE @role_id CHAR(36)

	DECLARE identity_cursor CURSOR FAST_FORWARD FOR
	SELECT DISTINCT [IDRole] 
	FROM [SecurityRolesIdentity] 
	WHERE [SID] = @user_sid

	OPEN identity_cursor
	BEGIN TRY
		FETCH NEXT FROM identity_cursor INTO @role_id

		WHILE(@@FETCH_STATUS = 0)
		BEGIN
			EXEC [security_remove_role_member_transaction]
				@role_id,
				@user_sid,
				@CurrentUtcTime

			FETCH NEXT FROM identity_cursor INTO @role_id
		END
	END TRY
	BEGIN CATCH
		CLOSE identity_cursor
		DEALLOCATE identity_cursor
		;THROW
	END CATCH

	CLOSE identity_cursor
	DEALLOCATE identity_cursor

	DECLARE @currentUserDomainName [NameType]

	SELECT @currentUserDomainName = Domain + '\' + AccountName
	FROM SecurityIdentity NOLOCK
	WHERE [sid] = @user_sid 

	--Delete user from database
	DELETE FROM [SecurityAccessCache2] WHERE [USER_SID] = @user_sid
	DELETE FROM [SecurityAccessEntryTimeProfile] WHERE [access_entry_id] IN (SELECT [access_entry_id] FROM [SecurityAccessEntry] WHERE [USER_SID]= @user_sid)
	DELETE FROM [SecurityAccessEntry] WHERE [USER_SID] = @user_sid
	DELETE FROM [SecurityMembershipCache] WHERE [MemberSID] = @user_sid
	DELETE FROM [CustomSettings] WHERE [OwnerSID] = @user_sid
	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Central' AND  TABLE_NAME = 'Users'))
	BEGIN
		DELETE FROM [Central].[Users] WHERE [SID] = @user_sid
	END

	--Delete from ServerGroup
	IF(@currentUserDomainName IS NOT NULL)
	BEGIN
		EXEC [delete_server_group_by_userdomainname_transaction] @currentUserDomainName
	END

	--Delete search templates/save searches created by user
	DELETE FROM [Search].[SearchTemplates] WHERE [Availability] = 'Private' AND [Owner] = @user_sid
	DELETE FROM [Search].[SearchTemplatesFind] WHERE [Owner] = @user_sid

	--Delete from securityIdentity
	DELETE FROM [SecurityIdentity] WHERE [SID] = @user_sid

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