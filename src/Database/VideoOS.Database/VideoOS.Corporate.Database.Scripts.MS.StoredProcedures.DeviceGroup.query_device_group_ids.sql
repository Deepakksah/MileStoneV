-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_group_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_group_ids]
GO

CREATE PROCEDURE [query_device_group_ids]
	@IDParentDeviceGroup [GuidType],
	@DeviceType NVARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON

	IF(@IDParentDeviceGroup = '' OR @IDParentDeviceGroup = '909749AE-88C4-4780-A381-383D691E98FA')
	BEGIN
		SELECT dg.[IDDeviceGroup]
		FROM [DeviceGroups] dg
		WHERE [IDParentDeviceGroup] IS NULL 
			AND [BuiltIn] = 0
			AND 
			(
				(@DeviceType = 'Camera' AND dg.[DeviceType] = 'Camera')
				OR
				(@DeviceType = 'Microphone' AND dg.[DeviceType] = 'AudioInput')
				OR
				(@DeviceType = 'Metadata' AND dg.[DeviceType] = 'Metadata')
				OR
				(@DeviceType = 'Speaker' AND dg.[DeviceType] = 'AudioOutput')
				OR
				(@DeviceType = 'Input' AND dg.[DeviceType] = 'Input')
				OR
				(@DeviceType = 'Output' AND dg.[DeviceType] = 'Output')
			)
	END
	ELSE
	BEGIN
		SELECT [IDDeviceGroup]
		FROM [DeviceGroups] dg
		WHERE [IDParentDeviceGroup] = @IDParentDeviceGroup
			AND [BuiltIn] = 0
			AND 
			(
				(@DeviceType = 'Camera' AND dg.[DeviceType] = 'Camera')
				OR
				(@DeviceType = 'Microphone' AND dg.[DeviceType] = 'AudioInput')
				OR
				(@DeviceType = 'Metadata' AND dg.[DeviceType] = 'Metadata')
				OR
				(@DeviceType = 'Speaker' AND dg.[DeviceType] = 'AudioOutput')
				OR
				(@DeviceType = 'Input' AND dg.[DeviceType] = 'Input')
				OR
				(@DeviceType = 'Output' AND dg.[DeviceType] = 'Output')
			)
	END
END
GO