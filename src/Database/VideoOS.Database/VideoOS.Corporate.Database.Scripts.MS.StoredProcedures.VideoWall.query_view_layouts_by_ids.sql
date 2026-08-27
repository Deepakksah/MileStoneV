-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_view_layouts_by_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_view_layouts_by_ids]
GO

CREATE PROCEDURE [query_view_layouts_by_ids]
	@viewLayoutIds tvp_StringTableGuidType READONLY
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		vl.[pkid],
		vl.[name],
		vl.[description],
		vl.[created_date],
		vl.[last_modified],
		vl.[definition_xml],
		sgm.[IDServerGroup] as view_layout_group_id
	FROM [VideoWallViewLayout] vl
	INNER JOIN @viewLayoutIds ids ON vl.[pkid] = ids.[ID]
	INNER JOIN [ServerGroupMembership] sgm ON vl.[pkid] = sgm.[IDObject]
END
GO