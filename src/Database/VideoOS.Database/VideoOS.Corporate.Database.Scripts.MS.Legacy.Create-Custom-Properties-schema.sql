-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[CustomProperty]') AND type in (N'U'))
BEGIN
	CREATE TABLE [CustomProperty] (
		[object_id] VARCHAR(128) NOT NULL,
		[name] [NameType] NOT NULL,
		[value] NVARCHAR(MAX)

		CONSTRAINT [PK_CustomProperty] PRIMARY KEY CLUSTERED
		(
			[object_id] ASC,
			[name] ASC
		)
	) ON [PRIMARY]
END

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[TempCustomProperty]') AND type in (N'U'))
BEGIN
	CREATE TABLE [TempCustomProperty] (
		[SPID] INT NOT NULL,
		[name] [NameType] NOT NULL,
		[value] NVARCHAR(MAX)

		CONSTRAINT [PK_TempCustomProperty] PRIMARY KEY CLUSTERED
		(
			[SPID] ASC,
			[name] ASC
		)
	) ON [PRIMARY]
END
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[TempCustomPropertyDelete]') AND type in (N'U'))
BEGIN
	CREATE TABLE [TempCustomPropertyDelete] (
		[SPID] INT NOT NULL,
		[object_id] VARCHAR(128) NOT NULL

		CONSTRAINT [PK_TempCustomPropertyDelete] PRIMARY KEY CLUSTERED
		(
			[SPID] ASC,
			[object_id] ASC
		)
	) ON [PRIMARY]
END
GO
