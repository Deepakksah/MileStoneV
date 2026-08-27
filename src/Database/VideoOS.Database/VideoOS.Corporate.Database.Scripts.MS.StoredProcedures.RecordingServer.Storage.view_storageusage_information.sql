-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[StorageUsageInformation]'))
DROP VIEW [dbo].[StorageUsageInformation]
GO

CREATE VIEW [dbo].[StorageUsageInformation]
AS
	SELECT	rs.[pkid],
			rs.[name],
			CAST(bank.value('@UsedSize', 'VARCHAR(20)') AS BIGINT) AS [used_size],
			CAST(bank.value('@LockedUsedSize', 'VARCHAR(20)') AS BIGINT) AS [locked_size]
	FROM	[RecordingStorage] rs
			LEFT JOIN [RecorderStatusData] rsd ON rsd.[IDRecorder] = rs.[recordingserver_id] AND rsd.[Type] = 'StorageUsage'
			OUTER APPLY rsd.[Data].nodes('StorageUsage/Bank[upper-case(@Id) = upper-case(xs:string(sql:column("rs.pkid")))]') AS storage_info(bank)
	UNION ALL
	SELECT	ar.[pkid],
			ar.[name],
			CAST(bank.value('@UsedSize', 'VARCHAR(20)') AS BIGINT) AS [used_size],
			CAST(bank.value('@LockedUsedSize', 'VARCHAR(20)') AS BIGINT) AS [locked_size]
	FROM	[ArchiveStorage] ar
			INNER JOIN [RecordingStorage] rs ON rs.[pkid] = ar.[recordingstorage_id]
			LEFT JOIN [RecorderStatusData] rsd ON rsd.[IDRecorder] = rs.[recordingserver_id] AND rsd.[Type] = 'StorageUsage'
			OUTER APPLY rsd.[Data].nodes('StorageUsage/Bank[upper-case(@Id) = upper-case(xs:string(sql:column("ar.pkid")))]') AS storage_info(bank)
GO
