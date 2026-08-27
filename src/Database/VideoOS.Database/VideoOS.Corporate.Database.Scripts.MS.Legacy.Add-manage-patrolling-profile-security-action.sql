-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(
	SELECT * FROM SecurityAction 
	WHERE [namespace_id] = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85' 
		AND [name] = 'MANAGE_PATROLLING_PROFILE')
BEGIN
	-- Reorder existing actions
	UPDATE [SecurityAction]
	SET order_no = order_no + 1
	WHERE namespace_id = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85'
	AND order_no > 20

	-- Insert new action at location 21
	INSERT [SecurityAction]([namespace_id], [name], [display_name], [order_no], [visible], [security_bit], [legacy_security_name])
	VALUES('623D03F8-C5D5-46BC-A2F4-4C03562D4F85', 'MANAGE_PATROLLING_PROFILE', 'Manage Patrolling Profiles', 21, 1, 67108864, NULL)
END
GO
