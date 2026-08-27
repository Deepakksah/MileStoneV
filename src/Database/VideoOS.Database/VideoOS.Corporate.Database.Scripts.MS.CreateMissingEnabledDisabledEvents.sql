-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF OBJECT_ID ( '[dbo].[InternalRegisterEventType]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[InternalRegisterEventType]
GO

CREATE PROCEDURE [dbo].[InternalRegisterEventType]
	@EventDefinitionXml AS NTEXT,
	@CurrentTime AS DATETIME
AS
BEGIN

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

END
GO


IF OBJECT_ID ( '[dbo].[StategroupCreateInitial]', 'P') IS NOT NULL
	DROP PROCEDURE [dbo].[StategroupCreateInitial]
GO

CREATE PROCEDURE [dbo].[StategroupCreateInitial]
	@Id [UniqueIdentifier],
	@Name [NameType],
	@States NVarchar(1024)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @CurrentUtcTime DATETIME
	SET @CurrentUtcTime = GETUTCDATE()

	DELETE [StateGroup] WHERE [Id] = @Id

	INSERT INTO [StateGroup]
	(
		[Id],
		[Name],
		[States],
		[last_modified]
	) VALUES (
		@Id,
		@Name,
		@States,
		@CurrentUtcTime
	)

	RETURN 0
END
GO


DECLARE @CurrentTime DateTime
SET @CurrentTime = GETUTCDATE()

DECLARE @EventDefinitionXml nvarchar(4000)

-- Enabled Event
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>0A51EF93-444C-492E-B4E3-6D6995E1AFD2</Id>
  <Name>Enabled</Name>
  <DisplayName referenceid="9BCDE816-453F-4D1E-ABC3-969195294354">Enabled</DisplayName>
  <Description referenceid="72E2AE13-B361-45C5-BC10-6D3596C75431">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>System</Type>
    <SubType />
  	<Id></Id>
  	<Name/>
	<GroupId />
    <GroupName />
  </Generator>
  <Sources>
    <Source>
      <Type>Hardware</Type>
      <Filters />
    </Source>
    <Source>
      <Type>Camera</Type>
      <Filters />
    </Source>
    <Source>
      <Type>AudioInput</Type>
      <Filters />
    </Source>
    <Source>
      <Type>AudioOutput</Type>
      <Filters />
    </Source>
    <Source>
      <Type>Metadata</Type>
      <Filters />
    </Source>
	<Source>
      <Type>Input</Type>
      <Filters />
    </Source>
    <Source>
      <Type>Output</Type>
      <Filters />
    </Source>
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>2F593DC6-FFA3-4DCB-8269-E5DAEB1D8AE3</CounterEventId>
  <StateGroupId>50AB7704-5C04-440F-AB7C-397485ED8B93</StateGroupId>
  <State>Yes</State>
</EventType>'

EXECUTE [dbo].[InternalRegisterEventType] @EventDefinitionXml, @CurrentTime

-- Disabled Event
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>2F593DC6-FFA3-4DCB-8269-E5DAEB1D8AE3</Id>
  <Name>Disabled</Name>
  <DisplayName referenceid="6020374A-0B11-4279-8D7E-ED6B23508B6A">Disabled</DisplayName>
  <Description referenceid="F31AD0EF-21F9-463C-957A-23832F3AC91C">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>System</Type>
    <SubType />
  	<Id></Id>
  	<Name/>
	<GroupId />
    <GroupName />
  </Generator>
  <Sources>
    <Source>
      <Type>Hardware</Type>
      <Filters />
    </Source>
    <Source>
      <Type>Camera</Type>
      <Filters />
    </Source>
    <Source>
      <Type>AudioInput</Type>
      <Filters />
    </Source>
    <Source>
      <Type>AudioOutput</Type>
      <Filters />
    </Source>
    <Source>
      <Type>Metadata</Type>
      <Filters />
    </Source>
	<Source>
      <Type>Input</Type>
      <Filters />
    </Source>
    <Source>
      <Type>Output</Type>
      <Filters />
    </Source>
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>0A51EF93-444C-492E-B4E3-6D6995E1AFD2</CounterEventId>
  <StateGroupId>50AB7704-5C04-440F-AB7C-397485ED8B93</StateGroupId>
  <State>No</State>
</EventType>'

EXECUTE [dbo].[InternalRegisterEventType] @EventDefinitionXml, @CurrentTime

EXECUTE [dbo].[StategroupCreateInitial] '50AB7704-5C04-440F-AB7C-397485ED8B93','Enabled','Yes,No'

GO