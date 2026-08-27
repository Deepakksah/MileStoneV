-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_group_members_by_group_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_group_members_by_group_ids]
GO

CREATE PROCEDURE [query_device_group_members_by_group_ids]
	@DeviceGroupIds tvp_StringTableGuidType READONLY
AS
BEGIN
	SET NOCOUNT ON

		SELECT dgm.IDDevice
		FROM DeviceGroupMembers dgm
		WHERE dgm.IDDeviceGroup IN (SELECT ID FROM @DeviceGroupIds)

END
GO