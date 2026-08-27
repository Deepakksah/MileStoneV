-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'RegisteredDeviceTypes') AND name = N'SecurityNamespaceName')
BEGIN
	ALTER TABLE [RegisteredDeviceTypes]
	ADD [SecurityNamespaceName] NVARCHAR(260)
END
GO

UPDATE [RegisteredDeviceTypes] SET [SecurityNamespaceName] = 'Camera' WHERE [DeviceType] = 'Camera'
UPDATE [RegisteredDeviceTypes] SET [SecurityNamespaceName] = 'Speaker' WHERE [DeviceType] = 'AudioOutput'
UPDATE [RegisteredDeviceTypes] SET [SecurityNamespaceName] = 'Microphone' WHERE [DeviceType] = 'AudioInput'
UPDATE [RegisteredDeviceTypes] SET [SecurityNamespaceName] = 'Input' WHERE [DeviceType] = 'Input'
UPDATE [RegisteredDeviceTypes] SET [SecurityNamespaceName] = 'Output' WHERE [DeviceType] = 'Output'
UPDATE [RegisteredDeviceTypes] SET [SecurityNamespaceName] = 'Metadata' WHERE [DeviceType] = 'Metadata'
GO

ALTER TABLE [RegisteredDeviceTypes]
ALTER COLUMN [SecurityNamespaceName] NVARCHAR(260) NOT NULL
GO
