-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @navigation XML
SET @navigation = (SELECT CAST(Setting as XML) FROM SystemSettings WHERE Name = 'NavigationXml')

DECLARE @sysDashboardNode XML
SET @sysDashboardNode = @navigation.query('/main/entry[@ID="Root"]/entry[@ID="SystemDashboard"]/entry[@ID="Thresholds"]')

IF (LEN(CONVERT(NVARCHAR(MAX), @sysDashboardNode)) = 0)
BEGIN
	SELECT @navigation
	SET @navigation.modify('
	insert
		<entry ID="Thresholds">
			<text>System Monitor Thresholds</text>
			<description>Defining event thresholds on system monitor data</description>
			<iconcollapsed>161</iconcollapsed>
			<iconexpanded>161</iconexpanded>
			<data>VideoOS.html</data>
			<launch>
				<instance>5753D8F7-0BC0-472A-96AE-4E274203677C</instance>
				<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
				<classname>VideoOS.Administration.AddIn.SystemMonitorThresholds</classname>
			</launch>
		</entry> 
     after (/main/entry[@ID="Root"]/entry[@ID="SystemDashboard"]/entry[@ID="SystemMonitor"])[1]')
    SELECT @navigation

     UPDATE SystemSettings
     SET Setting = CONVERT(NVARCHAR(MAX), @navigation)
     WHERE Name = 'NavigationXml'
END
GO