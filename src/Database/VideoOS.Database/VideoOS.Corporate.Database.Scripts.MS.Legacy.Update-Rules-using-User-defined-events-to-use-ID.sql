-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Temp table holding mapping values between Preset Id and the Preset name/number on a device
DECLARE @eventValues TABLE(
eventId CHAR(36),
eventName NVARCHAR(256))

INSERT INTO @eventValues
SELECT IdEventType, Name 
FROM EventTypes (NOLOCK)
where GeneratorSubType = 'UserDefined'

DECLARE @currentEventId NVARCHAR(50), @currentEventName NVARCHAR(256)

--Cursor running through all mapping compinations and updating rules
DECLARE eventCursor CURSOR FORWARD_ONLY STATIC READ_ONLY FOR
SELECT eventId, eventName FROM @eventValues

OPEN eventCursor

FETCH NEXT FROM eventCursor 
INTO @currentEventId, @currentEventName

WHILE @@FETCH_STATUS = 0
	BEGIN
		
		--select @currentEventId, @currentEventName
	  WHILE EXISTS (SELECT 1 FROM Rules WHERE [Definition].exist('(//Actions/Action[@name="VideoOS.TriggerUserDefinedEventOnRemoteSite"]/Parameter[@name="UserDefinedEvent"][text() = sql:variable("@currentEventName")])[1]') = 1)
	  BEGIN
	  
	    --update EventId based on the EventName
		UPDATE Rules
		SET [Definition].modify('replace value of (//Actions/Action[@name="VideoOS.TriggerUserDefinedEventOnRemoteSite"]/Parameter[@name="UserDefinedEvent"][text() = sql:variable("@currentEventName")]/text())[1] with sql:variable("@currentEventId")')
	  END

	  FETCH NEXT FROM eventCursor 
	  INTO @currentEventId, @currentEventName

END 
CLOSE eventCursor;
DEALLOCATE eventCursor;

GO