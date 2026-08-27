-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

/*
   Migration script for moving all the cameras/viewzones from the event server gismap items table to the devices table.
   If the camera exists multiple times in the event server table only one instance will be moved.
   Only cameras that exists in the devices table allready will be moved, but will always overwrite all gismap specific data.
*/

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[GisMap].[Items]') AND type = N'U') OR
   NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[GisMap].[ItemProperties]') AND type = N'U') OR
   NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[GisMap].[Layers]') AND type = N'U') OR
   NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[GisMap].[LayerProperties]') AND type = N'U') OR
   -- Because [Central].[Version] 3.4.3 has GisMap but no PropertyType column on the LayerProperties, we need this extra check.
   NOT EXISTS (SELECT * FROM information_schema.columns WHERE TABLE_SCHEMA = N'GisMap' AND TABLE_NAME = N'ItemProperties' AND COLUMN_NAME = N'PropertyType')
BEGIN
  -- It seems like the GisMap tables does not exists or is incomplete, so we do not proceed with the migration.
  RETURN
END

DECLARE @icons TABLE ([IconId] [uniqueidentifier], [Icon] INT)

DECLARE @cameras TABLE
(
  [Id] [uniqueidentifier],
  [Name] NVARCHAR(4000),
  [DeviceId] [uniqueidentifier],
  [IconId] [uniqueidentifier],
  [Position_X] FLOAT,
  [Position_Y] FLOAT,
  [Position_Z] FLOAT,
  [FieldOfView] FLOAT,
  [ViewDirection] FLOAT,
  [ViewDepth] FLOAT,
  [GisPoint] GEOGRAPHY,
  [ShortName] NVARCHAR(128) DEFAULT '',
  [Icon] INT DEFAULT 0,
  [CoverageDirection] FLOAT DEFAULT 0.25,
  [CoverageDepth] FLOAT DEFAULT 20.0,
  [CoverageFieldOfView] FLOAT DEFAULT 0.2
)

DECLARE @pointEmpty GEOGRAPHY
SET @pointEmpty = GEOGRAPHY::STPointFromText('POINT EMPTY', 4326)

DECLARE @mapId [uniqueidentifier]
SET @mapId = 'CEA7BCDA-59BA-4EEB-AE20-31198F3BCC54'

DECLARE @cameraLayerId [uniqueidentifier]
SET @cameraLayerId = 'CEA7BCDA-49BA-4EEB-AE20-31198F3BCC54'

DECLARE @viewZoneLayerId [uniqueidentifier]
SET @viewZoneLayerId = 'CEA7BCDA-79BA-4EEB-AE20-31198F3BCC54'

DECLARE @guidEmpty [uniqueidentifier]
SET @guidEmpty = '00000000-0000-0000-0000-000000000000'

-- Fill icon dictionary used when converting from IconId (Guid) to Icon (Int)
INSERT INTO @icons ([IconId], [Icon]) VALUES ('D9B92F60-4662-4429-B933-ACC72216F9F0', 0)
INSERT INTO @icons ([IconId], [Icon]) VALUES ('D9B92F60-4662-4429-B933-ACC72216F9F1', 1)
INSERT INTO @icons ([IconId], [Icon]) VALUES ('D9B92F60-4662-4429-B933-ACC72216F9F2', 2)
INSERT INTO @icons ([IconId], [Icon]) VALUES ('D9B92F60-4662-4429-B933-ACC72216F9F3', 3)
INSERT INTO @icons ([IconId], [Icon]) VALUES ('D9B92F60-4662-4429-B933-ACC72216F9F4', 4)
INSERT INTO @icons ([IconId], [Icon]) VALUES ('D9B92F60-4662-4429-B933-ACC72216F9F5', 5)
INSERT INTO @icons ([IconId], [Icon]) VALUES ('D9B92F60-4662-4429-B933-ACC72216F9F6', 6)
INSERT INTO @icons ([IconId], [Icon]) VALUES ('D9B92F60-4662-4429-B933-ACC72216F9F7', 7)
INSERT INTO @icons ([IconId], [Icon]) VALUES ('D9B92F60-4662-4429-B933-ACC72216F9F8', 8)
INSERT INTO @icons ([IconId], [Icon]) VALUES ('D9B92F60-4662-4429-B933-ACC72216F9F9', 9)

