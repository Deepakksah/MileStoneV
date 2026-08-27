-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @AdminOrderNo INT

IF NOT EXISTS(SELECT * FROM [SecurityAction] WHERE name = 'PLAYBACK_RESTRICTED_RECORDINGS')
BEGIN
	-- For cameras ---------------------
	SELECT @AdminOrderNo = [order_no] FROM [SecurityAction] WHERE [namespace_id] = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85' AND [Name] = 'READ_REMOTE_RECORDINGS'

	UPDATE [SecurityAction]
	SET [order_no] = [order_no] + 1
	WHERE [namespace_id] = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85'
	AND [order_no] >= @AdminOrderNo
	
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit], [legacy_security_name])
	VALUES ('623D03F8-C5D5-46BC-A2F4-4C03562D4F85', 'PLAYBACK_RESTRICTED_RECORDINGS', 'Playback restricted recordings', @AdminOrderNo, 1, 0, 'DEVICERECORDEDBROWSERESTRICTED')
	
	-- Update security bit for new actions and actions that have changed order number
	UPDATE [SecurityAction]
	SET [security_bit] = POWER(2, order_no)
	WHERE [namespace_id] = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85'
	AND [order_no] >= @AdminOrderNo	

	-- For metadata -----------------------
	SELECT @AdminOrderNo = [order_no] FROM [SecurityAction] WHERE [namespace_id] = 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D' AND [Name] = 'READ_REMOTE_RECORDINGS'

	UPDATE [SecurityAction]
	SET [order_no] = [order_no] + 1
	WHERE [namespace_id] = 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D'
	AND [order_no] >= @AdminOrderNo
	
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit], [legacy_security_name])
	VALUES ('EDE4D51C-F691-4894-9C0B-C3AE096DC04D', 'PLAYBACK_RESTRICTED_RECORDINGS', 'Playback restricted recordings',  @AdminOrderNo, 1, 0, 'DEVICEMETADATARECORDEDBROWSERESTRICTED')
	
	-- Update security bit for new actions and actions that have changed order number
	UPDATE [SecurityAction]
	SET [security_bit] = POWER(2, order_no)
	WHERE [namespace_id] = 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D'
	AND [order_no] >= @AdminOrderNo	

	-- For microphones ---------------------
	SELECT @AdminOrderNo = [order_no] FROM [SecurityAction] WHERE [namespace_id] = '15F48F88-CA89-4926-9A84-2B02864EC77A' AND [Name] = 'READ_REMOTE_RECORDINGS'

	UPDATE [SecurityAction]
	SET [order_no] = [order_no] + 1
	WHERE [namespace_id] = '15F48F88-CA89-4926-9A84-2B02864EC77A'
	AND [order_no] >= @AdminOrderNo
	
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit], [legacy_security_name])
	VALUES ('15F48F88-CA89-4926-9A84-2B02864EC77A', 'PLAYBACK_RESTRICTED_RECORDINGS', 'Playback restricted recordings', @AdminOrderNo, 1, 0, 'DEVICEMICRECORDEDBROWSERESTRICTED')
	
	-- Update security bit for new actions and actions that have changed order number
	UPDATE [SecurityAction]
	SET [security_bit] = POWER(2, order_no)
	WHERE [namespace_id] = '15F48F88-CA89-4926-9A84-2B02864EC77A'
	AND [order_no] >= @AdminOrderNo	

	-- For speakers ---------------------
	SELECT @AdminOrderNo = [order_no] FROM [SecurityAction] WHERE [namespace_id] = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD' AND [Name] = 'READ_REMOTE_RECORDINGS'

	UPDATE [SecurityAction]
	SET [order_no] = [order_no] + 1
	WHERE [namespace_id] = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD'
	AND [order_no] >= @AdminOrderNo
	
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit], [legacy_security_name])
	VALUES ('48B602BC-E752-4BBF-8E2A-7DE01F53A6DD', 'PLAYBACK_RESTRICTED_RECORDINGS', 'Playback restricted recordings', @AdminOrderNo, 1, 0, 'DEVICESPEAKERRECORDEDBROWSERESTRICTED')
	
	-- Update security bit for new actions and actions that have changed order number
	UPDATE [SecurityAction]
	SET [security_bit] = POWER(2, order_no)
	WHERE [namespace_id] = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD'
	AND [order_no] >= @AdminOrderNo	
