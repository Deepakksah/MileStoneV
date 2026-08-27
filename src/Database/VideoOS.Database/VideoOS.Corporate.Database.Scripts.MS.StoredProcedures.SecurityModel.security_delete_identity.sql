-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_delete_identity]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_delete_identity]
GO

CREATE PROCEDURE [security_delete_identity]
	@SID [SIDType],
	@AdSync [BIt] = NULL
AS
BEGIN
	SET NOCOUNT ON	
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRAN

		-- Lock tables for update
		EXEC [security_table_lock] @LockLevel = 0

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()
	
		-- Update the timestamp for all groups this identity is a member of
		UPDATE [SecurityIdentity]
		SET [LastModified] = @CurrentUtcTime
		WHERE [SID] IN (
			SELECT [Container]
			FROM [SecurityMembership]
			WHERE [MemberSID] = @SID
		)

		-- Insert members into deleted-members table, but assuming that they are still in use
		INSERT [TempSecurityDeletedIdentities] ( [SPID], [SID], [Used] )
		SELECT @@SPID, [MemberSID], 1
		FROM [SecurityMembership]
		WHERE [Container] = @SID

		-- Delete members from security membership
		DELETE FROM [SecurityMembership]
		WHERE [Container] = @SID OR [MemberSID] = @SID

		IF(@AdSync is NULL OR @AdSync <> 1)
		BEGIN
			-- Create a delete admin task in idp if the identity is part of an admin role before removing it from SecurityRoleIdentity
			IF EXISTS (SELECT * FROM [SecurityRolesIdentity] WHERE [SID] = @SID AND [IDROLE] = '1f30d14b-ba91-4e8a-8838-eda17931f8fc')
			BEGIN
				EXEC [create_task_delete_idp_admin_transaction]	@SID
			END	

			-- Remove the identity from any roles that have the identity as a member
			DELETE FROM [SecurityRolesIdentity]
			WHERE [SID] = @SID
		END

		-- Mark all unreferenced SIDs as deleted
		EXEC [security_delete_identities_transaction] @CurrentUtcTime

		-- Mark this identity as deleted
		UPDATE [SecurityIdentity]
		SET [deleted] = 1,
				[LastModified] = @CurrentUtcTime
		WHERE [SID] = @SID

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