-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[query_archive_storage_by_id]') AND [type] IN (N'P', N'PC'))
DROP PROCEDURE [query_archive_storage_by_id]
GO

CREATE PROCEDURE [query_archive_storage_by_id]
	@pkid [GuidType]
 AS
BEGIN
	SET NOCOUNT ON

	SELECT
		a.[pkid],
		a.[name],
		a.[description],
		a.[path],
		a.[retain_minutes],
		a.[max_size_mb],
		a.[framerate_reduction_enabled],
		a.[target_framerate],
		a.[schedule],
		a.[created_date],
		a.[last_modified],
		a.[recordingstorage_id],
		a.[mounted],
		rs.[recordingserver_id]
	FROM [ArchiveStorage] a
	INNER JOIN [RecordingStorage] rs ON a.recordingstorage_id = rs.pkid
	WHERE a.[pkid] = @pkid
END
GO
