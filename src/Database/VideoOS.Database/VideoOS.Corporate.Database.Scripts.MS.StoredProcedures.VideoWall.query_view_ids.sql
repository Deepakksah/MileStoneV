-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_view_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_view_ids]
GO

CREATE PROCEDURE [query_view_ids]
	@parentId [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	SELECT v.[pkid]
	FROM [VideoWallView] v
	INNER JOIN [ServerGroupMembership] sgm ON v.[pkid] = sgm.[IDObject]
	WHERE sgm.[IDServerGroup] = @parentId
END
GO