END

IF NOT EXISTS(SELECT * FROM [SecurityAction] WHERE name = 'CREATE_RESTRICTIONS')
BEGIN
	-- For cameras ---------------------
	SELECT @AdminOrderNo = [order_no] FROM [SecurityAction] WHERE [namespace_id] = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85' AND [Name] = 'START_MANUAL_RECORDING'

	UPDATE [SecurityAction]
	SET [order_no] = [order_no] + 1
	WHERE [namespace_id] = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85'
	AND [order_no] >= @AdminOrderNo
	
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit], [legacy_security_name])
	VALUES ('623D03F8-C5D5-46BC-A2F4-4C03562D4F85', 'CREATE_RESTRICTIONS', 'Create and extend live and playback restrictions', @AdminOrderNo, 1, 0, 'DEVICERESTRICTIONCREATE')
	
	-- Update security bit for new actions and actions that have changed order number
	UPDATE [SecurityAction]
	SET [security_bit] = POWER(2, order_no)
	WHERE [namespace_id] = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85'
	AND [order_no] >= @AdminOrderNo	

	-- For metadata -----------------------
	SELECT @AdminOrderNo = [order_no] FROM [SecurityAction] WHERE [namespace_id] = 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D' AND [Name] = 'START_MANUAL_RECORDING'

	UPDATE [SecurityAction]
	SET [order_no] = [order_no] + 1
	WHERE [namespace_id] = 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D'
	AND [order_no] >= @AdminOrderNo
	
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit], [legacy_security_name])
	VALUES ('EDE4D51C-F691-4894-9C0B-C3AE096DC04D', 'CREATE_RESTRICTIONS', 'Create and extend live and playback restrictions',  @AdminOrderNo, 1, 0, 'DEVICEMETADATARESTRICTIONCREATE')
	
	-- Update security bit for new actions and actions that have changed order number
	UPDATE [SecurityAction]
	SET [security_bit] = POWER(2, order_no)
	WHERE [namespace_id] = 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D'
	AND [order_no] >= @AdminOrderNo	

	-- For microphones ---------------------
	SELECT @AdminOrderNo = [order_no] FROM [SecurityAction] WHERE [namespace_id] = '15F48F88-CA89-4926-9A84-2B02864EC77A' AND [Name] = 'START_MANUAL_RECORDING'

	UPDATE [SecurityAction]
	SET [order_no] = [order_no] + 1
	WHERE [namespace_id] = '15F48F88-CA89-4926-9A84-2B02864EC77A'
	AND [order_no] >= @AdminOrderNo
	
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit], [legacy_security_name])
	VALUES ('15F48F88-CA89-4926-9A84-2B02864EC77A', 'CREATE_RESTRICTIONS', 'Create and extend live and playback restrictions', @AdminOrderNo, 1, 0, 'DEVICEMICRESTRICTIONCREATE')
	
	-- Update security bit for new actions and actions that have changed order number
	UPDATE [SecurityAction]
	SET [security_bit] = POWER(2, order_no)
	WHERE [namespace_id] = '15F48F88-CA89-4926-9A84-2B02864EC77A'
	AND [order_no] >= @AdminOrderNo	

	-- For speakers ---------------------
	SELECT @AdminOrderNo = [order_no] FROM [SecurityAction] WHERE [namespace_id] = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD' AND [Name] = 'START_MANUAL_RECORDING'

	UPDATE [SecurityAction]
	SET [order_no] = [order_no] + 1
	WHERE [namespace_id] = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD'
	AND [order_no] >= @AdminOrderNo
	
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit], [legacy_security_name])
	VALUES ('48B602BC-E752-4BBF-8E2A-7DE01F53A6DD', 'CREATE_RESTRICTIONS', 'Create and extend live and playback restrictions', @AdminOrderNo, 1, 0, 'DEVICESPEAKERRESTRICTIONCREATE')
	
	-- Update security bit for new actions and actions that have changed order number
	UPDATE [SecurityAction]
	SET [security_bit] = POWER(2, order_no)
	WHERE [namespace_id] = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD'
	AND [order_no] >= @AdminOrderNo	
