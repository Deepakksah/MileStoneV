-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_recording_server_id_by_service_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_recording_server_id_by_service_id]
GO

CREATE PROCEDURE [query_recording_server_id_by_service_id]
	@recording_server_service_id [GuidType]
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT rssr.[recording_server_service_id]
	FROM [RecordingServerServiceRecorder] rssr 
	WHERE rssr.[recorder_id] = @recording_server_service_id
END
GO