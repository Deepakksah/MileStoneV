-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_performanceCounter_storage_instances]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_performanceCounter_storage_instances]
GO

CREATE PROCEDURE [query_performanceCounter_storage_instances]
AS	
BEGIN
	SET NOCOUNT ON

	SELECT p.pkid as CounterId, p.counter_id as StorageId, p.data_collector_id as DatacollectorId, p.path as Path, r.max_size_mb as StorageSizeMb 
	FROM PerformanceCounterInstance as  p
	JOIN RecordingStorage as r
	ON p.counter_id = r.pkid
	UNION
	SELECT p.pkid as CounterId, p.counter_id as StorageId, p.data_collector_id as DatacollectorId, p.path as Path, r.max_size_mb as StorageSizeMb
	FROM PerformanceCounterInstance as  p
	JOIN ArchiveStorage as r
	ON p.counter_id = r.pkid
END
GO