-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_archive_storage_ids_by_recording_server_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_archive_storage_ids_by_recording_server_id]
GO

CREATE PROCEDURE [query_archive_storage_ids_by_recording_server_id]
	@recordingserver_id [GuidType]
 AS
BEGIN
	SET NOCOUNT ON

	SELECT
		a.[pkid]
	FROM [ArchiveStorage] a
	INNER JOIN [RecordingStorage] rs ON a.recordingstorage_id = rs.pkid
	WHERE rs.[recordingserver_id] = @recordingserver_id
END
GO
