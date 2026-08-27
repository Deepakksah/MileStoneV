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

-- DeviceCommunicationError
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>A334AF1C-4B4B-4957-9E5F-AB8CA07FEAB6</Id>
  <Name>CommunicationError</Name>
  <DisplayName referenceid="FF1E5727-8492-4496-A1B5-26508572773C">Communication Error</DisplayName>
  <Description referenceid="ef260c9f-fed1-4a42-b658-b7bbf111ebaa">
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
  <CounterEventId>A334AF1C-4B4B-4957-9E5F-AB8CA07FEAB6</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- DeviceCommunicationEstablished
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>DD3E6464-7DC0-405a-A92F-6150587563E8</Id>
  <Name>CommunicationStarted</Name>
  <DisplayName referenceid="90C6F34C-A520-4b17-94A3-3A190B3CEDDD">Communication Started</DisplayName>
  <Description referenceid="43e2304c-f523-49dd-9cc9-9595f4d50c1d">
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
  <CounterEventId>DD3E6464-7DC0-405a-A92F-6150587563E8</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- DeviceCommunicationStopped
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>0EE90664-2924-42A0-A816-4129D0ECABDC</Id>
  <Name>CommunicationStopped</Name>
  <DisplayName referenceid="A55048E2-074B-42c8-BAEE-3AE233FC2A2F">Communication Stopped</DisplayName>
  <Description referenceid="e36bab6f-e1e4-4969-8ac9-67ac225a2416">
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
  <CounterEventId>0EE90664-2924-42A0-A816-4129D0ECABDC</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- DeviceSettingsChanged
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>4C8BE5ED-80FA-43e7-BF5B-4E7570AD0025</Id>
  <Name>SettingsChanged</Name>
  <DisplayName referenceid="06B5CFCA-65DB-4d81-92DF-032B3EB931BC">Settings Changed</DisplayName>
  <Description referenceid="5c2db372-1c82-4966-8443-0bb49ada8fac">
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
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>4C8BE5ED-80FA-43e7-BF5B-4E7570AD0025</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- DeviceSettingsChangedError
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>87BE2A0D-C33B-4b5f-AE1F-AFA6770A2497</Id>
  <Name>SettingsChangedError</Name>
  <DisplayName referenceid="F107202C-ED13-43ef-A59C-4F2596D7E269">Settings Changed Error</DisplayName>
  <Description referenceid="6eda8f79-753d-4f3a-8a7a-9cc6ae84e1c5">
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
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>87BE2A0D-C33B-4b5f-AE1F-AFA6770A2497</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- DriverOverflowBegin
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>536AD730-05AE-42CD-B14C-07B07C293E79</Id>
  <Name>FeedOverflowBegin</Name>
  <DisplayName referenceid="01EC6C4A-5BC2-4b30-AE04-54227C738106">Feed Overflow Started</DisplayName>
  <Description referenceid="8eba9399-a224-49db-ad45-efb826e680f4">
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
  <CounterEventId>247FEF11-15DB-4C99-99DB-DE5475EAE443</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- DriverOverflowEnd
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>247FEF11-15DB-4C99-99DB-DE5475EAE443</Id>
  <Name>FeedOverflowStopped</Name>
  <DisplayName referenceid="38849FF8-735C-4607-94FC-D50BB48F49ED">Feed Overflow Stopped</DisplayName>
  <Description referenceid="85255f9b-901f-4352-a5ff-73c7a4e74776">
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
  <CounterEventId>536AD730-05AE-42CD-B14C-07B07C293E79</CounterEventId>
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
	<Source>
  	  <Type>Metadata</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>6EAB8BA5-6CAC-4636-BB2E-0A03875DF41A</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Images Received Event
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
	<Source>
  	  <Type>Metadata</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>C22C1456-9319-455F-A82C-07C8F87B0B50</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- LiveRequestStarted
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>EEDA47FF-4F3D-459e-8143-69896C7C74AD</Id>
  <Name>LiveClientFeedRequested</Name>
  <DisplayName referenceid="EAD68ADF-B912-404c-9746-1BD053757E4F">Live Client Feed Requested</DisplayName>
  <Description referenceid="d6bf3503-29f1-40b0-83af-ccecf40ac766">
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
  <CounterEventId>66BDD008-B856-44b6-B385-B1F6C3F76F1B</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- LiveRequestStopped
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>66BDD008-B856-44b6-B385-B1F6C3F76F1B</Id>
  <Name>LiveClientFeedTerminated</Name>
  <DisplayName referenceid="FAB2D652-21FC-45f1-8401-2E7A50513E31">Live Client Feed Terminated</DisplayName>
  <Description referenceid="463fdfff-cfc0-4ec5-9427-3c1b30317d84">
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
  <CounterEventId>EEDA47FF-4F3D-459e-8143-69896C7C74AD</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

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
	<Source>
  	  <Type>Metadata</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>3A82E9B3-9FFF-4BA1-9593-9927B0821076</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Recording Started
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>4577F552-765A-438c-BC7D-E5FF1F754BC3</Id>
  <Name>RecordingStarted</Name>
  <DisplayName referenceid="B9FF6C2B-2200-4ffc-BADD-4B7D97D7CB36">Recording Started</DisplayName>
  <Description referenceid="49a29ac2-1ea7-4726-9d4e-81f3b0c1a573">
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
  <CounterEventId>79A94F89-92DE-4fca-8A43-5561D407423D</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Recording Stopped
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>79A94F89-92DE-4fca-8A43-5561D407423D</Id>
  <Name>RecordingStopped</Name>
  <DisplayName referenceid="2AD39E8C-E11F-4202-9AB5-9D92E53A9F1B">Recording Stopped</DisplayName>
  <Description referenceid="5e3f89c9-2d29-4dab-967d-5d2ab1b85395">
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
  <CounterEventId>4577F552-765A-438c-BC7D-E5FF1F754BC3</CounterEventId>
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
		<Source>
  	  <Type>Metadata</Type>
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
	<Source>
  	  <Type>Metadata</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>3334B3E9-A135-497A-A0B9-9036533C2173</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime
GO

