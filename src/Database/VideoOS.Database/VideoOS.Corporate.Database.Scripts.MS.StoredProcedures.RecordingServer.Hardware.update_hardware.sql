-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_hardware]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_hardware]
GO

CREATE PROCEDURE [update_hardware]
	@IDHardware [GuidType],
	@name [NameType],
	@description [DescriptionType],
	@URI [UriTypeShort],
	@LoginID [UserIdType],
	@enabled BIT,
	@encryptedPassword [PasswordType],
	@session_id [GuidType],
	@credentialsLastModified datetime,
	@hardwareDataLastUpdated datetime
 AS
BEGIN
	SET NOCOUNT ON

		-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @rowCount INT
		DECLARE @dataSetBefore XML
		DECLARE @dataSetAfter XML
		DECLARE @hardware_name [NameType]
		DECLARE @new_hardware_name [NameType]
		DECLARE @recordingserver_id [GuidType]
		DECLARE @recordingserver_name [NameType]
		DECLARE @hardwareSettingsChanged BIT = 0

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		BEGIN TRAN
	
		-- Make sure that hardware/recording server exists
		SELECT @recordingserver_id = h.[IDRecorder],
			   @recordingserver_name = r.[Name],
			   @hardware_name = h.[Name],
			   @new_hardware_name = h.[Name]
		FROM [Hardware] h 
		INNER JOIN [Recorders] r ON r.IDRecorder = h.[IDRecorder] 
		WHERE h.[IDHardware] = @IDHardware
	
		SELECT @rowCount = @@ROWCOUNT
		IF(@rowCount = 0)
		BEGIN
			;THROW 60058, @procedureName, 1;
		END

		IF
		(
			@name IS NULL AND 
			@description IS NULL AND
			@URI IS NULL AND
			@LoginID IS NULL AND
			@enabled IS NULL AND
			@encryptedPassword IS NULL AND
			@credentialsLastModified IS NULL AND
			@hardwareDataLastUpdated IS NULL
		)
		BEGIN
			ROLLBACK TRAN
			RETURN 0
		END


		-- TODO : Check that last modified doesn't prevent user from updating object


		-- Get DataSet before changes (for DiffGram)
		SET @dataSetBefore = dbo.hardwareAsDataSet(@IDHardware)

		IF(@name IS NOT NULL)
		BEGIN
			-- Update hardware name
			UPDATE [Hardware]
			SET
				[LastModified] = @CurrentUtcTime,
				[Name] = @name,
				@new_hardware_name = @name
			WHERE 
				[IDHardware] = @IDHardware
				AND dbo.StringValueChanging([Name], @name) = 1

			SELECT @rowcount = @@ROWCOUNT

			IF (@rowcount > 0)
			BEGIN
				--	Log an entry similar to "Hardware '{old hardware name}' renamed to '{new hardware name}'."
				EXEC create_pending_log @session_id, 3614, @IDHardware, @name, @recordingserver_id, @recordingserver_name, @hardware_name
			END
		END

		IF(@enabled IS NOT NULL)
		BEGIN
			-- Enable/disable hardware
			UPDATE [Hardware]
			SET
				[LastModified] = @CurrentUtcTime,
				[Enabled] = @enabled
			WHERE 
				[IDHardware] = @IDHardware
				AND [Enabled] != @enabled

			SELECT @rowcount = @@ROWCOUNT
			IF (@rowcount > 0)
			BEGIN
				IF (@enabled = 1)
				BEGIN
					--	Log an entry similar to "Hardware '{hardware name}' enabled."
					EXEC create_pending_log @session_id, 3612, @IDHardware, @new_hardware_name, @recordingserver_id, @recordingserver_name
				END
				ELSE
				BEGIN
					--	Log an entry similar to "Hardware '{hardware name}' disabled."
					EXEC create_pending_log @session_id, 3613, @IDHardware, @new_hardware_name, @recordingserver_id, @recordingserver_name
				END
			END
		END

		IF(@description IS NOT NULL)
		BEGIN
			UPDATE [Hardware]
			SET 
				[LastModified] = @CurrentUtcTime,
				[Description] = @description
			WHERE 
				[IDHardware] = @IDHardware
				AND dbo.StringValueChanging([Description], @description) = 1
		END

		IF(@URI IS NOT NULL)
		BEGIN
			UPDATE [Hardware]
			SET 
				[LastModified] = @CurrentUtcTime,
				[URI] = @URI,
				@hardwareSettingsChanged = 1
			WHERE 
				[IDHardware] = @IDHardware
				AND dbo.StringValueChanging([URI], @URI) = 1
		END

		IF(@LoginID IS NOT NULL)
		BEGIN
			UPDATE [Hardware]
			SET 
				[LastModified] = @CurrentUtcTime,
			    [LoginID] = @LoginID,
				@hardwareSettingsChanged = 1
			WHERE 
				[IDHardware] = @IDHardware
				AND dbo.StringValueChanging([LoginID], @LoginID) = 1
		END

		IF(@encryptedPassword IS NOT NULL)
		BEGIN
			UPDATE [Hardware]
			SET 
				[LastModified] = @CurrentUtcTime,
				[EncryptedPassword] = @encryptedPassword,
				@hardwareSettingsChanged = 1
			WHERE 
				[IDHardware] = @IDHardware
				AND dbo.StringValueChanging([EncryptedPassword], @encryptedPassword) = 1
		END

		IF(@credentialsLastModified IS NOT NULL)
		BEGIN
			UPDATE [Hardware]
			SET 
				[LastModified] = @CurrentUtcTime,
				[CredentialsLastModified] = @credentialsLastModified
			WHERE 
				[IDHardware] = @IDHardware
				AND ([CredentialsLastModified] != @credentialsLastModified OR [CredentialsLastModified] is NULL)
		END

		IF(@hardwareDataLastUpdated IS NOT NULL)
		BEGIN
			UPDATE [Hardware]
			SET 
				[LastModified] = @CurrentUtcTime,
				[HardwareDataLastUpdated] = @hardwareDataLastUpdated
			WHERE 
				[IDHardware] = @IDHardware
				AND ([HardwareDataLastUpdated] != @hardwareDataLastUpdated OR [HardwareDataLastUpdated] is NULL)
		END

		-- Get DataSet after changes
		SET @dataSetAfter = dbo.hardwareAsDataSet(@IDHardware)

		-- Send task to the server - as it may be required to re-claculate and send recorder licenses
		EXEC [create_task_transaction] 'diffgram_update', '909749AE-88C4-4780-A381-383D691E98FA', @dataSetBefore, @dataSetAfter, @CurrentUtcTime

		IF(@hardwareSettingsChanged = 1)
		BEGIN
			--	Log an entry similar to "Settings changed on '{0}'"
			EXEC create_pending_log @session_id, 3615, @IDHardware, @new_hardware_name, @recordingserver_id, @recordingserver_name
		END
		
		COMMIT TRAN

		RETURN 0

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
