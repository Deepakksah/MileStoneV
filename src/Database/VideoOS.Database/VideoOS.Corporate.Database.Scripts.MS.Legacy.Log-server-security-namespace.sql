-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @namespace_id UNIQUEIDENTIFIER
DECLARE @security_object_id UNIQUEIDENTIFIER
DECLARE @separator CHAR(1)

SELECT @namespace_id = 'C201AE6F-6FF3-41D4-BCBF-0F9DB5049988'
SELECT @security_object_id = 'C1A57212-F4F8-40F6-B829-4A8D15A8BE0F'
SELECT @separator = '/'

-- Log Server Namespace
IF NOT EXISTS(SELECT * FROM [SecurityNamespace] WHERE [namespace_id] = @namespace_id)
INSERT [SecurityNamespace] ([namespace_id], [name], [display_name], [separator], [structure], [order_no], [user_defined])
SELECT @namespace_id, 'LogServer', 'Server Logs', @separator, 0, 21, 0

-- Log Server Actions
IF NOT EXISTS(SELECT * FROM [SecurityAction] WHERE [namespace_id] = @namespace_id)
INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit])
SELECT @namespace_id, 'GENERIC_READ', 'Access to logs', 1, 0, 1 UNION ALL
SELECT @namespace_id, 'READ_SYSTEM_LOGS', 'Read system log entries', 2, 0, 2 UNION ALL
SELECT @namespace_id, 'READ_AUDIT_LOGS', 'Read audit log entries', 3, 0, 4 UNION ALL
SELECT @namespace_id, 'READ_RULE_LOGS', 'Read rule-triggered log entries', 4, 0, 8 UNION ALL
SELECT @namespace_id, 'READ_LOG_CONFIG', 'Read log configuration', 5, 0, 16 UNION ALL
SELECT @namespace_id, 'WRITE_LOG_CONFIG', 'Update log configuration', 6, 0, 32 UNION ALL
SELECT @namespace_id, 'ADMIN_SECURITY', 'Administrate security', 7, 0, 64

-- Register Log Server Security Object
IF NOT EXISTS(SELECT * FROM [SecurityObject] WHERE [namespace_id] = @namespace_id AND [security_object_id] = @security_object_id AND [object_id] = @separator)
INSERT [SecurityObject] ([namespace_id], [parent_path], [object_id], [object_type], [object_name], [security_object_id], [inherits_from_id])
SELECT @namespace_id, '', @separator, 1, 'LogServer', @security_object_id, NULL

GO