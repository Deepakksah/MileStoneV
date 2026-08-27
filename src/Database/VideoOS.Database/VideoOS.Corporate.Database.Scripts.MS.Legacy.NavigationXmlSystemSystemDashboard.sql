-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
DECLARE @navigation XML
SET @navigation = (SELECT CAST(Setting as XML) FROM SystemSettings WHERE Name = 'NavigationXml')

IF(@navigation.exist('/main/entry[@ID="Root"]/entry[@ID="SystemDashboard"]/entry[@ID="SystemMonitor"][1]') = 1)
BEGIN
	SET @navigation.modify('delete 
		/main/entry[@ID="Root"]/entry[@ID="SystemDashboard"]/entry[@ID="SystemMonitor"][1]') 
END		

BEGIN
	SET @navigation.modify('
	insert
		<entry ID="SystemMonitor">
			<text>System Monitor</text>
			<description>System Monitor</description>
			<iconcollapsed>135</iconcollapsed>
			<iconexpanded>135</iconexpanded>
			<data />
			<launch>
			  <instance>667DD8B3-7D76-4E67-BEF1-CAAA4B3C95CC</instance>
			  <assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
			  <classname>VideoOS.Administration.AddIn.SystemMonitorDashboardUserControl</classname>
			</launch>
		  </entry>
		  as first into (/main/entry[@ID="Root"]/entry[@ID="SystemDashboard"])[1]')
     
		UPDATE SystemSettings
		SET Setting = CONVERT(NVARCHAR(MAX), @navigation)
		WHERE Name = 'NavigationXml'
END
GO