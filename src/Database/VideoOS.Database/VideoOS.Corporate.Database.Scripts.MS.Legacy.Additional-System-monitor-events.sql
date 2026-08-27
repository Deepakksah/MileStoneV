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

-- Service Available Critical Event
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>2FE3A39D-7ACF-436C-9552-67FDF9978A69</Id>
  <Name>Service Available Critical</Name>
  <DisplayName referenceid="2AFFF902-8414-4417-B314-E8EA154E7DF4">Service Available Critical</DisplayName>
  <Description referenceid="A0069A2D-66F2-44BA-BA22-5E461F16B5E8">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>SystemDashboard</Type>
	<SubType>Server</SubType>
  	<Id></Id>
  	<Name/>
  </Generator>
  <Sources>
    <Source>
  	  <Type>GenericServer</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>True</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>2FE3A39D-7ACF-436C-9552-67FDF9978A69</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Servive Available Normal Event
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>6EF169EC-4B56-48E4-A8AA-65A07D0C35DC</Id>
  <Name>Service Available Normal</Name>
  <DisplayName referenceid="A5208A07-74C6-4447-BBBC-3540A1802C18">Service Available Normal</DisplayName>
  <Description referenceid="24510810-8E0E-4C10-B34E-5DF11EBE5E40">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>SystemDashboard</Type>
	<SubType>Server</SubType>
  	<Id></Id>
  	<Name/>
  </Generator>
  <Sources>
    <Source>
  	  <Type>GenericServer</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>True</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>6EF169EC-4B56-48E4-A8AA-65A07D0C35DC</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

GO