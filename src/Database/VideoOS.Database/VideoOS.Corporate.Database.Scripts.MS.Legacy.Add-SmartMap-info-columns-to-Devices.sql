-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Add [ShortName] columns to the [Devices] table as nvarchar(128)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Devices]') AND name = N'ShortName')
BEGIN
	ALTER TABLE [Devices]
	ADD [ShortName] nvarchar(128) NOT NULL
		CONSTRAINT DF_ShortName DEFAULT ''
END
GO

-- Add [Icon] columns to the [Devices] table as int
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Devices]') AND name = N'Icon')
BEGIN
	ALTER TABLE [Devices]
	ADD [Icon] [int] NOT NULL
		CONSTRAINT DF_Icon DEFAULT 0
END
GO

-- Add [CoverageDirection] columns to the [Devices] table as float
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Devices]') AND name = N'CoverageDirection')
BEGIN
	ALTER TABLE [Devices]
	ADD [CoverageDirection] [float] NOT NULL
		CONSTRAINT DF_CoverageDirection DEFAULT 0.0
END
GO

-- Add [CoverageDepth] columns to the [Devices] table as float
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Devices]') AND name = N'CoverageDepth')
BEGIN
	ALTER TABLE [Devices]
	ADD [CoverageDepth] [float] NOT NULL
		CONSTRAINT DF_CoverageDepth DEFAULT 0.0
END
GO

-- Add [CoverageFieldOfView] columns to the [Devices] table as float
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Devices]') AND name = N'CoverageFieldOfView')
BEGIN
	ALTER TABLE [Devices]
	ADD [CoverageFieldOfView] [float] NOT NULL
		CONSTRAINT DF_CoverageFieldOfView DEFAULT 0.0
END
GO
