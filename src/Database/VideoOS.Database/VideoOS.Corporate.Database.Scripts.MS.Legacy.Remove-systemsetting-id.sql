-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SystemSettings]') AND name = 'IX_SystemSettings')
ALTER TABLE [dbo].[SystemSettings] DROP CONSTRAINT [IX_SystemSettings]
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SystemSettings]') AND name = 'PK_SystemSettings')
ALTER TABLE [dbo].[SystemSettings] DROP CONSTRAINT [PK_SystemSettings]
GO

ALTER TABLE [SystemSettings] ADD CONSTRAINT
	PK_SystemSettings PRIMARY KEY CLUSTERED
	(
		[Name]
	) ON [PRIMARY]
GO

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[SystemSettings]') AND name = 'IDSystemSetting')
ALTER TABLE [SystemSettings] DROP COLUMN [IDSystemSetting]
GO
