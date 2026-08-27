-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Update namespace display name 
IF EXISTS (SELECT * FROM dbo.[SecurityNamespace] WHERE [namespace_id] = N'BB529DE8-608E-4EBB-B796-889504E54670')
	UPDATE [SecurityNamespace]
	SET [display_name] = 'Access Control'
	WHERE [namespace_id] = 'BB529DE8-608E-4EBB-B796-889504E54670'

IF EXISTS (SELECT * FROM dbo.[SecurityNamespace] WHERE [namespace_id] = N'6F714690-E09F-4D6A-A0CA-9F913CA125E8')
	UPDATE [SecurityNamespace]
	SET [display_name] = 'LPR'
	WHERE [namespace_id] = '6F714690-E09F-4D6A-A0CA-9F913CA125E8'

-- Update security actions - access control
IF EXISTS (SELECT * FROM dbo.[SecurityAction] WHERE [namespace_id] = N'BB529DE8-608E-4EBB-B796-889504E54670' AND [name] = 'GENERIC_READ')
	UPDATE [SecurityAction]
	SET [display_name] = 'Use access control'
	WHERE [name] = 'GENERIC_READ'
	AND [namespace_id] = 'BB529DE8-608E-4EBB-B796-889504E54670'

IF EXISTS (SELECT * FROM dbo.[SecurityAction] WHERE [namespace_id] = N'BB529DE8-608E-4EBB-B796-889504E54670' AND [name] = 'GENERIC_WRITE')
	UPDATE [SecurityAction]
	SET [display_name] = 'Edit'
	WHERE [name] = 'GENERIC_WRITE'
	AND [namespace_id] = 'BB529DE8-608E-4EBB-B796-889504E54670'

IF EXISTS (SELECT * FROM dbo.[SecurityAction] WHERE [namespace_id] = N'BB529DE8-608E-4EBB-B796-889504E54670' AND [name] = 'VIEW_CARDHOLDER_TAB')
	UPDATE [SecurityAction]
	SET [display_name] = 'View cardholders list'
	WHERE [name] = 'VIEW_CARDHOLDER_TAB'
	AND [namespace_id] = 'BB529DE8-608E-4EBB-B796-889504E54670'

IF EXISTS (SELECT * FROM dbo.[SecurityAction] WHERE [namespace_id] = N'BB529DE8-608E-4EBB-B796-889504E54670' AND [name] = 'GENERIC_NOTIFY')
	UPDATE [SecurityAction]
	SET [display_name] = 'Receive notifications'
	WHERE [name] = 'GENERIC_NOTIFY'
	AND [namespace_id] = 'BB529DE8-608E-4EBB-B796-889504E54670'

IF EXISTS (SELECT * FROM dbo.[SecurityAction] WHERE [namespace_id] = N'BB529DE8-608E-4EBB-B796-889504E54670' AND [name] = 'ADMIN_SECURITY')
	UPDATE [SecurityAction]
	SET [display_name] = 'Manage security'
	WHERE [name] = 'ADMIN_SECURITY'
	AND [namespace_id] = 'BB529DE8-608E-4EBB-B796-889504E54670'

-- Update security actions - LPR
IF EXISTS (SELECT * FROM dbo.[SecurityAction] WHERE [namespace_id] = N'6F714690-E09F-4D6A-A0CA-9F913CA125E8' AND [name] = 'LPR_READ')
	UPDATE [SecurityAction]
	SET [display_name] = 'Use LPR'
	WHERE [name] = 'LPR_READ'
	AND [namespace_id] = '6F714690-E09F-4D6A-A0CA-9F913CA125E8'

IF EXISTS (SELECT * FROM dbo.[SecurityAction] WHERE [namespace_id] = N'6F714690-E09F-4D6A-A0CA-9F913CA125E8' AND [name] = 'GENERIC_WRITE')
	UPDATE [SecurityAction]
	SET [display_name] = 'Manage license plate match lists'
	WHERE [name] = 'GENERIC_WRITE'
	AND [namespace_id] = '6F714690-E09F-4D6A-A0CA-9F913CA125E8'

