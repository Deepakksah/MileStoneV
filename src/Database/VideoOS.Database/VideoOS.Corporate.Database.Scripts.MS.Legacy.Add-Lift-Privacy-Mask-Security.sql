-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @AdminOrderNo INT

IF NOT EXISTS(SELECT * FROM [SecurityAction] WHERE name = 'LIFT_PRIVACY_MASKING')
BEGIN
	SELECT @AdminOrderNo = [order_no] FROM [SecurityAction] WHERE [namespace_id] = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85' AND [Name] = 'ADMIN_SECURITY'

	UPDATE [SecurityAction]
	SET [order_no] = [order_no] + 1
	WHERE [namespace_id] = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85'
	AND [order_no] >= @AdminOrderNo
	
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit])
	VALUES ('623D03F8-C5D5-46BC-A2F4-4C03562D4F85', 'LIFT_PRIVACY_MASKING', 'Lift privacy masking', @AdminOrderNo, 1, 0)
	
	-- Update security bit for new actions and actions that have changed order number
	UPDATE [SecurityAction]
	SET [security_bit] = POWER(2, order_no)
	WHERE [namespace_id] = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85'
	AND [order_no] >= @AdminOrderNo	
END
GO
