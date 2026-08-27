-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_hosted_hardware_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_hosted_hardware_ids]
GO

CREATE PROCEDURE [query_hosted_hardware_ids]
	@hosting_server_id INT
AS
BEGIN
	SET NOCOUNT ON

	SELECT [pkid]
	FROM [HostedHardware]
	WHERE [hosting_server_id] = @hosting_server_id
END
GO