-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_group_by_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_group_by_id]
GO

CREATE PROCEDURE [query_device_group_by_id]
	@IDDeviceGroup [GuidType]
AS
BEGIN
	SELECT
		[IDDeviceGroup],
		COALESCE([IDParentDeviceGroup], '') AS [IDParentDeviceGroup],
		[Name],
		[Description],
		[BuiltIn],
		[DeviceType],
		[LastModified],
		dbo.GetDeviceGroupRelations([IDParentDeviceGroup]) AS RelationIds
	FROM [DeviceGroups]
	WHERE [IDDeviceGroup] = @IDDeviceGroup
END
GO