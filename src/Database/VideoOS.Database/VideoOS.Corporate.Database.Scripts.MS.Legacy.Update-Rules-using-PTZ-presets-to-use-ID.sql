-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Temp table holding mapping values between Preset Id and the Preset name/number on a device
DECLARE @presetValues TABLE(
deviceId CHAR(36),
presetId NVARCHAR(50),
presetNumber NVARCHAR(50), 
presetName NVARCHAR(50))

INSERT INTO @presetValues
SELECT dvc.IDDevice as DeviceId
		, preset.value('@id', 'nvarchar(50)') AS presetId
		,preset.value('number[1]', 'nvarchar(50)') AS presetNumber
		,preset.value('name[1]', 'nvarchar(50)') AS presetName
FROM Devices dvc (NOLOCK)
CROSS apply dvc.Presets.nodes('presets/preset') Presets(preset)

DECLARE @currentPresetNumber NVARCHAR(50), @currentDeviceId NVARCHAR(50),
@currentPresetId NVARCHAR(50), @currentPresetName NVARCHAR(50)

--Cursor running through all mapping compinations and updating rules
DECLARE presetCursor CURSOR FORWARD_ONLY STATIC READ_ONLY FOR
SELECT deviceId, presetId, presetNumber, presetName FROM @presetValues

OPEN presetCursor

FETCH NEXT FROM presetCursor 
INTO @currentDeviceId, @currentPresetId, @currentPresetNumber, @currentPresetName

WHILE @@FETCH_STATUS = 0
	BEGIN
		
	  WHILE EXISTS (SELECT 1 FROM Rules WHERE [Definition].exist('(//Actions/Action/Parameter[@name="DeviceId"][text() = sql:variable("@currentDeviceId")]/../Parameter[@name="PresetId"][text() = sql:variable("@currentPresetNumber")])[1]') = 1)
	  BEGIN
	    -- update PresetId based on the PresetNumber and DeviceId  
		UPDATE Rules
		SET [Definition].modify('replace value of (//Actions/Action/Parameter[@name="DeviceId"][text() = sql:variable("@currentDeviceId")]/
		../Parameter[@name="PresetId"][text() = sql:variable("@currentPresetNumber")]/text())[1] with sql:variable("@currentPresetId")')
	  END

	  WHILE EXISTS (SELECT 1 FROM Rules WHERE [Definition].exist('(//Actions/Action/Parameter[@name="DeviceId"][text() = sql:variable("@currentDeviceId")]/../Parameter[@name="PresetId"][text() = sql:variable("@currentPresetName")])[1]') = 1)
	  BEGIN
		-- update PresetId based on the PresetName and DeviceId
		UPDATE Rules
		SET [Definition].modify('replace value of (//Actions/Action/Parameter[@name="DeviceId"][text() = sql:variable("@currentDeviceId")]/
		../Parameter[@name="PresetId"][text() = sql:variable("@currentPresetName")]/text())[1] with sql:variable("@currentPresetId")')
	  END

	  FETCH NEXT FROM presetCursor 
	  INTO @currentDeviceId, @currentPresetId, @currentPresetNumber, @currentPresetName

END 
CLOSE presetCursor;
DEALLOCATE presetCursor;

GO