-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_hosted_hardware_not_added]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_hosted_hardware_not_added]
GO

CREATE PROCEDURE [query_hosted_hardware_not_added]
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		hh.[pkid]
		, hh.[created_date]
		, hh.[last_modified]
		, hh.[type]
		, hh.[name]
		, hh.[description]
		, hh.[user_name]
		, hh.[password]
		, hh.[hosting_info]
		, hh.[manufacturer_id]
		, hh.[custom_data]
		, hh.[hosting_server_id]
	FROM
		[HostedHardware] hh LEFT OUTER JOIN [Hardware] h ON h.[HostedHardware_id] = hh.pkid
	WHERE
		h.[IDHardware] IS NULL
END
GO
