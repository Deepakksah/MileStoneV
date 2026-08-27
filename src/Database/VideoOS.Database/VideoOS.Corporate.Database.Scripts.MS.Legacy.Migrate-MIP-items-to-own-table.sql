-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================


IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_create_namespace]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_create_namespace]
GO

CREATE PROCEDURE [security_create_namespace]
	@namespace_id UNIQUEIDENTIFIER,
	@name NVARCHAR(260),
	@display_name NVARCHAR(260),
	@separator CHAR(1),
	@structure TINYINT,
	@user_defined BIT,
	@order_after_namespace_id UNIQUEIDENTIFIER,
	@action_values XML
AS
BEGIN
  SET NOCOUNT ON
 
  DECLARE @status INT

  -- Initialize the ProcedureName for error messages.
  DECLARE @procedureName SYSNAME
  SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
  DECLARE @ErrorArgs NVARCHAR(500)

  --BEGIN TRAN

	DECLARE @order_number INT
	SET @order_number = (SELECT [order_no] FROM [SecurityNamespace] WHERE [namespace_id] = @order_after_namespace_id)
	IF(@order_number IS NULL)
	BEGIN
		SET @order_number = (SELECT COALESCE(MAX([order_no]), 0) FROM [SecurityNamespace])
	END
	
	SET @order_number = @order_number + 1

	UPDATE [SecurityNamespace]
	SET [order_no] = [order_no] + 1
	WHERE [order_no] >= @order_number

	INSERT [SecurityNamespace] ([namespace_id], [name], [display_name], [separator], [structure], [order_no], [user_defined])
	VALUES (@namespace_id, @name, @display_name, @separator, @structure, @order_number, @user_defined)

	SET @status = @@ERROR

  IF(@status = 2601 OR @status = 2627) -- CONSTRAINT VIOLATION
	BEGIN
		-- The id or name already exists
		SET @ErrorArgs = @procedureName + N': %%Name=' + @Name + N'%%'
		;THROW 60001, @ErrorArgs, 1;
	END
	ELSE IF(@status <> 0)
	BEGIN
		;THROW 60000, @procedureName, 1;
	END
	
	IF @action_values IS NOT NULL
	BEGIN
	-- Insert actions for new security namespace
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit])
	SELECT ds.[namespace_id], ds.[name], ds.[display_name], ROW_NUMBER() OVER(ORDER BY ds.[temp]) AS [order_no], ds.[visible], CASE
			WHEN ROW_NUMBER() OVER(ORDER BY ds.[temp]) = 1 THEN 1
			ELSE POWER(2, ROW_NUMBER() OVER(ORDER BY ds.[temp]) - 1)
		 END as securityBit
	FROM
	(
		SELECT
			@namespace_id AS [namespace_id],
			[name] = v.value('@name', 'NVARCHAR(120)'),
			[display_name] = v.value('@display_name', 'NVARCHAR(120)'),
			[visible] = v.value('@visible', 'INT'),
			1 AS [temp]
		FROM @action_values.nodes('actions/action') AS T(v)
	) as ds
	END

	-- Create root group object for new security namespace
	INSERT [SecurityObject] ([namespace_id], [parent_path], [object_id], [object_type], [object_name], [security_object_id], [inherits_from_id])
	VALUES (@namespace_id, '', @separator, 1, @name, NEWID(), NULL )

	--COMMIT TRAN

	RETURN 0
END
GO


IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[CheckCreateMIPKind]') AND type in (N'P', N'PC'))
DROP PROCEDURE [CheckCreateMIPKind]
GO

CREATE PROCEDURE [CheckCreateMIPKind]
	@kind UNIQUEIDENTIFIER,
	@name as NVARCHAR(100),
	@pluginId UNIQUEIDENTIFIER,
	@itemtype NVARCHAR(10)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @GUIDEMPTY as UNIQUEIDENTIFIER
	SET @GUIDEMPTY = '00000000-0000-0000-0000-000000000000';

	IF NOT EXISTS( SELECT * FROM [Mip].[Kinds] WHERE [KindId] = @kind)
	BEGIN
		INSERT INTO [Mip].[Kinds]
		VALUES (@kind, @name, @pluginId, @name, 1, @itemtype, '', 0, @GUIDEMPTY, GETUTCDATE(), 'UpgradeScript')
	END
	ELSE
	BEGIN
		UPDATE [Mip].[Kinds]
		SET [Name] = @name, [DataVersion] = [DataVersion] + 1, [LastModified] = GETUTCDATE()
		WHERE [KindId] = @kind
	END
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[CheckCreateOverallNamespace]') AND type in (N'P', N'PC'))
DROP PROCEDURE [CheckCreateOverallNamespace]
GO

