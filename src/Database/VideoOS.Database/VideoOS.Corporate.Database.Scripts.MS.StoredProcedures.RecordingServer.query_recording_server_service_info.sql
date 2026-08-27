-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_recording_server_service_info]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_recording_server_service_info]
GO

CREATE PROCEDURE [query_recording_server_service_info]
    @recording_server_service_id [GuidType]
AS
BEGIN
    SET NOCOUNT ON  
    
    SELECT  rss.[pkid],
            CAST('Recorder' AS NVARCHAR(16)) AS [server_type],
            rssr.[recorder_id] AS [server_id]
    FROM    [RecordingServerService] rss
            INNER JOIN [RecordingServerServiceRecorder] rssr ON rss.[pkid] = rssr.[recording_server_service_id] AND rssr.[type] = 0
    WHERE   rss.[pkid] = @recording_server_service_id
    UNION ALL
    SELECT  rss.[pkid],
            CAST('Failover' AS NVARCHAR(16)) AS [server_type],
            fs.[IDFailoverServer] AS [server_id]
    FROM    [RecordingServerService] rss
            INNER JOIN [FailoverServers] fs ON rss.[pkid] = fs.[IDRecordingServerService]
    WHERE   rss.[pkid] = @recording_server_service_id
END
GO