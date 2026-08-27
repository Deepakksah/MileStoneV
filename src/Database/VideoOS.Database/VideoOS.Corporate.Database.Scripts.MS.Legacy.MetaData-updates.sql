-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @CurrentTime DateTime
SET @CurrentTime = GETUTCDATE()

-- Add new registered device types to support metadata
IF NOT EXISTS(SELECT * FROM [RegisteredDeviceTypes] WHERE [DeviceType] = 'Metadata')
INSERT INTO [RegisteredDeviceTypes] ([LastModified], [DeviceType], [Description], [SortOrder], [PrimaryIconNumber], [IsRecordingDevice])
VALUES(@CurrentTime, 'Metadata', '', 6, 147, 1)

-- Add new device groups to support metadata
IF NOT EXISTS(SELECT * FROM [DeviceGroups] WHERE [IDDeviceGroup] = '875B442C-9E07-4359-8A70-4B64F21E356E')
INSERT INTO [DeviceGroups] ([LastModified], [IDDeviceGroup], [IDParentDeviceGroup], [DeviceType], [Name], [Description], [BuiltIn], [AutoInsertNew], [IsUserControlled])
VALUES(@CurrentTime, '875B442C-9E07-4359-8A70-4B64F21E356E', NULL, 'Metadata', 'AllMetadataName', 'AllMetadataDescription', 1, 1, 0)   

-- Add new security attributes to support metadata

IF NOT EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'DEVICEMETADATA')
INSERT INTO [SecurityAttributes] ([LastModified], [SecurityAttributeID], [Name], [Description], [ParentSecurityAttributeName], [SecurityType], [HasObject], [DisplayOrder], [Definition], [TimeProfileBased], [TimeBackwardBased]) 
VALUES (@CurrentTime,'DEVICEMETADATA','Metadata','Metadata','','Metadata', 0, 1,'', 0, 0)

IF NOT EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'DEVICEMETADATAVISIBLE')
INSERT INTO [SecurityAttributes] ([LastModified], [SecurityAttributeID], [Name], [Description], [ParentSecurityAttributeName], [SecurityType], [HasObject], [DisplayOrder], [Definition], [TimeProfileBased], [TimeBackwardBased]) 
VALUES (@CurrentTime,'DEVICEMETADATAVISIBLE','Visible','Visible','DEVICEMETADATA','Metadata', 0, 10,'', 0, 0)

IF NOT EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'DEVICEMETADATALIVE')
INSERT INTO [SecurityAttributes] ([LastModified], [SecurityAttributeID], [Name], [Description], [ParentSecurityAttributeName], [SecurityType], [HasObject], [DisplayOrder], [Definition], [TimeProfileBased], [TimeBackwardBased]) 
VALUES (@CurrentTime,'DEVICEMETADATALIVE','Live','Live','DEVICEMETADATA','Metadata', 0, 12,'', 0, 0)

IF NOT EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'DEVICEMETADATALIVEVIEW')
INSERT INTO [SecurityAttributes] ([LastModified], [SecurityAttributeID], [Name], [Description], [ParentSecurityAttributeName], [SecurityType], [HasObject], [DisplayOrder], [Definition], [TimeProfileBased], [TimeBackwardBased]) 
VALUES (@CurrentTime,'DEVICEMETADATALIVEVIEW','Get live metadata within time profile','Get live metadata','DEVICEMETADATALIVE','Metadata', 0, 30,'', 1, 0)

IF NOT EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'DEVICEMETADATARECORDED')
INSERT INTO [SecurityAttributes] ([LastModified], [SecurityAttributeID], [Name], [Description], [ParentSecurityAttributeName], [SecurityType], [HasObject], [DisplayOrder], [Definition], [TimeProfileBased], [TimeBackwardBased]) 
VALUES (@CurrentTime,'DEVICEMETADATARECORDED','Recorded metadata','Recorded metadata','DEVICEMETADATA','Metadata', 0, 14,'', 0, 0)

IF NOT EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'DEVICEMETADATARECORDEDBROWSE')
INSERT INTO [SecurityAttributes] ([LastModified], [SecurityAttributeID], [Name], [Description], [ParentSecurityAttributeName], [SecurityType], [HasObject], [DisplayOrder], [Definition], [TimeProfileBased], [TimeBackwardBased]) 
VALUES (@CurrentTime,'DEVICEMETADATARECORDEDBROWSE','Playback metadata','Play back metadata','DEVICEMETADATARECORDED','Metadata', 0, 50,'', 1, 1)