CREATE PROCEDURE [CheckCreateOverallNamespace]
	@namespaceId UNIQUEIDENTIFIER,
	@displayName as NVARCHAR(100),
	@orderAfterNamespaceId UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON
	-- Define the actions to be created
	DECLARE @defaultactions XML = N'<actions>
		<action name="GENERIC_READ" display_name="Read" visible="0"/>
		<action name="GENERIC_WRITE" display_name="Edit" visible="0"/>
		<action name="ADMIN_SECURITY" display_name="Manage security" visible="0"/>
	</actions>'

	-- First check if an old one with the signature: 1111-2222-3333 in the middle exists
	DECLARE @namespaceString as NVARCHAR(36)
	DECLARE @mixString as VARCHAR(36)
	DECLARE @CurrentUtcTime DATETIME
	SET @namespaceString = convert(varchar(36), @namespaceId);
	SET @mixString = SUBSTRING(@namespaceString, 0, 9) + '-1111-2222-3333' + SUBSTRING(@namespaceString, 24, 13);
	DECLARE @mix uniqueidentifier;
	SET @mix = CONVERT(UNIQUEIDENTIFIER, @mixString);
	DECLARE @status INT
    SET @CurrentUtcTime = GETUTCDATE()

	DECLARE @dup INT;

	IF EXISTS( SELECT * FROM [dbo].[SecurityNamespace] WHERE [namespace_id] = @mix)
	BEGIN
		
		-- We have a current namespace containing 1111-2222-3333
		IF @displayName = ''
		BEGIN
			SET @displayName = (SELECT [name] FROM SecurityNamespace WHERE namespace_id = @mix)
		END
		IF NOT EXISTS( SELECT * FROM [dbo].[SecurityNamespace] WHERE [namespace_id] = @namespaceId)
		BEGIN
			EXEC security_create_namespace
				@namespace_id = @namespaceId,
				@name = @namespaceId,
				@display_name = @displayName,
				@separator = N'/',
				@structure = 1,
				@user_defined = 1,
				@order_after_namespace_id = @orderAfterNamespaceId,
				@action_values = NULL			-- No default actions, will be moved from old namespace
			EXEC @status = [security_register_object_transaction]
				@namespaceString,
				@namespaceString,
				2,
				'',
				'/',
				@CurrentUtcTime,
				'/';
			SET @dup = 0
		END ELSE
		BEGIN
			SET @dup = 1
		END

		DECLARE @secObjectMixKind uniqueidentifier;
		DECLARE @secObjectKind uniqueidentifier;
		SET @secObjectMixKind = (SELECT security_object_id from [dbo].[SecurityObject] WHERE ([namespace_id] = @mix AND [object_id] = '/'));
		SET @secObjectKind = (SELECT security_object_id from [dbo].[SecurityObject] WHERE ([namespace_id] = @namespaceId AND [object_id] = '/'));

		IF @secObjectMixKind = NULL
		BEGIN
			ROLLBACK
			return 9999
		END
		IF @secObjectKind = NULL
		BEGIN
			ROLLBACK
			return 9998
		END

		IF @dup = 0
		BEGIN
			-- Move old Actions to new namespace
			UPDATE [dbo].[SecurityAction] SET [namespace_id] = @namespaceId WHERE [namespace_id] = @mix
			UPDATE [dbo].[SecurityAccessEntry] SET [security_object_id] = @secObjectKind WHERE ([security_object_id] = @secObjectMixKind);
		END ELSE
		BEGIN
			-- Migrate AccessEntry from 1111-2222-3333 version to the new combined namespace
			DECLARE @tickmarkActions TABLE ( [actionId] NVARCHAR(260), [user_sid] NVARCHAR(269) );
			INSERT INTO @tickmarkActions SELECT [action_name],[user_sid] FROM SecurityAccessEntry WHERE [security_object_id] = @secObjectMixKind AND [access_deny] = 0 AND [deleted] = 0;

			DECLARE @actionName NVARCHAR(260);
			DECLARE @userSid NVARCHAR(260);

			DECLARE the_actionName_cursor CURSOR FAST_FORWARD
			FOR SELECT [actionId], [user_sid] from @tickmarkActions;
			OPEN the_actionName_cursor
			FETCH NEXT FROM the_actionName_cursor INTO @actionName, @userSid
			WHILE @@FETCH_STATUS = 0
			BEGIN
				IF EXISTS(SELECT * FROM [SecurityAccessEntry] WHERE ([security_object_id] = @secObjectKind) AND [user_sid] = @userSid AND ([action_name] = @actionName))
				BEGIN
					UPDATE [SecurityAccessEntry] SET [access_deny] = 0, [deleted] = 0 WHERE ([security_object_id] = @secObjectKind) AND [user_sid] = @userSid AND ([action_name] = @actionName);
				END ELSE
				BEGIN
					DECLARE @zeroBit BIT
					SET @zeroBit = 0
					INSERT INTO [SecurityAccessEntry] ([security_object_id], [user_sid], [action_name], [access_deny], [deleted]) VALUES ( @secObjectKind, @userSid, @actionName, @zeroBit, @zeroBit)
				END
				FETCH NEXT FROM the_actionName_cursor INTO @actionName, @userSid
			END
			CLOSE the_actionName_cursor
			DEALLOCATE the_actionName_cursor

			DELETE [dbo].[SecurityAccessEntry] WHERE security_object_id = @secObjectMixKind;

			DECLARE @cnt INT
			SELECT @cnt = COUNT(*) FROM [dbo].[SecurityAccessEntry] WHERE security_object_id = @secObjectMixKind;
			IF @cnt <> 0
			BEGIN				
				ROLLBACK
				return 9997
			END
		END

		UPDATE [dbo].[SecurityObject] SET [namespace_id] = @namespaceId WHERE ([namespace_id] = @mix) AND ([object_id] <> '/')
		DELETE [dbo].[SecurityObject] WHERE [namespace_id] = @mix AND [object_id] = '/'
		DELETE [dbo].[SecurityAction] WHERE [namespace_id] = @mix
		DELETE [dbo].[SecurityNamespace] WHERE [namespace_id] = @mix
	END ELSE
	IF NOT EXISTS( SELECT * FROM [dbo].[SecurityNamespace] WHERE [namespace_id] = @namespaceId)
	BEGIN
		-- No current namespace defined, create a default version
		EXEC security_create_namespace
			@namespace_id = @namespaceId,
			@name = @namespaceId,
			@display_name = @displayName,
			@separator = N'/',
			@structure = 1,
			@user_defined = 1,
			@order_after_namespace_id = @orderAfterNamespaceId,
			@action_values = @defaultactions
		EXEC @status = [security_register_object_transaction]
			@namespaceString,
			@namespaceString,
			2,
			'',
			'/',
			@CurrentUtcTime,
			'/';
	END
