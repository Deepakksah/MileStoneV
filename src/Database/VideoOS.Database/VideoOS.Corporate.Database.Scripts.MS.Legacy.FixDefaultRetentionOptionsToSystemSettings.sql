-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================


IF EXISTS(SELECT * FROM [SystemSettings] WHERE [Name] = 'RetentionOptions')
BEGIN
	DELETE FROM [SystemSettings]
	WHERE [SystemSettings].Name = 'RetentionOptions'
END 

BEGIN
	INSERT [SystemSettings]
	VALUES(GETUTCDATE(), 'RetentionOptions', '<?xml version="1.0" encoding="utf-16"?>
<RetentionOptionsXmlFormatter xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns="urn:milestone-systems">
  <RetentionOptions>
    <RetentionTimeOption>
      <Default>true</Default>
      <Id>b665dde9-1000-41fe-af15-ef5946314da7</Id>
      <Name>Option 1</Name>
      <RetentionTimes>
        <RetentionTime>
          <Unit>1</Unit>
          <Option>Days</Option>
        </RetentionTime>
		<RetentionTime>
          <Unit>1</Unit>
          <Option>Weeks</Option>
        </RetentionTime>
		<RetentionTime>
          <Unit>1</Unit>
          <Option>Month</Option>
        </RetentionTime>
        <RetentionTime>
          <Unit>1</Unit>
          <Option>Years</Option>
        </RetentionTime>
		<RetentionTime>
          <Unit>-1</Unit>
          <Option>Indefinite</Option>
        </RetentionTime>
        <RetentionTime>
          <Unit>-1</Unit>
          <Option>UserDefined</Option>
        </RetentionTime>
      </RetentionTimes>
    </RetentionTimeOption>
  </RetentionOptions>
</RetentionOptionsXmlFormatter>')
END

GO

