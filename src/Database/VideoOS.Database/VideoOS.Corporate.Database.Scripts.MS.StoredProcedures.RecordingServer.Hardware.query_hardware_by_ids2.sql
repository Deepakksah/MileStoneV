-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_hardware_by_ids2]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_hardware_by_ids2]
GO

CREATE PROCEDURE [query_hardware_by_ids2]
	@ids tvp_StringTableGuidType READONLY
 AS
BEGIN
	SET NOCOUNT ON

	SELECT
		[LastModified], 
		[IDHardware],
		[Name],
		[Description],
		[IDRecorder],
		[SubType],
		[URI]
	FROM [Hardware] h
	INNER JOIN @ids ids ON h.[IDHardware] = ids.[ID]
END
GO
