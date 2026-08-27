-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

SET NOCOUNT ON

DECLARE @status INT
DECLARE @msg NVARCHAR(1024)

-- First, parse all rules to extract all contained guids into the #RuleGuids table
IF OBJECT_ID('TempDb..#RuleGuids', 'U') IS NOT NULL DROP TABLE #RuleGuids;
CREATE TABLE #RuleGuids (
                rule_id CHAR(36) COLLATE SQL_Latin1_General_CP1_CI_AS,
                guid_value CHAR(36) COLLATE SQL_Latin1_General_CP1_CI_AS)
CREATE INDEX IX_Rule_Guid ON #RuleGuids ([guid_value])
DECLARE @guidPattern VARCHAR(512) = '%[0-9a-f][0-9a-f][0-9a-f][0-9a-f][0-9a-f][0-9a-f][0-9a-f][0-9a-f]-[0-9a-f][0-9a-f][0-9a-f][0-9a-f]-[0-9a-f][0-9a-f][0-9a-f][0-9a-f]-[0-9a-f][0-9a-f][0-9a-f][0-9a-f]-[0-9a-f][0-9a-f][0-9a-f][0-9a-f][0-9a-f][0-9a-f][0-9a-f][0-9a-f][0-9a-f][0-9a-f][0-9a-f][0-9a-f]%'
 
;WITH rule_guids_cte(rule_id, guid_val, def2, start_pos, [Definition])
AS (
    SELECT    IDRule
            , SUBSTRING([Def], [StartPos], 36)
            , SUBSTRING([Def], [StartPos] + 36, LEN([Def]) - 36)
            , PATINDEX(@guidPattern, SUBSTRING([Def], [StartPos] + 36, LEN([Def])))
            , [Definition]
    FROM    
            (SELECT
                  IDRule
                , CAST([Definition] AS NVARCHAR(MAX)) AS [Def]
                , PATINDEX(@guidPattern, CAST([Definition] AS NVARCHAR(MAX))) AS [StartPos]
                , [Definition]
             FROM
                Rules) R
    WHERE
            StartPos > 0
    
    UNION ALL
 
    SELECT    rule_id
            , SUBSTRING(def2, start_pos, 36)
            , SUBSTRING(def2, start_pos + 36, LEN(def2) - 36)
            , PATINDEX(@guidPattern, SUBSTRING(def2, start_pos + 36, LEN(def2)))
            , [Definition]
    FROM    
            rule_guids_cte
    WHERE
            start_pos > 0
)
INSERT INTO #RuleGuids
    SELECT DISTINCT rule_id, guid_val
    FROM rule_guids_cte
OPTION (MAXRECURSION 0)

IF OBJECT_ID('TempDb..#AllDuplicateDevices', 'U') IS NOT NULL DROP TABLE #AllDuplicateDevices;
CREATE TABLE #AllDuplicateDevices (
                    [IDHardware] CHAR(36) COLLATE SQL_Latin1_General_CP1_CI_AS,
                    [IDDevice] CHAR(36) COLLATE SQL_Latin1_General_CP1_CI_AS,
                    [DeviceType] VARCHAR(256) COLLATE SQL_Latin1_General_CP1_CI_AS,
                    [DeviceIndex] INT,
                    [Enabled] BIT,
                    [SecurityObjectId] UNIQUEIDENTIFIER
                )
CREATE INDEX IX_AllDuplicateDevices_IDDevice ON #AllDuplicateDevices ([IDDevice])
INSERT INTO #AllDuplicateDevices
                SELECT
                        d1.[IDHardware],
                        d1.[IDDevice],
                        d1.[DeviceType],
                        d1.[DeviceIndex],
                        d1.[Enabled],
                        so.[security_object_id]
                FROM    [Devices] d1
                        LEFT JOIN [RegisteredDeviceTypes] rdt ON rdt.[DeviceType] = d1.[DeviceType]
                        LEFT JOIN [SecurityNamespace] sn ON sn.[name] = rdt.[SecurityNamespaceName]
                        LEFT JOIN [SecurityObject] so ON so.[namespace_id] = sn.[namespace_id] AND so.[object_id] = d1.[IDDevice]
                WHERE   d1.[DeviceType] IN ('Input', 'Output')
                        AND EXISTS (
                            SELECT  1
                            FROM    [Devices] d2
                            WHERE   d2.[IDHardware] = d1.[IDHardware]
                                    AND d2.[IDDevice] <> d1.[IDDevice]
                                    AND d2.[DeviceType] = d1.[DeviceType]
                                    AND d2.[DeviceIndex] = d1.[DeviceIndex]
                            )
            
