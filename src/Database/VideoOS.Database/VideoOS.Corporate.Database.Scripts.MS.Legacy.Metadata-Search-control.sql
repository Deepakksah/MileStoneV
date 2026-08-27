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

IF (@navigation.exist('/main/entry[@ID="Root"]/entry[@ID="MetadataUse"][1]') = 1)
BEGIN
	SET @navigation.modify('delete /main/entry[@ID="Root"]/entry[@ID="MetadataUse"][1]')
END

SET @navigation.modify('
insert
    <entry ID=''MetadataUse''>
        <text>Metadata Use</text>
        <description>Metadata Use</description>
        <iconcollapsed>147</iconcollapsed>
        <iconexpanded>147</iconexpanded>
        <data>MetadataUse.html</data>
        <launch>
		   <instance>AA334DC0-158A-4715-A81E-FBD17D6E0998</instance>
		    <assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
		    <classname>VideoOS.Administration.AddIn.UserControlHelp</classname>
        </launch>
        <entry ID=''MetadataSearch''>
            <text>Metadata Search</text>
            <description>Metadata Search</description>
            <iconcollapsed>165</iconcollapsed>
            <iconexpanded>165</iconexpanded>
            <data/>
            <launch>
			    <instance>5C03FA00-A055-4B45-ABA3-9FC57CB8B6FB</instance>
			    <assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
			    <classname>VideoOS.Administration.AddIn.UserControlMetadataSearch</classname>
            </launch>
        </entry>
    </entry>
    after (/main/entry[@ID="Root"]/entry[@ID="ServerLogs"])[1]')
     
    UPDATE SystemSettings
    SET Setting = CONVERT(NVARCHAR(MAX), @navigation)
    OUTPUT CAST(DELETED.Setting AS XML) AS [Before], CAST(INSERTED.Setting AS XML) AS [After]
    WHERE Name = 'NavigationXml'

GO
