-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

UPDATE [EventTypes]
SET	[Definition].modify('
	replace value of (/EventType/OccursGlobally/text())[1]
	with "True"	')
WHERE IDEventType = 'C83A58A9-3272-4E30-BC6D-5A8393FB1DCE'
OR IDEventType = 'F0C89E43-4757-423A-A9E9-2426F228A2E5'
OR IDEventType = 'B08BFF9D-A4A4-4760-8C99-B3DA8C8B6A1B'
GO