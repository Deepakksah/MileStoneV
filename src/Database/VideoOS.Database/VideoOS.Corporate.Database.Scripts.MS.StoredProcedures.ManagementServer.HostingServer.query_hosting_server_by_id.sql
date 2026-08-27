-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_hosting_server_by_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_hosting_server_by_id]
GO

CREATE PROCEDURE [query_hosting_server_by_id]
	@pkid INT
AS
BEGIN
	SET NOCOUNT ON

	SELECT
			[pkid],
			[created_date],
			[last_modified],
			[type],
			[name],
			[description],
			[user_name],
			[password],
			[internal_address],
			[external_address]
	FROM [HostingServer]
	WHERE [pkid] = @pkid
END
GO