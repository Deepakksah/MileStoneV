-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @IDDevice [GuidType]
DECLARE @PresetsXml XML
DECLARE @id [GuidType]
 
DECLARE preset_cursor CURSOR FOR
SELECT [IDDevice], [Presets] FROM [Devices] 
WHERE [Presets] IS NOT NULL AND [Presets].exist('/presets/preset[@id]') = 0
 
OPEN preset_cursor
FETCH NEXT FROM preset_cursor INTO @IDDevice, @PresetsXml
 
WHILE @@FETCH_STATUS = 0
BEGIN
    DECLARE @seed int = 0
    WHILE @seed < @PresetsXml.value('count(/presets/preset)', 'int')
    BEGIN
        SET @seed = @seed + 1
		SET @id = NEWID()
        SET @PresetsXml.modify('insert attribute id {sql:variable("@id")} as first into (/presets/preset)[sql:variable("@seed")][1]')
    END

    UPDATE [Devices] SET [Presets] = @PresetsXml WHERE [IDDevice] = @IDDevice

    FETCH NEXT FROM preset_cursor INTO @IDDevice, @PresetsXml
END
CLOSE preset_cursor
DEALLOCATE preset_cursor


GO
