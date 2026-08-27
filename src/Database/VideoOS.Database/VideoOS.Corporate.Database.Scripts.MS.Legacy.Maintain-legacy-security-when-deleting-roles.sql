-- This header is used by the dbupdate7 tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Clean up unused security permissions
DELETE [SecurityPermissions]
WHERE [IDRole] NOT IN (SELECT [IDRole] FROM [SecurityRoles])

DELETE [SecurityPermissionsDevice]
WHERE [IDRole] NOT IN (SELECT [IDRole] FROM [SecurityRoles])
	 OR [IDDevice] NOT IN (SELECT [IDDevice] FROM [Devices])

DELETE [SecurityPermissionsEnterpriseServer]
WHERE [IDRole] NOT IN (SELECT [IDRole] FROM [SecurityRoles])
	 OR [IDEnterpriseServer] NOT IN (SELECT [IDEnterpriseServer] FROM [EnterpriseServers])

DELETE [SecurityPermissionsMatrix]
WHERE [IDRole] NOT IN (SELECT [IDRole] FROM [SecurityRoles])
	 OR [IDMatrix] NOT IN (SELECT [IDMatrix] FROM [Matrix])

-- Add constraints between permission tables and security roles and entity-tables
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_SecurityPermissions_SecurityRole]') AND parent_object_id = OBJECT_ID(N'[SecurityPermissions]'))
ALTER TABLE [SecurityPermissions]
DROP CONSTRAINT [FK_SecurityPermissions_SecurityRole]
GO

ALTER TABLE [SecurityPermissions] WITH CHECK
ADD CONSTRAINT [FK_SecurityPermissions_SecurityRole] FOREIGN KEY (IDRole)
  REFERENCES [SecurityRoles] (IDRole)
  ON DELETE CASCADE
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_SecurityPermissions_SecurityAttributes]') AND parent_object_id = OBJECT_ID(N'[SecurityPermissions]'))
ALTER TABLE [SecurityPermissions]
DROP CONSTRAINT [FK_SecurityPermissions_SecurityAttributes]
GO

ALTER TABLE [SecurityPermissions] WITH CHECK
ADD CONSTRAINT [FK_SecurityPermissions_SecurityAttributes] FOREIGN KEY (SecurityAttributeID)
  REFERENCES [SecurityAttributes] (SecurityAttributeID)
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_SecurityPermissions_TimeProfiles]') AND parent_object_id = OBJECT_ID(N'[SecurityPermissions]'))
ALTER TABLE [SecurityPermissions]
DROP CONSTRAINT [FK_SecurityPermissions_TimeProfiles]
GO

ALTER TABLE [SecurityPermissions] WITH CHECK
ADD CONSTRAINT [FK_SecurityPermissions_TimeProfiles] FOREIGN KEY([IDTimeProfile])
	REFERENCES [TimeProfiles] ([IDTimeProfile])
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_SecurityPermissionsDevice_SecurityRole]') AND parent_object_id = OBJECT_ID(N'[SecurityPermissionsDevice]'))
ALTER TABLE [SecurityPermissionsDevice]
DROP CONSTRAINT [FK_SecurityPermissionsDevice_SecurityRole]
GO

ALTER TABLE [SecurityPermissionsDevice] WITH CHECK
ADD CONSTRAINT [FK_SecurityPermissionsDevice_SecurityRole] FOREIGN KEY (IDRole)
  REFERENCES [SecurityRoles] (IDRole)
  ON DELETE CASCADE
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_SecurityPermissionsDevice_Devices]') AND parent_object_id = OBJECT_ID(N'[SecurityPermissionsDevice]'))
ALTER TABLE [SecurityPermissionsDevice]
DROP CONSTRAINT [FK_SecurityPermissionsDevice_Devices]
GO

ALTER TABLE [SecurityPermissionsDevice] WITH CHECK
ADD CONSTRAINT [FK_SecurityPermissionsDevice_Devices] FOREIGN KEY (IDDevice)
  REFERENCES [Devices] (IDDevice)
  ON DELETE CASCADE
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_SecurityPermissionsDevice_SecurityAttributes]') AND parent_object_id = OBJECT_ID(N'[SecurityPermissionsDevice]'))
ALTER TABLE [SecurityPermissionsDevice]
DROP CONSTRAINT [FK_SecurityPermissionsDevice_SecurityAttributes]
GO

