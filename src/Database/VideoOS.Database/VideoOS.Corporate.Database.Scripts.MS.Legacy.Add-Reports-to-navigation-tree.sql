-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @navigation XML
SET @navigation = (SELECT CAST(Setting as XML) FROM SystemSettings WHERE Name = 'NavigationXml')

DECLARE @reportNode XML
SET @reportNode = @navigation.query('/main/entry[@ID="Root"]/entry[@ID="SystemDashboard"]/entry[@ID="Reports"]')

IF (LEN(CONVERT(NVARCHAR(MAX), @reportNode)) = 0)
BEGIN
	SELECT @navigation
	SET @navigation.modify('
	insert
		<entry ID="Reports">
			<text>Reports</text>
			<description>Reports</description>
			<iconcollapsed>1</iconcollapsed>
			<iconexpanded>1</iconexpanded>
			<data />
			<launch>
				<instance>667DD8B3-7D76-4E67-BEF1-CAAA4B3C95CC</instance>
				<assemblyfilename>VideoOS.Administration.Addin.dll</assemblyfilename>
				<classname>VideoOS.Administration.AddIn.UserControlReports</classname>
			</launch>
		</entry> 
     after (/main/entry[@ID="Root"]/entry[@ID="SystemDashboard"]/entry[@ID="StorageOverview"])[1]')
    SELECT @navigation
     
     UPDATE SystemSettings
     SET Setting = CONVERT(NVARCHAR(MAX), @navigation)
     WHERE Name = 'NavigationXml'
END
GO