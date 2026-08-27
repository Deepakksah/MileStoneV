---- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

BEGIN TRAN

DECLARE @CurrentTime DateTime
SET @CurrentTime = GETUTCDATE()

DECLARE @status INT
DECLARE @namespace NVARCHAR(260)

DECLARE @procedureName SYSNAME
SELECT  @procedureName = @@SERVERNAME  + '.' + db_name()
DECLARE @ErrorArgs NVARCHAR(500)


DECLARE @Builtin bit
DECLARE @EventDefinitionXml nvarchar(4000)


-- RequestPlayAudio
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<EventType>
  <Id>7605F8B0-7F5F-4432-B223-0BB2DC3F1F5C</Id>
  <Name>RequestPlayAudio</Name>
  <DisplayName referenceid="351D8238-2F50-4CD2-B512-67270AD7AD5E">RequestPlayAudio</DisplayName>
  <Description referenceid="76B63B48-57DC-4E15-9E02-711BC946B4C4" />
  <Parameters />
  <Generator>
    <Type>External</Type>
    <SubType>System</SubType>
    <Id />
    <Name />
  </Generator>
  <Sources>
    <Source>
      <Type>External</Type>
      <Filters />
    </Source>
  </Sources>
  <OccursGlobally>True</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>7605F8B0-7F5F-4432-B223-0BB2DC3F1F5C</CounterEventId>
</EventType>'

DECLARE @event_id [GuidType]
SET @event_id = '7605F8B0-7F5F-4432-B223-0BB2DC3F1F5C'

IF NOT EXISTS(SELECT * FROM [EventTypes] WHERE [IDEventType] = @event_id)
BEGIN
    EXEC [InternalRegisterEventType] @EventDefinitionXml, @CurrentTime

    SET @namespace = 'UserDefinedEvent'

    EXEC [security_register_object_transaction] 
                                        @namespace_name = @namespace,
                                        @object_id = @event_id,
                                        @parent_path = '/',
                                        @CurrentUtcTime = @CurrentTime
    IF (@status <> 0)
    BEGIN
		SET @ErrorArgs = @procedureName + N': %%Status=' + CAST(@status AS NVARCHAR(10))  + N'%%; %%Namespace=' + @namespace + N'%%; %%ID=' + CAST(@event_id AS NVARCHAR(36)) + N'%%'
		;THROW 65000, @ErrorArgs, 1;
    END
END



-- Add rule to play audio message from metadata on devices from metadata
DECLARE @RuleXml nvarchar(4000)
SET @RuleXml = 
N'<Rule>
  <StartRule>
    <RuleType type="Event">
      <EventType>7605F8B0-7F5F-4432-B223-0BB2DC3F1F5C</EventType>
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
      <Action name="VideoOS.ActionPlaySound" instanceId="6CB710CA-A8D4-420D-802C-8B5D40D9073A">
        <Parameter name="DeviceIds">11111111-1111-1111-1111-111111111111</Parameter>
        <Parameter name="DeviceGroupIds" />
        <Parameter name="MessageId">11111111-1111-1111-1111-111111111111</Parameter>
        <Parameter name="Priority">1</Parameter>
        <Parameter name="Delay">0</Parameter>
      </Action>
    </Actions>
  </StartRule>
  <StopRule />
</Rule>'


DECLARE @rule_id [GuidType]
SET @rule_id = 'A38F72D5-05E2-4EA5-84EC-E4269E8242FE'

IF NOT EXISTS(SELECT * FROM [Rules] WHERE [IDRule] = @rule_id)
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
        @rule_id,
        'Default Play Audio on Request Rule',
        '',
        @RuleXml,
        1,
        100,
        @CurrentTime
    )


    SET @namespace = 'Rule'

    EXEC @status = security_register_object_transaction
                                        @namespace_name = @namespace,
                                        @object_id = @rule_id,
                                        @parent_path = '/',
                                        @CurrentUtcTime = @CurrentTime
    IF (@status <> 0)
    BEGIN
		SET @ErrorArgs = @procedureName + N': %%Status=' + CAST(@status AS NVARCHAR(10))  + N'%%; %%Namespace=' + @namespace + N'%%; %%ID=' + CAST(@rule_id AS NVARCHAR(36)) + N'%%'
		;THROW 65000, @ErrorArgs, 1;
    END

END

COMMIT TRAN

GO
