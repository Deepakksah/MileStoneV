SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[DeviceRestrictedMedia] 
AS
	SELECT	rm.[IDRestrictedMedia]
			, rmd.[IDDevice]
			, h.[IDRecorder]
			, rm.[StartTime]
			, rm.[EndTime]
	FROM	[RestrictedMedia] rm
			INNER JOIN [RestrictedMediaDevice] rmd ON rmd.[IDRestrictedMedia] = rm.[IDRestrictedMedia]
			INNER JOIN [Devices] d ON d.[IDDevice] = rmd.[IDDevice]
			INNER JOIN [Hardware] h ON h.[IDHardware] = d.[IDHardware]
	UNION ALL
	SELECT	rml.[IDDevice]
			, rml.[IDDevice]
			, h.[IDRecorder]
			, rml.[StartTime]
			, NULL
	FROM	[RestrictedMediaLive] rml
			INNER JOIN [Devices] d ON d.[IDDevice] = rml.[IDDevice]
			INNER JOIN [Hardware] h ON h.[IDHardware] = d.[IDHardware]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[DeviceTrackEvidenceLock] 
AS
SELECT 
	md.[IDMarkedData],
	mdd.[IDDevice],
	dt.[IDTrack],
	md.[StartTime], 
	md.[EndTime], 
	h.[IDRecorder]
FROM [MarkedData] md
	INNER JOIN [MarkedDataDevices] mdd ON md.[IDMarkedData] = mdd.[IDMarkedData]
	INNER JOIN [Devices] d ON mdd.[IDDevice] = d.[IDDevice]
	INNER JOIN [DeviceTrack] dt ON mdd.[IDDevice] = dt.[IDDevice]
	INNER JOIN [Hardware] h ON h.[IDHardware] = d.[IDHardware]
WHERE md.[MarkDataType] = 2
UNION ALL
SELECT 
	md.[IDMarkedData],
	mdd.[IDDevice],
	CAST(ddd.[track_id] AS UNIQUEIDENTIFIER) AS IDTrack,
	md.[StartTime], 
	md.[EndTime], 
	ddd.[recorder_id] AS IDRecorder
FROM [MarkedData] md
	INNER JOIN [MarkedDataDevices] mdd ON md.[IDMarkedData] = mdd.[IDMarkedData]
	INNER JOIN [DeviceDataDistribution] ddd ON mdd.[IDDevice] = ddd.[device_id]
WHERE md.[MarkDataType] = 2

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [DeviceTrackInfo]
AS
  SELECT	CAST(DD.[recorder_id] AS UNIQUEIDENTIFIER) AS [IDRecorder],
			CAST(DD.[track_id] AS UNIQUEIDENTIFIER) AS [IDTrack],
			CAST(DD.[device_id] AS UNIQUEIDENTIFIER) AS [IDDevice],
			D.[Name],
			D.[DeviceType],
			DT.[IDTrackDefinition]
	FROM	[DeviceDataDistribution] DD
			INNER JOIN [Devices] D ON D.[IDDevice] = DD.[device_id]
			INNER JOIN [DeviceTrack] DT ON DT.[IDTrack] = DD.[track_id]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[FailoverInfo]
AS
	SELECT
		rssr.[recording_server_service_id]
		, rss.[host_name]
		, rss.[database_port]
	FROM
		[RecordingServerServiceRecorder] rssr
		INNER JOIN [RecordingServerService] rss ON rss.[pkid] = rssr.[recording_server_service_id]
	WHERE
		rssr.[type] <> 0

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[RecorderInfo]
AS
SELECT
	rssr.[recorder_id]
	, rssr.[recording_server_service_id]
	, rss.[host_name]
	, rss.[database_port]
FROM
	[RecordingServerServiceRecorder] rssr
	INNER JOIN [RecordingServerService] rss ON rss.[pkid] = rssr.[recording_server_service_id]
WHERE
	rssr.[type] = 0

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [RoleMembership]
AS
	SELECT rm.[IDRole], rm.[MemberSID]
	FROM dbo.udfRoleMembers() rm

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[SecurityGroupAccessEntry]
AS
SELECT
	so.[object_id],
	sn.[namespace_id],
	sae.[action_name],
	sae.[user_sid],
	sae.[access_deny]
FROM [SecurityAccessEntry] sae
	INNER JOIN [SecurityAction] sa ON sa.[name] = sae.[action_name]
	INNER JOIN [SecurityNamespace] sn ON sn.[namespace_id] = sa.[namespace_id]
	INNER JOIN [SecurityIdentity] si ON si.[SID] = sae.[user_sid] AND si.[Deleted] = 0
	INNER JOIN [SecurityObject] so ON so.[namespace_id] = sn.[namespace_id] AND so.[security_object_id] = sae.[security_object_id] AND so.[object_id] = '/'

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW TaskProperties
AS
SELECT	t.[pkid] AS TaskId,
		t.[type] AS TaskType,
		TaskPropertyName = CASE tp.value('local-name(.)', 'VARCHAR(200)')
		WHEN 'idStorage' THEN 'storage_id'			-- Unifying storage id property names, for simpler queries when using this view
		WHEN 'idStorageTarget' THEN 'storage_id'
		WHEN 'idDevice' THEN 'device_id'			-- Unifying device id property names, for simpler queries when using this view
		ELSE tp.value('local-name(.)', 'VARCHAR(200)')
		END,
		tp.value('.' , 'VARCHAR(200)') as TaskPropertyValue
FROM	[Task] t WITH(UPDLOCK, HOLDLOCK)
		-- Cross apply all child nodes of the task xml definition (data_a column of Task table)
		CROSS APPLY t.[data_a].nodes('/task/*') MyTaskProperties(tp)
WHERE   t.[type] NOT IN ('Database.Archiving', 'Database.Repairing2')	-- These are never considered conflicting tasks anyway
		AND t.[type] LIKE 'Database.%'									-- Currently we are only using this table with database tasks
		AND t.[task_status] NOT IN (2, 3)								-- Ignore tasks that are Succeeded (2) or Failed (3)

GO
