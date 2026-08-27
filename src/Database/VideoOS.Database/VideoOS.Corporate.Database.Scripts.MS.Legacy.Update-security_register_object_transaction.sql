-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

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
	DELETE FROM [SecurityAccessCache2]

	SET @status = @@ERROR

	IF(@status <> 0)
	BEGIN
		;THROW 60000, @procedureName, 1;
	END

	RETURN 0
END
GO
