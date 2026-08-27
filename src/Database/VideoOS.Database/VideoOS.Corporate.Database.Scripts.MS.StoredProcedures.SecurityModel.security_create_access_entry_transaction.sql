-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_create_access_entry_transaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_create_access_entry_transaction]
GO

CREATE PROCEDURE [security_create_access_entry_transaction]
	@namespace_id UNIQUEIDENTIFIER,
	@object_id NVARCHAR(300),
	@object_name [NameType],
	@replace BIT,
	@CurrentUtcTime DATETIME,
	@session_id [GuidType] = NULL
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	-- Make sure that a current transaction is active
	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END

	DECLARE @security_object_id UNIQUEIDENTIFIER

	-- Verify that the object exists
	SELECT @security_object_id = [security_object_id]
	FROM [SecurityObject]
	WHERE [namespace_id] = @namespace_id AND [object_id] = @object_id

	IF(@security_object_id IS NULL)
	BEGIN
		SET @ErrorArgs = @procedureName + N': %%IDObject=' + @object_id + N'%%'
		;THROW 60021, @ErrorArgs, 1;
	END

	DECLARE @namespaceName NVARCHAR(260)
	SELECT @namespaceName =
		CASE @namespace_id
			WHEN '8732FCAC-A63C-46EA-9683-532B4C6644E7' THEN '{SmartWall}' -- Video wall OEM
			WHEN '988D493D-1597-4BE6-AFF0-396BFD5D047A' THEN  '{MatrixApplicationName}' -- Matrix OEM
			ELSE [display_name]
		END
	FROM [SecurityNamespace] 
	WHERE [namespace_id] = @namespace_id
	
	-- Verify that the Namespace/Action relations is valid
	IF EXISTS (
		SELECT temp.[action_name]
		FROM [TempSecurityAccessEntry] temp
		LEFT JOIN [SecurityAction] sa ON sa.[namespace_id] = @namespace_id AND sa.[name] = temp.[action_name]
		WHERE temp.[SPID] = @@SPID AND sa.[name] IS NULL
	)
	BEGIN
		;THROW 60022, @procedureName, 1;
	END

	-- Now split up the access entries into what we want to insert, update and delete

	-- Mark all access entries as no change
	UPDATE [TempSecurityAccessEntry]
	SET [change] = 0
	WHERE [SPID] = @@SPID

	IF(@replace = 1)
	BEGIN
		INSERT [TempSecurityAccessEntry] (
			[SPID],
			[user_sid],
			[action_name],
			[access_deny],
			[change]
		)
		SELECT
			@@SPID,
			sae.[user_sid],
			sae.[action_name],
			sae.[access_deny],
			3
		FROM [SecurityAccessEntry] sae
		LEFT JOIN [TempSecurityAccessEntry] temp ON
			sae.[user_sid] = temp.[user_sid]
			AND sae.[action_name] = temp.[action_name]
			AND sae.[access_deny] = temp.[access_deny]
		WHERE sae.[security_object_id] = @security_object_id
			AND temp.[SPID] IS NULL
	END

	-- Mark the access entries that have to be inserted
	UPDATE [TempSecurityAccessEntry]
	SET [change] = 1
	FROM [TempSecurityAccessEntry] temp
	LEFT JOIN [SecurityAccessEntry] sae ON
				sae.[security_object_id] = @security_object_id
		AND temp.[action_name] = sae.[action_name]
		AND temp.[user_sid] = sae.[user_sid]
		AND temp.[access_deny] = sae.[access_deny]
	WHERE temp.[SPID] = @@SPID
	AND sae.[user_sid] IS NULL 
	AND sae.[action_name] IS NULL
	AND sae.[access_deny] IS NULL

	-- If there is no changes, let's return
	IF NOT EXISTS (SELECT [change] FROM [TempSecurityAccessEntry] WHERE [change] != 0)
	BEGIN
		RETURN 0
	END

	-- Delete all rows with legacy_security_actions from temp-table
	-- only do this if security is on an object.
	IF(@object_id <> '/')
	BEGIN
		DELETE [TempSecurityAccessEntry]
		FROM [TempSecurityAccessEntry] tsae
		LEFT JOIN SecurityAction sa ON sa.name = tsae.action_name
		WHERE sa.[legacy_security_name] IS NOT NULL
			AND sa.[namespace_id] = @namespace_id
			AND tsae.change = 1
	END
		
	IF(@object_name IS NULL OR @object_name = '')
	BEGIN
		SET @object_name = @object_id
	END

	-- Insert new access entries
	INSERT [SecurityAccessEntry] (
		[security_object_id],
		[action_name],
		[user_sid],
		[access_deny]
	)
	SELECT
		@security_object_id,
		temp.[action_name],
		temp.[user_sid],
		temp.[access_deny]
	FROM [TempSecurityAccessEntry] temp
	WHERE temp.[SPID] = @@SPID
		AND temp.[change] = 1

	-- Audit log added permissions

	DECLARE @action_name NVARCHAR(260)
	DECLARE @action_id NVARCHAR(260)
	DECLARE @access_deny BIT
	DECLARE @roleName NameType
	DECLARE @roleID GuidType
	
	IF(@session_id IS NOT NULL)
	BEGIN
		DECLARE cursorInsert CURSOR FAST_FORWARD
		FOR
			SELECT
				sr.[IDRole],
				sr.[name],
				sa.[display_name],
				temp.[access_deny]
			FROM [TempSecurityAccessEntry] temp
			INNER JOIN SecurityRoles sr ON [user_sid] = IDRole
			INNER JOIN SecurityAction sa ON sa.[name] = temp.[action_name] AND sa.[namespace_id] = @namespace_id
			WHERE temp.[SPID] = @@SPID
				AND temp.[change] = 1
				
		OPEN cursorInsert
		BEGIN TRY
			WHILE(1 = 1)
			BEGIN
				FETCH cursorInsert
				INTO @roleID, @roleName, @action_name, @access_deny

				IF(@@FETCH_STATUS <> 0)
				BEGIN
					BREAK
				END

				IF(@object_id <> '/')
				BEGIN
					-- object
					IF(@access_deny = 0)
					BEGIN
						EXEC create_pending_log @session_id, 9014, @namespaceName, @action_name, @object_id, @object_name, @roleID, @roleName 
					END
				END
				ELSE
				BEGIN
					-- Overall
					IF(@access_deny = 1)
					BEGIN
						EXEC create_pending_log @session_id, 9012, @namespaceName, @action_name, @object_id, @roleID, @roleName 
					END
					ELSE
					BEGIN
						EXEC create_pending_log @session_id, 9010, @namespaceName, @action_name, @object_id, @roleID, @roleName 
					END
				END
			END
		END TRY
		BEGIN CATCH
			CLOSE cursorInsert
			DEALLOCATE cursorInsert
			;THROW
		END CATCH

		CLOSE cursorInsert
		DEALLOCATE cursorInsert
	END
   
	-- Delete the access entries that is previously marked for deletion (replace was true)
	DECLARE @SID SidType
	DECLARE cursorDelete CURSOR FAST_FORWARD
	FOR
		SELECT [user_sid], [action_name], [access_deny]
		FROM [TempSecurityAccessEntry]
		WHERE [SPID] = @@SPID AND [change] = 3

	OPEN cursorDelete
	BEGIN TRY
		WHILE(1 = 1)
		BEGIN
			FETCH cursorDelete
			INTO @SID, @action_id, @access_deny

			IF(@@FETCH_STATUS <> 0)
			BEGIN
				BREAK
			END

			EXEC [security_delete_access_entry_transaction]
				@namespace_id,
				@object_id,
				@action_id,
				@SID,
				@access_deny,
				@CurrentUtcTime,
				@session_id,
				@object_name

		END
	END TRY
	BEGIN CATCH
		CLOSE cursorDelete
		DEALLOCATE cursorDelete
		;THROW
	END CATCH

	CLOSE cursorDelete
	DEALLOCATE cursorDelete

	-- Now cleanup the temp table
	DELETE [TempSecurityAccessEntry]
	WHERE [SPID] = @@SPID

	-- Clear the permissions cache, it will be regenerated as needed
	DELETE FROM [SecurityAccessCache2]
END
GO