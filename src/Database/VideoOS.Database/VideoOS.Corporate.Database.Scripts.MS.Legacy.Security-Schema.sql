-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[TempSecurityAccessEntry]') AND type in (N'U'))
DROP TABLE [TempSecurityAccessEntry]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SecurityAccessCache2]') AND type in (N'U'))
DROP TABLE [SecurityAccessCache2]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SecurityAccessEntry]') AND type in (N'U'))
DROP TABLE [SecurityAccessEntry]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SecurityAccessEntryTimeProfile]') AND type in (N'U'))
DROP TABLE [SecurityAccessEntryTimeProfile]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SecurityObject]') AND type in (N'U'))
DROP TABLE [SecurityObject]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SecurityAction]') AND type in (N'U'))
DROP TABLE [SecurityAction]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SecurityNamespace]') AND type in (N'U'))
DROP TABLE [SecurityNamespace]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SecurityGroup]') AND type in (N'U'))
DROP TABLE [SecurityGroup]
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SecurityNamespace]') AND type in (N'U'))
CREATE TABLE [SecurityNamespace]
(
	[namespace_id] UNIQUEIDENTIFIER NOT NULL,
	[name] NVARCHAR(260) NOT NULL,
	[display_name] NVARCHAR(260) NOT NULL,
	[separator] CHAR(1) NOT NULL,
	[structure] INT NOT NULL, -- 0: Flat, 1: Hierachial
	[order_no] INT NOT NULL,
	[user_defined] BIT NOT NULL,

	CONSTRAINT [PK_namespace_id] PRIMARY KEY NONCLUSTERED ( [namespace_id] ASC ),
	CONSTRAINT [IX_name] UNIQUE CLUSTERED ( [name] ASC )
)
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SecurityAction]') AND type in (N'U'))
CREATE TABLE [SecurityAction]
(
	[namespace_id] UNIQUEIDENTIFIER NOT NULL,
	[name] NVARCHAR(260) NOT NULL,
	[display_name] NVARCHAR(260) NOT NULL,
	[order_no] INT NOT NULL,
	[visible] BIT NOT NULL,

	CONSTRAINT [PK_namespace_id_name] PRIMARY KEY CLUSTERED ( [namespace_id] ASC, [name] ASC ),

	CONSTRAINT [FK_SecurityNamespace_SecurityAction] FOREIGN KEY ( [namespace_id] )
		REFERENCES [SecurityNamespace] ( [namespace_id] )
)
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SecurityObject]') AND type in (N'U'))
CREATE TABLE [SecurityObject]
(
	[namespace_id] UNIQUEIDENTIFIER NOT NULL,
	[parent_path] NVARCHAR(400) NOT NULL,
	[object_id] NVARCHAR(300) NOT NULL,
	[object_type] TINYINT NOT NULL,
	[object_name] NVARCHAR(100) NULL,
	[security_object_id] UNIQUEIDENTIFIER NOT NULL,
	[inherits_from_id] NVARCHAR(300) NULL,
	[full_path] AS ( [parent_path] + [object_id]),

	CONSTRAINT [PK_security_object_id] PRIMARY KEY NONCLUSTERED ( [security_object_id] ASC),
	CONSTRAINT [IX_namespace_id_object_id] UNIQUE CLUSTERED ( [namespace_id] ASC, [parent_path] ASC, [object_id] ASC ),

	CONSTRAINT [FK_SecurityNamespace_SecurityObject] FOREIGN KEY ( [namespace_id] )
		REFERENCES [SecurityNamespace] ( [namespace_id] )
)

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SecurityAccessEntry]') AND type in (N'U'))
CREATE TABLE [SecurityAccessEntry]
(
	[access_entry_id] INT IDENTITY(1,1) NOT NULL,
	[security_object_id] UNIQUEIDENTIFIER NOT NULL,
	[user_sid] [SIDType] NOT NULL,
	[action_name] NVARCHAR(260) NOT NULL,
	[access_deny] BIT NOT NULL,
	[deleted] BIT NOT NULL,

	CONSTRAINT [PK_access_entry_id] PRIMARY KEY CLUSTERED ( [access_entry_id] ASC),

	CONSTRAINT [FK_SecurityObject_SecurityObject] FOREIGN KEY ( [security_object_id] )
		REFERENCES [SecurityObject] ( [security_object_id] ),

	CONSTRAINT [FK_SecurityObject_SecurityIdentity] FOREIGN KEY ( [user_sid] )
		REFERENCES [SecurityIdentity] ( [SID] )
)

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SecurityAccessEntryTimeProfile]') AND type in (N'U'))
CREATE TABLE [SecurityAccessEntryTimeProfile]
(
	[access_entry_id] INT NOT NULL,
	[timeprofile_id] UNIQUEIDENTIFIER NOT NULL,
	[is_default] BIT NOT NULL,

	CONSTRAINT [PK_timeprofile_access_entry_id] PRIMARY KEY CLUSTERED ( [access_entry_id] ASC),

	CONSTRAINT [FK_SecurityAccessEntry_TimeProfile] FOREIGN KEY ( [access_entry_id] )
		REFERENCES [SecurityAccessEntryTimeProfile] ( [access_entry_id] )
)

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[TempSecurityAccessEntry]') AND type in (N'U'))
CREATE TABLE [TempSecurityAccessEntry]
(
	[SPID] INT NOT NULL,
	[user_sid] [SIDType] NOT NULL,
	[action_name] NVARCHAR(260) NOT NULL,
	[access_deny] BIT NOT NULL,
	[change] TINYINT
)

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SecurityAccessCache2]') AND type in (N'U'))
CREATE TABLE [SecurityAccessCache2]
(
	[namespace_id] UNIQUEIDENTIFIER NOT NULL,
	[user_sid] [SidType] NOT NULL,
	[action_name] NVARCHAR(260) NOT NULL,
	[object_id] NVARCHAR(300) NOT NULL,
	[allowed_access] BIT NOT NULL
)

