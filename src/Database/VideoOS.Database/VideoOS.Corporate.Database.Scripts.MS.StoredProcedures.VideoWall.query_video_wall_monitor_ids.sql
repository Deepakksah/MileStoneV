-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_video_wall_monitor_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_video_wall_monitor_ids]
GO

CREATE PROCEDURE [query_video_wall_monitor_ids]
	@videoWallId [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	SELECT m.[pkid]
	FROM [VideoWallMonitor] m
	INNER JOIN [VideoWall] vw ON vw.[pkid] = m.[videowall_id]
	WHERE vw.[pkid] = @videoWallId
END
GO