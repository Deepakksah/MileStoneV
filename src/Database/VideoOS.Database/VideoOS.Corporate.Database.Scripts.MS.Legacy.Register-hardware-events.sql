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

-- HardwareCommunicationError
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>6BAAD64B-C395-4F52-B6A3-DD1B64AA2F0F</Id>
  <Name>CommunicationErrorHardware</Name>
  <DisplayName referenceid="2EB77A64-6E48-4324-B704-E92D35CAA850">Communication Error (Hardware)</DisplayName>
  <Description referenceid="FEC2475E-FBF6-4978-A640-FA0E9029603E">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>DriverHardware</Type>
	<SubType/>
  	<Id/>
  	<Name/>
  </Generator>
  <Sources>
    <Source>
  	  <Type>Hardware</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>6BAAD64B-C395-4F52-B6A3-DD1B64AA2F0F</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime


-- HardwareCommunicationEstablished
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>0553C396-5E16-4C22-B3D1-F548E42DFBB4</Id>
  <Name>CommunicationStartedHardware</Name>
  <DisplayName referenceid="BB7DF246-2F5B-432F-9065-EF9F6D4BCE48">Communication Started (Hardware)</DisplayName>
  <Description referenceid="8612D3CE-34CA-49FE-95FE-76F42165BF49">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>DriverHardware</Type>
	<SubType/>
  	<Id/>
  	<Name/>
  </Generator>
  <Sources>
    <Source>
  	  <Type>Hardware</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>63EA1F06-5A83-4F39-9FAB-49959FDE7B66</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime


-- HardwareCommunicationStopped
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>63EA1F06-5A83-4F39-9FAB-49959FDE7B66</Id>
  <Name>CommunicationStoppedHardware</Name>
  <DisplayName referenceid="5FD24F5E-4808-4E55-89BC-DFC8747C487C">Communication Stopped (Hardware)</DisplayName>
  <Description referenceid="869C4511-D2A1-48F9-BE9A-C95279EF41D4">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>DriverHardware</Type>
	<SubType/>
  	<Id/>
  	<Name/>
  </Generator>
  <Sources>
    <Source>
  	  <Type>Hardware</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>0553C396-5E16-4C22-B3D1-F548E42DFBB4</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

GO

