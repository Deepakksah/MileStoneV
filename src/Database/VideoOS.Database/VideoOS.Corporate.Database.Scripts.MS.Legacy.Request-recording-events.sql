---- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[set_event_security]') AND type in (N'P', N'PC'))
DROP PROCEDURE [set_event_security]
GO

CREATE PROCEDURE [set_event_security]
	@eventId [GuidType],
	@CurrentUtcTime DATETIME
AS
BEGIN
	DECLARE @status INT
	
	-- Ensure that the USER_DEFINED_EVENT class exists
	DECLARE @IDClass VARCHAR(128)
	SET @IDClass = 'USER_DEFINED_EVENT'

	DECLARE @IDAction	VARCHAR(128)
	SET @IDAction = 'GENERIC_READ'
	
	IF NOT EXISTS(SELECT * FROM [SecurityActions] WHERE [IDClass] = @IDClass AND [IDAction] = @IDAction)
	INSERT [SecurityActions] ([IDClass], [IDAction], [OrderNo], [ShowAction])
	VALUES(@IDClass, @IDAction, 0, 1)

	SET @IDAction = 'GENERIC_WRITE'

	IF NOT EXISTS(SELECT * FROM [SecurityActions] WHERE [IDClass] = @IDClass AND [IDAction] = @IDAction)
	INSERT [SecurityActions] ([IDClass], [IDAction], [OrderNo], [ShowAction])
	VALUES(@IDClass, @IDAction, 1, 1)

	SET @IDAction = 'TRIGGER_EVENT'

	IF NOT EXISTS(SELECT * FROM [SecurityActions] WHERE [IDClass] = @IDClass AND [IDAction] = @IDAction)
	INSERT [SecurityActions] ([IDClass], [IDAction], [OrderNo], [ShowAction])
	VALUES(@IDClass, @IDAction, 2, 1)

	DECLARE @adminRoleId AS [SIDType]
	SET @adminRoleId = '1f30d14b-ba91-4e8a-8838-eda17931f8fc'

	-- Register the object in the security system
	EXEC @status = [security_register_object_transaction]
		@SID = @adminRoleId,
		@IDObject = @eventId,
		@IDClass = @IDClass,
		@IDParentObject	= NULL,
		@CurrentUtcTime	= @CurrentUtcTime

	IF(@status <> 0)
	BEGIN
		RETURN @status
	END

	RETURN 0
END
GO

DECLARE @CurrentTime DateTime
SET @CurrentTime = GETUTCDATE()

DECLARE @Builtin bit
DECLARE @EventDefinitionXml nvarchar(4000)

-- RequestStartRecording
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>85867627-B287-4439-9E55-A63701E1715B</Id>
  <Name>RequestStartRecording</Name>
  <DisplayName referenceid="10B5C0B4-7733-4145-8437-B9796BF4BCF6">RequestStartRecording</DisplayName>
  <Description referenceid="9B080CFB-6390-454F-B955-0A05A99195F0">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
    <Type>External</Type>
  	<Id/>
  	<Name/>
  </Generator>
  <Sources>
    <Source>
      <Type>External</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>True</OccursGlobally>
  <CounterEventId>8AD836D9-EB2F-4679-AFD4-20511500DDE2</CounterEventId>
</EventType>'

IF NOT EXISTS(SELECT * FROM [EventTypes] WHERE [IDEventType] = '85867627-B287-4439-9E55-A63701E1715B')
BEGIN
	EXEC [InternalRegisterEventType] @EventDefinitionXml, @Builtin, @CurrentTime
END

-- Configure security for RequestStartRecording
EXEC [set_event_security]
	@eventId = '85867627-B287-4439-9E55-A63701E1715B',
	@CurrentUtcTime = @CurrentTime

-- RequestStopRecording
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>8AD836D9-EB2F-4679-AFD4-20511500DDE2</Id>
  <Name>RequestStopRecording</Name>
  <DisplayName referenceid="0638899C-3163-4B89-9C75-767D6354B92F">RequestStopRecording</DisplayName>
  <Description referenceid="0289C8E2-6C63-45DD-A021-C3C4E83C6FA4">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
    <Type>External</Type>
  	<Id/>
  	<Name/>
  </Generator>
  <Sources>
    <Source>
      <Type>External</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>True</OccursGlobally>
  <CounterEventId>85867627-B287-4439-9E55-A63701E1715B</CounterEventId>
</EventType>'

IF NOT EXISTS(SELECT * FROM [EventTypes] WHERE [IDEventType] = '8AD836D9-EB2F-4679-AFD4-20511500DDE2')
BEGIN
	EXEC [InternalRegisterEventType] @EventDefinitionXml, @Builtin, @CurrentTime
END

-- Configure security for RequestStopRecording
EXEC [set_event_security]
	@eventID = '8AD836D9-EB2F-4679-AFD4-20511500DDE2',
	@CurrentUtcTime = @CurrentTime

-- Add rule to record on devices from metadata
DECLARE @RuleXml nvarchar(4000)
SET @RuleXml = 
N'<Rule>
  <StartRule>
    <RuleType type="Event">
      <EventType>85867627-b287-4439-9e55-a63701e1715b</EventType>
      <Sources>
        <SourceList>
          <SourceId>e8bd6cee-1119-4296-ba91-e3803e2c591e</SourceId>
        </SourceList>
        <GroupList />
      </Sources>
    </RuleType>
    <Conditions>
      <TimeCondition />
      <Condition />
    </Conditions>
    <Actions>
      <Action name="VideoOS.DeviceRecording" instanceId="83f3a78d-2e36-41f1-9ba8-de9af5dae950">
        <Parameter name="State">On</Parameter>
        <Parameter name="DeviceIds">11111111-1111-1111-1111-111111111111</Parameter>
        <Parameter name="DeviceGroupIds" />
        <Parameter name="Delay">0</Parameter>
      </Action>
    </Actions>
  </StartRule>
  <StopRule>
    <RuleType type="Event">
      <EventType>8ad836d9-eb2f-4679-afd4-20511500dde2</EventType>
      <Sources>
        <SourceList>
          <SourceId>e8bd6cee-1119-4296-ba91-e3803e2c591e</SourceId>
        </SourceList>
        <GroupList />
      </Sources>
    </RuleType>
    <Actions>
      <Action name="VideoOS.DeviceRecording" instanceId="c6cf8af7-7f6b-4949-8620-0cfc6f4f2ed4">
        <Parameter name="State">Off</Parameter>
        <Parameter name="DeviceIds">11111111-1111-1111-1111-111111111111</Parameter>
        <Parameter name="DeviceGroupIds" />
        <Parameter name="Delay">0</Parameter>
      </Action>
    </Actions>
  </StopRule>
</Rule>'

IF NOT EXISTS(SELECT * FROM [Rules] WHERE [IDRule] = 'FA2F8209-8D9B-4580-A6EB-17E58C99A610' )
BEGIN
	INSERT Rules (
		[IDRule],
		[Name],
		[Description],
		[Definition],
		[Enabled],
		[Priority],
		[LastModified]
	)
	VALUES (
		'FA2F8209-8D9B-4580-A6EB-17E58C99A610',
		'Default Record on Request',
		'',
		@RuleXml,
		1,
		100,
		@CurrentTime
	)
END
GO

DROP PROCEDURE [set_event_security]
GO
