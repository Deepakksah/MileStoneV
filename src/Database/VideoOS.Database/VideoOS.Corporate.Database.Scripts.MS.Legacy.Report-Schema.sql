-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Report]') AND type in (N'U'))
BEGIN
	CREATE TABLE [Report] (
		[pkid] INT IDENTITY(1,1) NOT NULL,
		[name] [NameType] NOT NULL,
		[description] [DescriptionType] NOT NULL,
		[url] VARCHAR(200) NOT NULL,
		[created_date] DATETIME NOT NULL,
		[last_modified] DATETIME NOT NULL,

		CONSTRAINT [PK_Report] PRIMARY KEY NONCLUSTERED
		(
			[pkid] ASC
		)
	)
END
GO
