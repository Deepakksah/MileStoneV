---- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @CurrentTime DateTime
SET @CurrentTime = GETUTCDATE()

DECLARE @EventDefinitionXml nvarchar(4000)

-- Manual Recording Started
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>D740951A-984D-4049-99B5-25F5CBABD21F</Id>
  <Name>ManualRecordingStarted</Name>
  <DisplayName referenceid="6D440430-E6DC-4014-98F1-E712C0558C95">Manual Recording Started</DisplayName>
  <Description referenceid="6A1CE48B-8152-4EBB-A231-ADCF44FF328A">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>Device</Type>
	<SubType/>
  	<Id/>
  	<Name/>
  </Generator>
  <Sources>
    <Source>
  	  <Type>Camera</Type>
  	  <Filters>
  	  </Filters>
    </Source>
    <Source>
  	  <Type>AudioInput</Type>
  	  <Filters>
  	  </Filters>
    </Source>
    <Source>
  	  <Type>AudioOutput</Type>
  	  <Filters>
  	  </Filters>
    </Source>
	<Source>
  	  <Type>Metadata</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>A9C1F6FA-ADD6-4BF4-BA2D-A0CEDDB73C51</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Manual Recording Stopped
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>A9C1F6FA-ADD6-4BF4-BA2D-A0CEDDB73C51</Id>
  <Name>ManualRecordingStopped</Name>
  <DisplayName referenceid="DC47B6BD-50C3-4F00-8766-37C398C8A2C0">Manual Recording Stopped</DisplayName>
  <Description referenceid="498EDB1C-249D-41A7-8FD8-A6135EE05CC8">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>Device</Type>
	<SubType/>
  	<Id/>
  	<Name/>
  </Generator>
  <Sources>
    <Source>
  	  <Type>Camera</Type>
  	  <Filters>
  	  </Filters>
    </Source>
    <Source>
  	  <Type>AudioInput</Type>
  	  <Filters>
  	  </Filters>
    </Source>
    <Source>
  	  <Type>AudioOutput</Type>
  	  <Filters>
  	  </Filters>
    </Source>
	<Source>
  	  <Type>Metadata</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>D740951A-984D-4049-99B5-25F5CBABD21F</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

GO
