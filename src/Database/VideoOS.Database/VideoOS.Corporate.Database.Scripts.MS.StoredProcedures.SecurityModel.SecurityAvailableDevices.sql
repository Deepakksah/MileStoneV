-- This header is used by the dbupdate7 tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SecurityAvailableDevices]') AND type in (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [SecurityAvailableDevices]
GO

CREATE FUNCTION [SecurityAvailableDevices] (@RoleIds XML, @SecurityGroupActionName VARCHAR(100), @EnabledDevicesOnly BIT, @IDRecorder GuidType)
RETURNS @Devices TABLE (IDDevice GuidType)
AS
BEGIN
	DECLARE @RoleIdTable TABLE (ID GuidType)
	INSERT INTO @RoleIdTable
	SELECT ID FROM XmlToIdTable(@RoleIds)
	
	DECLARE @IsAdmin BIT
	DECLARE @CameraOverallAllow BIT
	DECLARE @MicrophoneOverallAllow BIT
	DECLARE @SpeakerOverallAllow BIT
	DECLARE @OutputOverallAllow BIT
	DECLARE @InputOverallAllow BIT
	DECLARE @MetadataOverallAllow BIT
	DECLARE @CameraOverallDeny BIT
	DECLARE @MicrophoneOverallDeny BIT
	DECLARE @SpeakerOverallDeny BIT
	DECLARE @OutputOverallDeny BIT
	DECLARE @InputOverallDeny BIT
	DECLARE @MetadataOverallDeny BIT
	
	IF(EXISTS(SELECT ID FROM @RoleIdTable roles2 WHERE ID = '1f30d14b-ba91-4e8a-8838-eda17931f8fc'))
	BEGIN
		SET @IsAdmin = 1
	END
	ELSE
	BEGIN
		SET @IsAdmin = 0
	END

	IF(@SecurityGroupActionName = 'GENERIC_READ')
	BEGIN
		IF(@IsAdmin = 1)
		BEGIN
			INSERT @Devices 
			SELECT IDDevice
			FROM Devices
			INNER JOIN [Hardware] ON [Devices].[IDHardware] = [Hardware].[IDHardware]
			WHERE
				([Hardware].[IDRecorder] = @IDRecorder OR @IDRecorder IS NULL) AND
				([Hardware].[Enabled] = 1 AND [Devices].[Enabled] = 1 OR @EnabledDevicesOnly = 0)
		END
		ELSE
		BEGIN
			SELECT @CameraOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85' THEN 1
					ELSE 0
					END), 0),
				@CameraOverallDeny = COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85' THEN 1
					ELSE 0
					END), 0),
				@MicrophoneOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = '15F48F88-CA89-4926-9A84-2B02864EC77A' THEN 1
					ELSE 0
					END),0),
				@MicrophoneOverallDeny =COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = '15F48F88-CA89-4926-9A84-2B02864EC77A' THEN 1
					ELSE 0
					END),0),
				@SpeakerOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD' THEN 1
					ELSE 0
					END),0),
				@SpeakerOverallDeny =COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD' THEN 1
					ELSE 0
					END),0),
				@OutputOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = 'A95FCE90-753D-4CB7-86FB-F8CAD9F93B67' THEN 1
					ELSE 0
					END),0),
				@OutputOverallDeny =COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = 'A95FCE90-753D-4CB7-86FB-F8CAD9F93B67' THEN 1
					ELSE 0
					END),0),
				@InputOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = '3ED79EF0-EE2F-40B8-A49A-17E9380C5932' THEN 1
					ELSE 0
					END),0),
				@InputOverallDeny =COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = '3ED79EF0-EE2F-40B8-A49A-17E9380C5932' THEN 1
					ELSE 0
					END),0),
				@MetadataOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D' THEN 1
					ELSE 0
					END),0),
				 @MetadataOverallDeny =COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D' THEN 1
					ELSE 0
					END), 0)
			FROM [SecurityObject] so
					INNER JOIN [SecurityAccessEntry] sae ON so.[security_object_id] = sae.[security_object_id]
					INNER JOIN @RoleIdTable roles ON sae.[user_sid] = roles.ID
					WHERE so.[object_id] = '/'
					AND sae.[action_name] = 'GENERIC_READ'
					AND	namespace_id IN 
					('623D03F8-C5D5-46BC-A2F4-4C03562D4F85'
					,'15F48F88-CA89-4926-9A84-2B02864EC77A'
					,'48B602BC-E752-4BBF-8E2A-7DE01F53A6DD'
					,'3ED79EF0-EE2F-40B8-A49A-17E9380C5932'
					,'A95FCE90-753D-4CB7-86FB-F8CAD9F93B67'
					,'EDE4D51C-F691-4894-9C0B-C3AE096DC04D')

			INSERT @Devices 
			SELECT DISTINCT (d.IDDevice)
			FROM [Devices] d
			INNER JOIN [Hardware] h ON d.[IDHardware] = h.[IDHardware]
			LEFT JOIN
			(
				SELECT * FROM [SecurityPermissionsDevice]
				WHERE [Value] = 'True' 
				AND SecurityAttributeID IN 
				('DEVICEVISIBLE',
				 'DEVICEMICVISIBLE',
				 'DEVICESPEAKERVISIBLE',
				 'DEVICEINPUTVISIBLE',
				 'DEVICEOUTPUTVISIBLE',
				 'DEVICEMETADATAVISIBLE')
				AND IDRole IN (SELECT ID FROM @RoleIdTable)
			) spd ON spd.IDDevice = d.IDDevice
			WHERE 
			(h.[IDRecorder] = @IDRecorder OR @IDRecorder IS NULL) AND
			(@EnabledDevicesOnly = 0 OR (h.[Enabled] = 1 AND d.[Enabled] = 1)) AND
			(
				(@CameraOverallAllow = 1 AND @CameraOverallDeny = 0 AND d.DeviceType = 'Camera') OR
				(@MicrophoneOverallAllow = 1 AND @MicrophoneOverallDeny = 0 AND d.DeviceType = 'AudioInput') OR
				(@SpeakerOverallAllow = 1 AND @SpeakerOverallDeny = 0 AND d.DeviceType = 'AudioOutput') OR
				(@InputOverallAllow = 1 AND @InputOverallDeny = 0 AND d.DeviceType = 'Input') OR
				(@OutputOverallAllow = 1 AND @OutputOverallDeny = 0 AND d.DeviceType = 'Output') OR
				(@MetadataOverallAllow = 1 AND @MetadataOverallDeny = 0 AND d.DeviceType = 'Metadata')
				OR
				(
					((spd.SecurityAttributeID = 'DEVICEVISIBLE' AND  @CameraOverallDeny = 0) OR
					(spd.SecurityAttributeID = 'DEVICEMICVISIBLE'  AND @MicrophoneOverallDeny = 0) OR
					(spd.SecurityAttributeID = 'DEVICESPEAKERVISIBLE' AND @SpeakerOverallDeny = 0) OR
					(spd.SecurityAttributeID = 'DEVICEINPUTVISIBLE' AND @InputOverallDeny = 0) OR
					(spd.SecurityAttributeID = 'DEVICEOUTPUTVISIBLE' AND @OutputOverallDeny = 0) OR
					(spd.SecurityAttributeID = 'DEVICEMETADATAVISIBLE' AND @MetadataOverallDeny = 0))
				)
			)
		END
	END
	ELSE IF(@SecurityGroupActionName = 'READ_BOOKMARKS')
	BEGIN
		IF(@IsAdmin = 1)
		BEGIN
			INSERT @Devices 
			SELECT IDDevice
			FROM Devices
			INNER JOIN [Hardware] ON [Devices].[IDHardware] = [Hardware].[IDHardware]
			WHERE
				([Hardware].[IDRecorder] = @IDRecorder OR @IDRecorder IS NULL) AND
				([Hardware].[Enabled] = 1 AND [Devices].[Enabled] = 1 OR @EnabledDevicesOnly = 0) AND
				([Devices].[DeviceType] = 'Camera' OR [Devices].[DeviceType] = 'AudioInput' OR [Devices].[DeviceType] = 'AudioOutput')
		END
		ELSE
		BEGIN
			SELECT @CameraOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85' THEN 1
					ELSE 0
					END),0),
				@CameraOverallDeny = COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85' THEN 1
					ELSE 0
					END),0),
				@MicrophoneOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = '15F48F88-CA89-4926-9A84-2B02864EC77A' THEN 1
					ELSE 0
					END),0),
				@MicrophoneOverallDeny =COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = '15F48F88-CA89-4926-9A84-2B02864EC77A' THEN 1
					ELSE 0
					END),0),
				@SpeakerOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD' THEN 1
					ELSE 0
					END),0),
				@SpeakerOverallDeny =COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD' THEN 1
					ELSE 0
					END), 0)
			FROM [SecurityObject] so
					INNER JOIN [SecurityAccessEntry] sae ON so.[security_object_id] = sae.[security_object_id]
					INNER JOIN @RoleIdTable roles ON sae.[user_sid] = roles.ID
					WHERE so.[object_id] = '/'
					AND sae.[action_name] = 'READ_BOOKMARKS'
					AND	namespace_id IN 
					('623D03F8-C5D5-46BC-A2F4-4C03562D4F85'
					,'15F48F88-CA89-4926-9A84-2B02864EC77A'
					,'48B602BC-E752-4BBF-8E2A-7DE01F53A6DD'
					)

			INSERT @Devices 
			SELECT DISTINCT (d.IDDevice)
			FROM [Devices] d
			INNER JOIN [Hardware] h ON d.[IDHardware] = h.[IDHardware]
			LEFT JOIN
			(
				SELECT * FROM [SecurityPermissionsDevice]
				WHERE [Value] = 'True' 
				AND (SecurityAttributeID = 'DEVICEBOOKMARKVIEW' OR
					 SecurityAttributeID = 'DEVICEMICBOOKMARKVIEW' OR
					 SecurityAttributeID = 'DEVICESPEAKERBOOKMARKVIEW')
				AND IDRole IN (SELECT ID FROM @RoleIdTable)
			
			) spd ON spd.IDDevice = d.IDDevice
			WHERE 
			(h.[IDRecorder] = @IDRecorder OR @IDRecorder IS NULL) AND
			(@EnabledDevicesOnly = 0 OR (h.[Enabled] = 1 AND d.[Enabled] = 1)) AND
			(
				(@CameraOverallAllow = 1 AND @CameraOverallDeny = 0 AND d.DeviceType = 'Camera') OR
				(@MicrophoneOverallAllow = 1 AND @MicrophoneOverallDeny = 0 AND d.DeviceType = 'AudioInput') OR
				(@SpeakerOverallAllow = 1 AND @SpeakerOverallDeny = 0 AND d.DeviceType = 'AudioOutput')
				OR
				(
					(spd.SecurityAttributeID = 'DEVICEBOOKMARKVIEW' AND @CameraOverallDeny = 0) OR
					(spd.SecurityAttributeID = 'DEVICEMICBOOKMARKVIEW' AND @MicrophoneOverallDeny = 0) OR
					(spd.SecurityAttributeID = 'DEVICESPEAKERBOOKMARKVIEW' AND @SpeakerOverallDeny = 0)
				)
			)
		END
	END
	ELSE IF(@SecurityGroupActionName = 'READ_EVIDENCE_LOCK')
	BEGIN
		IF(@IsAdmin = 1)
		BEGIN
			INSERT @Devices 
			SELECT IDDevice
			FROM Devices
			INNER JOIN [Hardware] ON [Devices].[IDHardware] = [Hardware].[IDHardware]
			WHERE
				([Hardware].[IDRecorder] = @IDRecorder OR @IDRecorder IS NULL) AND
				([Hardware].[Enabled] = 1 AND [Devices].[Enabled] = 1 OR @EnabledDevicesOnly = 0) AND
				([Devices].[DeviceType] = 'Camera' OR [Devices].[DeviceType] = 'AudioInput' OR [Devices].[DeviceType] = 'AudioOutput' OR [Devices].[DeviceType] = 'Metadata')
		END
		ELSE
		BEGIN
			SELECT @CameraOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85' THEN 1
					ELSE 0
					END), 0),
				@CameraOverallDeny = COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85' THEN 1
					ELSE 0
					END), 0),
				@MicrophoneOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = '15F48F88-CA89-4926-9A84-2B02864EC77A' THEN 1
					ELSE 0
					END), 0),
				@MicrophoneOverallDeny =COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = '15F48F88-CA89-4926-9A84-2B02864EC77A' THEN 1
					ELSE 0
					END), 0),
				@SpeakerOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD' THEN 1
					ELSE 0
					END), 0),
				@SpeakerOverallDeny =COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD' THEN 1
					ELSE 0
					END), 0),
				@MetadataOverallAllow = COALESCE(MAX(CASE 
					WHEN [access_deny] = 0 AND namespace_id = 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D' THEN 1
					ELSE 0
					END), 0),
				 @MetadataOverallDeny =COALESCE(MAX(CASE 
					WHEN [access_deny] = 1 AND namespace_id = 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D' THEN 1
					ELSE 0
					END), 0)
			FROM [SecurityObject] so
					INNER JOIN [SecurityAccessEntry] sae ON so.[security_object_id] = sae.[security_object_id]
					INNER JOIN @RoleIdTable roles ON sae.[user_sid] = roles.ID
					WHERE so.[object_id] = '/'
					AND sae.[action_name] = 'READ_EVIDENCE_LOCK'
					AND	namespace_id IN 
					('623D03F8-C5D5-46BC-A2F4-4C03562D4F85'
					,'15F48F88-CA89-4926-9A84-2B02864EC77A'
					,'48B602BC-E752-4BBF-8E2A-7DE01F53A6DD'
					,'EDE4D51C-F691-4894-9C0B-C3AE096DC04D')

			INSERT @Devices 
			SELECT DISTINCT (d.IDDevice)
			FROM [Devices] d
			INNER JOIN [Hardware] h ON d.[IDHardware] = h.[IDHardware]
			LEFT JOIN
			(
				SELECT IDDevice, SecurityAttributeID
				FROM [SecurityPermissionsDevice]
				WHERE [Value] = 'True' 
				AND (SecurityAttributeID = 'DEVICEMARKEDDATAVIEW' OR
					 SecurityAttributeID = 'DEVICEMICMARKEDDATAVIEW' OR
					 SecurityAttributeID = 'DEVICESPEAKERMARKEDDATAVIEW' OR
					 SecurityAttributeID = 'DEVICEMETADATAMARKEDDATAVIEW') 
				AND IDRole IN (SELECT ID FROM @RoleIdTable)
			
			) spd ON spd.IDDevice = d.IDDevice
			WHERE 
			(h.[IDRecorder] = @IDRecorder OR @IDRecorder IS NULL) AND
			(@EnabledDevicesOnly = 0 OR (h.[Enabled] = 1 AND d.[Enabled] = 1)) AND
			(
				(@CameraOverallAllow = 1 AND @CameraOverallDeny = 0 AND d.DeviceType = 'Camera') OR
				(@MicrophoneOverallAllow = 1 AND @MicrophoneOverallDeny = 0 AND d.DeviceType = 'AudioInput') OR
				(@SpeakerOverallAllow = 1 AND @SpeakerOverallDeny = 0 AND d.DeviceType = 'AudioOutput') OR
				(@MetadataOverallAllow = 1 AND @MetadataOverallDeny = 0 AND d.DeviceType = 'Metadata')
				OR
				(
					(spd.SecurityAttributeID = 'DEVICEMARKEDDATAVIEW' AND @CameraOverallDeny = 0) OR
					(spd.SecurityAttributeID = 'DEVICEMICMARKEDDATAVIEW' AND @MicrophoneOverallDeny = 0) OR
					(spd.SecurityAttributeID = 'DEVICESPEAKERMARKEDDATAVIEW' AND @SpeakerOverallDeny = 0) OR
					(spd.SecurityAttributeID = 'DEVICEMETADATAMARKEDDATAVIEW' AND @MetadataOverallDeny = 0)
				)
			)
		END
	END

	RETURN
END
GO