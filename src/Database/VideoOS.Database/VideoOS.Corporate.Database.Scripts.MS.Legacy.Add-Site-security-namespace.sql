-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- ***************************************
-- Create Site security namespace
-- ***************************************
IF NOT EXISTS(SELECT * FROM [SecurityNamespace] WHERE [name] = 'Site')
BEGIN
	DECLARE @namespace_id NVARCHAR(260)
	SET @namespace_id = '875F4A4E-4D78-4A8D-A978-290227253475'

	DECLARE @separator CHAR(1)
	SET @separator = '/'

	INSERT [SecurityNamespace] ([namespace_id], [name], [display_name], [separator], [structure], [order_no], [user_defined])
	VALUES 
	(
		@namespace_id,  -- namespace_id
		'Site',         -- name
		'Sites',        -- display_name
		@separator,     -- separator
		1,              -- structure
		16,             -- order_no
		0               -- user_defined
	)

	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible])
	SELECT @namespace_id, 'GENERIC_READ', 'Read', 0, 0 UNION ALL
	SELECT @namespace_id, 'GENERIC_WRITE', 'Edit', 1, 0 UNION ALL
	SELECT @namespace_id, 'ADMIN_SECURITY', 'Administrate security', 2, 0

	DECLARE @security_object_id UNIQUEIDENTIFIER
	SELECT @security_object_id = '89FD4905-FD87-49D5-B7D1-8207E9B052BA'

	-- ***************************************
	-- Register root security group
	-- ***************************************
	IF NOT EXISTS(SELECT * FROM [SecurityObject] WHERE [namespace_id] = @namespace_id AND [security_object_id] = @security_object_id AND [object_id] = @separator)
	INSERT [SecurityObject] ([namespace_id], [parent_path], [object_id], [object_type], [object_name], [security_object_id], [inherits_from_id])
	SELECT @namespace_id, '', @separator, 1, 'Site', @security_object_id, NULL

	-- ***************************************
	-- Register sites in security model
	-- ***************************************
	INSERT [SecurityObject] ([namespace_id], [security_object_id], [object_id], [inherits_from_id], [parent_path], [object_type], [object_name])
	SELECT @namespace_id, NEWID(), [IDSite], @security_object_id, '/', 2, ''
	FROM [Sites] s
	LEFT JOIN [SecurityObject] so ON so.[object_id] = s.[IDSite]
	WHERE so.[object_id] IS NULL

	-- ***************************************
	-- Migrate site permission
	-- ***************************************
	INSERT [SecurityAccessEntry] ([security_object_id], [user_sid], [action_name], [access_deny], [deleted])
	SELECT 
		so1.[security_object_id], 
		sae.[SID], 
		sae.[IDAction],
		sae.[AccessDeny], 
		sae.[Deleted]
	FROM [SecurityAccessEntries] sae
	INNER JOIN [SecurityObjects] so ON sae.[IDSecurityObject] = so.[IDSecurityObject]
	INNER JOIN [SecurityObject] so1 ON so1.[object_id] = so.[IDObject] AND so1.[namespace_id] = @namespace_id
	WHERE so.[IDClass] = 'SITES'
END
GO
