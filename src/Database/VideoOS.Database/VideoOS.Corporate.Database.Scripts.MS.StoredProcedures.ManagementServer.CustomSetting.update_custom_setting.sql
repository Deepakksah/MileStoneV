-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_custom_setting]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_custom_setting]
GO

CREATE PROCEDURE [update_custom_setting]
	@user_sid [SidType],
	@setting_id [GuidType],
	@data_version INT,
	@setting_data_xml NVARCHAR(MAX),
	@modified_user NVARCHAR(256),
	@force_override [BIT]
AS
BEGIN
	SET NOCOUNT ON;

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRANSACTION
  
		DECLARE @currentVersion INT
		
		SELECT @currentVersion = DataVersion 
		FROM [CustomSettings] WITH (ROWLOCK)
		WHERE [OwnerSID] = @user_sid
		AND [IDCustomSetting] = @setting_id
  
		IF(@currentVersion IS NOT NULL AND @force_override = 0 AND @data_version <> @currentVersion + 1)
		BEGIN
			;THROW 61033, @procedureName, 1;
		END
	
		IF(@currentVersion IS NULL)
		BEGIN
			INSERT INTO [CustomSettings]([LastModified]
				,[IDCustomSetting]
				,[OwnerSID]
				,[DataVersion]
				,[SettingDataXml]
				,[ModifiedDate]
				,[ModifiedUser])
			VALUES( 
				GETUTCDATE()
				, @setting_id
				, @user_sid
				, 1
				, @setting_data_xml
				, GETUTCDATE()
				, @modified_user
			)
		END
		ELSE
		BEGIN
			UPDATE [CustomSettings]
			SET [DataVersion] = [DataVersion] + 1,
				[SettingDataXml] = @setting_data_xml,
				[ModifiedUser] = @modified_user,
				[ModifiedDate] = GETUTCDATE(),
				[LastModified] = GETUTCDATE()
			WHERE [OwnerSID] = @user_sid
			AND [IDCustomSetting] = @setting_id
		END

		COMMIT TRANSACTION
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
