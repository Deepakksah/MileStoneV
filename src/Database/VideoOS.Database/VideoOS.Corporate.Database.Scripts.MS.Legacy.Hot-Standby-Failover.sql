-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RecordingServerService]') AND type in (N'U'))
BEGIN
	CREATE TABLE [RecordingServerService] (
		[last_modified] DATETIME NOT NULL,
		[pkid] [GuidType] NOT NULL,
		[host_name] [NameType] NOT NULL,
		[webserver_uri] NVARCHAR(MAX),
		[webserver_uri_configuration] NVARCHAR(MAX),
		[scan_ranges] NVARCHAR(MAX),
		[time_zone_name] [NameType] NOT NULL,
		[failover_udp_port] INT NULL,
		[devicepack_info] NVARCHAR(MAX),
		[x64] BIT NOT NULL,
		
		CONSTRAINT [PK_RecordingServerService] PRIMARY KEY NONCLUSTERED
		(
			[pkid] ASC
		),

		CONSTRAINT chk_failover_udp_port CHECK ([failover_udp_port]>0 AND [failover_udp_port]<65536)
	) ON [PRIMARY]

	IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RecordingServerServiceRecorder]') AND type in (N'U'))
	BEGIN
		DROP TABLE [RecordingServerServiceRecorder]
	END

	CREATE TABLE [RecordingServerServiceType] (
		[pkid] INT NOT NULL,
		[type] VARCHAR(25) NOT NULL,      -- RECORDER; FAILOVER; HOT_STANDBY

		CONSTRAINT [PK_RecordingServerServiceType] PRIMARY KEY CLUSTERED
		(
			[pkid] ASC
		)
	) ON [PRIMARY]

	CREATE TABLE [RecordingServerServiceRecorder] (
		[last_modified] DATETIME NOT NULL,
		[recording_server_service_id] [GuidType] NOT NULL,
		[recorder_id] [GuidType] NOT NULL,
		[type] INT NOT NULL,
		[active] BIT NOT NULL,

		CONSTRAINT [PK_RecordingServerServiceRecorder] PRIMARY KEY NONCLUSTERED
		(
			[recording_server_service_id] ASC
		),

		CONSTRAINT [FK_RecordingServerServiceRecorder_RecordingServerService] FOREIGN KEY
		(
			[recording_server_service_id]
		) REFERENCES [RecordingServerService] ([pkid]),

		CONSTRAINT [FK_RecordingServerServiceRecorder_Recorder] FOREIGN KEY
		(
			[recorder_id]
		) REFERENCES [Recorders] ([IDRecorder]),

		CONSTRAINT [FK_RecordingServerServiceRecorder_RecordingServerServiceType] FOREIGN KEY
		(
			[type]
		) REFERENCES [RecordingServerServiceType] ([pkid])

	) ON [PRIMARY]


	INSERT INTO [RecordingServerServiceType] ([pkid], [type]) VALUES (0, 'RECORDER')
	INSERT INTO [RecordingServerServiceType] ([pkid], [type]) VALUES (1, 'FAILOVER')
	INSERT INTO [RecordingServerServiceType] ([pkid], [type]) VALUES (2, 'HOT_STANDBY')

	INSERT INTO [RecordingServerService] ([last_modified], [pkid], [host_name], [webserver_uri], [webserver_uri_configuration], [scan_ranges], [time_zone_name], [failover_udp_port] , [devicepack_info], [x64])
		SELECT
			[LastModified], [IDRecorder], [HostName], [WebServerUri], [WebServerUriConfiguration], [ScanRanges], [TimeZoneName], [FailoverUdpPort], [DevicePackInfo], [x64]
		FROM
			[Recorders]

	INSERT INTO [RecordingServerServiceRecorder] ([last_modified], [recording_server_service_id], [recorder_id], [type], [active])
		SELECT
			[LastModified], [IDRecorder], [IDRecorder], 0, 1
		FROM
			[Recorders]

	IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FK_HardwareDriverRecorder_Recorders]') AND type in (N'F'))
	BEGIN
		ALTER TABLE [dbo].[HardwareDriverRecorder] DROP CONSTRAINT [FK_HardwareDriverRecorder_Recorders]
	END

	EXEC sp_rename N'dbo.HardwareDriverRecorder.IDRecorder', N'IDRecordingServerService', 'COLUMN' 

	IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FK_HardwareDriverRecorder_RecordingServerService]') AND type in (N'F'))
	BEGIN
		ALTER TABLE [dbo].[HardwareDriverRecorder] WITH CHECK
			ADD 
			CONSTRAINT [FK_HardwareDriverRecorder_RecordingServerService] FOREIGN KEY([IDRecordingServerService])
			REFERENCES [dbo].[RecordingServerService] ([pkid]) 
			ON DELETE CASCADE
	END


	ALTER TABLE [dbo].[Recorders] DROP CONSTRAINT [DF_DevicePackInfo]
	ALTER TABLE [dbo].[Recorders] DROP CONSTRAINT [DF_FailoverHostName]
	ALTER TABLE [dbo].[Recorders] DROP CONSTRAINT [DF_IPScanRangeWarning]
	ALTER TABLE [dbo].[Recorders] DROP CONSTRAINT [DF_Recorders_FailoverId]
	ALTER TABLE [dbo].[Recorders] DROP CONSTRAINT [DF_x64]


	ALTER TABLE [Recorders] DROP COLUMN [HostName]
	ALTER TABLE [Recorders] DROP COLUMN [WebServerUri]
	ALTER TABLE [Recorders] DROP COLUMN [WebServerUriConfiguration]
	ALTER TABLE [Recorders] DROP COLUMN [ScanRanges]
	ALTER TABLE [Recorders] DROP COLUMN [TimeZoneName]
	ALTER TABLE [Recorders] DROP COLUMN [FailoverHostName]
	ALTER TABLE [Recorders] DROP COLUMN [FailoverUdpPort]
	ALTER TABLE [Recorders] DROP COLUMN [DevicePackInfo]
	ALTER TABLE [Recorders] DROP COLUMN [FailoverId]
	ALTER TABLE [Recorders] DROP COLUMN [x64]

	ALTER TABLE [Recorders] DROP COLUMN [IPScanRangeWarning]


	IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RecorderPluginRecorder]') AND type in (N'U'))
	BEGIN
		DROP TABLE [dbo].[RecorderPluginRecorder]
	END

	ALTER TABLE [FailoverServers] ADD CONSTRAINT chk_udp_port CHECK ([UdpPort]>0 AND [UdpPort]<65536)
