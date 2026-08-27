-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DELETE FROM [Rules] WHERE [IDRule] = '3D60DF41-AA8C-49AF-A06D-AA30F6CE4360'
GO

DECLARE @CurrentTime DateTime
SET @CurrentTime = GETUTCDATE()

-- Create new default rule to start feed on audio devices
DECLARE @RuleXml nvarchar(4000)
SET @RuleXml = 
N'<Rule>
	<StartRule>
	<RuleType type="Event">
		<EventType>7edbf312-8857-79c1-ea32-14b86f6334a8</EventType>
		<Sources>
			<SourceList />
			<GroupList>
				<GroupId>c3bad46c-bba8-0fb0-1580-244bcfe26add</GroupId>
			</GroupList>
		</Sources>
	</RuleType>
	<Conditions>
		<TimeCondition />
		<Condition />
	</Conditions>
	<Actions>
		<Action name="VideoOS.MIPAction:40121049-598f-4741-8fd3-0b00a64882d2" instanceId="0a76d67e-599d-4b73-a2c6-0ef271e5429f">
		<Parameter name="MipId">40121049-598f-4741-8fd3-0b00a64882d2</Parameter>
		<Parameter name="MipName">Access request notification action</Parameter>
		<Parameter name="Items">3ED8A2DB-EB3E-4E44-A013-4854535D8854</Parameter>
		<Parameter name="ItemGroups" />
		<Parameter name="Delay">0</Parameter>
		</Action>
	</Actions>
	</StartRule>
	<StopRule />
</Rule>'

IF NOT EXISTS(SELECT * FROM [Rules] WHERE [IDRule] = '3D60DF41-AA8C-49AF-A06D-AA30F6CE4360')
INSERT INTO Rules ([LastModified], [IDRule], [Name], [Description], [Definition], [Enabled], [Priority])
VALUES(@CurrentTime, '3D60DF41-AA8C-49AF-A06D-AA30F6CE4360', 'Default Show Access Request Notification Rule', '', @RuleXml, 1, 100)
GO

-- Register security object
IF NOT EXISTS(SELECT * FROM [SecurityObject] WHERE [namespace_id] = 'C24C91A7-7D89-459B-B78E-C4548C74640D' AND [object_id] = '3D60DF41-AA8C-49AF-A06D-AA30F6CE4360')
INSERT [SecurityObject] ([namespace_id], [parent_path], [object_id], [object_type], [object_name], [security_object_id], [inherits_from_id])
VALUES
(
	'C24C91A7-7D89-459B-B78E-C4548C74640D', -- namespace_id
	'/',                                    -- parent_path
	'3D60DF41-AA8C-49AF-A06D-AA30F6CE4360', -- object_id
	2,                                      -- object_type
	'',                                     -- object_name
	'2E2D0C37-1D35-4537-8022-82F535984CAC', -- security_object_id
	'BA3EEB9C-1784-4BB8-A195-DE5B3C8D0EE2'  -- inherits_from_id
)
GO
