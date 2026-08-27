-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @DefaultMotionDetectionSettingsXml XML 
SET @DefaultMotionDetectionSettingsXml = N'<properties>
										  <settings>
											<setting>
											  <name>Enabled</name>
											  <value>True</value>
											</setting>
											<setting>
											  <name>MotionPixCountThreshold</name>
											  <value>2000</value>
											</setting>
											<setting>
											  <name>MotionPixThreshold</name>
											  <value>100</value>
											</setting>
											<setting>
											  <name>PixelStep</name>
											  <value>3</value>
											</setting>
											<setting>
											  <name>TimeThresholdMs</name>
											  <value>500</value>
											</setting>
											<setting>
											  <name>KeyFramesOnly</name>
											  <value>True</value>
											</setting>
											<setting>
											  <name>ManualSensitivity</name>
											  <value>False</value>
											</setting>
											<setting>
											  <name>GenerateSearchData</name>
											  <value>True</value>
											</setting>
											<setting>
											  <name>UseRegions</name>
											  <value>False</value>
											</setting>
											<setting>
											  <name>RegionSettings</name>
											  <value />
											</setting>
										  </settings>
										</properties>'
										
DECLARE @DefaultMotionDetectionUISettingsXml XML 
SET @DefaultMotionDetectionUISettingsXml = N'<properties>
  <settings>
    <setting>
      <name>ShowRegions</name>
      <value>True</value>
    </setting>
    <setting>
      <name>ShowGrid</name>
      <value>True</value>
    </setting>
  </settings>
</properties>'

INSERT INTO [RecorderPluginDevice] ([LastModified],[IDDevice],[ProcessOrder],[IDRecorderPlugin],[Settings],[UISettings],[Enabled])
SELECT GETUTCDATE(), IDDevice, 0, 'f020c881-72bf-4014-8504-2b4a5a658f1a', @DefaultMotionDetectionSettingsXml, @DefaultMotionDetectionUISettingsXml, 0
FROM [Devices]
WHERE [DeviceType] = 'Camera'
AND [IDDevice] NOT IN 
(
	SELECT IDDevice
	FROM [RecorderPluginDevice]
	WHERE IDRecorderPlugin = 'f020c881-72bf-4014-8504-2b4a5a658f1a'
)
GO
