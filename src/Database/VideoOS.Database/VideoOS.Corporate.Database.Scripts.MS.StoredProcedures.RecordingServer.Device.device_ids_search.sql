-- This header is used by the dbupdate tool]
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on] The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes]
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[device_ids_search]') AND [type] IN (N'P', N'PC'))
DROP PROCEDURE [device_ids_search]
GO

CREATE PROCEDURE [device_ids_search]
	@DeviceNameContains			NVARCHAR(100),
	@DeviceShortNameContains	NVARCHAR(100),
	@DeviceDescriptionContains	NVARCHAR(100),
	@HardwareAddressContains	NVARCHAR(100),
	@DeviceTypes				tvp_StringTableNvarchar READONLY,
	@IDDeviceEquals				VARCHAR(37), -- 37 to make sure we only match on exact id (without other chars after)
	@IDHardwareEquals			VARCHAR(37), -- 37 to make sure we only match on exact id (without other chars after)
	@IncludeDisabledDevices		BIT
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @AllDeviceTypes BIT
	IF EXISTS (SELECT 1 FROM @DeviceTypes)
	BEGIN
		SET @AllDeviceTypes = 0
	END
	ELSE
	BEGIN
		SET @AllDeviceTypes = 1
	END
	SELECT	d.[IDDevice]
	FROM	[Devices] d
			INNER JOIN [Hardware] h ON h.[IDHardware] = d.[IDHardware]
	WHERE	(	(@DeviceNameContains = '' AND @DeviceShortNameContains = '' AND @DeviceDescriptionContains = '' AND @HardwareAddressContains = '' AND @IDDeviceEquals = '' AND  @IDHardwareEquals = '')
				OR (@DeviceNameContains <> '' AND d.[Name] LIKE N'%' + @DeviceNameContains + N'%')
				OR (@DeviceShortNameContains <> '' AND d.[ShortName] LIKE N'%' + @DeviceShortNameContains + N'%')
				OR (@DeviceDescriptionContains <> '' AND d.[Description] LIKE N'%' + @DeviceDescriptionContains + N'%')
				OR (@HardwareAddressContains <> '' AND h.[URI] LIKE N'%' + @HardwareAddressContains + N'%')
				OR (d.[IDDevice] = @IDDeviceEquals)
				OR (h.[IDHardware] = @IDHardwareEquals)
			)
			AND (@AllDeviceTypes = 1 OR d.[DeviceType] IN (SELECT [ID] FROM @DeviceTypes))
			AND	(@IncludeDisabledDevices = 1 OR (h.[Enabled] = 1 AND d.[Enabled] = 1))
END
GO
