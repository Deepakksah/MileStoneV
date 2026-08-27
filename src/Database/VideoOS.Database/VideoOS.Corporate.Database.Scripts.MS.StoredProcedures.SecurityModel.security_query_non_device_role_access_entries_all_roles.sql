-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_query_non_device_role_access_entries_all_roles]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_query_non_device_role_access_entries_all_roles]
GO

CREATE PROCEDURE [security_query_non_device_role_access_entries_all_roles]
	@namespace_id UNIQUEIDENTIFIER,
	@object_id NVARCHAR(300)
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	SELECT so.object_id as [object_id],
		sae.[action_name],
		sae.[user_sid] as [role_id]
	FROM [SecurityAccessEntry] sae
	INNER JOIN [SecurityObject] so ON 
		so.security_object_id = sae.security_object_id and 
		so.[namespace_id] = @namespace_id AND 
		so.[object_id] = @object_id

END
GO