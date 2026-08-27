-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_query_access_time_profile_entries_all_roles]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_query_access_time_profile_entries_all_roles]
GO

CREATE PROCEDURE [security_query_access_time_profile_entries_all_roles]
	@namespace_id UNIQUEIDENTIFIER,
	@object_id NVARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @security_object_id UNIQUEIDENTIFIER
	SET @security_object_id = (SELECT security_object_id FROM [SecurityObject] WHERE [namespace_id] = @namespace_id AND object_id = @object_id)

	SELECT
		sae.[action_name],
		sate.[timeprofile_id],
		sate.[is_default],
		sae.[user_sid] as [role_id]
	FROM [SecurityAccessEntryTimeProfile] sate
	INNER JOIN [SecurityAccessEntry] sae ON sae.access_entry_id = sate.access_entry_id
	WHERE sae.[security_object_id] = @security_object_id

END
GO