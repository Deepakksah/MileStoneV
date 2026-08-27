-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_default_storage_id_by_recording_server]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_default_storage_id_by_recording_server]
GO

CREATE PROCEDURE [query_default_storage_id_by_recording_server]
	@recording_server_id [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		rs.[pkid]
	FROM
		[RecordingStorage] rs
	WHERE
		rs.[is_default] = 1
		AND rs.[recordingserver_id] = @recording_server_id
END
GO
