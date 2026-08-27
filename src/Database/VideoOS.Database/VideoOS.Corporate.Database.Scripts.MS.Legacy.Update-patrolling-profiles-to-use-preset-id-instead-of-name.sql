-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @patrollingId [GuidType]
DECLARE @deviceId [GuidType]
DECLARE @patrollingXml XML
DECLARE @presetXml XML

DECLARE profile_cursor CURSOR FOR
SELECT pp.[IDPatrollingProfile], pp.[IDDevice], CAST(pp.[Definition] AS XML) AS PatrollingXml, d.[Presets]
FROM [PatrollingProfiles] pp
INNER JOIN [Devices] d ON d.[IDDevice] = pp.[IDDevice]

OPEN profile_cursor
FETCH NEXT FROM profile_cursor INTO @patrollingId, @deviceId, @patrollingXml, @presetXml

WHILE @@FETCH_STATUS = 0
BEGIN

	DECLARE @presetname VARCHAR(500)
	DECLARE @presetid VARCHAR(500)

	DECLARE preset_cursor CURSOR FOR
	SELECT x.value('.','VARCHAR(500)') AS PresetName, x.value('../@id', 'VARCHAR(500)') AS PresetId
	FROM @presetXml.nodes('/presets/preset/name') y(x)
 
	OPEN preset_cursor
	FETCH NEXT FROM preset_cursor INTO @presetname, @presetid

	WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE @seed INT = 0
		DECLARE @numberOfPresets INT = @patrollingXml.value('count(//PresetKey)', 'INT')
		WHILE (@seed < @numberOfPresets)
		BEGIN
				SET @seed = @seed + 1
				DECLARE @existingname VARCHAR(500)
				SELECT @existingname = @patrollingXml.value('(//PresetKey)[sql:variable("@seed")][1]', 'VARCHAR(500)')
				IF(@existingname = @presetname)
				BEGIN
					SET @patrollingXml.modify('replace value of ((//PresetKey)[sql:variable("@seed")]/text())[1] with sql:variable("@presetid")')
				END
		END

		FETCH NEXT FROM preset_cursor INTO @presetname, @presetid

	END
	CLOSE preset_cursor
	DEALLOCATE preset_cursor

	UPDATE [PatrollingProfiles]
	SET [Definition] = CAST(@patrollingXml AS NVARCHAR(MAX))
	WHERE IDPatrollingProfile = @patrollingId

	FETCH NEXT FROM profile_cursor INTO @patrollingId, @deviceId, @patrollingXml, @presetXml

END
CLOSE profile_cursor
DEALLOCATE profile_cursor
GO
