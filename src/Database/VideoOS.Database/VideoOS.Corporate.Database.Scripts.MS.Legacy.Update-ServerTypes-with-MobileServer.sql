-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[ServerType]') AND type = 'U')
BEGIN
	IF (SELECT COUNT(upgrade_code) FROM [ServerType] WHERE [name] = 'MobileServer') = 0
	BEGIN
		INSERT [ServerType]([name], [upgrade_code])
		SELECT 'MobileServer', '{8E0CEC16-139F-45AB-9F4B-F1ADC5A81456}'
	END		
END

GO