-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_archive_storage_ids_by_device_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_archive_storage_ids_by_device_id]
GO

CREATE PROCEDURE [query_archive_storage_ids_by_device_id]
	@device_id [GuidType]
 AS
BEGIN
	SET NOCOUNT ON

	SELECT
		a.[pkid]
	FROM [ArchiveStorage] a
	INNER JOIN [RecordingStorageDevice] rsd ON a.recordingstorage_id = rsd.[recordingstorage_id]
	WHERE rsd.[device_id] = @device_id
END
GO
