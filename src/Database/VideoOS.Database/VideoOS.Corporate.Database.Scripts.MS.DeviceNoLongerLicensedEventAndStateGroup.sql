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

DECLARE @CurrentTime DateTime
SET @CurrentTime = GETUTCDATE()

DECLARE @EventDefinitionXml nvarchar(4000)


-- DeviceLicenseRemoved
-- Added StateGroupId and State
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>65D84F0C-F671-4F99-A502-C0F470F72131</Id>
  <Name>DeviceLicenseRemoved</Name>
  <DisplayName referenceid="4F0D3B8F-ED10-44D6-9848-446C81F162DA">Device license removed</DisplayName>
  <Description referenceid="AE95A2E6-7D06-4203-AD27-C4B2B63178A0" />
  <Parameters />
  <Generator>
    <Type>Device</Type>
    <Id/>
    <Name/>
    <SubType/>
  </Generator>
  <Sources>
    <Source>
      <Type>Camera</Type>
      <Filters/>
    </Source>
    <Source>
      <Type>AudioInput</Type>
      <Filters/>
    </Source>
    <Source>
      <Type>AudioOutput</Type>
      <Filters/>
    </Source>
    <Source>
      <Type>Metadata</Type>
      <Filters/>
    </Source>
    <Source>
      <Type>Input</Type>
      <Filters/>
    </Source>
    <Source>
      <Type>Output</Type>
      <Filters/>
    </Source>
  </Sources>
  <OccursGlobally>False</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>65D84F0C-F671-4F99-A502-C0F470F72131</CounterEventId>
  <StateGroupId>53B40C77-CF48-42A2-A432-DB65AE3AFC7A</StateGroupId>
  <State>NotLicensed</State>
</EventType>'
EXECUTE [dbo].[InternalRegisterEventType] @EventDefinitionXml, @CurrentTime

GO

-- Update Communication State Group

IF OBJECT_ID ( '[dbo].[stategroup_update_initial]', 'P') IS NOT NULL
    DROP PROCEDURE stategroup_update_initial
GO

CREATE PROCEDURE [dbo].stategroup_update_initial
    @Id [UniqueIdentifier],
    @Name [NameType],
    @States NVarchar(1024)
AS
BEGIN
    SET NOCOUNT ON

    DECLARE @CurrentUtcTime DATETIME
    SET @CurrentUtcTime = GETUTCDATE()

    UPDATE [StateGroup] SET 
        [Name] = @Name,
        [States] = @States,
        [last_modified] = @CurrentUtcTime
    WHERE [Id] = @Id

    RETURN 0
END
GO

EXECUTE [dbo].[stategroup_update_initial] '53B40C77-CF48-42A2-A432-DB65AE3AFC7A','Communication','Stopped,Started,Error,NotLicensed'

GO
