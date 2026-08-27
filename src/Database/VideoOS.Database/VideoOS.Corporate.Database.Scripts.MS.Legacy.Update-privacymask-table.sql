-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

UPDATE [DevicePrivacyMask] SET [PrivacyMask] = '' WHERE [PrivacyMask] IS NULL
ALTER TABLE [DevicePrivacyMask] ALTER COLUMN [PrivacyMask] XML NOT NULL
GO


IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE [name] = N'Enabled' AND [object_id] = Object_ID(N'DevicePrivacyMask'))
BEGIN
	ALTER TABLE [DevicePrivacyMask] ADD [Enabled] BIT NULL
END
GO


UPDATE	[DevicePrivacyMask]
SET		[Enabled] = COALESCE([PrivacyMask].value('(/mask/@enabled)[1]', 'BIT'), 0)
WHERE	[Enabled] IS NULL
GO


ALTER TABLE [DevicePrivacyMask] ALTER COLUMN [Enabled] BIT NOT NULL
GO


UPDATE [DevicePrivacyMask]
SET [PrivacyMask].modify('delete (/mask/@enabled)')
GO
