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
	    <EventType>eed47283-694f-4683-9e4d-18e7d93cf796</EventType>
	    <Sources>
		  <SourceList/>
 		  <GroupList>
		    <GroupId>0e1b0ad3-f67c-4d5f-b792-4bd6c3cf52f8</GroupId>
		  </GroupList>
		</Sources>
	  </RuleType>
	  <Conditions>
	    <TimeCondition/>
		<Condition/>
	  </Conditions>
	  <Actions>
		<Action name="VideoOS.GotoDefaultPreset" instanceId="e7e13260-ec2b-43ab-aba7-caf3ffa1c037">
			<Parameter name="DeviceIds">00000000-0000-0000-0000-000000000000</Parameter> 
			<Parameter name="DeviceGroupIds" /> 
			<Parameter name="Delay">0</Parameter> 
		</Action>
	  </Actions>
	</StartRule>
	<StopRule />
  </Rule>'

IF NOT EXISTS(SELECT TOP 1 * FROM RULES WHERE [IDRule] = '7AA28F82-F3FF-4398-9781-061299178C7F' )
BEGIN
	INSERT INTO Rules
	VALUES(@CurrentTime,'7AA28F82-F3FF-4398-9781-061299178C7F','Default Goto Preset when PTZ is done','',@RuleXml,0,100)   
END
GO

