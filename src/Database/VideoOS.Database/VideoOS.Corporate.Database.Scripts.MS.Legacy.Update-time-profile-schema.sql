-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[TimeProfile]') AND type in (N'U'))
BEGIN
	CREATE TABLE [TimeProfile] (
		[pkid] [GuidType] NOT NULL,
		[name] [NameType] NOT NULL,
		[description] NVARCHAR(MAX),
		[type] NVARCHAR(15) NOT NULL,
		[xml_data] XML NOT NULL,
		[created_date] DATETIME NOT NULL,
		[last_modified] DATETIME NOT NULL,

		CONSTRAINT [PK_TimeProfile] PRIMARY KEY NONCLUSTERED
		(
			[pkid] ASC
		)
	) ON [PRIMARY]END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[TimeProfile]') AND name = 'IDX_NameType')
CREATE UNIQUE CLUSTERED INDEX IDX_NameType ON TimeProfile ([type], [name]);
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[TimeProfiles]') AND type in (N'U'))
BEGIN
	UPDATE tp
	SET tp.NAME = dt.uniqueName
	FROM TimeProfiles tp
	INNER JOIN
	(
		SELECT tp2.IDTimeProfile as ID, Name + '_' + CAST(ROW_NUMBER() OVER(PARTITION by tp2.Name order by tp2.Name) as NVARCHAR(max)) as uniqueName
		FROM TIMEPROFILES tp2
		WHERE tp2.NAME IN 
		(
			SELECT tp3.NAME
			FROM TIMEPROFILES tp3
			GROUP BY tp3.Name
			HAVING COUNT(*) > 1
		)

		UNION 

		SELECT tp4.IDTimeProfile as ID, tp4.Name as uniqueName
		FROM TIMEPROFILES tp4
		WHERE tp4.NAME IN 
		(
			SELECT tp5.NAME
			FROM TIMEPROFILES tp5
			GROUP BY tp5.Name
			HAVING COUNT(*) = 1
		)
	) dt
	ON tp.IDTimeProfile = dt.ID
	
	INSERT [TimeProfile] ([pkid], [name], [description], [type], [xml_data], [created_date], [last_modified])
	SELECT 
		[IDTimeProfile],
		[Name],
		[Description],
		'CalendarProfile',
		[Definition],
		GETUTCDATE(),
		[LastModified]
	FROM [TimeProfiles]
END

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_SecurityPermissions_TimeProfiles]') AND parent_object_id = OBJECT_ID(N'[SecurityPermissions]'))
ALTER TABLE [SecurityPermissions]
DROP CONSTRAINT [FK_SecurityPermissions_TimeProfiles]
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_TimeProfileDefaultRoleRelation_TimeProfile]') AND parent_object_id = OBJECT_ID(N'[TimeProfileDefaultRoleRelation]'))
ALTER TABLE [TimeProfileDefaultRoleRelation]
DROP CONSTRAINT [FK_TimeProfileDefaultRoleRelation_TimeProfile]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_SecurityPermissions_TimeProfile]') AND parent_object_id = OBJECT_ID(N'[SecurityPermissions]'))
ALTER TABLE [SecurityPermissions]
ADD CONSTRAINT [FK_SecurityPermissions_TimeProfile] FOREIGN KEY ([IDTimeProfile]) REFERENCES [TimeProfile] ([pkid])
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_SecurityPermissionsDevice_TimeProfile]') AND parent_object_id = OBJECT_ID(N'[SecurityPermissionsDevice]'))
ALTER TABLE [SecurityPermissionsDevice]
ADD CONSTRAINT [FK_SecurityPermissionsDevice_TimeProfile] FOREIGN KEY ([IDTimeProfile]) REFERENCES [TimeProfile] ([pkid])
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[FK_TimeProfileDefaultRoleRelation_TimeProfile]') AND parent_object_id = OBJECT_ID(N'[TimeProfileDefaultRoleRelation]'))
ALTER TABLE [TimeProfileDefaultRoleRelation]
ADD CONSTRAINT [FK_TimeProfileDefaultRoleRelation_TimeProfile] FOREIGN KEY ([IDTimeProfile]) REFERENCES [TimeProfile] ([pkid])
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[TimeProfiles]') AND type in (N'U'))
DROP TABLE [TimeProfiles]
GO