END
GO


-- We use IDFailoverServer as id for the failover recording server
IF EXISTS(SELECT * FROM sys.columns WHERE [object_id] = OBJECT_ID(N'[FailoverServers]') AND name = N'WebServerUri')
BEGIN
	-- Most of these values are updated by the failover recording server the
	-- first time it connects to the management server.
	INSERT [RecordingServerService] (
		[last_modified], 
		[pkid], 
		[host_name], 
		[webserver_uri], 
		[webserver_uri_configuration], 
		[scan_ranges], 
		[time_zone_name], 
		[failover_udp_port], 
		[devicepack_info], 
		[x64])	SELECT
					GETUTCDATE(),
					[IDFailoverServer],
					[HostName],
					COALESCE([WebServerUri], N'http://' + [HostName] + N':7563/'),
					COALESCE([WebServerUriConfiguration], ''),
					N'<?xml version="1.0"?><AddressIntervals />',
					N'Romance Standard Time',
					11000,
					N'<?xml version="1.0" encoding="utf-8"?><DevicePackInfo />',
					[x64]
				FROM
					[FailoverServers]

	ALTER TABLE [FailoverServers] DROP COLUMN [WebServerUri]
	ALTER TABLE [FailoverServers] DROP COLUMN [WebServerUriConfiguration]
END
GO

IF EXISTS(SELECT * FROM sys.columns WHERE [object_id] = OBJECT_ID(N'[FailoverServers]') AND name = N'IDRecordingServer')
BEGIN
	ALTER TABLE [FailoverServers] DROP COLUMN [IDRecordingServer]
END
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [object_id] = OBJECT_ID(N'[FailoverServers]') AND name = N'IDRecordingServerService')
BEGIN
	ALTER TABLE [FailoverServers] ADD [IDRecordingServerService] [GuidType] NULL
END
GO


UPDATE [FailoverServers] SET [IDRecordingServerService] = [IDFailoverServer] WHERE [IDRecordingServerService] IS NULL
GO


ALTER TABLE [FailoverServers]  ALTER COLUMN [IDRecordingServerService] [GuidType] NOT NULL
GO


-- Update current registered database tasks by inserting an
-- <idRecorder> element after the <target> element in the 
-- task xml located in data_a column in the Task table.
-- 
-- The target is already correct because we just use the
-- recorder id as service id for upgraded recorders
UPDATE [Task]
SET
	[data_a].modify('insert
						(
							element idRecorder
							{
								sql:column("target")
							}
						)
						after (task/target)[1]')
OUTPUT CAST(DELETED.data_a AS XML) AS [Before], CAST(INSERTED.data_a AS XML) AS [After]
WHERE
	[type] IN ( 'Database.TruncateDeviceStorage',
				'Database.DeleteArchive',
				'Database.DeleteStorage',
				'Database.MoveArchive',
				'Database.MoveStorage',
				'Database.MoveDeviceDb',
				'Database.SetEncryptionPassword',
				'Database.SetEncryptionMethod',
				'Database.Archiving',
				'Database.Repairing',
				'DatabaseUpgrade.UpgradeTo40MoveDevice')
GO
