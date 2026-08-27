-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================


ALTER TABLE [Devices]
	ALTER COLUMN [SubType] INT NOT NULL
GO

ALTER TABLE [Hardware]
	ALTER COLUMN [SubType] INT NOT NULL
GO
