-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @AllEvents TABLE (id UNIQUEIDENTIFIER, displayName NVARCHAR(256), st INTEGER);
INSERT INTO @AllEvents 
	SELECT e.[IDEventType] AS [ID], '', 0 FROM [EventTypes] e 
	WHERE e.[GeneratorType] = 'DriverDevice' AND (e.[Definition].value('/EventType[1]/Generator[1]/Name[1]', 'NVARCHAR(MAX)') = 'DynamicEvent');
INSERT INTO @AllEvents 
	SELECT e.[IDEventType] AS [ID], e.[Definition].value('/EventType[1]/DisplayName[1]', 'NVARCHAR(MAX)'), 1 FROM [EventTypes] e 
	WHERE (e.[Definition].value('/EventType[1]/Generator[1]/Name[1]', 'NVARCHAR(MAX)') <> 'DynamicEvent');
	
DECLARE eventCursor CURSOR FAST_FORWARD FOR 
	SELECT id FROM @AllEvents;

	
DECLARE @id uniqueidentifier
OPEN eventCursor;
FETCH NEXT FROM eventCursor INTO @id
WHILE @@FETCH_STATUS = 0
BEGIN
	DECLARE @count integer
	DECLARE @displayName NVARCHAR(256)
	DECLARE @st integer

	SELECT @st = st FROM @AllEvents WHERE id = @id;
	IF (@st = 0)
	BEGIN
		SELECT @displayName = ([Definition].value('/EventType[1]/DisplayName[1]', 'NVARCHAR(MAX)')) FROM [EventTypes] WHERE IDEventType = @id;


		SELECT @count = COUNT(*) FROM @AllEvents WHERE displayname = @displayName;
		IF (@count = 0)
		BEGIN
			/* Update state to keep */
			UPDATE @AllEvents SET st = 1,displayname = @displayName WHERE id = @id;		
		END
		ELSE
		BEGIN
			/* Update state to delete */
			UPDATE @AllEvents SET st = 2,displayname = @displayName WHERE id = @id;		
		END
	END

	FETCH NEXT FROM eventCursor INTO @id
END
CLOSE eventCursor

/* Now delete dublicates in EventTypes */
OPEN eventCursor;
FETCH NEXT FROM eventCursor INTO @id
WHILE @@FETCH_STATUS = 0
BEGIN
	IF (SELECT st as Result FROM @AllEvents WHERE id = @id) = 2
	BEGIN
		DELETE FROM [EventTypes] WHERE [IDEventType] = @id
	END

	FETCH NEXT FROM eventCursor INTO @id
END
CLOSE eventCursor
DEALLOCATE eventCursor

GO
