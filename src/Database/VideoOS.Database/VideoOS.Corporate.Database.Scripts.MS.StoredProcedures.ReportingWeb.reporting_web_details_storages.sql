-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[reporting_web_details_storages]') AND type in (N'P', N'PC'))
DROP PROCEDURE [reporting_web_details_storages]
GO

CREATE PROCEDURE [reporting_web_details_storages]
	@IDRecorder GuidType
AS
BEGIN
	SELECT
	  storageId as StorageId,
	  ArchiveId,
	  name as StorageName,
	  path as StoragePath,
	  is_default as IsDefaultStorage,
	  max_size_mb AS StorageSize,
	  retain_minutes as RetainDays,
	  RecRetain, 
	  calculated_value AS StorageUsed,
	  max_retain,
	  instance_id
	FROM
	(
		SELECT rst.pkid as StorageId,
			   '' as ArchiveId, 
			   rst.retain_minutes/1440 as RecRetain,
			   rst.name, 
			   rst.path, 
			   rst.is_default, 
			   -1 as retain_minutes, 
			   rst.max_size_mb, 
			   0 as max_retain, 
			   dt.calculated_value,
			   pci.instance_id
		FROM RecordingStorage rst
		OUTER APPLY 
		(
			SELECT storage.value('@UsedSize', 'bigint') as calculated_value
			FROM RecordingStorage rs
			INNER JOIN RecorderStatusData rsd ON rsd.IDRecorder = rs.recordingserver_id
			CROSS APPLY rsd.Data.nodes('StorageUsage/Bank[fn:lower-case(@Id)=fn:lower-case(sql:column("rst.pkid"))]') Storages(storage)
			WHERE rs.pkid = rst.pkid
			AND rsd.Type = 'StorageUsage'
		) as dt
		OUTER APPLY 
		(
			SELECT pkid as instance_id
			FROM PerformanceCounterInstance
			WHERE category_name = 'VideoOS Recording Server Storage'
			AND counter_name = 'Used Bytes'
			AND instance_name LIKE '%' + rst.pkid + '%'
			AND is_obsolete = 0
		) as pci
		WHERE rst.recordingserver_id = @IDRecorder
		UNION
		SELECT ast.recordingstorage_id as StorageId, 
			   ast.pkid as ArchiveId, 
			   ast.retain_minutes/1440 as RecRetain, 
			   ast.name, 
			   ast.path, 
			   'false' as is_default, 
			   ast.retain_minutes,
			   ast.max_size_mb, 
			   max_retain =
				(
					select max(retain_minutes)
					from ArchiveStorage tmp
					where tmp.recordingstorage_id = ast.recordingstorage_id
				),
				dt.calculated_value,
				pci.instance_id
		FROM ArchiveStorage ast
		INNER JOIN RecordingStorage rst ON ast.recordingstorage_id = rst.pkid
		OUTER APPLY 
		(
			SELECT storage.value('@UsedSize', 'bigint') as calculated_value
			FROM RecordingStorage rs
			INNER JOIN RecorderStatusData rsd ON rsd.IDRecorder = rs.recordingserver_id
			CROSS APPLY rsd.Data.nodes('StorageUsage/Bank[fn:lower-case(@Id)=fn:lower-case(sql:column("ast.pkid"))]') Storages(storage)
			WHERE rs.pkid = ast.recordingstorage_id
			AND rsd.Type = 'StorageUsage'
		) as dt
		OUTER APPLY 
		(
			SELECT pkid as instance_id
			FROM PerformanceCounterInstance
			WHERE category_name = 'VideoOS Recording Server Storage'
			AND counter_name = 'Used Bytes'
			AND instance_name LIKE '%' + ast.pkid + '%'
			AND is_obsolete = 0
		) as pci
		WHERE rst.recordingserver_id = @IDRecorder
	) as Storages

	ORDER BY StorageId, retain_minutes
END
GO