-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================


DECLARE @CurrentTime DateTime
SET @CurrentTime = GETUTCDATE()

DECLARE @Builtin bit
DECLARE @EventDefinitionXml nvarchar(4000)

-- FailoverAuthorizationError
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>D7940922-5CB4-40E4-AC62-7CE361DA8298</Id>
  <Name>FailoverAuthorizationError</Name>
  <DisplayName referenceid="7B3B58AE-6774-4D94-B215-5203181F1DFD">Failover authorization error</DisplayName>
  <Description referenceid="C286DFA3-2C94-4567-A002-A0127BEEF5E3">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>Recorder</Type>
  	<Id/>
   <SubType/>
  	<Name/>
  </Generator>
  <Sources>
    <Source>
  	  <Type>Recorder</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>D7940922-5CB4-40E4-AC62-7CE361DA8298</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime
GO