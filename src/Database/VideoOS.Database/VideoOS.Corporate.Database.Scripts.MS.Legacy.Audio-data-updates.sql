-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @CurrentTime DateTime
SET @CurrentTime = GETUTCDATE()

IF EXISTS(SELECT * FROM [RegisteredDeviceTypes] WHERE [DeviceType] = 'Camera')
UPDATE [RegisteredDeviceTypes]
SET [SortOrder] = 2 
WHERE [DeviceType] = 'Camera'

IF EXISTS(SELECT * FROM [RegisteredDeviceTypes] WHERE [DeviceType] = 'Input')
UPDATE [RegisteredDeviceTypes]
SET [SortOrder] = 7
WHERE [DeviceType] = 'Input'

IF EXISTS(SELECT * FROM [RegisteredDeviceTypes] WHERE [DeviceType] = 'Output')
UPDATE [RegisteredDeviceTypes]
SET [SortOrder] = 9
WHERE [DeviceType] = 'Output'

-- Add new registered device types to support audio
IF NOT EXISTS(SELECT * FROM [RegisteredDeviceTypes] WHERE [DeviceType] = 'AudioInput')
INSERT INTO [RegisteredDeviceTypes] ([LastModified], [DeviceType], [SortOrder], [PrimaryIconNumber], [IsRecordingDevice])
VALUES(@CurrentTime, 'AudioInput', 4, 63, 1)

IF NOT EXISTS(SELECT * FROM [RegisteredDeviceTypes] WHERE [DeviceType] = 'AudioOutput')
INSERT INTO [RegisteredDeviceTypes] ([LastModified], [DeviceType], [SortOrder], [PrimaryIconNumber], [IsRecordingDevice])
VALUES(@CurrentTime, 'AudioOutput', 5, 64, 1)

-- Add new security attributes to support audio
IF NOT EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'DEVICEMICRECORDEDGETSEQUENCES')
INSERT INTO [SecurityAttributes] ([LastModified], [SecurityAttributeID], [Name], [Description], [ParentSecurityAttributeName], [SecurityType], [HasObject], [DisplayOrder], [Definition])
VALUES(@CurrentTime, 'DEVICEMICRECORDEDGETSEQUENCES', 'Get sequences', 'Get sequences', 'DEVICEMICRECORDED', 'AudioInput', 0, 8, '')

IF NOT EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'DEVICESPEAKERLIVE')
INSERT INTO [SecurityAttributes] ([LastModified], [SecurityAttributeID], [Name], [Description], [ParentSecurityAttributeName], [SecurityType], [HasObject], [DisplayOrder], [Definition])
VALUES(@CurrentTime, 'DEVICESPEAKERLIVE', 'Live', 'Live', 'DEVICESPEAKER', 'AudioOutput', 0, 3, '')

IF NOT EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'DEVICESPEAKERLIVEVIEW')
INSERT INTO [SecurityAttributes] ([LastModified], [SecurityAttributeID], [Name], [Description], [ParentSecurityAttributeName], [SecurityType], [HasObject], [DisplayOrder], [Definition])
VALUES(@CurrentTime, 'DEVICESPEAKERLIVEVIEW', 'Listen to live audio', 'Listen to live audio', 'DEVICESPEAKERLIVE', 'AudioOutput', 0, 4, '')

IF NOT EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'DEVICESPEAKERRECORDED')
INSERT INTO [SecurityAttributes] ([LastModified], [SecurityAttributeID], [Name], [Description], [ParentSecurityAttributeName], [SecurityType], [HasObject], [DisplayOrder], [Definition])
VALUES(@CurrentTime, 'DEVICESPEAKERRECORDED', 'Recorded audio', 'Recorded audio', 'DEVICESPEAKER', 'AudioOutput', 0, 5, '')

IF NOT EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'DEVICESPEAKERRECORDEDBROWSE')
INSERT INTO [SecurityAttributes] ([LastModified], [SecurityAttributeID], [Name], [Description], [ParentSecurityAttributeName], [SecurityType], [HasObject], [DisplayOrder], [Definition])
VALUES(@CurrentTime, 'DEVICESPEAKERRECORDEDBROWSE', 'Browse audio', 'Browse audio', 'DEVICESPEAKERRECORDED', 'AudioOutput', 0, 6, '')

IF NOT EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'DEVICESPEAKERRECORDEDEXPORT')
INSERT INTO [SecurityAttributes] ([LastModified], [SecurityAttributeID], [Name], [Description], [ParentSecurityAttributeName], [SecurityType], [HasObject], [DisplayOrder], [Definition])
VALUES(@CurrentTime, 'DEVICESPEAKERRECORDEDEXPORT', 'Export audio', 'Export audio', 'DEVICESPEAKERRECORDED', 'AudioOutput', 0, 7, '')

