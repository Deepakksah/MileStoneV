-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[PerformanceCounterType]') AND type = 'U')
BEGIN
	IF (SELECT COUNT([counter_type]) FROM [PerformanceCounterType] WHERE category_name = 'VideoOS Recording Server GPU Performance') = 0
	BEGIN
		INSERT [PerformanceCounterType]([counter_direction], [enable_history], [trigger_per_value], [counter_type], [category_name], [counter_name])
		SELECT 1, 0, 0, 15, 'VideoOS Recording Server GPU Performance', '% Decoder Usage'
		INSERT [PerformanceCounterType]([counter_direction], [enable_history], [trigger_per_value], [counter_type], [category_name], [counter_name])
		SELECT 1, 0, 0, 16, 'VideoOS Recording Server GPU Performance', '% Memory Usage'
		INSERT [PerformanceCounterType]([counter_direction], [enable_history], [trigger_per_value], [counter_type], [category_name], [counter_name])
		SELECT 1, 0, 0, 17, 'VideoOS Recording Server GPU Performance', '% Rendering Usage'
	END
END
GO

if EXISTS (SELECT * FROM sys.objects where object_id = OBJECT_ID(N'[SystemDashboardConfiguration]') AND type = 'U')
BEGIN
	UPDATE [SystemDashboardConfiguration] SET event_types = '<Ids><Id>1</Id><Id>2</Id><Id>6</Id><Id>7</Id><Id>15</Id><Id>16</Id><Id>17</Id></Ids>'
	WHERE state_source_type = '2B2D3601-5857-44A3-880F-BE7957B5B27E' -- All Recording Servers tile ID
END

DECLARE @CurrentTime DateTime
SET @CurrentTime = GETUTCDATE()


DECLARE @Builtin bit
DECLARE @EventDefinitionXml nvarchar(4000)

-- Nvidia Decoding Critical Event
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>B4D94AFC-9EFE-4083-89AF-078E6AB871E4</Id>
  <Name>Nvidia Decoding Critical</Name>
  <DisplayName referenceid="E5FDBFA3-77C3-41D1-A2AA-E2F89F7CA541">Nvidia Decoding Critical</DisplayName>
  <Description referenceid="3CD1568A-CC50-4FC7-B9CB-1AC9C8D9190E">
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
  <CounterEventId>B4D94AFC-9EFE-4083-89AF-078E6AB871E4</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Nvidia Decoding Warning Event
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>3526491F-9CAC-47FB-BEC2-F9C016C26CD5</Id>
  <Name>Nvidia Decoding Warning</Name>
  <DisplayName referenceid="89FFE154-6394-44FF-AF07-CB8113267420">Nvidia Decoding Warning</DisplayName>
  <Description referenceid="B347328A-CACD-4852-91CC-72F1C8EFFD6F">
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
  <CounterEventId>3526491F-9CAC-47FB-BEC2-F9C016C26CD5</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Nvidia Decoding Normal Event
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>94831D23-3267-46E2-811F-D1DA115D63CB</Id>
  <Name>Nvidia Decoding Normal</Name>
  <DisplayName referenceid="83FA8D48-8520-4033-9EC3-752C3EFE1AD4">Nvidia Decoding Normal</DisplayName>
  <Description referenceid="DAFF4E46-C6F8-44C7-9873-AEE0663C2000">
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
  <CounterEventId>94831D23-3267-46E2-811F-D1DA115D63CB</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Nvidia Rendering Critical Event
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>CED4736D-B018-4116-A9F6-1E679741CD77</Id>
  <Name>Nvidia Rendering Critical</Name>
  <DisplayName referenceid="750A6061-3EAF-44E1-B542-A13793E155E1">Nvidia Rendering Critical</DisplayName>
  <Description referenceid="F3511CE0-84D6-4524-9FEF-60623B3EF33F">
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
  <CounterEventId>CED4736D-B018-4116-A9F6-1E679741CD77</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Nvidia Rendering Warning Event
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>2FEBE17F-CB67-4A44-B6D5-D3C5828BF770</Id>
  <Name>Nvidia Rendering Warning</Name>
  <DisplayName referenceid="9C9285A0-1DC4-4257-8B5F-B16A7C6AD8E8">Nvidia Rendering Warning</DisplayName>
  <Description referenceid="46B39BBA-37F9-441A-8E84-B3E2A2DC3DF6">
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
  <CounterEventId>2FEBE17F-CB67-4A44-B6D5-D3C5828BF770</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Nvidia Rendering Normal Event
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>BDC4E2BB-C011-4094-994A-AAA9861163D4</Id>
  <Name>Nvidia Rendering Normal</Name>
  <DisplayName referenceid="6815827E-A00F-421A-AAE3-234E47153536">Nvidia Rendering Normal</DisplayName>
  <Description referenceid="733CC3E9-E385-4D79-883E-A843C801300C">
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
  <CounterEventId>BDC4E2BB-C011-4094-994A-AAA9861163D4</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Nvidia Memory Critical Event
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>CBE4E664-8AD6-404F-9EA3-63633EBB1273</Id>
  <Name>Nvidia Memory Critical</Name>
  <DisplayName referenceid="A081A197-3721-4F9B-A275-4A6082E60A47">Nvidia Memory Critical</DisplayName>
  <Description referenceid="92C348BD-E7A1-4DAC-AE72-51717F7CEE03">
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
  <CounterEventId>CBE4E664-8AD6-404F-9EA3-63633EBB1273</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Nvidia Memory Warning Event
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>48248FDA-AE5B-40BA-99EA-A02B7F39235B</Id>
  <Name>Nvidia Memory Warning</Name>
  <DisplayName referenceid="1F1BAEF6-FCD8-4E41-A40C-B2A844E7A84B">Nvidia Memory Warning</DisplayName>
  <Description referenceid="5A5E7365-9CD2-4DA5-A811-51E6BB913765">
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
  <CounterEventId>48248FDA-AE5B-40BA-99EA-A02B7F39235B</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Nvidia Memory Normal Event
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>CB218E23-934F-4E65-A0A6-11F1822C0F99</Id>
  <Name>Nvidia Memory Normal</Name>
  <DisplayName referenceid="B6C4D91A-A08A-4395-9D55-67CDA3FAE759">Nvidia Memory Normal</DisplayName>
  <Description referenceid="8F1B325D-2120-4DC1-8542-D41A54691445">
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
  <CounterEventId>CB218E23-934F-4E65-A0A6-11F1822C0F99</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

GO