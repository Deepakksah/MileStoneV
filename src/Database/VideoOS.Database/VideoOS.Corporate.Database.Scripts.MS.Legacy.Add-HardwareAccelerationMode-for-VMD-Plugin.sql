-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
DECLARE @hw XML = '<setting><name>HardwareAccelerationMode</name><value>Off</value></setting>'
	 
UPDATE [RecorderPluginDevice]
SET Settings.modify('insert sql:variable("@hw") as last into (properties/settings)[1]') where
					  IDRecorderPlugin = 'f020c881-72bf-4014-8504-2b4a5a658f1a' and
	                  Settings.exist('(properties/settings/setting [name/text()="HardwareAccelerationMode"])[1]')=0

GO