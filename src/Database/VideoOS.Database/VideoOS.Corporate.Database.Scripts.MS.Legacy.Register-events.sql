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

-- RelativeTimerEvent
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>6A00FD9A-A7F1-446C-ABC2-29639CECD595</Id>
  <Name>RelativeTimerEvent</Name>
  <DisplayName referenceid="F8271FD6-B9A8-4b70-AB5F-3D8B125AB1A4">Relative Timer</DisplayName>
  <Description referenceid="2b54937b-172c-41f8-9a3e-0deb09de445c">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>Timer</Type>
	<SubType/>
  	<Id/>
  	<Name/>
  </Generator>
  <Sources>
    <Source>
	  <Type>Timer</Type>
	  <Filters>
	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>True</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>6A00FD9A-A7F1-446C-ABC2-29639CECD595</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime


-- PeriodicTimerEvent
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>F07AC0E3-9662-4781-9176-EC9E488AA494</Id>
  <Name>PeriodicTimerEvent</Name>
  <DisplayName referenceid="575F09A1-7525-496f-9CF3-F69F7BFA21D6">Periodic Timer</DisplayName>
  <Description referenceid="ae8d0ba2-1e46-403d-bffb-47c1d27eb98a">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>Timer</Type>
	<SubType/>
  	<Id/>
  	<Name/>
  </Generator>
  <Sources>
    <Source>
  	  <Type>Timer</Type>
   	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>True</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>F07AC0E3-9662-4781-9176-EC9E488AA494</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime


-- DatabaseDiskFullEvent
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>A4935AA3-C4B6-42a3-B4C5-6D4C20B043CF</Id>
  <Name>DatabaseDiskFullEvent</Name>
  <DisplayName referenceid="71B9D7A5-2187-4e49-AA73-C062A9B53A9F">Database Disk Full</DisplayName>
  <Description referenceid="51d9e3e2-5f62-49f4-a7f1-0fb84517794d">
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
  <CounterEventId>A4935AA3-C4B6-42a3-B4C5-6D4C20B043CF</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime


-- DatabaseRepairEvent
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>B6C4B695-7404-414d-A229-33BF83FFE255</Id>
  <Name>DatabaseRepairEvent</Name>
  <DisplayName referenceid="20857682-D7A8-4786-BB68-6CB8116664A0">Database Repair</DisplayName>
  <Description referenceid="ba07e116-5347-4b35-b0d7-13220e7c3b3c">
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
  <CounterEventId>B6C4B695-7404-414d-A229-33BF83FFE255</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime


-- DatabaseDiskUnavailable
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>29986AA1-924B-4c8d-AAC7-A876883DC82C</Id>
  <Name>DatabaseDiskUnavailable</Name>
  <DisplayName referenceid="92258234-DB1F-4da1-98A1-D6EDB9F72BCD">Database Storage Area Unavailable</DisplayName>
  <Description referenceid="CABA89A0-D83C-4696-92FE-9D3A21CD785F">
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
  <CounterEventId>29986AA1-924B-4c8d-AAC7-A876883DC82C</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime


-- DatabaseDiskAvailable
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>FAB03C4D-0589-48c8-9328-E28B76C95AF0</Id>
  <Name>DatabaseDiskAvailable</Name>
  <DisplayName referenceid="2E143FA2-500E-490d-9D31-64EE776F2DB0">Database Storage Area Available</DisplayName>
  <Description referenceid="FB50BC76-7DAC-4942-B536-6913BBF57810">
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
  <CounterEventId>FAB03C4D-0589-48c8-9328-E28B76C95AF0</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime


-- ArchiveDiskUnavailable
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>72C29AAE-DDB2-46df-B6FD-7F728A85F292</Id>
  <Name>ArchiveDiskUnavailable</Name>
  <DisplayName referenceid="469AEF62-790F-47ea-8BAB-7BBBE1A30BD7">Archive Unavailable</DisplayName>
  <Description referenceid="1E39BAEC-B585-45ae-904F-F974C10F933C">
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
  <CounterEventId>72C29AAE-DDB2-46df-B6FD-7F728A85F292</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime


-- ArchiveDiskAvailable
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>B7F0003B-1392-450e-B6CC-9BBE6E7DF065</Id>
  <Name>ArchiveDiskAvailable</Name>
  <DisplayName referenceid="1CE32A5C-07E9-4302-BCE0-89FE8CCD93FE">Archive Available</DisplayName>
  <Description referenceid="A4486616-33BA-4bb3-ADE6-BB6E1A25823D">
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
  <CounterEventId>B7F0003B-1392-450e-B6CC-9BBE6E7DF065</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime


