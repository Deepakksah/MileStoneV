-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================


ALTER TABLE [DevicePrivacyMask] DROP CONSTRAINT [PK_DevicePrivacyMask]
GO

ALTER TABLE [DevicePrivacyMask] ADD CONSTRAINT [PK_DevicePrivacyMask] PRIMARY KEY NONCLUSTERED
(
	[IDDevice] ASC,
	[LastModified] ASC
)
GO


DECLARE @OldDate DateTime
SET @OldDate = '1980-01-01T10:00:00'

IF NOT EXISTS(SELECT TOP (1) * FROM DevicePrivacyMask WHERE [LastModified] = @OldDate)
BEGIN
	-- Modify the mask to 'removable' before making a copy
	UPDATE [DevicePrivacyMask]
	SET [PrivacyMask].modify('replace value of (/mask/methods/solid/@removable)[1] with "true"')
	WHERE [PrivacyMask].exist('/mask/methods/solid/@removable') = 1
	
	-- Modify the mask to 'motionDetection' before making a copy
	UPDATE [DevicePrivacyMask]
	SET [PrivacyMask].modify('replace value of (/mask/methods/solid/@motionDetection)[1] with "true"')
	WHERE [PrivacyMask].exist('/mask/methods/solid/@motionDetection') = 1

	-- Add a copy back in time to cover oldest media
	INSERT INTO DevicePrivacyMask ([LastModified], [IDDevice], [PrivacyMask], [Enabled] )
	SELECT @OldDate, [IDDevice], [PrivacyMask], [Enabled] FROM DevicePrivacyMask
END
GO


