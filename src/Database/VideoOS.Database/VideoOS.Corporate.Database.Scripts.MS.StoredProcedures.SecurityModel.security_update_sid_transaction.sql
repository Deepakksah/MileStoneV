-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_update_sid]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_update_sid]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_update_sid_transaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_update_sid_transaction]
GO

CREATE PROC [security_update_sid_transaction]
	@oldsid NVARCHAR(256),
	@newsid NVARCHAR(256),
	@newaccount NVARCHAR(256),
	@newdomain NVARCHAR(256),
	@force BIT
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	DECLARE @CurrentUtcTime DATETIME
	SET @CurrentUtcTime = GETUTCDATE()

	-- Make sure that a current transaction is active
	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END

	IF NOT EXISTS(SELECT [SID] FROM [SecurityIdentity] WHERE [sid] = @oldsid)
	BEGIN
		;THROW 60000, @procedureName, 1;
	END

	-- If the newsid has a conflicting entry in the table,
	-- fail if we're not forcing and entry is not marked as deleted
	-- otherwise, force by marking conflicting user as not-deleted rather than inserting a row
	-- and by marking conflicting user's permissions as deleted
	IF EXISTS(SELECT [SID] FROM [SecurityIdentity] WHERE [SID] = @newsid)
	BEGIN
		DECLARE @new_deleted BIT
		DECLARE @old_deleted BIT

		SELECT @old_deleted = [Deleted] FROM [SecurityIdentity] WHERE [sid] = @oldsid
		SELECT @new_deleted = [Deleted] FROM [SecurityIdentity] WHERE [sid] = @newsid

		-- No change needed
		IF((@newsid = @oldsid) OR
			 (@old_deleted = 1 AND @new_deleted = 0) OR
			 (@old_deleted = 0 AND @new_deleted = 0 AND @force = 0))
		BEGIN
			;THROW 60000, @procedureName, 1;
		END

		-- Mark the the conflicting new account's row as not deleted
		UPDATE [SecurityIdentity]
		SET
			[Domain] = @newdomain,
			[AccountName] = @newaccount,
			[Deleted] = 0,
			[LastModified] = @CurrentUtcTime
		WHERE [SID] = @newsid
	END
	ELSE
	BEGIN
		-- Make a new row for this account in identity cache (everything else will be stale until synched later)
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
			@newsid,
			[Type],
			[DisplayName],
			[Description],
			@newdomain,
			@newaccount,
			[DistinguishedName],
			[Deleted],
			@CurrentUtcTime
		FROM [SecurityIdentity]
		WHERE [SID] = @oldsid
	END
	
	-- delete the new identity's access entries (if any)
	DELETE FROM [SecurityAccessEntryTimeProfile]
	WHERE [access_entry_id] IN
	(
		SELECT [access_entry_id]
		FROM [SecurityAccessEntry]
		WHERE [user_sid] = @newsid
	)

	DELETE FROM [SecurityAccessEntry]
	WHERE [user_sid] = @newsid

	-- Update the last modified of groups that had new identity as a member (if any).
	UPDATE [SecurityIdentity]
	SET [LastModified] = @CurrentUtcTime
	FROM [SecurityMembership] groups
	INNER JOIN [SecurityIdentity] ids ON groups.[Container] = ids.[SID]
	WHERE groups.[MemberSID] = @newsid
	
	-- Delete the new identity from memberships where new identity is member (if any).
	DELETE FROM [SecurityMembership]
	WHERE [MemberSID] = @newsid

	-- Delete the new identity from membership cache where new identity is member (if any).
	DELETE FROM [SecurityMembershipCache]
	WHERE [MemberSID] = @newsid

	-- Delete the new identity from membership cache where new identity is container (if any).
	DELETE FROM [SecurityMembershipCache]
	WHERE [GroupSID] = @newsid

	-- Transfer the old identity's permissions to new identity

	-- Grant any ACLs owned purely by the old sid
	INSERT [SecurityAccessEntry] (
		[security_object_id],
		[action_name],
		[user_sid],
		[access_deny]
	)
	SELECT 
		old_acls.[security_object_id],
		old_acls.[action_name],
		@newsid,
		old_acls.[access_deny]
	FROM [SecurityAccessEntry] old_acls
	LEFT JOIN [SecurityAccessEntry] new_acls ON old_acls.[security_object_id] = new_acls.[security_object_id]
				AND old_acls.[action_name] = new_acls.[action_name] 
				AND new_acls.[user_sid] = @newsid
	WHERE old_acls.[user_sid] = @oldsid AND new_acls.[user_sid] IS NULL

	-- delete old identity's acls
	DELETE FROM [SecurityAccessEntryTimeProfile]
	WHERE [access_entry_id] IN
	(
		SELECT [access_entry_id]
		FROM [SecurityAccessEntry]
		WHERE [user_sid] = @oldsid
	)

	DELETE FROM [SecurityAccessEntry]
	WHERE [user_sid] = @oldsid

	-- Update the last modified of groups that have the old identity as a member.
	UPDATE [SecurityIdentity]
	SET [LastModified] = @CurrentUtcTime
	FROM [SecurityMembershipCache] groups
	INNER JOIN [SecurityIdentity] ids ON groups.[GroupSID] = ids.[SID]
	WHERE groups.[MemberSID] = @oldsid

	-- Transfer memberships of old identity to new identity
	UPDATE [SecurityMembership]
	SET [MemberSID] = @newsid
	WHERE [MemberSID] = @oldsid

	-- Update the membership cache because the customer may have ignored instruction and left the app pool might be running
	-- In that case, we do not want the membership cache to get out of sync.

	-- Transfer memberships of old identity to new identity
	UPDATE [SecurityMembership]
	SET [MemberSID] = @newsid
	WHERE [MemberSID] = @oldsid

	-- Transfer members of old identity to new identity (if identity is a group, otherwise does nothing)
	UPDATE [SecurityMembership]
	SET [Container] = @newsid
	WHERE [Container] = @oldsid

	-- Clear non-vital permission cache which will be regenerated by subsequent sync.
	DELETE FROM [SecurityAccessCache2]

	-- Clear non-vital membership cache which will be regenerated by subsequent sync.
	DELETE FROM [SecurityMembershipCache]
END
GO