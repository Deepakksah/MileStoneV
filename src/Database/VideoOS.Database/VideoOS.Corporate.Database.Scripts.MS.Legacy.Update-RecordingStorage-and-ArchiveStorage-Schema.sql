-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_archive_storage_set_default]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_archive_storage_set_default]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_create_mediadatabase]') AND type = 'P')
DROP PROCEDURE [admin_create_mediadatabase]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_query_mediadatabase_by_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_query_mediadatabase_by_id]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_query_mediadatabase_by_name]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_query_mediadatabase_by_name]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_query_mediadatabases]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_query_mediadatabases]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_update_mediadatabase]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_update_mediadatabase]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_delete_mediadatabase]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_delete_mediadatabase]
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RecordingStorage]') AND type in (N'U'))
CREATE TABLE [RecordingStorage]
(
	[pkid] [GuidType] NOT NULL,
	[recordingserver_id] [GuidType] NOT NULL,
	[name] [NameType] NOT NULL,
	[description] NVARCHAR(MAX) NOT NULL,
	[path] NVARCHAR(512) NOT NULL,
	[is_default] BIT NOT NULL,
	[retain_minutes] INT NOT NULL,
	[max_size_mb] BIGINT NOT NULL,
	[created_date] DATETIME NOT NULL,
	[last_modified] DATETIME NOT NULL,
	
	CONSTRAINT [PK_RecordingStorage] PRIMARY KEY NONCLUSTERED
	(
		[pkid] ASC
	),
	
	CONSTRAINT [FK_RecordingStorage_Recorders] FOREIGN KEY
	(
		[recordingserver_id]
	)
	REFERENCES [Recorders] ([IDRecorder]),

	CONSTRAINT [CK_RecordingStorage_RetainMinutes] CHECK (([retain_minutes] > (0))),
	CONSTRAINT [CK_RecordingStorage_MaxSizeMB] CHECK (([max_size_mb] > (0)))
)
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[RecordingStorage]') AND name = N'IX_RecordingStorage')
CREATE UNIQUE CLUSTERED INDEX [IX_RecordingStorage] ON [RecordingStorage] 
(
	[recordingserver_id] ASC,
	[name] ASC
)
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ArchiveStorage]') AND type in (N'U'))
CREATE TABLE [ArchiveStorage]
(
	[pkid] [GuidType] NOT NULL,
	[recordingstorage_id] [GuidType] NOT NULL,
	[name] [NameType] NOT NULL,
	[description] NVARCHAR(MAX) NOT NULL,
	[path] NVARCHAR(512) NOT NULL,
	[retain_minutes] INT NOT NULL,
	[max_size_mb] BIGINT NOT NULL,
	[target_framerate] FLOAT NOT NULL,
	[schedule] XML NOT NULL,
	[created_date] DATETIME NOT NULL,
	[last_modified] DATETIME NOT NULL,

	CONSTRAINT [PK_ArchiveStorage] PRIMARY KEY NONCLUSTERED
	(
		[pkid] ASC
	),
	
	CONSTRAINT [FK_ArchiveStorage_RecordingStorage] FOREIGN KEY
	(
		[recordingstorage_id]
	)
	REFERENCES [RecordingStorage] ([pkid]),
	
	CONSTRAINT [CK_ArchiveStorage_RetainMinutes] CHECK (([retain_minutes] > (0))),
	CONSTRAINT [CK_ArchiveStorage_MaxSizeMB] CHECK (([max_size_mb] > (0))),
	CONSTRAINT [CK_ArchiveStorage_TargetFramerate] CHECK (([target_framerate] > (0)))
)
GO

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[ArchiveStorage]') AND name = N'IX_ArchiveStorage')
CREATE UNIQUE CLUSTERED INDEX [IX_ArchiveStorage] ON [ArchiveStorage] 
(
	[recordingstorage_id] ASC,
	[name] ASC
)
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RecordingStorageDevice]') AND type in (N'U'))
CREATE TABLE [RecordingStorageDevice]
(
	[recordingstorage_id] [GuidType] NOT NULL,
	[device_id] [GuidType] NOT NULL,
	[recording_enabled] BIT NOT NULL,
	[recording_framerate] FLOAT NOT NULL,
	[prebuffer_enabled] BIT NOT NULL,
	[prebuffer_seconds] INT NOT NULL,

	CONSTRAINT [PK_RecordingStorageDevice] PRIMARY KEY NONCLUSTERED
	(
		[device_id] ASC
	),
	
	CONSTRAINT [FK_RecordingStorageDevice_Devices] FOREIGN KEY
	(
		[device_id]
	)
	REFERENCES [Devices] ([IDDevice]),

	CONSTRAINT [FK_RecordingStorageDevice_RecordingStorage] FOREIGN KEY
	(
		[recordingstorage_id]
	)
	REFERENCES [RecordingStorage] ([pkid])
)
GO
