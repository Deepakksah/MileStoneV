-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_recording_storage_statusdata]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_recording_storage_statusdata]
GO

CREATE PROCEDURE [query_recording_storage_statusdata]
  @IDStorage [GuidType]
 AS
BEGIN
	SET NOCOUNT ON

	SELECT rs.recordingserver_id, rs.pkid, rsd.Data
	FROM [RecordingStorage] rs
	LEFT JOIN [RecorderStatusData] rsd ON rs.recordingserver_id = rsd.IDRecorder
	WHERE rsd.Type = 'StorageUsage' AND rs.pkid = @IDStorage
END
GO
