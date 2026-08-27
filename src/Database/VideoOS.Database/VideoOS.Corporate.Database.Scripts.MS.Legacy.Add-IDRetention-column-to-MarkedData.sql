-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.columns WHERE [object_id] = OBJECT_ID(N'MarkedData') AND name = N'IDRetention')
BEGIN
	ALTER TABLE MarkedData
	ADD [IDRetention] VARCHAR(50) NULL
END
GO

UPDATE MarkedData
SET IDRetention = NewId()
WHERE IsNull(IDRetention, '') = ''
GO