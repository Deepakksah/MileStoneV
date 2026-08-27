-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @IDDevice GuidType
DECLARE @PresetXMLData xml
 
DECLARE preset_xml_cursor CURSOR FOR
SELECT IDDevice, Presets
FROM Devices
WHERE Presets IS NOT NULL AND
Presets.exist('//preset') = 1
 
OPEN preset_xml_cursor
FETCH NEXT FROM preset_xml_cursor INTO @IDDevice, @PresetXMLData
 
WHILE @@FETCH_STATUS = 0
BEGIN
	DECLARE @seed INT = 0
	DECLARE @lockedExists BIT 
	WHILE @seed < @PresetXMLData.value('count(/presets/preset)', 'int')
	BEGIN
		SET @seed = @seed + 1
		
		SET @lockedExists = (SELECT @PresetXMLData.exist('(/presets/preset)[sql:variable("@seed")]/locked'))
		IF(@lockedExists = 0)
		BEGIN
			SET @PresetXMLData.modify('insert element locked {"False"} after 
				((/presets/preset)[sql:variable("@seed")]/isdefault)[1]')
		END
	END

	UPDATE DEVICES
	SET Presets = @PresetXMLData 
	WHERE [IDDevice] = @IDDevice

	FETCH NEXT FROM preset_xml_cursor INTO @IDDevice, @PresetXMLData
END
CLOSE preset_xml_cursor
DEALLOCATE preset_xml_cursor
GO