END
GO


IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[MigrateNamespaceChildren]') AND type in (N'P', N'PC'))
DROP PROCEDURE [MigrateNamespaceChildren]
GO

CREATE PROCEDURE [MigrateNamespaceChildren]
	@kind UNIQUEIDENTIFIER,
	@parentKind UNIQUEIDENTIFIER
AS
BEGIN
  SET NOCOUNT ON
  DECLARE @kindmix as NVARCHAR(36);
  DECLARE @kindString as NVARCHAR(36);
  DECLARE @ObjectId as uniqueidentifier;
  DECLARE @ObjectIdString as NVARCHAR(36);
  DECLARE @kindGlobalSecurityObjectId as uniqueidentifier;
  DECLARE @childNamespaceIdString as NVARCHAR(36);
  DECLARE @childNamespaceId as uniqueidentifier;

  SET @kindString = convert(varchar(36), @kind);

  DECLARE the_parent_item_cursor CURSOR FAST_FORWARD
  FOR SELECT ItemId from [Mip].[Items] WHERE Kind = @parentKind;

  SET @kindGlobalSecurityObjectId = (SELECT security_object_id from [dbo].[SecurityObject] WHERE ([namespace_id] = @kind AND [object_id] = '/'));

  DECLARE @CurrentUtcTime DATETIME
  SET @CurrentUtcTime = GETUTCDATE()

  DECLARE @firstTime BIT
  SET @firstTime = 1

  OPEN the_parent_item_cursor
  FETCH NEXT FROM the_parent_item_cursor INTO @ObjectId
  WHILE @@FETCH_STATUS = 0
  BEGIN
	-- We now have an Item with zero or more children (e.g. a Controller)
	SET @ObjectIdString = CONVERT(NVARCHAR(36), @ObjectId);
	SET @childNamespaceIdString = SUBSTRING(@ObjectIdString, 1,18) + SUBSTRING(@kindString, 19, 18);
	SET @childNamespaceId = CONVERT(UNIQUEIDENTIFIER, @childNamespaceIdString);
	---- Modify Action definitions, rename or delete

	IF @firstTime = 1 
	BEGIN
		UPDATE [dbo].[SecurityAction] SET namespace_id = @kind WHERE namespace_id = @childNamespaceId
		SET @firstTime = 0
	END
	ELSE
	BEGIN
		DELETE FROM [dbo].[SecurityAction] WHERE namespace_id = @childNamespaceId
	END

	-- Now update SecurityObject table for all Child Items or one parent
	DECLARE @childNamespaceIdStringPath as NVARCHAR(100);
	SET @childNamespaceIdStringPath = '/' + @childNamespaceIdString + '/';
	DECLARE @prevSecurityObjectId as UNIQUEIDENTIFIER;
	SET @prevSecurityObjectId = (SELECT security_object_id FROM [dbo].[SecurityObject] WHERE (namespace_id = @childNamespaceId) AND (object_id = @childNamespaceIdString));

	-- Create group level object, representing all childs under a specific parent Item
	DECLARE @parentPath as NVARCHAR(100);
	SET @parentPath = '/' + CONVERT(NVARCHAR(36), @ObjectId) + '/';

	DECLARE @status as int;
	EXEC @status = [security_register_object_transaction]
		@kind,
		@ObjectId,
		2,
		'',
		'/',
		@CurrentUtcTime,
		'/';
	DECLARE @newSecurityObjectId AS UNIQUEIDENTIFIER
	SET @newSecurityObjectId = (SELECT security_object_id FROM dbo.SecurityObject WHERE (namespace_id = @kind) AND (object_id = @ObjectIdString));

	-- Update old security object to inherit new one
	UPDATE [dbo].[SecurityObject] 
		SET namespace_id = @kind,
			parent_path = @parentPath,
			inherits_from_id = @newSecurityObjectId
		WHERE namespace_id = @childNamespaceId AND parent_path = @childNamespaceIdStringPath;

	-- New update all access entries to use new security object
	UPDATE dbo.SecurityAccessEntry SET security_object_id = @newSecurityObjectId WHERE security_object_id = @prevSecurityObjectId; 

	-- Delete 2 old security objects
	DELETE FROM  [dbo].[SecurityObject] WHERE namespace_id = @childNamespaceId AND parent_path = '/';
	DELETE FROM  [dbo].[SecurityObject] WHERE namespace_id = @childNamespaceId AND parent_path = '';

	-- Delete unused namespace
	DELETE FROM  [dbo].[SecurityNamespace] WHERE namespace_id = @childNamespaceId;

	FETCH NEXT FROM the_parent_item_cursor INTO @ObjectId
  END
  CLOSE the_parent_item_cursor
  DEALLOCATE the_parent_item_cursor

  RETURN 0
