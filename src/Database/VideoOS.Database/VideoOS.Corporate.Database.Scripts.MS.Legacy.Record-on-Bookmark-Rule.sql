---- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @CurrentTime DateTime
SET @CurrentTime = GETUTCDATE()

DECLARE @RuleXml nvarchar(4000)
SET @RuleXml = 
N'<Rule>
  <StartRule>
    <RuleType type="Event">
      <EventType>7cfc6709-2c16-405f-9182-0bdb6f9f7e43</EventType>
      <Sources>
        <SourceList />
        <GroupList>
          <GroupId>0e1b0ad3-f67c-4d5f-b792-4bd6c3cf52f8</GroupId>
          <GroupId>76d123cb-1e0e-4a2a-bdd5-e4b4b2894614</GroupId>
          <GroupId>d92e5c68-f712-49d7-a99d-f88256645d5a</GroupId>
        </GroupList>
      </Sources>
    </RuleType>
    <Conditions>
      <TimeCondition />
      <Condition />
    </Conditions>
    <Actions>
      <Action name="VideoOS.DeviceRecording" instanceId="0e539049-414f-44e1-b2fb-a287318dd938">
        <Parameter name="State">On</Parameter>
        <Parameter name="DeviceIds">00000000-0000-0000-0000-000000000000</Parameter>
        <Parameter name="DeviceGroupIds" />
        <Parameter name="Delay">-3</Parameter>
      </Action>
    </Actions>
  </StartRule>
  <StopRule>
    <RuleType type="Timer">
      <TimerValue>30</TimerValue>
    </RuleType>
    <Actions>
      <Action name="VideoOS.DeviceRecording" instanceId="0eee292d-d492-417b-9f04-b47df3b39542">
        <Parameter name="State">Off</Parameter>
        <Parameter name="DeviceIds">00000000-0000-0000-0000-000000000000</Parameter>
        <Parameter name="DeviceGroupIds" />
        <Parameter name="Delay">0</Parameter>
      </Action>
    </Actions>
  </StopRule>
</Rule>'

IF NOT EXISTS(SELECT TOP 1 * FROM RULES WHERE [IDRule] = '4CE46D3E-C4C7-46b2-A580-FC98CDC24611' )
BEGIN
	INSERT INTO Rules
	VALUES(@CurrentTime,'4CE46D3E-C4C7-46b2-A580-FC98CDC24611','Default Record on Bookmark','',@RuleXml,1,100)   
END
GO

