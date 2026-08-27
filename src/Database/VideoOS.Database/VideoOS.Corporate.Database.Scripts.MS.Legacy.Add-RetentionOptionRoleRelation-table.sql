-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RetentionOptionRoleRelation]') AND type in (N'U'))
BEGIN
	CREATE TABLE [RetentionOptionRoleRelation] (
		[IdRetentionOption] [GuidType] NOT NULL,
		[IdRole] [GuidType] NOT NULL
		
		CONSTRAINT [PK_RetentionOptionRoleRelation] PRIMARY KEY NONCLUSTERED
		(
			[IdRole] ASC
		)
	)
END
GO