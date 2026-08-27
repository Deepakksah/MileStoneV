-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_is_permitted]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_is_permitted]
GO

CREATE PROCEDURE [security_is_permitted]
	@namespace_id UNIQUEIDENTIFIER,
	@user_sid [SidType],
	@action_name NVARCHAR(260),
	@object_id NVARCHAR(300),
	@legacy_security_area NVARCHAR(260) = NULL,
	@legacy_security_attribute NVARCHAR(260) = NULL
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	DECLARE @is_permitted BIT
	SET @is_permitted = NULL

	-- Before starting to resolve the permission we try to find it in the cache
	SELECT @is_permitted = [allowed_access]
	FROM [SecurityAccessCache2]
	WHERE [namespace_id] = @namespace_id
		AND [object_id] = @object_id
		AND [action_name] = @action_name
		AND [user_sid] = @user_sid

	IF(@is_permitted IS NOT NULL)
	BEGIN
		-- We found a cached entry, return it
		SELECT @is_permitted as is_permitted
		RETURN
	END

	-- Temporaty table to hold access entries
	DECLARE @tempAccessEntry TABLE (
		[access_entry_id] INT,
		[user_sid] [SidType],
		[access_deny] BIT
	)

	DECLARE @is_member BIT
	DECLARE @access_entry_id INT
	DECLARE @access_entry_sid [SidType]
	DECLARE @access_deny BIT

	DECLARE @security_object_id UNIQUEIDENTIFIER
	DECLARE @security_inherits_from_id NVARCHAR(300)

	-- Get the security object data from object table
	SELECT @security_object_id = [security_object_id],
		@security_inherits_from_id = [inherits_from_id]
	FROM [SecurityObject]
	WHERE	[namespace_id] = @namespace_id
		AND [object_id] = @object_id

	-- Ensure that the security object actually exists
	IF(@security_object_id IS NULL)
	BEGIN
		SET @ErrorArgs = @procedureName + N': %%IDObject=' + @object_id + N'%%'
		;THROW 60012, @ErrorArgs, 1;
	END

	 --Verify if user is member of the administrator role
	 --If true, return is_permitted = true
	EXEC [security_is_member] @user_sid, '1F30D14B-BA91-4E8A-8838-EDA17931F8FC', @is_member OUTPUT

	IF(@is_member = 1)
	BEGIN
		SET @is_permitted = 1
		GOTO update_cache
	END

	-- Loop through the current security object and its inheritance
	WHILE(@security_object_id IS NOT NULL)
	BEGIN
		-- Copy all access entries on the current object
		INSERT INTO @tempAccessEntry([access_entry_id], [user_sid], [access_deny])
		SELECT [access_entry_id], [user_sid], [access_deny]
		FROM [SecurityAccessEntry]
		WHERE @security_object_id = [security_object_id]
			AND [action_name] = @action_name

		WHILE(1 = 1)
		BEGIN
			SET @access_entry_id = NULL

			SELECT TOP 1 
				@access_entry_id = [access_entry_id], 
				@access_entry_sid = [user_sid], 
				@access_deny = [access_deny]
			FROM @tempAccessEntry

			-- Check if there are more entries
			IF(@access_entry_id IS NULL)
			BREAK

			-- Check if the access entry sid matches the users sid
			-- If it does the user is permitted unless denied is specified
			IF(@user_sid = @access_entry_sid)
			BEGIN
				SET @is_permitted = ~@access_deny
			END
			ELSE
			BEGIN
				-- The access entry sid may also be a group sid
				-- In that case, let's check if the user is a member
				-- of that group. If the user is a member of the group
				-- the user is permitted unless denied is specified
				EXEC [security_is_member] @user_sid, @access_entry_sid, @is_member OUTPUT
				IF(@is_member = 1)
				BEGIN
					SET @is_permitted = ~@access_deny
				END
			END

			-- If the user is denied we do not want to check
			-- anything else. The used is denied access
			IF(@is_permitted = 0)
			BEGIN
				GOTO update_cache
			END

			-- Now we can delete the access entry we have 
			-- examined above
			DELETE FROM @tempAccessEntry WHERE [access_entry_id] = @access_entry_id
		END

		-- Check if the security object is an inherited object
		IF(@security_inherits_from_id IS NULL)
			BREAK
		ELSE
		BEGIN
			SET @security_object_Id = NULL

			SELECT @security_object_id = [security_object_id],
						 @security_inherits_from_id = [inherits_from_id]
			FROM [SecurityObject]
			WHERE [namespace_id] = @namespace_id
				AND [security_object_id] = @security_inherits_from_id
		END
	END

	IF((@is_permitted IS NULL OR (@is_permitted IS NOT NULL AND @is_permitted = 0)) 
		AND @legacy_security_area IS NOT NULL
		AND @legacy_security_attribute IS NOT NULL)
	BEGIN
		-- check legacy object security
		IF(@legacy_security_area = 'DEVICE')
		BEGIN
			IF(EXISTS(SELECT CAST(spd.[Value] AS BIT)
			FROM [SecurityPermissionsDevice] spd
			INNER JOIN [RoleMembership] rm ON spd.IDRole = rm.IDRole AND rm.MemberSID = @user_sid
			WHERE spd.[SecurityAttributeID] = @legacy_security_attribute
				AND spd.[IDDevice] = @object_id))
			BEGIN
				SET @is_permitted = 1
			END
		END

		IF(@legacy_security_area = 'Matrix')
		BEGIN
			IF(EXISTS(SELECT CAST(spm.[Value] AS BIT)
			FROM [SecurityPermissionsMatrix] spm
			INNER JOIN [RoleMembership] rm ON spm.IDRole = rm.IDRole AND rm.MemberSID = @user_sid
			WHERE spm.[SecurityAttributeID] = @legacy_security_attribute
				AND spm.[IDMatrix] = @object_id))
			BEGIN
				SET @is_permitted = 1
			END
		END
	END

	update_cache:

	-- If we haven't found any access entries
	-- the user is denied access
	IF(@is_permitted IS NULL)
	BEGIN
		SET @is_permitted = 0
	END
	
	-- Cache the result
	BEGIN TRY
		INSERT [SecurityAccessCache2] ([namespace_id], [object_id], [action_name], [user_sid], [allowed_access])
		VALUES(@namespace_id, @object_id, @action_name, @user_sid, @is_permitted)
	END TRY
	BEGIN CATCH
		-- Ignore any error. We assume it is a PK violation caused by another session.
	END CATCH

	SELECT @is_permitted AS is_permitted
END
GO