-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

declare @navigation xml
declare @Value varchar(255)
select @navigation = setting from SystemSettings where IDSystemSetting = '5B868950-9A55-4E0E-9F95-710CC42D184C'

SELECT @Value = 'Server.html'
SET @navigation.modify('
	replace value of (/main/entry[@ID="Root"]/data/text()) [1]
	with sql:variable("@Value")')

SELECT @Value = 'Basics.html'
SET @navigation.modify('
	replace value of (/main/entry[@ID="Root"]/entry[@ID="Basics"]/data/text()) [1]
	with sql:variable("@Value")')

SELECT @Value = 'Client.html'
SET @navigation.modify('
	replace value of (/main/entry[@ID="Root"]/entry[@ID="Client"]/data/text()) [1]
	with sql:variable("@Value")')

SELECT @Value = 'Devices.html'
SET @navigation.modify('
	replace value of (/main/entry[@ID="Root"]/entry[@ID="Devices"]/data/text()) [1]
	with sql:variable("@Value")')

SELECT @Value = 'Servers.html'
SET @navigation.modify('
	replace value of (/main/entry[@ID="Root"]/entry[@ID="Servers"]/data/text()) [1]
	with sql:variable("@Value")')

SELECT @Value = 'Rules.html'
SET @navigation.modify('
	replace value of (/main/entry[@ID="Root"]/entry[@ID="RulesAlerts"]/data/text()) [1]
	with sql:variable("@Value")')
	
SELECT @Value = 'Security.html'
SET @navigation.modify('
	replace value of (/main/entry[@ID="Root"]/entry[@ID="Security"]/data/text()) [1]
	with sql:variable("@Value")')

SELECT @Value = 'ServerLogs.html'
SET @navigation.modify('
	replace value of (/main/entry[@ID="Root"]/entry[@ID="ServerLogs"]/data/text()) [1]
	with sql:variable("@Value")')
	
SELECT @Value = 'SystemDashboard.html'
SET @navigation.modify('
	replace value of (/main/entry[@ID="Root"]/entry[@ID="SystemDashboard"]/data/text()) [1]
	with sql:variable("@Value")')
	
UPDATE [SystemSettings]
SET [Setting] = CONVERT(NVARCHAR(MAX), @navigation)
WHERE [Name] = 'NavigationXML'

GO