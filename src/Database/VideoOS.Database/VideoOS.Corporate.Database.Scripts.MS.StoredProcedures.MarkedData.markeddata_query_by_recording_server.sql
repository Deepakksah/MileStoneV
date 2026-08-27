-- This header is used by the dbupdate tool]
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on] The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes]
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[markeddata_query_by_recording_server]') AND type in (N'P', N'PC'))
DROP PROCEDURE [markeddata_query_by_recording_server]
GO

CREATE PROCEDURE [dbo].[markeddata_query_by_recording_server]
	@recording_server_id GuidType
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT
			md.[IDMarkedData]
		, (
				SELECT	[IDDevice], [Size]
				FROM	[MarkedDataDevices]
				WHERE	[IDMarkedData] = md.[IDMarkedData] 
				FOR XML PATH('Device'), ROOT('MarkedDataDevices'), Elements
			) AS [IDDevices]
		, md.[LastModified]
		, md.[CreatedDate]
		, md.[Reference]
		, md.[UserName]
		, md.[Header]
		, md.[Description]
		, md.[MarkDataType]
		, md.[StartTime]
		, md.[TagTime]
		, md.[EndTime]
		, md.[RetentionType]
		, md.[RetentionUnits]
		, md.[RetentionExpire]
		, md.[ProtectionSize]
	FROM	[MarkedData] md
	WHERE	EXISTS (
				SELECT	1
				FROM	[MarkedDataDevices] mdd
						INNER JOIN [Devices] d ON d.[IDDevice] = mdd.[IDDevice]
						INNER JOIN [Hardware] h ON d.[IDHardware] = h.[IDHardware]
				WHERE	mdd.[IDMarkedData] = md.[IDMarkedData]
						AND h.[IDRecorder] = @recording_server_id
			)
			AND md.[MarkDataType] = 2 -- Only evidence lock
END
GO