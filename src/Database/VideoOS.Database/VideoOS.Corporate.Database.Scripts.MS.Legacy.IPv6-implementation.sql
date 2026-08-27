-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Adds column IPScanRangeWarning to Recorders
IF NOT EXISTS (
	SELECT * 
	FROM sys.columns 
	WHERE object_id = OBJECT_ID(N'[Recorders]') AND 
		name = N'IPScanRangeWarning')
BEGIN
	ALTER TABLE [Recorders]
		ADD [IPScanRangeWarning] int NULL
END
GO

UPDATE [Recorders]
	SET IPScanRangeWarning=255
	WHERE IPScanRangeWarning IS NULL
GO

IF NOT EXISTS(SELECT * FROM [SystemSettings] WHERE [IDSystemSetting] = '7E4B4214-7576-4d79-8ED2-3C36AB762C0F')
BEGIN
	INSERT [SystemSettings]
	VALUES(GETUTCDATE(), '7E4B4214-7576-4d79-8ED2-3C36AB762C0F', 'ServerRecorderIPv6Comm', 'False')
END
GO
