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

DECLARE @navigation XML
SET @navigation = (SELECT CAST(Setting as XML) FROM SystemSettings WHERE Name = 'NavigationXml')

SET @navigation.modify('delete (/main/entry[@ID="Root"]/entry[@ID="ServerLogs"]/entry[@ID="EventLog"])')

UPDATE SystemSettings
SET Setting = CONVERT(NVARCHAR(MAX), @navigation)
WHERE Name = 'NavigationXml'

DELETE FROM SystemSettings
WHERE Name IN ('LogEventEnabled', 'LogEventMaxLogEntries', 'LogEventTimeToKeepLogEntries')
GO

