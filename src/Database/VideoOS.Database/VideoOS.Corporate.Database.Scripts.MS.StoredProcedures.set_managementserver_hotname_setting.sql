-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[set_managementserver_hostname_setting]') AND type in (N'P', N'PC'))
DROP PROCEDURE [set_managementserver_hostname_setting]
GO

CREATE PROCEDURE [set_managementserver_hostname_setting]
	@settingName VARCHAR(255),
	@hostName VARCHAR(255)
AS
BEGIN
	SET NOCOUNT ON

	IF(NOT EXISTS(SELECT [Name] FROM [SystemSettings] WHERE [Name] = @settingName))
	BEGIN
		INSERT INTO SystemSettings
		VALUES
		(
			GetUtcDate(), @settingName, @hostName
		)		
	END
	ELSE
	BEGIN
		UPDATE [SystemSettings]
		SET [Setting] = @hostName
		WHERE [Name] = @settingName
	END
END
GO
