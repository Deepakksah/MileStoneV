---- This header is used by the dbupdate tool.
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
	@IDObject [GuidType],
	@IDAction VARCHAR(128),
	@SID [SidType],
	@AccessDeny BIT,
	@CurrentUtcTime DATETIME
AS
BEGIN
  SET NOCOUNT ON
 
  DECLARE @status INT
	DECLARE @rowCount INT

	DECLARE @IDSecurityObject CHAR(36)
	DECLARE @IDClass VARCHAR(128)

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
	SELECT @IDSecurityObject = [IDSecurityObject],
				 @IDClass = [IDClass]
	FROM [SecurityObjects]
	WHERE [IDObject] = @IDObject

	IF(@IDSecurityObject IS NULL)
	BEGIN
		SET @ErrorArgs = @procedureName + N': %%IDObject=' + @IDSecurityObject + N'%%'
		;THROW 60021, @ErrorArgs, 1;
	END

	IF NOT EXISTS (
		SELECT [IDAction]
		FROM [SecurityActions]
		WHERE [IDClass] = @IDClass
					AND [IDAction] = @IDAction
	)
	BEGIN
		SET @ErrorArgs = @procedureName + N': %%IDAction=' + @IDAction + N'%%'
		;THROW 60013, @ErrorArgs, 1;
	END

	-- Mark the access entry as deleted
	UPDATE [SecurityAccessEntries]
	SET [Deleted] = 1
	WHERE [IDSecurityObject] = @IDSecurityObject
				AND [IDAction] = @IDAction
				AND [SID] = @SID
				AND [AccessDeny] = @AccessDeny
				AND [Deleted] = 0


	SELECT @status = @@ERROR, @rowCount = @@ROWCOUNT

	IF(@status <> 0)
	BEGIN
		;THROW 60000, @procedureName, 1;
	END

--	IF(@rowCount = 0)
--	BEGIN
--		DECLARE @userName NVARCHAR(512)
--
--		SELECT @userName = [DisplayName]
--		FROM [SecurityIdentity]
--		WHERE [SID] = @SID
--
--		IF (@userName IS NULL)
--		BEGIN
--			SET @userNAme = @SID
--		END
--
--		SET @ErrorArgs = @procedureName + N': %%IDObject=' + CAST(@IDObject AS NVARCHAR(36)) + N'%%; %%IDAction=' + @IDAction + N'%%; %%IDUser=' + @userName + N'%%'
--		;THROW 60024, @ErrorArgs, 1;
--	END

	-- Clear the permissions cache, it will be regenerated as needed
	DELETE FROM [SecurityAccessCache]

	SET @status = @@ERROR

	IF(@status <> 0)
	BEGIN
		;THROW 60000, @procedureName, 1;
	END

	RETURN 0
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_create_access_entry_transaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_create_access_entry_transaction]
GO

CREATE PROCEDURE [security_create_access_entry_transaction]
	@IDObject [GuidType],
	@Replace BIT,
	@CurrentUtcTime DATETIME
