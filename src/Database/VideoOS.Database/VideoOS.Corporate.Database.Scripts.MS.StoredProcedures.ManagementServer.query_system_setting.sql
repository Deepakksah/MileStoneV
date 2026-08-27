-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_system_setting]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_system_setting]
GO

CREATE PROCEDURE [query_system_setting]
	@settingName [NameType]
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT [Setting]
	FROM [SystemSettings]
	WHERE [Name] = @settingName
END
GO