-- This header is used by the dbupdate tool]
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on] The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes]
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_all_hardware_settings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_all_hardware_settings]
GO

CREATE PROCEDURE [query_all_hardware_settings]
	@hardware_id [GuidType]
 AS
BEGIN
	SET NOCOUNT ON

	SELECT [Settings]
	FROM [Hardware]
	WHERE [IDHardware] = @hardware_id
	UNION ALL
	SELECT [Settings]
	FROM [Devices]
	WHERE [IDHardware] = @hardware_id
END
GO
