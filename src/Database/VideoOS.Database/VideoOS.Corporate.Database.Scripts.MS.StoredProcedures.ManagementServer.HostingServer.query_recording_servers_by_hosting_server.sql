-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_recording_servers_by_hosting_server]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_recording_servers_by_hosting_server]
GO

CREATE PROCEDURE [query_recording_servers_by_hosting_server]
	@pkid INT
AS
BEGIN
	SET NOCOUNT ON

	SELECT DISTINCT hd.[IDRecorder]
	FROM [Hardware] hd
	INNER JOIN [HostedHardware] hh ON hd.[HostedHardware_id] = hh.[pkid]
	INNER JOIN [HostingServer] hs ON hh.[hosting_server_id] = hs.[pkid]
	WHERE hs.[pkid] = @pkid
END
GO