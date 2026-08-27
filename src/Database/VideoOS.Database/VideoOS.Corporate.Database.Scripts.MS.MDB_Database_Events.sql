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


-- DatabaseArchivingSessionStarted
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>9EAF1770-42ED-4C5B-A06F-95722721883D</Id>
  <Name>DatabaseArchivingSessionStarted</Name>
  <DisplayName referenceid="9FA9ADF8-5523-419E-ADE6-03DEDD3AE99E">Database archiving session started</DisplayName>
  <Description referenceid="87AE6B10-B6D9-4B4F-B83E-9295CF9B3CD3">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>Recorder</Type>
	<SubType/>
  	<Id/>
  	<Name/>
  </Generator>
  <Sources>
    <Source>
  	  <Type>Database</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>B73F8241-3320-45CF-89F3-80E4A090E487</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- DatabaseArchivingSessionEnded
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>B73F8241-3320-45CF-89F3-80E4A090E487</Id>
  <Name>DatabaseArchivingSessionEnded</Name>
  <DisplayName referenceid="0026FEAC-5851-46A2-A0FF-F2E4AC325926">Database archiving session ended</DisplayName>
  <Description referenceid="701B110F-9153-445D-9395-F25C5ECDA33D">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>Recorder</Type>
	<SubType/>
  	<Id/>
  	<Name/>
  </Generator>
  <Sources>
    <Source>
  	  <Type>Database</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>9EAF1770-42ED-4C5B-A06F-95722721883D</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- DatabaseRepairSessionStarted
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>54D7CE97-2343-40D5-92D2-350F5DFB3029</Id>
  <Name>DatabaseRepairSessionStarted</Name>
  <DisplayName referenceid="BB5A0224-4759-496E-8B15-E847FB5DC1C0">Database repair session started</DisplayName>
  <Description referenceid="B812CE5A-3AC0-436F-B6C9-BC3AC3B9E666">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>Recorder</Type>
	<SubType/>
  	<Id/>
  	<Name/>
  </Generator>
  <Sources>
    <Source>
  	  <Type>Database</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>2D7FBAD3-54DB-4155-B471-3BA9ECB5CBAB</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- DatabaseRepairSessionEnded
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>2D7FBAD3-54DB-4155-B471-3BA9ECB5CBAB</Id>
  <Name>DatabaseRepairSessionEnded</Name>
  <DisplayName referenceid="EE502117-37BC-46F0-BADF-E08273C3ED9B">Database repair session ended</DisplayName>
  <Description referenceid="F5DB5B4E-AAA1-4F72-AFE1-A9367B3CE9FC">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>Recorder</Type>
	<SubType/>
  	<Id/>
  	<Name/>
  </Generator>
  <Sources>
    <Source>
  	  <Type>Database</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>54D7CE97-2343-40D5-92D2-350F5DFB3029</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

GO