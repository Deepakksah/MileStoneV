---- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Update Camera Playback
UPDATE SecurityAttributes
SET TimeBackwardBased = 1, Name = 'Playback video'
WHERE SecurityAttributeID = 'DEVICERECORDEDBROWSE'
GO

-- Update Microphone Playback
UPDATE SecurityAttributes
SET TimeBackwardBased = 1, Name = 'Playback audio'
WHERE SecurityAttributeID = 'DEVICEMICRECORDEDBROWSE'
GO

-- Update Speaker Playback
UPDATE SecurityAttributes
SET TimeBackwardBased = 1, Name = 'Playback audio'
WHERE SecurityAttributeID = 'DEVICESPEAKERRECORDEDBROWSE'
GO
