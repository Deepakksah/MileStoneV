-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @navigation XML
SET @navigation = (SELECT CAST(Setting as XML) FROM SystemSettings WHERE Name = 'NavigationXml')

SET @navigation.modify('delete 
/main/entry[@ID="Root"]/entry[@ID="Basics"]/entry[@ID="GettingStarted"][1]')
    
UPDATE SystemSettings
SET Setting = CONVERT(NVARCHAR(MAX), @navigation)
WHERE Name = 'NavigationXml'
GO
