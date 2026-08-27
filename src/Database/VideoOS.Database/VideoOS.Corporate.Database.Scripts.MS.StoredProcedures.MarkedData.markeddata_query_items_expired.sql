-- This header is used by the dbupdate tool]
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on] The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes]
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[markeddata_query_items_expired]') AND type in (N'P', N'PC'))
DROP PROCEDURE [markeddata_query_items_expired]
GO

CREATE PROCEDURE [markeddata_query_items_expired]
	@RetentionExpireFromTime as BigInt,
	@RetentionExpireToTime as BigInt
AS
BEGIN
	SELECT
		  [IDMarkedData]
		, (
			SELECT IDDevice, Size
			FROM [MarkedDataDevices] mdd 
			WHERE [IDMarkedData] = [MarkedData].[IDMarkedData] 
			FOR XML PATH('Device'), ROOT('MarkedDataDevices'), Elements
		) AS IDDevices
		, [LastModified]
		, [CreatedDate]
		, [Reference]
		, [UserName]
		, [Header]
		, [Description]
		, [MarkDataType]
		, [StartTime]
		, [TagTime]
		, [EndTime]
		, [RetentionType]
		, [RetentionUnits]
		, [RetentionExpire]
		, [ProtectionSize]
  FROM [MarkedData]
  WHERE 
	RetentionType <> 0 
	AND @RetentionExpireFromTime <= RetentionExpire 
	AND @RetentionExpireToTime >= RetentionExpire
END
GO