IF NOT EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'DEVICEMETADATARECORDEDGETSEQUENCES')
INSERT INTO [SecurityAttributes] ([LastModified], [SecurityAttributeID], [Name], [Description], [ParentSecurityAttributeName], [SecurityType], [HasObject], [DisplayOrder], [Definition], [TimeProfileBased], [TimeBackwardBased]) 
VALUES (@CurrentTime,'DEVICEMETADATARECORDEDGETSEQUENCES','Get sequences','Get sequences','DEVICEMETADATARECORDED','Metadata', 0, 52,'', 0, 0)

IF NOT EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'DEVICEMETADATARECORDEDEXPORT')
INSERT INTO [SecurityAttributes] ([LastModified], [SecurityAttributeID], [Name], [Description], [ParentSecurityAttributeName], [SecurityType], [HasObject], [DisplayOrder], [Definition], [TimeProfileBased], [TimeBackwardBased]) 
VALUES (@CurrentTime,'DEVICEMETADATARECORDEDEXPORT','Export metadata','Export metadata','DEVICEMETADATARECORDED','Metadata', 0, 58,'', 0, 0)

IF NOT EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'DEVICEMETADATAEDGE')
INSERT INTO [SecurityAttributes] ([LastModified], [SecurityAttributeID], [Name], [Description], [ParentSecurityAttributeName], [SecurityType], [HasObject], [DisplayOrder], [Definition], [TimeProfileBased], [TimeBackwardBased]) 
VALUES (@CurrentTime,'DEVICEMETADATAEDGE','Metadata','Metadata','','EdgeMetadata', 0, 1,'', 0, 0)

IF NOT EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'DEVICEMETADATARETRIEVEEDGERECORDINGS')
INSERT INTO [SecurityAttributes] ([LastModified], [SecurityAttributeID], [Name], [Description], [ParentSecurityAttributeName], [SecurityType], [HasObject], [DisplayOrder], [Definition], [TimeProfileBased], [TimeBackwardBased]) 
VALUES (@CurrentTime,'DEVICEMETADATARETRIEVEEDGERECORDINGS','Retrieve edge recordings','Retrieve edge recordings','DEVICEMETADATAEDGE','EdgeMetadata', 0, 10,'', 0, 0)


-- Create new default rule to start feed on audio devices
DECLARE @RuleXml nvarchar(4000)
SET @RuleXml = 
N'<Rule>
    <StartRule>
	  <RuleType type="TimeInterval">
		<EventType>f07ac0e3-9662-4781-9176-ec9e488aa494</EventType>
	  </RuleType>
	  <Conditions>
	    <TimeCondition>
	      <Region name="VideoOS.Always">
		    <Token type="Value" datatype="Boolean">True</Token>
		  </Region>
        </TimeCondition>
	    <Condition/>
	  </Conditions>
	  <Actions>
	    <Action name="VideoOS.DeviceStartStopFeed" instanceId="73b61206-8fc2-4bfb-9cdb-c3e0b2e191ac">
		  <Parameter name="State">On</Parameter>
		  <Parameter name="DeviceIds"/>
		  <Parameter name="DeviceGroupIds">875B442C-9E07-4359-8A70-4B64F21E356E</Parameter>
	    </Action>
	  </Actions>
    </StartRule>
    <StopRule>
	  <RuleType type="TimeInterval">
        <EventType>f07ac0e3-9662-4781-9176-ec9e488aa494</EventType>
	  </RuleType>
	  <Actions>
	    <Action name="VideoOS.DeviceStartStopFeed" instanceId="1546660a-2b11-4e9c-99d1-aabb193d4644">
	      <Parameter name="State">Off</Parameter>
		  <Parameter name="DeviceIds"/>
		  <Parameter name="DeviceGroupIds">875B442C-9E07-4359-8A70-4B64F21E356E</Parameter>
		  <Parameter name="Delay">0</Parameter>
	    </Action>
	  </Actions>
    </StopRule>
  </Rule>'

IF NOT EXISTS(SELECT * FROM [Rules] WHERE [IDRule] = 'FE61841F-544E-44D7-B7A8-CD709195162D')
INSERT INTO Rules ([LastModified], [IDRule], [Name], [Description], [Definition], [Enabled], [Priority])
VALUES(@CurrentTime, 'FE61841F-544E-44D7-B7A8-CD709195162D', 'Default Start Metadata Feed Rule', '', @RuleXml, 1, 100)
GO
