-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[FailoverServers]') AND name = N'ServiceState')
BEGIN
 -- ServiceState might have been removed - will temporary be added and 
 -- removed again in later script
	ALTER TABLE [FailoverServers] ADD
	[ServiceState] INT NOT NULL DEFAULT 1
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_unregister_object_transaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_unregister_object_transaction]
GO

CREATE PROCEDURE [security_unregister_object_transaction]
	@namespace_name NVARCHAR(260),
	@object_id NVARCHAR(300),
	@CurrentUtcTime DATETIME
AS
BEGIN
  SET NOCOUNT ON
 
  DECLARE @status INT
  DECLARE @rowCount INT

	DECLARE @security_object_id UNIQUEIDENTIFIER

  -- Initialize the ProcedureName for error messages.
  DECLARE @procedureName SYSNAME
  SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	-- Make sure that a current transaction is active
	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END

	DECLARE @namespace_id UNIQUEIDENTIFIER
	SELECT @namespace_id = [namespace_id] FROM [SecurityNamespace] WHERE [name] = @namespace_name

	SELECT @security_object_id = [security_object_id]
	FROM [SecurityObject]
	WHERE	[namespace_id] = @namespace_id
		AND [object_id] = @object_id

	-- Verify that the object exists
	IF(@security_object_id IS NULL)
	BEGIN
		RETURN 0
	END
	
	DECLARE @currentLevel INT
	SET @currentLevel = 0

	DECLARE @objectList AS TABLE (
		[security_object_id] UNIQUEIDENTIFIER,
		[level] INT
	)

	INSERT @objectList ([security_object_id], [level])
	VALUES (@security_object_id, @currentLevel)

	SELECT @status = @@ERROR, @rowcount = @@ROWCOUNT

	IF(@status <> 0)
	BEGIN
		;THROW 60000, @procedureName, 1;
	END

	-- Delete all access control entries on the object
	DELETE aces
	FROM [SecurityAccessEntry] aces
	INNER JOIN @objectList objCache ON aces.[security_object_id] = objCache.[security_object_id]
	
	SET @status = @@ERROR

	IF(@status <> 0)
	BEGIN
		;THROW 60000, @procedureName, 1;
	END

	-- Delete the security object and its children
	DELETE so
	FROM [SecurityObject] so
	INNER JOIN @objectList objCache ON so.[security_object_id] = objCache.[security_object_id]

	SET @status = @@ERROR

	IF(@status <> 0)
	BEGIN
		;THROW 60000, @procedureName, 1;
	END

	-- Now that the security permissions are changed, we need to clear the permission cache
	-- This will be populated again when needed.
	DELETE FROM [SecurityAccessCache2]

	SET @status = @@ERROR

	IF(@status <> 0)
	BEGIN
		;THROW 60000, @procedureName, 1;
	END
	
	RETURN 0
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[remove_server_group_member_transaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [remove_server_group_member_transaction]
GO

CREATE PROCEDURE [remove_server_group_member_transaction]
	@IDObject [GuidType]
AS
BEGIN

	SET NOCOUNT ON

	DECLARE @status INT

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	-- Make sure that a current transaction is active
	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END

	DELETE [ServerGroupMembership]
	WHERE [IDObject] = @IDObject

	SET @status = @@ERROR

	IF(@status <> 0)
	BEGIN
		;THROW 60000, @procedureName, 1;
	END
	
	RETURN 0
END
GO

-- Adds stored procedure to delete failover servers.
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_failover_server_transaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_failover_server_transaction]
GO

CREATE PROCEDURE [delete_failover_server_transaction]
	@IDFailoverServer [GuidType]
