-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

--Creating Function GetTimeDifference
CREATE FUNCTION GetTimeDifference (@time1 BIGINT, @time2 BIGINT)
RETURNS BIGINT 
AS
BEGIN
  RETURN ABS(@time1 - @time2) / 10000
END
GO

--Removing Duplicate Evidence locks 
DECLARE @duplicates TABLE(Id UNIQUEIDENTIFIER);

WITH
CTE_ORDER_BY_START_TIME
AS
(
  SELECT
    MD.IDMarkedData
    ,MD.[UserName]
    ,MD.[Header]
    ,MD.[MarkDataType]
    ,MD.[StartTime]
    ,MD.[EndTime]
    ,MD.[RetentionExpire]
    ,MDD.[IDDevice]
    ,LAG(MD.StartTime) OVER (PARTITION BY MD.[UserName], MD.[Header], MD.[RetentionExpire], MDD.[IDDevice]
                             ORDER BY     MD.[UserName], MD.[Header], MD.[StartTime], MD.[EndTime], MD.[RetentionExpire]) AS XRow_StartTime
    ,LAG(MD.EndTime)   OVER (PARTITION BY MD.[UserName], MD.[Header], MD.[RetentionExpire], MDD.[IDDevice]
                             ORDER BY     MD.[UserName], MD.[Header], MD.[StartTime], MD.[EndTime], MD.[RetentionExpire]) AS XRow_EndTime
  FROM MarkedData MD
       INNER JOIN [MarkedDataDevices] MDD ON MDD.IDMarkedData = MD.IDMarkedData and MD.MarkDataType = 2
)
,CTE_ORDER_BY_START_TIME_DIFF
AS
(
  SELECT
    IDMarkedData
    ,dbo.GetTimeDifference(XRow_StartTime,StartTime) AS DiffTimeInStartTime
    ,dbo.GetTimeDifference(XRow_EndTime,EndTime)     AS DiffTimeInEndTime
  FROM CTE_ORDER_BY_START_TIME
)
,CTE_ORDER_BY_END_TIME
AS
(
  SELECT
    MD.IDMarkedData
    ,MD.[UserName]
    ,MD.[Header]
    ,MD.[MarkDataType]
    ,MD.[StartTime]
    ,MD.[EndTime]
    ,MD.[RetentionExpire]
    ,MDD.[IDDevice]
    ,LAG(MD.StartTime) OVER (PARTITION BY MD.[UserName], MD.[Header], MD.[RetentionExpire], MDD.[IDDevice]
                             ORDER BY     MD.[UserName], MD.[Header], MD.[EndTime], MD.[StartTime], MD.[RetentionExpire]) AS XRow_StartTime
    ,LAG(MD.EndTime)   OVER (PARTITION BY MD.[UserName], MD.[Header], MD.[RetentionExpire], MDD.[IDDevice]
                             ORDER BY     MD.[UserName], MD.[Header], MD.[EndTime], MD.[StartTime], MD.[RetentionExpire]) AS XRow_EndTime
  FROM MarkedData MD
       INNER JOIN [MarkedDataDevices] MDD ON MDD.IDMarkedData = MD.IDMarkedData and MD.MarkDataType = 2
)
,CTE_ORDER_BY_END_TIME_DIFF
AS
(
  SELECT
    IDMarkedData
    ,dbo.GetTimeDifference(XRow_StartTime,StartTime) AS DiffTimeInStartTime
    ,dbo.GetTimeDifference(XRow_EndTime,EndTime)     AS DiffTimeInEndTime
  FROM CTE_ORDER_BY_END_TIME
)

--Inserting into @duplicates
INSERT INTO @duplicates
  SELECT  IDMarkedData
  FROM  CTE_ORDER_BY_START_TIME_DIFF 
  WHERE  (DiffTimeInStartTime BETWEEN -1 AND 1) AND (DiffTimeInEndTime BETWEEN -1 AND 1)
UNION
  SELECT  IDMarkedData
  FROM  CTE_ORDER_BY_END_TIME_DIFF 
  WHERE  (DiffTimeInStartTime BETWEEN -1 AND 1) AND (DiffTimeInEndTime BETWEEN -1 AND 1)

--Deleting from @duplicates
DELETE FROM  MarkedDataDevices
WHERE    IDMarkedData IN  (SELECT Id AS IDMarkedData FROM @duplicates);

DELETE FROM MarkedData
WHERE    IDMarkedData IN (SELECT Id AS IDMarkedData FROM @duplicates);
GO

IF OBJECT_ID(N'GetTimeDifference', N'FN') IS NOT NULL
  DROP FUNCTION GetTimeDifference
GO