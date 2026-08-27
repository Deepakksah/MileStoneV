-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @CurrentTime DATETIME = GETUTCDATE()
DECLARE @AdminOrderNo INT

DECLARE @CameraNamespaceId UNIQUEIDENTIFIER = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85'
DECLARE @MetadataNamespaceId UNIQUEIDENTIFIER = 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D'
DECLARE @SpeakerNamespaceId UNIQUEIDENTIFIER = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD'
DECLARE @MicrophoneNamespaceId UNIQUEIDENTIFIER = '15F48F88-CA89-4926-9A84-2B02864EC77A'

-- Register overall security actions
IF NOT EXISTS(SELECT 1 FROM [SecurityAction] WHERE [name] = 'VIEW_RESTRICTED_LIVE')
BEGIN
	-- Cameras
	SELECT @AdminOrderNo = [order_no] FROM [SecurityAction] WHERE [namespace_id] = @CameraNamespaceId AND [Name] = 'PLAYBACK'

	UPDATE [SecurityAction]
	SET    [order_no] = [order_no] + 1,
	       [security_bit] = POWER(CAST(2 AS DECIMAL(20)), CAST([order_no] + 1 AS DECIMAL(20)))
	WHERE  [namespace_id] = @CameraNamespaceId AND [order_no] >= @AdminOrderNo

	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit], [legacy_security_name])
	VALUES (@CameraNamespaceId, 'VIEW_RESTRICTED_LIVE', 'View restricted live', @AdminOrderNo, 1, POWER(CAST(2 AS DECIMAL(20)), CAST(@AdminOrderNo AS DECIMAL(20))), 'DEVICELIVEVIEWRESTRICTED')


	-- Microphones
	SELECT @AdminOrderNo = [order_no] FROM [SecurityAction] WHERE [namespace_id] = @MicrophoneNamespaceId AND [Name] = 'PLAYBACK'

	UPDATE [SecurityAction]
	SET    [order_no] = [order_no] + 1,
	       [security_bit] = POWER(CAST(2 AS DECIMAL(20)), CAST([order_no] + 1 AS DECIMAL(20)))
	WHERE  [namespace_id] = @MicrophoneNamespaceId AND [order_no] >= @AdminOrderNo
	
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit], [legacy_security_name])
	VALUES (@MicrophoneNamespaceId, 'LISTEN_RESTRICTED_LIVE', 'Listen to restricted live', @AdminOrderNo, 1, POWER(CAST(2 AS DECIMAL(20)), CAST(@AdminOrderNo AS DECIMAL(20))), 'DEVICEMICLIVEVIEWRESTRICTED')


	-- Speakers
	SELECT @AdminOrderNo = [order_no] FROM [SecurityAction] WHERE [namespace_id] = @SpeakerNamespaceId AND [Name] = 'SPEAK'

	UPDATE [SecurityAction]
	SET    [order_no] = [order_no] + 1,
	       [security_bit] = POWER(CAST(2 AS DECIMAL(20)), CAST([order_no] + 1 AS DECIMAL(20)))
	WHERE  [namespace_id] = @SpeakerNamespaceId AND [order_no] >= @AdminOrderNo
	
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit], [legacy_security_name])
	VALUES (@SpeakerNamespaceId, 'LISTEN_RESTRICTED_LIVE', 'Listen to restricted live', @AdminOrderNo, 1, POWER(CAST(2 AS DECIMAL(20)), CAST(@AdminOrderNo AS DECIMAL(20))), 'DEVICESPEAKERLIVEVIEWRESTRICTED')


	-- Metadata
	SELECT @AdminOrderNo = [order_no] FROM [SecurityAction] WHERE [namespace_id] = @MetadataNamespaceId AND [Name] = 'PLAYBACK'

	UPDATE [SecurityAction]
	SET    [order_no] = [order_no] + 1,
	       [security_bit] = POWER(CAST(2 AS DECIMAL(20)), CAST([order_no] + 1 AS DECIMAL(20)))
	WHERE  [namespace_id] = @MetadataNamespaceId AND [order_no] >= @AdminOrderNo
	
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit], [legacy_security_name])
	VALUES (@MetadataNamespaceId, 'VIEW_RESTRICTED_LIVE', 'View restricted live',  @AdminOrderNo, 1, POWER(CAST(2 AS DECIMAL(20)), CAST(@AdminOrderNo AS DECIMAL(20))), 'DEVICEMETADATALIVEVIEWRESTRICTED')

END

-- Device legacy security permissions
IF NOT EXISTS(SELECT 1 FROM [SecurityAttributes] WHERE SecurityAttributeID = 'DEVICELIVEVIEWRESTRICTED')
	INSERT INTO [SecurityAttributes] ([LastModified], [SecurityAttributeID], [Name], [Description], [ParentSecurityAttributeName], [SecurityType], [HasObject], [DisplayOrder], [Definition])
	VALUES(@CurrentTime,'DEVICELIVEVIEWRESTRICTED', 'View restricted live', 'View restricted live', 'DEVICELIVE', 'Camera', 0, 31, '')

IF NOT EXISTS(SELECT 1 FROM [SecurityAttributes] WHERE SecurityAttributeID = 'DEVICEMICLIVEVIEWRESTRICTED')
	INSERT INTO [SecurityAttributes] ([LastModified], [SecurityAttributeID], [Name], [Description], [ParentSecurityAttributeName], [SecurityType], [HasObject], [DisplayOrder], [Definition])
	VALUES(@CurrentTime,'DEVICEMICLIVEVIEWRESTRICTED', 'Listen to restricted live', 'Listen to restricted live', 'DEVICEMICLIVE', 'AudioInput', 0, 31, '')

IF NOT EXISTS(SELECT 1 FROM [SecurityAttributes] WHERE SecurityAttributeID = 'DEVICESPEAKERLIVEVIEWRESTRICTED')
	INSERT INTO [SecurityAttributes] ([LastModified], [SecurityAttributeID], [Name], [Description], [ParentSecurityAttributeName], [SecurityType], [HasObject], [DisplayOrder], [Definition])
	VALUES(@CurrentTime,'DEVICESPEAKERLIVEVIEWRESTRICTED', 'Listen to restricted live', 'Listen to restricted live', 'DEVICESPEAKERLIVE', 'AudioOutput', 0, 31, '')

IF NOT EXISTS(SELECT 1 FROM [SecurityAttributes] WHERE SecurityAttributeID = 'DEVICEMETADATALIVEVIEWRESTRICTED')
	INSERT INTO [SecurityAttributes] ([LastModified], [SecurityAttributeID], [Name], [Description], [ParentSecurityAttributeName], [SecurityType], [HasObject], [DisplayOrder], [Definition])
	VALUES(@CurrentTime,'DEVICEMETADATALIVEVIEWRESTRICTED', 'View restricted live', 'View restricted live', 'DEVICEMETADATALIVE', 'Metadata', 0, 31, '')


-- Rename 'Listen' to 'Listen live'
UPDATE [SecurityAction]
SET    [display_name] = 'Listen live'
WHERE  [namespace_id] = @MicrophoneNamespaceId AND [name] = 'LISTEN'

UPDATE [SecurityAction]
SET    [display_name] = 'Listen live'
WHERE  [namespace_id] = @SpeakerNamespaceId AND [name] = 'LISTEN'

GO
