-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_delete_identities_transaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_delete_identities_transaction]
GO

CREATE PROCEDURE [security_delete_identities_transaction]
	@CurrentUtcTime DATETIME
AS
BEGIN
	SET NOCOUNT ON
 
	DECLARE @rowCount INT

  -- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	-- Make sure that a current transaction is active
	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END

	-- Check if any identities is not referenced
	WHILE(1 = 1)
	BEGIN
		-- Find unused security identities
		UPDATE [TempSecurityDeletedIdentities]
		SET [Used] = 0
		WHERE [SPID] = @@SPID
			AND [SID] NOT IN (SELECT sm.[MemberSID] FROM [SecurityMembership] as sm)
			AND [SID] NOT IN (SELECT sri.[SID] FROM [SecurityRolesIdentity] as sri)

		-- Now mark any unused security identities as deleted in the identity table
		UPDATE [SecurityIdentity]
		SET [LastModified] = @CurrentUtcTime,
			[Deleted] = 1
		FROM [SecurityIdentity] si
		INNER JOIN [TempSecurityDeletedIdentities] tmpDeleted ON tmpDeleted.[SPID] = @@SPID
			AND si.[SID] = tmpDeleted.[SID]
			AND tmpDeleted.[Used] = 0

		-- If any of the unused identities are groups, add the group members to the list
		INSERT INTO [TempSecurityDeletedIdentities] (
			[SPID],
			[SID],
			[Used]
		)
		SELECT DISTINCT @@SPID, sm.[MemberSID], 1
		FROM [SecurityMembership] sm
		INNER JOIN [TempSecurityDeletedIdentities] tmpDeleted ON tmpDeleted.[SPID] = @@SPID
			AND sm.[Container] = tmpDeleted.[SID]
			AND tmpDeleted.[used] = 0
		WHERE sm.[MemberSID] NOT IN (
			SELECT [SID]
			FROM [TempSecurityDeletedIdentities]
			WHERE [SPID] = @@SPID)

		SELECT @rowCount = @@ROWCOUNT

		-- If nothing was inserted, we're done
		IF(@rowCount = 0)
			BREAK

		-- Now deleted the members of the unused groups from SecurityMembership
		DELETE [SecurityMembership]
		WHERE [Container] IN (
			SELECT [SID]
			FROM [TempSecurityDeletedIdentities]
			WHERE [SPID] = @@SPID AND [Used] = 0)

		-- Now deleted the unused security identities from temp table
		DELETE [TempSecurityDeletedIdentities]
		WHERE [SPID] = @@SPID AND [Used] = 0
	END

	BEGIN TRY
		-- Before we leave we should try to clear the temporary table
		-- We're not interested in any errors, the temporary table will
		-- be cleared before used again
		DELETE [TempSecurityDeletedIdentities]
		WHERE [SPID] = @@SPID
	END TRY
	BEGIN CATCH
	END CATCH
END
GO