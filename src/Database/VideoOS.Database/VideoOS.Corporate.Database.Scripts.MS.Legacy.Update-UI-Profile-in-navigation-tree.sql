-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @navigation XML
SET @navigation = (SELECT CAST(Setting as XML) FROM SystemSettings WHERE Name = 'NavigationXml')

IF(@navigation.exist('/main/entry[@ID="Root"]/entry[@ID="Client"]/entry[@ID="ManagementClientProfiles"]') = 1)
BEGIN
	SET @navigation.modify('delete 
		/main/entry[@ID="Root"]/entry[@ID="Client"]/entry[@ID="ManagementClientProfiles"][1]') 
END		

IF(@navigation.exist('/main/entry[@ID="Root"]/entry[@ID="Client"]/entry[@ID="ClientProfiles"]') = 1)
BEGIN
	SET @navigation.modify('
	insert
		<entry ID="ManagementClientProfiles">
			<text>Management Client Profiles</text>
			<description>Management Client Profiles</description>
			<iconcollapsed>156</iconcollapsed>
			<iconexpanded>156</iconexpanded>
			<data>VideoOS.html</data>
			<launch>
				<instance>BE9A98D4-2F20-4ED3-B666-79668BE80E81</instance>
				<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
				<classname>VideoOS.Administration.AddIn.UserControlUIProfiles</classname>
			</launch>
		</entry> 
		after (/main/entry[@ID="Root"]/entry[@ID="Client"]/entry[@ID="ClientProfiles"])[1]')
     
		UPDATE SystemSettings
		SET Setting = CONVERT(NVARCHAR(MAX), @navigation)
		WHERE Name = 'NavigationXml'
END
ELSE
BEGIN
	SET @navigation.modify('
	insert
		<entry ID="ManagementClientProfiles">
			<text>Management Client Profiles</text>
			<description>Management Client Profiles</description>
			<iconcollapsed>156</iconcollapsed>
			<iconexpanded>156</iconexpanded>
			<data>VideoOS.html</data>
			<launch>
				<instance>BE9A98D4-2F20-4ED3-B666-79668BE80E81</instance>
				<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
				<classname>VideoOS.Administration.AddIn.UserControlUIProfiles</classname>
			</launch>
		</entry> 
		as first into (/main/entry[@ID="Root"]/entry[@ID="Client"])[1]')
     
		UPDATE SystemSettings
		SET Setting = CONVERT(NVARCHAR(MAX), @navigation)
		WHERE Name = 'NavigationXml'
END
GO