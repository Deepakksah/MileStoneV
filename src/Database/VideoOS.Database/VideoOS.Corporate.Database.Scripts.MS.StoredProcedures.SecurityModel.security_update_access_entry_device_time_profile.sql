-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_update_access_entry_device_time_profile]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_update_access_entry_device_time_profile]
GO

CREATE PROCEDURE [security_update_access_entry_device_time_profile]
	@roleId [GuidType],
	@deviceId [GuidType],
	@deviceName [NameType],
	@actionName NVARCHAR(100),
	@value NVARCHAR(100),
	@timeProfileId [GuidType],
	@timeBackward INT,
	@isDefault BIT,
	@session_id [GuidType]
AS
BEGIN
  -- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		DECLARE @dataSetBefore XML
		DECLARE @dataSetAfter XML

		BEGIN TRAN

		DECLARE @action_display_name NVARCHAR(260)
		DECLARE @namespace_display_name NVARCHAR(260)
		DECLARE @recorderId [GuidType]
		DECLARE @timeProfileName [NameType]
		DECLARE @roleName [NameType]
		DECLARE @priority_display_current NVARCHAR(50)

		SELECT @roleName = [Name] FROM [SecurityRoles] WHERE [IDRole] = @roleId
		
		SELECT @timeProfileName = [name] 
		FROM [TimeProfile] 
		WHERE [pkid] = @timeProfileId

		SELECT @action_display_name = sa.display_name, @namespace_display_name = sn.display_name
		FROM [SecurityAction] sa
			INNER JOIN [SecurityNamespace] sn ON sa.[namespace_id] = sn.[namespace_id]
		WHERE [legacy_security_name] = @actionName

		IF(@actionName = 'DEVICESPEAKPRIORITY')
		BEGIN
			SET @action_display_name = 'Speaker priority'
			SELECT @namespace_display_name = [display_name] FROM [SecurityNamespace] WHERE [namespace_id] = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD'
		END

		IF(@actionName = 'DEVICEPTZPRIORITY')
		BEGIN
			SET @action_display_name = 'PTZ priority'
			SELECT @namespace_display_name = [display_name] FROM [SecurityNamespace] WHERE [namespace_id] = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85'
		END

		SELECT @recorderId = r.[IDRecorder]
		FROM [Devices] AS d 
			INNER JOIN [Hardware] AS h ON d.[IDHardware] = h.[IDHardware]
			INNER JOIN [Recorders] AS r ON h.[IDRecorder] = r.[IDRecorder]
		WHERE d.IDDevice = @deviceId

		IF EXISTS (SELECT * FROM [SecurityPermissionsDevice] WHERE 
			[SecurityAttributeId] = @actionName AND
			[IDRole] = @roleId AND
			[IDDevice] = @deviceId )
		BEGIN
			IF @actionName = 'DEVICEPTZPRIORITY' AND @value = '0' 
			BEGIN
				SET @dataSetBefore = dbo.securityPermissionsDeviceAsDataSet(@roleId, @recorderId)

				DELETE FROM [SecurityPermissionsDevice] WHERE
					[SecurityAttributeId] = @actionName AND
					[IDRole] = @roleId AND
					[IDDevice] = @deviceId

				EXEC create_pending_log @session_id, 9030, @namespace_display_name, @action_display_name, @deviceId, @deviceName, @roleID, @roleName, 3000

				SET @dataSetAfter = dbo.securityPermissionsDeviceAsDataSet(@roleId, @recorderId)
				IF (@dataSetAfter IS NULL)
					EXEC [create_task_transaction]
						@type = 'diffgram_security_delete',
						@target = '909749AE-88C4-4780-A381-383D691E98FA',
						@data_a = @dataSetBefore,
						@current_time = @CurrentUtcTime
				ELSE
					EXEC [create_task_transaction]
						@type = 'diffgram_security_update',
						@target = '909749AE-88C4-4780-A381-383D691E98FA',
						@data_a = @dataSetBefore,
						@data_b = @dataSetAfter,
						@current_time = @CurrentUtcTime
			END
			ELSE
			BEGIN
				SET @dataSetBefore = dbo.securityPermissionsDeviceAsDataSet(@roleId, @recorderId)

				DECLARE @CurrentValue NVARCHAR(256)
				DECLARE @CurrentTimeProfileId [GuidType]
				DECLARE @CurrentTimeBackwards INT
				DECLARE @CurrentIsDefaultTimeProfile BIT

				SELECT
					@CurrentValue = [Value],
					@CurrentTimeProfileId = [IDTimeProfile],
					@CurrentTimeBackwards = [TimeBackward],
					@CurrentIsDefaultTimeProfile = [IsDefaultTimeProfile]
				FROM 
					[SecurityPermissionsDevice]
				WHERE
					[SecurityAttributeId] = @actionName AND
					[IDRole] = @roleId AND
					[IDDevice] = @deviceId

				UPDATE [SecurityPermissionsDevice]
				SET
					[LastModified] = @CurrentUtcTime,
					[Value] = @value,
					[IDTimeProfile] = @timeProfileId,
					[TimeBackward] = @timeBackward,
					[IsDefaultTimeProfile] = @isDefault
				WHERE
					[SecurityAttributeId] = @actionName AND
					[IDRole] = @roleId AND
					[IDDevice] = @deviceId

				-- AUDIT LOG

				IF(@actionName = 'DEVICESPEAKPRIORITY')
				BEGIN
					DECLARE @priority_display NVARCHAR(50)
					SELECT @priority_display = 
						CASE @value
							WHEN 1000 THEN 'Very Low'
							WHEN 2000 THEN 'Low'
							WHEN 3000 THEN 'Medium'
							WHEN 4000 THEN 'High'
							WHEN 5000 THEN 'Very High'
						END

					SELECT @priority_display_current = 
						CASE @CurrentValue
							WHEN 1000 THEN 'Very Low'
							WHEN 2000 THEN 'Low'
							WHEN 3000 THEN 'Medium'
							WHEN 4000 THEN 'High'
							WHEN 5000 THEN 'Very High'
						END

					IF(@CurrentValue IS NULL AND @value IS NOT NULL)
					BEGIN
						-- Change from default Medium to @value
						EXEC create_pending_log @session_id, 9028, @namespace_display_name, @action_display_name, @deviceId, @deviceName, @roleID, @roleName, 'Medium', @priority_display
					END
					ELSE IF(@CurrentValue <> @value)
					BEGIN
						-- Change from @CurrentValue to @value
						EXEC create_pending_log @session_id, 9028, @namespace_display_name, @action_display_name, @deviceId, @deviceName, @roleID, @roleName, @priority_display_current, @priority_display
					END
				END
				ELSE IF(@actionName = 'DEVICEPTZPRIORITY')
				BEGIN
					IF(@CurrentValue IS NULL AND @value IS NOT NULL )
					BEGIN
						-- Change from default 3000 to @value
						EXEC create_pending_log @session_id, 9028, @namespace_display_name, @action_display_name, @deviceId, @deviceName, @roleID, @roleName, 3000, @value
					END
					ELSE IF(@CurrentValue <> @value)
					BEGIN
						-- Change from @CurrentValue to @value
						EXEC create_pending_log @session_id, 9028, @namespace_display_name, @action_display_name, @deviceId, @deviceName, @roleID, @roleName, @CurrentValue, @value
					END
				END
				ELSE
				BEGIN
					IF(	@timeBackward IS NOT NULL AND 
						@CurrentTimeBackwards IS NOT NULL AND 
						@timeBackward <> @CurrentTimeBackwards )
					BEGIN
						DECLARE @timeBackwardDays INT
						SELECT @timeBackwardDays = @timeBackward / 1440

						DECLARE @currentTimeBackwardDays INT
						SELECT @currentTimeBackwardDays = @CurrentTimeBackwards / 1440

						-- Timebackward changed
						IF(@CurrentTimeBackwards = 0)
						BEGIN
							-- Current is "No Restriction"
							IF(@timeBackward < 1440)
							BEGIN
								-- Restriction changed from no restrictions to restrictions minutes
								EXEC create_pending_log @session_id, 9021, @namespace_display_name, @action_display_name, @deviceId, @deviceName, @roleID, @roleName, 0, @timeBackward
							END
							ELSE
							BEGIN
								-- Restriction changed from no restrictions to restrictions days
								EXEC create_pending_log @session_id, 9020, @namespace_display_name, @action_display_name, @deviceId, @deviceName, @roleID, @roleName, 0, @timeBackwardDays
							END
						END
						ELSE IF(@CurrentTimeBackwards < 1440)
						BEGIN
							-- Current is restrictions in minutes
							IF(@timeBackward = 0)
							BEGIN
								-- Restriction changed from restrictions minutes to no restrictions
								EXEC create_pending_log @session_id, 9022, @namespace_display_name, @action_display_name, @deviceId, @deviceName, @roleID, @roleName, @CurrentTimeBackwards, 0
							END
							ELSE IF(@timeBackward < 1440)
							BEGIN
								-- Restriction changed from restrictions minutes to restrictions minutes
								EXEC create_pending_log @session_id, 9023, @namespace_display_name, @action_display_name, @deviceId, @deviceName, @roleID, @roleName, @CurrentTimeBackwards, @timeBackward
							END
							ELSE
							BEGIN
								-- Restriction changed from restrictions minutes to restrictions days
								EXEC create_pending_log @session_id, 9024, @namespace_display_name, @action_display_name, @deviceId, @deviceName, @roleID, @roleName, @CurrentTimeBackwards, @timeBackwardDays
							END
						END
						ELSE
						BEGIN
							-- Current is restrictions in days
							IF(@timeBackward = 0)
							BEGIN
								-- Restriction changed from restrictions days to no restrictions
								EXEC create_pending_log @session_id, 9025, @namespace_display_name, @action_display_name, @deviceId, @deviceName, @roleID, @roleName, @currentTimeBackwardDays, 0
							END
							ELSE IF(@timeBackward < 1440)
							BEGIN
								-- Restriction changed from restrictions days to restrictions minutes
								EXEC create_pending_log @session_id, 9026, @namespace_display_name, @action_display_name, @deviceId, @deviceName, @roleID, @roleName, @currentTimeBackwardDays, @timeBackward
							END
							ELSE
							BEGIN
								-- Restriction changed from restrictions days to restrictions days
								EXEC create_pending_log @session_id, 9027, @namespace_display_name, @action_display_name, @deviceId, @deviceName, @roleID, @roleName, @currentTimeBackwardDays, @timeBackwardDays
							END
						END
					END

					IF(@CurrentIsDefaultTimeProfile = 1 AND (@isDefault IS NULL OR @isDefault = 0))
					BEGIN
						-- Timeprofile changed from Default 
						IF(@timeProfileName IS NULL)
						BEGIN
								-- Profile changed from Default to Always
							EXEC create_pending_log @session_id, 9019, @namespace_display_name, @action_display_name, @deviceId, @deviceName, @roleID, @roleName, 'Default', 'Always'
						END
						ELSE
						BEGIN
							-- Profile changed from Default to timeprofile
							EXEC create_pending_log @session_id, 9018, @namespace_display_name, @action_display_name, @deviceId, @deviceName, @roleID, @roleName, 'Default', @timeProfileName
						END
					END
					ELSE IF(@CurrentTimeProfileId IS NOT NULL)
					BEGIN
						DECLARE @currentTimeProfileName NameType
	
						SELECT @currentTimeProfileName = [name] 
						FROM [TimeProfile] 
						WHERE [pkid] = @CurrentTimeProfileId

						-- Timeprofile changed from profile
						IF(@isDefault = 1)
						BEGIN
							-- Profile changed from timeprofile to default
							EXEC create_pending_log @session_id, 9017, @namespace_display_name, @action_display_name, @deviceId, @deviceName, @roleID, @roleName, @currentTimeProfileName, 'Default'
						END
						ELSE IF(@timeProfileName IS NOT NULL AND @CurrentTimeProfileId <> @timeProfileId)
						BEGIN 
								-- Profile changed from profile to profile
							EXEC create_pending_log @session_id, 9016, @namespace_display_name, @action_display_name, @deviceId, @deviceName, @roleID, @roleName, @currentTimeProfileName, @timeProfileName
						END
						ELSE IF(@timeProfileName IS NULL)
						BEGIN
							-- Profile changed from timeprofile to Always
							EXEC create_pending_log @session_id, 9017, @namespace_display_name, @action_display_name, @deviceId, @deviceName, @roleID, @roleName, @currentTimeProfileName, 'Always'
						END
					END
					ELSE IF(@CurrentIsDefaultTimeProfile <> 1)
					BEGIN
						-- Timeprofile changed from Always
						IF(@isDefault = 1)
						BEGIN
							-- Profile changed from Always to Default
							EXEC create_pending_log @session_id, 9019, @namespace_display_name, @action_display_name, @deviceId, @deviceName, @roleID, @roleName, 'Always', 'Default'

						END
						ELSE IF(@timeProfileName IS NOT NULL)
						BEGIN
							-- Profile changed from Always to timeprofile
							EXEC create_pending_log @session_id, 9018, @namespace_display_name, @action_display_name, @deviceId, @deviceName, @roleID, @roleName, 'Always', @timeProfileName
						END
					END
				END

				SET @dataSetAfter = dbo.securityPermissionsDeviceAsDataSet(@roleId, @recorderId)

				EXEC [create_task_transaction]
					@type = 'diffgram_security_update',
					@target = '909749AE-88C4-4780-A381-383D691E98FA',
					@data_a = @dataSetBefore,
					@data_b = @dataSetAfter,
					@current_time = @CurrentUtcTime
			END
		END
		ELSE IF NOT(@actionName = 'DEVICEPTZPRIORITY' AND @value = '0')
		BEGIN
			INSERT INTO [SecurityPermissionsDevice] ([LastModified], [IDRole],[IDDevice],[SecurityAttributeId],
				[Value], [IDTimeProfile], [TimeBackward], [IsDefaultTimeProfile]) 
			VALUES (@CurrentUtcTime, @roleId, @deviceId, @actionName,
				@value, @timeProfileId, @timeBackward, @isDefault)

			IF(@actionName = 'DEVICESPEAKPRIORITY')
			BEGIN
				-- Add audit for speakpriority with default Medium
				EXEC create_pending_log @session_id, 9029, @namespace_display_name, @action_display_name, @deviceId, @deviceName, @roleID, @roleName, 'Medium'

				IF(@value <> 3000)
				BEGIN
					SELECT @priority_display = 
						CASE @value
							WHEN 1000 THEN 'Very Low'
							WHEN 2000 THEN 'Low'
							WHEN 3000 THEN 'Medium'
							WHEN 4000 THEN 'High'
							WHEN 5000 THEN 'Very High'
						END

					-- Change from default Medium to @value
					EXEC create_pending_log @session_id, 9028, @namespace_display_name, @action_display_name, @deviceId, @deviceName, @roleID, @roleName, 'Medium', @priority_display
				END
			END
			ELSE IF(@actionName = 'DEVICEPTZPRIORITY')
			BEGIN

				-- Add ptz priority with default 3000
				EXEC create_pending_log @session_id, 9029, @namespace_display_name, @action_display_name, @deviceId, @deviceName, @roleID, @roleName, 3000

				IF(@value <> 3000)
				BEGIN
					-- Change from default 3000 to @value
					EXEC create_pending_log @session_id, 9028, @namespace_display_name, @action_display_name, @deviceId, @deviceName, @roleID, @roleName, 3000, @value
				END
			END
			ELSE
			BEGIN
				-- AuditLog
				EXEC create_pending_log @session_id, 9014, @namespace_display_name, @action_display_name, @deviceId, @deviceName, @roleID, @roleName

				IF(@isDefault = 1)
				BEGIN
					-- Profile changed from Always to Default
					EXEC create_pending_log @session_id, 9019, @namespace_display_name, @action_display_name, @deviceId, @deviceName, @roleID, @roleName, 'Always', 'Default'
				END
				ELSE IF(@timeProfileName IS NOT NULL)
				BEGIN
					-- Profile changed from Always to timeprofile
					EXEC create_pending_log @session_id, 9018, @namespace_display_name, @action_display_name, @deviceId, @deviceName, @roleID, @roleName, 'Always', @timeProfileName
				END
			END

			SET @dataSetAfter = dbo.securityPermissionsDeviceAsDataSet(@roleId, @recorderId)

			EXEC [create_task_transaction]
				@type = 'diffgram_security_insert',
				@target = '909749AE-88C4-4780-A381-383D691E98FA',
				@data_a = @dataSetAfter,
				@current_time = @CurrentUtcTime
		END

		COMMIT TRAN
	END TRY
	BEGIN CATCH 
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION
		END;
		DECLARE @shouldReThrowError BIT = dbo.[ShouldReThrowError](ERROR_NUMBER(), ERROR_SEVERITY())
		IF(@shouldReThrowError = 1)
			THROW;
		SET @ErrorArgs = dbo.[FormatErrorMessage](ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE())
		;THROW 60000, @ErrorArgs, 1;
	END CATCH;
END
GO