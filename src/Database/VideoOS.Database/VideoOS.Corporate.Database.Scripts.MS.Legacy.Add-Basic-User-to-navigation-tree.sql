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
SET @basicUserNode = @navigation.query('/main/entry[@ID="Root"]/entry[@ID="Security"]/entry[@ID="BasicUser"]')

IF (LEN(CONVERT(NVARCHAR(MAX), @basicUserNode)) = 0)
BEGIN
	SELECT @navigation
	SET @navigation.modify('
	insert
		<entry ID="BasicUser">
			<text>Basic Users</text>
			<description>Basic Users</description>
			<iconcollapsed>139</iconcollapsed>
			<iconexpanded>139</iconexpanded>
			<data>VideoOS.html</data>
			<launch>
				<instance>7CC366D6-7554-4ADE-BB98-5AB0EDCCB46C</instance>
				<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
				<classname>VideoOS.Administration.AddIn.UserControlBasicUser</classname>
			</launch>
		</entry> 
     after (/main/entry[@ID="Root"]/entry[@ID="Security"]/entry[@ID="Roles"])[1]')
    SELECT @navigation

     UPDATE SystemSettings
     SET Setting = CONVERT(NVARCHAR(MAX), @navigation)
     WHERE Name = 'NavigationXml'
END
GO
