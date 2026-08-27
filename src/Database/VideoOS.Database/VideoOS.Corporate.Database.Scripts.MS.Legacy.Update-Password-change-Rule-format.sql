-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @params XML = '<Parameter name="HardwareIds">C5552DB0-A4ED-4BB0-B01C-E96E500D801F</Parameter>'
	 
UPDATE [Rules]
SET [Definition].modify('insert sql:variable("@params") as last into (/Rule/StartRule/Actions/Action[@name="VideoOS.ChangeHardwarePassword"])[1]') 
WHERE Definition.exist('(/Rule/StartRule/Actions/Action[@name="VideoOS.ChangeHardwarePassword"])') = 1 -- it is a ChangeHardwarePassword action
  AND Definition.exist('(/Rule/StartRule/Actions/Action[@name="VideoOS.ChangeHardwarePassword"]/Parameter)') = 0 -- and it is the old format

GO