IF NOT EXISTS(SELECT * FROM [SecurityNamespace])
INSERT [SecurityNamespace] ([namespace_id], [name], [display_name], [separator], [structure], [order_no], [user_defined])
SELECT 'E9914E39-A67A-496C-A3C1-5B4AAAB28F15', 'ManagementServer', 'Management Server', '/', 0, 1, 0 UNION ALL
SELECT '9710735D-EB10-44EA-888D-FA8AB05A6433', 'RecordingServer', 'Recording Server', '/', 1, 2, 0 UNION ALL
SELECT '6339122C-04F5-41F2-B3E3-3555997CFCEF', 'FailoverServer', 'Failover Server', '/', 1, 3, 0 UNION ALL
SELECT 'F97772D8-7B6D-4EA4-B6B5-EE4EA010FA2C', 'Hardware', 'Hardware', '/', 1, 4, 0 UNION ALL
SELECT '623D03F8-C5D5-46BC-A2F4-4C03562D4F85', 'Camera', 'Cameras', '/', 1, 5, 0 UNION ALL
SELECT '15F48F88-CA89-4926-9A84-2B02864EC77A', 'Microphone', 'Microphones', '/', 1, 6, 0 UNION ALL
SELECT '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD', 'Speaker', 'Speakers', '/', 1, 7, 0 UNION ALL
SELECT 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D', 'Metadata', 'Metadata', '/', 1, 8, 0 UNION ALL
SELECT '3ED79EF0-EE2F-40B8-A49A-17E9380C5932', 'Input', 'Inputs', '/', 1, 9, 0 UNION ALL
SELECT 'A95FCE90-753D-4CB7-86FB-F8CAD9F93B67', 'Output', 'Outputs', '/', 1, 10, 0 UNION ALL
SELECT '8732FCAC-A63C-46EA-9683-532B4C6644E7', 'SmartWall', 'Smart Walls', '/', 1, 11, 0 UNION ALL
SELECT 'EC543C56-ADF7-4600-B9AD-97E166552BE9', 'ViewGroup', 'View Groups', '/', 1, 12, 0 UNION ALL
SELECT 'D4FF31CD-D9AC-46DF-8E92-5B545D191BAF', 'UserDefinedEvent', 'User-defined Events', '/', 1, 13, 0 UNION ALL
SELECT '988D493D-1597-4BE6-AFF0-396BFD5D047A', 'Matrix', 'Matrix', '/', 1, 14, 0 UNION ALL
SELECT 'C24C91A7-7D89-459B-B78E-C4548C74640D', 'Rule', 'Rules', '/', 1, 15, 0
GO

