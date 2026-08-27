-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
ALTER TABLE [Rules]
ALTER COLUMN [definition] XML
GO

UPDATE [Rules]
SET [definition].modify('
	insert <Parameter name="Priority">1</Parameter>
	as last into (/Rule/StartRule/Actions/Action[@name=sql:column("RuleName")])[1]
')
FROM (
	SELECT 
		r.IDRule,
		Actions.value('@name', 'VARCHAR(200)') AS RuleName,
		Actions.value('Parameter[@name="Priority"][1]', 'VARCHAR(200)') AS HasPriority
	FROM [Rules] r
	CROSS APPLY r.[definition].nodes('/Rule/StartRule/Actions/Action') MyActions(Actions)
	WHERE Actions.value('Parameter[@name="Priority"][1]', 'VARCHAR(200)') IS NULL
	  AND Actions.value('@name', 'VARCHAR(200)') = 'VideoOS.DeviceStartStopPatrolling'
) AS x
WHERE x.IDRule = [Rules].IDRule

UPDATE [Rules]
SET [definition].modify('
	insert <Parameter name="Priority">1</Parameter>
	as last into (/Rule/StartRule/Actions/Action[@name=sql:column("RuleName")])[1]
')
FROM (
	SELECT 
		r.IDRule,
		Actions.value('@name', 'VARCHAR(200)') AS RuleName,
		Actions.value('Parameter[@name="Priority"][1]', 'VARCHAR(200)') AS HasPriority
	FROM [Rules] r
	CROSS APPLY r.[definition].nodes('/Rule/StartRule/Actions/Action') MyActions(Actions)
	WHERE Actions.value('Parameter[@name="Priority"][1]', 'VARCHAR(200)') IS NULL
	  AND Actions.value('@name', 'VARCHAR(200)') = 'VideoOS.GotoPreset'
) AS x
WHERE x.IDRule = [Rules].IDRule

UPDATE [Rules]
SET [definition].modify('
	insert <Parameter name="Priority">1</Parameter>
	as last into (/Rule/StartRule/Actions/Action[@name=sql:column("RuleName")])[1]
')
FROM (
	SELECT 
		r.IDRule,
		Actions.value('@name', 'VARCHAR(200)') AS RuleName,
		Actions.value('Parameter[@name="Priority"][1]', 'VARCHAR(200)') AS HasPriority
	FROM [Rules] r
	CROSS APPLY r.[definition].nodes('/Rule/StartRule/Actions/Action') MyActions(Actions)
	WHERE Actions.value('Parameter[@name="Priority"][1]', 'VARCHAR(200)') IS NULL
	  AND Actions.value('@name', 'VARCHAR(200)') = 'VideoOS.GotoDefaultPreset'
) AS x
WHERE x.IDRule = [Rules].IDRule
GO
