-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- The SUPPORTS_TRANSACTION property specifies if the scripts supports
-- running in multi-statement transaction or if it should be running
-- using implicit transactions
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

UPDATE Rules
SET [Definition].modify('delete (/Rule/StartRule/Actions/Action[@name="VideoOS.GenerateAlert"])')

DELETE FROM Rules
where [Definition].exist('(/Rule/StartRule/Actions/Action)[1]') = 0

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[AlertTypeGroupMembers]') AND type in (N'U'))
BEGIN
	DROP TABLE AlertTypeGroupMembers
END

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[AlertTypeGroups]') AND type in (N'U'))
BEGIN
	DROP TABLE AlertTypeGroups
END

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[AlertTypes]') AND type in (N'U'))
BEGIN
	DROP TABLE AlertTypes
END

DECLARE @navigation XML
SET @navigation = (SELECT CAST(Setting as XML) FROM SystemSettings WHERE Name = 'NavigationXml')

SET @navigation.modify(
'
	replace value of (/main/entry[@ID="Root"]/entry[@ID="RulesAlerts"]/@ID)[1]
	with "RulesEvents"
')

SET @navigation.modify(
'
	replace value of (/main/entry[@ID="Root"]/entry[@ID="RulesEvents"]/description/text())[1]
	with "Rules and Events"
')

SET @navigation.modify(
'
	replace value of (/main/entry[@ID="Root"]/entry[@ID="RulesEvents"]/text/text())[1]
	with "Rules and Events"
')

SET @navigation.modify('delete (/main/entry[@ID="Root"]/entry[@ID="RulesEvents"]/entry[@ID="Alerts"])')

SET @navigation.modify('delete (/main/entry[@ID="Root"]/entry[@ID="ServerLogs"]/entry[@ID="AlertLog"])')

UPDATE SystemSettings
SET Setting = CONVERT(NVARCHAR(MAX), @navigation)
WHERE Name = 'NavigationXml'

UPDATE ClientProfile
SET Settings.modify('delete (/Settings/Setting[@key="PanesPlaybackAlerts"])')

DELETE FROM SystemSettings
WHERE Name IN ('LogAlertEnabled', 'LogAlertMaxLogEntries', 'LogAlertTimeToKeepLogEntries')
GO

