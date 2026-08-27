-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_server_group_by_userdomainname_transaction]') AND type = 'P')
DROP PROCEDURE [delete_server_group_by_userdomainname_transaction]
GO

CREATE PROCEDURE [delete_server_group_by_userdomainname_transaction]
	@currentUserDomainName [NameType]
AS
BEGIN
	SET NOCOUNT ON
	
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	DECLARE @serverGroupId GuidType = (select [IDServerGroup] from [ServerGroup] where [Name] = @currentUserDomainName)	
	DECLARE @currentViewId GuidType;

	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END

	DECLARE viewCursor CURSOR FORWARD_ONLY STATIC READ_ONLY FOR
	WITH CTE_SERVER_GROUP_HIERARCHY AS
	(
		SELECT 
			sg1.[IDServerGroup],
			sg1.[IDParentGroup]
		FROM [ServerGroup] sg1 WITH (NOLOCK)
		WHERE sg1.[IDServerGroup] = @serverGroupId
		
		UNION ALL

		SELECT 
			sg2.[IDServerGroup],
			sg2.[IDParentGroup]
		FROM CTE_SERVER_GROUP_HIERARCHY cte
		JOIN [ServerGroup] sg2 ON sg2.[IDParentGroup] = cte.[IDServerGroup]
	)
	SELECT sgm.IDObject FROM CTE_SERVER_GROUP_HIERARCHY sg
	JOIN ServerGroupMembership sgm WITH (NOLOCK) ON sg.IDServerGroup = sgm.IDServerGroup

	OPEN viewCursor
	BEGIN TRY
		FETCH NEXT FROM viewCursor 
		INTO @currentViewId

		WHILE @@FETCH_STATUS = 0
		BEGIN		
			EXEC delete_video_wall_view_transaction null, @currentViewId
			
			FETCH NEXT FROM viewCursor 
			INTO @currentViewId
		END
	END TRY
	BEGIN CATCH
		CLOSE viewCursor
		DEALLOCATE viewCursor
		;THROW
	END CATCH

	CLOSE viewCursor
	DEALLOCATE viewCursor

	IF(@serverGroupId IS NOT NULL)
	BEGIN
		EXEC [delete_server_group_transaction] @serverGroupId
	END
END
GO