-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_update_identity]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_update_identity]
GO

CREATE PROCEDURE [security_update_identity]
	@MemberSID SidType,
	@Type TINYINT,
	@DisplayName NVARCHAR(256),
	@Description NVARCHAR(1024),
	@Domain NVARCHAR(256),
	@AccountName NVARCHAR(256),
	@DistinguishedName NVARCHAR(1024),
	@CurrentUtcTime DATETIME,
	@UpdateMembers BIT,
	@failingDomains tvp_StringTableNvarchar READONLY
AS
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @rowCount INT

		DECLARE @membersChanged BIT

		-- TODO: We still need to handle this better
		SET @membersChanged = 0

		BEGIN TRAN

		-- Lock tables for update
		EXEC [security_table_lock] @LockLevel = 0

		-- First clear the list of deleted identities in case we delete any
		DELETE [TempSecurityDeletedIdentities]
		WHERE [SPID] = @@SPID
	
		-- We need to check if other identities with this domain/account name already exists
		-- with a different sid. This could happen if the user or group has been deleted and 
		-- then created from Active Directory. If this is the case, the identity in the cache 
		-- is deleted from AD and we should mark it as deleted.

		-- First insert them in to the list of deleted identities
		IF EXISTS(
			SELECT [SID]
			FROM [SecurityIdentity]
			WHERE [Domain] = @Domain
				AND [AccountName] = @AccountName
				AND [SID] <> @MemberSID
				AND [Deleted] = 0
				AND [Type] <> 0 -- This does not apply for InvalidIdentity
				AND [Type] <> 1 -- This does not apply for UnknownIdentityType
				AND [Type] <> 4 -- This does not apply for Role
				AND [Type] <> 5 -- This does not apply for VMS user
		)
		BEGIN
			SET @membersChanged = 1

			INSERT [TempSecurityDeletedIdentities] ([SPID], [SID], [Used])
			SELECT @@SPID, [SID], 0
			FROM [SecurityIdentity]
			WHERE [Domain] = @Domain
				AND [AccountName] = @AccountName
				AND [SID] <> @MemberSID
				AND [Deleted] = 0
				AND [Type] <> 0 -- This does not apply for InvalidIdentity
				AND [Type] <> 1 -- This does not apply for UnknownIdentityType
				AND [Type] <> 4 -- This does not apply for Role
				AND [Type] <> 5 -- This does not apply for VMS user

			-- Then mark them as deleted in the identity table
			UPDATE [SecurityIdentity]
			SET [LastModified] = @CurrentUtcTime,
				[Deleted] = 1
			WHERE [Domain] = @Domain
				AND [AccountName] = @AccountName
				AND [SID] <> @MemberSID
				AND [Deleted] = 0
				AND [Type] <> 0 -- This does not apply for InvalidIdentity
				AND [Type] <> 1 -- This does not apply for UnknownIdentityType
				AND [Type] <> 4 -- This does not apply for Role
				AND [Type] <> 5 -- This does not apply for VMS user
		
			DECLARE @deleted_sid VARCHAR(256)
		
			DECLARE identity_cursor CURSOR FAST_FORWARD FOR
			SELECT [SID] FROM [TempSecurityDeletedIdentities]

			OPEN identity_cursor

			BEGIN TRY
				FETCH NEXT FROM identity_cursor INTO @deleted_sid
				WHILE(@@FETCH_STATUS = 0)
				BEGIN
					EXEC [security_update_sid_transaction]
						@oldsid = @deleted_sid,
						@newsid = @MemberSID,
						@newaccount = @AccountName,
						@newdomain = @Domain,
						@force = 1

					FETCH NEXT FROM identity_cursor INTO @deleted_sid
				END
			END TRY
			BEGIN CATCH
				CLOSE identity_cursor
				DEALLOCATE identity_cursor
				;THROW
			END CATCH

			CLOSE identity_cursor
			DEALLOCATE identity_cursor
		END

		-- Check if the identity exists
		IF EXISTS(
			SELECT [SID]
			FROM [SecurityIdentity]
			WHERE [SID] = @MemberSID
		)
		BEGIN
			-- The identity is already added - update it if anything changed
			UPDATE [SecurityIdentity]
			SET [Type] = @Type,
				[Domain] = @Domain,
				[AccountName] = @AccountName,
				[Deleted] = 0
			WHERE [SID] = @MemberSID
				AND (
						[Domain] <> @Domain
					OR [AccountName] <> @AccountName
					OR [Deleted] = 1
				)

			SELECT @rowCount = @@ROWCOUNT

			IF(@rowCount > 0)
			BEGIN
				SET @membersChanged = 1
			END

			UPDATE [SecurityIdentity]
			SET	[DisplayName] = @DisplayName,
				[Description] = @Description,
				[DistinguishedName] = @DistinguishedName,
				[LastModified] = @CurrentUtcTime
			WHERE [SID] = @MemberSID
				AND (
					[DisplayName] <> @DisplayName
					OR [Description] <> @Description
					OR [DistinguishedName] <> @DistinguishedName
					OR [Domain] <> @Domain
					OR [AccountName] <> @AccountName
					OR [Deleted] = 1
					)	
		END
		ELSE
		BEGIN
			SET @membersChanged = 1
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
				@MemberSID,
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

		-- Check if this is a WindowsGroup or Role and we're updating child members
		IF(@UpdateMembers = 1 AND (@Type = 3 OR @Type = 4)) -- Type 3 is defined as WindowsGroups; Type 4 is Roles
		BEGIN
			-- First we need to find any removed members from the group
			-- and insert them into the temporary table to hold deleted identities
			INSERT INTO [TempSecurityDeletedIdentities] (
				[SPID],
				[SID],
				[Used]
			)
			SELECT @@SPID, sm.MemberSID, 1
			FROM [SecurityMembership] sm
			LEFT JOIN [TempSecurityIdentities] identitiesToAdd ON 
						sm.MemberSID = identitiesToAdd.[SID] 
				AND identitiesToAdd.[SPID] = @@SPID
			INNER JOIN SecurityIdentity si ON sm.[MemberSid] = si.[SID]
			WHERE sm.Container = @MemberSID
				AND identitiesToAdd.[SID] IS NULL
				AND si.Domain NOT IN
				(
					SELECT ID FROM @failingDomains
				)

			-- Again we need to check if other identities with the same domain/account name already exists
			-- with a different sid. This could happen if one of the group members has been deleted and then 
			-- created from Active Directory. If this is the case, the identities in the cache is deleted from
			-- AD and we should mark them deleted.

			IF EXISTS(
				SELECT si.[sid]
				FROM [SecurityIdentity] si
				INNER JOIN [TempSecurityIdentities] tsi ON
							si.[Domain] = tsi.[domain]
					AND si.[AccountName] = tsi.[AccountName]
					AND si.[SID] <> tsi.[SID]
					AND tsi.[SPID] = @@SPID
				WHERE si.[Deleted] = 0
					AND (si.[Type] = 2 OR si.[Type] = 3))
			BEGIN
				INSERT [TempSecurityDeletedIdentities] (
					[SPID],
					[SID],
					[Used]
				)
				SELECT @@SPID, si.[SID], 0
				FROM [SecurityIdentity] si
				INNER JOIN [TempSecurityIdentities] tsi ON
							si.[Domain] = tsi.[domain]
					AND si.[AccountName] = tsi.[AccountName]
					AND si.[SID] <> tsi.[SID]
					AND tsi.[SPID] = @@SPID
				LEFT JOIN [TempSecurityDeletedIdentities] deletedIdentities ON si.[SID] = deletedIdentities.[SID]
				WHERE si.[Deleted] = 0
					AND (si.[Type] = 2 OR si.[Type] = 3)
					AND deletedIdentities.[SID] IS NULL
					AND si.Domain NOT IN
					(
						SELECT ID FROM @failingDomains
					)
		
				-- Then mark them as deleted in the identity table
				UPDATE [SecurityIdentity]
				SET [LastModified] = @CurrentUtcTime,
						[Deleted] = 1
				FROM  [SecurityIdentity] si
				INNER JOIN [TempSecurityIdentities] tsi ON
							si.[Domain] = tsi.[Domain]
					AND si.[AccountName] = tsi.[AccountName]
					AND si.[SID] <> tsi.[SID]
					AND tsi.[SPID] = @@SPID
					AND si.Domain NOT IN
					(
						SELECT ID FROM @failingDomains
					)
			END

			-- Update existing members
			UPDATE [SecurityIdentity]
			SET [Type] = tmp.[Type],
				[Domain] = tmp.[Domain],
				[AccountName] = tmp.[AccountName],
				[Deleted] = 0
			FROM [SecurityIdentity] si
			INNER JOIN [TempSecurityIdentities] tmp ON si.[SID] = tmp.[SID] AND tmp.[SPID] = @@SPID
			WHERE (si.[Domain] != tmp.[Domain]
						OR si.[AccountName] != tmp.[AccountName]
						OR si.[Deleted] = 1)
					AND si.Domain NOT IN
					(
						SELECT ID FROM @failingDomains
					)

			SELECT @rowCount = @@RowCount

			IF(@rowCount > 0)
			BEGIN
				SET @membersChanged = 1
			END

			UPDATE [SecurityIdentity]
			SET [Type] = tmp.[Type],
					[DisplayName] = tmp.[DisplayName],
					[Description] = tmp.[Description],
					[DistinguishedName] = tmp.[DistinguishedName],
					[LastModified] = @CurrentUtcTime
			FROM [SecurityIdentity] si
			INNER JOIN [TempSecurityIdentities] tmp ON si.[SID] = tmp.[SID] AND tmp.[SPID] = @@SPID
			WHERE (si.[DisplayName] != tmp.[DisplayName]
						OR si.[Description] != tmp.[Description]
						OR si.[Domain] != tmp.[Domain]
						OR si.[AccountName] != tmp.[AccountName]
						OR si.[DistinguishedName] != tmp.[DistinguishedName]
						OR si.[Deleted] = 1)
					AND si.Domain NOT IN
					(
						SELECT ID FROM @failingDomains
					)

			-- Insert new identities
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
			SELECT
				tsi.[SID],
				tsi.[Type],
				tsi.[DisplayName],
				tsi.[Description],
				tsi.[Domain],
				tsi.[AccountName],
				tsi.[DistinguishedName],
				0,
				@CurrentUtcTime
			FROM [TempSecurityIdentities] tsi
			LEFT JOIN [SecurityIdentity] si ON si.[SID] = tsi.[SID]
			WHERE tsi.[SPID] = @@SPID AND si.[SID] IS NULL
	
			SELECT @rowCount = @@RowCount

			IF(@rowCount > 0)
			BEGIN
				SET @membersChanged = 1
			END

			-- Now we need to remove any deleted Windows group members 
			DELETE [SecurityMembership]
			FROM [SecurityMembership] sm
			LEFT JOIN [TempSecurityIdentities] tsi ON sm.[MemberSID] = tsi.[SID]
				AND tsi.[SPID] = @@SPID 
			LEFT JOIN [SecurityIdentity] si ON si.[SID] = sm.[MemberSID]
			WHERE sm.[Container] = @MemberSID 
			AND tsi.[SID] IS NULL 
			AND si.[Type] <> 5
			AND si.Domain NOT IN
			(
				SELECT ID FROM @failingDomains
			)

			SELECT @rowCount = @@ROWCOUNT

			IF(@rowCount > 0)
			BEGIN
				SET @membersChanged = 1
			END

			-- Now we need to insert any new group members
			INSERT INTO [SecurityMembership] (
				[Container],
				[MemberSID]
			)
			SELECT @MemberSID, tsi.[SID]
			FROM [TempSecurityIdentities] tsi
			LEFT JOIN [SecurityMembership] sm ON sm.[Container] = @MemberSID 
				AND sm.[MemberSID] = tsi.[SID]
			WHERE tsi.[SPID] = @@SPID AND sm.[MemberSID] IS NULL
		
			SELECT @rowCount = @@ROWCOUNT

			IF(@rowCount > 0)
			BEGIN
				SET @membersChanged = 1
			END

			-- If we added or removed group members, update the group
			IF(@membersChanged = 1)
			BEGIN
				UPDATE [SecurityIdentity]
				SET [LastModified] = @CurrentUtcTime
				WHERE [SID] = @MemberSID
			END
		END

		-- Now we need to clear membership cache and the permission 
		-- cache. These will be populated again when needed.
		DELETE FROM [SecurityAccessCache2]

		DELETE FROM [SecurityMembershipCache]

		-- If we found any identities to delete, then perform the delete
		IF EXISTS(
			SELECT [SID]
			FROM [TempSecurityDeletedIdentities]
			WHERE [SPID] = @@SPID
		)
		BEGIN

			SET @membersChanged = 1
			-- Call the delete procedure
			EXEC security_delete_identities_transaction @CurrentUtcTime
		END

		SELECT @membersChanged AS [MembersChanged]

		COMMIT TRAN
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