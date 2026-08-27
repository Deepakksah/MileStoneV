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

-- Scheduled password change started
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>B08BFF9D-A4A4-4760-8C99-B3DA8C8B6A1B</Id>
  <Name>Scheduled password change started</Name>
  <DisplayName referenceid="C8E522BE-7B2C-4348-A1F4-6D958AD3A87D">Scheduled password change started</DisplayName>
  <Description referenceid="C18881BB-46D6-4FCC-9860-F6CC7C8E0AD1">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
    <Type>Server</Type>
    <SubType />
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
  <CounterEventId>B08BFF9D-A4A4-4760-8C99-B3DA8C8B6A1B</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime


-- Scheduled password change completed successfully
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>C83A58A9-3272-4E30-BC6D-5A8393FB1DCE</Id>
  <Name>Scheduled password change completed successfully</Name>
  <DisplayName referenceid="2DF698FE-C3E2-4100-9994-8823395CADAF">Scheduled password change completed successfully</DisplayName>
  <Description referenceid="C46F5A49-CF50-411F-81A6-6FD5CF183175">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
    <Type>Server</Type>
    <SubType />
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
  <CounterEventId>C83A58A9-3272-4E30-BC6D-5A8393FB1DCE</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

-- Scheduled password change completed with errors
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>F0C89E43-4757-423A-A9E9-2426F228A2E5</Id>
  <Name>Scheduled password change completed with errors</Name>
  <DisplayName referenceid="B9E6A24C-562C-4F1D-BCD4-AFA9C01A5308">Scheduled password change completed with errors</DisplayName>
  <Description referenceid="C45FD76C-FF46-4CCE-B2CF-6BFCA9CD22D0">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
    <Type>Server</Type>
    <SubType />
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
  <CounterEventId>F0C89E43-4757-423A-A9E9-2426F228A2E5</CounterEventId>
</EventType>'

EXECUTE InternalRegisterEventType @EventDefinitionXml, @CurrentTime

GO