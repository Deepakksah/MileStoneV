-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Copy UseRegions and RegionSettings (XML fragments) from Settings into UISettings

DECLARE @settings NVARCHAR(MAX)
DECLARE @ui_settings NVARCHAR(MAX)
DECLARE @region_settings NVARCHAR(MAX)
DECLARE @use_regions NVARCHAR(MAX)
DECLARE @use_regions_name_pos INT
DECLARE @use_regions_end_value_pos INT
DECLARE @region_sett_name_pos INT
DECLARE @region_sett_end_value_pos INT

DECLARE recorder_plugin_device_cursor CURSOR FOR 
  SELECT [Settings], [UISettings] FROM [dbo].[RecorderPluginDevice] FOR UPDATE OF [UISettings]

OPEN recorder_plugin_device_cursor

FETCH NEXT FROM recorder_plugin_device_cursor
INTO @settings, @ui_settings

WHILE @@FETCH_STATUS = 0
BEGIN
	SET @use_regions = ''
	SET @region_settings = ''

	--get 'UseRegions' setting
	SET @use_regions_name_pos = PATINDEX('%<name>UseRegions</name>%', @settings)
	IF @use_regions_name_pos > 0
	BEGIN
		--get value element stop
		SET @use_regions_end_value_pos = CHARINDEX('</value>', @settings, @use_regions_name_pos + 23)
		IF @use_regions_end_value_pos > 0
		BEGIN
			--get all contents of the <setting>
			SET @use_regions = SUBSTRING(@settings, @use_regions_name_pos, @use_regions_end_value_pos + 8 - @use_regions_name_pos)
		END
	END

	--get 'RegionSettings' setting
	SET @region_sett_name_pos = PATINDEX('%<name>RegionSettings</name>%', @settings)
	IF @region_sett_name_pos > 0
	BEGIN
		--get value element stop
		SET @region_sett_end_value_pos = CHARINDEX('</value>', @settings, @region_sett_name_pos + 27)
		IF @region_sett_end_value_pos > 0
		BEGIN
			--get all contents of the <setting>
			SET @region_settings = SUBSTRING(@settings, @region_sett_name_pos, @region_sett_end_value_pos + 8 - @region_sett_name_pos)
		END
	END

	IF LEN(@use_regions) > 0
	BEGIN
		SET @use_regions = '<setting>' + @use_regions + '</setting>'
	END

	IF LEN(@region_settings) > 0
	BEGIN
		SET @region_settings = '<setting>' + @region_settings + '</setting>'
	END

	UPDATE [dbo].[RecorderPluginDevice] SET [UISettings] = REPLACE(@ui_settings, '</settings>', @use_regions + @region_settings + '</settings>')
	WHERE CURRENT OF recorder_plugin_device_cursor

	FETCH NEXT FROM recorder_plugin_device_cursor
	INTO @settings, @ui_settings
END

CLOSE recorder_plugin_device_cursor
DEALLOCATE recorder_plugin_device_cursor
GO
