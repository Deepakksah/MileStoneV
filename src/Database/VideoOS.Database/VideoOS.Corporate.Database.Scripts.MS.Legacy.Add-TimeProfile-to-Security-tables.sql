---- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.columns WHERE [object_id] = OBJECT_ID(N'[SecurityAttributes]') AND name = N'TimeProfileBased')
BEGIN
	ALTER TABLE SecurityAttributes
	ADD [TimeProfileBased] BIT NOT NULL CONSTRAINT TimeProfileBasedDefault DEFAULT 0
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE [object_id] = OBJECT_ID(N'[SecurityAttributes]') AND name = N'TimeBackwardBased')
BEGIN
	ALTER TABLE SecurityAttributes
	ADD [TimeBackwardBased] BIT NOT NULL CONSTRAINT TimeBackwardBasedDefault DEFAULT 0
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE [object_id] = OBJECT_ID(N'[SecurityPermissions]') AND name = N'IDTimeProfile')
BEGIN
	ALTER TABLE SecurityPermissions
	ADD [IDTimeProfile] GUIDTYPE NULL
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE [object_id] = OBJECT_ID(N'[SecurityPermissions]') AND name = N'IsDefaultTimeProfile')
BEGIN
	ALTER TABLE SecurityPermissions
	ADD [IsDefaultTimeProfile] BIT NOT NULL CONSTRAINT SecurityPermissions_IsDefaultTimeProfileConstraint DEFAULT 1
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE [object_id] = OBJECT_ID(N'[SecurityPermissionsDevice]') AND name = N'IDTimeProfile')
BEGIN
	ALTER TABLE SecurityPermissionsDevice
	ADD [IDTimeProfile] GUIDTYPE NULL
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE [object_id] = OBJECT_ID(N'[SecurityPermissionsDevice]') AND name = N'TimeBackward')
BEGIN
	ALTER TABLE SecurityPermissionsDevice
	ADD [TimeBackward] INT NOT NULL CONSTRAINT TimeBackwardDefault DEFAULT 0
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE [object_id] = OBJECT_ID(N'[SecurityPermissionsDevice]') AND name = N'IsDefaultTimeProfile')
BEGIN
	ALTER TABLE SecurityPermissionsDevice
	ADD [IsDefaultTimeProfile] BIT NOT NULL CONSTRAINT SecurityPermissionsDevice_IsDefaultTimeProfileConstraint DEFAULT 1
END
GO

-- Add new Smart Client/Remote Client attribute
IF NOT EXISTS(SELECT * FROM [SecurityAttributes] WHERE [SecurityAttributeID] = 'APPLICATIONACCESSCLIENTLOGIN')
BEGIN
	INSERT INTO SecurityAttributes(LastModified,SecurityAttributeID,[Name],Description,ParentSecurityAttributeName,SecurityType,HasObject,DisplayOrder,Definition,TimeProfileBased,TimeBackwardBased) 
	VALUES(GETUTCDATE(),'APPLICATIONACCESSCLIENTLOGIN','Smart Client/Remote Client within time profile','Smart Client/Remove Client within time profile access','APPLICATIONACCESS','Application',0,4,'',1,0)
END
GO

-- Update Camera Live View
UPDATE SecurityAttributes
SET TimeProfileBased = 1, Name = 'View live within time profile'
WHERE SecurityAttributeID = 'DEVICELIVEVIEW'
GO

-- Update Camera Playback
UPDATE SecurityAttributes
SET TimeProfileBased = 1, TimeBackwardBased = 1, Name = 'Recorded video within time profile'
WHERE SecurityAttributeID = 'DEVICERECORDEDBROWSE'
GO

-- Update Microphone Live View
UPDATE SecurityAttributes
SET TimeProfileBased = 1, Name = 'Listen to live audio within time profile'
WHERE SecurityAttributeID = 'DEVICEMICLIVEVIEW'
GO

-- Update Microphone Playback
UPDATE SecurityAttributes
SET TimeProfileBased = 1, TimeBackwardBased = 1, Name = 'Recorded audio within time profile'
WHERE SecurityAttributeID = 'DEVICEMICRECORDEDBROWSE'
GO

-- Update Speaker Live View
UPDATE SecurityAttributes
SET TimeProfileBased = 1, Name = 'Listen to live audio within time profile'
WHERE SecurityAttributeID = 'DEVICESPEAKERLIVEVIEW'
GO

-- Update Speaker Playback
UPDATE SecurityAttributes
SET TimeProfileBased = 1, TimeBackwardBased = 1, Name = 'Recorded audio within time profile'
WHERE SecurityAttributeID = 'DEVICESPEAKERRECORDEDBROWSE'
GO

-- Update Activate Output
UPDATE SecurityAttributes
SET TimeProfileBased = 1, Name = 'Activate output within time profile'
WHERE SecurityAttributeID = 'DEVICEACTIVEOUTPUT'
GO

-- Update PTZ Control
UPDATE SecurityAttributes
SET TimeProfileBased = 1, Name = 'PTZ control within time profile'
WHERE SecurityAttributeID = 'DEVICEPTZCONTROL'
GO

-- Update PTZ Presets
UPDATE SecurityAttributes
SET TimeProfileBased = 1, Name = 'Activate PTZ presets within time profile'
WHERE SecurityAttributeID = 'DEVICEPTZPRESETS'
GO

-- Update Speak Control
UPDATE SecurityAttributes
SET TimeProfileBased = 1, Name = 'Speak live within time profile'
WHERE SecurityAttributeID = 'DEVICESPEAKCONTROL'
GO

