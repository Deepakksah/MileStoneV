-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_hardware_ids_changed_after]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_hardware_ids_changed_after]
GO

CREATE PROCEDURE [query_hardware_ids_changed_after]
@changedAfter DATETIME
 AS
BEGIN
	SET NOCOUNT ON

	SELECT
		h.[IDHardware],
		h.[LastModified]
	FROM 
		[Hardware] h
	WHERE
		h.[LastModified] > @changedAfter
END
GO
