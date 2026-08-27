-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[BasicUser]') AND type in (N'U'))
BEGIN
	CREATE TABLE [BasicUser] (
		[id] [GuidType] NOT NULL,
		[sid] [SidType] NOT NULL,
		[user_name] [NameType] NOT NULL,
		[password_hash] VARBINARY(MAX) NOT NULL,
		[encryption_method] NVARCHAR(50) NOT NULL,
		[salt_value] [PasswordType] NOT NULL,
		[description] [DescriptionType] NOT NULL,
		[can_change_password] BIT NOT NULL,
		[created_date] DATETIME NOT NULL,
		[last_modified] DATETIME NOT NULL,
		
		CONSTRAINT [PK_BasicUser] PRIMARY KEY CLUSTERED
		(
			[id] ASC
		)
	)

	CREATE UNIQUE NONCLUSTERED INDEX [IX_UniqueName] ON [BasicUser]
	(
		[user_name] ASC
	) ON [PRIMARY]
END
GO