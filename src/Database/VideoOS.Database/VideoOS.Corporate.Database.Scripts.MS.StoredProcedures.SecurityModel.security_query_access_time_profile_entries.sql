-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_query_access_time_profile_entries]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_query_access_time_profile_entries]
GO

CREATE PROCEDURE [security_query_access_time_profile_entries]
	@namespace_id UNIQUEIDENTIFIER,
	@object_ids tvp_StringTableGuidType READONLY,
	@role_id [SIDType]
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @objectids tvp_StringTableGuidType
	INSERT INTO @objectids SELECT security_object_id FROM [SecurityObject] WHERE [Object_id] in (SELECT ID from @object_ids)

	SELECT
		so.[object_id],
		sae.[action_name],
		sate.[timeprofile_id],
		sate.[is_default]
	FROM [SecurityAccessEntryTimeProfile] sate
	INNER JOIN [SecurityAccessEntry] sae ON sae.access_entry_id = sate.access_entry_id
	INNER JOIN [SecurityObject] so ON so.security_object_id = sae.security_object_id AND so.namespace_id = @namespace_id
	WHERE sae.[security_object_id] in (select ID from @objectids) AND sae.[user_sid] = @role_id

END
GO