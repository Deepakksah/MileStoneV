-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

declare @navigation xml
declare @Value varchar(255)
select @navigation = setting from SystemSettings where Name = 'NavigationXML'

SELECT @Value = 'Devices.html'
SET @navigation.modify('
	delete (/main/entry[@ID="Root"]/entry[@ID="Devices"]/data)')

SET @navigation.modify('
	insert <data>{sql:variable("@Value")}</data>
	after (/main/entry[@ID="Root"]/entry[@ID="Devices"]/iconexpanded)[1]')

SELECT @Value = 'VideoOS.Administration.AddIn.UserControlHelp'
SET @navigation.modify('
	replace value of (/main/entry[@ID="Root"]/entry[@ID="Devices"]/launch/classname/text()) [1]
	with sql:variable("@Value")')
	
SELECT @Value = 'D4474C73-8924-41dc-B7DD-72559182C41E'
SET @navigation.modify('
	replace value of (/main/entry[@ID="Root"]/entry[@ID="Devices"]/launch/instance/text()) [1]
	with sql:variable("@Value")')
	
UPDATE [SystemSettings]
SET [Setting] = CONVERT(NVARCHAR(MAX), @navigation)
WHERE [Name] = 'NavigationXML'

GO
