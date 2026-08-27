-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[get_custom_property]') AND type in (N'P', N'PC'))
DROP PROCEDURE [get_custom_property]
GO

CREATE PROCEDURE [get_custom_property]
	@object_id VARCHAR(128),
	@name [NameType]
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		[name],
		[value]
	FROM [CustomProperty] WITH (ROWLOCK)
	WHERE [object_id] = @object_id
		AND [name] = @name
END
GO
