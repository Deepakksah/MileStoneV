-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_update_role]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_update_role]
GO

CREATE PROCEDURE [security_update_role]
	@IDRole [GuidType],
	@Name [NameType],
	@Description [DescriptionType],
	@DualAuthorizationRequired BIT,
	@PtzReservedAsAnonymous BIT,
	@AllowMobileClientLogOn BIT,
	@AllowSmartClientLogOn BIT,
	@AllowWebClientLogOn BIT,
	@session_id [GuidType]
 AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRAN

		DECLARE @currentRoleName [NameType]
		DECLARE @currentDualAuthorizationRequired BIT
		DECLARE @currentPtzReservedAsAnonymous BIT
		DECLARE @currentAllowMobileClientLogOn BIT
		DECLARE @currentAllowSmartClientLogOn BIT
		DECLARE @currentAllowWebClientLogOn BIT
		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()
	
		-- Make sure role exists
		IF NOT EXISTS (
			SELECT [IDRole]
			FROM [SecurityRoles] WITH(UPDLOCK)
			WHERE [IDRole] = @IDRole)
		BEGIN
			SET @ErrorArgs = @procedureName + N': %%IDRole=' + CAST(@IDRole AS NVARCHAR(36)) + N'%%'
			;THROW 60002, @ErrorArgs, 1;
		END
	
		SELECT @currentRoleName = sr.[Name], 
			@currentDualAuthorizationRequired = sr.[DualRequired], 
			@currentPtzReservedAsAnonymous = sr.[PtzReservedAsAnonymous], 
			@currentAllowMobileClientLogOn = sr.[AllowMobileClientLogOn], 
			@currentAllowSmartClientLogOn = sr.[AllowSmartClientLogOn], 
			@currentAllowWebClientLogOn = sr.[AllowWebClientLogOn]
		FROM [SecurityRoles] sr
		WHERE [IDRole] = @IDRole

		IF (@Name IS NULL AND @Description IS NULL AND @DualAuthorizationRequired IS NULL AND @PtzReservedAsAnonymous IS NULL AND @AllowMobileClientLogOn IS NULL AND @AllowSmartClientLogOn IS NULL AND @AllowWebClientLogOn IS NULL)
		BEGIN
			ROLLBACK TRAN
			RETURN 0
		END

		DECLARE @dataSetBefore XML
		SET @dataSetBefore = dbo.securityRolesAsDataSet(@IDRole)

		-- Lock tables for update
		EXEC [security_table_lock] @LockLevel = 0

		BEGIN TRY
			UPDATE [SecurityRoles]
			SET [Name] = COALESCE(@Name, [Name]),
					[Description] = COALESCE(@Description, [Description]),
					[DualRequired] = COALESCE(@DualAuthorizationRequired, [DualRequired]),
					[PtzReservedAsAnonymous] = COALESCE(@PtzReservedAsAnonymous, [PtzReservedAsAnonymous]),
					[AllowMobileClientLogOn] = COALESCE(@AllowMobileClientLogOn, [AllowMobileClientLogOn]),
					[AllowSmartClientLogOn] = COALESCE(@AllowSmartClientLogOn, [AllowSmartClientLogOn]),
					[AllowWebClientLogOn] = COALESCE(@AllowWebClientLogOn, [AllowWebClientLogOn]),
					[LastModified] = @CurrentUtcTime
			WHERE [IDRole] = @IDRole
		END TRY
		BEGIN CATCH
			IF(ERROR_NUMBER() = 2601 OR ERROR_NUMBER() = 2627) -- CONSTRAINT VIOLATION
			BEGIN
				-- The name already exists
				SET @ErrorArgs = @procedureName + N': %%Name=' + @Name + N'%%'
				;THROW 60003, @ErrorArgs, 1;
			END
			;THROW
		END CATCH

		-- We also must update the security identity table
		UPDATE [SecurityIdentity]
		SET [DisplayName] = @Name,
			[Description] = @Description,
			[LastModified] = @CurrentUtcTime
		WHERE [SID] = @IDRole
  
		-- Get DataSet after changes
		DECLARE @dataSetAfter XML
		SET @dataSetAfter = dbo.securityRolesAsDataSet(@IDRole)

		EXEC [create_task_transaction]
			@type = 'diffgram_security_update',
			@target = '909749AE-88C4-4780-A381-383D691E98FA',
			@data_a = @dataSetBefore,
			@data_b = @dataSetAfter,
			@current_time = @CurrentUtcTime

		COMMIT TRAN
	
		IF(@currentRoleName <> @Name)
		BEGIN
			EXEC create_pending_log @session_id, 9004, @IDRole, @Name, @currentRoleName
		END
		IF(@currentDualAuthorizationRequired <> @DualAuthorizationRequired)
		BEGIN
			IF(@DualAuthorizationRequired = 1)
				EXEC create_pending_log @session_id, 9036, @IDRole, @Name	
			ELSE
				EXEC create_pending_log @session_id, 9037, @IDRole, @Name
		END
		IF(@currentPtzReservedAsAnonymous <> @PtzReservedAsAnonymous)
		BEGIN
			IF(@PtzReservedAsAnonymous = 1)
				EXEC create_pending_log @session_id, 9038, @IDRole, @Name	
			ELSE
				EXEC create_pending_log @session_id, 9039, @IDRole, @Name
		END
		IF(@currentAllowMobileClientLogOn <> @AllowMobileClientLogOn)
		BEGIN
			IF(@AllowMobileClientLogOn = 1)
				EXEC create_pending_log @session_id, 9034, @IDRole, @Name, 'mobileclient'	
			ELSE
				EXEC create_pending_log @session_id, 9035, @IDRole, @Name, 'mobileclient'
		END
		IF(@currentAllowSmartClientLogOn <> @AllowSmartClientLogOn)
		BEGIN
			IF(@AllowSmartClientLogOn = 1)
				EXEC create_pending_log @session_id, 9034, @IDRole, @Name, 'smartclient'	
			ELSE
				EXEC create_pending_log @session_id, 9035, @IDRole, @Name, 'smartclient'
		END
		IF(@currentAllowWebClientLogOn <> @AllowWebClientLogOn)
		BEGIN
			IF(@AllowWebClientLogOn = 1)
				EXEC create_pending_log @session_id, 9034, @IDRole, @Name, 'webclient'	
			ELSE
				EXEC create_pending_log @session_id, 9035, @IDRole, @Name, 'webclient'
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