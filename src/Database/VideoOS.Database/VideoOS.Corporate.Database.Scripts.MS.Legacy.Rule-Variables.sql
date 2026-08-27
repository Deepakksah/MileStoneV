-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[RuleVariables]') AND type in (N'U'))
CREATE TABLE [RuleVariables]
(
	[LastModified] DATETIME NOT NULL,
	[IDVariable] [GuidType] NOT NULL,
	[Name] [dbo].[NameType] NOT NULL,
	[Description] NVARCHAR(MAX) NOT NULL,
	[Value] NVARCHAR(MAX) NOT NULL,
	[Reset] BIT NOT NULL

	CONSTRAINT [PK_RuleVariables] PRIMARY KEY CLUSTERED 
	(
		[IDVariable] ASC
	)
) ON [PRIMARY]
GO