ALTER TABLE [SecurityPermissionsDevice] WITH CHECK
ADD CONSTRAINT [FK_SecurityPermissionsDevice_SecurityAttributes] FOREIGN KEY (SecurityAttributeID)
  REFERENCES [SecurityAttributes] (SecurityAttributeID)
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_SecurityPermissionsEnterpriseServer_SecurityRole]') AND parent_object_id = OBJECT_ID(N'[SecurityPermissionsEnterpriseServer]'))
ALTER TABLE [SecurityPermissionsEnterpriseServer]
DROP CONSTRAINT [FK_SecurityPermissionsEnterpriseServer_SecurityRole]
GO

ALTER TABLE [SecurityPermissionsEnterpriseServer] WITH CHECK
ADD CONSTRAINT [FK_SecurityPermissionsEnterpriseServer_SecurityRole] FOREIGN KEY (IDRole)
  REFERENCES [SecurityRoles] (IDRole)
  ON DELETE CASCADE
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_SecurityPermissionsEnterpriseServer_EnterpriseServers]') AND parent_object_id = OBJECT_ID(N'[SecurityPermissionsEnterpriseServer]'))
ALTER TABLE [SecurityPermissionsEnterpriseServer]
DROP CONSTRAINT [FK_SecurityPermissionsEnterpriseServer_EnterpriseServers]
GO

ALTER TABLE [SecurityPermissionsEnterpriseServer] WITH CHECK
ADD CONSTRAINT [FK_SecurityPermissionsEnterpriseServer_EnterpriseServers] FOREIGN KEY (IDEnterpriseServer)
  REFERENCES [EnterpriseServers] (IDEnterpriseServer)
  ON DELETE CASCADE
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_SecurityPermissionsEnterpriseServer_SecurityAttributes]') AND parent_object_id = OBJECT_ID(N'[SecurityPermissionsEnterpriseServer]'))
ALTER TABLE [SecurityPermissionsEnterpriseServer]
DROP CONSTRAINT [FK_SecurityPermissionsEnterpriseServer_SecurityAttributes]
GO

ALTER TABLE [SecurityPermissionsEnterpriseServer] WITH CHECK
ADD CONSTRAINT [FK_SecurityPermissionsEnterpriseServer_SecurityAttributes] FOREIGN KEY (SecurityAttributeID)
  REFERENCES [SecurityAttributes] (SecurityAttributeID)
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_SecurityPermissionsMatrix_SecurityRole]') AND parent_object_id = OBJECT_ID(N'[SecurityPermissionsMatrix]'))
ALTER TABLE [SecurityPermissionsMatrix]
DROP CONSTRAINT [FK_SecurityPermissionsMatrix_SecurityRole]
GO

ALTER TABLE [SecurityPermissionsMatrix] WITH CHECK
ADD CONSTRAINT [FK_SecurityPermissionsMatrix_SecurityRole] FOREIGN KEY (IDRole)
  REFERENCES [SecurityRoles] (IDRole)
  ON DELETE CASCADE
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_SecurityPermissionsMatrix_Matrix]') AND parent_object_id = OBJECT_ID(N'[SecurityPermissionsMatrix]'))
ALTER TABLE [SecurityPermissionsMatrix]
DROP CONSTRAINT [FK_SecurityPermissionsMatrix_Matrix]
GO

ALTER TABLE [SecurityPermissionsMatrix] WITH CHECK
ADD CONSTRAINT [FK_SecurityPermissionsMatrix_Matrix] FOREIGN KEY (IDMatrix)
  REFERENCES [Matrix] (IDMatrix)
  ON DELETE CASCADE
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_SecurityPermissionsMatrix_SecurityAttributes]') AND parent_object_id = OBJECT_ID(N'[SecurityPermissionsMatrix]'))
ALTER TABLE [SecurityPermissionsMatrix]
DROP CONSTRAINT [FK_SecurityPermissionsMatrix_SecurityAttributes]
GO

ALTER TABLE [SecurityPermissionsMatrix] WITH CHECK
ADD CONSTRAINT [FK_SecurityPermissionsMatrix_SecurityAttributes] FOREIGN KEY (SecurityAttributeID)
  REFERENCES [SecurityAttributes] (SecurityAttributeID)
GO
