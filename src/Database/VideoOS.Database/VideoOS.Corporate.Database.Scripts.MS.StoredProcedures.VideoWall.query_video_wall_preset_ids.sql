-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_video_wall_preset_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_video_wall_preset_ids]
GO

CREATE PROCEDURE [query_video_wall_preset_ids]
	@videoWallId [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	SELECT [pkid]
	FROM  [VideoWallPreset]
	WHERE [videowall_id] = @videoWallId 
END
GO