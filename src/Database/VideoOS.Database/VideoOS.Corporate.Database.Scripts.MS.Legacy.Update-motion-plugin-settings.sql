-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS (SELECT * FROM sys.columns WHERE [object_id] = OBJECT_ID(N'[RecorderPluginDevice]') AND [name] = N'UISettings')
BEGIN
	DECLARE @motionPluginId [GuidType]
	SET @motionPluginId = 'f020c881-72bf-4014-8504-2b4a5a658f1a'

	-- Create and populate a temp table with appropriate values extracted from UISettings xml
	DECLARE @RPD TABLE (IDDevice CHAR(36),
						IDRecorderPlugin CHAR(36),
						UseRegions NVARCHAR(MAX),
						RegionSettings NVARCHAR(MAX)) 
	INSERT INTO @RPD
	SELECT	[IDDevice],
			[IDRecorderPlugin],
			[UISettings].value('(properties/settings/setting[name="UseRegions"]/value/text())[1]', 'NVARCHAR(MAX)') as [UseRegions],
			[UISettings].value('(properties/settings/setting[name="RegionSettings"]/value/text())[1]', 'NVARCHAR(MAX)') as [RegionSettings]
	FROM	[RecorderPluginDevice]
	WHERE	[IDRecorderPlugin] = @motionPluginId
			AND [UISettings].exist('properties/settings/setting[name="UseRegions"]') = 1
			AND [UISettings].exist('properties/settings/setting[name="RegionSettings"]') = 1

	-- Take value 'UseRegions' from settings in UISettings and apply to settings in Settings
	UPDATE rpd1
	SET [Settings].modify('replace value of (properties/settings/setting[name="UseRegions"]/value/text())[1] with sql:column("UseRegions")')
	FROM [RecorderPluginDevice] rpd1
	INNER JOIN (SELECT [IDDevice], [IDRecorderPlugin], [UseRegions] FROM @RPD) rpd2 ON rpd2.[IDDevice] = rpd1.[IDDevice] AND rpd2.[IDRecorderPlugin] = rpd1.[IDRecorderPlugin]

	-- Take value 'RegionSettings' from settings in UISettings and apply to settings in Settings
	UPDATE rpd1
	SET [Settings].modify('replace value of (properties/settings/setting[name="RegionSettings"]/value/text())[1] with sql:column("RegionSettings")')
	FROM [RecorderPluginDevice] rpd1
	INNER JOIN (SELECT [IDDevice], [IDRecorderPlugin], [RegionSettings] FROM @RPD) rpd2 ON rpd2.[IDDevice] = rpd1.[IDDevice] AND rpd2.[IDRecorderPlugin] = rpd1.[IDRecorderPlugin]

	ALTER TABLE [RecorderPluginDevice] DROP COLUMN [UISettings]

END
GO