IF EXISTS (SELECT * FROM dbo.[SecurityAction] WHERE [namespace_id] = N'6F714690-E09F-4D6A-A0CA-9F913CA125E8' AND [name] = 'GENERIC_READ')
	UPDATE [SecurityAction]
	SET [display_name] = 'Read license plate match lists'
	WHERE [name] = 'GENERIC_READ'
	AND [namespace_id] = '6F714690-E09F-4D6A-A0CA-9F913CA125E8'

IF EXISTS (SELECT * FROM dbo.[SecurityAction] WHERE [namespace_id] = N'6F714690-E09F-4D6A-A0CA-9F913CA125E8' AND [name] = 'ADMIN_SECURITY')
	UPDATE [SecurityAction]
	SET [display_name] = 'Manage security'
	WHERE [name] = 'ADMIN_SECURITY'
	AND [namespace_id] = '6F714690-E09F-4D6A-A0CA-9F913CA125E8'

-- Update security actions - ONVIF bridge
IF EXISTS (SELECT * FROM dbo.[SecurityAction] WHERE [namespace_id] = N'24A01849-A322-4AD3-8284-0EBCEB1A8036' AND [name] = 'GENERIC_READ')
	UPDATE [SecurityAction]
	SET [display_name] = 'Read'
	WHERE [name] = 'GENERIC_READ'
	AND [namespace_id] = '24A01849-A322-4AD3-8284-0EBCEB1A8036'

IF EXISTS (SELECT * FROM dbo.[SecurityAction] WHERE [namespace_id] = N'24A01849-A322-4AD3-8284-0EBCEB1A8036' AND [name] = 'GENERIC_WRITE')
	UPDATE [SecurityAction]
	SET [display_name] = 'Edit'
	WHERE [name] = 'GENERIC_WRITE'
	AND [namespace_id] = '24A01849-A322-4AD3-8284-0EBCEB1A8036'

-- Update security actions - Alarm
IF EXISTS (SELECT * FROM dbo.[SecurityAction] WHERE [namespace_id] = N'46BB0392-FD06-465B-8776-B90AC16639B3' AND [name] = 'GENERIC_READ')
	UPDATE [SecurityAction]
	SET [display_name] = 'View'
	WHERE [name] = 'GENERIC_READ'
	AND [namespace_id] = '46BB0392-FD06-465B-8776-B90AC16639B3'

IF EXISTS (SELECT * FROM dbo.[SecurityAction] WHERE [namespace_id] = N'46BB0392-FD06-465B-8776-B90AC16639B3' AND [name] = 'GENERIC_WRITE')
	UPDATE [SecurityAction]
	SET [display_name] = 'Manage'
	WHERE [name] = 'GENERIC_WRITE'
	AND [namespace_id] = '46BB0392-FD06-465B-8776-B90AC16639B3'

IF EXISTS (SELECT * FROM dbo.[SecurityAction] WHERE [namespace_id] = N'46BB0392-FD06-465B-8776-B90AC16639B3' AND [name] = 'GENERIC_DISABLE')
	UPDATE [SecurityAction]
	SET [display_name] = 'Disable alarms'
	WHERE [name] = 'GENERIC_DISABLE'
	AND [namespace_id] = '46BB0392-FD06-465B-8776-B90AC16639B3'

IF EXISTS (SELECT * FROM dbo.[SecurityAction] WHERE [namespace_id] = N'46BB0392-FD06-465B-8776-B90AC16639B3' AND [name] = 'GENERIC_NOTIFY')
	UPDATE [SecurityAction]
	SET [display_name] = 'Receive notifications'
	WHERE [name] = 'GENERIC_NOTIFY'
	AND [namespace_id] = '46BB0392-FD06-465B-8776-B90AC16639B3'
GO