END

IF NOT EXISTS(SELECT * FROM [SecurityAction] WHERE name = 'READ_RESTRICTIONS')
BEGIN
	-- For cameras ---------------------
	SELECT @AdminOrderNo = [order_no] FROM [SecurityAction] WHERE [namespace_id] = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85' AND [Name] = 'START_MANUAL_RECORDING'

	UPDATE [SecurityAction]
	SET [order_no] = [order_no] + 1
	WHERE [namespace_id] = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85'
	AND [order_no] >= @AdminOrderNo
	
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit], [legacy_security_name])
	VALUES ('623D03F8-C5D5-46BC-A2F4-4C03562D4F85', 'READ_RESTRICTIONS', 'Read live and playback restrictions', @AdminOrderNo, 1, 0, 'DEVICEREADRESTRICTIONS')
	
	-- Update security bit for new actions and actions that have changed order number
	UPDATE [SecurityAction]
	SET [security_bit] = POWER(2, order_no)
	WHERE [namespace_id] = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85'
	AND [order_no] >= @AdminOrderNo	

	-- For metadata -----------------------
	SELECT @AdminOrderNo = [order_no] FROM [SecurityAction] WHERE [namespace_id] = 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D' AND [Name] = 'START_MANUAL_RECORDING'

	UPDATE [SecurityAction]
	SET [order_no] = [order_no] + 1
	WHERE [namespace_id] = 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D'
	AND [order_no] >= @AdminOrderNo
	
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit], [legacy_security_name])
	VALUES ('EDE4D51C-F691-4894-9C0B-C3AE096DC04D', 'READ_RESTRICTIONS', 'Read live and playback restrictions', @AdminOrderNo, 1, 0, 'DEVICEMETADATAREADRESTRICTIONS')
	
	-- Update security bit for new actions and actions that have changed order number
	UPDATE [SecurityAction]
	SET [security_bit] = POWER(2, order_no)
	WHERE [namespace_id] = 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D'
	AND [order_no] >= @AdminOrderNo	

	-- For microphones ---------------------
	SELECT @AdminOrderNo = [order_no] FROM [SecurityAction] WHERE [namespace_id] = '15F48F88-CA89-4926-9A84-2B02864EC77A' AND [Name] = 'START_MANUAL_RECORDING'

	UPDATE [SecurityAction]
	SET [order_no] = [order_no] + 1
	WHERE [namespace_id] = '15F48F88-CA89-4926-9A84-2B02864EC77A'
	AND [order_no] >= @AdminOrderNo
	
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit], [legacy_security_name])
	VALUES ('15F48F88-CA89-4926-9A84-2B02864EC77A', 'READ_RESTRICTIONS', 'Read live and playback restrictions', @AdminOrderNo, 1, 0, 'DEVICEMICREADRESTRICTIONS')
	
	-- Update security bit for new actions and actions that have changed order number
	UPDATE [SecurityAction]
	SET [security_bit] = POWER(2, order_no)
	WHERE [namespace_id] = '15F48F88-CA89-4926-9A84-2B02864EC77A'
	AND [order_no] >= @AdminOrderNo	

	-- For speakers ---------------------
	SELECT @AdminOrderNo = [order_no] FROM [SecurityAction] WHERE [namespace_id] = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD' AND [Name] = 'START_MANUAL_RECORDING'

	UPDATE [SecurityAction]
	SET [order_no] = [order_no] + 1
	WHERE [namespace_id] = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD'
	AND [order_no] >= @AdminOrderNo
	
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit], [legacy_security_name])
	VALUES ('48B602BC-E752-4BBF-8E2A-7DE01F53A6DD', 'READ_RESTRICTIONS', 'Read live and playback restrictions', @AdminOrderNo, 1, 0,'DEVICESPEAKERREADRESTRICTIONS')
	
	-- Update security bit for new actions and actions that have changed order number
	UPDATE [SecurityAction]
	SET [security_bit] = POWER(2, order_no)
	WHERE [namespace_id] = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD'
	AND [order_no] >= @AdminOrderNo	
