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

SELECT @Value = ''
SET @navigation.modify('
	replace value of (/main/entry[@ID="Root"]/entry[@ID="Devices"]/data/text()) [1]
	with sql:variable("@Value")')
	
SELECT @Value = 'VideoOS.Administration.AddIn.UserControlAutomaticallyDetectedCameras'
SET @navigation.modify('
	replace value of (/main/entry[@ID="Root"]/entry[@ID="Devices"]/launch/classname/text()) [1]
	with sql:variable("@Value")')
	
SELECT @Value = '5CDF5910-F246-40E8-A03E-8169AEB95B05'
SET @navigation.modify('
	replace value of (/main/entry[@ID="Root"]/entry[@ID="Devices"]/launch/instance/text()) [1]
	with sql:variable("@Value")')
	
UPDATE [SystemSettings]
SET [Setting] = CONVERT(NVARCHAR(MAX), @navigation)
WHERE [Name] = 'NavigationXML'

GO