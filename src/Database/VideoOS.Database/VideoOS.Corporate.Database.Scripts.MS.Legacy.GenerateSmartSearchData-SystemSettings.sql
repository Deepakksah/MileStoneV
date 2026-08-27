-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT Name FROM SystemSettings WHERE Name = 'AdministratorGenerateSearchData')
BEGIN
	INSERT INTO SystemSettings
	VALUES (GetUtcDate(), NewId(), 'AdministratorGenerateSearchData', 'True')
END
GO

UPDATE [RecorderPluginDevice]
	SET Settings.modify('
	insert
		<setting>
		  <name>ManualSensitivity</name>
		  <value>True</value>
		</setting>
     as last into (/properties/settings)[1]')
	WHERE IDRecorderPlugin = 'f020c881-72bf-4014-8504-2b4a5a658f1a'
	AND Settings.exist('(/properties/settings/setting/name[text() = "ManualSensitivity"])') = 0

UPDATE [RecorderPluginDevice]
	SET Settings.modify('
	insert
		<setting>
		  <name>GenerateSearchData</name>
		  <value>False</value>
		</setting>
     as last into (/properties/settings)[1]')
	WHERE IDRecorderPlugin = 'f020c881-72bf-4014-8504-2b4a5a658f1a'
	AND Settings.exist('(/properties/settings/setting/name[text() = "GenerateSearchData"])') = 0

GO
