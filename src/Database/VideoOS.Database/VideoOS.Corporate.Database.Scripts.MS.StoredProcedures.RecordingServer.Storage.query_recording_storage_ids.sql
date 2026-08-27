-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_recording_storage_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_recording_storage_ids]
GO

CREATE PROCEDURE [query_recording_storage_ids]
	@recordingserver_id [GuidType]
 AS
BEGIN
	SET NOCOUNT ON

	SELECT
		[pkid]
	FROM [RecordingStorage]
	WHERE [recordingserver_id] = @recordingserver_id
END
GO
