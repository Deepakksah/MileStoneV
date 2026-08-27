-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @navigation XML
SET @navigation = (SELECT CAST(Setting as XML) FROM SystemSettings WHERE Name = 'NavigationXml')

DECLARE @evidenceLockNode XML
SET @evidenceLockNode = @navigation.query('/main/entry[@ID="Root"]/entry[@ID="SystemDashboard"]/entry[@ID="EvidenceLock"]')

IF (LEN(CONVERT(NVARCHAR(MAX), @evidenceLockNode)) = 0)
BEGIN
	SELECT @navigation
	SET @navigation.modify('
	insert
		<entry ID="EvidenceLock">
			<text>Evidence Lock</text>
			<description>Evidence Lock</description>
			<iconcollapsed>139</iconcollapsed>
			<iconexpanded>139</iconexpanded>
			<data>VideoOS.html</data>
			<launch>
				<instance>BC4A0485-8FF5-43F1-8EE2-C41696F213EF</instance>
				<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
				<classname>VideoOS.Administration.AddIn.UserControlEvidenceLockAddIn</classname>
			</launch>
		</entry> 
     after (/main/entry[@ID="Root"]/entry[@ID="SystemDashboard"]/entry[@ID="SystemMonitor"])[1]')
    SELECT @navigation

     UPDATE SystemSettings
     SET Setting = CONVERT(NVARCHAR(MAX), @navigation)
     WHERE Name = 'NavigationXml'
END
GO