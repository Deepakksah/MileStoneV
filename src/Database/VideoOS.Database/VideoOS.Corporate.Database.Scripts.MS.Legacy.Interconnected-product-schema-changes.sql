-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.columns WHERE [object_id] = OBJECT_ID(N'Hardware') AND name = N'RemoteAdminUserID')
BEGIN
	ALTER TABLE [Hardware] ADD
		[RemoteAdminUserID] UserIdType NULL,
		[RemoteAdminPassword] PasswordType NULL,
		[EdgeSettings] XML NULL,
		[SubType] INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE [object_id] = OBJECT_ID(N'Devices') AND name = N'SubType')
BEGIN
	ALTER TABLE [Devices] ADD [SubType] INT NULL
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE [object_id] = OBJECT_ID(N'RecordingStorageDevice') AND name = N'edgestorage_playback')
BEGIN
	ALTER TABLE [RecordingStorageDevice] ADD [edgestorage_playback] BIT NULL
END
GO

-- Set default values and alter colums to not null
IF EXISTS (SELECT [RemoteAdminUserID] FROM [Hardware] WHERE [RemoteAdminUserID] IS NULL)
BEGIN
	UPDATE [Hardware]
	SET [RemoteAdminUserID] = ''
	WHERE [RemoteAdminUserID] IS NULL

	UPDATE [Hardware]
	SET [RemoteAdminPassword] = ''
	WHERE [RemoteAdminPassword] IS NULL

	UPDATE [Hardware]
	SET [EdgeSettings] = '<properties />'
	WHERE [EdgeSettings] IS NULL

	UPDATE [Hardware]
	SET [SubType] = 1
	WHERE [SubType] IS NULL

	ALTER TABLE [Hardware] ALTER COLUMN [RemoteAdminUserID] UserIdType NOT NULL
	ALTER TABLE [Hardware] ALTER COLUMN [RemoteAdminPassword] PasswordType NOT NULL
	ALTER TABLE [Hardware] ALTER COLUMN [EdgeSettings] XML NOT NULL
	ALTER TABLE [Hardware] ALTER COLUMN [SubType] INT NOT NULL
END
GO

-- Set default values and alter colums to not null
IF EXISTS (SELECT [SubType] FROM [Devices] WHERE [SubType] IS NULL)
BEGIN
	UPDATE [Devices]
	SET [SubType] = 1
	WHERE [SubType] IS NULL

	ALTER TABLE [Devices] ALTER COLUMN [SubType] INT NOT NULL
END
GO

-- Set default values and alter colums to not null
IF EXISTS (SELECT [edgestorage_playback] FROM [RecordingStorageDevice] WHERE [edgestorage_playback] IS NULL)
BEGIN
	UPDATE [RecordingStorageDevice]
	SET [edgestorage_playback] = 0
	WHERE [edgestorage_playback] IS NULL

	ALTER TABLE [RecordingStorageDevice] ALTER COLUMN [edgestorage_playback] BIT NOT NULL
END
GO


