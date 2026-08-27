-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @usersToUpdate TABLE
(
	[ID] nvarchar(256) NOT NULL,
	[SID] varchar(256) NOT NULL
)

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.[MigrateIdpBasicUsers]') AND type in (N'U'))
BEGIN
	INSERT INTO @usersToUpdate
	SELECT [ID], [SID]
	FROM dbo.[MigrateIdpBasicUsers]
END
ELSE
	IF EXISTS(SELECT * FROM [{IdpDatabaseName}].sys.objects WHERE object_id = OBJECT_ID(N'[{IdpDatabaseName}].dbo.[AspNetUsers]') AND type in (N'U'))
	BEGIN
		INSERT INTO @usersToUpdate
		SELECT u.[ID], c.[ClaimValue] FROM [{IdpDatabaseName}].dbo.[AspNetUsers] AS u
		INNER JOIN [{IdpDatabaseName}].dbo.[AspNetUserClaims] AS c
		ON u.[ID] = c.[UserId]
		WHERE c.[ClaimType] = 'MigrateSID'
	END
			
DELETE FROM [SecurityAccessCache2]
DELETE FROM [SecurityMembershipCache]

IF EXISTS (SELECT * FROM sys.foreign_keys  WHERE object_id = OBJECT_ID(N'FK_SecurityIdentity_SecurityMembership_Identity') AND parent_object_id = OBJECT_ID(N'[SecurityMembership]'))
BEGIN
	ALTER TABLE [SecurityMembership] DROP CONSTRAINT [FK_SecurityIdentity_SecurityMembership_Identity];
	ALTER TABLE [SecurityMembership] ADD CONSTRAINT [FK_SecurityIdentity_SecurityMembership_Identity] FOREIGN KEY([MemberSID]) REFERENCES [SecurityIdentity]([SID]) ON UPDATE CASCADE
END

IF EXISTS (SELECT * FROM sys.foreign_keys  WHERE object_id = OBJECT_ID(N'FK_SecurityObject_SecurityIdentity') AND parent_object_id = OBJECT_ID(N'[SecurityAccessEntry]'))
BEGIN
	ALTER TABLE	[SecurityAccessEntry] DROP CONSTRAINT [FK_SecurityObject_SecurityIdentity];
	ALTER TABLE [SecurityAccessEntry] ADD CONSTRAINT [FK_SecurityObject_SecurityIdentity] FOREIGN KEY([user_sid]) REFERENCES [SecurityIdentity]([SID]) ON UPDATE CASCADE
END

UPDATE si SET si.[SID] = users.[ID] FROM [SecurityIdentity] si 
	INNER JOIN @usersToUpdate users ON si.[SID] = users.[SID]

IF EXISTS (SELECT * FROM sys.foreign_keys  WHERE object_id = OBJECT_ID(N'FK_SecurityIdentity_SecurityMembership_Identity') AND parent_object_id = OBJECT_ID(N'[SecurityMembership]'))
BEGIN
	ALTER TABLE [SecurityMembership] DROP CONSTRAINT [FK_SecurityIdentity_SecurityMembership_Identity];
	ALTER TABLE [SecurityMembership] ADD CONSTRAINT [FK_SecurityIdentity_SecurityMembership_Identity] FOREIGN KEY([MemberSID]) REFERENCES [SecurityIdentity]([SID])
END

IF EXISTS (SELECT * FROM sys.foreign_keys  WHERE object_id = OBJECT_ID(N'FK_SecurityObject_SecurityIdentity') AND parent_object_id = OBJECT_ID(N'[SecurityAccessEntry]'))
BEGIN
	ALTER TABLE	[SecurityAccessEntry] DROP CONSTRAINT [FK_SecurityObject_SecurityIdentity];
	ALTER TABLE [SecurityAccessEntry] ADD CONSTRAINT [FK_SecurityObject_SecurityIdentity] FOREIGN KEY([user_sid]) REFERENCES [SecurityIdentity]([SID])
END

UPDATE sri SET sri.[SID] = users.[ID] FROM [SecurityRolesIdentity] sri 
	INNER JOIN @usersToUpdate users ON sri.[SID] = users.[SID]

UPDATE cs SET cs.[OwnerSID] = users.[ID] FROM [CustomSettings] cs 
	INNER JOIN @usersToUpdate users ON cs.[OwnerSID] = users.[SID]
		
IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'Central' AND  TABLE_NAME = 'Users'))
BEGIN
	UPDATE cu SET cu.[SID] = users.[ID] FROM [Central].[Users] cu
		INNER JOIN @usersToUpdate users ON cu.[SID] = users.[SID]
END

UPDATE st SET st.[Owner] = users.[ID] FROM [Search].[SearchTemplates] st
	INNER JOIN @usersToUpdate users ON st.[Owner] = users.[SID]

UPDATE stf SET stf.[Owner] = users.[ID] FROM [Search].[SearchTemplatesFind] stf
	INNER JOIN @usersToUpdate users ON stf.[Owner] = users.[SID]

GO