-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Feature]') AND type in (N'U'))
BEGIN
	CREATE TABLE [Feature] (
		[Name] NVARCHAR(128) NOT NULL,
		[DisplayName] NVARCHAR(MAX) NOT NULL,
		[Visible] BIT NOT NULL

		CONSTRAINT [PK_Feature] PRIMARY KEY CLUSTERED
		(
			[Name] ASC
		)
	) ON [PRIMARY]
END
GO

IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FeatureParameter]') AND type in (N'U'))
BEGIN
	CREATE TABLE [FeatureParameter] (
		[Name] NVARCHAR(128) NOT NULL,
		[Feature_Name] NVARCHAR(128) NOT NULL,
		[DisplayName] NVARCHAR(MAX) NOT NULL,
		[DisplayOrder] SMALLINT NOT NULL,
		[Value] NVARCHAR(MAX) NOT NULL,
		[Visible] BIT NOT NULL

		CONSTRAINT [PK_FeatureParameter] PRIMARY KEY CLUSTERED
		(
			[Name] ASC,
			[Feature_Name] ASC
		)

		CONSTRAINT [FK_FeatureParameter_Feature] FOREIGN KEY([Feature_Name])
		REFERENCES [Feature] ([Name])
	) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT [Name] FROM [Feature] WHERE [Name] = 'Audio')
BEGIN
	INSERT INTO [Feature] ([Name], [DisplayName], [Visible]) VALUES ('Audio', '', 0)
END

IF NOT EXISTS (SELECT [Name] FROM [Feature] WHERE [Name] = 'DeviceChannels')
BEGIN
	INSERT INTO [Feature] ([Name], [DisplayName], [Visible]) VALUES ('DeviceChannels', '', 0)
END
GO