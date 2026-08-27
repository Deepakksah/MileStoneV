---- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Update DEVICE attributes
UPDATE SecurityAttributes
SET DisplayOrder = 1
WHERE SecurityAttributeID = 'DEVICE'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 10
WHERE SecurityAttributeID = 'DEVICEVISIBLE'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 12
WHERE SecurityAttributeID = 'DEVICELIVE'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 14
WHERE SecurityAttributeID = 'DEVICERECORDED'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 16
WHERE SecurityAttributeID = 'DEVICEBOOKMARK'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 18
WHERE SecurityAttributeID = 'DEVICEPTZAUX'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 30
WHERE SecurityAttributeID = 'DEVICELIVEVIEW'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 50
WHERE SecurityAttributeID = 'DEVICERECORDEDBROWSE'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 52
WHERE SecurityAttributeID = 'DEVICERECORDEDGETSEQUENCES'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 54
WHERE SecurityAttributeID = 'DEVICERECORDEDSMARTSEARCH'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 56
WHERE SecurityAttributeID = 'DEVICERECORDEDEXPORTAVI'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 58
WHERE SecurityAttributeID = 'DEVICERECORDEDEXPORTDATABASE'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 70
WHERE SecurityAttributeID = 'DEVICEBOOKMARKVIEW'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 72
WHERE SecurityAttributeID = 'DEVICEBOOKMARKEDIT'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 74
WHERE SecurityAttributeID = 'DEVICEBOOKMARKADD'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 76
WHERE SecurityAttributeID = 'DEVICEBOOKMARKDELETE'
GO

-- Update DEVICEMIC attributes
UPDATE SecurityAttributes
SET DisplayOrder = 1
WHERE SecurityAttributeID = 'DEVICEMIC'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 10
WHERE SecurityAttributeID = 'DEVICEMICVISIBLE'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 12
WHERE SecurityAttributeID = 'DEVICEMICLIVE'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 14
WHERE SecurityAttributeID = 'DEVICEMICRECORDED'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 16
WHERE SecurityAttributeID = 'DEVICEMICBOOKMARK'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 30
WHERE SecurityAttributeID = 'DEVICEMICLIVEVIEW'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 50
WHERE SecurityAttributeID = 'DEVICEMICRECORDEDBROWSE'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 52
WHERE SecurityAttributeID = 'DEVICEMICRECORDEDGETSEQUENCES'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 58
WHERE SecurityAttributeID = 'DEVICEMICRECORDEDEXPORT'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 70
WHERE SecurityAttributeID = 'DEVICEMICBOOKMARKVIEW'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 72
WHERE SecurityAttributeID = 'DEVICEMICBOOKMARKEDIT'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 74
WHERE SecurityAttributeID = 'DEVICEMICBOOKMARKADD'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 76
WHERE SecurityAttributeID = 'DEVICEMICBOOKMARKDELETE'
GO

-- Update DEVICESPEAKER attributes
UPDATE SecurityAttributes
SET DisplayOrder = 1
WHERE SecurityAttributeID = 'DEVICESPEAKER'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 10
WHERE SecurityAttributeID = 'DEVICESPEAKERVISIBLE'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 12
WHERE SecurityAttributeID = 'DEVICESPEAKERLIVE'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 14
WHERE SecurityAttributeID = 'DEVICESPEAKERRECORDED'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 16
WHERE SecurityAttributeID = 'DEVICESPEAKERBOOKMARK'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 30
WHERE SecurityAttributeID = 'DEVICESPEAKERLIVEVIEW'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 50
WHERE SecurityAttributeID = 'DEVICESPEAKERRECORDEDBROWSE'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 52
WHERE SecurityAttributeID = 'DEVICESPEAKERRECORDEDGETSEQUENCES'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 58
WHERE SecurityAttributeID = 'DEVICESPEAKERRECORDEDEXPORT'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 70
WHERE SecurityAttributeID = 'DEVICESPEAKERBOOKMARKVIEW'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 72
WHERE SecurityAttributeID = 'DEVICESPEAKERBOOKMARKEDIT'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 74
WHERE SecurityAttributeID = 'DEVICESPEAKERBOOKMARKADD'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 76
WHERE SecurityAttributeID = 'DEVICESPEAKERBOOKMARKDELETE'
GO

-- Update DEVICEPTZ attributes
UPDATE SecurityAttributes
SET DisplayOrder = 1
WHERE SecurityAttributeID = 'DEVICEPTZ'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 10
WHERE SecurityAttributeID = 'DEVICEPTZCONTROL'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 12
WHERE SecurityAttributeID = 'DEVICEPTZPRESETS'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 14
WHERE SecurityAttributeID = 'DEVICEPTZPRIORITY'
GO

-- Update DEVICESPEAK attributes
UPDATE SecurityAttributes
SET DisplayOrder = 1
WHERE SecurityAttributeID = 'DEVICESPEAK'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 10
WHERE SecurityAttributeID = 'DEVICESPEAKCONTROL'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 12
WHERE SecurityAttributeID = 'DEVICESPEAKPRIORITY'
GO

-- Update APPLICATIONACCESS attributes
UPDATE SecurityAttributes
SET DisplayOrder = 1
WHERE SecurityAttributeID = 'APPLICATIONACCESS'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 10
WHERE SecurityAttributeID = 'APPLICATIONACCESSCLIENTLOGIN'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 12
WHERE SecurityAttributeID = 'APPLICATIONACCESSSTATUSAPI'
GO

UPDATE SecurityAttributes
SET DisplayOrder = 14
WHERE SecurityAttributeID = 'APPLICATIONACCESSSERVICEREGAPI'
GO

