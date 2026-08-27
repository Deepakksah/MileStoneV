-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_remove_role_member_transaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_remove_role_member_transaction]
GO

CREATE PROCEDURE [security_remove_role_member_transaction]
	@IDRole GuidType,
	@MemberSID SidType,
	@CurrentUtcTime DATETIME,
	@RemoveDynamic BIT = NULL
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	DECLARE @rowCount INT
	DECLARE @AdminUsersCount INT
	DECLARE @RoleName NameType	
	DECLARE @IsStaticOrg BIT
	DECLARE @IsDynamicOrg BIT

	-- Make sure that a current transaction is active
	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END

	-- Make sure the group exists and get the role name for any errors
	SELECT @RoleName = roles.[Name]
	FROM [SecurityRoles] roles WITH(UPDLOCK)
	WHERE roles.[IDRole] = @IDRole

	IF(@RoleName IS NULL)
	BEGIN
		IF(@RemoveDynamic IS NOT NULL AND @RemoveDynamic = 1)
		BEGIN
			RETURN 0 -- Ignore if row is not present when removing IsDynamic 
		END

		SET @ErrorArgs = @procedureName + N': %%Name=' + CAST(@IDRole AS NVARCHAR(36)) + N'%%'
		;THROW 60009, @ErrorArgs, 1;
	END

	-- Get the original IsStatic and IsDynamic values
	SELECT @IsStaticOrg = [IsStatic], @IsDynamicOrg = [IsDynamic]
	FROM [SecurityRolesIdentity] 
	WHERE ([IDRole] = @IDRole AND [SID] = @MemberSID)

	-- We cannot remove the last admin user identity from Administrators role (static binding required)
	DECLARE @administratorsRoleId GuidType = '1F30D14B-BA91-4E8A-8838-EDA17931F8FC'	
	IF(@IDRole = @administratorsRoleId AND @IsStaticOrg = 1)
	BEGIN
		SELECT @AdminUsersCount = COUNT(*) FROM [SecurityRolesIdentity] WHERE [IDRole] = @administratorsRoleId AND [IsStatic] = 1
		IF(@AdminUsersCount <= 1)
		BEGIN
			;THROW 66040, @procedureName, 1;
		END	
	END

	IF(@RemoveDynamic IS NOT NULL) -- NULL means delete the full row, not evaluating IsStatic or IsDynamic
	BEGIN
		DECLARE @RemoveStatic BIT 
		SET @RemoveStatic = ~@RemoveDynamic

		-- Does the relation just needs update in IsStatic or IsDynamic
		IF(@IsStaticOrg IS NOT NULL AND @IsDynamicOrg IS NOT NULL)
		BEGIN
			IF(@IsStaticOrg = 1 AND @IsDynamicOrg = 1) -- Only modify if both IsStatic and IsDymanic is set
			BEGIN
				IF(@RemoveStatic = 1)
				BEGIN
					UPDATE [SecurityRolesIdentity]
					SET [LastModified] = @CurrentUtcTime,
						[IsStatic] = 0
					WHERE([IDRole] = @IDRole AND [SID] = @MemberSID)
				END
				ELSE         -- => @RemoveDynamic
				BEGIN
					UPDATE [SecurityRolesIdentity]
					SET [LastModified] = @CurrentUtcTime,
						[IsDynamic] = 0
					WHERE([IDRole] = @IDRole AND [SID] = @MemberSID)
				END

				RETURN 0 -- Row was only updated, thus no changes in security
			END
			ELSE
			BEGIN
				IF(@RemoveStatic = 1) AND (@IsStaticOrg = 0)
				BEGIN
					SET @ErrorArgs = @procedureName + N': %%MemberSid=' + @MemberSID + N'%%'
					;THROW 60011, @ErrorArgs, 1;
				END
			END
		END
		ELSE
		BEGIN
			IF(@RemoveDynamic = 1)
			BEGIN
				RETURN 0 -- Ignore if row is not present when removing IsDynamic 
			END
		END
	END

	-- Delete the member from the SecurityRolesIdentity table
	DELETE [SecurityRolesIdentity]
	WHERE [IDRole] = @IDRole AND [SID] = @MemberSid

	SELECT @rowCount = @@ROWCOUNT

	-- The identity is not a member of the role
	IF(@rowCount = 0)
	BEGIN
		SET @ErrorArgs = @procedureName + N': %%MemberSid=' + @MemberSid + N'%%'
		;THROW 60011, @ErrorArgs, 1;
	END

	-- Now we're ready to update the cache tables

	-- First we remove the sid from the membership cache
	DELETE [SecurityMembership]
	WHERE [Container] = @IDRole AND [MemberSID] = @MemberSID

	-- The clear the temporary table of deleted identities
	DELETE [TempSecurityDeletedIdentities]
	WHERE [SPID] = @@SPID

	-- Add the identity as a possible deleted SecurityIdentity
	INSERT INTO [TempSecurityDeletedIdentities] (
		[SPID],
		[SID],
		[used]
	)
	VALUES (
		@@SPID,
		@MemberSid,
		1
	)

	-- Call the delete procedure
	EXEC security_delete_identities_transaction @CurrentUtcTime

	-- Mark the role identity as changed
	UPDATE [SecurityIdentity]
	SET [LastModified] = @CurrentUtcTime
	WHERE [SID] = @IDRole

	-- Now that the membership is changed, we need to clear the membership cache
	-- and the permission cache. These will be populated again when needed.
	DELETE FROM [SecurityAccessCache2]

	DELETE FROM [SecurityMembershipCache]

	-- Add the task for deleting identity in idp if the role is admin role
	IF(@IDRole = '1f30d14b-ba91-4e8a-8838-eda17931f8fc')
	BEGIN
		EXEC [create_task_delete_idp_admin_transaction] @MemberSID
	END
END
GO