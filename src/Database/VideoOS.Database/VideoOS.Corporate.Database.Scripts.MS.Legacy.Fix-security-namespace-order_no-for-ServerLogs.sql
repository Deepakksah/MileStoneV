-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- It seems that Alarm Definition and Server Logs security namespace both use order_no 21
-- We need to fix this, by incrementing the Server Logs order_no and all order_no greater than 21.
DECLARE @serverLogsNamespaceId UNIQUEIDENTIFIER = 'C201AE6F-6FF3-41D4-BCBF-0F9DB5049988'

DECLARE @order_number INT
SELECT @order_number = [order_no] FROM [dbo].[SecurityNamespace] WHERE [namespace_id] = @serverLogsNamespaceId

IF (@order_number = 21)
BEGIN
    UPDATE [dbo].[SecurityNamespace]
    SET [order_no] = [order_no] + 1
    WHERE [order_no] > 21 OR [namespace_id] = @serverLogsNamespaceId
END

GO
