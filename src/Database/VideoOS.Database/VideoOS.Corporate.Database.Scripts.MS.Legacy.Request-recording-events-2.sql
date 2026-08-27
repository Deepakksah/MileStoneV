---- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DELETE FROM EventTypes WHERE IDEventType IN ('85867627-B287-4439-9E55-A63701E1715B', '8AD836D9-EB2F-4679-AFD4-20511500DDE2')

DELETE FROM Rules WHERE IDRule = 'FA2F8209-8D9B-4580-A6EB-17E58C99A610'
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
  <CounterEventId>77B1E70D-BA8D-4bb8-9EE8-43B09746D82A</CounterEventId>
</EventType>'

IF NOT EXISTS(SELECT * FROM [EventTypes] WHERE [IDEventType] = '85867627-B287-4439-9E55-A63701E1715B')
BEGIN
	EXEC [InternalRegisterEventType] @EventDefinitionXml, @Builtin, @CurrentTime
END


-- RequestStopRecording
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>77B1E70D-BA8D-4bb8-9EE8-43B09746D82A</Id>
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

IF NOT EXISTS(SELECT * FROM [EventTypes] WHERE [IDEventType] = '77B1E70D-BA8D-4bb8-9EE8-43B09746D82A')
BEGIN
	EXEC [InternalRegisterEventType] @EventDefinitionXml, @Builtin, @CurrentTime
END


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
      <EventType>77B1E70D-BA8D-4bb8-9EE8-43B09746D82A</EventType>
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

UPDATE SecurityObjects SET IDObject = '77B1E70D-BA8D-4bb8-9EE8-43B09746D82A' where IDObject = '8AD836D9-EB2F-4679-AFD4-20511500DDE2'

GO
