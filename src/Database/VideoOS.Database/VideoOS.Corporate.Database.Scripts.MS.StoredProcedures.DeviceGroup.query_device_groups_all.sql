-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_groups_all]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_groups_all]
GO

CREATE PROCEDURE [query_device_groups_all]
	@DeviceDBType [InternalIdType]
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		[IDDeviceGroup],
		COALESCE([IDParentDeviceGroup], '') AS [IDParentDeviceGroup],
		[Name],
		[Description],
		[BuiltIn],
		[DeviceType],
		[LastModified],
		'' AS [RelationIds]
	FROM [DeviceGroups] WHERE [BuiltIn] = 0 AND [DeviceType] = @DeviceDBType
END
GO