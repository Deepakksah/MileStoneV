-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @AdminOrderNo INT

IF EXISTS(SELECT * FROM [SecurityAction] WHERE [name] = 'EXEC_CONFIG_REPORT' AND [namespace_id] = 'E9914E39-A67A-496C-A3C1-5B4AAAB28F15')
BEGIN

	DELETE from [SecurityAccessEntry]
	WHERE [security_object_id] = 
	(
		SELECT [security_object_id] 
		FROM [SecurityObject] 
		WHERE [namespace_id] = 'E9914E39-A67A-496C-A3C1-5B4AAAB28F15'
	)
	AND [action_name] = 'EXEC_CONFIG_REPORT'

	SELECT @AdminOrderNo = [order_no] 
	FROM [SecurityAction] 
	WHERE [namespace_id] = 'E9914E39-A67A-496C-A3C1-5B4AAAB28F15' 
	AND [Name] = 'EXEC_CONFIG_REPORT'

	DELETE from [SecurityAction] 
	WHERE [namespace_id] ='E9914E39-A67A-496C-A3C1-5B4AAAB28F15'
	AND [name] = 'EXEC_CONFIG_REPORT'

	UPDATE [SecurityAction]
	SET [order_no] = [order_no] - 1
	WHERE [namespace_id] = 'E9914E39-A67A-496C-A3C1-5B4AAAB28F15'
	AND [order_no] >= @AdminOrderNo

	UPDATE [SecurityAction]
	SET [security_bit] = POWER(2, order_no)
	WHERE [namespace_id] = 'E9914E39-A67A-496C-A3C1-5B4AAAB28F15'
	AND [order_no] >= @AdminOrderNo
END
GO