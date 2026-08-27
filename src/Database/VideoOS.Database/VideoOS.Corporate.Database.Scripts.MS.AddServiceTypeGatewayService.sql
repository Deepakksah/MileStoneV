-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ServerType]') AND type = 'U')
BEGIN
	IF (SELECT COUNT(upgrade_code) FROM [ServerType] WHERE [name] = 'GatewayService') = 0
	BEGIN
		INSERT [ServerType]([name], [upgrade_code], [service_type])
		SELECT 'GatewayService', '{D172B768-4F9A-478A-92C7-1C761C66B93B}', 'E46B7BF9-03CE-44EB-BBDC-8BA16D0AAA80'
	END		
END

GO