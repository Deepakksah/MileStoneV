-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_group_members]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_group_members]
GO

CREATE PROCEDURE [query_device_group_members]
	@DeviceDBType NVARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON

		SELECT dgm.IDDevice, dgm.IDDeviceGroup
		FROM DeviceGroupMembers dgm
		INNER JOIN DeviceGroups dg ON dg.IDDeviceGroup = dgm.IDDeviceGroup
		WHERE dg.DeviceType = @DeviceDbType AND dg.BuiltIn = 0

END
GO