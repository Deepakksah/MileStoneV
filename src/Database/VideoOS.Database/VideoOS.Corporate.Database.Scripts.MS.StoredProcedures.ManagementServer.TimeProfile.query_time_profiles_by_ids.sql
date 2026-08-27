-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_time_profiles_by_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_time_profiles_by_ids]
GO

CREATE PROCEDURE [query_time_profiles_by_ids]
	@timeProfileIds tvp_StringTableGuidType READONLY
AS
BEGIN
	SET NOCOUNT ON

	SELECT
			t.[pkid],
			t.[name],
			t.[description],
			t.[xml_data],
			t.[type],
			t.[created_date],
			t.[last_modified]
	FROM [TimeProfile] t
	INNER JOIN @timeProfileIds ids ON t.[pkid] = ids.[ID]
END
GO