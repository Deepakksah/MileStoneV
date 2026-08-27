-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[reporting_web_recording_server_names]') AND type in (N'P', N'PC'))
DROP PROCEDURE [reporting_web_recording_server_names]
GO

CREATE PROCEDURE [reporting_web_recording_server_names]
	@recorder_ids tvp_StringTableGuidType READONLY
AS
BEGIN
	SELECT
		r.[IDRecorder],
		r.[Name]
	FROM
		[Recorders] r 
		INNER JOIN @recorder_ids ids ON r.[IDRecorder] = ids.[ID]
END
GO