-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_video_walls_by_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_video_walls_by_ids]
GO

CREATE PROCEDURE [query_video_walls_by_ids]
	@VideoWallIds tvp_StringTableGuidType READONLY
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		[pkid],
		[name],
		[description],
		[width],
		[height],
		[statustext],
		[titlebar],
		[created_date],
		[last_modified]
	FROM [VideoWall] v
	INNER JOIN @VideoWallIds ids
	ON v.[pkid] = ids.[ID]
END
GO