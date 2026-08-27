-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_hardware_blocks]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_hardware_blocks]
GO

CREATE PROCEDURE [query_hardware_blocks]
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		[pkid],
		[hardware_id],
		[expires],
		[owner],
		[data]
	FROM
		[HardwareBlock]
END
GO