IF NOT EXISTS(SELECT * FROM [SecurityAction])
BEGIN
	-- Management Server Actions
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible])
	SELECT 'E9914E39-A67A-496C-A3C1-5B4AAAB28F15', 'GENERIC_READ', 'Read server-level information', 0, 0 UNION ALL
	SELECT 'E9914E39-A67A-496C-A3C1-5B4AAAB28F15', 'GENERIC_WRITE', 'Edit server-level information', 1, 0 UNION ALL
	SELECT 'E9914E39-A67A-496C-A3C1-5B4AAAB28F15', 'EXEC_CONFIG_REPORT', 'Execute configuration reports', 2, 0 UNION ALL
	SELECT 'E9914E39-A67A-496C-A3C1-5B4AAAB28F15', 'STATUS_API', 'Status API', 3, 1 UNION ALL
	SELECT 'E9914E39-A67A-496C-A3C1-5B4AAAB28F15', 'ADMIN_FEDERATED_HIERARCHY', 'Administrate Federated site hierarchy', 4, 0 UNION ALL
	SELECT 'E9914E39-A67A-496C-A3C1-5B4AAAB28F15', 'BACKUP_CONFIGURATION', 'Backup Configuration', 5, 0 UNION ALL
	SELECT 'E9914E39-A67A-496C-A3C1-5B4AAAB28F15', 'ADMIN_SECURITY', 'Administrate security', 7, 0

	-- Recording Server Actions
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible])
	SELECT '9710735D-EB10-44EA-888D-FA8AB05A6433', 'GENERIC_READ', 'Read', 0, 0 UNION ALL
	SELECT '9710735D-EB10-44EA-888D-FA8AB05A6433', 'GENERIC_WRITE', 'Edit', 1, 0 UNION ALL
	SELECT '9710735D-EB10-44EA-888D-FA8AB05A6433', 'DELETE', 'Delete', 2, 0 UNION ALL
	SELECT '9710735D-EB10-44EA-888D-FA8AB05A6433', 'ADMIN_HARDWARE', 'Administrate hardware', 3, 0 UNION ALL
	SELECT '9710735D-EB10-44EA-888D-FA8AB05A6433', 'ADMIN_STORAGE', 'Administrate storage', 4, 0 UNION ALL
	SELECT '9710735D-EB10-44EA-888D-FA8AB05A6433', 'AUTHORIZE_RECORDING_SERVER', 'Authorize recording server', 5, 0 UNION ALL
	SELECT '9710735D-EB10-44EA-888D-FA8AB05A6433', 'ADMIN_SECURITY', 'Administrate security', 6, 0

	-- Failover Server Actions
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible])
	SELECT '6339122C-04F5-41F2-B3E3-3555997CFCEF', 'GENERIC_READ', 'Read', 0, 0 UNION ALL
	SELECT '6339122C-04F5-41F2-B3E3-3555997CFCEF', 'GENERIC_WRITE', 'Edit', 1, 0 UNION ALL
	SELECT '6339122C-04F5-41F2-B3E3-3555997CFCEF', 'ADMIN_SECURITY', 'Administrate security', 2, 0

	-- Hardware Actions
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible])
	SELECT 'F97772D8-7B6D-4EA4-B6B5-EE4EA010FA2C', 'GENERIC_READ', 'Read', 0, 0 UNION ALL
	SELECT 'F97772D8-7B6D-4EA4-B6B5-EE4EA010FA2C', 'GENERIC_WRITE', 'Edit', 1, 0 UNION ALL
	SELECT 'F97772D8-7B6D-4EA4-B6B5-EE4EA010FA2C', 'DELETE', 'Delete', 2, 0 UNION ALL
	SELECT 'F97772D8-7B6D-4EA4-B6B5-EE4EA010FA2C', 'ADD_HARDWARE', 'Add hardware', 3, 0 UNION ALL
	SELECT 'F97772D8-7B6D-4EA4-B6B5-EE4EA010FA2C', 'ADMIN_SECURITY', 'Administrate security', 4, 0

	-- Camera Actions
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible])
	SELECT '623D03F8-C5D5-46BC-A2F4-4C03562D4F85', 'GENERIC_READ', 'Read', 0, 1 UNION ALL
	SELECT '623D03F8-C5D5-46BC-A2F4-4C03562D4F85', 'GENERIC_WRITE', 'Edit', 1, 0 UNION ALL
	SELECT '623D03F8-C5D5-46BC-A2F4-4C03562D4F85', 'VIEW_LIVE', 'View Live', 2, 1 UNION ALL
	SELECT '623D03F8-C5D5-46BC-A2F4-4C03562D4F85', 'PLAYBACK', 'Playback', 3, 1 UNION ALL
	SELECT '623D03F8-C5D5-46BC-A2F4-4C03562D4F85', 'READ_REMOTE_RECORDINGS', 'Read remote recordings', 4, 1 UNION ALL
	SELECT '623D03F8-C5D5-46BC-A2F4-4C03562D4F85', 'READ_SEQUENCES', 'Read sequences', 5, 1 UNION ALL
	SELECT '623D03F8-C5D5-46BC-A2F4-4C03562D4F85', 'SMARTSEARCH', 'Smart search', 6, 1 UNION ALL
	SELECT '623D03F8-C5D5-46BC-A2F4-4C03562D4F85', 'EXPORT', 'Export', 7, 1 UNION ALL
	SELECT '623D03F8-C5D5-46BC-A2F4-4C03562D4F85', 'CREATE_BOOKMARKS', 'Create bookmarks', 8, 1 UNION ALL
	SELECT '623D03F8-C5D5-46BC-A2F4-4C03562D4F85', 'READ_BOOKMARKS', 'Read bookmarks', 9, 1 UNION ALL
	SELECT '623D03F8-C5D5-46BC-A2F4-4C03562D4F85', 'EDIT_BOOKMARKS', 'Edit bookmarks', 10, 1 UNION ALL
	SELECT '623D03F8-C5D5-46BC-A2F4-4C03562D4F85', 'DELETE_BOOKMARKS', 'Delete bookmarks', 11, 1 UNION ALL
	SELECT '623D03F8-C5D5-46BC-A2F4-4C03562D4F85', 'CREATE_EVIDENCE_LOCK', 'Create evidence lock', 12, 1 UNION ALL
	SELECT '623D03F8-C5D5-46BC-A2F4-4C03562D4F85', 'READ_EVIDENCE_LOCK', 'Read evidence lock', 13, 1 UNION ALL
	SELECT '623D03F8-C5D5-46BC-A2F4-4C03562D4F85', 'DELETE_EVIDENCE_LOCK', 'Delete evidence lock', 14, 1 UNION ALL
	SELECT '623D03F8-C5D5-46BC-A2F4-4C03562D4F85', 'START_MANUAL_RECORDING', 'Start manual recording', 15, 1 UNION ALL
	SELECT '623D03F8-C5D5-46BC-A2F4-4C03562D4F85', 'STOP_MANUAL_RECORDING', 'Stop manual recording', 16, 1 UNION ALL
	SELECT '623D03F8-C5D5-46BC-A2F4-4C03562D4F85', 'AUX_COMMANDS', 'AUX commands', 17, 1 UNION ALL
	SELECT '623D03F8-C5D5-46BC-A2F4-4C03562D4F85', 'PTZ_CONTROL', 'PTZ control', 18, 1 UNION ALL
	SELECT '623D03F8-C5D5-46BC-A2F4-4C03562D4F85', 'ACTIVATE_PTZ_PRESET', 'Activate PTZ preset', 19, 1 UNION ALL
	SELECT '623D03F8-C5D5-46BC-A2F4-4C03562D4F85', 'DELETE_RECORDINGS', 'Delete recordings', 20, 1 UNION ALL
	SELECT '623D03F8-C5D5-46BC-A2F4-4C03562D4F85', 'ADMIN_SECURITY', 'Administrate security', 21, 0
	
	-- Microphone Actions
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible])
	SELECT '15F48F88-CA89-4926-9A84-2B02864EC77A', 'GENERIC_READ', 'Read', 0, 1 UNION ALL
	SELECT '15F48F88-CA89-4926-9A84-2B02864EC77A', 'GENERIC_WRITE', 'Edit', 1, 0 UNION ALL
	SELECT '15F48F88-CA89-4926-9A84-2B02864EC77A', 'LISTEN', 'Listen', 2, 1 UNION ALL
	SELECT '15F48F88-CA89-4926-9A84-2B02864EC77A', 'PLAYBACK', 'Playback', 3, 1 UNION ALL
	SELECT '15F48F88-CA89-4926-9A84-2B02864EC77A', 'READ_SEQUENCES', 'Read sequences', 4, 1 UNION ALL
	SELECT '15F48F88-CA89-4926-9A84-2B02864EC77A', 'EXPORT', 'Export', 5, 1 UNION ALL
	SELECT '15F48F88-CA89-4926-9A84-2B02864EC77A', 'CREATE_BOOKMARKS', 'Create bookmarks', 6, 1 UNION ALL
	SELECT '15F48F88-CA89-4926-9A84-2B02864EC77A', 'READ_BOOKMARKS', 'Read bookmarks', 7, 1 UNION ALL
	SELECT '15F48F88-CA89-4926-9A84-2B02864EC77A', 'EDIT_BOOKMARKS', 'Edit bookmarks', 8, 1 UNION ALL
	SELECT '15F48F88-CA89-4926-9A84-2B02864EC77A', 'DELETE_BOOKMARKS', 'Delete bookmarks', 9, 1 UNION ALL
	SELECT '15F48F88-CA89-4926-9A84-2B02864EC77A', 'CREATE_EVIDENCE_LOCK', 'Create evidence lock', 10, 1 UNION ALL
	SELECT '15F48F88-CA89-4926-9A84-2B02864EC77A', 'READ_EVIDENCE_LOCK', 'Read evidence lock', 11, 1 UNION ALL
	SELECT '15F48F88-CA89-4926-9A84-2B02864EC77A', 'DELETE_EVIDENCE_LOCK', 'Delete evidence lock', 12, 1 UNION ALL
	SELECT '15F48F88-CA89-4926-9A84-2B02864EC77A', 'START_MANUAL_RECORDING', 'Start manual recording', 13, 1 UNION ALL
	SELECT '15F48F88-CA89-4926-9A84-2B02864EC77A', 'STOP_MANUAL_RECORDING', 'Stop manual recording', 14, 1 UNION ALL
	SELECT '15F48F88-CA89-4926-9A84-2B02864EC77A', 'DELETE_RECORDINGS', 'Delete recordings', 15, 1 UNION ALL
	SELECT '15F48F88-CA89-4926-9A84-2B02864EC77A', 'ADMIN_SECURITY', 'Administrate security', 16, 0

	-- Speaker Actions
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible])
	SELECT '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD', 'GENERIC_READ', 'Read', 0, 1 UNION ALL
	SELECT '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD', 'GENERIC_WRITE', 'Edit', 1, 0 UNION ALL
	SELECT '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD', 'LISTEN', 'Listen', 2, 1 UNION ALL
	SELECT '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD', 'SPEAK', 'Speak', 3, 1 UNION ALL
	SELECT '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD', 'PLAYBACK', 'Playback', 4, 1 UNION ALL
	SELECT '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD', 'READ_SEQUENCES', 'Read sequences', 5, 1 UNION ALL
	SELECT '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD', 'EXPORT', 'Export', 6, 1 UNION ALL
	SELECT '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD', 'CREATE_BOOKMARKS', 'Create bookmarks', 7, 1 UNION ALL
	SELECT '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD', 'READ_BOOKMARKS', 'Read bookmarks', 8, 1 UNION ALL
	SELECT '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD', 'EDIT_BOOKMARKS', 'Edit bookmarks', 9, 1 UNION ALL
	SELECT '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD', 'DELETE_BOOKMARKS', 'Delete bookmarks', 10, 1 UNION ALL
	SELECT '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD', 'CREATE_EVIDENCE_LOCK', 'Create evidence lock', 11, 1 UNION ALL
	SELECT '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD', 'READ_EVIDENCE_LOCK', 'Read evidence lock', 12, 1 UNION ALL
	SELECT '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD', 'DELETE_EVIDENCE_LOCK', 'Delete evidence lock', 13, 1 UNION ALL
	SELECT '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD', 'START_MANUAL_RECORDING', 'Start manual recording', 14, 1 UNION ALL
	SELECT '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD', 'STOP_MANUAL_RECORDING', 'Stop manual recording', 15, 1 UNION ALL
	SELECT '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD', 'DELETE_RECORDINGS', 'Delete recordings', 16, 1 UNION ALL
	SELECT '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD', 'ADMIN_SECURITY', 'Administrate security', 17, 0

	-- Metadata Actions
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible])
	SELECT 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D', 'GENERIC_READ', 'Read', 0, 1 UNION ALL
	SELECT 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D', 'GENERIC_WRITE', 'Edit', 1, 0 UNION ALL
	SELECT 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D', 'LISTEN', 'Listen', 2, 1 UNION ALL
	SELECT 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D', 'SPEAK', 'Speak', 3, 1 UNION ALL
	SELECT 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D', 'PLAYBACK', 'Playback', 4, 1 UNION ALL
	SELECT 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D', 'READ_SEQUENCES', 'Read sequences', 5, 1 UNION ALL
	SELECT 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D', 'EXPORT', 'Export', 6, 1 UNION ALL
	SELECT 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D', 'CREATE_BOOKMARKS', 'Create bookmarks', 7, 1 UNION ALL
	SELECT 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D', 'READ_BOOKMARKS', 'Read bookmarks', 8, 1 UNION ALL
	SELECT 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D', 'EDIT_BOOKMARKS', 'Edit bookmarks', 9, 1 UNION ALL
	SELECT 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D', 'DELETE_BOOKMARKS', 'Delete bookmarks', 10, 1 UNION ALL
	SELECT 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D', 'CREATE_EVIDENCE_LOCK', 'Create evidence lock', 11, 1 UNION ALL
	SELECT 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D', 'READ_EVIDENCE_LOCK', 'Read evidence lock', 12, 1 UNION ALL
	SELECT 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D', 'DELETE_EVIDENCE_LOCK', 'Delete evidence lock', 13, 1 UNION ALL
	SELECT 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D', 'START_MANUAL_RECORDING', 'Start manual recording', 14, 1 UNION ALL
	SELECT 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D', 'STOP_MANUAL_RECORDING', 'Stop manual recording', 15, 1 UNION ALL
	SELECT 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D', 'DELETE_RECORDINGS', 'Delete recordings', 16, 1 UNION ALL
	SELECT 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D', 'ADMIN_SECURITY', 'Administrate security', 17, 0
	
	-- Input Actions
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible])
	SELECT '3ED79EF0-EE2F-40B8-A49A-17E9380C5932', 'GENERIC_READ', 'Read', 0, 1 UNION ALL
	SELECT '3ED79EF0-EE2F-40B8-A49A-17E9380C5932', 'GENERIC_WRITE', 'Edit', 1, 0 UNION ALL
	SELECT '3ED79EF0-EE2F-40B8-A49A-17E9380C5932', 'ADMIN_SECURITY', 'Administrate security', 2, 0

	-- Output Actions
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible])
	SELECT 'A95FCE90-753D-4CB7-86FB-F8CAD9F93B67', 'GENERIC_READ', 'Read', 0, 1 UNION ALL
	SELECT 'A95FCE90-753D-4CB7-86FB-F8CAD9F93B67', 'GENERIC_WRITE', 'Edit', 1, 0 UNION ALL
	SELECT 'A95FCE90-753D-4CB7-86FB-F8CAD9F93B67', 'ACTIVATE', 'Activate', 2, 1 UNION ALL
	SELECT 'A95FCE90-753D-4CB7-86FB-F8CAD9F93B67', 'ADMIN_SECURITY', 'Administrate security', 3, 0
		
	-- SmartWall Actions
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible])
	SELECT '8732FCAC-A63C-46EA-9683-532B4C6644E7', 'GENERIC_READ', 'Read', 0, 1 UNION ALL
	SELECT '8732FCAC-A63C-46EA-9683-532B4C6644E7', 'GENERIC_WRITE', 'Edit', 1, 0 UNION ALL
	SELECT '8732FCAC-A63C-46EA-9683-532B4C6644E7', 'DELETE', 'Delete', 2, 0 UNION ALL
	SELECT '8732FCAC-A63C-46EA-9683-532B4C6644E7', 'OPERATE', 'Operate', 3, 1 UNION ALL
	SELECT '8732FCAC-A63C-46EA-9683-532B4C6644E7', 'ADMIN_SECURITY', 'Administrate security', 4, 0 UNION ALL
	SELECT '8732FCAC-A63C-46EA-9683-532B4C6644E7', 'CREATE_SMARTWALL', 'Create smart wall', 5, 0 

	-- ViewGroup Actions
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible])
	SELECT 'EC543C56-ADF7-4600-B9AD-97E166552BE9', 'GENERIC_READ', 'Read', 0, 1 UNION ALL
	SELECT 'EC543C56-ADF7-4600-B9AD-97E166552BE9', 'GENERIC_WRITE', 'Edit', 1, 0 UNION ALL
	SELECT 'EC543C56-ADF7-4600-B9AD-97E166552BE9', 'DELETE', 'Delete', 2, 1 UNION ALL
	SELECT 'EC543C56-ADF7-4600-B9AD-97E166552BE9', 'OPERATE', 'Operate', 3, 1 UNION ALL
	SELECT 'EC543C56-ADF7-4600-B9AD-97E166552BE9', 'ADMIN_SECURITY', 'Administrate security', 4, 1 UNION ALL
	SELECT 'EC543C56-ADF7-4600-B9AD-97E166552BE9', 'CREATE_VIEWGROUP', 'Create view group', 5, 1 

	-- User-defined Event Actions
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible])
	SELECT 'D4FF31CD-D9AC-46DF-8E92-5B545D191BAF', 'GENERIC_READ', 'Read', 0, 1 UNION ALL
	SELECT 'D4FF31CD-D9AC-46DF-8E92-5B545D191BAF', 'GENERIC_WRITE', 'Edit', 1, 0 UNION ALL
	SELECT 'D4FF31CD-D9AC-46DF-8E92-5B545D191BAF', 'DELETE', 'Delete', 2, 0 UNION ALL
	SELECT 'D4FF31CD-D9AC-46DF-8E92-5B545D191BAF', 'TRIGGER', 'Trigger', 3, 1 UNION ALL
	SELECT 'D4FF31CD-D9AC-46DF-8E92-5B545D191BAF', 'ADMIN_SECURITY', 'Administrate security', 4, 0 UNION ALL
	SELECT 'D4FF31CD-D9AC-46DF-8E92-5B545D191BAF', 'CREATE_USER_DEFINED_EVENT', 'Create user-defined event', 5, 0 

	-- Matrix Actions
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible])
	SELECT '988D493D-1597-4BE6-AFF0-396BFD5D047A', 'GENERIC_READ', 'Read', 0, 1 UNION ALL
	SELECT '988D493D-1597-4BE6-AFF0-396BFD5D047A', 'GENERIC_WRITE', 'Edit', 1, 0 UNION ALL
	SELECT '988D493D-1597-4BE6-AFF0-396BFD5D047A', 'DELETE', 'Delete', 2, 0 UNION ALL
	SELECT '988D493D-1597-4BE6-AFF0-396BFD5D047A', 'ADMIN_SECURITY', 'Administrate security', 3, 0 UNION ALL
	SELECT '988D493D-1597-4BE6-AFF0-396BFD5D047A', 'CREATE_MATRIX', 'Create matrix', 4, 0 

	-- Rule Actions
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible])
	SELECT 'C24C91A7-7D89-459B-B78E-C4548C74640D', 'GENERIC_READ', 'Read', 0, 0 UNION ALL
	SELECT 'C24C91A7-7D89-459B-B78E-C4548C74640D', 'GENERIC_WRITE', 'Edit', 1, 0 UNION ALL
	SELECT 'C24C91A7-7D89-459B-B78E-C4548C74640D', 'DELETE', 'Delete', 2, 0 UNION ALL
	SELECT 'C24C91A7-7D89-459B-B78E-C4548C74640D', 'ADMIN_SECURITY', 'Administrate security', 3, 0 UNION ALL
	SELECT 'C24C91A7-7D89-459B-B78E-C4548C74640D', 'CREATE_RULE', 'Create rule', 4, 0 
