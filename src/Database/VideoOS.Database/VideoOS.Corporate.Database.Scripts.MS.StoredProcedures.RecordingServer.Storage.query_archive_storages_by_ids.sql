-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[query_archive_storages_by_ids]') AND [type] IN (N'P', N'PC'))
DROP PROCEDURE [query_archive_storages_by_ids]
GO

CREATE PROCEDURE [query_archive_storages_by_ids]
	@archive_storage_ids tvp_StringTableGuidType READONLY
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
	INNER JOIN @archive_storage_ids ids ON a.[pkid] = ids.[ID]
END
GO
