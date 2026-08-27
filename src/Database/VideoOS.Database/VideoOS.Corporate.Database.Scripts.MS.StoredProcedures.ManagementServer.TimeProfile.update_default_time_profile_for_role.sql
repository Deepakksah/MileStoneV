-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_default_time_profile_for_role]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_default_time_profile_for_role]
GO

CREATE PROCEDURE [update_default_time_profile_for_role]
	@IDRole GuidType,
	@IDTimeProfile GuidType,
	@Session_id GuidType
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
  		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

  		BEGIN TRANSACTION

		DECLARE @dataSetBefore XML
		DECLARE @dataSetAfter XML
		DECLARE @beforeTimeProfileId [GuidType]
		DECLARE @beforeTimeProfileName [NameType]
	
		SET @dataSetBefore = dbo.timeProfileDefaultRoleRelationAsDataSet(@IDRole)

		SELECT @beforeTimeProfileId = [IDTimeProfile]
		FROM [TimeProfileDefaultRoleRelation]
		WHERE IDRole = @IDRole

		IF (@beforeTimeProfileId IS NOT NULL)
		BEGIN
			SELECT @beforeTimeProfileName = [name]
			FROM [TimeProfile]
			WHERE [pkid] = @beforeTimeProfileId
		
			IF (@IDTimeProfile IS NULL)
			BEGIN
				DELETE [TimeProfileDefaultRoleRelation] WHERE [IDRole] = @IDRole
				SET @dataSetAfter = dbo.timeProfileDefaultRoleRelationAsDataSet(@IDRole)

				EXEC [create_task_transaction]
					@type = 'diffgram_security_delete',
					@target = '909749AE-88C4-4780-A381-383D691E98FA',		--ManagementServerFixedId
					@data_a = @dataSetBefore,
					@current_time = @CurrentUtcTime
			END
			ELSE
			BEGIN
				UPDATE [TimeProfileDefaultRoleRelation] SET [IDTimeProfile] = @IDTimeProfile WHERE [IDRole] = @IDRole
				SET @dataSetAfter = dbo.timeProfileDefaultRoleRelationAsDataSet(@IDRole)

				EXEC [create_task_transaction]
					@type = 'diffgram_security_update',
					@target = '909749AE-88C4-4780-A381-383D691E98FA',		--ManagementServerFixedId
					@data_a = @dataSetBefore,
					@data_b = @dataSetAfter,
					@current_time = @CurrentUtcTime			
			END
		END
		ELSE
		BEGIN
			if (@IDTimeProfile IS NOT NULL)
			BEGIN
				INSERT INTO [TimeProfileDefaultRoleRelation] ([IDRole],[IDTimeProfile] ) values (@IDRole, @IDTimeProfile)
				SET @dataSetAfter = dbo.timeProfileDefaultRoleRelationAsDataSet(@IDRole)

				EXEC [create_task_transaction]
					@type = 'diffgram_security_insert',
					@target = '909749AE-88C4-4780-A381-383D691E98FA',
					@data_a = @dataSetAfter,
					@current_time = @CurrentUtcTime
			END
		END

		DECLARE @afterTimeProfileName [NameType]

		IF (@IDTimeProfile IS NOT NULL)
		BEGIN
			SELECT @afterTimeProfileName = [name]
			FROM [TimeProfile]
			WHERE [pkid] = @IDTimeProfile
		END
	
		DECLARE @roleName [LongNameType]
		SELECT @roleName = [SecurityRoles].[Name]
		FROM [SecurityRoles]
		WHERE [SecurityRoles].[IDRole] = @IdRole

		IF(@afterTimeProfileName IS NULL)
			SET @afterTimeProfileName = 'Always' -- default timeprofile value
		IF(@beforeTimeProfileName IS NULL)
			SET @beforeTimeProfileName = 'Always' -- default timeprofile value
	
		DECLARE @auditLog BIT
		-- Audit log the change
		IF(@beforeTimeProfileName <> @afterTimeProfileName)
		BEGIN
			SET @auditLog = 1
		END

		COMMIT TRANSACTION

		IF (@auditLog = 1)
		BEGIN
			EXEC create_pending_log @Session_id, 9032, @roleName, @beforeTimeProfileName, @afterTimeProfileName, @IDRole
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