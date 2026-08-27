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

-- CPU Usage Critical Event
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>1B36A57E-B656-402E-81E2-E4B873201F06</Id>
  <Name>CPU Usage Critical</Name>
  <DisplayName referenceid="637D0A40-9E99-449E-BC9F-A55C65DF1A0B">CPU Usage Critical</DisplayName>
  <Description referenceid="74F2D3CB-8E64-40D4-8DAB-B30BBE4DB485">
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
  <CounterEventId>1B36A57E-B656-402E-81E2-E4B873201F06</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- CPU Usage Warning Event
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>8E98B3E2-D0EA-4608-AD4D-66FCBB422A62</Id>
  <Name>CPU Usage Warning</Name>
  <DisplayName referenceid="75EB3AC2-7FB5-4257-BD52-34C47AF504DA">CPU Usage Warning</DisplayName>
  <Description referenceid="8816753C-F0F7-4D2E-A4EB-02F0DDF1F6DD">
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
  <CounterEventId>8E98B3E2-D0EA-4608-AD4D-66FCBB422A62</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- CPU Usage Normal Event
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>727A8D92-75FD-424F-A2D7-EDDBBBC84305</Id>
  <Name>CPU Usage Normal</Name>
  <DisplayName referenceid="37043E7D-46DE-4F53-9232-FAD840CD418C">CPU Usage Normal</DisplayName>
  <Description referenceid="1AE1B5E0-3971-4963-ACBC-E9079CB1169A">
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
  <CounterEventId>727A8D92-75FD-424F-A2D7-EDDBBBC84305</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Memory Usage Critical Event
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>363741CF-4D7A-426D-9FE2-A201C4DB51C0</Id>
  <Name>Memory Usage Critical</Name>
  <DisplayName referenceid="E83B2B12-AD2C-494B-87FE-6714BAD3B9B8">Memory Usage Critical</DisplayName>
  <Description referenceid="CA39F828-64E2-41EE-8A49-FC08789686A7">
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
  <CounterEventId>363741CF-4D7A-426D-9FE2-A201C4DB51C0</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Memory Usage Warning Event
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>0C5B3A2D-F60F-4CF0-A99C-1D62DCAEE5B4</Id>
  <Name>Memory Usage Warning</Name>
  <DisplayName referenceid="39F5D8B5-EFBA-409E-8066-35517002C366">Memory Usage Warning</DisplayName>
  <Description referenceid="CA2437DB-871D-4D6F-8FFE-013337B141BF">
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
  <CounterEventId>0C5B3A2D-F60F-4CF0-A99C-1D62DCAEE5B4</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Memory Usage Normal Event
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>2457EF7A-5742-4550-9948-AD5CC51D0645</Id>
  <Name>Memory Usage Normal</Name>
  <DisplayName referenceid="2B46ADD1-930B-48E7-9B69-FADD2E195CF2">Memory Usage Normal</DisplayName>
  <Description referenceid="7669AE3D-FC1E-4D3B-88B1-6A5D8E7521C3">
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
  <CounterEventId>2457EF7A-5742-4550-9948-AD5CC51D0645</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Used Space Critical Event
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>766CF688-F3E2-4D2F-8D08-012193783739</Id>
  <Name>Used Space Critical</Name>
  <DisplayName referenceid="EA768EFB-03B4-4BCE-A6EA-2D2CB77A2B95">Used Space Critical</DisplayName>
  <Description referenceid="98803BA5-350F-4236-ABFB-D2664380E4B5">
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
  <CounterEventId>766CF688-F3E2-4D2F-8D08-012193783739</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Used Space Warning Event
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>E68C0857-8D86-4190-B528-96AF21BE0F62</Id>
  <Name>Used Space Warning</Name>
  <DisplayName referenceid="230F4FAC-9279-4E4E-9D56-FDAA5B7B784B">Used Space Warning</DisplayName>
  <Description referenceid="50B4846D-2F48-4669-BA45-FE857E7A4F75">
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
  <CounterEventId>E68C0857-8D86-4190-B528-96AF21BE0F62</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Used Space Normal Event
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>C8E0A296-43C7-48CC-825D-74B90B6CBDE6</Id>
  <Name>Used Space Normal</Name>
  <DisplayName referenceid="E44A9C7E-80F9-435E-BC91-8A5EDD1965D1">Used Space Normal</DisplayName>
  <Description referenceid="0374DDF8-A684-4CBB-A0E3-34A13F8C7007">
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
  <CounterEventId>C8E0A296-43C7-48CC-825D-74B90B6CBDE6</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Live FPS Critical Event
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>A4CC82CE-B91D-4CC3-96B0-75B527C5F8F4</Id>
  <Name>Live FPS Critical</Name>
  <DisplayName referenceid="DE63A14E-C677-448E-8F3E-B2BB12A16692">Live FPS Critical</DisplayName>
  <Description referenceid="8B19D8F5-46AA-4E2C-8A1B-ED2648275BC9">
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
  <CounterEventId>A4CC82CE-B91D-4CC3-96B0-75B527C5F8F4</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Live FPS Warning Event
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>54CDA1C8-FB7F-4DAB-B506-B0AD707F2B86</Id>
  <Name>Live FPS Warning</Name>
  <DisplayName referenceid="B0EB72AE-255E-493D-9207-20F3A40FBBEC">Live FPS Warning</DisplayName>
  <Description referenceid="F1AC70AD-712F-479F-B2FE-FF9B0ACA6F49">
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
  <CounterEventId>54CDA1C8-FB7F-4DAB-B506-B0AD707F2B86</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Live FPS Normal Event
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>E14E849F-7355-4D03-97FE-77C292FE01C7</Id>
  <Name>Live FPS Normal</Name>
  <DisplayName referenceid="0D2290F0-6963-4AE6-B43D-10ED7E1D587C">Live FPS Normal</DisplayName>
  <Description referenceid="CC9BE427-958A-4CA6-BFED-FB1EF58F6705">
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
  <CounterEventId>E14E849F-7355-4D03-97FE-77C292FE01C7</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Recording FPS Critical Event
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>7A60A969-E2B9-4F47-84E7-A2F6D07C56FD</Id>
  <Name>Recording FPS Critical</Name>
  <DisplayName referenceid="8BF52CB9-4844-4DDB-8658-72F96823F5F7">Recording FPS Critical</DisplayName>
  <Description referenceid="EFF51435-8060-45C8-977C-8A293C0B0320">
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
  <CounterEventId>7A60A969-E2B9-4F47-84E7-A2F6D07C56FD</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Recording FPS Warning Event
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>C93E7128-BD54-43AA-9FB2-971E7926D14B</Id>
  <Name>Recording FPS Warning</Name>
  <DisplayName referenceid="A7ED35F6-1F7F-4269-8865-B2E343652BAA">Recording FPS Warning</DisplayName>
  <Description referenceid="51F7BDA2-D7F1-4A78-98D3-00169F23E7A4">
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
  <CounterEventId>C93E7128-BD54-43AA-9FB2-971E7926D14B</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Recording FPS Normal Event
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>839754E6-82AF-44FC-9E2F-437413D602D6</Id>
  <Name>Recording FPS Normal</Name>
  <DisplayName referenceid="C94C8C7A-89A9-4826-B83C-81EBE2F37C4A">Recording FPS Normal</DisplayName>
  <Description referenceid="71005CDF-728F-4869-A5EB-4251DB69B468">
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
  <CounterEventId>839754E6-82AF-44FC-9E2F-437413D602D6</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Free Space Critical Event
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>F80AF822-6A89-4C2F-AB38-B55D0D2CD13A</Id>
  <Name>Free Space Critical</Name>
  <DisplayName referenceid="9A6F8EFC-4215-4073-94CC-C297707E458C">Free Space Critical</DisplayName>
  <Description referenceid="E9D9BC96-4975-4F76-AE88-73127B578142">
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
  <CounterEventId>F80AF822-6A89-4C2F-AB38-B55D0D2CD13A</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Free Space Warning Event
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>B3862572-A265-4A2D-B847-1BE7AFE5610E</Id>
  <Name>Free Space Warning</Name>
  <DisplayName referenceid="C7ED1F46-9D87-4C15-92FE-10F9DAABC228">Free Space Warning</DisplayName>
  <Description referenceid="BE07223B-FD00-4755-8569-7C44023473D2">
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
  	  <Type>GenericServer</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>True</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>B3862572-A265-4A2D-B847-1BE7AFE5610E</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Free Space Normal Event
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>C9FFDF1E-3766-49C8-BC84-91E95C2E5225</Id>
  <Name>Free Space Normal</Name>
  <DisplayName referenceid="F9586C19-4B64-446E-BD29-6A8662135393">Free Space Normal</DisplayName>
  <Description referenceid="12FF0469-86D8-4B5B-9793-3F28EBBD58A5">
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
  	  <Type>GenericServer</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>True</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>C9FFDF1E-3766-49C8-BC84-91E95C2E5225</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Retention time Critical Event
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>D1603FBE-4BB8-49EB-9928-0E8EB3C736CE</Id>
  <Name>Retention time Critical</Name>
  <DisplayName referenceid="552F310C-BE2D-4D8C-94DE-BD944CF45271">Retention time Critical</DisplayName>
  <Description referenceid="DBD393CA-9367-4347-B4BF-D071D0EDA44B">
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
  <CounterEventId>D1603FBE-4BB8-49EB-9928-0E8EB3C736CE</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Retention time Warning Event
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>5851E907-35CD-480D-9198-669E9BD8610A</Id>
  <Name>Retention time Warning</Name>
  <DisplayName referenceid="A64DA543-B3B5-433C-9136-E0B091CA327D">Retention time Warning</DisplayName>
  <Description referenceid="5E7030A1-8E84-4520-81B5-09F541E52CAF">
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
  	  <Type>GenericServer</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>True</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>5851E907-35CD-480D-9198-669E9BD8610A</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Retention time Normal Event
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>11C329C1-B36E-4F3F-9AB9-9B3A462DC0DA</Id>
  <Name>Retention time Normal</Name>
  <DisplayName referenceid="054148D0-D5DB-40A3-80CC-7FAD658453EE">Retention time Normal</DisplayName>
  <Description referenceid="13EAA89D-42F7-492D-BEAF-94A6CD8E3459">
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
  	  <Type>GenericServer</Type>
  	  <Filters>
  	  </Filters>
    </Source>
  </Sources>
  <OccursGlobally>True</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>11C329C1-B36E-4F3F-9AB9-9B3A462DC0DA</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime
GO