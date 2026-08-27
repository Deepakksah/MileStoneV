-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @CurrentTime DateTime
SET @CurrentTime = GETUTCDATE()

DECLARE @CameraNamespaceId GuidType
SET @CameraNamespaceId = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85'

-- Add legacy security attribute
IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEPTZMANAGE')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEPTZMANAGE', 'Manage PTZ presets', 'Manage PTZ preset with create/update/delete', 'DEVICEPTZ', 'PTZ', 0, 16, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEPTZMANAGELOCK')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEPTZMANAGELOCK', 'Manage PTZ presets', 'Manage PTZ preset lock - enables to lock and unlock presets', 'DEVICEPTZ', 'PTZ', 0, 17, '')

-- Add new 'Lock/Unlock PTZ preset' to 'Camera' security namespace
IF NOT EXISTS(SELECT * FROM [SecurityAction] WHERE name = 'MANAGE_LOCK_PTZ_PRESET')
BEGIN
	UPDATE [SecurityAction]
	SET [order_no] = [order_no] + 1
	WHERE [namespace_id] = @CameraNamespaceId
	AND [order_no] >= 20
	
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit], [legacy_security_name])
	VALUES (@CameraNamespaceId, 'MANAGE_LOCK_PTZ_PRESET', 'Lock/Unlock PTZ preset', 20, 1, 0, 'DEVICEPTZMANAGELOCK')
	
	-- Update security bit for new actions and actions that have changed order number
	UPDATE [SecurityAction]
	SET [security_bit] = POWER(2, order_no)
	WHERE [namespace_id] = @CameraNamespaceId
	AND [order_no] >= 20	
END

-- Add new 'Manage PTZ preset' to 'Camera' security namespace
IF NOT EXISTS(SELECT * FROM [SecurityAction] WHERE name = 'MANAGE_PTZ_PRESET')
BEGIN
	UPDATE [SecurityAction]
	SET [order_no] = [order_no] + 1
	WHERE [namespace_id] = @CameraNamespaceId
	AND [order_no] >= 20

	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit], [legacy_security_name])
	VALUES (@CameraNamespaceId, 'MANAGE_PTZ_PRESET', 'Manage PTZ preset', 20, 1, 0, 'DEVICEPTZMANAGE')
	
	-- Update security bit for new actions and actions that have changed order number
	UPDATE [SecurityAction]
	SET [security_bit] = POWER(2, order_no)
	WHERE [namespace_id] = @CameraNamespaceId
	AND [order_no] >= 20
END
GO