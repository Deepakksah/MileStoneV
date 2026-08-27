-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[get_custom_properties]') AND type in (N'P', N'PC'))
DROP PROCEDURE [get_custom_properties]
GO

CREATE PROCEDURE [get_custom_properties]
	@object_id VARCHAR(128)
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		cp.[name],
		cp.[value]
	FROM [CustomProperty] cp
	WHERE cp.[object_id] = @object_id
END
GO
