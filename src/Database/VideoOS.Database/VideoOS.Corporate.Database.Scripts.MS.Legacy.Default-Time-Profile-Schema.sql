---- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[TimeProfileDefaultRoleRelation]') AND type in (N'U'))
BEGIN
	CREATE TABLE [TimeProfileDefaultRoleRelation] (
		[IDTimeProfile] [GuidType] NOT NULL,
		[IDRole] [GuidType] NOT NULL
		
		CONSTRAINT [PK_TimeProfileDefaultRoleRelation] PRIMARY KEY NONCLUSTERED
		(
			[IDRole] ASC
		),
		
		CONSTRAINT [FK_TimeProfileDefaultRoleRelation_SecurityRole] FOREIGN KEY
		(
			[IDRole]
		)
		REFERENCES [SecurityRoles] ([IDRole]),
		
		CONSTRAINT [FK_TimeProfileDefaultRoleRelation_TimeProfile] FOREIGN KEY
		(
			[IDTimeProfile]
		)
		REFERENCES [TimeProfiles] ([IDTimeProfile])
	) ON [PRIMARY]
END
GO