AS
BEGIN
  SET NOCOUNT ON
 
  DECLARE @status INT

	DECLARE @IDSecurityObject CHAR(36)
	DECLARE @IDClass VARCHAR(128)
	DECLARE @IDO CHAR(36)

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
	SELECT @IDSecurityObject = [IDSecurityObject],
				 @IDClass = [IDClass]
	FROM [SecurityObjects]
	WHERE [IDObject] = @IDObject

	IF(@IDSecurityObject IS NULL)
	BEGIN
		SET @ErrorArgs = @procedureName + N': %%IDObject=' + @IDSecurityObject + N'%%'
		;THROW 60021, @ErrorArgs, 1;
	END

	-- Verify that the IDClass/IDAction relations is valid
	IF EXISTS (
		SELECT  temp.[IDAction]
		FROM [TempSecurityAccessEntries] temp
		LEFT JOIN [SecurityActions] sa ON
			sa.[IDClass] = @IDClass 
			AND sa.[IDAction] = temp.[IDAction]
		WHERE temp.[SPID] = @@SPID 
					AND sa.[IDAction] IS NULL
	)
	BEGIN
		;THROW 60022, @procedureName, 1;
	END

	-- Now split up the access entries into what we want to insert, update and delete

	-- Mark all access entries as no change
	UPDATE [TempSecurityAccessEntries]
	SET [Change] = 0
	WHERE SPID = @@SPID

	SET @status = @@ERROR

	IF(@status <> 0)
	BEGIN
		;THROW 60000, @procedureName, 1;
	END

	-- Mark the access entries that have to be inserted
	UPDATE [TempSecurityAccessEntries]
	SET [Change] = 1
	FROM [TempSecurityAccessEntries] temp
	LEFT JOIN [SecurityAccessEntries] sae ON
		sae.[IDSecurityObject] = @IDSecurityObject
		AND temp.[IDAction] = sae.[IDAction]
		AND temp.[SID] = sae.[SID]
		AND temp.[AccessDeny] = sae.[AccessDeny]
	WHERE temp.[SPID] = @@SPID
				AND sae.[Deleted] IS NULL

	SET @status = @@ERROR

	IF (@status <> 0)
	BEGIN
		;THROW 60000, @procedureName, 1;
	END

	-- Mark the access entries that have to be undeleted
	UPDATE [TempSecurityAccessEntries]
	SET [Change] = 2
	FROM [TempSecurityAccessEntries] temp
	LEFT JOIN [SecurityAccessEntries] sae ON
		sae.[IDSecurityObject] = @IDSecurityObject
		AND temp.[IDAction] = sae.[IDAction]
		AND temp.[SID] = sae.[SID]
		AND temp.[AccessDeny] = sae.[AccessDeny]
	WHERE temp.[SPID] = @@SPID
				AND sae.[Deleted] = 1

	SET @status = @@ERROR

	IF(@status <> 0)
	BEGIN
		;THROW 60000, @procedureName, 1;
	END

	IF(@Replace = 1)
	BEGIN
		INSERT [TempSecurityAccessEntries] (
			[SPID],
			[SID],
			[IDAction],
			[AccessDeny],
			[Change]
		)
		SELECT
			@@SPID,
			sae.[SID],
			sae.[IDAction],
			sae.[AccessDeny],
			3
		FROM [SecurityAccessEntries] sae
		LEFT JOIN [TempSecurityAccessEntries] temp ON
			sae.[SID] = temp.[SID]
			AND sae.[IDAction] = temp.[IDAction]
			AND sae.[AccessDeny] = temp.[AccessDeny]
		WHERE sae.[IDSecurityObject] = @IDSecurityObject
					AND sae.[Deleted] = 0
					AND temp.[SPID] IS NULL

		SET @status = @@ERROR

		IF(@status <> 0)
		BEGIN
			;THROW 60000, @procedureName, 1;
		END
	END

	-- If there is no changes, let's return
	IF NOT EXISTS (
		SELECT [Change]
		FROM [TempSecurityAccessEntries]
		WHERE [Change] != 0
	)
	BEGIN
		RETURN 0
	END

	-- Insert new access entries
	INSERT [SecurityAccessEntries] (
		[IDSecurityObject],
		[IDAction],
		[SID],
		[AccessDeny],
		[Deleted]
	)
	SELECT
		@IDSecurityObject,
		temp.[IDAction],
		temp.[SID],
		temp.[AccessDeny],
		0
	FROM [TempSecurityAccessEntries] temp
	WHERE temp.[SPID] = @@SPID
				AND temp.[Change] = 1

	SET @status = @@ERROR

	IF(@status <> 0)
	BEGIN
		;THROW 60000, @procedureName, 1;
	END

	-- Undelete any previously existing access entries
	UPDATE [SecurityAccessEntries]
	SET [Deleted] = 0
	FROM [SecurityAccessEntries] sae
	INNER JOIN [TempSecurityAccessEntries] temp ON
		sae.[IDSecurityObject] = @IDSecurityObject
		AND temp.[IDAction] = sae.[IDAction]
		AND temp.[SID] = sae.[SID]
		AND temp.[AccessDeny] = sae.[AccessDeny]
	WHERE temp.[SPID] = @@SPID
				AND temp.[Change] = 2

	SET @status = @@ERROR

	IF(@status <> 0)
	BEGIN
		;THROW 60000, @procedureName, 1;
	END

	DECLARE @SID [SidType]
	DECLARE @IDAction VARCHAR(128)
	DECLARE @AccessDeny BIT

	-- Delete the access entries that is previously marked for deletion (replace was true)
	DECLARE cursorDelete CURSOR FAST_FORWARD
	FOR
		SELECT
			[SID],
			[IDAction],
			[AccessDeny]
		FROM [TempSecurityAccessEntries]
		WHERE [SPID] = @@SPID
			AND [Change] = 3

	OPEN cursorDelete

	WHILE(1 = 1)
	BEGIN
		FETCH cursorDelete
		INTO @SID, @IDAction, @AccessDeny

		IF(@@FETCH_STATUS <> 0)
		BEGIN
			BREAK
		END

		EXEC @status = [security_delete_access_entry_transaction]
			@IDObject = @IDObject,
			@IDAction = @IDAction,
			@SID = @SID,
			@AccessDeny = @AccessDeny,
			@CurrentUtcTime = @CurrentUtcTime

		IF(@status <> 0)
		BEGIN
			-- we don't need to raise an error, security_delete_access_entry_transaction does it for us
			BREAK
		END
	END

	CLOSE cursorDelete
	DEALLOCATE cursorDelete

	IF(@status <> 0)
	BEGIN
		RETURN @status
	END

	-- Now cleanup the temp table
	DELETE [TempSecurityAccessEntries]
	WHERE [SPID] = @@SPID

	SET @status = @@ERROR

	IF(@status <> 0)
	BEGIN
		;THROW 60000, @procedureName, 1;
	END

	-- Clear the permissions cache, it will be regenerated as needed
	DELETE FROM [SecurityAccessCache]

	SET @status = @@ERROR

	IF(@status <> 0)
	BEGIN
		;THROW 60000, @procedureName, 1;
	END

	RETURN 0
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_register_object_transaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_register_object_transaction]
GO

