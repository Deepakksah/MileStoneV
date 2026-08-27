-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_add_role_member_transaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_add_role_member_transaction]
GO

CREATE PROCEDURE [security_add_role_member_transaction]
	@IDRole [GuidType],
	@MemberSID [SidType],
	@Type TINYINT,
	@DisplayName NVARCHAR(256),
	@Description NVARCHAR(1024),
	@Domain NVARCHAR(256),
	@AccountName NVARCHAR(256),
	@DistinguishedName NVARCHAR(1024),
	@CurrentUtcTime DATETIME,
	@IsDynamic BIT
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	DECLARE @rowCount INT
	DECLARE @RoleName [NameType]

	DECLARE @IsStatic BIT
	SET @IsStatic = ~@IsDynamic  -- -> @IsStatic = NOT @IsDynamic

	-- Make sure that a current transaction is active
	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END

	-- Make sure that the role exists and get the role display name for any errors
	SELECT @RoleName = [Name]
	FROM [SecurityRoles] WITH(UPDLOCK)
	WHERE [IDRole] = @IDRole

	IF(@RoleName IS NULL)
	BEGIN
		IF(@IsDynamic = 1)
		BEGIN
			Return 0 -- Ignore role deleted when mapping dynamicly
		END

		SET @ErrorArgs = @procedureName + N': %%IDRole=' + CAST(@IDRole AS NVARCHAR(36)) + N'%%'
		;THROW 60006, @ErrorArgs, 1;
	END

	DECLARE @IsStaticOrg BIT
	DECLARE @IsDynamicOrg BIT
	SELECT @IsStaticOrg = [IsStatic], @IsDynamicOrg = [IsDynamic]
	FROM [SecurityRolesIdentity] 
	WHERE ([IDRole] = @IDRole AND [SID] = @MemberSID)

	-- Does the the relation already exist, i.e. just needs update in IsStatic or IsDynamic
	IF(@IsStaticOrg IS NOT NULL AND @IsDynamicOrg IS NOT NULL)
	BEGIN

		DECLARE @IsUserDeleted BIT
		SELECT @IsUserDeleted = [Deleted]
		FROM [SecurityIdentity]
		WHERE [SID] = @MemberSID

		IF(@IsStatic = 1)
		BEGIN
			if(@IsStaticOrg = 1)
			BEGIN
				IF(@IsUserDeleted = 1)
				BEGIN
					UPDATE [SecurityIdentity]
					SET [Deleted] = 0, [LastModified] = @CurrentUtcTime
					WHERE [SID] = @MemberSID
				END

				IF(@IsUserDeleted = 0)
				BEGIN
					-- The identity already exists on the role as 'Static', fail the operation
					SET @ErrorArgs = @procedureName + N': %%RoleName=' + @RoleName + N'%%; %%MemberName=' + @DisplayName + N'%%'
					;THROW 60007, @ErrorArgs, 1;
				END
			END

			UPDATE [SecurityRolesIdentity]
			SET [LastModified] = @CurrentUtcTime,
				[IsStatic] = 1
			WHERE([IDRole] = @IDRole AND [SID] = @MemberSID)
		END
		ELSE
		BEGIN
			-- Do not fail for multiple updates of IsDynamic = 1
			IF(@IsDynamicOrg = 0)
			BEGIN
				UPDATE [SecurityRolesIdentity]
				SET [LastModified] = @CurrentUtcTime,
					[IsDynamic] = 1
				WHERE([IDRole] = @IDRole AND [SID] = @MemberSID)
			END

			IF(@IsUserDeleted = 1)
			BEGIN
				UPDATE [SecurityIdentity]
				SET [Deleted] = 0, [LastModified] = @CurrentUtcTime
				WHERE [SID] = @MemberSID
			END
		END

		RETURN 0 -- Row was here, thus no changes in security
	END

	BEGIN TRY
		-- Add identity to the role
		INSERT [SecurityRolesIdentity] (
			[IDRole],
			[SID],
			[LastModified],
			[IsStatic],
			[IsDynamic]
		)
		VALUES (
			@IDRole,
			@MemberSid,
			@CurrentUtcTime,
			@IsStatic,
			@IsDynamic
		)
	END TRY
	BEGIN CATCH
		IF(ERROR_NUMBER() = 2601 OR ERROR_NUMBER() = 2627) -- CONSTRAINT VIOLATION
		BEGIN
			-- This indicates that the identity already exists on the role
			SET @ErrorArgs = @procedureName + N': %%RoleName=' + @RoleName + N'%%; %%MemberName=' + @DisplayName + N'%%'
			;THROW 60007, @ErrorArgs, 1;
		END
		;THROW
	END CATCH

	-- Add the task for admin creation in idp if the role is admin role
	IF(@IDRole = '1f30d14b-ba91-4e8a-8838-eda17931f8fc')
	BEGIN
		EXEC [create_task_add_idp_admin_transaction] @MemberSID
	END

	-- We need to check if other identities with this domain/account name already exists
	-- with a different sid. This could happen if the user or group has been deleted and 
	-- then created from Active Directory. If this is the case, the identity in the cache 
	-- is deleted from AD and we should mark it as deleted.

	DECLARE @deletedSIDs TABLE(
		[SID] SIDType
	)

	INSERT @deletedSIDs
	SELECT [SID]
	FROM [SecurityIdentity]
	WHERE [Domain] = @Domain
		AND [AccountName] = @AccountName
		AND [SID] <> @MemberSID
		AND [Deleted] = 0
		AND [Type] <> 4 -- don't delete role identities

	SELECT  @rowCount = @@ROWCOUNT

	IF (@rowCount <> 0)
	BEGIN
		-- There are some deleted SIDs we need to remove
		UPDATE [SecurityIdentity]
		SET [LastModified] = @CurrentUtcTime,
			[Deleted] = 1
		FROM [SecurityIdentity] si
		INNER JOIN @deletedSIDs sids ON sids.[sid] = si.[SID]	

		-- Now we need to mark any groups that contains these sid as modified
		UPDATE [SecurityIdentity]
		SET [LastModified] = @CurrentUtcTime
		WHERE [SID] IN (
			SELECT [Container]
			FROM [SecurityMembership] sm
			INNER JOIN @deletedSIDs sids ON sm.[MemberSID] = sids.[SID]
		)

		-- Create the task for deleting the sids from the idp Admin table before removing it from SecurityRoleIdentity
		DECLARE @deletedSid SIDType

		DECLARE sid_cursor CURSOR FAST_FORWARD FOR
		SELECT [SID]
		FROM @deletedSIDs

		OPEN sid_cursor
		BEGIN TRY
			FETCH NEXT FROM sid_cursor INTO @deletedSid

			WHILE(@@FETCH_STATUS = 0)
			BEGIN
				-- Add the task if the role is admin role
				IF EXISTS (SELECT * FROM [SecurityRolesIdentity] WHERE [SID] = @deletedSid AND [IDROLE] = '1f30d14b-ba91-4e8a-8838-eda17931f8fc')
				BEGIN
					EXEC [create_task_delete_idp_admin_transaction] @deletedSid
				END	
				FETCH NEXT FROM sid_cursor INTO @deletedSid
			END
		END TRY
		BEGIN CATCH
			CLOSE sid_cursor
			DEALLOCATE sid_cursor
			;THROW
		END CATCH

		CLOSE sid_cursor
		DEALLOCATE sid_cursor

		-- Now delete the sids from the SecurityRolesIdentity table
		DELETE [SecurityRolesIdentity]
		WHERE [SID] IN (
			SELECT [SID]
			FROM @deletedSIDs
		)

		-- Lastly we delete the sids from the security membership table
		DELETE [SecurityMembership]
		WHERE [MemberSID] IN (
			SELECT [SID]
			FROM @deletedSIDs
		)
	END

	-- Now check if the identity already exists
	IF EXISTS( SELECT [SID] FROM [SecurityIdentity] WITH(UPDLOCK) WHERE [SID] = @MemberSid )
	BEGIN
		-- The identity is already added - update it if anything changed
		UPDATE [SecurityIdentity]
		SET [Type] = @Type,
			[DisplayName] = @DisplayName,
			[Description] = @Description,
			[Domain] = @Domain,
			[AccountName] = @AccountName,
			[DistinguishedName] = @DistinguishedName,
			[Deleted] = 0,
			[LastModified] = @CurrentUtcTime
		WHERE [SID] = @MemberSid
			AND (
				[DisplayName] <> @DisplayName
				OR [Description] <> @Description
				OR [Domain] <> @Domain
				OR [AccountName] <> @AccountName
				OR [DistinguishedName] <> @DistinguishedName
				OR [Deleted] = 1
			)
	
	END
	ELSE
	BEGIN
		-- The identity doesn't exist - create it
		INSERT INTO [SecurityIdentity] (
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
		VALUES (
			@MemberSid,
			@Type,
			@DisplayName,
			@Description,
			@Domain,
			@AccountName,
			@DistinguishedName,
			0,
			@CurrentUtcTime
		)
	END

	-- Now add the identity to the membership table
	INSERT INTO [SecurityMembership] (
		[Container],
		[MemberSID]
	)
	VALUES (
		@IDRole,
		@MemberSID
	)

	-- Mark the role identity as changed
	UPDATE [SecurityIdentity]
	SET [LastModified] = GETUTCDATE()
	WHERE [SID] = @IDRole

	-- Now that the membership is changed, we need to clear the membership cache
	-- and the permission cache. These will be populated again when needed.

	DELETE FROM [SecurityAccessCache2]

	DELETE FROM [SecurityMembershipCache]
END
GO