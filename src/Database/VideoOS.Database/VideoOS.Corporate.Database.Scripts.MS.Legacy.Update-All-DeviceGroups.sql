---- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Update AllXxxxGroups to aid translation
UPDATE DeviceGroups
SET [Name] = 'AllCamerasName', [Description] = 'AllCamerasDescription'
WHERE IDDeviceGroup = '0e1b0ad3-f67c-4d5f-b792-4bd6c3cf52f8'

UPDATE DeviceGroups
SET [Name] = 'AllMicrophonesName', [Description] = 'AllMicrophonesDescription'
WHERE IDDeviceGroup = '76D123CB-1E0E-4a2a-BDD5-E4B4B2894614'

UPDATE DeviceGroups
SET [Name] = 'AllSpeakersName', [Description] = 'AllSpeakersDescription'
WHERE IDDeviceGroup = 'D92E5C68-F712-49d7-A99D-F88256645D5A'

UPDATE DeviceGroups
SET [Name] = 'AllInputsName', [Description] = 'AllInputsDescription'
WHERE IDDeviceGroup = '299a8967-caed-4673-9e7a-a71879ab35fe'

UPDATE DeviceGroups
SET [Name] = 'AllOutputsName', [Description] = 'AllOutputsDescription'
WHERE IDDeviceGroup = '80d217d2-b379-4684-a172-ebdc32cd6611'

GO

