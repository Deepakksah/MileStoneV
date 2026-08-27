-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================


UPDATE [Rules]
SET [Definition].modify('delete (/Rule/StartRule/Actions/Action[@name="VideoOS.SaveImageAttachments"])')
WHERE [Definition].exist('(/Rule/StartRule/Actions/Action)[@name="VideoOS.SaveImageAttachments"]') = 1

DELETE FROM [Rules]
WHERE [Definition].exist('(/Rule/StartRule/Actions/Action)[1]') = 0

DELETE FROM [EventTypes]
WHERE [IDEventType] = 'C22C1456-9319-455F-A82C-07C8F87B0B50'

GO