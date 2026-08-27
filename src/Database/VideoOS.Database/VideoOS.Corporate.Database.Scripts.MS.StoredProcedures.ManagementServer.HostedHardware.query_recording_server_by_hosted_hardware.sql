-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_recording_server_by_hosted_hardware]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_recording_server_by_hosted_hardware]
GO

CREATE PROCEDURE [query_recording_server_by_hosted_hardware]
	@pkid INT
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		h.[IDRecorder]
	FROM
		[Hardware] h
	WHERE
		h.[HostedHardware_id] = @pkid
END
GO