CREATE PROCEDURE [security_register_object_transaction]
	@SID [SidType],
	@IDObject [GuidType],
	@IDParentObject [GuidType],
	@IDClass VARCHAR(128),
	@CurrentUtcTime DATETIME
 AS
BEGIN
  SET NOCOUNT ON
 
  DECLARE @status INT

	DECLARE @IDParent CHAR(36)
	DECLARE @IDParentClass VARCHAR(128)

  -- Initialize the ProcedureName for error messages.
  DECLARE @procedureName SYSNAME
  SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
  DECLARE @ErrorArgs NVARCHAR(500)

	-- Make sure that a current transaction is active
	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END

	-- Verify that the object doesn't already exists
	IF EXISTS (
		SELECT [IDObject]
		FROM [SecurityObjects]
		WHERE [IDObject] = @IDObject
	)
	BEGIN
		SET @ErrorArgs = @procedureName + N': %%IDObject=' + CAST(@IDObject AS NVARCHAR(36)) + N'%%'
		;THROW 60016, @ErrorArgs, 1;
	END

	-- Verify that the object class exists
	IF NOT EXISTS (
		SELECT [IDClass]
		FROM [SecurityActions]
		WHERE [IDClass] = @IDClass
	)
	BEGIN
		SET @ErrorArgs = @procedureName + N': %%IDClass=' + @IDClass + N'%%'
		;THROW 60017, @ErrorArgs, 1;
	END

	-- Get the parent object id if necessary
	IF(@IDParentObject IS NOT NULL)
	BEGIN
		SELECT @IDParent = [IDSecurityObject],
					 @IDParentClass = [IDClass]
		FROM [SecurityObjects]
		WHERE @IDParentObject = [IDObject]

		IF(@IDParent IS NULL)
		BEGIN
			SET @ErrorArgs = @procedureName + N': %%IDParentObject=' + CAST(@IDParentObject AS NVARCHAR(36)) + N'%%'
			;THROW 60018, @ErrorArgs, 1;
		END

		--IF(@IDClass != @IDParentClass)
		--BEGIN
		--	SET @ErrorArgs = @procedureName + N': %%IDParentObject=' + CAST(@IDParentObject AS NVARCHAR(36)) + N'%%; %%IDObject=' + CAST(@IDObject AS NVARCHAR(36)) + N'%%'
		--	;THROW 60019, @ErrorArgs, 1;
		--END
	END

	DECLARE @IDSecurityObject CHAR(36)
	SET @IDSecurityObject = NEWID()

	-- Now create the security object
	INSERT [SecurityObjects] (
		[IDSecurityObject],
		[IDObject],
		[IDClass],
		[IDInheritsFrom]
	)
	VALUES (
		@IDSecurityObject,
		@IDObject,
		@IDClass,
		@IDParentObject
	)

	SET @status = @@ERROR

	IF(@status <> 0)
	BEGIN
		;THROW 60000, @procedureName, 1;
	END

	-- Clear quick cache for is_permitted
	DELETE FROM [SecurityAccessCache]

	SET @status = @@ERROR

	IF(@status <> 0)
	BEGIN
		;THROW 60000, @procedureName, 1;
	END

	-- Clear the temporary table

	DELETE [TempSecurityAccessEntries]
	WHERE SPID = @@SPID

	SET @status = @@ERROR
	IF(@status <> 0)
	BEGIN
		;THROW 60000, @procedureName, 1;
	END
	
	DELETE [TempSecurityIdentities]
	WHERE SPID = @@SPID

	SET @status = @@ERROR
	IF(@status <> 0)
	BEGIN
		;THROW 60000, @procedureName, 1;
	END

	-- First we give GENERIC_READ to the creating user
	INSERT [TempSecurityAccessEntries] ([SPID], [SID], [IDAction], [AccessDeny])
	VALUES (@@SPID, @SID, 'GENERIC_READ', 0)

	SET @status = @@ERROR

	IF(@status <> 0)
	BEGIN
		;THROW 60000, @procedureName, 1;
	END

	-- Then we give GENERIC_WRITE to the creating user
	INSERT [TempSecurityAccessEntries] ([SPID], [SID], [IDAction], [AccessDeny])
	VALUES (@@SPID, @SID, 'GENERIC_WRITE', 0)

	SET @status = @@ERROR

	IF(@status <> 0)
	BEGIN
		;THROW 60000, @procedureName, 1;
	END

	EXEC @status = [security_create_access_entry_transaction]
		@IDObject = @IDObject,
		@Replace = 0,
		@CurrentUtcTime = @CurrentUtcTime

	IF(@status <> 0)
	BEGIN
		-- security_create_access_entry_transaction raises any error, we just need to return
		RETURN @status
	END

	-- Now cleanup the temp table
	DELETE [TempSecurityAccessEntries]
	WHERE [SPID] = @@SPID

	SET @status = @@ERROR

	IF(@status <> 0)
	BEGIN
		;THROW 60000, @procedureName, 1;
	END

	RETURN 0
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_unregister_object_transaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_unregister_object_transaction]
GO

