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
  <Id>1DD4F1AB-3CDA-4F5C-93D7-B0C78D2BC6AD</Id>
  <Name>DatabaseDeleteBeforeSetRetentionTimeEvent</Name>
  <DisplayName referenceid="81A2A3C6-8016-41BD-AA80-A2E948F3CC7A">Database Deleting Recordings Before Set Retention Time</DisplayName>
  <Description referenceid="5BA24387-F8FF-4321-9A45-1DC048324BD7">
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
  <CounterEventId>1DD4F1AB-3CDA-4F5C-93D7-B0C78D2BC6AD</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- DatabaseDeleteBeforeSetRetentionSizeEvent
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>023D4D8D-5848-43CF-8F94-4F7F230966EF</Id>
  <Name>DatabaseDeleteBeforeSetRetentionSizeEvent</Name>
  <DisplayName referenceid="7908EE1E-4BED-4EA2-9D35-54B1685C7BF2">Database Deleting Recordings Before Set Retention Size</DisplayName>
  <Description referenceid="D1C412E0-1400-4E62-B47D-093E2125615E">
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
  <CounterEventId>023D4D8D-5848-43CF-8F94-4F7F230966EF</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- DatabaseDiskFullAutoArchiveEvent
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>59ED52FD-A07D-4EDD-8BD6-24D0420CEA34</Id>
  <Name>DatabaseDiskFullAutoArchiveEvent</Name>
  <DisplayName referenceid="B7D8BCDA-12E3-41EC-8F69-EE8F3808E21D">Database Disk Full - Auto Archiving</DisplayName>
  <Description referenceid="E66F2FB4-3C6A-455D-AF6F-DD15930C207B">
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
  <CounterEventId>59ED52FD-A07D-4EDD-8BD6-24D0420CEA34</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

GO

