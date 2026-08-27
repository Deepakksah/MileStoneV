-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_failover_main_recorder]') AND type = 'P')
DROP PROCEDURE [query_failover_main_recorder]
GO

CREATE PROCEDURE [query_failover_main_recorder]
	@IDFailoverServer [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	SELECT	rssr.[recorder_id] AS main_recorder_recorder_id
	FROM	[RecordingServerServiceRecorder] rssr
			INNER JOIN [RecordingServerServiceRecorder] rssr2 ON rssr2.[recorder_id] = rssr.[recorder_id] AND rssr.[type] = 0
			INNER JOIN [FailoverServers] fs ON fs.[IdRecordingServerService] = rssr2.[recording_server_service_id]
	WHERE	fs.[IDFailoverServer] = @IDFailoverServer
END
GO