CREATE PROCEDURE [security_unregister_object_transaction]
	@IDObject [GuidType],
	@CurrentUtcTime DATETIME
AS
BEGIN
  SET NOCOUNT ON
 
  DECLARE @status INT
  DECLARE @rowCount INT

	DECLARE @IDSecurityObject [GuidType]

  -- Initialize the ProcedureName for error messages.
  DECLARE @procedureName SYSNAME
  SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
  DECLARE @ErrorArgs NVARCHAR(500)

	-- Make sure that a current transaction is active
	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END

	SELECT @IDSecurityObject = [IDSecurityObject]
	FROM [SecurityObjects]
	WHERE [IDObject] = @IDObject

	-- Verify that the object exists
	IF(@IDSecurityObject IS NULL)
	BEGIN
		SET @ErrorArgs = @procedureName + N': %%IDObject=' + CAST(@IDObject AS NVARCHAR(36)) + N'%%'
		;THROW 60012, @ErrorArgs, 1;
	END
	
	DECLARE @currentLevel INT
	SET @currentLevel = 0

	DECLARE @objectList AS TABLE (
		[IDSecurityObject] [GuidType],
		[Level] INT
	)

	INSERT @objectList ([IDSecurityObject], [Level])
	VALUES (@IDSecurityObject, @currentLevel)

	SELECT @status = @@ERROR, @rowcount = @@ROWCOUNT

	IF(@status <> 0)
	BEGIN
		;THROW 60000, @procedureName, 1;
	END

	-- loop until all objects have been examined
	WHILE(@rowCount <> 0)
	BEGIN
		INSERT @objectList ([IDSecurityObject], [Level])
		SELECT DISTINCT so.[IDSecurityObject], @currentLevel + 1
		FROM [SecurityObjects] so
		INNER JOIN @objectList objCache ON objCache.[IDSecurityObject] = so.[IDInheritsFrom] AND objCache.[Level] = @currentLevel
		WHERE so.[IDSecurityObject] NOT IN (
			SELECT objCache.[IDSecurityObject]
			FROM @objectList cache
		)

		SELECT @status = @@ERROR, @rowcount = @@ROWCOUNT

		IF(@status <> 0)
		BEGIN
			;THROW 60000, @procedureName, 1;
		END

		SET @currentLevel = @currentLevel + 1
	END
	
	-- Delete all access control entries on the object
	DELETE aces
	FROM [SecurityAccessEntries] aces
	INNER JOIN @objectList objCache ON aces.[IDSecurityObject] = objCache.[IDSecurityObject]
	
	SET @status = @@ERROR

	IF(@status <> 0)
	BEGIN
		;THROW 60000, @procedureName, 1;
	END

	-- Delete the security object and its children
	DELETE so
	FROM [SecurityObjects] so
	INNER JOIN @objectList objCache ON so.[IDSecurityObject] = objCache.[IDSecurityObject]

	SET @status = @@ERROR

	IF(@status <> 0)
	BEGIN
		;THROW 60000, @procedureName, 1;
	END

	-- Now that the security permissions are changed, we need to clear the permission cache
	-- This will be populated again when needed.
	DELETE FROM [SecurityAccessCache]

	SET @status = @@ERROR

	IF(@status <> 0)
	BEGIN
		;THROW 60000, @procedureName, 1;
	END
	
	RETURN 0
END
GO

-- Add roles to SecurityIdentity
INSERT [SecurityIdentity] (
	[SID],
	[Type],
	[DisplayName],
	[Description],
	[Domain],
	[AccountName],
	[DistinguishedName],
	[Deleted],
	[LastModified]
)
SELECT 
	sr.[IDRole],
	4,
	sr.[Name],
	sr.[Description],
	'', -- Roles don't have domain
	'', -- Roles don't have account name
	'', -- Roles don't have distinguished name
	0,
	sr.[LastModified]
FROM [SecurityRoles] sr
LEFT JOIN [SecurityIdentity] si ON si.[SID] = sr.[IDRole]
WHERE si.[SID] IS NULL

-- Add roles/identities to SecurityMembership
INSERT [SecurityMembership] ([Container], [MemberSID])
SELECT sri.[IDRole], sri.[SID]
FROM [SecurityRolesIdentity] sri
LEFT JOIN [SecurityMembership] sm ON sm.[Container] = sri.[IDRole] AND sm.[MemberSID] = sri.[SID]
WHERE sm.[Container] IS NULL AND sm.[MemberSID] IS NULL
GO
