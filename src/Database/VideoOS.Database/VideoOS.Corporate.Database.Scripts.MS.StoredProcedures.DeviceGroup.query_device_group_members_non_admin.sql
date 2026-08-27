-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_group_members_non_admin]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_group_members_non_admin]
GO

CREATE PROCEDURE [query_device_group_members_non_admin]
	@DeviceType NVARCHAR(100),
	@MemberSid NVARCHAR(100)
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

	SELECT dgm.IDDeviceGroup, spd.IDDevice
		FROM SecurityPermissionsDevice spd
		INNER JOIN RoleMembership rm ON spd.IDRole = rm.IDRole
		INNER JOIN DeviceGroupMembers dgm ON spd.IDDevice = dgm.IDDevice
		WHERE SecurityAttributeID = @secAttrID AND MemberSID = @MemberSid 
		ORDER BY spd.IDDevice
 
END
GO