-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_query_access_entries_for_objectid_all_roles]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_query_access_entries_for_objectid_all_roles]
GO

CREATE PROCEDURE [security_query_access_entries_for_objectid_all_roles]
	@namespace_id UNIQUEIDENTIFIER,
	@object_id NVARCHAR(300)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @security_object_id UNIQUEIDENTIFIER
	SET @security_object_id = (SELECT security_object_id FROM [SecurityObject] WHERE [namespace_id] = @namespace_id AND object_id = @object_id)

	SELECT spd.[IDDevice] as [object_id], sa.[name] AS [action_name], spd.[IDRole] as [role_id]
	FROM [SecurityPermissionsDevice] spd
	INNER JOIN [SecurityAction] sa ON sa.[legacy_security_name] = spd.[SecurityAttributeID]
	WHERE spd.IDDevice = @object_id

	UNION

	SELECT
	    so.object_id as [object_id],
		sae.[action_name],
		sae.[user_sid] as [role_id]
	FROM [SecurityAccessEntry] sae, [SecurityObject] so
	WHERE 
		so.security_object_id = sae.security_object_id AND
		sae.[security_object_id] = @security_object_id
		AND [action_name] NOT IN ( 	
		
			SELECT sa.[name]
			FROM [SecurityPermissionsDevice] spd
			INNER JOIN [SecurityAction] sa ON sa.[legacy_security_name] = spd.[SecurityAttributeID]
			WHERE spd.[IDDevice] = @object_id
		)
END
GO