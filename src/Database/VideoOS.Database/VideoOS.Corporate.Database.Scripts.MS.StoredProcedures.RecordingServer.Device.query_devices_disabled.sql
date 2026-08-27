-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_devices_disabled]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_devices_disabled]
GO

CREATE PROCEDURE [query_devices_disabled]
 AS
BEGIN
	SET NOCOUNT ON

	SELECT 
		r.[IDRecorder], h.[IDHardware], d.[IDDevice], d.[DeviceType], d.[Name]
	FROM
		[Devices] AS d 
			INNER JOIN [Hardware] AS h ON d.[IDHardware] = h.[IDHardware]
			INNER JOIN [Recorders] AS r ON h.[IDRecorder] = r.[IDRecorder]
	WHERE
		(h.[Enabled] = 0) OR (d.[Enabled] = 0)
END
GO
