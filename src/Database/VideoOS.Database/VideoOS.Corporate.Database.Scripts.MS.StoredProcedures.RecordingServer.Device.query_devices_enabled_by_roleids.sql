-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================


IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_devices_enabled_by_roleids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_devices_enabled_by_roleids]
GO


CREATE PROCEDURE [query_devices_enabled_by_roleids]
    @RoleIds tvp_StringTableGuidType READONLY
 AS
BEGIN
    SET NOCOUNT ON
    
    SELECT
        d.[IDDevice], d.[IDHardware], d.[DeviceType], d.[Name], d.[Description], d.[Streams], d.[Shortcut], 
        d.[Settings], d.[Presets], d.[MulticastEnabled], d.[DeviceIndex], d.[GisPoint].AsTextZM() AS [GisPointString], 
        d.[ShortName], d.[Icon], d.[CoverageDirection], d.[CoverageDepth], d.[CoverageFieldOfView], rsd.[edgestorage_playback], 
        rsd.[manual_timeout_enabled], rsd.[manual_timeout], h.[IDRecorder]
    FROM 
        [Devices] AS d 
        INNER JOIN [SecurityAvailableDevicesTable](@RoleIds, 'GENERIC_READ', 1, null) ad ON d.IDDevice = ad.IDDevice
        INNER JOIN [Hardware] h ON h.[IDHardware] = d.[IDHardware]
        LEFT JOIN [RecordingStorageDevice] AS rsd ON d.[IDDevice] = rsd.[device_id]
        
END
GO
