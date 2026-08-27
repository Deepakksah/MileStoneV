-- This header is used by the dbupdate tool]
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on] The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes]
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT 1 FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[device_info_query_by_ids]') AND [type] IN (N'P', N'PC'))
DROP PROCEDURE [device_info_query_by_ids]
GO

CREATE PROCEDURE [device_info_query_by_ids]
	@ids	tvp_StringTableGuidType READONLY
AS
BEGIN
	SET NOCOUNT ON

	SELECT	d.[IDDevice],
			d.[Name],
			d.[DeviceType],
			d.[IDHardware]
	FROM	[Devices] d
			INNER JOIN @ids ids ON d.[IDDevice] = ids.[ID]
END
GO
