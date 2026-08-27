-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_query_access_entries]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_query_access_entries]
GO

CREATE PROCEDURE [security_query_access_entries]
	@namespace_id UNIQUEIDENTIFIER,
	@object_id NVARCHAR(300)
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	DECLARE @security_object_id UNIQUEIDENTIFIER

	SELECT @security_object_id = [security_object_id]
	FROM [SecurityObject]
	WHERE [namespace_id] = @namespace_id AND @object_id = [object_id]

	IF(@security_object_id IS NULL)
	BEGIN
		SET @ErrorArgs = @procedureName + N': %%IDObject=' + @object_id + N'%%'
		;THROW 60012, @ErrorArgs, 1;
	END

	SELECT sa.[name] AS [action_name],  spd.[IDRole] AS [user_sid], CAST(0 AS BIT) AS [access_deny]
	FROM [SecurityPermissionsDevice] spd
	INNER JOIN [SecurityAction] sa ON sa.[legacy_security_name] = spd.[SecurityAttributeID]
	WHERE spd.IDDevice = @object_id

	UNION

	SELECT
		sae.[action_name],
		sae.[user_sid],
		sae.[access_deny]
	FROM [SecurityAccessEntry] sae
		INNER JOIN [SecurityAction] sa ON sa.[namespace_id] = @namespace_id AND sa.[name] = sae.[action_name]
		INNER JOIN [SecurityIdentity] si ON si.[SID] = sae.[user_sid]
	WHERE 
		sae.[security_object_id] = @security_object_id
		AND [action_name] NOT IN ( 	
		
			SELECT sa.[name]
			FROM [SecurityPermissionsDevice] spd
			INNER JOIN [SecurityAction] sa ON sa.[legacy_security_name] = spd.[SecurityAttributeID]
			WHERE spd.[IDDevice] = @object_id
		)
END
GO