-- Insert all cameras into temp. table @cameras. (But only unique cameras)
INSERT INTO @cameras ([Id], [Name], [DeviceId], [IconId], [Position_X], [Position_Y], [Position_Z])
SELECT a.[Id], a.[Name], a.[DeviceId], a.[IconId], a.[Position_X], a.[Position_Y], a.[Position_Z] FROM
(
  SELECT [Id], [Name], CAST([DeviceId] AS [uniqueidentifier]) AS DeviceId, CAST([IconId] AS [uniqueidentifier]) AS IconId, CAST([Position_X] AS FLOAT) AS Position_X, CAST([Position_Y] AS FLOAT) AS Position_Y, CAST([Position_Z] AS FLOAT) AS Position_Z FROM
  (
    SELECT [Id], [Name], [Field], [Value]
    FROM [GisMap].[Items] i
    LEFT JOIN [GisMap].[ItemProperties] ip ON i.[Id] = ip.[ItemId]
    WHERE i.[TypeName] = 'CameraItem' AND [LayerId] = @cameraLayerId
  ) source
  PIVOT
  (
    MAX([Value])
    FOR [Field] IN ([DeviceId], [IconId], [Position_X], [Position_Y], [Position_Z])
  ) AS properties
) AS a
INNER JOIN
(
  SELECT MIN(CAST([Id] as varchar(36))) AS Id, CAST([DeviceId] AS [uniqueidentifier]) AS DeviceId FROM
  (
    SELECT [Id], [Field], [Value]
    FROM [GisMap].[Items] i
    LEFT JOIN [GisMap].[ItemProperties] ip ON i.[Id] = ip.[ItemId]
    WHERE i.[TypeName] = 'CameraItem' AND [LayerId] = @cameraLayerId
  ) source
  PIVOT
  (
    MAX([Value])
    FOR [Field] IN (DeviceId)
  ) AS properties
  GROUP BY DeviceId
) AS b
ON a.Id = b.Id

-- Update view zone info for each camera in temp. table @cameras.
UPDATE @cameras
SET FieldOfView = viewZone.[FieldOfView],
ViewDirection = viewZone.[ViewDirection],
ViewDepth = viewZone.[ViewDepth]
FROM
(
  SELECT a.[Id], a.[DependsOnId], a.[FieldOfView], a.[ViewDirection], a.[ViewDepth] FROM
  (
    SELECT [Id], CAST([DependsOnId] AS [uniqueidentifier]) AS DependsOnId, CAST([FieldOfView] AS FLOAT) AS FieldOfView, CAST([ViewDirection] AS FLOAT) AS ViewDirection, CAST([ViewDepth] AS FLOAT) AS ViewDepth FROM
    (
      SELECT [Id], [Name], [Field], [Value]
      FROM [GisMap].[Items] i
      LEFT JOIN [GisMap].[ItemProperties] ip ON i.[Id] = ip.[ItemId]
      WHERE i.[TypeName] = 'ViewZoneItem' AND [LayerId] = @viewZoneLayerId
    ) source
    PIVOT
    (
      MAX([Value])
      FOR [Field] IN ([DependsOnId], [FieldOfView], [ViewDirection], [ViewDepth])
    ) AS properties
  ) AS a
  WHERE a.[DependsOnId] IN (SELECT [Id] FROM @cameras)
) AS viewZone
WHERE viewZone.[DependsOnId] = [@cameras].[Id]

-- Make sure to set a POINT EMPTY in all GisPoint column rows. (Simply adding a default GisPoint value)
UPDATE @cameras
SET [GisPoint] = @pointEmpty

-- Update the short name, by limiting the name to 128 chars or empty if the device name is equal to new short name.
UPDATE @cameras
SET [ShortName] = SUBSTRING(name.[ShortName], 0, 129)
FROM
(
  SELECT [Id],
    CASE
      WHEN c.[Name] != d.[Name] THEN c.[Name]
      ELSE ''
    END AS ShortName
  FROM @cameras AS c
  LEFT JOIN [dbo].[Devices] AS d
  ON c.DeviceId = d.IDDevice
) AS name
WHERE name.[Id] = [@cameras].[Id]

