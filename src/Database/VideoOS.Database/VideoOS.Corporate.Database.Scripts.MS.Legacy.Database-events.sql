---- This header is used by the dbupdate tool.
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

-- DatabaseFullAutoArchive
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>1FDE8231-F0E4-4919-AED1-B2B142D11E54</Id>
  <Name>DatabaseFullAutoArchive</Name>
  <DisplayName referenceid="736DA5FC-9BA5-4D23-A475-F7CCC13C7648">Database full - auto archive</DisplayName>
  <Description referenceid="7F794105-B766-4104-938E-93554EBEFAF7">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>Recorder</Type>
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
  <CounterEventId>1FDE8231-F0E4-4919-AED1-B2B142D11E54</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @Builtin, @CurrentTime

-- ArchiveNotFinished
SET @Builtin = 1
SET @EventDefinitionXml = 
N'<?xml version="1.0" encoding="UTF-16"?>
<EventType>
  <Id>4DA7CFF5-BF0D-46D9-8B35-85F3288904F4</Id>
  <Name>ArchiveNotFinished</Name>
  <DisplayName referenceid="2FB90F92-5F41-4D60-9BA9-4CF4988A275A">Archive not finished</DisplayName>
  <Description referenceid="42BB2656-B982-44C6-85B8-8647DE4DBE75">
  </Description>
  <Parameters>
  </Parameters>
  <Generator>
  	<Type>Recorder</Type>
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
  <CounterEventId>4DA7CFF5-BF0D-46D9-8B35-85F3288904F4</CounterEventId>
</EventType>'
EXECUTE InternalRegisterEventType @EventDefinitionXml, @Builtin, @CurrentTime

GO