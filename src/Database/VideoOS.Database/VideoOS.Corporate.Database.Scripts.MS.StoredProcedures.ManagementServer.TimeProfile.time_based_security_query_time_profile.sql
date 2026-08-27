-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[time_based_security_query_time_profile]') AND type in (N'P', N'PC'))
DROP PROCEDURE [time_based_security_query_time_profile]
GO

CREATE PROCEDURE [time_based_security_query_time_profile]
	@pkid [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	SELECT
			[pkid],
			[name],
			[description],
			[xml_data],
			[type],
			[last_modified]
	FROM [TimeProfile]
	WHERE [pkid] = @pkid
END
GO
