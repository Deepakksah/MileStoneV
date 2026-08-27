-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_groups_by_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_groups_by_ids]
GO

CREATE PROCEDURE [query_device_groups_by_ids]
	@ids tvp_StringTableGuidType READONLY
AS
BEGIN
	SELECT
		dg.[IDDeviceGroup],
		COALESCE(dg.[IDParentDeviceGroup], '') AS [IDParentDeviceGroup],
		dg.[Name],
		dg.[Description],
		dg.[BuiltIn],
		dg.[DeviceType],
		dg.[LastModified],
		COALESCE( sg.[RelationIds], dg.[IDParentDeviceGroup]) AS [RelationIds]
	FROM [DeviceGroups] dg
	LEFT JOIN [ServerGroup] sg ON sg.IDServerGroup = dg.IDParentDeviceGroup
	INNER JOIN @ids ids ON dg.[IDDeviceGroup] = ids.[ID]
END
GO