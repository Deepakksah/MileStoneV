-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

UPDATE
	ArchiveStorage
SET
	schedule.modify('
		replace value of (schedule/subday/recurring/@frequency)[1] 
		with 3600'
	)
WHERE
	(schedule.value('(schedule/subday/recurring/@frequency)[1]', 'int')) < 3600

GO