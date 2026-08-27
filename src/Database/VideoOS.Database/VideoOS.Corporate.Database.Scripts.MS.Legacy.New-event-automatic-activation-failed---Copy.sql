-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @CurrentTime DateTime
SET @CurrentTime = GETUTCDATE()

DECLARE @EventDefinitionXml nvarchar(4000)

-- DatabaseDeleteBeforeSetRetentionTimeEvent
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>D85E2670-F19E-4932-B217-D8AAFFAFF4A3</Id>
  <Name>AutomaticActivationFailed</Name>
  <DisplayName referenceid="FA0B647A-8373-46F0-81AD-303CFE311A72">Automatic license activation failed</DisplayName>
  <Description referenceid="E7B61E22-93C7-49E2-B9D9-7BDB61792321">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>Server</Type>
	<SubType/>
  	<Id/>
  	<Name/>
  </Generator>
  <Sources>
    <Source>
  	  <Type>Server</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>D85E2670-F19E-4932-B217-D8AAFFAFF4A3</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

GO

