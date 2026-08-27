-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[ServerGroupKind]') AND name = N'CustomData')
BEGIN
	ALTER TABLE [ServerGroupKind]
	ADD CustomData NVARCHAR(MAX) NULL
END
GO
