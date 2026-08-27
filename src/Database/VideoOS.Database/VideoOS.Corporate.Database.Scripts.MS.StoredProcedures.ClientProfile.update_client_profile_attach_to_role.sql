-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_client_profile_attach_to_role]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_client_profile_attach_to_role]
GO

CREATE PROCEDURE [update_client_profile_attach_to_role]
	@IDClientProfile [GuidType],
	@IDRole [GuidType],
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

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()
	
		DECLARE @profile_type NVARCHAR(200)
		DECLARE @newProfileIsDefault [BIT]
		DECLARE @newProfileName [LongNameType]

		SELECT @profile_type = profile_type, @newProfileIsDefault = DefaultProfile, @newProfileName = [Name]
		FROM [ClientProfile]
		WHERE [IDClientProfile] = @IDClientProfile
	
		DECLARE @oldProfileName [LongNameType]
		DECLARE @oldProfileIsDefault [BIT]
	
		SELECT @oldProfileName = cp.[Name], @oldProfileIsDefault = cp.DefaultProfile 
		FROM ClientProfileRoleRelation cprr
		INNER JOIN ClientProfile cp ON cprr.IDClientProfile = cp.IDClientProfile
		WHERE cprr.IDRole = @IDRole
		AND cp.profile_type = @profile_type

		IF (@oldProfileName IS NOT NULL)
		BEGIN
			DELETE FROM ClientProfileRoleRelation
			WHERE IDRole = @IDRole
			AND IDClientProfile IN
			(
				SELECT cp.IDClientProfile
				FROM ClientProfile cp
				WHERE cp.profile_type = @profile_type
			)
		END
	
		INSERT INTO ClientProfileRoleRelation(IDClientProfile, IDRole)
		VALUES(@IDClientProfile, @IDRole)
	
		DECLARE @roleName [LongNameType]
		SELECT @roleName = [SecurityRoles].[Name]
		FROM [SecurityRoles]
		WHERE [SecurityRoles].[IDRole] = @IdRole

		COMMIT TRAN

		EXEC create_pending_log @Session_id, 9031, @roleName, @profile_type, @oldProfileName, @oldProfileIsDefault, @newProfileName, @newProfileIsDefault, @IDRole
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