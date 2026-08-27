-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_query_non_device_role_access_entries]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_query_non_device_role_access_entries]
GO

CREATE PROCEDURE [security_query_non_device_role_access_entries]
	@namespace_id UNIQUEIDENTIFIER,
	@role_id NVARCHAR(300),
	@object_ids tvp_StringTableGuidType READONLY
AS
BEGIN
	SET NOCOUNT ON

	SELECT so.object_id as [object_id],
		sae.[action_name]
	FROM [SecurityAccessEntry] sae
	INNER JOIN [SecurityObject] so ON 
		so.security_object_id = sae.security_object_id and 
		so.[namespace_id] = @namespace_id AND 
		sae.[user_sid] = @role_id AND 
		so.[object_id] in (SELECT ID FROM  @object_ids)

END
GO