END

IF NOT EXISTS(SELECT * FROM [SecurityAction] WHERE name = 'DELETE_RESTRICTIONS')
BEGIN
	-- For cameras ---------------------
	SELECT @AdminOrderNo = [order_no] FROM [SecurityAction] WHERE [namespace_id] = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85' AND [Name] = 'START_MANUAL_RECORDING'

	UPDATE [SecurityAction]
	SET [order_no] = [order_no] + 1
	WHERE [namespace_id] = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85'
	AND [order_no] >= @AdminOrderNo
	
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit], [legacy_security_name])
	VALUES ('623D03F8-C5D5-46BC-A2F4-4C03562D4F85', 'DELETE_RESTRICTIONS', 'Delete and reduce live and playback restrictions', @AdminOrderNo, 1, 0, 'DEVICERESTRICTIONREMOVE')
	
	-- Update security bit for new actions and actions that have changed order number
	UPDATE [SecurityAction]
	SET [security_bit] = POWER(2, order_no)
	WHERE [namespace_id] = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85'
	AND [order_no] >= @AdminOrderNo	


	-- For metadata -----------------------
	SELECT @AdminOrderNo = [order_no] FROM [SecurityAction] WHERE [namespace_id] = 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D' AND [Name] = 'START_MANUAL_RECORDING'

	UPDATE [SecurityAction]
	SET [order_no] = [order_no] + 1
	WHERE [namespace_id] = 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D'
	AND [order_no] >= @AdminOrderNo
	
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit], [legacy_security_name])
	VALUES ('EDE4D51C-F691-4894-9C0B-C3AE096DC04D', 'DELETE_RESTRICTIONS', 'Delete and reduce live and playback restrictions',  @AdminOrderNo, 1, 0, 'DEVICEMETADATARESTRICTIONREMOVE')
	
	-- Update security bit for new actions and actions that have changed order number
	UPDATE [SecurityAction]
	SET [security_bit] = POWER(2, order_no)
	WHERE [namespace_id] = 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D'
	AND [order_no] >= @AdminOrderNo	

	-- For microphones ---------------------
	SELECT @AdminOrderNo = [order_no] FROM [SecurityAction] WHERE [namespace_id] = '15F48F88-CA89-4926-9A84-2B02864EC77A' AND [Name] = 'START_MANUAL_RECORDING'

	UPDATE [SecurityAction]
	SET [order_no] = [order_no] + 1
	WHERE [namespace_id] = '15F48F88-CA89-4926-9A84-2B02864EC77A'
	AND [order_no] >= @AdminOrderNo
	
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit], [legacy_security_name])
	VALUES ('15F48F88-CA89-4926-9A84-2B02864EC77A', 'DELETE_RESTRICTIONS', 'Delete and reduce live and playback restrictions', @AdminOrderNo, 1, 0, 'DEVICEMICRESTRICTIONREMOVE')
	
	-- Update security bit for new actions and actions that have changed order number
	UPDATE [SecurityAction]
	SET [security_bit] = POWER(2, order_no)
	WHERE [namespace_id] = '15F48F88-CA89-4926-9A84-2B02864EC77A'
	AND [order_no] >= @AdminOrderNo	

	-- For speakers ---------------------
	SELECT @AdminOrderNo = [order_no] FROM [SecurityAction] WHERE [namespace_id] = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD' AND [Name] = 'START_MANUAL_RECORDING'

	UPDATE [SecurityAction]
	SET [order_no] = [order_no] + 1
	WHERE [namespace_id] = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD'
	AND [order_no] >= @AdminOrderNo
	
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit], [legacy_security_name])
	VALUES ('48B602BC-E752-4BBF-8E2A-7DE01F53A6DD', 'DELETE_RESTRICTIONS', 'Delete and reduce live and playback restrictions', @AdminOrderNo, 1, 0, 'DEVICESPEAKERRESTRICTIONREMOVE')
	
	-- Update security bit for new actions and actions that have changed order number
	UPDATE [SecurityAction]
	SET [security_bit] = POWER(2, order_no)
	WHERE [namespace_id] = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD'
	AND [order_no] >= @AdminOrderNo	
END

GO
