-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_rules_by_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_rules_by_ids]
GO

CREATE PROCEDURE [query_rules_by_ids]
	@ids tvp_StringTableGuidType READONLY
AS
BEGIN
	SET NOCOUNT ON

	SELECT  r.[IDRule],
			[Name],
			[Description],
			[LastModified],
			[Definition],
			[Enabled]
	FROM    [Rules] r
			INNER JOIN @ids ids ON r.[IDRule] = ids.[ID]
END
GO