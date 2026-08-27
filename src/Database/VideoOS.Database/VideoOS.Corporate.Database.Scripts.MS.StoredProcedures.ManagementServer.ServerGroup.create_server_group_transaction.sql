-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_server_group_transaction]') AND type = 'P')
DROP PROCEDURE [create_server_group_transaction]
GO

CREATE PROCEDURE [create_server_group_transaction]
	@IDServerGroup [GuidType],
	@Name [NameType],
	@Description NVARCHAR(1024),
	@IDParentGroup [GuidType],
	@StructureType VARCHAR(64),
	@user_sid [SIDType],
	@custom_properties XML = NULL
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	DECLARE @CurrentUtcTime DATETIME
	SET @CurrentUtcTime = GETUTCDATE()

	DECLARE @ParentGroupName [NameType]
	DECLARE @ParentPath NVARCHAR(MAX)
	DECLARE @Level INT
	DECLARE @OrderNo INT

	-- If object is a ViewGroup then register in security system
	DECLARE @SmartClientGroup [GuidType]
	SET @SmartClientGroup = '430C2FE2-B233-42DA-B508-1A22EF03383D'

	DECLARE @PrivateGroupRoot [GuidType]
	SET @PrivateGroupRoot = '4BF8A8B0-54AB-4E82-994F-98D8E9584176'

	-- Make sure that a current transaction is active
	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END

	-- Construct the path
	SELECT
		@ParentGroupName = [Name],
		@ParentPath = [path] + @Name + '\',
		--@StructureType = [StructureType],
		@Level = [Level] + 1
	FROM [ServerGroup] WITH(UPDLOCK)
	WHERE [IDServerGroup] = @IDParentGroup

	-- parent group must exist
	IF(@ParentGroupName IS NULL)
	BEGIN
		;THROW 60056, @procedureName, 1;
	END

	-- Allow a maximum of 12 levels
	IF(@level = 12)
	BEGIN
		;THROW 60057, @procedureName, 1;
	END

	-- Determine default new order number	
	SELECT @OrderNo = COUNT(*)
	FROM [ServerGroup]
	WHERE [IDParentGroup] = @IDParentGroup
	
	IF(EXISTS(SELECT IDServerGroup FROM [ServerGroup] WHERE IDParentGroup = @PrivateGroupRoot AND IDParentGroup = @IDParentGroup AND Name = @Name))
	BEGIN
		-- Private group already exist. Return
		RETURN 0
	END

	BEGIN TRY
		INSERT [ServerGroup] (
			[IDServerGroup],
			[Name],
			[Description],
			[IDParentGroup],
			[StructureType],
			[OrderNo],
			[Level],
			[Path],
			[CreatedDate],
			[LastModified],
			[RelationIds]
		)
		VALUES (
			@IDServerGroup,
			@Name,
			@Description,
			@IDParentGroup,
			@StructureType,
			@OrderNo,
			@level,
			@ParentPath,
			@CurrentUtcTime,
			@CurrentUtcTime,
			''
		)
	END TRY
	BEGIN CATCH
		IF(ERROR_NUMBER() = 2601 OR ERROR_NUMBER() = 2627) -- INDEX VIOLATION
		BEGIN
			SET @ErrorArgs = @procedureName + N': %%Name=' + @Name + N'%%; %%groupName=' + @ParentGroupName + N'%%'
			;THROW 60055, @ErrorArgs, 1;
		END

		;THROW;
	END CATCH

    UPDATE [ServerGroup]
    SET [RelationIds] = dbo.GetServerGroupRelations(@IDServerGroup)
    WHERE [IDServerGroup] = @IDServerGroup

	-- Update custom properties
	EXEC [update_custom_properties_transaction]
		@object_id = @IDServerGroup,
		@custom_properties = @custom_properties

	DECLARE @Path NVARCHAR(400)
	SET @Path = '/'

	IF(@IDParentGroup = @PrivateGroupRoot)
	BEGIN
		SET @Path = @Path + @PrivateGroupRoot + '/'
	END

	IF(@IDParentGroup = @SmartClientGroup OR @IDParentGroup = @PrivateGroupRoot)
	BEGIN
		-- Register the object in the security system
		EXEC [security_register_object_transaction]
			@namespace_name = 'ViewGroup',
			@object_id = @IDServerGroup,
			@parent_path = @Path,
			@CurrentUtcTime	= @CurrentUtcTime
	END

	IF(@IDParentGroup = @PrivateGroupRoot)
	BEGIN
		DECLARE @view_group_namespace UNIQUEIDENTIFIER

		SELECT @view_group_namespace = [namespace_id]
		FROM [SecurityNamespace]
		WHERE [name] = 'ViewGroup'

		-- Then we give all permissions to the creating user
		INSERT [TempSecurityAccessEntry] ([SPID], [user_sid], [action_name], [access_deny])
		SELECT @@SPID, @user_sid, 'GENERIC_READ', 0 UNION ALL
		SELECT @@SPID, @user_sid, 'GENERIC_WRITE', 0 UNION ALL
		SELECT @@SPID, @user_sid, 'DELETE', 0 UNION ALL
		SELECT @@SPID, @user_sid, 'OPERATE', 0 UNION ALL
		SELECT @@SPID, @user_sid, 'ADMIN_SECURITY', 0 UNION ALL
		SELECT @@SPID, @user_sid, 'CREATE_VIEWGROUP', 0

		EXEC [security_create_access_entry_transaction]
			@namespace_id = @view_group_namespace,
			@object_id = @IDServerGroup,
			@object_name = @Name,
			@replace = 0,
			@CurrentUtcTime = @CurrentUtcTime
	END

	RETURN 0
END
GO