IF NOT EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'DEVICESPEAKERRECORDEDGETSEQUENCES')
INSERT INTO [SecurityAttributes] ([LastModified], [SecurityAttributeID], [Name], [Description], [ParentSecurityAttributeName], [SecurityType], [HasObject], [DisplayOrder], [Definition])
VALUES(@CurrentTime, 'DEVICESPEAKERRECORDEDGETSEQUENCES', 'Get sequences', 'Get sequences', 'DEVICESPEAKERRECORDED', 'AudioOutput', 0, 8, '')

IF NOT EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'DEVICESPEAK')
INSERT INTO [SecurityAttributes] ([LastModified], [SecurityAttributeID], [Name], [Description], [ParentSecurityAttributeName], [SecurityType], [HasObject], [DisplayOrder], [Definition])
VALUES(@CurrentTime,'DEVICESPEAK','Speak','Speak','','SPEAK',0,1,'')

IF NOT EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'DEVICESPEAKCONTROL')
INSERT INTO [SecurityAttributes] ([LastModified], [SecurityAttributeID], [Name], [Description], [ParentSecurityAttributeName], [SecurityType], [HasObject], [DisplayOrder], [Definition])
VALUES(@CurrentTime,'DEVICESPEAKCONTROL','Speak live','Speak live','DEVICESPEAK','SPEAK',0,2,'')

IF NOT EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'DEVICESPEAKPRIORITY')
INSERT INTO [SecurityAttributes] ([LastModified], [SecurityAttributeID], [Name], [Description], [ParentSecurityAttributeName], [SecurityType], [HasObject], [DisplayOrder], [Definition])
VALUES(@CurrentTime,'DEVICESPEAKPRIORITY','Speak priority','Priority assigned to Speak requests','DEVICESPEAK','SPEAK',1,3,'<attributedefinition><type>enumeration</type><enumeration><value><name>1000</name><displayname referenceid="A249B2B9-7575-4ffc-BE48-1FC212D96DA4">Very low</displayname></value><value><name>2000</name><displayname referenceid="9530D3A1-2861-424f-B016-12CFCF8259E2">Low</displayname></value><value><name>3000</name><displayname referenceid="ACB73175-EA2A-4022-9205-5788070A93D9">Medium</displayname></value><value><name>4000</name><displayname referenceid="CEA588B8-3323-4941-99F4-15212242338A">High</displayname></value><value><name>5000</name><displayname referenceid="0D9EAC28-7F8C-424e-961A-F8E120528299">Very high</displayname></value><defaultvalue>3000</defaultvalue><administratorvalue>10000</administratorvalue></enumeration></attributedefinition>')

-- Add new device groups to support audio
IF NOT EXISTS(SELECT * FROM [DeviceGroups] WHERE [IDDeviceGroup] = '76D123CB-1E0E-4a2a-BDD5-E4B4B2894614')
INSERT INTO [DeviceGroups] ([LastModified], [IDDeviceGroup], [IDParentDeviceGroup], [DeviceType], [Name], [Description], [BuiltIn], [AutoInsertNew], [IsUserControlled])
VALUES(@CurrentTime, '76D123CB-1E0E-4a2a-BDD5-E4B4B2894614', NULL, 'AudioInput', 'All Microphones', 'All Microphones configured in the system', 1, 1, 0)   

IF NOT EXISTS(SELECT * FROM [DeviceGroups] WHERE [IDDeviceGroup] = 'D92E5C68-F712-49d7-A99D-F88256645D5A')
INSERT INTO [DeviceGroups] ([LastModified], [IDDeviceGroup], [IDParentDeviceGroup], [DeviceType], [Name], [Description], [BuiltIn], [AutoInsertNew], [IsUserControlled])
VALUES(@CurrentTime, 'D92E5C68-F712-49d7-A99D-F88256645D5A', NULL, 'AudioOutput', 'All Speakers', 'All Speakers configured in the system', 1, 1, 0)   

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
		  <Parameter name="DeviceGroupIds">76d123cb-1e0e-4a2a-bdd5-e4b4b2894614,d92e5c68-f712-49d7-a99d-f88256645d5a</Parameter>
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
		  <Parameter name="DeviceGroupIds">76d123cb-1e0e-4a2a-bdd5-e4b4b2894614,d92e5c68-f712-49d7-a99d-f88256645d5a</Parameter>
		  <Parameter name="Delay">0</Parameter>
	    </Action>
	  </Actions>
    </StopRule>
  </Rule>'

IF NOT EXISTS(SELECT * FROM [Rules] WHERE [IDRule] = '162FDB73-E0DC-4A2D-BAA6-54B0D2B16684')
INSERT INTO Rules ([LastModified], [IDRule], [Name], [Description], [Definition], [Enabled], [Priority])
VALUES(@CurrentTime, '162FDB73-E0DC-4A2D-BAA6-54B0D2B16684', 'Default Start Audio Feed Rule', '', @RuleXml, 1, 100)
GO