END
GO


IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_register_object]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_register_object]
GO

CREATE PROCEDURE [security_register_object]
	@namespace_id NVARCHAR(260),
	--@user_sid [SIDType] = NULL,
	@object_id NVARCHAR(300),
	@object_name NVARCHAR(100) = '',
	@object_type TINYINT = 2,
	@parent_path NVARCHAR(400) = '/',
	@parent_object_id NVARCHAR(260) = NULL
AS
BEGIN
  SET NOCOUNT ON
 
  DECLARE @status INT

  -- Initialize the ProcedureName for error messages.
  DECLARE @procedureName SYSNAME
  SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	--BEGIN TRAN

	-- Lock tables for update
	--TODO: EXEC [security_table_lock] @LockLevel = 0

	DECLARE @CurrentUtcTime DATETIME
	SET @CurrentUtcTime = GETUTCDATE()

	-- Get the name of the namespace from the namespace id
	DECLARE @namespace_name NVARCHAR(260)
	SELECT @namespace_name = [name] FROM [SecurityNamespace] WHERE [namespace_id] = @namespace_id

	IF(@parent_object_id IS NOT NULL AND @parent_object_id <> '/')
	BEGIN
		SELECT @parent_path = [full_path] + '/' FROM [SecurityObject] WHERE [object_id] = @parent_object_id
	END
	ELSE
	BEGIN
		SELECT @parent_path = '/', @parent_object_id = '/'
	END
	
	EXEC @status = [security_register_object_transaction]
		@namespace_name,
		@object_id,
		@object_type,
		@object_name,
		@parent_path,
		@CurrentUtcTime,
		@parent_object_id

	IF(@status <> 0)
	BEGIN
		-- security_register_object_transaction raises errors, so we don't have to
		--ROLLBACK TRAN
		RETURN @status
	END

	-- TODO: We need to handle if security changed
	SELECT CAST(1 AS BIT) AS security_changed

	--COMMIT TRAN

	RETURN 0
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_register_object_transaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_register_object_transaction]
GO

CREATE PROCEDURE [security_register_object_transaction]
	@namespace_name NVARCHAR(260),
	@object_id NVARCHAR(300),
	@object_type TINYINT = 2,
	@object_name NVARCHAR(100) = '',
	@parent_path NVARCHAR(400) = '/',
	@CurrentUtcTime DATETIME,
	@parent_object_id NVARCHAR(300) = '/'