-- Update with correct icon int instead of uniqueidentifier.
UPDATE @cameras
SET [Icon] = icon.[Icon]
FROM
(
  SELECT [IconId], [Icon]
  FROM @icons
) AS icon
WHERE icon.[IconId] = [@cameras].[IconId]

-- Update correct gis point instead of position x,y,z.
UPDATE @cameras
SET [GisPoint] = GEOGRAPHY::STPointFromText(position.[GisPointString], 4326)
FROM
(
  SELECT [Id], 'POINT (' + LTRIM(STR([Position_X], 255, 100)) + ' ' + LTRIM(STR([Position_Y], 255, 100)) + ' ' + LTRIM(STR([Position_Z], 255, 100)) + ')' AS GisPointString
  FROM @cameras
  WHERE [Position_X] IS NOT NULL AND [Position_Y] IS NOT NULL AND [Position_Z] IS NOT NULL
) AS position
WHERE position.[Id] = [@cameras].[Id]

-- Update normalized coverage direction.
-- Old direction is 0.0 east CCW range: -2*pi to +2*pi
-- New normalized direction is 0.0 north CW range: -1.0 to +1.0
UPDATE @cameras
SET [CoverageDirection] = direction.[Direction] / (PI() * 2) -- Normalize direction.
FROM
(
  SELECT [Id], (PI() * 2.0) - [ViewDirection] - (PI() / 2.0 * 3.0) AS Direction -- Convert from CCW to CW and from where 0.0 is east to where 0.0 is north.
  FROM @cameras
  WHERE [ViewDirection] IS NOT NULL
) AS direction
WHERE direction.[Id] = [@cameras].[Id]

-- Update coverage depth.
UPDATE @cameras
SET [CoverageDepth] =
  CASE
    WHEN depth.[ViewDepth] > 1000000000.0 THEN 1000000000.0
	WHEN depth.[ViewDepth] < 0.0 THEN 0.0
	ELSE depth.[ViewDepth]
  END
FROM
(
  SELECT [Id], [ViewDepth]
  FROM @cameras
  WHERE [ViewDepth] IS NOT NULL
) AS depth
WHERE depth.[Id] = [@cameras].[Id]

-- Update normalized coverage field of view.
-- Old field of view 0.0 to 2*.pi
-- New normalized field of view 0.0 to 1.0
UPDATE @cameras
SET [CoverageFieldOfView] = fieldofview.[FieldOfView] / (PI() * 2)
FROM
(
  SELECT [Id], [FieldOfView]
  FROM @cameras
  WHERE [FieldOfView] IS NOT NULL
) AS fieldofview
WHERE fieldofview.[Id] = [@cameras].[Id]

-- Update device table with new data.
UPDATE [dbo].[Devices]
SET [GisPoint] = camera.[GisPoint], [ShortName] = camera.[ShortName], [Icon] = camera.[Icon], [CoverageDirection] = camera.[CoverageDirection], [CoverageDepth] = camera.[CoverageDepth], [CoverageFieldOfView] = camera.[CoverageFieldOfView]
FROM
(
  SELECT [DeviceId], [GisPoint], [ShortName], [Icon], [CoverageDirection], [CoverageDepth], [CoverageFieldOfView]
  FROM @cameras
  WHERE [GisPoint].STEquals(@pointEmpty) != 1 -- Only update the device if the GisPoint is actually set.
) AS camera
WHERE camera.DeviceId = [dbo].[Devices].IDDevice AND [dbo].[Devices].DeviceType = 'Camera'

