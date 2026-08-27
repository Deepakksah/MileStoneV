-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[InternalRegisterEventType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [InternalRegisterEventType]
GO

CREATE PROCEDURE [InternalRegisterEventType]
	@EventDefinitionXml AS NTEXT,
	@CurrentTime AS DATETIME
AS

	DECLARE @idoc INT
	DECLARE @IDEventType GuidType
	DECLARE @GeneratorType InternalIdType
	DECLARE @GeneratorSubType VARCHAR(36)
	DECLARE @IDGenerator GuidType
	DECLARE @Name NameType
	DECLARE @BuiltIn AS BIT

	-- Create an internal representation of the XML document.
	EXEC sp_xml_preparedocument @idoc OUTPUT, @EventDefinitionXml

	-- Execute a SELECT statement using OPENXML rowset provider.
	SELECT 
		@IDEventType = [IDEventType], 
		@Name = [Name],
		@GeneratorType = [GeneratorType],
		@GeneratorSubType = [GeneratorSubType],
		@IDGenerator = [IDGenerator],
		@BuiltIn = [BuiltIn]
	FROM OPENXML (@idoc, '/')
		WITH (IDEventType      GuidType       'EventType/Id',
		      Name             NameType       'EventType/Name',
		      GeneratorType    InternalIdType 'EventType/Generator/Type',
		      GeneratorSubType InternalIdType 'EventType/Generator/SubType',
		      IDGenerator      GuidType       'EventType/Generator/Id',
			  BuiltIn          BIT            'EventType/BuiltIn'
		)
	EXEC sp_xml_removedocument @idoc

	DELETE [EventTypes] WHERE [IDEventType] = @IDEventType

	INSERT INTO 
		EventTypes 
	Values (
	  @CurrentTime,
		@IDEventType,
		@EventDefinitionXml,
		@GeneratorType,
		@IDGenerator,
		@Builtin,
		@Name,
		@GeneratorSubType
	)
GO


DECLARE @CurrentTime DateTime
SET @CurrentTime = GETUTCDATE()

DECLARE @EventDefinitionXml nvarchar(4000)

-- Configuration Changed Event
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>06C5010D-11F2-4D70-BD79-4CEC3A20D589</Id>
  <Name>System configuration changed</Name>
  <DisplayName referenceid="06C5010D-11F2-4D70-BD79-4CEC3A20D589">System configuration changed</DisplayName>
  <Description referenceid="06C5010D-11F2-4D70-BD79-4CEC3A20D589">An item has been added, removed or updated.</Description>
  <Parameters>
  </Parameters>
  <Generator>
    <Type>Server</Type>
    <SubType />
    <Id />
    <Name />
    <GroupId />
    <GroupName />
  </Generator>
  <Sources>
    <Source>
      <Type>Server</Type>
      <Filters />
    </Source>
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>06C5010D-11F2-4D70-BD79-4CEC3A20D589</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Item Position Changed Event
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>A7255B28-D427-4B55-9109-CEEEFC4542B8</Id>
  <Name>MIP item position changed</Name>
  <DisplayName referenceid="A7255B28-D427-4B55-9109-CEEEFC4542B8">Item position changed</DisplayName>
  <Description referenceid="A7255B28-D427-4B55-9109-CEEEFC4542B8">The position of a plugin MIP item has changed in Smart Map.</Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>Device</Type>
  	<Id/>
  	<Name/>
	<SubType/>
  </Generator>
  <Sources>
    <Source>
      <Type>Camera</Type>
      <Filters />
    </Source>
    <Source>
      <Type>Input</Type>
      <Filters />
    </Source>
    <Source>
      <Type>Metadata</Type>
      <Filters />
    </Source>
    <Source>
      <Type>AudioInput</Type>
      <Filters />
    </Source>
    <Source>
      <Type>Output</Type>
      <Filters />
    </Source>
    <Source>
      <Type>AudioOutput</Type>
      <Filters />
    </Source>
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>A7255B28-D427-4B55-9109-CEEEFC4542B8</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime


-- Smart Wall Changed Event
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>629405da-f906-44d6-a3d1-57d3de1d1eb8</Id>
  <Name>Smart Wall changed</Name>
  <DisplayName referenceid="629405da-f906-44d6-a3d1-57d3de1d1eb8">Smart Wall changed</DisplayName>
  <Description referenceid="629405da-f906-44d6-a3d1-57d3de1d1eb8">Smart Wall created, modified, deleted or Monitor in Smart Wall created, modified, deleted.</Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>Server</Type>
  	<Id/>
  	<Name/>
	<SubType/>
  </Generator>
  <Sources>
    <Source>
      <Type>Smart Wall</Type>
      <Filters />
    </Source>
    <Source>
      <Type>Smart Wall Monitor</Type>
	  </Source>
	</Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>629405da-f906-44d6-a3d1-57d3de1d1eb8</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

GO