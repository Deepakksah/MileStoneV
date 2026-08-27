-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_time_profiles]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_time_profiles]
GO

CREATE PROCEDURE [query_time_profiles]
AS
BEGIN
	SET NOCOUNT ON

	SELECT
			[pkid],
			[name],
			[description],
			[xml_data],
			[type],
			[created_date],
			[last_modified]
	FROM [TimeProfile]
END
GO