END
GO

DECLARE @namespace_id NVARCHAR(260)
DECLARE @separator CHAR(1)
DECLARE @security_object_id UNIQUEIDENTIFIER

-- Register Management Server Security Object
SELECT @namespace_id = [namespace_id], @separator = [separator] FROM [SecurityNamespace] WHERE [name] = 'ManagementServer'
SELECT @security_object_id = '85082DD6-A02C-4D29-B99E-B39DA68A8E49'

IF NOT EXISTS(SELECT * FROM SecurityObject WHERE [namespace_id] = @namespace_id AND [security_object_id] = @security_object_id AND [object_id] = @separator)
INSERT [SecurityObject] ([namespace_id], [parent_path], [object_id], [object_type], [object_name], [security_object_id], [inherits_from_id])
SELECT @namespace_id, '', @separator, 1, 'ManagementServer', @security_object_id, NULL

-- Register Recording Server Security Objects
SELECT @namespace_id = [namespace_id], @separator = [separator] FROM [SecurityNamespace] WHERE [name] = 'RecordingServer'
SELECT @security_object_id = 'CF958BC8-5956-4EA0-92D3-28FD2C2FAA51'

IF NOT EXISTS(SELECT * FROM SecurityObject WHERE [namespace_id] = @namespace_id AND [security_object_id] = @security_object_id AND [object_id] = @separator)
INSERT [SecurityObject] ([namespace_id], [parent_path], [object_id], [object_type], [object_name], [security_object_id], [inherits_from_id])
SELECT @namespace_id, '', @separator, 1, 'RecordingServer', @security_object_id, NULL

