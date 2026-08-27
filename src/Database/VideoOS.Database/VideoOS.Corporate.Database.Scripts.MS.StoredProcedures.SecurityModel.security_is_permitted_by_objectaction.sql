-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_is_permitted_by_objectaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_is_permitted_by_objectaction]
GO

CREATE PROCEDURE [security_is_permitted_by_objectaction]
	@namespace_id UNIQUEIDENTIFIER,
	@user_sid [SidType],
	@action_name NVARCHAR(260)
AS
BEGIN
	SET NOCOUNT ON

	-- Temporary table to hold objects
	DECLARE @tempSecurityObjects TABLE
	(
		[object_id] NVARCHAR(300)
	)

	-- Temporaty table to hold access entries
	DECLARE @tempAccessEntry TABLE
	(
		[access_entry_id] INT,
		[user_sid] [SidType],
		[access_deny] BIT
	)

	DECLARE @is_permitted BIT
	DECLARE @is_member BIT
	DECLARE @access_entry_id INT
	DECLARE @access_entry_sid [SidType]
	DECLARE @access_deny BIT

	DECLARE @security_object_id UNIQUEIDENTIFIER
	DECLARE @security_inherits_from_id NVARCHAR(300)
	
	DECLARE @object_id NVARCHAR(300)

	-- Verify if user is member of the administrator role
	-- If true, return is_permitted = true
	EXEC [security_is_member] @user_sid, '1f30d14b-ba91-4e8a-8838-eda17931f8fc', @is_member OUTPUT

	IF(@is_member = 1)
	BEGIN
		SELECT [object_id] FROM [SecurityObject] WHERE [namespace_id] = @namespace_id
		RETURN
	END

	DECLARE object_cursor CURSOR FOR
	SELECT [security_object_id], [inherits_from_id], [object_id] FROM [SecurityObject] WHERE [namespace_id] = @namespace_id

	OPEN object_cursor
	BEGIN TRY
		FETCH NEXT FROM object_cursor INTO @security_object_id, @security_inherits_from_id, @object_id;
		WHILE @@FETCH_STATUS = 0
		BEGIN
			-- Loop through the current security object and its inheritance
			WHILE(@security_object_id IS NOT NULL)
			BEGIN
				-- Copy all access entries on the current object
				INSERT INTO @tempAccessEntry([access_entry_id], [user_sid], [access_deny])
				SELECT [access_entry_id], [user_sid], [access_deny]
				FROM [SecurityAccessEntry]
				WHERE [security_object_id] = @security_object_id
					AND [action_name] = @action_name

				WHILE(1 = 1)
				BEGIN
					SET @access_entry_id = NULL

					SELECT TOP 1 
						@access_entry_id = [access_entry_id], 
						@access_entry_sid = [user_sid], 
						@access_deny = [access_deny]
					FROM @tempAccessEntry

					-- Check if the is more entries
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
					-- anything else. The used is denied access - proceed with next object
					IF(@is_permitted = 0)
					BEGIN
						GOTO fetch_next
					END

					-- Now we can delete the access entry we have examined above
					DELETE FROM @tempAccessEntry WHERE [access_entry_id] = @access_entry_id
				END

				-- Check if the security object is an inherited object
				IF(@security_inherits_from_id IS NULL)
				BEGIN
					IF(@is_permitted = 1)
					BEGIN
						INSERT @tempSecurityObjects
						VALUES (@object_id)
					END
					BREAK
				END
				ELSE
				BEGIN
					SET @security_object_id = NULL

					SELECT
						@security_object_id = [security_object_id],
						@security_inherits_from_id = [inherits_from_id]
					FROM [SecurityObject]
					WHERE [security_object_id] = @security_inherits_from_id
				END
			END
		
			fetch_next:
		
			SELECT @is_permitted = NULL
		
			FETCH NEXT FROM object_cursor
			INTO @security_object_id, @security_inherits_from_id, @object_id;
		END
	END TRY
	BEGIN CATCH
		CLOSE object_cursor;
		DEALLOCATE object_cursor;
		;THROW
	END CATCH

	CLOSE object_cursor;
	DEALLOCATE object_cursor;

	--update_cache:

	-- If we haven't found any access entries
	-- the user is denied access
	--IF(@is_permitted IS NULL)
	--BEGIN
	--	SET @is_permitted = 0
	--END
	
	---- Cache the result
	--BEGIN TRY
	--	INSERT [SecurityAccessCache] ([IDObject], [IDAction], [SID], [AllowedAccess])
	--	VALUES(@IDObject, @IDAction, @SID, @is_permitted)
	--END TRY
	--BEGIN CATCH
	--	-- Ignore any error. We assume it is a PK violation caused by another session.
	--END CATCH

	--SELECT @is_permitted AS is_permitted
	
	SELECT [object_id] FROM @tempSecurityObjects
END
GO