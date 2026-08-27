-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_hardware_info]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_hardware_info]
GO


CREATE PROCEDURE [query_hardware_info]
	@recorder_id [GuidType] = NULL,
	@hardware_id [GuidType] = NULL
AS
BEGIN
	SET NOCOUNT ON

	SELECT	h.[IDRecorder] AS [recorder_id],
			h.[IDHardware] AS [hardware_id],
			h.[Name] AS [name],
			UPPER(h.MacAddress) AS [mac_address],
			h.[LoginID] AS [username]
	FROM	[Hardware] h
	WHERE	(@recorder_id IS NULL OR h.[IDRecorder] = @recorder_id) AND
			(@hardware_id IS NULL OR h.[IDHardware] = @hardware_id)
END
GO