AS
BEGIN
	DECLARE @status INT
	DECLARE @deleted_service_service_id [GuidType]
	DECLARE @deleted_service_recorder_id [GuidType]
	DECLARE @deleted_service_active BIT

	DECLARE @taskTarget [GuidType]
	DECLARE @dataSetBefore XML

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	-- Make sure that a current transaction is active
	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END

	DECLARE @CurrentUtcTime DATETIME
	SET @CurrentUtcTime = GETUTCDATE()
	
	-- Remove from ServerGroup
	EXEC remove_server_group_member_transaction @IDFailoverServer

	-- Get recording server service id.
	-- Also lock the failover server row to handle multi user issues
	SELECT	@deleted_service_service_id = [IDRecordingServerService]
	FROM	[FailoverServers] f WITH(UPDLOCK)
	WHERE	[IDFailoverServer] = @IDFailoverServer


	-- Get the recorder id and active state (if present) for the failover recorder service
	SELECT	@deleted_service_recorder_id = [recorder_id],
			@deleted_service_active = [active]
	FROM	[RecordingServerServiceRecorder]
	WHERE	[recording_server_service_id] = @deleted_service_service_id


	-- Lock on all RecordingServerServiceRecorder rows for the recorder id
	IF (@deleted_service_recorder_id IS NOT NULL)
	BEGIN
		-- Just assigning to @status to avoid getting a result set
		SELECT @status = COUNT([recorder_id])
		FROM [RecordingServerServiceRecorder] WITH(UPDLOCK)
		WHERE [recorder_id] = @deleted_service_recorder_id
	END


	-- Lock the RecordingServerService row (if it exists)
	IF (@deleted_service_service_id IS NOT NULL)
	BEGIN
		SELECT @deleted_service_service_id = [pkid]
		FROM [RecordingServerService] WITH(UPDLOCK)
		WHERE [pkid] = @deleted_service_service_id
	END

	-- Delete the RecordingServerService row for this failover server and a possible relation in the RecordingServerServiceRecorder
	IF (@deleted_service_service_id IS NOT NULL)
	BEGIN
		-- The failover did have a service associated with it's recording server

		IF (@deleted_service_recorder_id IS NOT NULL)
		BEGIN
			DELETE [RecordingServerServiceRecorder] WHERE [recording_server_service_id] = @deleted_service_service_id
			SET @status = @@Error
			IF(@status <> 0)
			BEGIN
				;THROW 60000, @procedureName, 1;
			END
		END
	
		-- Delete RecordingServerServiceConfigurable row
		DELETE [RecordingServerServiceConfigurable] WHERE [pkid] = @deleted_service_service_id
		SET @status = @@Error
		IF(@status <> 0)
		BEGIN
			;THROW 60000, @procedureName, 1;
		END

		-- Delete RecordingServerService row
		DELETE [RecordingServerService] WHERE [pkid] = @deleted_service_service_id
		SET @status = @@Error
		IF(@status <> 0)
		BEGIN
			;THROW 60000, @procedureName, 1;
		END

		-- Make sure that the main recording server service is set to active (in case we just deleted the active service)
		IF (@deleted_service_active = 1) -- if @deleted_service_active is null, this if will also be false
		BEGIN
			UPDATE [RecordingServerServiceRecorder]
			SET [active] = 1,
				[last_modified] = @CurrentUtcTime
			WHERE [recorder_id] = @deleted_service_recorder_id AND [type] = 0

			SET @status = @@Error
			IF(@status <> 0)
			BEGIN
				;THROW 60000, @procedureName, 1;
			END
		END
	END

	-- Delete the failover server	
	DELETE [FailoverServers]
	WHERE [IDFailoverServer] = @IDFailoverServer

	SET @status = @@Error
	IF(@status <> 0)
	BEGIN
		;THROW 60000, @procedureName, 1;
	END

	-- Unregister the security object
	EXEC @status = [security_unregister_object_transaction]
										@namespace_name = 'FailoverServer',
										@object_id = @IDFailoverServer,
										@CurrentUtcTime = @CurrentUtcTime
	IF(@status <> 0)
	BEGIN
		RETURN @status
	END

	RETURN 0
end
GO