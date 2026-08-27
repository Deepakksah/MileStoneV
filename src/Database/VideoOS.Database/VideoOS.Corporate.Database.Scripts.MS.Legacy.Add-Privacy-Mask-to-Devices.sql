-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Devices]') AND name = N'PrivacyMask')
BEGIN
	ALTER TABLE [Devices]
	ADD [PrivacyMask] [xml] NULL
END
GO

UPDATE Devices
SET [PrivacyMask] = '<PrivacyMask enabled="false" showCells="false" showGrid="false"><Cells noofXCells="0" noofYCells="0"></Cells></PrivacyMask>'
WHERE [PrivacyMask] IS NULL AND [DeviceType] = 'Camera'
GO