AS
BEGIN
  SET NOCOUNT ON
 
  -- Initialize the ProcedureName for error messages.
  DECLARE @procedureName SYSNAME
  SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
  DECLARE @ErrorArgs NVARCHAR(500)

  DECLARE @status INT

	-- Make sure that a current transaction is active
	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END

	-- Get the id of the namespace from the namespace name and the separator
	DECLARE @namespace_id UNIQUEIDENTIFIER
	DECLARE @separator CHAR(1)
	SELECT @namespace_id = [namespace_id], @separator = [separator] FROM [SecurityNamespace] WHERE [name] = @namespace_name

	-- Verify that the namespace exists
	IF(@namespace_id IS NULL)
	BEGIN
		SET @ErrorArgs = @procedureName + N': %%IDClass=' + @namespace_name + N'%%'
		;THROW 60017, @ErrorArgs, 1;
	END

	-- Ensure that parent path is specified
	IF(@parent_path IS NULL)
	BEGIN
		-- TODO: Get right number and parameter
		SET @ErrorArgs = @procedureName + N': %%IDClass=' + @namespace_name + N'%%'
		;THROW 60017, @ErrorArgs, 1;
	END

	-- Ensure that parent path specified is a group
	IF(RIGHT(@parent_path, 1) <> @separator)
	BEGIN
		-- TODO: Get right number and parameter
		SET @ErrorArgs = @procedureName + N': %%IDClass=' + @namespace_name + N'%%'
		;THROW 60017, @ErrorArgs, 1;
	END

	-- Ensure that object id doesn't contain a separator. Only trailing separators are allowed, indicating a group
	IF((CHARINDEX(@separator, @object_id) <> LEN(@object_id)) AND (CHARINDEX(@separator, @object_id) <> 0))
	BEGIN
		-- TODO: Get right number and parameter
		SET @ErrorArgs = @procedureName + N': %%IDClass=' + @namespace_name + N'%%'
		;THROW 60017, @ErrorArgs, 1;
	END

	-- Ensure that parent_path + object_id is 900 bytes or less. This resolves to 12 levels (which is reasonable)
	IF(DATALENGTH(@parent_path + @object_id) > 900)
	BEGIN
		-- TODO: Get right number and parameter
		SET @ErrorArgs = @procedureName + N': %%IDClass=' + @namespace_name + N'%%'
		;THROW 60017, @ErrorArgs, 1;
	END

	DECLARE @parent_namespace_id UNIQUEIDENTIFIER
	DECLARE @parent_security_object_id UNIQUEIDENTIFIER

	-- Verify that the parent group exists and get the parent security object id
	SELECT @parent_security_object_id = [security_object_id], @parent_namespace_id = [namespace_id]
	FROM [SecurityObject]
	WHERE [namespace_id] = @namespace_id AND [object_id] = @parent_object_id

	IF(@parent_security_object_id IS NULL)
	BEGIN
		SET @ErrorArgs = @procedureName + N': %%IDParentObject=' + @parent_object_id + N'%%'
		;THROW 60018, @ErrorArgs, 1;
	END

	IF(@namespace_id != @parent_namespace_id)
	BEGIN
		SET @ErrorArgs = @procedureName + N': %%IDParentObject=' + @parent_object_id + N'%%; %%IDObject=' + @object_id + N'%%'
		;THROW 60019, @ErrorArgs, 1;
	END

	-- Verify that the object doesn't already exists in the same level
	IF EXISTS(SELECT [object_id] FROM [SecurityObject] WHERE [namespace_id] = @namespace_id AND [parent_path] = @parent_path AND [object_id] = @object_id)
	BEGIN
		SET @ErrorArgs = @procedureName + N': %%IDObject=' + @object_id + N'%%'
		;THROW 60016, @ErrorArgs, 1;
	END
	
	-- Completed all validation and found the parent group

	-- Generate a new security object id
	DECLARE @security_object_id UNIQUEIDENTIFIER
	SET @security_object_id = NEWID()

	-- Now create the security object
	INSERT [SecurityObject] (
		[namespace_id],
		[parent_path],
		[object_id],
		[object_type],
		[object_name],
		[security_object_id],
		[inherits_from_id]
	)
	VALUES (
		@namespace_id,
		@parent_path,
		@object_id,
		@object_type,
		@object_name,
		@security_object_id,
		@parent_security_object_id
	)

	SET @status = @@ERROR

	IF(@status <> 0)
	BEGIN
		;THROW 60000, @procedureName, 1;
	END

	-- Clear quick cache for is_permitted
	--TODO: DELETE FROM [SecurityAccessCache2]

	SET @status = @@ERROR

	IF(@status <> 0)
	BEGIN
		;THROW 60000, @procedureName, 1;
	END

	RETURN 0
END
GO


DECLARE @IDCustomSetting as [dbo].[GuidType];
DECLARE @SettingDataXml as XML;
DECLARE @LastModified as DATETIME;
DECLARE @ModifiedUser as NVARCHAR(256);
DECLARE @DataVersion as INT;
DECLARE @PluginId as UNIQUEIDENTIFIER;
DECLARE @PluginCursor as CURSOR;
DECLARE @IndexPluginId as UNIQUEIDENTIFIER;
DECLARE @IndexLastModified as DATETIME;
DECLARE @IndexSettingDataXml as XML;
DECLARE @PresentPlugins TABLE  ( PluginId UNIQUEIDENTIFIER );
--DECLARE @ParentKindId as UNIQUEIDENTIFIER;
DECLARE @PresentKindParentKinds TABLE ( Kind UNIQUEIDENTIFIER, ParentKind UNIQUEIDENTIFIER);
DECLARE @GUIDEMPTY as UNIQUEIDENTIFIER

-- This is the fixed GUID used for the index row in the table
SET @IndexPluginId = '7ba35bda-87da-4d9b-9403-a1edae42daa8';
SET @GUIDEMPTY = '00000000-0000-0000-0000-000000000000';

-- Define the actions to be created
DECLARE @defaultactions XML = N'<actions>
		<action name="GENERIC_READ" display_name="Read" visible="1"/>
		<action name="GENERIC_WRITE" display_name="Edit" visible="1"/>
		<action name="ADMIN_SECURITY" display_name="Manage security" visible="0"/>
	</actions>'

