-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.views WHERE [object_id] = OBJECT_ID(N'[dbo].[DeviceRestrictedMedia]'))
DROP VIEW [dbo].[DeviceRestrictedMedia]
GO

CREATE VIEW [dbo].[DeviceRestrictedMedia] 
AS
	SELECT	rm.[IDRestrictedMedia]
			, rmd.[IDDevice]
			, h.[IDRecorder]
			, rm.[StartTime]
			, rm.[EndTime]
	FROM	[RestrictedMedia] rm
			INNER JOIN [RestrictedMediaDevice] rmd ON rmd.[IDRestrictedMedia] = rm.[IDRestrictedMedia]
			INNER JOIN [Devices] d ON d.[IDDevice] = rmd.[IDDevice]
			INNER JOIN [Hardware] h ON h.[IDHardware] = d.[IDHardware]
	UNION ALL
	SELECT	rml.[IDDevice]
			, rml.[IDDevice]
			, h.[IDRecorder]
			, rml.[StartTime]
			, NULL
	FROM	[RestrictedMediaLive] rml
			INNER JOIN [Devices] d ON d.[IDDevice] = rml.[IDDevice]
			INNER JOIN [Hardware] h ON h.[IDHardware] = d.[IDHardware]

GO