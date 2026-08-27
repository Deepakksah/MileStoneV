-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================


UPDATE [Devices]
SET [EdgeTrackId] = '00000000-0000-0000-0000-000000000000'
WHERE [EdgeTrackId] = '                                    '
GO

ALTER TABLE [Devices]
ALTER COLUMN[EdgeTrackId] UniqueIdentifier NOT NULL
GO