BEGIN TRAN

	-- Find the namespace_id's that are userdefined, is a GUID AND does NOT match the name and does NOT contain the '1111-2222-3333' pattern
	DECLARE @NamespaceToBeUpdated TABLE  ( namespace_id UNIQUEIDENTIFIER );

	INSERT @NamespaceToBeUpdated Select [namespace_id] from [SecurityNamespace] where 
		[user_defined] = 1 AND
		[name] <> [namespace_id] AND
		[name] LIKE '[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]-[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]-[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]-[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]-[0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]' AND
		SUBSTRING(convert(varchar(36), [namespace_id]),10,14) <> '1111-2222-3333'

	--Select * from @NamespaceToBeUpdated

	-- Update the found namespace_id's by:
	---- Create new SecurityNamespace
	---- Create new set of actions in SecurityActions
	---- Update SecurityObject to new namespace_id
	---- Delete SecurityActions
	---- Delete SecurityNamespace

	DECLARE @ns uniqueidentifier
	DECLARE the_namespace_cursor CURSOR FAST_FORWARD
	FOR SELECT [namespace_id] from @NamespaceToBeUpdated;
	OPEN the_namespace_cursor
	FETCH NEXT FROM the_namespace_cursor INTO @ns
	WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE @new_ns uniqueidentifier
		SET @new_ns = CONVERT(UNIQUEIDENTIFIER, (SELECT [name] FROM [SecurityNamespace] WHERE [namespace_id] = @ns));

		DECLARE @real_name NVARCHAR(256)
		SET @real_name = (SELECT [name] FROM [SecurityNamespace] WHERE [namespace_id] = @ns);

		UPDATE [SecurityNamespace] SET [name] = [name] + 'temp' where [namespace_id] = @ns

		INSERT INTO [SecurityNamespace] ([namespace_id], [name], [display_name], [separator], [structure], [order_no], [user_defined])
			SELECT @new_ns, @real_name, [display_name], [separator], [structure], [order_no], [user_defined] FROM [SecurityNamespace] WHERE [namespace_id] = @ns

		INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit])
			SELECT @new_ns, [name], [display_name], [order_no], [visible], [security_bit] FROM [SecurityAction] WHERE [namespace_id] = @ns;

		UPDATE [SecurityObject] SET [namespace_id] = @new_ns WHERE [namespace_id] = @ns

		DELETE FROM [SecurityAction] WHERE [namespace_id] = @ns

		DELETE FROM [SecurityNamespace] WHERE [namespace_id] = @ns

		FETCH NEXT FROM the_namespace_cursor INTO @ns
	END
	CLOSE the_namespace_cursor
	DEALLOCATE the_namespace_cursor


-- Make cursor running through all rows in the existing CustomSettings database
SET @PluginCursor = CURSOR FOR
SELECT LastModified, IDCustomSetting, DataVersion, CAST(SettingDataXml AS XML), ModifiedUser
	FROM dbo.CustomSettings;

OPEN @PluginCursor;
FETCH NEXT FROM @PluginCursor INTO @LastModified, @IDCustomSetting, @DataVersion, @SettingDataXml, @ModifiedUser;
-- For all other rows than the one containing the index, if it is a PlatformConfiguration we will run through the items inside and add them to MIP.Items table 
WHILE @@FETCH_STATUS = 0
BEGIN
	SET @PluginId = null;
	SELECT
		@PluginId = FullTable.value('(@ID)[1]', 'uniqueidentifier')
	FROM @SettingDataXml.nodes('(/PlatformConfiguration)') FullTables(FullTable)

	IF @PluginId = @IndexPluginId
	BEGIN
		-- This is the index row so store the xml for traversal after we are doing running through the items
		SET @IndexLastModified = @LastModified;
		SET @IndexSettingDataXml = @SettingDataXml;
	END
	ELSE IF @PluginId IS NOT NULL
	BEGIN
		-- Register that this plugin ID has been seen and thus is a 'proper' plugin (and not some internal stuff)
		INSERT INTO @PresentPlugins 
			SELECT @PluginId
		-- Now store all the items in the MIP.Items table
		INSERT INTO [Mip].[Items]
			SELECT
				ItemTable.value('(Id)[1]', 'uniqueidentifier'),
				CASE
				WHEN ItemTable.value('(Name)[1]', 'nvarchar(256)') IS NULL
				THEN ''
				ELSE ItemTable.value('(Name)[1]', 'nvarchar(256)')
				END,
				ItemTable.value('(FQID/Kind)[1]', 'uniqueidentifier'),
				CASE
				WHEN ItemTable.value('(FQID/ParentId)[1]', 'uniqueidentifier') IS NULL
				THEN '00000000-0000-0000-0000-000000000000'
				ELSE ItemTable.value('(FQID/ParentId)[1]', 'uniqueidentifier')
				END,
				ItemTable.value('(FQID/ServerId/Id)[1]', 'uniqueidentifier'),
				ItemTable.value('(FQID/ServerId/Hostname)[1]', 'nvarchar(256)'),
				ItemTable.value('(FQID/ServerId/Port)[1]', 'integer'),
				ItemTable.value('(FQID/ServerId/Type)[1]', 'nvarchar(256)'),
				~ItemTable.query('properties').exist('properties/property[(@name="Enabled" and (@value="No" or @value="False")) or (@name="Disabled" and (@value="Yes" or @value="True"))]'),
				CONVERT (NVARCHAR(MAX), ItemTable.query('properties')),
				geography::STPointFromText('POINT EMPTY', 4326),
				@LastModified
			FROM @SettingDataXml.nodes('(/PlatformConfiguration/Configurations/*)') SubTables(ItemTable)
	END
	FETCH NEXT FROM @PluginCursor INTO @LastModified, @IDCustomSetting, @DataVersion, @SettingDataXml, @ModifiedUser;
END

CLOSE @PluginCursor;
DEALLOCATE @PluginCursor;

