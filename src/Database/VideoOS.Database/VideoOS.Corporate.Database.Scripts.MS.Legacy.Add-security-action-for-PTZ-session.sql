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
IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEPTZRESERVESESSION')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEPTZRESERVESESSION', 'Reserve PTZ session', 'Make PTZ sessions reserved so they can not be taken over', 'DEVICEPTZ', 'PTZ', 0, 18, '')

IF NOT EXISTS(SELECT * FROM SecurityAttributes WHERE SecurityAttributeID = 'DEVICEPTZRELEASESESSION')
INSERT INTO SecurityAttributes(LastModified, SecurityAttributeID, [Name], [Description], ParentSecurityAttributeName, SecurityType, HasObject, DisplayOrder, [Definition])
VALUES(@CurrentTime,'DEVICEPTZRELEASESESSION', 'Release PTZ session', 'Release session on a camera even if established reserved by other user', 'DEVICEPTZ', 'PTZ', 0, 19, '')

-- Add new 'Release PTZ session' to 'Camera' security namespace
IF NOT EXISTS(SELECT * FROM [SecurityAction] WHERE name = 'RELEASE_PTZ_SESSION')
BEGIN
	UPDATE [SecurityAction]
	SET [order_no] = [order_no] + 1
	WHERE [namespace_id] = @CameraNamespaceId
	AND [order_no] >= 22
	
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit], [legacy_security_name])
	VALUES (@CameraNamespaceId, 'RELEASE_PTZ_SESSION', 'Release PTZ session', 22, 1, 0, 'DEVICEPTZRELEASESESSION')
	
	-- Update security bit for new actions and actions that have changed order number
	UPDATE [SecurityAction]
	SET [security_bit] = POWER(2, order_no)
	WHERE [namespace_id] = @CameraNamespaceId
	AND [order_no] >= 22	
END

-- Add new 'Reserve PTZ session' to 'Camera' security namespace
IF NOT EXISTS(SELECT * FROM [SecurityAction] WHERE name = 'RESERVE_PTZ_SESSION')
BEGIN
	UPDATE [SecurityAction]
	SET [order_no] = [order_no] + 1
	WHERE [namespace_id] = @CameraNamespaceId
	AND [order_no] >= 22

	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit], [legacy_security_name])
	VALUES (@CameraNamespaceId, 'RESERVE_PTZ_SESSION', 'Reserve PTZ session', 22, 1, 0, 'DEVICEPTZRESERVESESSION')
	
	-- Update security bit for new actions and actions that have changed order number
	UPDATE [SecurityAction]
	SET [security_bit] = POWER(2, order_no)
	WHERE [namespace_id] = @CameraNamespaceId
	AND [order_no] >= 22
END
GO