-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Change SQL type of several columns to Xml from NText. 

-- AlertTypes
UPDATE [AlertTypes] SET [Description] = '' WHERE [Description] IS NULL
ALTER TABLE [AlertTypes] ALTER COLUMN [Description] NVARCHAR(MAX) NOT NULL
UPDATE [AlertTypes] SET [Description] = [Description]
GO

-- Devices
DECLARE @constraint_name SYSNAME
DECLARE devices_constraints_cursor CURSOR FOR 
  SELECT [name] FROM sys.objects WHERE [type] = 'D' AND [parent_object_id] = OBJECT_ID(N'Devices')
	
OPEN devices_constraints_cursor

FETCH NEXT FROM devices_constraints_cursor INTO @constraint_name

WHILE @@FETCH_STATUS = 0
BEGIN
	EXEC('ALTER TABLE [dbo].[Devices] DROP CONSTRAINT ' + @constraint_name)
	FETCH NEXT FROM devices_constraints_cursor INTO @constraint_name
END

CLOSE devices_constraints_cursor
DEALLOCATE devices_constraints_cursor
GO

UPDATE [Devices] SET [Description] = '' WHERE [Description] IS NULL
ALTER TABLE [Devices] ALTER COLUMN [Description] NVARCHAR(MAX) NOT NULL
ALTER TABLE [Devices] ALTER COLUMN [Location] NVARCHAR(MAX) NOT NULL
ALTER TABLE [Devices] ALTER COLUMN [Streams] NVARCHAR(MAX) NOT NULL
UPDATE [Devices] SET [Description] = [Description]
UPDATE [Devices] SET [Location] = [Location]
UPDATE [Devices] SET [Streams] = [Streams]
GO

ALTER TABLE [dbo].[Devices] ADD  CONSTRAINT [DF_DevicesStreams]  DEFAULT ('') FOR [Streams]
GO

-- EventTypes
ALTER TABLE [EventTypes] ALTER COLUMN [Definition] NVARCHAR(MAX) NOT NULL
UPDATE [EventTypes] SET [Definition] = [Definition]
GO

-- ExternalPlugins
ALTER TABLE [ExternalPlugins] ALTER COLUMN [Data] NVARCHAR(MAX) NOT NULL
UPDATE [ExternalPlugins] SET [Data] = [Data]
GO

-- Matrix
UPDATE [Matrix] SET [Description] = '' WHERE [Description] IS NULL
UPDATE [Matrix] SET [Address] = '' WHERE [Address] IS NULL
ALTER TABLE [Matrix] ALTER COLUMN [Description] NVARCHAR(MAX) NOT NULL
ALTER TABLE [Matrix] ALTER COLUMN [Address] NVARCHAR(MAX) NOT NULL
UPDATE [Matrix] SET [Description] = [Description]
UPDATE [Matrix] SET [Address] = [Address]
GO

-- RecorderPluginDevice
ALTER TABLE [RecorderPluginDevice] ALTER COLUMN [Settings] NVARCHAR(MAX) NOT NULL
ALTER TABLE [RecorderPluginDevice] ALTER COLUMN [UISettings] NVARCHAR(MAX) NOT NULL
UPDATE [RecorderPluginDevice] SET [Settings] = [Settings]
UPDATE [RecorderPluginDevice] SET [UISettings] = [UISettings]
GO

-- Recorders
DECLARE @constraint_name SYSNAME
DECLARE recorder_constraints_cursor CURSOR FOR 
  SELECT [name] FROM sys.objects WHERE [type] = 'D' AND [parent_object_id] = OBJECT_ID(N'Recorders')
	
OPEN recorder_constraints_cursor

FETCH NEXT FROM recorder_constraints_cursor INTO @constraint_name

WHILE @@FETCH_STATUS = 0
BEGIN
	EXEC('ALTER TABLE [dbo].[Recorders] DROP CONSTRAINT ' + @constraint_name)
	FETCH NEXT FROM recorder_constraints_cursor INTO @constraint_name
END

CLOSE recorder_constraints_cursor
DEALLOCATE recorder_constraints_cursor
GO

UPDATE [Recorders] SET [WebServerUriConfiguration] = '' WHERE [WebServerUriConfiguration] IS NULL
UPDATE [Recorders] SET [Description] = '' WHERE [Description] IS NULL
ALTER TABLE [Recorders] ALTER COLUMN [WebServerUri] NVARCHAR(MAX) NOT NULL
ALTER TABLE [Recorders] ALTER COLUMN [WebServerUriConfiguration] NVARCHAR(MAX) NOT NULL
ALTER TABLE [Recorders] ALTER COLUMN [Description] NVARCHAR(MAX) NOT NULL
ALTER TABLE [Recorders] ALTER COLUMN [ScanRanges] NVARCHAR(MAX) NOT NULL
ALTER TABLE [Recorders] ALTER COLUMN [MulticastSettings] NVARCHAR(MAX) NOT NULL
ALTER TABLE [Recorders] ALTER COLUMN [DevicePackInfo] NVARCHAR(MAX) NOT NULL
UPDATE [Recorders] SET [WebServerUri] = [WebServerUri]
UPDATE [Recorders] SET [WebServerUriConfiguration] = [WebServerUriConfiguration]
UPDATE [Recorders] SET [Description] = [Description]
UPDATE [Recorders] SET [ScanRanges] = [ScanRanges]
UPDATE [Recorders] SET [MulticastSettings] = [MulticastSettings]
UPDATE [Recorders] SET [DevicePackInfo] = [DevicePackInfo]
GO

ALTER TABLE [dbo].[Recorders] ADD  CONSTRAINT [DF_IPScanRangeWarning]  DEFAULT ((255)) FOR [IPScanRangeWarning]
GO

ALTER TABLE [dbo].[Recorders] ADD  CONSTRAINT [DF_FailoverHostName]  DEFAULT ('') FOR [FailoverHostName]
GO

ALTER TABLE [dbo].[Recorders] ADD  CONSTRAINT [DF_DevicePackInfo]  DEFAULT ('') FOR [DevicePackInfo]
GO