-- Motion Start
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>6EB95DD6-7CCC-4BCE-99F8-AF0D0B582D77</Id>
  <Name>MotionStarted</Name>
  <DisplayName referenceid="2D2ED68A-2639-47fb-9473-7EB91D003E98">Motion Started</DisplayName>
  <Description referenceid="6d7300a4-4a92-45e2-8ee0-5f2d8b931a09">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>RecorderPlugin</Type>
	<SubType/>
  	<Id>F020C881-72BF-4014-8504-2B4A5A658F1A</Id>
  	<Name>Motion Detection Plugin</Name>
  </Generator>
  <Sources>
    <Source>
  	  <Type>Camera</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>False</BuiltIn>
  <CounterEventId>6F55A7A7-D21C-4629-AC18-AF1975E395A2</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime


-- Motion End
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>6F55A7A7-D21C-4629-AC18-AF1975E395A2</Id>
  <Name>MotionStopped</Name>
  <DisplayName referenceid="6E0554C4-DD35-49aa-A194-269DCF786338">Motion Stopped</DisplayName>
  <Description referenceid="86cfea68-1f07-41dd-a4ca-b744dfc5ba37">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>RecorderPlugin</Type>
	<SubType/>
  	<Id>F020C881-72BF-4014-8504-2B4A5A658F1A</Id>
  	<Name>Motion Detection Plugin</Name>
  </Generator>
  <Sources>
    <Source>
  	  <Type>Camera</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>False</BuiltIn>
  <CounterEventId>6EB95DD6-7CCC-4BCE-99F8-AF0D0B582D77</CounterEventId>
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
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>4577F552-765A-438c-BC7D-E5FF1F754BC3</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime


-- Bookmark reference requested
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>7CFC6709-2C16-405f-9182-0BDB6F9F7E43</Id>
  <Name>BookmarkReferenceRequested</Name>
  <DisplayName referenceid="66D28507-A42C-4055-9B6C-6FC6CC6487D4">Bookmark Reference Requested</DisplayName>
  <Description referenceid="AB94787C-B903-4b00-AEF0-2D6E44ED6C2B">
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
  <CounterEventId>7CFC6709-2C16-405f-9182-0BDB6F9F7E43</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime


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
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>536AD730-05AE-42CD-B14C-07B07C293E79</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime


-- InputActivated
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>836CA458-A833-4742-8EE0-64B2380984BD</Id>
  <Name>InputActivated</Name>
  <DisplayName referenceid="B23DC965-8F5E-40a3-A6BC-4613DABAF394">Input Activated</DisplayName>
  <Description referenceid="74150d10-caff-4cea-af8b-87af933a823f">
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
  	  <Type>Input</Type>
  	 <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>8666E3DC-57A7-4F38-9611-51D774EE7358</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime


-- InputChanged
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>FDA121B3-2070-4F31-A086-9F74E16580C0</Id>
  <Name>InputChanged</Name>
  <DisplayName referenceid="617DA0EB-D3B1-42d0-B57B-39B6645626BE">Input Changed</DisplayName>
  <Description referenceid="99dfec57-120d-4326-9873-def42bd6ecb8">
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
  	  <Type>Input</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>FDA121B3-2070-4F31-A086-9F74E16580C0</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime


-- InputDeactivated
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>8666E3DC-57A7-4F38-9611-51D774EE7358</Id>
  <Name>InputDeactivated</Name>
  <DisplayName referenceid="5120B8E2-D2AB-4601-8F81-2F186B63B29F">Input Deactivated</DisplayName>
  <Description referenceid="13656c6e-d1a8-422d-9688-4145d5aee741">
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
  	  <Type>Input</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>836CA458-A833-4742-8EE0-64B2380984BD</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime


-- OutputActivated
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>7A78F5BB-D8C3-4997-89B7-CAE72713B7DB</Id>
  <Name>OutputActivated</Name>
  <DisplayName referenceid="1AE32471-76DF-479f-917C-3FDDD7DF6396">Output Activated</DisplayName>
  <Description referenceid="ec3e4681-f286-4dd0-96c0-3e7125073d09">
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
  	  <Type>Output</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>35742498-BCC5-4f0a-9800-827C9388D1CD</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime


-- OutputChanged
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>672F9F80-55FE-496f-BEE4-6C870BAB5064</Id>
  <Name>OutputChanged</Name>
  <DisplayName referenceid="9DB74081-B292-4014-91A0-33235DFB1D0E">Output Changed</DisplayName>
  <Description referenceid="d1793220-dc52-4f84-a88f-2734210e0988">
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
  	  <Type>Output</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>672F9F80-55FE-496f-BEE4-6C870BAB5064</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime


-- OutputDeactivated
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>35742498-BCC5-4f0a-9800-827C9388D1CD</Id>
  <Name>OutputDeactivated</Name>
  <DisplayName referenceid="3CB9DE24-AE40-4075-882C-97BE574C87AD">Output Deactivated</DisplayName>
  <Description referenceid="0249c067-943e-443c-9a57-126f9aaed802">
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
  	  <Type>Output</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>7A78F5BB-D8C3-4997-89B7-CAE72713B7DB</CounterEventId>
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
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>EEDA47FF-4F3D-459e-8143-69896C7C74AD</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime


