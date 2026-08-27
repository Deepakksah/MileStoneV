-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- First drop stored procedures that currently use the DescriptionType
-- The stored procedures will be created again after all update scripts are completed.

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_update_role]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_update_role]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_recording_server]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_recording_server]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_hardware]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_hardware]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_device]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_device]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_create_failover_history_entry]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_create_failover_history_entry]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_create_failover_server]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_create_failover_server]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_update_failover_history_entry]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_update_failover_history_entry]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_update_failover_server]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_update_failover_server]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[log_create_entry]') AND type in (N'P', N'PC'))
DROP PROCEDURE [log_create_entry]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_recording_server]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_recording_server]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_hardware]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_hardware]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_copy_role]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_copy_role]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_create_role]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_create_role]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_create_role_transaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_create_role_transaction]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_update_role]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_update_role]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_archive_storage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [create_archive_storage]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_recording_storage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [create_recording_storage]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_archive_storage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_archive_storage]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_recording_storage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_recording_storage]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_server_group]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_server_group]
GO

-- Rename the original type (the ntext based type)
IF  EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'DescriptionType' AND ss.name = N'dbo')
EXEC sp_rename N'DescriptionType', N'DescriptionTypeNText', N'USERDATATYPE'
GO

-- Create the new type (nvarchar(max))
CREATE TYPE [dbo].[DescriptionType] FROM NVARCHAR(MAX) NOT NULL
GO


-- Now update all tables
UPDATE [ClientProfile] SET [Description] = '' WHERE [Description] IS NULL
ALTER TABLE [ClientProfile] ALTER COLUMN [Description] [DescriptionType] NOT NULL
UPDATE [ClientProfile] SET [Description] = [Description]

UPDATE [FailoverHistory] SET [Description] = '' WHERE [Description] IS NULL
ALTER TABLE [FailoverHistory] ALTER COLUMN [Description] [DescriptionType] NOT NULL
UPDATE [FailoverHistory] SET [Description] = [Description]

UPDATE [FailoverServers] SET [Description] = '' WHERE [Description] IS NULL
ALTER TABLE [FailoverServers] ALTER COLUMN [Description] [DescriptionType] NOT NULL
UPDATE [FailoverServers] SET [Description] = [Description]

-- DatabaseLocation should not use DescriptionType as it is not a description
ALTER TABLE [FailoverServers] ALTER COLUMN [DatabaseLocation] NVARCHAR(MAX) NULL
UPDATE [FailoverServers] SET [DatabaseLocation] = [DatabaseLocation]

UPDATE [Hardware] SET [Description] = '' WHERE [Description] IS NULL
ALTER TABLE [Hardware] ALTER COLUMN [Description] [DescriptionType] NOT NULL
UPDATE [Hardware] SET [Description] = [Description]

UPDATE [HardwareTemplates] SET [Description] = '' WHERE [Description] IS NULL
ALTER TABLE [HardwareTemplates] ALTER COLUMN [Description] [DescriptionType] NOT NULL
UPDATE [HardwareTemplates] SET [Description] = [Description]

UPDATE [NotificationProfiles] SET [Description] = '' WHERE [Description] IS NULL
ALTER TABLE [NotificationProfiles] ALTER COLUMN [Description] [DescriptionType] NOT NULL
UPDATE [NotificationProfiles] SET [Description] = [Description]

UPDATE [PatrollingProfiles] SET [Description] = '' WHERE [Description] IS NULL
ALTER TABLE [PatrollingProfiles] ALTER COLUMN [Description] [DescriptionType] NOT NULL
UPDATE [PatrollingProfiles] SET [Description] = [Description]

UPDATE [RecorderPlugins] SET [Description] = '' WHERE [Description] IS NULL
ALTER TABLE [RecorderPlugins] ALTER COLUMN [Description] [DescriptionType] NOT NULL
UPDATE [RecorderPlugins] SET [Description] = [Description]

UPDATE [RegisteredDeviceTypes] SET [Description] = '' WHERE [Description] IS NULL
ALTER TABLE [RegisteredDeviceTypes] ALTER COLUMN [Description] [DescriptionType] NOT NULL
UPDATE [RegisteredDeviceTypes] SET [Description] = [Description]

UPDATE [RegisteredServers] SET [Description] = '' WHERE [Description] IS NULL
ALTER TABLE [RegisteredServers] ALTER COLUMN [Description] [DescriptionType] NOT NULL
UPDATE [RegisteredServers] SET [Description] = [Description]

UPDATE [Rules] SET [Description] = '' WHERE [Description] IS NULL
ALTER TABLE [Rules] ALTER COLUMN [Description] [DescriptionType] NOT NULL
UPDATE [Rules] SET [Description] = [Description]

UPDATE [SecurityAttributes] SET [Description] = '' WHERE [Description] IS NULL
ALTER TABLE [SecurityAttributes] ALTER COLUMN [Description] [DescriptionType] NOT NULL
UPDATE [SecurityAttributes] SET [Description] = [Description]

UPDATE [SecurityRoles] SET [Description] = '' WHERE [Description] IS NULL
ALTER TABLE [SecurityRoles] ALTER COLUMN [Description] [DescriptionType] NOT NULL
UPDATE [SecurityRoles] SET [Description] = [Description]

UPDATE [TimeProfile] SET [Description] = '' WHERE [Description] IS NULL
ALTER TABLE [TimeProfile] ALTER COLUMN [Description] [DescriptionType] NOT NULL
UPDATE [TimeProfile] SET [Description] = [Description]

-- Finally drop the old ntext based type (should no longer be referenced)
IF  EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'DescriptionTypeNText' AND ss.name = N'dbo')
DROP TYPE [dbo].[DescriptionTypeNText]
GO
