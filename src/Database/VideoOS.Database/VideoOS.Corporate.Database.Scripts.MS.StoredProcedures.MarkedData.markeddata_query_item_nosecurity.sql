-- This header is used by the dbupdate tool]
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on] The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes]
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[markeddata_query_item_nosecurity]') AND type in (N'P', N'PC'))
DROP PROCEDURE [markeddata_query_item_nosecurity]
GO

CREATE PROCEDURE [markeddata_query_item_nosecurity]
	@IDMarkedData [UNIQUEIDENTIFIER]
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT [MarkedData].[IDMarkedData]
		, (SELECT IDDevice, Size FROM [MarkedDataDevices] WHERE [IDMarkedData] = [MarkedData].[IDMarkedData]
			   FOR XML PATH('Device'), ROOT('MarkedDataDevices'), Elements) AS IDDevices
			, [MarkedData].[LastModified]
			, [MarkedData].[CreatedDate]
			, [MarkedData].[Reference]
			, [MarkedData].[UserName]
			, [MarkedData].[Header]
			, [MarkedData].[Description]
			, [MarkedData].[MarkDataType]
			, [MarkedData].[StartTime]
			, [MarkedData].[TagTime]
			, [MarkedData].[EndTime]
			, [MarkedData].[RetentionType]
			, [MarkedData].[RetentionUnits]
			, [MarkedData].[RetentionExpire]
			, [MarkedData].[ProtectionSize]
		FROM [MarkedData]
		WHERE
		[MarkedData].[IDMarkedData] = @IDMarkedData
END
GO