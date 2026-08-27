-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_query_access_time_profile_entry]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_query_access_time_profile_entry]
GO

CREATE PROCEDURE [security_query_access_time_profile_entry]
	@namespace_id UNIQUEIDENTIFIER,
	@object_id NVARCHAR(300),
	@action_name NVARCHAR(260),
	@user_sid [SIDType]
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @security_object_id UNIQUEIDENTIFIER

	SET @security_object_id = (SELECT TOP 1 security_object_id FROM [SecurityObject] WHERE [object_id] = @object_id AND [namespace_id] = @namespace_id)

	SELECT
		sae.[action_name],
		@user_sid AS [user_sid],
		sate.[timeprofile_id],
		sate.[is_default]
	FROM [SecurityAccessEntryTimeProfile] sate
	INNER JOIN [SecurityAccessEntry] sae ON sae.access_entry_id = sate.access_entry_id
	WHERE sae.[security_object_id] = @security_object_id
		AND sae.[user_sid] = @user_sid
		AND sae.[action_name] = @action_name
END
GO