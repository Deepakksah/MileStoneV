-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Update namespace display name 
DECLARE @EventDefinitionXml nvarchar(4000)

SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>FB78A562-B1F4-4C39-B68D-05B87488E858</Id>
  <Name>ManagementServerFailoverNodeStarted</Name>
  <DisplayName referenceid="E23AFBFF-5259-416A-B996-E70D78E0AE63">Failover management server started</DisplayName>
  <Description referenceid="C45FD76C-FF46-4CCE-B2CF-6BFCA9CD22D0">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
    <Type>Server</Type>
    <SubType>MSFailover</SubType>
    <Id />
    <Name />
  </Generator>
  <Sources>
    <Source>
      <Type>Server</Type>
      <Filters />
    </Source>
  </Sources>
  <OccursGlobally>True</OccursGlobally>
  <BuiltIn>True</BuiltIn>
  <CounterEventId>FB78A562-B1F4-4C39-B68D-05B87488E858</CounterEventId>
</EventType>'

UPDATE [EventTypes]
SET [Definition] = @EventDefinitionXml, [GeneratorSubType] = 'MSFailover'
WHERE [IDEventType] = 'FB78A562-B1F4-4C39-B68D-05B87488E858'

GO