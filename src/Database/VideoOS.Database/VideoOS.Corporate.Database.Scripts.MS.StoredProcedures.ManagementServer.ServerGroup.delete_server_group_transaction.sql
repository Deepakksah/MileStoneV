-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_server_group_transaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_server_group_transaction]
GO

CREATE PROCEDURE [delete_server_group_transaction]
	@IDServerGroup [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END

	DECLARE @CurrentUtcTime DATETIME
	SET @CurrentUtcTime = GETUTCDATE()

	DECLARE @Name [NameType]
	DECLARE @IDParentGroup [GuidType]

	SELECT @IDParentGroup = [IDParentGroup],
				 @Name = [Name]
	FROM [ServerGroup] WITH(UPDLOCK)
	WHERE [IDServerGroup] = @IDServerGroup

	-- Does the server group exist?
	IF(@Name IS NULL)
	BEGIN
		;THROW 60059, @procedureName, 1;
	END

	-- Is this the primary group
	IF(@IDParentGroup = '')
	BEGIN
		;THROW 60075, @procedureName, 1;
	END
	
	-- Is this a valid server group that we can delete?
	-- We cannot remove the Private ViewGroups Root
	IF(@IDServerGroup = '4BF8A8B0-54AB-4E82-994F-98D8E9584176')
	BEGIN
		;THROW 60067, @procedureName, 1;
	END

	-- Does the server group contain any members other than other server groups
	DECLARE @CountMembers INT;

	WITH CTE_SERVER_GROUP_HIERARCHY AS
	(
		SELECT 
			sg1.[IDServerGroup],
			sg1.[IDParentGroup]
		FROM [ServerGroup] sg1  WITH(UPDLOCK)
		WHERE sg1.[IDServerGroup] = @IDServerGroup
		
		UNION ALL

		SELECT 
			sg2.[IDServerGroup],
			sg2.[IDParentGroup]
		FROM CTE_SERVER_GROUP_HIERARCHY cte
		JOIN [ServerGroup] sg2  WITH(UPDLOCK) ON sg2.[IDParentGroup] = cte.[IDServerGroup]
	)
	SELECT @CountMembers = COUNT(*) FROM [ServerGroupMembership] sgm
	INNER JOIN CTE_SERVER_GROUP_HIERARCHY cte ON sgm.[IDServerGroup] = cte.[IDServerGroup]

	IF(@CountMembers <> 0)
	BEGIN
		;THROW 60074, @procedureName, 1;
	END;

	-- Delete the server group and all child groups
	WITH CTE_SERVER_GROUP_HIERARCHY AS
	(
		SELECT 
			sg1.[IDServerGroup],
			sg1.[IDParentGroup]
		FROM [ServerGroup] sg1
		WHERE sg1.[IDServerGroup] = @IDServerGroup
		
		UNION ALL

		SELECT 
			sg2.[IDServerGroup],
			sg2.[IDParentGroup]
		FROM CTE_SERVER_GROUP_HIERARCHY cte
		JOIN [ServerGroup] sg2 ON sg2.[IDParentGroup] = cte.[IDServerGroup]
	)
	DELETE FROM [ServerGroup]
	WHERE [IDServerGroup] IN (SELECT [IDServerGroup] FROM CTE_SERVER_GROUP_HIERARCHY)

	IF(@IDParentGroup = '430C2FE2-B233-42DA-B508-1A22EF03383D' OR @IDParentGroup = '4BF8A8B0-54AB-4E82-994F-98D8E9584176')
	BEGIN
		-- Unregister the security object
		EXEC [security_unregister_object_transaction]
			@namespace_name = 'ViewGroup',
			@object_id = @IDServerGroup,
			@CurrentUtcTime = @CurrentUtcTime
	END

	-- If group is secondary failover group on a recorder, clear it
	UPDATE [Recorders]
	SET [LastModified] = @CurrentUtcTime,
		[IDFailoverSecondaryGroup] = '00000000-0000-0000-0000-000000000000'
	WHERE [IDFailoverSecondaryGroup] = @IDServerGroup

	-- If group is primary failover group on a recorder, move secondary to primary
	UPDATE [Recorders]
	SET [LastModified] = @CurrentUtcTime,
		[IDFailoverPrimaryGroup] = [IDFailoverSecondaryGroup],
		[IDFailoverSecondaryGroup] = '00000000-0000-0000-0000-000000000000'
	WHERE [IDFailoverPrimaryGroup] = @IDServerGroup

	RETURN 0
END
GO
