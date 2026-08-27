-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_recorder_service_versions]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_recorder_service_versions]
GO

CREATE PROCEDURE [query_recorder_service_versions]
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		[pkid] AS recording_server_service_id,
		[version]
	FROM 
		[RecordingServerService]
END
GO