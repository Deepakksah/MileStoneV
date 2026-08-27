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

-- CPU Usage Undefined Event
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>D003BAE9-0E12-413F-A57B-673A83C56D1A</Id>
  <Name>CPU Usage Undefined</Name>
  <DisplayName referenceid="5BF8CF4E-B5D1-49A8-9F78-8E98021F1398">CPU Usage Undefined</DisplayName>
  <Description referenceid="178C9B5E-7516-42FF-8A92-5075C44968D7">
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
  <CounterEventId>D003BAE9-0E12-413F-A57B-673A83C56D1A</CounterEventId>
  <StateGroupId>8C958201-AA02-40CF-A643-FA7F91D0BED2</StateGroupId>
  <State>Undefined</State>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Memory Usage Undefined Event
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>2C3F82DC-1D15-4A60-9EAB-EB517592861D</Id>
  <Name>Memory Usage Undefined</Name>
  <DisplayName referenceid="A415A0D6-55F0-4D5E-98EE-9BF31C26A261">Memory Usage Undefined</DisplayName>
  <Description referenceid="51D362EF-FD94-4B96-A115-0BEFF8D9A7E4">
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
  <CounterEventId>2C3F82DC-1D15-4A60-9EAB-EB517592861D</CounterEventId>
  <StateGroupId>FEB52F64-AD52-41F4-AC13-16576460DB7D</StateGroupId>
  <State>Undefined</State>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Used Space Undefined Event
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>0DD65A18-DFD1-493A-BA6A-F15D37FFE037</Id>
  <Name>Used Space Undefined</Name>
  <DisplayName referenceid="D42F4BC8-D6F6-4A0F-8987-5D96DF110849">Used Space Undefined</DisplayName>
  <Description referenceid="65A677F5-3FD7-474C-9EB1-3802EB0A5194">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>SystemDashboard</Type>
	<SubType>Device</SubType>
  	<Id></Id>
  	<Name/>
  </Generator>
  <Sources>
    <Source>
  	  <Type>Camera</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>True</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>0DD65A18-DFD1-493A-BA6A-F15D37FFE037</CounterEventId>
  <StateGroupId>72614B0B-62BF-46C2-B74F-9CDE0EEA87EA</StateGroupId>
  <State>Undefined</State>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Live FPS Undefined Event
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>FE3D0779-1F51-486E-AAB3-1D12A4FCB23C</Id>
  <Name>Live FPS Undefined</Name>
  <DisplayName referenceid="8721E869-E0E3-4839-878E-88186139B30E">Live FPS Undefined</DisplayName>
  <Description referenceid="D393F610-FBDC-4219-A46F-E18EB5FBF5AA">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>SystemDashboard</Type>
	<SubType>Device</SubType>
  	<Id></Id>
  	<Name/>
  </Generator>
  <Sources>
    <Source>
  	  <Type>Camera</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>True</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>FE3D0779-1F51-486E-AAB3-1D12A4FCB23C</CounterEventId>
  <StateGroupId>8B9B8FA4-3510-4725-8BD0-01AB4E8A71C1</StateGroupId>
  <State>Undefined</State>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Recording FPS Undefined Event
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>C39716BF-9501-4FC4-98D6-CD4CA1EA11BD</Id>
  <Name>Recording FPS Undefined</Name>
  <DisplayName referenceid="CDD0948E-4D6D-49F9-9568-DE11655FA597">Recording FPS Undefined</DisplayName>
  <Description referenceid="4BF44EAC-196D-4DD6-8292-40F57BEBEC07">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>SystemDashboard</Type>
	<SubType>Device</SubType>
  	<Id></Id>
  	<Name/>
  </Generator>
  <Sources>
    <Source>
  	  <Type>Camera</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>True</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>C39716BF-9501-4FC4-98D6-CD4CA1EA11BD</CounterEventId>
  <StateGroupId>60217956-BEA0-4C7D-ADD5-6485C2176FFD</StateGroupId>
  <State>Undefined</State>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Free Space Undefined Event
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>07EEFA87-32B9-49B2-883F-05B75B11877A</Id>
  <Name>Free Space Undefined</Name>
  <DisplayName referenceid="2A94FF8C-E19C-4C0B-8AF8-80CA0FB131AC">Free Space Undefined</DisplayName>
  <Description referenceid="A8FF650E-F00B-432A-906A-5E43589B69B1">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>SystemDashboard</Type>
	<SubType>Disk</SubType>
  	<Id></Id>
  	<Name/>
  </Generator>
  <Sources>
    <Source>
  	  <Type>GenericSever</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>True</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>07EEFA87-32B9-49B2-883F-05B75B11877A</CounterEventId>
  <StateGroupId>F4261175-44D3-4705-B653-C33C8F0CA70E</StateGroupId>
  <State>Undefined</State>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Retention time Undefined Event
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>5AA16452-3CB3-4986-BB8C-87BF77C2AC11</Id>
  <Name>Retention time Undefined</Name>
  <DisplayName referenceid="D1FE4031-9782-4945-8C5A-B0A7D852B22D">Retention time Undefined</DisplayName>
  <Description referenceid="F775600F-D10A-4786-8DD0-0017A1E029DD">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>SystemDashboard</Type>
	<SubType>Storage</SubType>
  	<Id></Id>
  	<Name/>
  </Generator>
  <Sources>
    <Source>
  	  <Type>GenericSever</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>True</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>5AA16452-3CB3-4986-BB8C-87BF77C2AC11</CounterEventId>
  <StateGroupId>9D48B11A-0690-409F-8C7A-8FBC8812F657</StateGroupId>
  <State>Undefined</State>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- GPU Decoding Undefined Event
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>E4E414A7-2788-4F76-B2D9-A67B73929453</Id>
  <Name>GPU Decoding Undefined</Name>
  <DisplayName referenceid="424F8AD9-4C2B-4327-9F56-883C8A3FA3DF">GPU Decoding Undefined</DisplayName>
  <Description referenceid="5879AA77-2139-470D-82D0-263D6C8A77BC">
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
  <CounterEventId>E4E414A7-2788-4F76-B2D9-A67B73929453</CounterEventId>
  <StateGroupId>A4F206A2-EAF1-4BC3-9C35-21E9DF0F0FC8</StateGroupId>
  <State>Undefined</State>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- GPU Rendering Undefined Event
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>44ECFD80-C265-4545-896A-BAFC0403FA94</Id>
  <Name>GPU Rendering Undefined</Name>
  <DisplayName referenceid="96F6DBAB-7637-4388-87A7-E3221DBFC699">GPU Rendering Undefined</DisplayName>
  <Description referenceid="E571FB36-2B0B-4AC4-A3B1-B492A29AD5D9">
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
  <CounterEventId>44ECFD80-C265-4545-896A-BAFC0403FA94</CounterEventId>
  <StateGroupId>120B550D-3DBD-46A1-94B9-420CAB8E39E5</StateGroupId>
  <State>Undefined</State>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- GPU Memory Undefined Event
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>21704364-3869-4A2C-AED9-C5C99FE6FCAB</Id>
  <Name>GPU Memory Undefined</Name>
  <DisplayName referenceid="6F051BDF-74E4-4F7C-A16B-4558C4DB35DF">GPU Memory Undefined</DisplayName>
  <Description referenceid="11B77DAE-64A6-42EC-9CD3-5ADA25583E03">
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
  <CounterEventId>21704364-3869-4A2C-AED9-C5C99FE6FCAB</CounterEventId>
  <StateGroupId>8710ED5D-1FB8-4551-81FB-49B269267D5D</StateGroupId>
  <State>Undefined</State>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Service Available Undefined Event
SET @EventDefinitionXml = 
N'<EventType>
  <Id>1254477F-6650-491B-A204-DC463FF75F4E</Id>
  <Name>Service Available Undefined</Name>
  <DisplayName referenceid="7180FF4D-6FCD-4B52-B917-D62CFC83BADD">Service Available Undefined</DisplayName>
  <Description referenceid="4174EB26-7C8B-4255-A35F-37550215C171" />
  <Parameters />
  <Generator>
    <Type>SystemDashboard</Type>
    <SubType>Server</SubType>
    <Id />
    <Name />
    <GroupId />
    <GroupName />
  </Generator>
  <Sources>
    <Source>
      <Type>GenericServer</Type>
      <Filters />
    </Source>
  </Sources>
  <OccursGlobally>True</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>1254477F-6650-491B-A204-DC463FF75F4E</CounterEventId>
  <StateGroupId>C1EB0D2B-1903-45F6-B0C8-3F4CD1879B2A</StateGroupId>
  <State>Undefined</State>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

GO