-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT * FROM SecurityAction WHERE [namespace_id] = 'F97772D8-7B6D-4EA4-B6B5-EE4EA010FA2C' AND [name] = 'HARDWARECOMMAND')
BEGIN
	UPDATE [SecurityAction]
	SET [security_bit] = 16
	WHERE [namespace_id] = 'F97772D8-7B6D-4EA4-B6B5-EE4EA010FA2C'
	AND [order_no] = 4

	INSERT INTO SecurityAction ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit])
	SELECT 'F97772D8-7B6D-4EA4-B6B5-EE4EA010FA2C', 'HARDWARECOMMAND', 'Driver commands', 3, 0, 8

END
GO