INSERT [SecurityObject] ([namespace_id], [security_object_id], [object_id], [inherits_from_id], [parent_path], [object_type], [object_name])
SELECT @namespace_id, NEWID(), [IDRecorder], @security_object_id, '/', 2, ''
FROM [Recorders] r
LEFT JOIN [SecurityObject] so ON so.[object_id] = r.[IDRecorder]
WHERE so.[object_id] IS NULL

-- Register Failover Server Security Object
SELECT @namespace_id = [namespace_id], @separator = [separator] FROM [SecurityNamespace] WHERE [name] = 'FailoverServer'
SELECT @security_object_id = '96291CDB-898D-4C9B-99E7-57396C5CD0F3'

IF NOT EXISTS(SELECT * FROM SecurityObject WHERE [namespace_id] = @namespace_id AND [security_object_id] = @security_object_id AND [object_id] = @separator)
INSERT [SecurityObject] ([namespace_id], [parent_path], [object_id], [object_type], [object_name], [security_object_id], [inherits_from_id])
SELECT @namespace_id, '', @separator, 1, 'FailoverServer', @security_object_id, NULL

-- Register Hardware Security Objects
SELECT @namespace_id = [namespace_id], @separator = [separator] FROM [SecurityNamespace] WHERE [name] = 'Hardware'
SELECT @security_object_id = 'D11BEC62-022D-41AF-AD30-28F4F563D69A'

