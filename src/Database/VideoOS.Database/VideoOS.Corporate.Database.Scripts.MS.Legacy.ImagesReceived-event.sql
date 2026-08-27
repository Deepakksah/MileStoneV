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

-- Images Received Event
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>C22C1456-9319-455F-A82C-07C8F87B0B50</Id>
  <Name>ImagesReceived</Name>
  <DisplayName referenceid="8B8E2D4E-2093-48E8-8FD5-BFAD3B614115">Images Received</DisplayName>
  <Description referenceid="2DAC338E-DEA1-4BC7-B71D-654DDE21B89A">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>DriverDevice</Type>
  	<Id></Id>
  	<Name/>
  </Generator>
  <Sources>
    <Source>
  	  <Type>Camera</Type>
  	  <Filters>
  	  </Filters>
    </Source>
    <Source>
  	  <Type>Input</Type>
  	  <Filters>
  	  </Filters>
    </Source>
    <Source>
  	  <Type>Output</Type>
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
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <CounterEventId>C22C1456-9319-455F-A82C-07C8F87B0B50</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @Builtin, @CurrentTime

GO