IF OBJECT_ID('TempDb..#DuplicateIODevices', 'U') IS NOT NULL DROP TABLE #DuplicateIODevices;
CREATE TABLE #DuplicateIODevices (
                        [IDDevice] CHAR(36) COLLATE SQL_Latin1_General_CP1_CI_AS,
                        [security_object_id] UNIQUEIDENTIFIER
                    )
 CREATE INDEX IX_DuplicateIODevices_Guid ON #DuplicateIODevices ([IDDevice])

-- Now, find all the duplicates
;WITH Duplicates_CTE ([DeviceType], [DeviceIndex], [SecurityObjectId], [DeviceImportance], [Enabled], [GroupCount], [RuleCount], [DeviceLinkCount], [IDDevice], [RowNumber])
AS (
    SELECT  [DeviceType],
            [DeviceIndex],
            [SecurityObjectId],
            [DeviceImportance],
            DuplicateDeviceWithImportance.[Enabled],
            [GroupCount],
            [RuleCount],
            [DeviceLinkCount],
            [IDDevice],
            ROW_NUMBER() OVER (PARTITION BY DuplicateDeviceWithImportance.[IDHardware], [DeviceType], [DeviceIndex] 
                               ORDER BY CASE WHEN [SecurityObjectId] IS NULL THEN 1 ELSE 0 END, [DeviceImportance] DESC)
    FROM
        -- The following is a result set containing all duplicate devices
        (SELECT [IDHardware],
                [IDDevice],
                [DeviceType],
                [DeviceIndex],
                [Enabled],
                [SecurityObjectId],
                (SELECT COUNT(1) FROM DeviceGroupMembers dgm INNER JOIN DeviceGroups dg ON dg.IDDeviceGroup = dgm.IDDeviceGroup WHERE dgm.IDDevice = AllDuplicateDevices.IDDevice AND dg.BuiltIn <> 1) AS GroupCount,
                (SELECT COUNT(1) FROM #RuleGuids WHERE [guid_value] = AllDuplicateDevices.IDDevice) AS RuleCount,
                (SELECT COUNT(*) FROM (
                        SELECT 1 AS C FROM DeviceLinks dl1 WHERE dl1.IDDevice = AllDuplicateDevices.IDDevice
                        UNION ALL
                        SELECT 1 AS C FROM DeviceLinks dl2 WHERE dl2.IDDeviceLinked = AllDuplicateDevices.IDDevice) LNK
                ) AS DeviceLinkCount
         FROM 
                #AllDuplicateDevices AllDuplicateDevices
        ) DuplicateDeviceWithImportance
        CROSS APPLY (
                    SELECT [Enabled] + [GroupCount] + [RuleCount] + [DeviceLinkCount] AS [Deviceimportance]
        ) Importance
)
INSERT INTO #DuplicateIODevices
    SELECT Duplicates_CTE.[IDDevice]
           , so.[security_object_id]
    FROM   Duplicates_CTE
           INNER JOIN [RegisteredDeviceTypes] rdt ON rdt.[DeviceType] = Duplicates_CTE.[DeviceType]
           LEFT JOIN [SecurityNamespace] sn ON sn.[name] = rdt.[SecurityNamespaceName]
           LEFT JOIN [SecurityObject] so ON so.[namespace_id] = sn.[namespace_id] AND so.[object_id] = Duplicates_CTE.[IDDevice]
    WHERE  Duplicates_CTE.[RowNumber] > 1 AND
           Duplicates_CTE.[DeviceImportance] = 0
OPTION (MAXRECURSION 0)


-- Now, let's delete...
DECLARE @device_count INT
SELECT @device_count = COUNT(*) FROM #DuplicateIODevices


PRINT N'Deleting from DeviceLinks...'
DELETE dl
FROM [DeviceLinks] dl
     INNER JOIN #DuplicateIODevices dd ON dd.[IDDevice] = dl.[IDDevice] OR dd.[IDDevice] = dl.[IDDeviceLinked]
SET @status = @@ERROR
IF(@status <> 0)
BEGIN
    SET @msg = N'DELETE from [DeviceLinks] failed with status ' + CONVERT(NVARCHAR(6), @status)
	;THROW 50000, @msg, 1;
END


PRINT N'Deleting from RecorderPluginDevice...'
DELETE rpd
FROM [RecorderPluginDevice] rpd
     INNER JOIN #DuplicateIODevices dd ON dd.[IDDevice] = rpd.[IDDevice]
SET @status = @@ERROR
IF(@status <> 0)
BEGIN
    SET @msg = N'DELETE from [RecorderPluginDevice] failed with status ' + CONVERT(NVARCHAR(6), @status)
    ;THROW 50000, @msg, 1;
END


PRINT N'Deleting from DeviceGroupMembers...'
DELETE dgm
FROM [DeviceGroupMembers] dgm
     INNER JOIN #DuplicateIODevices dd ON dd.[IDDevice] = dgm.[IDDevice]
SET @status = @@ERROR
IF(@status <> 0)
BEGIN
    SET @msg = N'DELETE from [DeviceGroupMembers] failed with status ' + CONVERT(NVARCHAR(6), @status)
    ;THROW 50000, @msg, 1;
END


PRINT N'Deleting from SecurityAccessEntry...'
DELETE aces
FROM [SecurityAccessEntry] aces
     INNER JOIN #DuplicateIODevices dd ON dd.[security_object_id] IS NOT NULL AND dd.[security_object_id] = aces.[security_object_id]
SET @status = @@ERROR
IF(@status <> 0)
BEGIN
    SET @msg = N'DELETE from [SecurityAccessEntry] failed with status ' + CONVERT(NVARCHAR(6), @status)
    ;THROW 50000, @msg, 1;
END


PRINT N'Deleting from SecurityObject...'
DELETE so
FROM [SecurityObject] so
     INNER JOIN #DuplicateIODevices dd ON dd.[security_object_id] IS NOT NULL AND dd.[security_object_id] = so.[security_object_id]
SET @status = @@ERROR
IF(@status <> 0)
BEGIN
    SET @msg = N'DELETE from [SecurityObject] failed with status ' + CONVERT(NVARCHAR(6), @status)
    ;THROW 50000, @msg, 1;
END

PRINT N'Deleting SecurityAccessCache2...'
DELETE FROM [SecurityAccessCache2]

DELETE dgm
FROM [DeviceGroupMembers] dgm
     INNER JOIN #DuplicateIODevices dd ON dd.[IDDevice] = dgm.[IDDevice]
SET @status = @@ERROR
IF(@status <> 0)
BEGIN
    SET @msg = N'DELETE from [DeviceGroupMembers] failed with status ' + CONVERT(NVARCHAR(6), @status)
    ;THROW 50000, @msg, 1;
END


PRINT N'Deleting from Devices (' + CONVERT(NVARCHAR(6), @device_count) + ' rows)...'
DELETE d
FROM   [Devices] d
       INNER JOIN #DuplicateIODevices dd ON dd.[IDDevice] = d.[IDDevice]
SET @status = @@ERROR
IF(@status <> 0)
BEGIN
    SET @msg = N'DELETE from Devices failed with status ' + CONVERT(NVARCHAR(6), @status)
    ;THROW 50000, @msg, 1;
END


DROP TABLE #RuleGuids
DROP TABLE #AllDuplicateDevices
DROP TABLE #DuplicateIODevices

GO
