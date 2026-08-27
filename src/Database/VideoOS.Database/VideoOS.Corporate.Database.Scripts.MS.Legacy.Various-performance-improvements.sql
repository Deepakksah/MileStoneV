-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

BEGIN TRAN

IF NOT EXISTS(SELECT * FROM sys.indexes WHERE [object_id] = OBJECT_ID(N'ServerGroupMembership') AND name = N'IX_IDObject')
BEGIN
	ALTER TABLE [ServerGroupMembership] DROP CONSTRAINT [FK_ServerGroupMembership_ServerGroup]

	ALTER TABLE [ServerGroup] DROP CONSTRAINT [PK_ServerGroup]

	ALTER TABLE [ServerGroup] ADD CONSTRAINT [PK_ServerGroup] PRIMARY KEY NONCLUSTERED ([IDServerGroup])

	CREATE CLUSTERED INDEX [IX_ServerGroup_StructureType] ON [ServerGroup] ([IDParentGroup], [StructureType])

	DROP INDEX [IX_ServerGroupParentName] ON [ServerGroup]

	ALTER TABLE [ServerGroupMembership] WITH CHECK ADD CONSTRAINT [FK_ServerGroupMembership_ServerGroup] FOREIGN KEY([IDServerGroup])
	REFERENCES [ServerGroup] ([IDServerGroup])

	CREATE NONCLUSTERED INDEX [IX_IDObject] ON [ServerGroupMembership] ([IDObject])

	ALTER TABLE [SecurityAction]
	ALTER COLUMN legacy_security_name VARCHAR(100) NULL

	CREATE NONCLUSTERED INDEX [IX_SAE_Name_Deny_Deleted] ON [SecurityAccessEntry] ([action_name], [access_deny], [deleted])
	INCLUDE ([security_object_id], [user_sid])

	ALTER TABLE [SecurityPermissionsDevice] DROP CONSTRAINT [PK_SecurityPermissionsDevice]

	ALTER TABLE [SecurityPermissionsDevice] ADD CONSTRAINT [PK_SecurityPermissionsDevice] PRIMARY KEY NONCLUSTERED
	(
		[SecurityAttributeID] ASC,
		[IDRole] ASC,
		[IDDevice] ASC
	)

	CREATE CLUSTERED INDEX [IX_SecurityAttributeID] ON [SecurityPermissionsDevice] ([SecurityAttributeID])
END

COMMIT TRAN
GO
