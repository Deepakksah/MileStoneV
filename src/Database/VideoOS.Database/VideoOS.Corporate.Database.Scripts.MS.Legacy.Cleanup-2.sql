-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AlertTypeGroupMembers]') AND type in (N'U'))
DROP TABLE dbo.AlertTypeGroupMembers
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AlertTypeGroups]') AND type in (N'U'))
DROP TABLE dbo.AlertTypeGroups
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EventTypeGroupMembers]') AND type in (N'U'))
DROP TABLE dbo.EventTypeGroupMembers
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EventTypeGroups]') AND type in (N'U'))
DROP TABLE dbo.EventTypeGroups
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HardwareGroupMembers]') AND type in (N'U'))
DROP TABLE dbo.HardwareGroupMembers
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HardwareGroups]') AND type in (N'U'))
DROP TABLE dbo.HardwareGroups
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NotificationProfileGroupMembers]') AND type in (N'U'))
DROP TABLE dbo.NotificationProfileGroupMembers
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NotificationProfileGroups]') AND type in (N'U'))
DROP TABLE dbo.NotificationProfileGroups
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PatrollingProfileGroupMembers]') AND type in (N'U'))
DROP TABLE dbo.PatrollingProfileGroupMembers
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PatrollingProfileGroups]') AND type in (N'U'))
DROP TABLE dbo.PatrollingProfileGroups
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RecorderGroupMembers]') AND type in (N'U'))
DROP TABLE dbo.RecorderGroupMembers
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RecorderGroups]') AND type in (N'U'))
DROP TABLE dbo.RecorderGroups
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RuleGroupMembers]') AND type in (N'U'))
DROP TABLE dbo.RuleGroupMembers
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RuleGroups]') AND type in (N'U'))
DROP TABLE dbo.RuleGroups
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TimeProfileGroupMembers]') AND type in (N'U'))
DROP TABLE dbo.TimeProfileGroupMembers
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TimeProfileGroups]') AND type in (N'U'))
DROP TABLE dbo.TimeProfileGroups
GO


UPDATE [Hardware] SET [IDHardwareDriver] = '' WHERE [IDHardwareDriver] IS NULL
ALTER TABLE [Hardware] ALTER COLUMN [IDHardwareDriver] GuidType NOT NULL
GO

ALTER TABLE [Hardware] ALTER COLUMN [DriverProxyName] NameType NOT NULL
GO

UPDATE [Rules] SET [Definition] = '' WHERE [Definition] IS NULL
ALTER TABLE [Rules] ALTER COLUMN [Definition] XML NOT NULL
GO

ALTER TABLE [RegisteredServers] ALTER COLUMN [CustomData] NVARCHAR(MAX) NOT NULL
UPDATE [RegisteredServers] SET [CustomData]  = [CustomData] 
GO
