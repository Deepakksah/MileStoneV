-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_tracks_by_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_tracks_by_ids]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_device_tracks_by_device_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_device_tracks_by_device_ids]
GO


CREATE PROCEDURE [query_device_tracks_by_device_ids]
	@ids tvp_StringTableGuidType READONLY
 AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	SELECT
		dt.[IDDevice],
		dt.[IDTrack],
		dt.[DefaultPlayback],
		dt.[MergeEdge],
		dtd.[IsPrimary],
		dtd.[IsEdge]
	FROM [DeviceTrack] dt
	INNER JOIN DeviceTrackDefinition dtd ON dt.IDTrackDefinition = dtd.IDTrackDefinition
	INNER JOIN @ids ids ON dt.[IDDevice] = ids.[ID]
	WHERE dt.InUse = 1
END
GO
