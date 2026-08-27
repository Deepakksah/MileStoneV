-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_ids_by_hardware]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_ids_by_hardware]
GO

CREATE PROCEDURE [query_device_ids_by_hardware]
	@IDHardware [GuidType]
 AS
BEGIN
	SET NOCOUNT ON

	SELECT [IDDevice]
	FROM [Devices]
	WHERE [IDHardware] = @IDHardware
END
GO
