-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dataset_hosting_server_select_by_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dataset_hosting_server_select_by_ids]
GO

CREATE PROCEDURE [dataset_hosting_server_select_by_ids]
	@ids XML		
AS
BEGIN
	SELECT [pkid], [created_date], [last_modified], [type], [name], [description], [user_name], [password], [internal_address], [external_address]
	FROM [HostingServer] hs
	INNER JOIN XmlToIdTable(@ids) ids ON hs.[pkid] = ids.[ID]
END
GO