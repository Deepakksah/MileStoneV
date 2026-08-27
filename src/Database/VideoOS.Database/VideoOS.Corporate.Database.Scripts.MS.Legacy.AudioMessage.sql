-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[AudioMessage]') AND type in (N'U'))
CREATE TABLE [AudioMessage]
(
	[LastModified] DATETIME NOT NULL, 
	[ID] [GuidType] NOT NULL,
	[Name] [NameType] NOT NULL,
	[Description] NVARCHAR(MAX) NOT NULL,
	[OriginalFileName] [LongNameType] NOT NULL,
	[Content] VARBINARY(max) NOT NULL

	CONSTRAINT [PK_AudioMessage] PRIMARY KEY NONCLUSTERED
	(
		[ID] ASC
	),

	CONSTRAINT [UQ_AudioMessage_Name] UNIQUE NONCLUSTERED
	(
		[Name] ASC
	),
) ON [PRIMARY]
GO


IF NOT EXISTS(SELECT * FROM [SystemSettings] WHERE [Name] = 'AudioMessageMaxBytes')
BEGIN
	INSERT INTO [SystemSettings] ([LastModified], [Name], [Setting])
	VALUES (GETUTCDATE(), 'AudioMessageMaxBytes', '1048576')
END
GO


IF NOT EXISTS(SELECT * FROM [SystemSettings] WHERE [Name] = 'AudioMessageMaxCount')
BEGIN
	INSERT INTO [SystemSettings] ([LastModified], [Name], [Setting])
	VALUES (GETUTCDATE(), 'AudioMessageMaxCount', '50')
END
GO
