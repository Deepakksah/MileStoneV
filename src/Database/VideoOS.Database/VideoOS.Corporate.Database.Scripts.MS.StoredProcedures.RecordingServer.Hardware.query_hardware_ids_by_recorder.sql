-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_hardware_ids_by_recorder]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_hardware_ids_by_recorder]
GO

CREATE PROCEDURE [query_hardware_ids_by_recorder]
	@IDRecorder [GuidType]
 AS
BEGIN
	SET NOCOUNT ON

	SELECT [IDHardware]
	FROM [Hardware]
	WHERE [IDRecorder] = @IDRecorder
END
GO
