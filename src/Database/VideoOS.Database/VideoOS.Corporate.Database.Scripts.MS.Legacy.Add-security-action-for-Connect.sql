-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @ManagementServerNamespaceId GuidType
SET @ManagementServerNamespaceId = 'E9914E39-A67A-496C-A3C1-5B4AAAB28F15'

IF NOT EXISTS(SELECT * FROM SecurityAction WHERE [namespace_id] = @ManagementServerNamespaceId AND [name] = 'CONNECT')
BEGIN
	UPDATE [SecurityAction]
	SET [order_no] = [order_no] + 1
	WHERE [namespace_id] = @ManagementServerNamespaceId

	INSERT INTO SecurityAction ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit])
	SELECT @ManagementServerNamespaceId, 'CONNECT', 'Connect', 0, 1, 1
	
	UPDATE [SecurityAction]
	SET [security_bit] = POWER(2, order_no)
	WHERE [namespace_id] = @ManagementServerNamespaceId
END
GO