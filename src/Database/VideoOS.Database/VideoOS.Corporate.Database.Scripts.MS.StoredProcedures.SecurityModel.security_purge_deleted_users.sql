-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_purge_deleted_users]') AND type = 'P')
DROP PROCEDURE [security_purge_deleted_users]
GO

CREATE PROCEDURE [security_purge_deleted_users]
	@expiryDay INT
AS
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		--Number of user to delete
		DECLARE @deletedUsers INT

		BEGIN TRAN
		
		DECLARE @userToPurge TABLE
		(
			[SID] varchar(256) NOT NULL,
			[Domain] nvarchar(512),
			[AccountName] nvarchar(512),
			[LastModified] datetime
		)

		--Get user to delete	
		INSERT INTO @userToPurge
		SELECT [SID], [Domain], [AccountName], [LastModified]
		FROM [SecurityIdentity]
		WHERE DATEDIFF(d,LastModified, GETDATE()) >= @expiryDay AND Deleted = 1
	
		-- Create the task for deleting the sids from the idp Admin before removing it from SecurityRoleIdentity
		DECLARE @deletedSid varchar(256)

		DECLARE sid_cursor CURSOR FAST_FORWARD FOR
		SELECT [SID]
		FROM @userToPurge

		OPEN sid_cursor
		BEGIN TRY
			FETCH NEXT FROM sid_cursor INTO @deletedSid

			WHILE(@@FETCH_STATUS = 0)
			BEGIN
				-- Add the task only if the role is admin role
				IF EXISTS (SELECT * FROM [SecurityRolesIdentity] WHERE [SID] = @deletedSid AND [IDROLE] = '1f30d14b-ba91-4e8a-8838-eda17931f8fc')
				BEGIN
					EXEC [create_task_delete_idp_admin_transaction] @deletedSid
				END	

				FETCH NEXT FROM sid_cursor INTO @deletedSid
			END
		END TRY
		BEGIN CATCH
			CLOSE sid_cursor
			DEALLOCATE sid_cursor
			;THROW
		END CATCH

		CLOSE sid_cursor
		DEALLOCATE sid_cursor

		--Delete users from database
		DELETE FROM [SecurityAccessCache2] WHERE [USER_SID] IN (SELECT [SID] FROM @userToPurge)
		DELETE FROM [SecurityAccessEntryTimeProfile] WHERE [access_entry_id] IN (SELECT [access_entry_id] FROM [SecurityAccessEntry] WHERE [USER_SID] in (SELECT [SID] FROM @userToPurge))
		DELETE FROM [SecurityAccessEntry] WHERE [USER_SID] IN (SELECT [SID] FROM @userToPurge)
		DELETE FROM [SecurityMembershipCache] WHERE [MemberSID] IN (SELECT [SID] FROM @userToPurge)
		DELETE FROM [SecurityRolesIdentity] WHERE [SID] IN (SELECT [SID] FROM @userToPurge)
		DELETE FROM [CustomSettings] WHERE [OwnerSID] IN (SELECT [SID] FROM @userToPurge)
		IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Central' AND  TABLE_NAME = 'Users'))
		BEGIN
			DELETE FROM [Central].[Users] WHERE [SID] IN (SELECT [SID] FROM @userToPurge)
		END

		-- Remove membership
		DELETE FROM [SecurityMembership] WHERE [MemberSID] IN (SELECT [SID] FROM @userToPurge)
		DELETE FROM [SecurityMembership] WHERE [Container] IN (SELECT [SID] FROM @userToPurge)
		-- Remove unrooted memberships
		;WITH CTE_SECURITY_MEMBERSHIP_HIERARCHY AS
		(
			SELECT 
				sm1.[Container],
				sm1.[MemberSid],
				1 AS GroupLevel,
				0 AS IsCycle,
				'.' + CAST(sm1.[MemberSID] AS VARCHAR(MAX)) + '.' AS [Path]
			FROM [SecurityMembership] sm1
			WHERE [Container] IN 
			(
				SELECT [IDRole] FROM SecurityRoles
			)
				
			UNION ALL

			SELECT 
				sm2.[Container],
				sm2.[MemberSid],
				cte.GroupLevel + 1 AS GroupLevel,
				CASE
					WHEN cte.[Path] LIKE '%.' + CAST(sm2.[MemberSID] AS VARCHAR(MAX)) + '.%' 
					THEN 1 
					ELSE 0 
				END AS IsCycle,
				cte.[Path] + CAST(sm2.[MemberSID] AS VARCHAR(MAX)) + '.' AS [Path]
			FROM CTE_SECURITY_MEMBERSHIP_HIERARCHY cte
			JOIN [SecurityMembership] sm2 ON sm2.[Container] = cte.[MemberSID]
			WHERE GroupLevel < 100 AND cte.[IsCycle] = 0
		)
		DELETE FROM SecurityMembership 
		WHERE NOT EXISTS
		(
			SELECT cte2.Container, cte2.MemberSID
			FROM CTE_SECURITY_MEMBERSHIP_HIERARCHY cte2
			WHERE cte2.Container = SecurityMembership.[Container] AND cte2.MemberSID = SecurityMembership.[MemberSID]
		)

		--Delete from ServerGroup 
		DECLARE @USER_SID NVARCHAR(256)
		DECLARE @currentUserDomainName [NameType]
		DECLARE processedCursor CURSOR FORWARD_ONLY STATIC READ_ONLY FOR SELECT [SID], ([Domain] + '\' + [AccountName]) FROM @userToPurge
		
		OPEN processedCursor
		BEGIN TRY
			FETCH NEXT FROM processedCursor 
			INTO @USER_SID, @currentUserDomainName
		
			WHILE @@FETCH_STATUS = 0 
			BEGIN
				IF(@currentUserDomainName IS NOT NULL)
				BEGIN
					EXEC [delete_server_group_by_userdomainname_transaction] @currentUserDomainName
				END

				FETCH NEXT FROM processedCursor 
				INTO @USER_SID, @currentUserDomainName
			END
		END TRY
		BEGIN CATCH
			CLOSE processedCursor
			DEALLOCATE processedCursor
			;THROW
		END CATCH

		CLOSE processedCursor
		DEALLOCATE processedCursor

		--Delete search templates/save searches created by the deleted users
		DELETE FROM [Search].[SearchTemplates] WHERE [Availability] = 'Private' AND [Owner] IN (SELECT [SID] FROM @userToPurge)
		DELETE FROM [Search].[SearchTemplatesFind] WHERE [Owner] IN (SELECT [SID] FROM @userToPurge)

		--Delete from securityIdentity
		DELETE FROM [SecurityIdentity] WHERE [SID] IN (SELECT [SID] FROM @userToPurge)

		--Number of user deleted
		SET @deletedUsers = (select COUNT([SID]) from @userToPurge)

		COMMIT TRAN;

		RETURN @deletedUsers;
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