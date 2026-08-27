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
WHERE Name = 'NavigationXml'

IF (@navigation.exist('/main/entry[@ID="Root"]/entry[@ID="ServerLogs"][1]') = 1)
BEGIN
	SET @navigation.modify('delete /main/entry[@ID="Root"]/entry[@ID="ServerLogs"][1]')
END

SET @navigation.modify('
insert
    <entry ID=''ServerLogs''>
        <text>Server Logs</text>
        <description>Server logs</description>
        <iconcollapsed>19</iconcollapsed>
        <iconexpanded>19</iconexpanded>
        <data/>
        <launch>
			<instance>0E17A149-2380-4501-ACF3-05DA7492123C</instance>
			<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
			<classname>VideoOS.Administration.AddIn.UserControlLogViewer</classname>
        </launch>
    </entry>
    after (/main/entry[@ID="Root"]/entry[@ID="SystemDashboard"])[1]')
     
    UPDATE SystemSettings
    SET Setting = CONVERT(NVARCHAR(MAX), @navigation)
    OUTPUT CAST(DELETED.Setting AS XML) AS [Before], CAST(INSERTED.Setting AS XML) AS [After]
    WHERE Name = 'NavigationXml'

GO

