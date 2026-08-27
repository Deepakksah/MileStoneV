-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF NOT EXISTS(SELECT * FROM [SecurityNamespace] WHERE [name] = 'SystemMonitor')
BEGIN
	INSERT [SecurityNamespace] ([namespace_id], [name], [display_name], [separator], [structure], [order_no], [user_defined])
	SELECT '2D4048FA-41DA-435A-A800-2419137E4A3E', 'SystemMonitor', 'SystemMonitors', '/', 1, 17, 0

	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit])
	SELECT '2D4048FA-41DA-435A-A800-2419137E4A3E', 'GENERIC_READ', 'Read', 0, 1, 1 UNION ALL
	SELECT '2D4048FA-41DA-435A-A800-2419137E4A3E', 'GENERIC_WRITE', 'Edit', 1, 1, 2 

	DECLARE @namespace_id NVARCHAR(260)
	DECLARE @separator CHAR(1)
	DECLARE @security_object_id UNIQUEIDENTIFIER
	SELECT @namespace_id = [namespace_id], @separator = [separator] FROM [SecurityNamespace] WHERE [name] = 'SystemMonitor'
	SELECT @security_object_id = 'A7D12808-DDEC-4466-8E72-153B14E99BB1'

	IF NOT EXISTS(SELECT * FROM SecurityObject WHERE [namespace_id] = @namespace_id AND [security_object_id] = @security_object_id AND [object_id] = @separator)
	INSERT [SecurityObject] ([namespace_id], [parent_path], [object_id], [object_type], [object_name], [security_object_id], [inherits_from_id])
	SELECT @namespace_id, '', @separator, 1, 'SystemMonitor', @security_object_id, NULL

END
GO