IF NOT EXISTS(SELECT * FROM SecurityObject WHERE [namespace_id] = @namespace_id AND [security_object_id] = @security_object_id AND [object_id] = @separator)
INSERT [SecurityObject] ([namespace_id], [parent_path], [object_id], [object_type], [object_name], [security_object_id], [inherits_from_id])
SELECT @namespace_id, '', @separator, 1, 'Hardware', @security_object_id, NULL

INSERT [SecurityObject]([namespace_id], [security_object_id], [object_id], [inherits_from_id], [parent_path], [object_type], [object_name])
SELECT @namespace_id, NEWID(), [IDHardware], @security_object_id, '/', 2, ''
FROM [Hardware] r
LEFT JOIN [SecurityObject] so ON so.[object_id] = r.[IDHardware]
WHERE so.[object_id] IS NULL

-- Register Camera Security Objects
SELECT @namespace_id = [namespace_id], @separator = [separator] FROM [SecurityNamespace] WHERE [name] = 'Camera'
SELECT @security_object_id = '517B2A09-1C1A-43F6-8148-8DDFEE65D587'

IF NOT EXISTS(SELECT * FROM SecurityObject WHERE [namespace_id] = @namespace_id AND [security_object_id] = @security_object_id AND [object_id] = @separator)
INSERT [SecurityObject] ([namespace_id], [parent_path], [object_id], [object_type], [object_name], [security_object_id], [inherits_from_id])
SELECT @namespace_id, '', @separator, 1, 'Camera', @security_object_id, NULL

INSERT [SecurityObject]([namespace_id], [security_object_id], [object_id], [inherits_from_id], [parent_path], [object_type], [object_name])
SELECT @namespace_id, NEWID(), [IDDevice], @security_object_id, '/', 2, ''
FROM [Devices] r
LEFT JOIN [SecurityObject] so ON so.[object_id] = r.[IDDevice]
WHERE r.[DeviceType] = 'Camera' AND so.[object_id] IS NULL

-- Register Microphone Security Objects
SELECT @namespace_id = [namespace_id], @separator = [separator] FROM [SecurityNamespace] WHERE [name] = 'Microphone'
SELECT @security_object_id = 'D1547F78-1CB6-46A7-8D56-8301757B937C'

IF NOT EXISTS(SELECT * FROM SecurityObject WHERE [namespace_id] = @namespace_id AND [security_object_id] = @security_object_id AND [object_id] = @separator)
INSERT [SecurityObject] ([namespace_id], [parent_path], [object_id], [object_type], [object_name], [security_object_id], [inherits_from_id])
SELECT @namespace_id, '', @separator, 1, 'Microphone', @security_object_id, NULL

