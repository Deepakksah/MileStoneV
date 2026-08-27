-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_is_member]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_is_member]
GO

CREATE PROCEDURE [security_is_member]
	@SID [SidType],
	@GroupSID [SidType],
	@is_member BIT OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	DECLARE @rowCount INT

	DECLARE @identity_cache_entry INT 

	-- Query cache entry	
	SELECT @identity_cache_entry = SUM(CASE WHEN GroupSid = @groupSid THEN 1 ELSE 0 END) 
	FROM [SecurityMembershipCache]
	WHERE [MemberSid] = @SID
	
	-- The cache_entry variable can be in 3 states:
	--   NULL: cache entry does not exists, recalculation is needed
	--      0: cache entry exists, and user is not member
	--      1: cache entry exists, and user is member
	IF(@identity_cache_entry IS NOT NULL)
	BEGIN
		IF(@identity_cache_entry = 0)
		BEGIN
			SET @is_member = 0
		END
		ELSE
		BEGIN
			SET @is_member = 1
		END
	END
	ELSE
	BEGIN
		-- The MemberSID is not in the cache. This means that we
		-- have to refresh the cache.
		BEGIN TRAN
		
		-- Lock tables for update
		EXEC [security_table_lock] @LockLevel = 0

		-- We need to see if someone else updated the cache while we waited for the lock
		IF NOT EXISTS(SELECT [GroupSID] FROM [SecurityMembershipCache] WITH(READCOMMITTEDLOCK) WHERE [MemberSID] = @SID)
		BEGIN
			DECLARE @currentLevel INT
			SET @currentLevel = 0

			DECLARE @groupList AS TABLE (
				[GroupSID] [SidType],
				[Level] INT
			)

			BEGIN TRY
				INSERT @groupList([GroupSID], [Level])
				SELECT [Container], @currentLevel
				FROM [SecurityMembership] WITH(READCOMMITTEDLOCK)
				WHERE [MemberSID] = @SID

				SELECT @rowcount = @@ROWCOUNT

				WHILE(@rowcount <> 0)
				BEGIN
					-- insert new groups for this level
					INSERT @grouplist([GroupSID], [Level])
					SELECT DISTINCT membership.[Container], @currentLevel + 1
					FROM [SecurityMembership] membership WITH(READCOMMITTEDLOCK)
					JOIN  @groupList grouplist ON grouplist.[GroupSID] = membership.[MemberSID] AND grouplist.[Level] = @currentLevel
					WHERE membership.[Container] NOT IN (
						SELECT [GroupSID] FROM @groupList
					)

					SELECT @rowcount = @@ROWCOUNT

					-- move to the next level
					SET @currentLevel = @currentLevel + 1
				END

				-- now that we have the answer in the memory table, put it into the cache
				INSERT [SecurityMembershipCache]( [MemberSID], [GroupSID] )
				SELECT @SID, [GroupSID]
				FROM @groupList
			END TRY
			BEGIN CATCH
				ROLLBACK TRAN
				;THROW 60000, @procedureName, 1;
			END CATCH
		END

		-- now that the cache has been updated, find return the answer
		IF EXISTS (
			SELECT [GroupSID]
			FROM [SecurityMembershipCache] WITH(READCOMMITTEDLOCK)
			WHERE [MemberSID] = @SID
				AND [GroupSID] = @GroupSID
		)
		BEGIN
			-- the cache says that the sid is a member of the group
			SET @is_member = 1
		END
		ELSE
		BEGIN
			SET @is_member = 0
		END

		COMMIT TRAN
	END

	RETURN 0
END
GO