-- Find Parent Kind Id and build Parent Kind table
DECLARE @Kind as UNIQUEIDENTIFIER
DECLARE @ParentId as UNIQUEIDENTIFIER

DECLARE KindCursor  CURSOR FAST_FORWARD
	FOR SELECT DISTINCT Kind, ParentId FROM Mip.Items;

OPEN KindCursor
FETCH NEXT FROM KindCursor INTO @Kind, @ParentId;
WHILE @@FETCH_STATUS = 0
BEGIN
	DECLARE @ParentKind AS UNIQUEIDENTIFIER
	IF @ParentId <> @GUIDEMPTY
	BEGIN
		SET @ParentKind = (SELECT Kind FROM Mip.Items WHERE ItemId = @ParentId);
	END
	ELSE
	BEGIN
		SET @ParentKind = @GUIDEMPTY
	END
	IF NOT EXISTS(SELECT ParentKind FROM @PresentKindParentKinds WHERE Kind = @Kind)
	BEGIN
		INSERT INTO @PresentKindParentKinds VALUES (@Kind, @ParentKind);
	END

	FETCH NEXT FROM KindCursor INTO @Kind, @ParentId;
END
CLOSE KindCursor
DEALLOCATE KindCursor

-- Finally we run through the XML from the index row and add all kinds found there, which also has a PlatformConfiguration entry (and thus is a proper plugin), to the MIP.Kinds table
IF @IndexSettingDataXml IS NOT NULL
BEGIN
		INSERT INTO [Mip].[Kinds]
		SELECT
			ItemTable.value('(Id/node())[1]', 'uniqueidentifier'),
			'',
			CASE 
			WHEN ItemTable.query('properties').value('(properties/property[@name="PluginId"]/@value)[1]','uniqueidentifier') IS NULL 
			THEN ItemTable.value('(Id/node())[1]', 'uniqueidentifier')
			ELSE ItemTable.query('properties').value('(properties/property[@name="PluginId"]/@value)[1]','uniqueidentifier')
			END,
			CASE 
			WHEN ItemTable.query('properties').value('(properties/property[@name="PluginName"]/@value)[1]','nvarchar(256)') IS NULL 
			THEN ''
			ELSE ItemTable.query('properties').value('(properties/property[@name="PluginName"]/@value)[1]','nvarchar(256)')
			END,
			ItemTable.query('properties').value('(properties/property[@name="DataVersion"]/@value)[1]','integer'),
			CASE
			WHEN ItemTable.query('properties').value('(properties/property[@name="PluginId"]/@value)[1]','uniqueidentifier') IS NULL 
				AND EXISTS( SELECT * FROM @PresentPlugins WHERE PluginId = ItemTable.value('(Id/node())[1]', 'uniqueidentifier'))
		      OR ItemTable.query('properties').value('(properties/property[@name="PluginId"]/@value)[1]','uniqueidentifier') IS NOT NULL 
				AND EXISTS( SELECT * FROM @PresentPlugins WHERE PluginId = ItemTable.query('properties').value('(properties/property[@name="PluginId"]/@value)[1]','uniqueidentifier'))
			THEN 'ITEM'
			ELSE 'OPTION'
			END,
			'',
			1, -- DisplayOnGisMap						
			@GUIDEMPTY,
			@LastModified,
			@ModifiedUser
		FROM @IndexSettingDataXml.nodes('(/PlatformConfiguration/Configurations/*)') SubTables(ItemTable)	

		UPDATE Mip.Kinds SET ParentKindId = 
			(CASE 
				WHEN (SELECT ParentKind FROM @PresentKindParentKinds WHERE Kind = KindId) <> NULL
			 THEN 
				(SELECT ParentKind FROM @PresentKindParentKinds WHERE Kind = KindId) 
			 ELSE
				@GUIDEMPTY
			END)

		-- Now migrate security definitions - top level first

		DECLARE the_cursor CURSOR FAST_FORWARD
		FOR SELECT KindId from [Mip].[Kinds] WHERE ParentKindId = @GUIDEMPTY;

		OPEN the_cursor
		FETCH NEXT FROM the_cursor INTO @kind
		WHILE @@FETCH_STATUS = 0
		BEGIN
			EXEC CheckCreateOverallNamespace @kind, '', NULL
			FETCH NEXT FROM the_cursor INTO @kind
		END
		CLOSE the_cursor
		DEALLOCATE the_cursor

		DECLARE the_cursor CURSOR FAST_FORWARD
		FOR SELECT KindId from [Mip].[Kinds] WHERE ParentKindId != @GUIDEMPTY;

		OPEN the_cursor
		FETCH NEXT FROM the_cursor INTO @kind
		WHILE @@FETCH_STATUS = 0
		BEGIN
			SET @parentKind = (SELECT ParentKindId FROM [Mip].[Kinds] WHERE KindId = @kind);
			EXEC dbo.security_create_namespace
				@namespace_id = @kind,
				@name = @kind,
				@display_name = @kind,
				@separator = N'/',
				@structure = 1,
				@user_defined = 1,
				@order_after_namespace_id = NULL,
				@action_values = NULL

			EXEC MigrateNamespaceChildren @kind, @parentKind
			FETCH NEXT FROM the_cursor INTO @kind
		END
		CLOSE the_cursor
		DEALLOCATE the_cursor

