-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @navigation XML
SET @navigation = (SELECT CAST(Setting as XML) FROM SystemSettings WHERE Name = 'NavigationXml')

IF(@navigation.exist('/main/entry[@ID="Root"]/entry[@ID="SystemDashboard"]/entry[@ID="CurrentTasks"][1]') = 1)
BEGIN
	SET @navigation.modify('delete 
		/main/entry[@ID="Root"]/entry[@ID="SystemDashboard"]/entry[@ID="CurrentTasks"][1]') 
END		

BEGIN
	SET @navigation.modify('
	insert
      <entry ID="CurrentTasks">
        <text>Current Tasks</text>
        <description>Current Tasks</description>
        <iconcollapsed>106</iconcollapsed>
        <iconexpanded>106</iconexpanded>
        <data />
        <launch>
          <instance>AD28373D-31B5-4aae-B23A-67D2D36AD190</instance>
          <assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
          <classname>VideoOS.Administration.AddIn.UserControlCurrentTasksOverview</classname>
        </launch>
      </entry>
		  as first into (/main/entry[@ID="Root"]/entry[@ID="SystemDashboard"])[1]')
     
		UPDATE SystemSettings
		SET Setting = CONVERT(NVARCHAR(MAX), @navigation)
		WHERE Name = 'NavigationXml'
END
GO