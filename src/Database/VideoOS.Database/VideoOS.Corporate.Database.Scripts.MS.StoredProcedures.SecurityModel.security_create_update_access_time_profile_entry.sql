-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_create_update_access_time_profile_entry]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_create_update_access_time_profile_entry]
GO

CREATE PROCEDURE [security_create_update_access_time_profile_entry]
	@namespace_id UNIQUEIDENTIFIER,
	@object_id NVARCHAR(300),
	@object_name [NameType],
	@action_name NVARCHAR(260),
	@user_sid [SIDType],
	@timeprofile_id [GuidType],
	@is_default BIT,
	@session_id GuidType
AS
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @security_object_id UNIQUEIDENTIFIER

		BEGIN TRAN
  
		SET @security_object_id = (SELECT TOP 1 security_object_id FROM [SecurityObject] WHERE [object_id] = @object_id AND [namespace_id] = @namespace_id)
  
		IF NOT EXISTS
		(
			SELECT [access_entry_id]
			FROM [SecurityAccessEntry]
			WHERE [security_object_id] = @security_object_id
				AND [action_name] = @action_name
				AND [user_sid] = @user_sid
		)
		BEGIN
			;THROW 60000, @procedureName, 1;
		END

		DECLARE @timeProfileName NameType
		SELECT @timeProfileName = [name] FROM [TimeProfile] WHERE [pkid] = @timeprofile_id

		IF(@is_default <> 1 AND @timeprofile_id IS NOT NULL AND @timeProfileName IS NULL)
		BEGIN
			;THROW 60000, @procedureName, 1;
		END

		DECLARE @roleName NameType
		DECLARE @roleID GuidType
		DECLARE @namespaceName NVARCHAR(260)
		DECLARE @access_entry_id INT
		DECLARE @action_display_name NVARCHAR(260)

		SELECT @access_entry_id= [access_entry_id]
		FROM [SecurityAccessEntry]
		WHERE [security_object_id] = @security_object_id
			AND [action_name] = @action_name
			AND [user_sid] = @user_sid

		SELECT @action_display_name = [display_name] FROM [SecurityAction] WHERE [name] = @action_name AND [namespace_id] = @namespace_id
		SELECT @roleID = [IDRole], @roleName = [Name] FROM [SecurityRoles] WHERE [IDRole] = @user_sid
		SELECT @namespaceName =
			CASE @namespace_id
				WHEN '8732FCAC-A63C-46EA-9683-532B4C6644E7' THEN '{SmartWall}' -- Video wall OEM
				WHEN '988D493D-1597-4BE6-AFF0-396BFD5D047A' THEN  '{MatrixApplicationName}' -- Matrix OEM
				ELSE [display_name]
			END
		FROM [SecurityNamespace] 
		WHERE [namespace_id] = @namespace_id

		IF NOT EXISTS(SELECT [access_entry_id] FROM [SecurityAccessEntryTimeProfile] WHERE [access_entry_id] = @access_entry_id)
		BEGIN
			IF(@timeprofile_id IS NOT NULL OR @is_default = 1)
			BEGIN
				INSERT INTO [SecurityAccessEntryTimeProfile] ([access_entry_id], [timeprofile_id], [is_default])
				VALUES(@access_entry_id, @timeprofile_id, @is_default)
			END

			IF(@roleID IS NOT NULL)
			BEGIN
				IF(@is_default = 1)
				BEGIN
					-- Profile changed from Always to Default
					EXEC create_pending_log @session_id, 9019, @namespaceName, @action_display_name, @object_id, @object_name, @roleID, @roleName, 'Always', 'Default'
				END
				ELSE IF(@timeProfileName IS NOT NULL)
				BEGIN
					-- Profile changed from Always to timeprofile
					EXEC create_pending_log @session_id, 9018, @namespaceName, @action_display_name, @object_id, @object_name, @roleID, @roleName, 'Always', @timeProfileName
				END
			END
		END
		ELSE
		BEGIN
			DECLARE @currentIsDefault BIT
			DECLARE @currentTimeProfileId [GuidType]
			DECLARE @currentTimeProfileName [NameType]

			SELECT @currentIsDefault = [is_default], @currentTimeProfileId = [timeprofile_id]
			FROM [SecurityAccessEntryTimeProfile]
			WHERE [access_entry_id] = @access_entry_id

			IF(@currentTimeProfileId IS NOT NULL)
			BEGIN
				SELECT @currentTimeProfileName = [name] FROM [TimeProfile] WHERE [pkid] = @currentTimeProfileId
			END

			IF(@timeprofile_id IS NULL AND @is_default = 1)
			BEGIN
				UPDATE [SecurityAccessEntryTimeProfile]
				SET 
					[timeprofile_id] = NULL,
					[is_default] = @is_default
				WHERE [access_entry_id] = @access_entry_id
			END
			ELSE IF(@timeprofile_id IS NOT NULL)
			BEGIN
				UPDATE [SecurityAccessEntryTimeProfile]
				SET 
					[timeprofile_id] = @timeprofile_id, 
					[is_default] = @is_default
				WHERE [access_entry_id] = @access_entry_id
			END
			ELSE
			BEGIN
				DELETE FROM [SecurityAccessEntryTimeProfile]
				WHERE [access_entry_id] = @access_entry_id
			END

			IF(@currentIsDefault = 0 AND @currentTimeProfileId IS NULL)
			BEGIN
				-- Existing is Always
				IF(@is_default = 1)
				BEGIN
					-- Profile changed from Always to Default
					EXEC create_pending_log @session_id, 9019, @namespaceName, @action_display_name, @object_id, @object_name, @roleID, @roleName, 'Always', 'Default'
				END
				ELSE IF(@timeProfileName IS NOT NULL)
				BEGIN
					-- Profile changed from Always to timeprofile
					EXEC create_pending_log @session_id, 9018, @namespaceName, @action_display_name, @object_id, @object_name, @roleID, @roleName, 'Always', @timeProfileName
				END
			END
			ELSE IF(@currentIsDefault = 1)
			BEGIN
				-- Existing Default
				IF(@is_default = 0)
				BEGIN
					IF(@timeProfileName IS NOT NULL)
					BEGIN
							-- Profile changed from Default to timeprofile
						EXEC create_pending_log @session_id, 9018, @namespaceName, @action_display_name, @object_id, @object_name, @roleID, @roleName, 'Default', @timeProfileName
					END
					ELSE
					BEGIN
							-- Profile changed from Default to Always
						EXEC create_pending_log @session_id, 9019, @namespaceName, @action_display_name, @object_id, @object_name, @roleID, @roleName, 'Default', 'Always'
					END
				END
			END
			ELSE 
			BEGIN

				IF(@currentTimeProfileName IS NOT NULL)
				BEGIN
					-- Existing timeprofile
					IF(@is_default = 1)
					BEGIN
						-- Profile changed from timeprofile to default
						EXEC create_pending_log @session_id, 9017, @namespaceName, @action_display_name, @object_id, @object_name, @roleID, @roleName, @currentTimeProfileName, 'Default'
					END
					ELSE IF(@timeProfileName IS NOT NULL AND @timeprofile_id <> @currentTimeProfileId)
					BEGIN
						-- Profile change from timeprofile to timeprofile
						EXEC create_pending_log @session_id, 9016, @namespaceName, @action_display_name, @object_id, @object_name, @roleID, @roleName, @currentTimeProfileName, @timeProfileName
					END
					ELSE IF(@timeProfileName IS NULL)
					BEGIN
						-- Profile changed from timeprofile to Always
						EXEC create_pending_log @session_id, 9017, @namespaceName, @action_display_name, @object_id, @object_name, @roleID, @roleName, @currentTimeProfileName, 'Always'
					END
				END
				ELSE
				BEGIN
					-- Existing is Always since timeprofile doesn't exists 
					IF(@is_default = 1)
					BEGIN
						-- Profile changed from Always to Default
						EXEC create_pending_log @session_id, 9019, @namespaceName, @action_display_name, @object_id, @object_name, @roleID, @roleName, 'Always', 'Default'
					END
					ELSE IF(@timeProfileName IS NOT NULL)
					BEGIN
						-- Profile changed from Always to timeprofile
						EXEC create_pending_log @session_id, 9018, @namespaceName, @action_display_name, @object_id, @object_name, @roleID, @roleName, 'Always', @timeProfileName
					END
				END
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