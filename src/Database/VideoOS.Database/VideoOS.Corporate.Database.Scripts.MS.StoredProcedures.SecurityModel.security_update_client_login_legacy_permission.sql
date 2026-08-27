-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_update_client_login_legacy_permission]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_update_client_login_legacy_permission]
GO

CREATE PROCEDURE [security_update_client_login_legacy_permission]
	@IDRole [GuidType],
	@IDTimeProfile [GuidType],
	@IsDefaultTimeProfile BIT,
	@Session_id [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRAN
	
		DECLARE @SecurityAttributeID varchar(30)
		SELECT @SecurityAttributeID = 'APPLICATIONACCESSCLIENTLOGIN'

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()
	
		-- Validate parameters
		IF(@IDTimeProfile IS NOT NULL AND @IsDefaultTimeProfile = 1 OR (@IDTimeProfile IS NULL AND @IsDefaultTimeProfile IS NULL))
		BEGIN
			;THROW 60000, @procedureName, 1;
		END
	
		DECLARE @beforeTimeProfileId [GuidType]
		DECLARE @beforeTimeProfileIsDefault [BIT]
		DECLARE @beforeTimeProfileName [NameType]
		DECLARE @afterTimeProfileName [NameType]
		DECLARE @lastModified [DateTime]

		SELECT @lastModified = LastModified, @beforeTimeProfileId = IDTimeProfile, @beforeTimeProfileIsDefault = IsDefaultTimeProfile
		FROM [SecurityPermissions] WITH (UPDLOCK)
		WHERE [IDRole] = @IDRole
		AND [SecurityAttributeID] = @SecurityAttributeID

		-- Make sure permission exists
		IF (@lastModified IS NULL)
		BEGIN
			;THROW 65010, @procedureName, 1;
		END

		-- Make sure timeprofile exists
		IF(@IDTimeProfile IS NOT NULL)
		BEGIN
			SELECT @afterTimeProfileName = [name]
			FROM [TimeProfile] WITH (UPDLOCK)
			WHERE [pkid] = @IDTimeProfile
		
			IF (@afterTimeProfileName IS NULL)
			BEGIN
				SET @ErrorArgs = @procedureName + N': %%IDTimeProfile=' + CAST(@IDTimeProfile AS NVARCHAR(36)) + N'%%'
				;THROW 65011, @ErrorArgs, 1;
			END
		END
	
		DECLARE @dataSetBefore XML
		SET @dataSetBefore = dbo.securityPermissionsAsDataSet(@IDRole)
	
		UPDATE [SecurityPermissions]
		SET [IDTimeProfile] = @IDTimeProfile,
			[IsDefaultTimeProfile] = COALESCE(@IsDefaultTimeProfile, [IsDefaultTimeProfile]),
			[LastModified] = @CurrentUtcTime
		WHERE [IDRole] = @IDRole
		AND [SecurityAttributeID] = @SecurityAttributeID

		-- Get DataSet after changes
		DECLARE @dataSetAfter XML
		SET @dataSetAfter = dbo.securityPermissionsAsDataSet(@IDRole)

		EXEC [create_task_transaction]
			@type = 'diffgram_security_update',
			@target = '909749AE-88C4-4780-A381-383D691E98FA',
			@data_a = @dataSetBefore,
			@data_b = @dataSetAfter,
			@current_time = @CurrentUtcTime
	
		DECLARE @roleName [LongNameType]
		SELECT @roleName = [SecurityRoles].[Name]
		FROM [SecurityRoles]
		WHERE [SecurityRoles].[IDRole] = @IdRole

		IF (@beforeTimeProfileId IS NOT NULL)
		BEGIN
			SELECT @beforeTimeProfileName = [name]
			FROM [TimeProfile] WITH (UPDLOCK)
			WHERE [pkid] = @beforeTimeProfileId
		END
		ELSE
		BEGIN
			IF (@beforeTimeProfileIsDefault = 1)
			BEGIN
				SET @beforeTimeProfileName = 'Default'
			END
			ELSE
			BEGIN
				SET @beforeTimeProfileName = 'Always'
			END
		END

		IF(@IsDefaultTimeProfile = 1)
		BEGIN
			SET @afterTimeProfileName = 'Default'
		END

		IF(@IsDefaultTimeProfile = 0 AND @IDTimeProfile IS NULL)
		BEGIN
			SET @afterTimeProfileName = 'Always'
		END

		-- Audit log the change
		EXEC create_pending_log @Session_id, 9033, @roleName, @beforeTimeProfileName, @afterTimeProfileName, @IDRole

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