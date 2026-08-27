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

-- Marked data reference requested
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>3A82E9B3-9FFF-4BA1-9593-9927B0821076</Id>
  <Name>MarkedDataReferenceRequested</Name>
  <DisplayName referenceid="1D3AA755-B3ED-4AD4-B055-51C14090F5CE">Marked Data Reference Requested</DisplayName>
  <Description referenceid="78BB2A69-3147-44F2-B4EB-580548EA23D0">
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
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>3A82E9B3-9FFF-4BA1-9593-9927B0821076</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Recordings evidence locked
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>54DC549C-A007-47BD-8060-87BBAE9497B0</Id>
  <Name>RecordingsEvidenceLocked</Name>
  <DisplayName referenceid="E1CE547D-588D-405F-89C3-B6CFA175390B">Evidence Lock</DisplayName>
  <Description referenceid="B848E396-8480-408A-9A4E-B0E890848B35">
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
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>54DC549C-A007-47BD-8060-87BBAE9497B0</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Recordings evidence unlocked
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>3334B3E9-A135-497A-A0B9-9036533C2173</Id>
  <Name>RecordingsEvidenceUnlocked</Name>
  <DisplayName referenceid="0F885ABA-B385-483A-B12D-D35835FF0562">Evidence Unlock</DisplayName>
  <Description referenceid="0A98DC7A-9584-4896-8C28-1A2742FCC0AD">
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
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>3334B3E9-A135-497A-A0B9-9036533C2173</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Evidence lock changed
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>6EAB8BA5-6CAC-4636-BB2E-0A03875DF41A</Id>
  <Name>EvidenceLockChanged</Name>
  <DisplayName referenceid="8B4F61D2-EEE0-47C5-BF4D-56B3C8ED93E4">Evidence lock changed</DisplayName>
  <Description referenceid="74E05EC3-8390-4292-A9C3-D6B746BE0A34">
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
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>6EAB8BA5-6CAC-4636-BB2E-0A03875DF41A</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

GO