END

    -- Remove dummy action targeted XPE support
    DELETE FROM [dbo].[SecurityAction] WHERE [Name] = 'ISNOWDEFINED';

-- Ensure build-in kinds exist in database
----Alarm Definitions
	EXEC [CheckCreateMIPKind] '9807ca8a-46c2-45ed-9446-6423dae2cd88', 'Alarm Definition', '46bb0392-fd06-465b-8776-b90ac16639b3', 'ITEM'
	EXEC [CheckCreateOverallNamespace] '46bb0392-fd06-465b-8776-b90ac16639b3', 'Alarm Definition', 'B71FD628-E593-42FC-B528-5A927EA9BDA4'

---- License Item
	EXEC [CheckCreateMIPKind] 'E145597A-ECAF-40A1-99E6-14BB9D45A09E', 'License', 'E145597A-ECAF-40A1-99E6-14BB9D45A09E', 'ITEM'
	EXEC [CheckCreateOverallNamespace] 'E145597A-ECAF-40A1-99E6-14BB9D45A09E', 'License', NULL

----Analytics Events
	EXEC [CheckCreateMIPKind] '2E319F3E-0350-4DF1-90C6-633EAD31C117', 'Analytics Events', '9F6E7E9D-3FD2-49B2-9CA0-F7E7F8F8C3A7', 'ITEM'
	EXEC [CheckCreateOverallNamespace] '2E319F3E-0350-4DF1-90C6-633EAD31C117', 'Analytics Events', 'D4FF31CD-D9AC-46DF-8E92-5B545D191BAF'

---- Analytics option menu is stored as one Item
	EXEC [CheckCreateMIPKind] '4de695a0-b30a-4f0f-b25e-ee353528dc02', 'Analytics Options', '9F6E7E9D-3FD2-49B2-9CA0-F7E7F8F8C3A7', 'ITEM'

----Generic Events
	EXEC [CheckCreateMIPKind] 'D4D19C01-03F2-4AC6-9D2B-5356B5DE62F1', 'Generic Event', 'FF9DE88E-45F2-4B1E-A551-30AD84E25102', 'ITEM'
	EXEC [CheckCreateOverallNamespace] 'D4D19C01-03F2-4AC6-9D2B-5356B5DE62F1', 'Generic Event', '2E319F3E-0350-4DF1-90C6-633EAD31C117'

----Generic Events DataSource
	EXEC [CheckCreateMIPKind] '2DCC9D9D-688E-468D-AF6F-C2A9ACD623FE', 'Generic Events', '2DCC9D9D-688E-468D-AF6F-C2A9ACD623FE', 'OPTION'

----Transact
	EXEC [CheckCreateMIPKind] '9A1492D1-FCA0-44D7-BA9C-3699C99063A7', 'Transaction source', '71E86D1A-30EE-4D58-8AD6-2CF7245DDA9F', 'ITEM'
	EXEC [CheckCreateOverallNamespace] '9A1492D1-FCA0-44D7-BA9C-3699C99063A7', 'Transaction source', NULL

	EXEC [CheckCreateMIPKind] 'B4EE79CC-7D0B-449F-A53A-EBF0A3D1F14B', 'Transaction definition', '71E86D1A-30EE-4D58-8AD6-2CF7245DDA9F', 'ITEM'
	EXEC [CheckCreateOverallNamespace] 'B4EE79CC-7D0B-449F-A53A-EBF0A3D1F14B', 'Transaction definition', NULL

----Customer Dashboard
	EXEC [CheckCreateMIPKind] '4D2859D2-90D9-4192-AFC3-3C435241AF0E', 'Customer Dashboard', 'A4A339FD-A4A6-4DA7-88C9-5D07AAE61CCA', 'OPTION'

----Access Control (Taken out again for now)
--	EXEC [CheckCreateMIPKind] 'DBB3A246-122E-498D-A804-6363A154A127', 'Access Control Server', 'FE9AA43F-508C-469F-93E7-94AB47F8E823', 'ITEM'
--	EXEC [CheckCreateOverallNamespace] 'DBB3A246-122E-498D-A804-6363A154A127', 'Access Control Server', NULL
--	EXEC [CheckCreateMIPKind] 'B71FD628-E593-42FC-B528-5A927EA9BDA4', 'Access Control System', 'BB529DE8-608E-4EBB-B796-889504E54670', 'ITEM'
--	EXEC [CheckCreateOverallNamespace] 'B71FD628-E593-42FC-B528-5A927EA9BDA4', 'Access Control System', '875F4A4E-4D78-4A8D-A978-290227253475'

----Mobile Servers	
	EXEC [CheckCreateMIPKind] '9947F340-397C-483B-9E03-F27253F7F403', 'Mobile Servers', '32C415C6-6DDB-4931-AE35-E971590DD8B6', 'ITEM'
	EXEC [CheckCreateOverallNamespace] '9947F340-397C-483B-9E03-F27253F7F403', 'Mobile Servers', '6339122C-04F5-41F2-B3E3-3555997CFCEF'

COMMIT TRAN

GO
