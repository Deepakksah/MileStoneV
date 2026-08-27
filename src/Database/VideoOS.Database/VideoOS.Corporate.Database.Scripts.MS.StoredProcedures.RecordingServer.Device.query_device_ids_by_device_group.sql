-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_ids_by_device_group]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_ids_by_device_group]
GO

CREATE PROCEDURE [query_device_ids_by_device_group]
	@IDDeviceGroup [GuidType],
	@DeviceTypeSearch NVARCHAR(128)
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	IF NOT EXISTS(SELECT * FROM [DeviceGroups] WITH(UPDLOCK) WHERE [IDDeviceGroup] = @IDDeviceGroup)
	BEGIN
        ROLLBACK TRAN
		;THROW 60090, @procedureName, 1;
	END
	
	SELECT
		d.[IDDevice]
	FROM [Devices] d
	INNER JOIN [DeviceGroupMembers] dgm ON dgm.[IDDevice] = d.[IDDevice]
	WHERE dgm.[IDDeviceGroup] = @IDDeviceGroup
	AND 
		(
			(@DeviceTypeSearch = 'All' OR @DeviceTypeSearch = '' OR @DeviceTypeSearch IS NULL)
			OR
			(@DeviceTypeSearch = 'RecordingDevice' AND d.[DeviceType] IN ('Camera', 'AudioInput', 'AudioOutput')) 
			OR
			(@DeviceTypeSearch = 'Camera' AND d.[DeviceType] = 'Camera')
			OR
			(@DeviceTypeSearch = 'Microphone' AND d.[DeviceType] = 'AudioInput')
			OR
			(@DeviceTypeSearch = 'Metadata' AND d.[DeviceType] = 'Metadata')
			OR
			(@DeviceTypeSearch = 'Speaker' AND d.[DeviceType] = 'AudioOutput')
			OR
			(@DeviceTypeSearch = 'Input' AND d.[DeviceType] = 'Input')
			OR
			(@DeviceTypeSearch = 'Output' AND d.[DeviceType] = 'Output')
		)
END
GO
