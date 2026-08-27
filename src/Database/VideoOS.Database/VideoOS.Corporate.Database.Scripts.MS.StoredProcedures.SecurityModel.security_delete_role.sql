-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_delete_role]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_delete_role]
GO

CREATE PROCEDURE [security_delete_role]
	@IDRole [GuidType],
	@session_id [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @roleType TINYINT
		DECLARE @roleName [NameType]

		BEGIN TRAN

		-- Lock tables for update
		EXEC [security_table_lock] @LockLevel = 0

		-- Is this a valid role that we can delete?
		SELECT @roleType = sr.Type, @roleName = sr.Name
		FROM [SecurityRoles] sr WITH(UPDLOCK)
		WHERE sr.[IDRole] = @IDRole

		-- Does the role exist?
		IF(@roleType IS NULL)
		BEGIN
			SET @ErrorArgs = @procedureName + N': %%IDRole=' + CAST(@IDRole AS NVARCHAR(36)) + N'%%'
			;THROW 60004, @ErrorArgs, 1;
		END

		-- Is this one of the special roles
		IF(@roleType <> 2)
		BEGIN
			;THROW 60005, @procedureName, 1;
		END

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		-- Mark the role in the identity table as deleted
		UPDATE [SecurityIdentity]
		SET [LastModified] = @CurrentUtcTime,
			[Deleted] = 1
		WHERE [SID] = @IDRole

		-- Now we delete all the members of the role in the membership cache
		DELETE [SecurityMembership]
		WHERE [Container] = @IDRole

		-- Clear the list of deleted identities
		DELETE [TempSecurityDeletedIdentities]
		WHERE [SPID] = @@SPID

		-- Insert the deleted members, assuming they are used.
		INSERT INTO [TempSecurityDeletedIdentities](
			[SPID],
			[SID],
			[Used])
		SELECT
			@@SPID,
			[SID],
			1
		FROM [SecurityRolesIdentity]
		WHERE IDRole = @IDRole

		-- Call the delete procedure
		EXEC [security_delete_identities_transaction] @CurrentUtcTime

		-- delete role specific access entries
		DELETE FROM [SecurityAccessEntryTimeProfile]
		WHERE [access_entry_id] IN
		(
			SELECT [access_entry_id]
			FROM [SecurityAccessEntry]
			WHERE [user_sid] = @IDRole
		)

		DELETE FROM [SecurityAccessEntry]
		WHERE [user_sid] = @IDRole

		-- Now delete all members of the role from the relation tables
		DELETE [SecurityRoleClaim]
		WHERE [IDRole] = @IDRole

		DELETE [SecurityRolesIdentity]
		WHERE [IDRole] = @IDRole

		-- Delete relations to client profile
		DELETE [ClientProfileRoleRelation]
		WHERE [IDRole] = @IDRole

		-- Delete relations to time profile
		DELETE [TimeProfileDefaultRoleRelation]
		WHERE [IDRole] = @IDRole

		-- Delete "SecurityPermissions" in legacy security model
		DELETE [SecurityPermissions]
		WHERE [IDRole] = @IDRole
	
		DECLARE @diffgram XML
		DECLARE @recordingserver_id [GuidType]
		DECLARE @recorder_service_id [GuidType]

		-- Create tasks to remove security-dataset entries on recording servers
		DECLARE recordingserver_cursor CURSOR FAST_FORWARD FOR
			SELECT [recorder_id], [recording_server_service_id]
			FROM [RecordingServerServiceRecorder]

		OPEN recordingserver_cursor
		BEGIN TRY
			FETCH NEXT FROM recordingserver_cursor INTO @recordingserver_id, @recorder_service_id
			WHILE(@@FETCH_STATUS = 0)
			BEGIN
				-- Create the diffgram and task for SecurityPermissionsDevice
				SELECT @diffgram = dbo.[securityPermissionsDeviceAsDataSet](@IDRole, @recordingserver_id)
				IF(@diffgram IS NOT NULL)
				BEGIN
					EXEC [create_task_transaction]
						@type = 'diffgram_security_delete',
						@target = @recorder_service_id,
						@data_a = @diffgram,
						@current_time = @CurrentUtcTime
				END

				FETCH NEXT FROM recordingserver_cursor INTO @recordingserver_id, @recorder_service_id
			END
		END TRY
		BEGIN CATCH
			CLOSE recordingserver_cursor
			DEALLOCATE recordingserver_cursor
			;THROW
		END CATCH

		CLOSE recordingserver_cursor
		DEALLOCATE recordingserver_cursor

		-- Delete "SecurityPermissionsDevice" in legacy security model
		DELETE [SecurityPermissionsDevice]
		WHERE [IDRole] = @IDRole

		-- Delete "SecurityPermissionsMatrix" in legacy security model
		DELETE [SecurityPermissionsMatrix]
		WHERE [IDRole] = @IDRole

		-- Get data set before changes
		DECLARE @dataSetBefore XML
		SET @dataSetBefore = dbo.securityRolesAsDataSet(@IDRole)

		-- Delete the role itself
		DELETE FROM [SecurityRoles]
		WHERE [IDRole] = @IDRole

		-- Now that the membership is changed, we need to clear the membership cache
		-- and the permission cache. These will be populated again when needed.

		DELETE FROM [SecurityAccessCache2]

		DELETE FROM [SecurityMembershipCache]

		-- Create diffgram
		EXEC [create_task_transaction]
			@type = 'diffgram_security_delete',
			@target = '909749AE-88C4-4780-A381-383D691E98FA',
			@data_a = @dataSetBefore,
			@current_time = @CurrentUtcTime

		COMMIT TRAN

		EXEC create_pending_log @session_id, 9002, @IDRole, @roleName 
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