-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @navTree NVARCHAR(MAX)
DECLARE @navTreeXml XML

SELECT @navTree = s.[Setting]
FROM [SystemSettings] s
WHERE s.[Name] = 'NavigationXML'

SET @navTreeXml = CONVERT(XML, @navTree)

-- Inserting "System Dashboard" node with "Storage Overview" child after "Security" node.
SET @navTreeXml.modify('
	insert  <entry ID=''SystemDashboard''>
				<text>System Dashboard</text>
				<description>System Dashboard</description>
				<iconcollapsed>105</iconcollapsed>
				<iconexpanded>105</iconexpanded>
				<data>VideoOS.html</data>
				<launch>
					<instance>D4474C73-8924-41dc-B7DD-72559182C41E</instance>
					<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
					<classname>VideoOS.Administration.AddIn.UserControlHelp</classname>
				</launch>
				<entry ID=''StorageOverview''>
					<text>Storage Overview</text>
					<description>Storage Overview</description>
					<iconcollapsed>106</iconcollapsed>
					<iconexpanded>106</iconexpanded>
					<data/>
					<launch>
						<instance>AD28373D-31B5-4aae-B23A-67D2D36AD190</instance>
						<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
						<classname>VideoOS.Administration.AddIn.UserControlStorageOverview</classname>
					</launch>
				</entry>
			</entry>
	after (/main/entry/entry[@ID=''Security''])[1]')

UPDATE [SystemSettings]
SET [Setting] = CONVERT(NVARCHAR(MAX), @navTreeXml)
WHERE [Name] = 'NavigationXML'

GO
