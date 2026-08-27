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

-- FailoverStarted
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>F951B1F0-2FED-48F7-88D3-49EB5999C923</Id>
  <Name>FailoverStarted</Name>
  <DisplayName referenceid="B00A1F17-76D6-486E-BBA3-7459B6D7300E">Failover Started</DisplayName>
  <Description referenceid="FA125B3C-F0C7-4F89-BC49-36CACC13370A">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>Recorder</Type>
  	<Id/>
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
  <CounterEventId>C7B9227A-8E11-451B-BC9C-8EA659731012</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @Builtin, @CurrentTime

-- FailoverStopped
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>C7B9227A-8E11-451B-BC9C-8EA659731012</Id>
  <Name>FailoverStopped</Name>
  <DisplayName referenceid="3F8E1607-ECFD-4D13-8005-4767123025DA">Failover Stopped</DisplayName>
  <Description referenceid="2530E29C-CA91-4A24-BDAF-C946A30429BF">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>Recorder</Type>
  	<Id/>
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
  <CounterEventId>F951B1F0-2FED-48F7-88D3-49EB5999C923</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @Builtin, @CurrentTime
GO
