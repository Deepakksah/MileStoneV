-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_delete_access_entry_transaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_delete_access_entry_transaction]
GO

CREATE PROCEDURE [security_delete_access_entry_transaction]
	@namespace_id UNIQUEIDENTIFIER,
	@object_id NVARCHAR(300),
	@action_name NVARCHAR(260),
	@user_sid [SidType],
	@access_deny BIT,
	@CurrentUtcTime DATETIME,
	@session_id [GuidType] = NULL,
	@object_name NVARCHAR(300) = NULL
AS
BEGIN
	SET NOCOUNT ON
 
	DECLARE @rowCount INT

	DECLARE @security_object_id UNIQUEIDENTIFIER
	
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	-- Make sure that a current transaction is active
	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END

	-- Verify that the object exists
	SELECT @security_object_id = [security_object_id]
	FROM [SecurityObject]
	WHERE [namespace_id] = @namespace_id AND [object_id] = @object_id

	IF(@security_object_id IS NULL)
	BEGIN
		SET @ErrorArgs = @procedureName + N': %%IDObject=' + @object_id + N'%%'
		;THROW 60021, @ErrorArgs, 1;
	END

	IF NOT EXISTS (
		SELECT [name]
		FROM [SecurityAction]
		WHERE [namespace_id] = @namespace_id AND [name] = @action_name
	)
	BEGIN
		SET @ErrorArgs = @procedureName + N': %%IDAction=' + @action_name + N'%%'
		;THROW 60013, @ErrorArgs, 1;
	END

	DECLARE @roleName NameType
	SELECT @roleName= [Name] FROM [SecurityRoles] WHERE [IDRole] = @user_sid

	IF(@object_name IS NULL OR @object_name = '')
	BEGIN
		SET @object_name = @object_id
	END

	-- delete access entries
	DELETE FROM [SecurityAccessEntryTimeProfile]
	WHERE [access_entry_id] IN
	(
		SELECT [access_entry_id]
		FROM [SecurityAccessEntry]
		WHERE [security_object_id] = @security_object_id
			AND [action_name] = @action_name
			AND [user_sid] = @user_sid
			AND [access_deny] = @access_deny
	)

	DELETE FROM [SecurityAccessEntry]
	WHERE [security_object_id] = @security_object_id
		AND [action_name] = @action_name
		AND [user_sid] = @user_sid
		AND [access_deny] = @access_deny

	SELECT @rowCount = @@ROWCOUNT

	-- Clear the permissions cache, it will be regenerated as needed
	DELETE FROM [SecurityAccessCache2]

	-- Audit log
	IF(@rowCount IS NOT NULL AND @rowCount > 0 AND @session_id IS NOT NULL AND @roleName IS NOT NULL)
	BEGIN
		DECLARE @namespaceDisplayName NVARCHAR(260)
		SELECT @namespaceDisplayName = [display_name] FROM [SecurityNamespace] WHERE [namespace_id] = @namespace_id

		DECLARE @actionDisplayName NVARCHAR(260)
		SELECT @actionDisplayName = [display_name] FROM [SecurityAction] WHERE [namespace_id] = @namespace_id AND [name] = @action_name

		IF(@actionDisplayName IS NULL OR @actionDisplayName = '')
		BEGIN
			SET @actionDisplayName = @action_name
		END

		IF(@object_id <> '/')
		BEGIN
			-- object
			IF(@access_deny = 0)
			BEGIN
				EXEC create_pending_log @session_id, 9015, @namespaceDisplayName, @actionDisplayName, @object_id, @object_name, @user_sid, @roleName 
			END
		END
		ELSE
		BEGIN
			-- Overall
			IF(@access_deny = 1)
			BEGIN
				EXEC create_pending_log @session_id, 9013, @namespaceDisplayName, @actionDisplayName, @object_id, @user_sid, @roleName 
			END
			ELSE
			BEGIN
				EXEC create_pending_log @session_id, 9011, @namespaceDisplayName, @actionDisplayName, @object_id, @user_sid, @roleName 
			END
		END
	END
END
GO