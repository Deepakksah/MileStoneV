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

-- FailoverSslCommunicationError
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>9826E6DA-C34E-475E-958F-93925A3324BC</Id>
  <Name>FailoverEncryptedCommunicationError</Name>
  <DisplayName referenceid="2C384520-6B79-4EF5-AF42-44BD14DE73DA">Failover encrypted communication error</DisplayName>
  <Description referenceid="4FF4C667-3F41-4970-A579-1C9BB136B71D">
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
  <CounterEventId>9826E6DA-C34E-475E-958F-93925A3324BC</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime
GO