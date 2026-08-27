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

-- Further modified again in 24.2.05
-- Keeping this one for better developper experience and history

-- DeviceLicenseRemoved
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
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

GO