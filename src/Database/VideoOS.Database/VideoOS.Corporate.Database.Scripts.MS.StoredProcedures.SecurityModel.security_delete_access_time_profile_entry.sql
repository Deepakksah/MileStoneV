-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_delete_access_time_profile_entry]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_delete_access_time_profile_entry]
GO

CREATE PROCEDURE [security_delete_access_time_profile_entry]
	@namespace_id UNIQUEIDENTIFIER,
	@object_id NVARCHAR(300),
	@object_name [NameType], 
	@action_name NVARCHAR(260),
	@user_sid [SIDType],
	@session_id GuidType
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRAN

		DECLARE @security_object_id UNIQUEIDENTIFIER
		SET @security_object_id = (SELECT TOP 1 security_object_id FROM [SecurityObject] WHERE [object_id] = @object_id)

		DECLARE @access_entry_id INT
		SELECT @access_entry_id = [access_entry_id]
		FROM [SecurityAccessEntry]
		WHERE [security_object_id] = @security_object_id
			AND [action_name] = @action_name
			AND [user_sid] = @user_sid
	
		IF(@access_entry_id IS NOT NULL)
		BEGIN
			DECLARE @roleName NameType
			DECLARE @roleID GuidType
			DECLARE @namespaceName NVARCHAR(260)
			DECLARE @currentIsDefault BIT
			DECLARE @currentTimeProfileId [GuidType]
			DECLARE @currentTimeProfileName [NameType]

			SELECT @currentIsDefault = [is_default], @currentTimeProfileId = [timeprofile_id]
			FROM [SecurityAccessEntryTimeProfile]
			WHERE [access_entry_id] = @access_entry_id 

			SELECT @currentTimeProfileName = [name] FROM [TimeProfile] WHERE [pkid] = @currentTimeProfileId

			SELECT @roleID = [IDRole], @roleName = [Name] FROM [SecurityRoles] WHERE [IDRole] = @user_sid

			SELECT @namespaceName =
				CASE @namespace_id
					WHEN '8732FCAC-A63C-46EA-9683-532B4C6644E7' THEN '{SmartWall}' -- Video wall OEM
					WHEN '988D493D-1597-4BE6-AFF0-396BFD5D047A' THEN  '{MatrixApplicationName}' -- Matrix OEM
					ELSE [display_name]
				END
			FROM [SecurityNamespace] 
			WHERE [namespace_id] = @namespace_id

			DELETE FROM [SecurityAccessEntryTimeProfile]
			WHERE [access_entry_id] IN
			(
				SELECT [access_entry_id]
				FROM [SecurityAccessEntry]
				WHERE [security_object_id] = @security_object_id
					AND [action_name] = @action_name
					AND [user_sid] = @user_sid
			)

			IF(@currentIsDefault = 1)
			BEGIN
				-- Profile changed from Default to Always
				EXEC create_pending_log @session_id, 9019, @namespaceName, @action_name, @object_id, @object_name, @roleID, @roleName, 'Default', 'Always'
			END
			ELSE IF(@currentTimeProfileName IS NOT NULL)
			BEGIN
				-- Profile changed from timeprofile to Always
				EXEC create_pending_log @session_id, 9017, @namespaceName, @action_name, @object_id, @object_name, @roleID, @roleName, @currentTimeProfileName, 'Always'
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