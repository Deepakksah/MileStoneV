-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Add [PickerText2] columns to the [MIPAction] table as NameType
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[MIPAction]') AND name = N'PickerText2')
BEGIN
	ALTER TABLE [MIPAction]
	ADD [PickerText2] NameType NOT NULL
		CONSTRAINT DF_PickerText2 DEFAULT ''
END
GO

-- Add [MIPKinds2] columns to the [MIPAction] table as XML
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[MIPAction]') AND name = N'MIPKinds2')
BEGIN
	ALTER TABLE [MIPAction]
	ADD [MIPKinds2] XML NOT NULL
		CONSTRAINT DF_MIPKinds2 DEFAULT '<MIPKinds />'
END
GO
