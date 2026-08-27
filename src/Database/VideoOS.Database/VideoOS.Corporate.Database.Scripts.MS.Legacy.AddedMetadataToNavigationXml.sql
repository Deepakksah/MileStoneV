-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @navigation XML
SET @navigation = (SELECT CAST(Setting as XML) FROM SystemSettings WHERE Name = 'NavigationXml')

DECLARE @metadataGrpNode XML
SET @metadataGrpNode = @navigation.query('/main/entry[@ID="Root"]/entry[@ID="Devices"]/entry[@ID="Metadata"]')

IF (LEN(CONVERT(NVARCHAR(MAX), @metadataGrpNode)) = 0)
BEGIN
	SELECT @navigation
	SET @navigation.modify('
	insert
		<entry ID="Metadata">
			<text>Metadata</text>
			<description>Metadata</description>
			<iconcollapsed>147</iconcollapsed>
			<iconexpanded>147</iconexpanded>
			<data>Metadata</data>
			<launch>
				<instance>4994BE14-56BA-4427-B0E2-24A3600A21F5</instance>
				<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
				<classname>VideoOS.Administration.AddIn.UserControlDevices</classname>
			</launch>
		</entry>
     after (/main/entry[@ID="Root"]/entry[@ID="Devices"]/entry[@ID="AudioOutput"])[1]')
    SELECT @navigation
     
     UPDATE SystemSettings
     SET Setting = CONVERT(NVARCHAR(MAX), @navigation)
     WHERE Name = 'NavigationXml'
END
GO
