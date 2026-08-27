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

IF (@navigation.exist('/main/entry[@ID="Root"]/entry[@ID="Client"]/entry[@ID="VideoWall"][1]') = 0)
BEGIN
	SET @navigation.modify('
	insert
        <entry ID=''VideoWall''>
            <text>Smart wall</text>
            <description>Smart wall</description>
            <iconcollapsed>76</iconcollapsed>
            <iconexpanded>76</iconexpanded>
            <data/>
            <launch>
                <instance>4EFA7D4E-7BF3-4D4F-BAAF-D88474A30EC5</instance>
                <assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
                <classname>VideoOS.Administration.AddIn.UserControlVideoWall</classname>
            </launch>
        </entry>
     before (/main/entry[@ID="Root"]/entry[@ID="Client"]/entry[@ID="ViewGroups"])[1]')
     
     UPDATE SystemSettings
     SET Setting = CONVERT(NVARCHAR(MAX), @navigation)
     OUTPUT CAST(DELETED.Setting AS XML) AS [Before], CAST(INSERTED.Setting AS XML) AS [After]
     WHERE [IDSystemSetting] = '5B868950-9A55-4E0E-9F95-710CC42D184C'
END
GO

