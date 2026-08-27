-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_recording_storage_by_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_recording_storage_by_id]
GO

CREATE PROCEDURE [query_recording_storage_by_id]
	@pkid [GuidType]
 AS
BEGIN
	SET NOCOUNT ON

	SELECT
		[pkid],
		[name],
		[description],
		[path],
		[is_default],
		[retain_minutes],
		[max_size_mb],
		[signing],
		[created_date],
		[encryption_method],
		[last_modified],
		[recordingserver_id]
	FROM [RecordingStorage]
	WHERE [pkid] = @pkid
END
GO
