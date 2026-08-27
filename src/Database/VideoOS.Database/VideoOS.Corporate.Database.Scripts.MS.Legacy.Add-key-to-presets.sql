-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
DECLARE @deviceId [GuidType]
DECLARE @presetXml XML

DECLARE device_cursor CURSOR FOR
SELECT [IDDevice], [Presets]
FROM [Devices]
WHERE [DeviceType] = 'Camera'
AND [Presets].exist('//key') = 0 -- NOT EXISTS

OPEN device_cursor
FETCH NEXT FROM device_cursor INTO @deviceId, @presetXml

WHILE @@FETCH_STATUS = 0
BEGIN
	DECLARE @presetName VARCHAR(500)
	DECLARE @presetId VARCHAR(500)
	DECLARE @presetNumber VARCHAR(500)
	DECLARE @presetIsNumber VARCHAR(500)
	DECLARE @presetIsDevicePreset VARCHAR(500)
	DECLARE @isPresetNameUnique BIT

	DECLARE @PresetsTable Table(Id CHAR(36), Name NVARCHAR(500), Number VARCHAR(500), IsNumber NVARCHAR(500), IsDevicePreset NVARCHAR(500), IsNameUnique BIT, [NewName] NVARCHAR(500)) 
	INSERT INTO @PresetsTable ([Id], [Name], [Number], [IsNumber], [IsDevicePreset], [IsNameUnique], [NewName])
	SELECT 
		preset.value('@id[1]', 'CHAR(36)') as PresetId,
		preset.value('name[1]','NVARCHAR(500)') as PresetName, 
		preset.value('number[1]','VARCHAR(500)') as PresetNumber,
		preset.value('isnumbered[1]','NVARCHAR(500)') as PresetIsNumber,
		preset.value('isdevicepreset[1]','NVARCHAR(500)') AS PresetIsDevicePreset, 
		CASE ROW_NUMBER() OVER(PARTITION BY preset.value('name[1]', 'NVARCHAR(500)') ORDER BY (SELECT 1)) 
			WHEN 1 THEN 1
			ELSE 0
		END as IsNameUnique,
		preset.value('name[1]','NVARCHAR(500)') as NewPresetName
	FROM @presetXml.nodes('presets/preset') Presets(preset) 

	DECLARE device_preset_cursor CURSOR FOR
	SELECT [Id], [Name], [Number], [IsNumber], [IsDevicePreset], [IsNameUnique]
	FROM @PresetsTable


	OPEN device_preset_cursor
	FETCH NEXT FROM device_preset_cursor INTO @presetId, @presetName, @presetNumber, @presetIsNumber, @presetIsDevicePreset, @isPresetNameUnique
	WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE @Key VARCHAR(500)
		IF(@presetIsNumber = 'True')
		BEGIN
			SET @Key = @presetNumber
		END
		ELSE IF(@presetIsDevicePreset = 'True')
		BEGIN
			SET @Key = @presetName
		END
		ELSE
		BEGIN
			SET @Key = @presetId
		END

		SET @presetXml.modify('insert <key>{ sql:variable("@Key") }</key> as first into (/presets/preset[@id=sql:variable("@presetId")])[1]')

		IF(@isPresetNameUnique = 0) -- NOT UNIQUE
		BEGIN
			DECLARE @NewName NVARCHAR(500)
			DECLARE @index INT = 0
			SET @NewName = @presetName
			WHILE((SELECT COUNT(*) FROM @PresetsTable WHERE [NewName] = @NewName) > 1)
			BEGIN
				SET @index = @index +1
				SELECT @NewName = 
							CASE ROW_NUMBER() OVER(PARTITION BY [NewName] ORDER BY [NewName])
								WHEN 1 THEN [NewName]
								ELSE @presetName + ' (' + CAST(@index AS VARCHAR(MAX)) + ')'
							END
				FROM @PresetsTable

				UPDATE @PresetsTable
				SET [NewName] = @NewName
				WHERE Id = @presetId
			END
			SET @presetXml.modify('replace value of (/presets/preset[@id=sql:variable("@presetId")]/name/text())[1] with sql:variable("@NewName")')
		END

		FETCH NEXT FROM device_preset_cursor INTO @presetId, @presetName, @presetNumber, @presetIsNumber, @presetIsDevicePreset, @isPresetNameUnique
	END

	CLOSE device_preset_cursor
	DEALLOCATE device_preset_cursor

	UPDATE [Devices]
	SET [Presets] = @presetXml
	WHERE IDDevice = @deviceId

	FETCH NEXT FROM device_cursor INTO  @deviceId, @presetXml
END
CLOSE device_cursor
DEALLOCATE device_cursor
GO
