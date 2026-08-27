-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @navigation XML
SET @navigation = (SELECT CAST(Setting as XML) FROM SystemSettings WHERE Name = 'NavigationXml')

DECLARE @basicUserNode XML
SET @basicUserNode = @navigation.query('/main/entry[@ID="Root"]/entry[@ID="Basics"]/entry[@ID="BasicInfo"]')

IF (LEN(CONVERT(NVARCHAR(MAX), @basicUserNode)) = 0)
BEGIN
	SELECT @navigation
	SET @navigation.modify('
	insert
		<entry ID="BasicInfo">
			<text>Site Information</text>
			<description>Site Information</description>
			<iconcollapsed>55</iconcollapsed>
			<iconexpanded>55</iconexpanded>
			<data />
			<launch>
				<instance>7CC366D6-7554-4ADE-BB98-5AB0EDCCB47D</instance>
				<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
				<classname>VideoOS.Administration.AddIn.UserControlBasicInformationAddIn</classname>
			</launch>
		</entry> 
     after (/main/entry[@ID="Root"]/entry[@ID="Basics"]/entry[@ID="LicenseInformation"])[1]')
    SELECT @navigation

     UPDATE SystemSettings
     SET Setting = CONVERT(NVARCHAR(MAX), @navigation)
     WHERE Name = 'NavigationXml'
END
GO
