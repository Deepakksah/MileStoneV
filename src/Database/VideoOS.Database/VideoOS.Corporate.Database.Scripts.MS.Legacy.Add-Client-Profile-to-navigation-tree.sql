-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @navigation XML
SET @navigation = (SELECT CAST(Setting as XML) FROM SystemSettings WHERE Name = 'NavigationXml')

DECLARE @clientNode XML
SET @clientNode = @navigation.query('/main/entry[@ID="Root"]/entry[@ID="Client"]/entry[@ID="ClientProfiles"]')

IF (LEN(CONVERT(NVARCHAR(MAX), @clientNode)) = 0)
BEGIN
	SELECT @navigation
	SET @navigation.modify('
	insert
		<entry ID="ClientProfiles">
			<text>Client Profiles</text>
			<description>Client Profiles</description>
			<iconcollapsed>115</iconcollapsed>
			<iconexpanded>115</iconexpanded>
			<data>VideoOS.html</data>
			<launch>
				<instance>B64A6CA6-AE59-4032-A92A-72B06B3F4F5D</instance>
				<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
				<classname>VideoOS.Administration.AddIn.UserControlClientProfiles</classname>
			</launch>
		</entry> 
     before (/main/entry[@ID="Root"]/entry[@ID="Client"]/entry[@ID="Matrix"])[1]')
    SELECT @navigation
     
     UPDATE SystemSettings
     SET Setting = CONVERT(NVARCHAR(MAX), @navigation)
     WHERE Name = 'NavigationXml'
END
GO