-- Insert empty camera layer if it does not exists.
IF NOT EXISTS (SELECT [Id] FROM [GisMap].[Layers] WHERE [Id] = @cameraLayerId AND [MapId] = @mapId)
BEGIN
  INSERT INTO [GisMap].[Layers] ([Id], [MapId], [Name], [IsVisible], [TypeName]) VALUES (@cameraLayerId, @mapId, 'Cameras', 1, 'CameraLayer')
  INSERT INTO [GisMap].[LayerProperties] ([LayerId], [Field], [Value]) VALUES (@cameraLayerId, 'RepositoryId', @guidEmpty)
  INSERT INTO [GisMap].[LayerProperties] ([LayerId], [Field], [Value]) VALUES (@cameraLayerId, 'Extent_UpperLeft_X', CONVERT(FLOAT, 0.0))
  INSERT INTO [GisMap].[LayerProperties] ([LayerId], [Field], [Value]) VALUES (@cameraLayerId, 'Extent_UpperLeft_Y', CONVERT(FLOAT, 0.0))
  INSERT INTO [GisMap].[LayerProperties] ([LayerId], [Field], [Value]) VALUES (@cameraLayerId, 'Extent_UpperLeft_Z', CONVERT(FLOAT, 0.0))
  INSERT INTO [GisMap].[LayerProperties] ([LayerId], [Field], [Value]) VALUES (@cameraLayerId, 'Extent_LowerRight_X', CONVERT(FLOAT, 0.0))
  INSERT INTO [GisMap].[LayerProperties] ([LayerId], [Field], [Value]) VALUES (@cameraLayerId, 'Extent_LowerRight_Y', CONVERT(FLOAT, 0.0))
  INSERT INTO [GisMap].[LayerProperties] ([LayerId], [Field], [Value]) VALUES (@cameraLayerId, 'Extent_LowerRight_Z', CONVERT(FLOAT, 0.0))
  INSERT INTO [GisMap].[LayerProperties] ([LayerId], [Field], [Value]) VALUES (@cameraLayerId, 'Modifiers', CONVERT(INT, 0))
  INSERT INTO [GisMap].[LayerProperties] ([LayerId], [Field], [Value]) VALUES (@cameraLayerId, 'IsCameraBackgroundDiskVisible', CONVERT(BIT, 1))
  INSERT INTO [GisMap].[LayerProperties] ([LayerId], [Field], [Value]) VALUES (@cameraLayerId, 'IsCameraTitleVisible', CONVERT(BIT, 1))
END

-- Insert empty view zone layer if it does not exists.
IF NOT EXISTS (SELECT [Id] FROM [GisMap].[Layers] WHERE [Id] = @viewZoneLayerId AND [MapId] = @mapId)
BEGIN
  INSERT INTO [GisMap].[Layers] ([Id], [MapId], [Name], [IsVisible], [TypeName]) VALUES (@viewZoneLayerId, @mapId, 'View zones', 1, 'ViewZoneLayer')
  INSERT INTO [GisMap].[LayerProperties] ([LayerId], [Field], [Value]) VALUES (@viewZoneLayerId, 'RepositoryId', @guidEmpty)
  INSERT INTO [GisMap].[LayerProperties] ([LayerId], [Field], [Value]) VALUES (@viewZoneLayerId, 'Extent_UpperLeft_X', CONVERT(FLOAT, 0.0))
  INSERT INTO [GisMap].[LayerProperties] ([LayerId], [Field], [Value]) VALUES (@viewZoneLayerId, 'Extent_UpperLeft_Y', CONVERT(FLOAT, 0.0))
  INSERT INTO [GisMap].[LayerProperties] ([LayerId], [Field], [Value]) VALUES (@viewZoneLayerId, 'Extent_UpperLeft_Z', CONVERT(FLOAT, 0.0))
  INSERT INTO [GisMap].[LayerProperties] ([LayerId], [Field], [Value]) VALUES (@viewZoneLayerId, 'Extent_LowerRight_X', CONVERT(FLOAT, 0.0))
  INSERT INTO [GisMap].[LayerProperties] ([LayerId], [Field], [Value]) VALUES (@viewZoneLayerId, 'Extent_LowerRight_Y', CONVERT(FLOAT, 0.0))
  INSERT INTO [GisMap].[LayerProperties] ([LayerId], [Field], [Value]) VALUES (@viewZoneLayerId, 'Extent_LowerRight_Z', CONVERT(FLOAT, 0.0))
  INSERT INTO [GisMap].[LayerProperties] ([LayerId], [Field], [Value]) VALUES (@cameraLayerId, 'Modifiers', CONVERT(INT, 0))
END

-- Delete all cameras and view zones from the gismap items table.
DELETE FROM [GisMap].[Items]
WHERE [LayerId] = @cameraLayerId OR [LayerId] = @viewZoneLayerId
GO
