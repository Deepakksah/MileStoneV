-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[reporting_web_details_disks]') AND type in (N'P', N'PC'))
DROP PROCEDURE [reporting_web_details_disks]
GO

CREATE PROCEDURE [reporting_web_details_disks]
	@IDRecorder GuidType
AS
BEGIN
	DECLARE @CollectorId INT
	SET @CollectorId = 
	(
		SELECT dc.[pkid]
		FROM [DataCollector] dc 
		INNER JOIN [DataCollectorInstalledServices] dis ON dc.[pkid] = dis.[data_collector_id]
		INNER JOIN [RecordingServerService] rss ON rss.[host_name] = dc.[host_name]
		INNER JOIN [RecordingServerServiceRecorder] rssr ON rss.[pkid] = rssr.[recording_server_service_id] AND rssr.[type] = 0
		WHERE dis.[service_name] = 'Recorder'
		AND rssr.[recorder_id] = @IDRecorder
	)

	SELECT DriveName, 
		mdu AS MediaDataUsed, 
		MediaDataUsedInstanceId,
		odu AS OtherDataUsed,
		DiskOtherDataInstanceId,
		fd AS FreeData,
		DiskFreeBytesInstanceId,
		ts AS TotalSize,
		DiskWriteSpeedInstanceId,
		DiskWriteSpeedInstanceName,
		DiskReadSpeedInstanceId,
		DiskReadSpeedInstanceName
	FROM
	(
		SELECT
			disk.value('@Path', 'nvarchar(100)') as DriveName,
			disk.value('@MediaData', 'bigint') as mdu,
			disk.value('@OtherData', 'bigint') as odu,
			disk.value('@Free', 'bigint') as fd,
			disk.value('@Size', 'bigint') as ts
		FROM [RecorderStatusData] rsd
		CROSS apply rsd.Data.nodes('DiskUsage/Disk') Disks(disk)
		WHERE rsd.Type = 'DiskUsage' AND IDRecorder = @IDRecorder
	) as DT
	OUTER APPLY
	(
			SELECT pkid as MediaDataUsedInstanceId
			FROM PerformanceCounterInstance
			WHERE data_collector_id = @CollectorId
			AND category_name = 'VideoOS Recording Server Database Disk'
			AND counter_name = 'Media Data Bytes'
			AND instance_name LIKE '%' + @IDRecorder + '%'
			AND instance_name LIKE '%'+ dbo.EscapeWildcards(DT.DriveName)+ '%' ESCAPE '\'
			AND is_obsolete = 0
	) as MediaDataUsedInstanceIdDT
	OUTER APPLY
	(
			SELECT pkid as DiskFreeBytesInstanceId
			FROM PerformanceCounterInstance
			WHERE data_collector_id = @CollectorId
			AND category_name = 'VideoOS Recording Server Database Disk'
			AND counter_name = 'Free Bytes'
			AND instance_name LIKE '%' + @IDRecorder + '%'
			AND instance_name LIKE '%'+ dbo.EscapeWildcards(DT.DriveName)+ '%' ESCAPE '\'
			AND is_obsolete = 0
	) as DiskFreeBytesInstanceIdDT
	OUTER APPLY
	(
			SELECT pkid as DiskOtherDataInstanceId
			FROM PerformanceCounterInstance
			WHERE data_collector_id = @CollectorId
			AND category_name = 'VideoOS Recording Server Database Disk'
			AND counter_name = 'Other Data Bytes'
			AND instance_name LIKE '%' + @IDRecorder + '%'
			AND instance_name LIKE '%'+ dbo.EscapeWildcards(DT.DriveName)+ '%' ESCAPE '\'
			AND is_obsolete = 0
	) as DiskOtherDataInstanceIdDT
	OUTER APPLY
	(
			SELECT pkid as DiskWriteSpeedInstanceId,
			instance_name as DiskWriteSpeedInstanceName
			FROM PerformanceCounterInstance
			WHERE data_collector_id = @CollectorId
			AND category_name = 'LogicalDisk'
			AND counter_name = 'Disk Write Bytes/sec'
			AND instance_name LIKE '%'+ dbo.EscapeWildcards(DT.DriveName)+ '%' ESCAPE '\'
			AND is_obsolete = 0
	) as DiskWriteSpeedInstanceIdDT
	OUTER APPLY
	(
			SELECT pkid as DiskReadSpeedInstanceId,
			instance_name as DiskReadSpeedInstanceName
			FROM PerformanceCounterInstance
			WHERE data_collector_id = @CollectorId
			AND category_name = 'LogicalDisk'
			AND counter_name = 'Disk Read Bytes/sec'
			AND instance_name LIKE '%'+ dbo.EscapeWildcards(DT.DriveName)+ '%' ESCAPE '\'
			AND is_obsolete = 0
	) as DiskReadSpeedInstanceIdDT
END
GO