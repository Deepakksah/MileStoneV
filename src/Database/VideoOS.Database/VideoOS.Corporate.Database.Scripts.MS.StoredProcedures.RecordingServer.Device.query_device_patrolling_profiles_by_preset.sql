-- This header is used by the dbupdate tool]
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on] The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes]
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_patrolling_profiles_by_preset]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_patrolling_profiles_by_preset]
GO

CREATE PROCEDURE [query_device_patrolling_profiles_by_preset]
	@device_id [GuidType],
	@preset_id [GuidType]
 AS
BEGIN
	SELECT pp.[IDPatrollingProfile], pp.[Name], pp.[Description], pp.[Definition], pp.[LastModified], pp.[IDDevice], h.[IDHardware], h.[IDRecorder]
	FROM PatrollingProfiles pp
	INNER JOIN Devices d ON pp.IDDevice = d.IDDevice
	INNER JOIN Hardware h ON h.IDHardware = d.IDHardware
	WHERE pp.IDDevice = @device_id 
	AND CAST(Definition as XML).exist('//PresetKey[text()=lower-case(sql:variable("@preset_id"))]') = 1
END
GO
