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

IF (@navigation.exist('/main/entry[@ID="Root"]/entry[@ID="SystemDashboard"]/entry[@ID="ConfigReports"][1]') = 0)
BEGIN
  SET @navigation.modify('insert
      <entry ID="ConfigReports">
        <text>Configuration Reports</text>
        <description>Configuration Reports</description>
        <iconcollapsed>134</iconcollapsed>
        <iconexpanded>134</iconexpanded>
        <data/>
        <launch>
          <instance>956F226E-99F0-4721-8046-D6B1DEAC0AE3</instance>
          <assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
          <classname>VideoOS.Administration.AddIn.UserControlConfigReports</classname>
        </launch>
      </entry>
     after (/main/entry[@ID="Root"]/entry[@ID="SystemDashboard"]/entry[@ID="StorageOverview"])[1]')
     
  UPDATE SystemSettings
  SET Setting = CONVERT(NVARCHAR(MAX), @navigation)
  WHERE [IDSystemSetting] = '5B868950-9A55-4E0E-9F95-710CC42D184C'
END

GO
