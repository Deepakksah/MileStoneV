-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_group_ids_non_admin]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_group_ids_non_admin]
GO

CREATE PROCEDURE [query_device_group_ids_non_admin]
	@MemberSid NVARCHAR(100),
	@DeviceType [InternalIdType]
AS
BEGIN
	SET NOCOUNT ON
 
 	DECLARE @secAttrID [InternalIdType]
	IF @DeviceType = 'Camera'
		SET @secAttrID = 'DEVICEVISIBLE'
	ELSE
	IF @DeviceType = 'AudioInput'
		SET @secAttrID = 'DEVICEMICVISIBLE'
	ELSE
	IF @DeviceType = 'AudioOutput'
		SET @secAttrID = 'DEVICESPEAKERVISIBLE'
	ELSE
	IF @DeviceType = 'Metadata'
		SET @secAttrID = 'DEVICEVISIBLE'
	ELSE
	IF @DeviceType = 'Input'
		SET @secAttrID = 'DEVICEVISIBLE'
	ELSE
	IF @DeviceType = 'Output'
		SET @secAttrID = 'DEVICEVISIBLE'

	;WITH CTEDeviceGroupHierarchy AS
    (
		  select distinct dg.IDParentDeviceGroup, dgm.IDDeviceGroup, 1 AS Level
		  from SecurityPermissionsDevice spd
		  INNER JOIN RoleMembership rm ON spd.IDRole = rm.IDRole
		  INNER JOIN DeviceGroupMembers dgm ON spd.IDDevice = dgm.IDDevice
		  INNER JOIN DeviceGroups dg ON dgm.IDDeviceGroup = dg.IDDeviceGroup
		  WHERE SecurityAttributeID = @secAttrID AND MemberSID = @MemberSid AND dg.BuiltIn = 0 AND dg.DeviceType = @DeviceType
		UNION ALL
		SELECT dg.IDParentDeviceGroup, dg.IDDeviceGroup, cte.[Level] + 1 As Level
		FROM [DeviceGroups] dg
		INNER JOIN CTEDeviceGroupHierarchy cte ON cte.IDParentDeviceGroup = dg.[IDDeviceGroup]
		WHERE cte.[Level] + 1 <= 12
	)
	SELECT distinct IDParentDeviceGroup, IDDeviceGroup
	FROM CTEDeviceGroupHierarchy

END
GO