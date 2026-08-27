-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dataset_hosted_hardware_select_by_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dataset_hosted_hardware_select_by_ids]
GO

CREATE PROCEDURE [dataset_hosted_hardware_select_by_ids]
	@ids XML
AS
BEGIN
	SELECT [pkid], [created_date], [last_modified], [type], [name], [description], [user_name], [password], [hosting_info], [manufacturer_id], [custom_data], [hosting_server_id]
	FROM [HostedHardware] hh
	INNER JOIN XmlToIdTable(@ids) ids ON hh.[pkid] = ids.[ID]
END
GO