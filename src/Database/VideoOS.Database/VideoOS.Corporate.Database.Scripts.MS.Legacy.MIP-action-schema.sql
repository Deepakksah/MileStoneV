-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[MIPAction]') AND type in (N'U'))
DROP TABLE MIPAction
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[MIPAction]') AND type in (N'U'))
CREATE TABLE [MIPAction]
(
	[IDMIPAction] [UNIQUEIDENTIFIER] NOT NULL,
	[LastModified] [DATETIME] NOT NULL,
	[CreatedDate] [DATETIME] NOT NULL,
	[Name] [NameType] NOT NULL,
	[DefiningPluginId] [UNIQUEIDENTIFIER] NOT NULL,
	[DefiningPluginName] [NameType] NOT NULL,
	[SelectionText] [NameType] NOT NULL,
	[DescriptionText] [NameType] NOT NULL,
	[PickerText] [NameType] NOT NULL,
	[MIPKinds] [XML] NOT NULL,

	CONSTRAINT [PK_MIPAction] PRIMARY KEY NONCLUSTERED
	(
		[IDMIPAction] ASC
	),
) ON [PRIMARY]
GO