-- PTZManualSessionStarted
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>2EEB582A-1506-4d73-96C9-AD356ACF85FB</Id>
  <Name>PTZManualSessionStarted</Name>
  <DisplayName referenceid="5590E50A-840D-4ff5-BA1A-B1B362429B1E">PTZ Manual Session Started</DisplayName>
  <Description referenceid="ad61fd59-1520-404f-91e6-a6441dca8ffc">
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
  	    <Filter>Ptz</Filter>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>EED47283-694F-4683-9E4D-18E7D93CF796</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime


-- PTZManualSessionStopped
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>EED47283-694F-4683-9E4D-18E7D93CF796</Id>
  <Name>PTZManualSessionStopped</Name>
  <DisplayName referenceid="09213E9B-055F-43f3-82A8-F92EDFD52EF3">PTZ Manual Session Stopped</DisplayName>
  <Description referenceid="cea4c564-d214-4893-81e2-e93a8834af72">
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
  	    <Filter>Ptz</Filter>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>2EEB582A-1506-4d73-96C9-AD356ACF85FB</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- TargetItemActivated
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>B0109E36-5CED-4E3F-B637-4E645A823DBF</Id>
  <Name>TargetItemActivated</Name>
  <DisplayName referenceid="1E9BC87C-E554-4f4e-853A-40D183F6E848">Target Item Activated</DisplayName>
  <Description referenceid="9593e4c9-b7df-4451-bada-e830f145fe6c">
    Internal event. No rules should be based on the occurrence of this event
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>System</Type>
	<SubType/>
  	<Id/>
  	<Name/>
  </Generator>
  <Sources>
    <Source>
  	  <Type>System</Type>
    </Source>
  </Sources>
  <OccursGlobally>True</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>B02CE8FA-21DC-469E-8C07-BD9616289694</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- TargetItemDeactivated
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>B02CE8FA-21DC-469E-8C07-BD9616289694</Id>
  <Name>TargetItemDeactivated</Name>
  <DisplayName referenceid="968D9A5C-0A4B-4711-943E-92F749B835B7">Target Item Deactivated</DisplayName>
  <Description referenceid="eca2ab03-87f7-4434-b39e-1b5d5f2073ea">
    Internal event. No rules should be based on the occurrence of this event
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>System</Type>
	<SubType/>
  	<Id/>
  	<Name/>
  </Generator>
  <Sources>
    <Source>
  	  <Type>System</Type>
    </Source>
  </Sources>
  <OccursGlobally>True</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>B0109E36-5CED-4E3F-B637-4E645A823DBF</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- DeviceGroupMemberAdded
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>A01CD5DA-EF57-476B-814B-344F776DA600</Id>
  <Name>DeviceGroupMemberAdded</Name>
  <DisplayName referenceid="43A6CE72-F645-4596-B8A9-9505F867A3C1">Device Group Member Added</DisplayName>
  <Description referenceid="b3ae4344-a56e-4a0b-ae43-89d8a4f39099">
    Internal event. No rules should be based on the occurrence of this event
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>System</Type>
	<SubType/>
  	<Id/>
  	<Name/>
  </Generator>
  <Sources>
    <Source>
  	  <Type>System</Type>
    </Source>
  </Sources>
  <OccursGlobally>True</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>A02E59BD-45D3-4DC7-901E-8B57C4DA31C9</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- DeviceGroupMemberDeleted
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>A02E59BD-45D3-4DC7-901E-8B57C4DA31C9</Id>
  <Name>DeviceGroupMemberDeleted</Name>
  <DisplayName referenceid="8CAC7BD3-5C3F-4409-A926-DB8723305857">Device Group Member Deleted</DisplayName>
  <Description referenceid="6c0d0c25-8200-4412-b668-0a5342159603">
    Internal event. No rules should be based on the occurrence of this event
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>System</Type>
	<SubType/>
  	<Id/>
  	<Name/>
  </Generator>
  <Sources>
    <Source>
  	  <Type>System</Type>
    </Source>
  </Sources>
  <OccursGlobally>True</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>A01CD5DA-EF57-476B-814B-344F776DA600</CounterEventId>
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
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>C22C1456-9319-455F-A82C-07C8F87B0B50</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- FailoverStarted
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
	<SubType/>
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
  <BuiltIn>True</BuiltIn>
  <CounterEventId>C7B9227A-8E11-451B-BC9C-8EA659731012</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- FailoverStopped
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
	<SubType/>
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
  <BuiltIn>True</BuiltIn>
  <CounterEventId>F951B1F0-2FED-48F7-88D3-49EB5999C923</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

GO

