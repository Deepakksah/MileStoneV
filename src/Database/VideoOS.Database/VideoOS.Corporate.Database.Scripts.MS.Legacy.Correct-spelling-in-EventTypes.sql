-- This header is used by the dbupdate tool.
-- The TARGET poperty is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

UPDATE [dbo].[EventTypes]
SET [Definition].modify('replace value of (EventType/Sources/Source/Type/text())[1] with "GenericServer"')
WHERE IDEventType = 'D1603FBE-4BB8-49EB-9928-0E8EB3C736CE' 
OR IDEventType = 'F80AF822-6A89-4C2F-AB38-B55D0D2CD13A'
GO