INSERT [SecurityObject]([namespace_id], [security_object_id], [object_id], [inherits_from_id], [parent_path], [object_type], [object_name])
SELECT @namespace_id, NEWID(), [IDDevice], @security_object_id, '/', 2, ''
FROM [Devices] r
LEFT JOIN [SecurityObject] so ON so.[object_id] = r.[IDDevice]
WHERE r.[DeviceType] = 'AudioInput' AND so.[object_id] IS NULL

-- Register Speaker Security Objects
SELECT @namespace_id = [namespace_id], @separator = [separator] FROM [SecurityNamespace] WHERE [name] = 'Speaker'
SELECT @security_object_id = '188263CF-E1CD-4878-9F9A-8FD12C6050DB'

IF NOT EXISTS(SELECT * FROM SecurityObject WHERE [namespace_id] = @namespace_id AND [security_object_id] = @security_object_id AND [object_id] = @separator)
INSERT [SecurityObject] ([namespace_id], [parent_path], [object_id], [object_type], [object_name], [security_object_id], [inherits_from_id])
SELECT @namespace_id, '', @separator, 1, 'Speaker', @security_object_id, NULL

INSERT [SecurityObject]([namespace_id], [security_object_id], [object_id], [inherits_from_id], [parent_path], [object_type], [object_name])
SELECT @namespace_id, NEWID(), [IDDevice], @security_object_id, '/', 2, ''
FROM [Devices] r
LEFT JOIN [SecurityObject] so ON so.[object_id] = r.[IDDevice]
WHERE r.[DeviceType] = 'AudioOutput' AND so.[object_id] IS NULL

-- Register Metadata Security Objects
SELECT @namespace_id = [namespace_id], @separator = [separator] FROM [SecurityNamespace] WHERE [name] = 'Metadata'
SELECT @security_object_id = '0056DA13-569F-44D8-A28B-D33F1D013CE6'

IF NOT EXISTS(SELECT * FROM SecurityObject WHERE [namespace_id] = @namespace_id AND [security_object_id] = @security_object_id AND [object_id] = @separator)
INSERT [SecurityObject] ([namespace_id], [parent_path], [object_id], [object_type], [object_name], [security_object_id], [inherits_from_id])
SELECT @namespace_id, '', @separator, 1, 'Metadata', @security_object_id, NULL

INSERT [SecurityObject]([namespace_id], [security_object_id], [object_id], [inherits_from_id], [parent_path], [object_type], [object_name])
SELECT @namespace_id, NEWID(), [IDDevice], @security_object_id, '/', 2, ''
FROM [Devices] r
LEFT JOIN [SecurityObject] so ON so.[object_id] = r.[IDDevice]
WHERE r.[DeviceType] = 'Metadata' AND so.[object_id] IS NULL

-- Register Input Security Objects
SELECT @namespace_id = [namespace_id], @separator = [separator] FROM [SecurityNamespace] WHERE [name] = 'Input'
SELECT @security_object_id = '3697E893-93FA-4639-B7E7-51A83E381E47'

IF NOT EXISTS(SELECT * FROM SecurityObject WHERE [namespace_id] = @namespace_id AND [security_object_id] = @security_object_id AND [object_id] = @separator)
INSERT [SecurityObject] ([namespace_id], [parent_path], [object_id], [object_type], [object_name], [security_object_id], [inherits_from_id])
SELECT @namespace_id, '', @separator, 1, 'Input', @security_object_id, NULL

INSERT [SecurityObject]([namespace_id], [security_object_id], [object_id], [inherits_from_id], [parent_path], [object_type], [object_name])
SELECT @namespace_id, NEWID(), [IDDevice], @security_object_id, '/', 2, ''
FROM [Devices] r
LEFT JOIN [SecurityObject] so ON so.[object_id] = r.[IDDevice]
WHERE r.[DeviceType] = 'Input' AND so.[object_id] IS NULL

-- Register Output Security Objects
SELECT @namespace_id = [namespace_id], @separator = [separator] FROM [SecurityNamespace] WHERE [name] = 'Output'
SELECT @security_object_id = '9941169E-7E3E-4170-B00D-ADF294E6952A'

IF NOT EXISTS(SELECT * FROM SecurityObject WHERE [namespace_id] = @namespace_id AND [security_object_id] = @security_object_id AND [object_id] = @separator)
INSERT [SecurityObject] ([namespace_id], [parent_path], [object_id], [object_type], [object_name], [security_object_id], [inherits_from_id])
SELECT @namespace_id, '', @separator, 1, 'Output', @security_object_id, NULL

INSERT [SecurityObject]([namespace_id], [security_object_id], [object_id], [inherits_from_id], [parent_path], [object_type], [object_name])
SELECT @namespace_id, NEWID(), [IDDevice], @security_object_id, '/', 2, ''
FROM [Devices] r
LEFT JOIN [SecurityObject] so ON so.[object_id] = r.[IDDevice]
WHERE r.[DeviceType] = 'Output' AND so.[object_id] IS NULL

-- Register SmartWall Security Objects
SELECT @namespace_id = [namespace_id], @separator = [separator] FROM [SecurityNamespace] WHERE [name] = 'SmartWall'
SELECT @security_object_id = '7A35AEF9-EF3C-449E-BB30-79D6468E07BE'

IF NOT EXISTS(SELECT * FROM SecurityObject WHERE [namespace_id] = @namespace_id AND [security_object_id] = @security_object_id AND [object_id] = @separator)
INSERT [SecurityObject] ([namespace_id], [parent_path], [object_id], [object_type], [object_name], [security_object_id], [inherits_from_id])
SELECT @namespace_id, '', @separator, 1, 'SmartWall', @security_object_id, NULL

INSERT [SecurityObject]([namespace_id], [security_object_id], [object_id], [inherits_from_id], [parent_path], [object_type], [object_name])
SELECT @namespace_id, NEWID(), [pkid], @security_object_id, '/', 2, ''
FROM [VideoWall] vw
LEFT JOIN [SecurityObject] so ON so.[object_id] = vw.[pkid]
WHERE so.[object_id] IS NULL

