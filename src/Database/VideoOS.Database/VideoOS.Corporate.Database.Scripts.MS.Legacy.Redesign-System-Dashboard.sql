-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @navigation XML
SELECT @navigation = CAST(Setting as XML)
FROM [SystemSettings]
WHERE [IDSystemSetting] = '5B868950-9A55-4E0E-9F95-710CC42D184C'

-- Reordering and renaming the entries under the System Dashboard node
-- to
-- System Dashboard
--   System Monitor ('Reports' renamed)
--   Current Tasks  ('Storage Overview' renamed)
--   Configuration Reports

IF (@navigation.exist('/main/entry[@ID="Root"]/entry[@ID="SystemDashboard"][1]') = 1)
BEGIN
	-- Delete existing entries in System Dashboard
	
	IF (@navigation.exist('/main/entry[@ID="Root"]/entry[@ID="SystemDashboard"]/entry[@ID="StorageOverview"][1]') = 1)
	BEGIN
		SET @navigation.modify('delete /main/entry[@ID="Root"]/entry[@ID="SystemDashboard"]/entry[@ID="StorageOverview"][1]')
	END
	IF (@navigation.exist('/main/entry[@ID="Root"]/entry[@ID="SystemDashboard"]/entry[@ID="ConfigReports"][1]') = 1)
	BEGIN
		SET @navigation.modify('delete /main/entry[@ID="Root"]/entry[@ID="SystemDashboard"]/entry[@ID="ConfigReports"][1]')
	END
	IF (@navigation.exist('/main/entry[@ID="Root"]/entry[@ID="SystemDashboard"]/entry[@ID="Reports"][1]') = 1)
	BEGIN
		SET @navigation.modify('delete /main/entry[@ID="Root"]/entry[@ID="SystemDashboard"]/entry[@ID="Reports"][1]')
	END

	-- Insert and rename in new order

	IF (@navigation.exist('/main/entry[@ID="Root"]/entry[@ID="SystemDashboard"]/entry[@ID="SystemMonitor"][1]') = 0)
	BEGIN
		SET @navigation.modify('insert
		  <entry ID="SystemMonitor">
			<text>System Monitor</text>
			<description>System Monitor</description>
			<iconcollapsed>135</iconcollapsed>
			<iconexpanded>135</iconexpanded>
			<data />
			<launch>
			  <instance>667DD8B3-7D76-4E67-BEF1-CAAA4B3C95CC</instance>
			  <assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
			  <classname>VideoOS.Administration.AddIn.UserControlReports</classname>
			</launch>
		  </entry>
		  as first into (/main/entry[@ID="Root"]/entry[@ID="SystemDashboard"])[1]')
	END
      
	IF (@navigation.exist('/main/entry[@ID="Root"]/entry[@ID="SystemDashboard"]/entry[@ID="CurrentTasks"][1]') = 0)
	BEGIN
		SET @navigation.modify('insert
		  <entry ID="CurrentTasks">
			<text>Current Tasks</text>
			<description>Current Tasks</description>
			<iconcollapsed>106</iconcollapsed>
			<iconexpanded>106</iconexpanded>
			<data />
			<launch>
			  <instance>AD28373D-31B5-4aae-B23A-67D2D36AD190</instance>
			  <assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
			  <classname>VideoOS.Administration.AddIn.UserControlRecorderOverview</classname>
			</launch>
		  </entry>
		  after (/main/entry[@ID="Root"]/entry[@ID="SystemDashboard"]/entry[@ID="SystemMonitor"])[1]')
	END
      
	IF (@navigation.exist('/main/entry[@ID="Root"]/entry[@ID="SystemDashboard"]/entry[@ID="ConfigReports"][1]') = 0)
	BEGIN
		SET @navigation.modify('insert
		  <entry ID="ConfigReports">
			<text>Configuration Reports</text>
			<description>Configuration Reports</description>
			<iconcollapsed>134</iconcollapsed>
			<iconexpanded>134</iconexpanded>
			<data />
			<launch>
			  <instance>956F226E-99F0-4721-8046-D6B1DEAC0AE3</instance>
			  <assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
			  <classname>VideoOS.Administration.AddIn.UserControlConfigReports</classname>
			</launch>
		  </entry>
		  after (/main/entry[@ID="Root"]/entry[@ID="SystemDashboard"]/entry[@ID="CurrentTasks"])[1]')
	END
     
	UPDATE SystemSettings
	SET Setting = CONVERT(NVARCHAR(MAX), @navigation)
	WHERE [IDSystemSetting] = '5B868950-9A55-4E0E-9F95-710CC42D184C'
END

GO