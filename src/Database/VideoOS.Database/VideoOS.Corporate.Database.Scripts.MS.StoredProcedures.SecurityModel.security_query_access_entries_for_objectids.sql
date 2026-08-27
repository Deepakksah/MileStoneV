-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_query_access_entries_for_objectids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_query_access_entries_for_objectids]
GO

CREATE PROCEDURE [security_query_access_entries_for_objectids]
	@namespace_id UNIQUEIDENTIFIER,
	@role_id NVARCHAR(300),
	@object_ids tvp_StringTableGuidType READONLY
AS
BEGIN
	SET NOCOUNT ON

	SELECT spd.[IDDevice] as [object_id], sa.[name] AS [action_name]
	FROM [SecurityPermissionsDevice] spd
	INNER JOIN [SecurityAction] sa ON sa.[legacy_security_name] = spd.[SecurityAttributeID]
	WHERE spd.IDDevice in (SELECT ID FROM @object_ids) AND spd.[IDRole] = @role_id

	UNION

	SELECT
	    so.object_id as [object_id],
		sae.[action_name]
	FROM [SecurityAccessEntry] sae, [SecurityObject] so
	WHERE 
		sae.[user_sid] = @role_id AND
		so.security_object_id = sae.security_object_id AND
		sae.[security_object_id] IN (SELECT [security_object_id] FROM [SecurityObject] WHERE [namespace_id] = @namespace_id AND object_id in (SELECT ID FROM  @object_ids))
		AND [action_name] NOT IN ( 	
		
			SELECT sa.[name]
			FROM [SecurityPermissionsDevice] spd
			INNER JOIN [SecurityAction] sa ON sa.[legacy_security_name] = spd.[SecurityAttributeID]
			WHERE spd.[IDDevice] in (SELECT ID FROM @object_ids)
		)
END
GO