-- Register ViewGroup Security Object
SELECT @namespace_id = [namespace_id], @separator = [separator] FROM [SecurityNamespace] WHERE [name] = 'ViewGroup'
SELECT @security_object_id = 'E930CDDB-841D-49B9-A0D8-14906EF65C48'

IF NOT EXISTS(SELECT * FROM SecurityObject WHERE [namespace_id] = @namespace_id AND [security_object_id] = @security_object_id AND [object_id] = @separator)
INSERT [SecurityObject] ([namespace_id], [parent_path], [object_id], [object_type], [object_name], [security_object_id], [inherits_from_id])
SELECT @namespace_id, '', @separator, 1, 'ViewGroup', @security_object_id, NULL

INSERT [SecurityObject]([namespace_id], [security_object_id], [object_id], [inherits_from_id], [parent_path], [object_type], [object_name])
SELECT @namespace_id, NEWID(), [IDServerGroup], @security_object_id, '/', 2, ''
FROM [ServerGroup]
WHERE [StructureType] = 'VIEW_GROUP' AND [IDParentGroup] = '430C2FE2-B233-42DA-B508-1A22EF03383D' AND [IDServerGroup] <> '4BF8A8B0-54AB-4E82-994F-98D8E9584176'

-- Define PrivateGroup as root for all private view groups
SELECT @security_object_id = '2FDA25CE-0CCC-4FF9-92EA-32A486940592'
INSERT [SecurityObject]([namespace_id], [security_object_id], [object_id], [inherits_from_id], [parent_path], [object_type], [object_name])
SELECT @namespace_id, @security_object_id, '4BF8A8B0-54AB-4E82-994F-98D8E9584176/', 'E930CDDB-841D-49B9-A0D8-14906EF65C48', '/', 1, '(Private Root) - 4BF8A8B0-54AB-4E82-994F-98D8E9584176'

-- Register private groups
INSERT [SecurityObject]([namespace_id], [security_object_id], [object_id], [inherits_from_id], [parent_path], [object_type], [object_name])
SELECT @namespace_id, NEWID(), [IDServerGroup], '2FDA25CE-0CCC-4FF9-92EA-32A486940592', '/4BF8A8B0-54AB-4E82-994F-98D8E9584176/', 2, ''
FROM [ServerGroup]
WHERE [StructureType] = 'VIEW_GROUP' AND [IDParentGroup] = '4BF8A8B0-54AB-4E82-994F-98D8E9584176'

-- Register UserDefinedEvent Security Objects
SELECT @namespace_id = [namespace_id], @separator = [separator] FROM [SecurityNamespace] WHERE [name] = 'UserDefinedEvent'
SELECT @security_object_id = '04A05AD5-DA09-48C6-8CD3-35CFCFA65D20'

IF NOT EXISTS(SELECT * FROM SecurityObject WHERE [namespace_id] = @namespace_id AND [security_object_id] = @security_object_id AND [object_id] = @separator)
INSERT [SecurityObject] ([namespace_id], [parent_path], [object_id], [object_type], [object_name], [security_object_id], [inherits_from_id])
SELECT @namespace_id, '', @separator, 1, 'UserDefinedEvent', @security_object_id, NULL

INSERT [SecurityObject]([namespace_id], [security_object_id], [object_id], [inherits_from_id], [parent_path], [object_type], [object_name])
SELECT @namespace_id, NEWID(), [IDEventType], @security_object_id, '/', 2, ''
FROM [EventTypes] et
LEFT JOIN [SecurityObject] so ON so.[object_id] = et.[IDEventType]
WHERE et.[GeneratorType] = 'External' AND so.[object_id] IS NULL

-- Register Matrix Security Objects
SELECT @namespace_id = [namespace_id], @separator = [separator] FROM [SecurityNamespace] WHERE [name] = 'Matrix'
SELECT @security_object_id = '870890FB-2D52-4F08-BFB2-FC81FECF8968'

IF NOT EXISTS(SELECT * FROM SecurityObject WHERE [namespace_id] = @namespace_id AND [security_object_id] = @security_object_id AND [object_id] = @separator)
INSERT [SecurityObject] ([namespace_id], [parent_path], [object_id], [object_type], [object_name], [security_object_id], [inherits_from_id])
SELECT @namespace_id, '', @separator, 1, 'Matrix', @security_object_id, NULL

INSERT [SecurityObject]([namespace_id], [security_object_id], [object_id], [inherits_from_id], [parent_path], [object_type], [object_name])
SELECT @namespace_id, NEWID(), [IDMatrix], @security_object_id, '/', 2, ''
FROM [Matrix] et
LEFT JOIN [SecurityObject] so ON so.[object_id] = et.[IDMatrix]
WHERE so.[object_id] IS NULL

-- Register Rule Security Objects
SELECT @namespace_id = [namespace_id], @separator = [separator] FROM [SecurityNamespace] WHERE [name] = 'Rule'
SELECT @security_object_id = 'BA3EEB9C-1784-4BB8-A195-DE5B3C8D0EE2'

IF NOT EXISTS(SELECT * FROM SecurityObject WHERE [namespace_id] = @namespace_id AND [security_object_id] = @security_object_id AND [object_id] = @separator)
INSERT [SecurityObject] ([namespace_id], [parent_path], [object_id], [object_type], [object_name], [security_object_id], [inherits_from_id])
SELECT @namespace_id, '', @separator, 1, 'Rule', @security_object_id, NULL

INSERT [SecurityObject] ([namespace_id], [security_object_id], [object_id], [inherits_from_id], [parent_path], [object_type], [object_name])
SELECT @namespace_id, NEWID(), [IDRule], @security_object_id, '/', 2, ''
FROM [Rules] et
LEFT JOIN [SecurityObject] so ON so.[object_id] = et.[IDRule]
WHERE so.[object_id] IS NULL
GO
