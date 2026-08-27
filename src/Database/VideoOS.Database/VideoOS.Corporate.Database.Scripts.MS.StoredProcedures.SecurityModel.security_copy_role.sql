-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_copy_role]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_copy_role]
GO

CREATE PROCEDURE [security_copy_role]
	@IDSourceRole [GuidType],
	@IDRole [GuidType],
	@Name [NameType],
	@Description [DescriptionType]
AS
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @LastModified DATETIME
		DECLARE @DualRequired BIT
		DECLARE @PtzReservedAsAnonymous BIT
		DECLARE @AllowMobileClientLogOn BIT
		DECLARE @AllowSmartClientLogOn BIT
		DECLARE @AllowWebClientLogOn BIT
		DECLARE @RetentionOptionId GuidType
		DECLARE @before TABLE ([object_id] NVARCHAR(300), namespace_id UNIQUEIDENTIFIER, action_name NVARCHAR(260), [user_sid] [SIDType], access_deny BIT)
		DECLARE @after TABLE ([object_id] NVARCHAR(300), namespace_id UNIQUEIDENTIFIER, action_name NVARCHAR(260), [user_sid] [SIDType], access_deny BIT)
		DECLARE @overallSecurityDataSet XML

		BEGIN TRAN

		-- Lock tables for update
		SET @LastModified = GETUTCDATE()

		SELECT @DualRequired = [DualRequired], 
			@PtzReservedAsAnonymous = [PtzReservedAsAnonymous], 
			@AllowMobileClientLogOn = [AllowMobileClientLogOn], 
			@AllowSmartClientLogOn = [AllowSmartClientLogOn],
			@AllowWebClientLogOn = [AllowWebClientLogOn]
		FROM [SecurityRoles] 
		WHERE [IDRole] = @IDSourceRole

		SELECT @RetentionOptionId = [IdRetentionOption] FROM [RetentionOptionRoleRelation] WHERE [IDRole] = @IDSourceRole
	
		DECLARE @managementClientProfile [GuidType]
		DECLARE @smartClientProfile [GuidType]
	
		-- Select relations from role to Management Client Profile
		SELECT @managementClientProfile = cpr.[IDClientProfile]
		FROM [ClientProfileRoleRelation] as cpr
		JOIN [ClientProfile] as cp
		ON cpr.IDClientProfile = cp.IDClientProfile
		WHERE [IDRole] = @IDSourceRole AND profile_type = 'ManagementClientProfile'
	
		-- Select relations from role to Smart Client Profile
		SELECT @smartClientProfile = cpr.[IDClientProfile]
		FROM [ClientProfileRoleRelation] as cpr
		JOIN [ClientProfile] as cp
		ON cpr.IDClientProfile = cp.IDClientProfile
		WHERE [IDRole] = @IDSourceRole AND profile_type = 'SmartClientProfile'

		EXEC [security_create_role_transaction] @IDRole, 2, @Name, @Description, @DualRequired, @PtzReservedAsAnonymous, @AllowMobileClientLogOn, @AllowSmartClientLogOn, @AllowWebClientLogOn, @LastModified, @RetentionOptionId, @managementClientProfile, @smartClientProfile
	
		INSERT @before
		SELECT [object_id], [namespace_id], [action_name], [user_sid], [access_deny] FROM [SecurityGroupAccessEntry]

		-- Remove permissions set on role when created (CONNECT)
		DELETE FROM [SecurityAccessEntry] 
		WHERE [user_sid] = @IDRole

		INSERT INTO SecurityAccessEntry([security_object_id], [action_name], [user_sid], [access_deny])
		SELECT [security_object_id], [action_name], @IDRole, [access_deny]
		FROM [SecurityAccessEntry]
		WHERE [user_sid] = @IDSourceRole

		INSERT @after
			SELECT [object_id], [namespace_id], [action_name], [user_sid], [access_deny] FROM [SecurityGroupAccessEntry]

			-- Create dataset for added rows
			;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
			SELECT @overallSecurityDataSet = (
				SELECT * FROM @after a 
				WHERE NOT EXISTS (SELECT * FROM @before b WHERE a.[object_id] = b.[object_id] AND a.namespace_id = b.namespace_id AND a.action_name = b.action_name AND a.[user_sid] = b.[user_sid] AND a.[access_deny] = b.[access_deny])
				FOR XML PATH ('SecurityGroupAccessEntry'), ROOT('SecurityDataSet'), TYPE
			)
			IF @overallSecurityDataSet IS NOT NULL
			BEGIN
				EXEC [create_task_transaction]
					'diffgram_security_insert',
					'909749AE-88C4-4780-A381-383D691E98FA',
					@overallSecurityDataSet,
					NULL,
					@LastModified
			END

		-- Copy any claims 
		INSERT [SecurityRoleClaim] (
			[IDRole],
			[IDClaim],
			[ClaimValue]
		)
		SELECT @IDRole, [IDClaim], [ClaimValue]
		FROM [SecurityRoleClaim]
		WHERE [IDRole] = @IDSourceRole

		-- Add static identity to the role (sync will take care of the dynamic once)
		INSERT [SecurityRolesIdentity] (
			[IDRole],
			[SID],
			[LastModified],
			[IsStatic], 
			[IsDynamic]
		)
		SELECT @IDRole, [SID], @LastModified, 1, 0
		FROM [SecurityRolesIdentity]
		WHERE [IDRole] = @IDSourceRole AND [IsStatic] = 1
	
		-- Now add the identity to the membership table
		INSERT INTO [SecurityMembership] (
			[Container],
			[MemberSID]
		)
		SELECT @IDRole, [MemberSID]
		FROM [SecurityMembership]
		WHERE [Container] = @IDSourceRole
		
		-- Now that the membership is changed, we need to clear the membership cache
		-- and the permission cache. These will be populated again when needed.

		DELETE FROM [SecurityAccessCache2]

		DELETE FROM [SecurityMembershipCache]

		-- Copy Smart Client login permission (SecurityPermissions)
		INSERT INTO [SecurityPermissions] ([LastModified], [IDRole], [SecurityAttributeID], [Value], [IDTimeProfile], [IsDefaultTimeProfile])
		SELECT @LastModified, @IDRole, [SecurityAttributeID], [Value], [IDTimeProfile], [IsDefaultTimeProfile]
		FROM [SecurityPermissions]
		WHERE [IDRole] = @IDSourceRole

		DECLARE @dataSetSecurityPermissionsAfter XML
		SET @dataSetSecurityPermissionsAfter = dbo.securityPermissionsAsDataSet(@IDRole)

		IF(@dataSetSecurityPermissionsAfter IS NOT NULL)
		BEGIN
			EXEC [create_task_transaction]
				@type = 'diffgram_security_insert',
				@target = '909749AE-88C4-4780-A381-383D691E98FA',
				@data_a = @dataSetSecurityPermissionsAfter,
				@current_time = @LastModified
		END

		-- Copy access time profile security 
		INSERT INTO [SecurityAccessEntryTimeProfile] ([access_entry_id], [timeprofile_id], [is_default]) 
		SELECT saeDestination.[access_entry_id], saeSourceDT.[timeprofile_id], saeSourceDT.[is_default]
		FROM SecurityAccessEntry saeDestination
		INNER JOIN
		(
			SELECT saeSource.[access_entry_id], saetSource.[timeprofile_id], saetSource.[is_default], saeSource.[security_object_id], saeSource.[action_name]
			FROM SecurityAccessEntryTimeProfile saetSource
			JOIN SecurityAccessEntry saeSource
			ON saetSource.[access_entry_id] = saeSource.[access_entry_id]
			WHERE saeSource.[user_sid] = @IDSourceRole
		) saeSourceDT ON saeSourceDT.[action_name] = saeDestination.[action_name] AND saeSourceDT.[security_object_id] = saeDestination.[security_object_id]
		WHERE saeDestination.[user_sid] = @IDRole
		
		-- Copy device security
		INSERT INTO [SecurityPermissionsDevice] ([LastModified], [SecurityAttributeID], [IDRole], [IDDevice], [Value], [IDTimeProfile], [TimeBackward], [IsDefaultTimeProfile])
		SELECT @LastModified, [SecurityAttributeID], @IDRole, [IDDevice], [Value], [IDTimeProfile], [TimeBackward], [IsDefaultTimeProfile]
		FROM [SecurityPermissionsDevice]
		WHERE [IDRole] = @IDSourceRole

		DECLARE @recordingserver_id [GuidType]
		DECLARE @recorder_service_id [GuidType]

		DECLARE recordingserver_cursor CURSOR FAST_FORWARD FOR
			SELECT [recorder_id], [recording_server_service_id]
			FROM [RecordingServerServiceRecorder]

		OPEN recordingserver_cursor
		BEGIN TRY
			FETCH NEXT FROM recordingserver_cursor INTO @recordingserver_id, @recorder_service_id

			WHILE(@@FETCH_STATUS = 0)
			BEGIN
				-- Create the diffgram and task for SecurityPermissionsDevice
				DECLARE @dataSetSecurityPermissionsDeviceAfter XML
				SET @dataSetSecurityPermissionsDeviceAfter = dbo.securityPermissionsDeviceAsDataSet(@IDRole, @recordingserver_id)
				IF(@dataSetSecurityPermissionsDeviceAfter IS NOT NULL)
				BEGIN
					EXEC [create_task_transaction]
						@type = 'diffgram_security_insert',
						@target = @recorder_service_id,
						@data_a = @dataSetSecurityPermissionsDeviceAfter,
						@current_time = @LastModified
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

		-- Copy matrix security
		INSERT INTO [SecurityPermissionsMatrix] ([LastModified], [SecurityAttributeID], [IDRole], [IDMatrix], [Value])
		SELECT @LastModified, [SecurityAttributeID], @IDRole, [IDMatrix], [Value]
		FROM [SecurityPermissionsMatrix]
		WHERE [IDRole] = @IDSourceRole

		DECLARE @dataSetSecurityPermissionsMatrixAfter XML
		SET @dataSetSecurityPermissionsMatrixAfter = dbo.securityPermissionsMatrixAsDataSet(@IDRole)

		IF(@dataSetSecurityPermissionsMatrixAfter IS NOT NULL)
		BEGIN
			EXEC [create_task_transaction]
				@type = 'diffgram_security_insert',
				@target = '909749AE-88C4-4780-A381-383D691E98FA',
				@data_a = @dataSetSecurityPermissionsMatrixAfter,
				@current_time = @LastModified
		END
		-- Copy default time profile setting

		IF EXISTS(SELECT * FROM [TimeProfileDefaultRoleRelation] WHERE [IDRole] = @IDSourceRole)
		BEGIN
			INSERT INTO [TimeProfileDefaultRoleRelation]([IDTimeProfile], [IDRole])
			SELECT [IDTimeProfile], @IDRole
			FROM [TimeProfileDefaultRoleRelation]
			WHERE [IDRole] = @IDSourceRole

			DECLARE @dataSetTimeProfileDefaultRoleRelationAfter XML
			SET @dataSetTimeProfileDefaultRoleRelationAfter = dbo.timeProfileDefaultRoleRelationAsDataSet(@IDRole)

			IF(@dataSetTimeProfileDefaultRoleRelationAfter IS NOT NULL)
			BEGIN
				EXEC [create_task_transaction]
					@type = 'diffgram_security_insert',
					@target = '909749AE-88C4-4780-A381-383D691E98FA',
					@data_a = @dataSetTimeProfileDefaultRoleRelationAfter,
					@current_time = @LastModified
			END
		END

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