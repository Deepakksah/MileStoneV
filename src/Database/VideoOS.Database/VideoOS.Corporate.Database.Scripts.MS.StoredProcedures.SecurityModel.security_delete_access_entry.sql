-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_delete_access_entry]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_delete_access_entry]
GO

CREATE PROCEDURE [security_delete_access_entry]
	@namespace_id UNIQUEIDENTIFIER,
	@object_id NVARCHAR(300),
	@object_name NameType,
	@entries XML,
	@session_Id GuidType
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @CurrentUtcTime DATETIME

		DECLARE @dataSetBefore XML
		DECLARE @dataSetAfter XML

		DECLARE @dataSetDevicePermissions XML

		DECLARE @before TABLE ([object_id] NVARCHAR(300), namespace_id UNIQUEIDENTIFIER, action_name NVARCHAR(260), [user_sid] [SIDType], access_deny BIT)
		DECLARE @after TABLE ([object_id] NVARCHAR(300), namespace_id UNIQUEIDENTIFIER, action_name NVARCHAR(260), [user_sid] [SIDType], access_deny BIT)
		DECLARE @users TABLE ([user_sid] [SIDType])
		DECLARE @legacyDevices TABLE ([roleId] [GuidType], roleName [NameType], action_display_name NVARCHAR(260), action_legacy_name NVARCHAR(100))

		BEGIN TRAN

		SET @CurrentUtcTime = GETUTCDATE()

		-- Lock tables for update
		EXEC [security_table_lock] @LockLevel = 0
		
		-- Ensure temporary table are clear
		DELETE [TempSecurityAccessEntry]
		WHERE [SPID] = @@SPID

		INSERT [TempSecurityAccessEntry]
		SELECT
			[SPID] = @@SPID,
			[user_sid] = v.value('@Sid', '[SIDType]'),
			[action_name] = v.value('@ActionId', 'NVARCHAR(260)'),
			[access_deny] = v.value('@AccessDeny', 'BIT'),
			[change] = NULL
		FROM @entries.nodes('AccessControlEntries/AccessControlEntry') AS T(v)

		INSERT @users
		SELECT [user_sid]
		FROM [TempSecurityAccessEntry]
		WHERE [SPID] = @@SPID

		-- Mark all access entries as delete
		UPDATE [TempSecurityAccessEntry]
		SET [Change] = 3
		WHERE SPID = @@SPID

		DECLARE @user_sid [SidType]
		DECLARE @action_name NVARCHAR(260)
		DECLARE @access_deny BIT

		-- Delete the access entries that is marked for deletion
		INSERT @before
			SELECT [object_id], [namespace_id], [action_name], [user_sid], [access_deny] FROM [SecurityGroupAccessEntry]
			WHERE [namespace_id] = @namespace_id 
			AND [user_sid] IN (SELECT [user_sid] FROM @users)

		DECLARE cursorDelete CURSOR FAST_FORWARD
		FOR
			SELECT [user_sid], [action_name], [access_deny]
			FROM [TempSecurityAccessEntry]
			WHERE [SPID] = @@SPID
				AND [change] = 3

		OPEN cursorDelete
		BEGIN TRY
			WHILE(1 = 1)
			BEGIN
				FETCH cursorDelete
				INTO @user_sid, @action_name, @access_deny

				IF(@@FETCH_STATUS <> 0)
				BEGIN
					BREAK
				END

				EXEC [security_delete_access_entry_transaction]
					@namespace_id,
					@object_id,
					@action_name,
					@user_sid,
					@access_deny,
					@CurrentUtcTime,
					@session_Id,
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

		INSERT @after
		SELECT [object_id], [namespace_id], [action_name], [user_sid], [access_deny] FROM [SecurityGroupAccessEntry]
		WHERE [namespace_id] = @namespace_id
			AND [user_sid] IN (SELECT [user_sid] FROM @users)


		-- Create dataset for added rows
		;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
		SELECT @dataSetAfter = (
			SELECT * FROM @after a 
			WHERE NOT EXISTS (SELECT * FROM @before b WHERE a.[object_id] = b.[object_id] AND a.namespace_id = b.namespace_id AND a.action_name = b.action_name AND a.[user_sid] = b.[user_sid] AND a.[access_deny] = b.[access_deny])
			FOR XML PATH ('SecurityGroupAccessEntry'), ROOT('SecurityDataSet'), TYPE
		)
		
		IF @dataSetAfter IS NOT NULL
		BEGIN
			EXEC [create_task_transaction]
				'diffgram_security_insert',
				'909749AE-88C4-4780-A381-383D691E98FA',
				@dataSetAfter,
				NULL,
				@CurrentUtcTime
		END

		-- Create dataset for deleted rows
		;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
		SELECT @dataSetBefore = (
			SELECT * FROM @before b 
			WHERE NOT EXISTS (SELECT * FROM @after a WHERE a.[object_id] = b.[object_id] AND a.namespace_id = b.namespace_id AND a.action_name = b.action_name AND a.[user_sid] = b.[user_sid] AND a.[access_deny] = b.[access_deny])
			FOR XML PATH ('SecurityGroupAccessEntry'), ROOT('SecurityDataSet'), TYPE
		)
		IF @dataSetBefore IS NOT NULL
		BEGIN
			EXEC [create_task_transaction]
				'diffgram_security_delete',
				'909749AE-88C4-4780-A381-383D691E98FA',
				@dataSetBefore,
				NULL,
				@CurrentUtcTime
		END

		-- Repopulate TempSecurityAccessEntry
		-- This is needed because the temp-table is cleared inside 'security_delete_access_entry_transaction'
		DELETE [TempSecurityAccessEntry]
		WHERE [SPID] = @@SPID

		INSERT [TempSecurityAccessEntry]
		SELECT
			[SPID] = @@SPID,
			[user_sid] = v.value('@Sid', '[SIDType]'),
			[action_name] = v.value('@ActionId', 'NVARCHAR(260)'),
			[access_deny] = v.value('@AccessDeny', 'BIT'),
			[change] = NULL
		FROM @entries.nodes('AccessControlEntries/AccessControlEntry') AS T(v)

		-- Create dataset for deleted legacy security rows
		;WITH XMLNAMESPACES ( DEFAULT '{XMLNS}')
		SELECT @dataSetDevicePermissions = (
			SELECT spd.[LastModified], spd.[SecurityAttributeID], spd.[IDRole], spd.[IDDevice], spd.[Value], spd.[IDTimeProfile], spd.[TimeBackward], spd.[IsDefaultTimeProfile]
			FROM [SecurityPermissionsDevice] AS spd
			INNER JOIN [SecurityAction] sa ON spd.[SecurityAttributeId] = sa.[legacy_security_name]
			INNER JOIN [TempSecurityAccessEntry] tsae ON tsae.[action_name] = sa.[name]
			WHERE spd.[IDRole] = tsae.[user_sid]
				AND spd.[SecurityAttributeId] = sa.[legacy_security_name]
				AND spd.[IDDevice] = @object_id
				AND sa.[namespace_id] = @namespace_id
				AND tsae.[SPID] = @@SPID
			FOR XML PATH ('SecurityPermissionsDevice'), ROOT('SecurityDataSet'), TYPE
		)
		IF(@dataSetDevicePermissions IS NOT NULL)
		BEGIN
			EXEC [create_task_transaction]
				@type = 'diffgram_security_delete',
				@target = '909749AE-88C4-4780-A381-383D691E98FA',
				@data_a = @dataSetDevicePermissions,
				@current_time = @CurrentUtcTime
		END

		-- delete devices in legacy table
		INSERT @legacyDevices
		SELECT sr.[IDRole], sr.[Name], sa.[display_name], sa.[legacy_security_name]
		FROM [SecurityPermissionsDevice] AS spd
		INNER JOIN [SecurityAction] sa ON spd.[SecurityAttributeId] = sa.[legacy_security_name]
		INNER JOIN [TempSecurityAccessEntry] tsae ON tsae.[action_name] = sa.[name]
		LEFT JOIN [SecurityRoles] sr ON sr.IDRole = tsae.[user_sid]
		WHERE spd.[IDRole] = tsae.[user_sid]
			AND spd.[SecurityAttributeId] = sa.[legacy_security_name]
			AND spd.[IDDevice] = @object_id
			AND sa.[namespace_id] = @namespace_id
			AND tsae.[SPID] = @@SPID

		DELETE [SecurityPermissionsDevice]
		FROM [SecurityPermissionsDevice] AS spd
		INNER JOIN [SecurityAction] sa ON spd.[SecurityAttributeId] = sa.[legacy_security_name]
		INNER JOIN [TempSecurityAccessEntry] tsae ON tsae.[action_name] = sa.[name]
		WHERE spd.[IDRole] = tsae.[user_sid]
			AND spd.[SecurityAttributeId] = sa.[legacy_security_name]
			AND spd.[IDDevice] = @object_id
			AND sa.[namespace_id] = @namespace_id
			AND tsae.[SPID] = @@SPID


		-- Audit log 
		DECLARE @action_display_name NVARCHAR(260)
		DECLARE @roleName NameType
		DECLARE @roleID GuidType
		DECLARE @namespaceName NVARCHAR(260)

		SELECT @namespaceName =
			CASE @namespace_id
				WHEN '8732FCAC-A63C-46EA-9683-532B4C6644E7' THEN '{SmartWall}' -- Video wall OEM
				WHEN '988D493D-1597-4BE6-AFF0-396BFD5D047A' THEN  '{MatrixApplicationName}' -- Matrix OEM
				ELSE [display_name]
			END
		FROM [SecurityNamespace] 
		WHERE [namespace_id] = @namespace_id

		DECLARE cursorInsert CURSOR FAST_FORWARD
		FOR
			SELECT roleId, roleName, action_display_name
			FROM @legacyDevices
				
		OPEN cursorInsert
		BEGIN TRY
			WHILE(1 = 1)
			BEGIN
				FETCH cursorInsert
				INTO @roleID, @roleName, @action_display_name

				IF(@@FETCH_STATUS <> 0)
				BEGIN
					BREAK
				END

				EXEC create_pending_log @session_id, 9015, @namespaceName, @action_display_name, @object_id, @object_name, @roleID, @roleName 
			END
		END TRY
		BEGIN CATCH
			CLOSE cursorInsert
			DEALLOCATE cursorInsert
			;THROW
		END CATCH

		CLOSE cursorInsert
		DEALLOCATE cursorInsert

		-- Now cleanup the temp table
		DELETE [TempSecurityAccessEntry]
		WHERE [SPID] = @@SPID

		-- Clear the permissions cache, it will be regenerated as needed
		DELETE FROM [SecurityAccessCache2]

		-- TODO: We need to handle if security changed
		SELECT CAST(1 AS BIT) AS security_changed

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